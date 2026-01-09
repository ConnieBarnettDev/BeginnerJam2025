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
	private Vector2I SCREEN_MIDDLE; 

	[Export]
	private PackedScene pauseMenuScene;
	[Export]
	private PackedScene slotMachineScene;
	
	//UI
	private static ProgressBar healthbar;
	private static TextureRect ammoTextureRect;
	private static CanvasLayer gameUI;
	private static SlotMachine slotMachineInstance;

	//Ammo
	public static bool isPaused { get; private set; } = false;
	public static PackedScene ammoScene = GD.Load<PackedScene>("res://scenes/game/ammo/ammo.tscn");
	public static PackedScene explosionScene = GD.Load<PackedScene>("res://scenes/game/ammo/explosion.tscn");
	public static AmmoResource currentAmmo;
	public static int ammoSpeed { get; private set; } = 500;

	public override void _Ready()
	{
		healthbar = GetNode<ProgressBar>("%HealthBar");
		ammoTextureRect = GetNode<TextureRect>("%AmmoTextureRect");
		gameUI = GetNode<CanvasLayer>("GameUI");

		SCREEN_MIDDLE = new Vector2I(GetViewport().GetWindow().Size.X/2, GetViewport().GetWindow().Size.Y/2);
		GD.Print(GetViewport().GetWindow().Size);
		GD.Print(SCREEN_MIDDLE);


		slotMachineInstance = slotMachineScene.Instantiate<SlotMachine>();
		gameUI.AddChild(slotMachineInstance);
		slotMachineInstance.Position = SCREEN_MIDDLE;
		slotMachineInstance.Visible = false;
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

		if (evt is InputEventKey keyEvent && keyEvent.Pressed)
    {
        if (keyEvent.Keycode == Key.Space)
        {
            slotMachineInstance.Visible = true;
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
			GD.Print("Out of ammo!"); //TODO: in-game message
			return; 
		}

		Ammo ammoInstance = ammoScene.Instantiate<Ammo>();
		player.GetTree().Root.AddChild(ammoInstance);
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

	private void OnPauseMenuResumePressed(PauseMenu pauseMenu)
	{
		pauseMenu.QueueFree();
		Engine.TimeScale = 1;
		isPaused = false;
	}



}
