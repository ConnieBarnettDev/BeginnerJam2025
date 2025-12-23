using Godot;
using Game.UI;
using Game.Autoload;

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

	private Label gameLabel;
	private TextEdit textEdit;

	private string className;
	private string difficultyName;
	private string currentRunText;
	public static bool isPaused { get; private set; } = false;

	public override void _Ready()
	{
		gameLabel = GetNode<Label>("%GameLabel");
		textEdit = GetNode<TextEdit>("%TextEdit");

		textEdit.TextChanged += OnTextEditTextChanged;

		className = LevelManager.currentClass.name;
		difficultyName = LevelManager.currentDifficulty.name;

		ApplyLevelSelectData();
		ApplySaveData();
	}

	private void ApplyLevelSelectData()
	{
		gameLabel.Text = "Class: " + className + "\n" + "Difficulty: " + difficultyName;
	}

	private void ApplySaveData()
	{
		if (LevelManager.saveData != null)
		{
			textEdit.Text = LevelManager.saveData.text;
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

	private void OnPauseMenuResumePressed(PauseMenu pauseMenu)
	{
		pauseMenu.QueueFree();
		Engine.TimeScale = 1;
		isPaused = false;
	}

	private void OnTextEditTextChanged()
	{
		LevelManager.WriteSaveData(new SaveData(
			textEdit.Text,
			LevelManager.currentClass,
			LevelManager.currentDifficulty
		));
	}

	public override void _Process(double delta)
	{
		/*
			This is where the magic happens B)
		*/
	}


}
