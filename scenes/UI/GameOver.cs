using Game.Autoload;
using Game.Manager;
using Godot;

namespace Game.UI;

public partial class GameOver : Control
{
	private Button playAgainButton;
	private Button mainMenuButton;
	private Button quitButton;
	public override void _Ready()
	{
		playAgainButton = GetNode<Button>("%PlayAgainButton");
		mainMenuButton = GetNode<Button>("%MainMenuButton");
		quitButton = GetNode<Button>("%QuitButton");

		AudioHelper.RegisterButtons(new Button[] {
			playAgainButton, mainMenuButton
		});

		playAgainButton.Pressed += OnPlayAgainPressed;
		mainMenuButton.Pressed += OnMainMenuPressed;
		quitButton.Pressed += OnQuitPressed;
	}

    private void OnQuitPressed()
    {
        GetTree().Quit();
    }


    private void OnPlayAgainPressed()
    {
        LevelManager.StartRun();
    }


    private void OnMainMenuPressed()
    {
        LevelManager.ChangeToMainMenu();
    }
}
