using Godot;
using Game.Autoload;

namespace Game.UI;

public partial class MainMenu : Node
{
	[Export]
	private PackedScene optionsMenuScene;
	[Export]
	private PackedScene creditsScene;
	//[Export]
	//private PackedScene levelSelectMenuScene;

	private Button playButton;
	private Button optionsButton;
	private Button creditsButton;
	private Button quitButton;
	private CanvasLayer mainMenuContainer;

	public override void _Ready()
	{
		playButton = GetNode<Button>("%PlayButton");
		optionsButton = GetNode<Button>("%OptionsButton");
		creditsButton = GetNode<Button>("%CreditsButton");
		quitButton = GetNode<Button>("%QuitButton");

		AudioHelper.RegisterButtons(new Button[] {
			playButton, optionsButton, creditsButton,quitButton
		});

		mainMenuContainer = GetNode<CanvasLayer>("%MainMenuContainer");

		playButton.Pressed += OnPlayButtonPressed;
		optionsButton.Pressed += OnOptionsButtonPressed;
		creditsButton.Pressed += OnCreditsButtonPressed;
		quitButton.Pressed += OnQuitButtonPressed;

		OptionsHelper.ApplyOptionsData();
	}

	private void OnPlayButtonPressed()
	{
		LevelManager.StartRun();

		/*
		//LEVEL SELECT MENU
		mainMenuContainer.Visible = false;
		var levelSelect = levelSelectMenuScene.Instantiate<LevelSelectMenu>();
		AddChild(levelSelect);
		levelSelect.BackPressed += () =>
		{
			OnLevelSelectBackPressed(levelSelect);
		};
		*/
	}

	/*
	private void OnLevelSelectBackPressed(LevelSelectMenu levelSelect)
	{
		levelSelect.QueueFree();
		mainMenuContainer.Visible = true;
	}
	*/

	private void OnOptionsButtonPressed()
	{
		mainMenuContainer.Visible = false;
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
		mainMenuContainer.Visible = true;
	}

	private void OnCreditsButtonPressed()
	{
		mainMenuContainer.Visible = false;
		var credits = creditsScene.Instantiate<Credits>();
		AddChild(credits);
		credits.BackPressed += () =>
		{
			OnCreditsBackPressed(credits);
		};
	}

	private void OnCreditsBackPressed(Credits credits)
	{
		credits.QueueFree();
		mainMenuContainer.Visible = true;
	}

	private void OnQuitButtonPressed()
	{
		GetTree().Quit();
	}

	
}
