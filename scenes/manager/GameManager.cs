using Godot;
using Game.UI;
using Game.Autoload;
using Game.Resources;
using System.Collections;
using System.IO;
using System;

namespace Game.Manager;

public partial class GameManager : Node
{
	private static GameManager Instance;

	public override void _Notification(int what)
	{
		if (what == NotificationSceneInstantiated)
		{
			Instance = this;
		}
	}

	private readonly StringName ACTION_PAUSE = "pause";
	private Vector2I SCREEN_MIDDLE; 

	[Export]
	private PackedScene pauseMenuScene;
	[Export]
	private PackedScene slotMachineScene;
	[Export]
	private PackedScene gameOverScene;
	[Export]
	private Node enemyParent;
	private Node ammoParent;
	[Export]
	private Node2D[] Spawners;
	[Export]
	private PackedScene[] EnemyScenes;
	
	//UI
	private static ProgressBar healthbar;
	private static TextureRect ammoTextureRect;
	private static CanvasLayer gameUI;
	private static Label timerLabel;
	private static Label scoreLabel;
	private static SlotMachine slotMachineInstance;
	private static Timer ammoChangeTimer;
	private static Timer enemySpawnTimer;
	private Random random = new Random();

	//Ammo
	public static bool isPaused { get; private set; } = false;
	public static PackedScene ammoScene = GD.Load<PackedScene>("res://scenes/game/ammo/ammo.tscn");
	public static PackedScene explosionScene = GD.Load<PackedScene>("res://scenes/game/ammo/explosion.tscn");
	public static AmmoResource currentAmmo;
	public static int ammoSpeed { get; private set; } = 500;

    //Player
	public static Vector2 playerPosition;
	public static int playerCurrentHealth;
	public static int playerMaxHealth = 100;
	public static int playerScore = 0; 

	public override void _Ready()
	{
		Engine.TimeScale = 1;
		healthbar = GetNode<ProgressBar>("%HealthBar");
		ammoTextureRect = GetNode<TextureRect>("%AmmoTextureRect");
		ammoChangeTimer = GetNode<Timer>("AmmoChangeTimer");
		enemySpawnTimer = GetNode<Timer>("EnemySpawnTimer");
		timerLabel = GetNode<Label>("%TimerLabel");
		gameUI = GetNode<CanvasLayer>("GameUI");
		scoreLabel = GetNode<Label>("%ScoreLabel");

		SCREEN_MIDDLE = new Vector2I(GetViewport().GetWindow().Size.X/2, GetViewport().GetWindow().Size.Y/2);

		slotMachineInstance = slotMachineScene.Instantiate<SlotMachine>();
		gameUI.AddChild(slotMachineInstance);
		slotMachineInstance.Position = SCREEN_MIDDLE;
		slotMachineInstance.Visible = false;
		slotMachineInstance.Visible = true;

		playerCurrentHealth = playerMaxHealth;
		healthbar.Value = playerCurrentHealth;

		ammoChangeTimer.Timeout += OnAmmoChangeTimerTimeout;
		enemySpawnTimer.Timeout += OnEnemySpawnTimerTimeout;
	}

    private void OnEnemySpawnTimerTimeout()
    {
        SpawnerEnemies();
    }

	private void SpawnerEnemies()
	{
		foreach(Node2D spawner in Spawners)
		{
			//random.Next(0, EnemyScenes.Length-1)
			Enemy enemyInstance = EnemyScenes[0].Instantiate<Enemy>();
			enemyParent.AddChild(enemyInstance);
			enemyInstance.Position = spawner.Position;
		}
	}


    public override void _Process(double delta)
    {
		if (!ammoChangeTimer.IsStopped())
		{
			timerLabel.Text = ((int)ammoChangeTimer.TimeLeft + 1).ToString();
		}
    }

    public override void _UnhandledInput(InputEvent evt)
	{
		
		if (evt.IsActionPressed(ACTION_PAUSE))
		{
			Node currentPauseMenu = GetTree().GetFirstNodeInGroup("Pause");
			Engine.TimeScale = 0;
			isPaused = true;
			
			if (currentPauseMenu != null)
			{
				OnPauseMenuResumePressed(GetNode<PauseMenu>("PauseMenu"));
			}
			else
			{
				var pauseMenu = pauseMenuScene.Instantiate<PauseMenu>();
				AddChild(pauseMenu);
				pauseMenu.ResumePressed += () =>
				{
					OnPauseMenuResumePressed(pauseMenu);
				};
			}
		}
	}

	public static void ChangeAmmoSprite(AmmoResource newAmmo)
	{
		currentAmmo = newAmmo;
		ammoTextureRect.Texture = currentAmmo.ammoSprite;
	}

	public static void FireAmmo(CharacterBody2D player)
	{
		if(currentAmmo == null) 
		{  
			return; 
		}

		Ammo ammoInstance = ammoScene.Instantiate<Ammo>();
		Instance.ammoParent.AddChild(ammoInstance);
		ammoInstance.sprite.Texture = currentAmmo.ammoSprite;
		ammoInstance.Position = player.Position;
		ammoInstance.LookAt(player.GetGlobalMousePosition());
		ammoInstance.ApplyImpulse( 
			new Vector2(ammoSpeed, 0).Rotated(ammoInstance.Rotation)
		);
		AudioHelper.PlayShoot();
	}

	public static void ExplodeAmmo(Ammo ammo)
	{
		Node2D explosionInstance = explosionScene.Instantiate<Node2D>();
		ammo.GetTree().Root.AddChild(explosionInstance);
		explosionInstance.Position = ammo.Position;
	}

	public static void ResetAmmoChangeTimer()
	{
		ammoChangeTimer.Start();
	}

	public static void DamagePlayer(int damage)
	{
		playerCurrentHealth -= damage;
		healthbar.Value = playerCurrentHealth;
		AudioHelper.PlayOof();
		if (playerCurrentHealth <= 0)
		{
			Instance.TriggerGameOver();
		}
	}

	public static void AddScore(int increase)
	{
		playerScore += increase;
		scoreLabel.Text = "Score: " + playerScore;
	}

	public static int GetAmmoDamage()
	{
		return currentAmmo.damage;
	}

	private void TriggerGameOver()
	{
		Engine.TimeScale = 0;
		Node gameOverSceneInstance = gameOverScene.Instantiate<Node>();
		gameUI.AddChild(gameOverSceneInstance);
	}

	private void OnAmmoChangeTimerTimeout()
    {
        slotMachineInstance.Visible = true;
		ammoChangeTimer.Stop();
		timerLabel.Text = "";
    }

	private void OnPauseMenuResumePressed(PauseMenu pauseMenu)
	{
		pauseMenu.QueueFree();
		Engine.TimeScale = 1;
		isPaused = false;
	}



}
