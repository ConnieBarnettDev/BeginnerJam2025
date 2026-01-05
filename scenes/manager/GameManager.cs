using Godot;
using Game.UI;
using Game.Autoload;
using Game.Resources;

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

	[Export]
	private PackedScene pauseMenuScene;

	// Probably cutting this out
	private string className;
	private string difficultyName;
	private string currentRunText;


	
	//Ammo
	public static bool isPaused { get; private set; } = false;
	public static PackedScene ammoScene = GD.Load<PackedScene>("res://scenes/game/ammo/ammo.tscn");
	public static Texture2D ammoSprite;
	public static int ammoSpeed { get; private set; } = 500;

	public override void _Ready()
	{
		className = LevelManager.currentClass.name;
		difficultyName = LevelManager.currentDifficulty.name;

		ApplyLevelSelectData();
		ApplySaveData();
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

	public override void _Process(double delta)
	{
		/*
			This is where the magic happens B)
		*/
	}

	public static void ChangeAmmoSprite(Texture2D newSprite)
	{
		ammoSprite = newSprite;
	}

	public static void FireAmmo(CharacterBody2D player)
	{
		if(ammoSprite == null) 
		{ 
			GD.Print("Out of ammo!");
			return; 
		}

		Ammo ammoInstance = ammoScene.Instantiate<Ammo>();
		//Order of operations stop an error
		player.GetTree().Root.AddChild(ammoInstance);
		ammoInstance.sprite.Texture = ammoSprite;
		ammoInstance.Position = player.Position;
		ammoInstance.LookAt(player.GetGlobalMousePosition());
		ammoInstance.ApplyImpulse( 
			new Vector2(ammoSpeed, 0).Rotated(ammoInstance.Rotation)
		);
		AudioHelper.PlayShoot();
	}

	private void ApplyLevelSelectData()
	{
		//level select
	}

	private void ApplySaveData()
	{
		if (LevelManager.saveData != null)
		{
			//save data
		}
	}

	private void OnPauseMenuResumePressed(PauseMenu pauseMenu)
	{
		pauseMenu.QueueFree();
		Engine.TimeScale = 1;
		isPaused = false;
	}

	private void OnTextEditTextChanged()
	{
		LevelManager.WriteSaveData(new SaveData(
			"temp",
			LevelManager.currentClass,
			LevelManager.currentDifficulty
		));
	}


}
