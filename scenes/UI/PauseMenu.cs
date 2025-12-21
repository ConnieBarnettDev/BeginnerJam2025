using Godot;
using Game.Autoload;

namespace Game.UI;

/*
	NOTES:
	-Need to add "Abandon Run" button that deletes save data and returns to main menu
	-Need to add a "New Run" that starts new run with the same class/difficulty
*/

public partial class PauseMenu : Node
{
	[Signal]
	public delegate void ResumePressedEventHandler();

	[Export]
	private PackedScene optionsMenuScene;
	[Export]
	private CanvasLayer pauseMenuContainer;

	private Button resumeButton;
	private Button optionsButton;
	private Button mainMenuButton;
	private Button quitButton;

	public override void _Ready()
	{
		resumeButton = GetNode<Button>("%ResumeButton");
		optionsButton = GetNode<Button>("%OptionsButton");
		mainMenuButton = GetNode<Button>("%MainMenuButton");
		quitButton = GetNode<Button>("%QuitButton");

		AudioHelper.RegisterButtons(new Button[] {
			resumeButton, optionsButton, mainMenuButton, quitButton
		});

		resumeButton.Pressed += OnResumeButtonPressed;
		optionsButton.Pressed += OnOptionsButtonPressed;
		mainMenuButton.Pressed += OnMainMenuButtonPressed;
		quitButton.Pressed += OnQuitButtonPressed;
	}

	private void OnResumeButtonPressed()
	{
		EmitSignal(SignalName.ResumePressed);
	}


	private void OnOptionsButtonPressed()
	{
		pauseMenuContainer.Visible = false;
		var optionsMenu = optionsMenuScene.Instantiate<OptionsMenu>();
		AddChild(optionsMenu);
		optionsMenu.BackPressed += () =>
		{
			OnOptionsBackPressed(optionsMenu);
		};
	}

	private void OnOptionsBackPressed(OptionsMenu optionsMenu)
	{
		optionsMenu.QueueFree();
		pauseMenuContainer.Visible = true;
	}


	private void OnMainMenuButtonPressed()
	{
		LevelManager.ChangeToMainMenu();
	}


	private void OnQuitButtonPressed()
	{
		GetTree().Quit();
	}
}
