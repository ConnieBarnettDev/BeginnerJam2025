using Godot;
using Game.UI;
using Game.Autoload;
using System;

namespace Game.Manager;

public partial class GameManager : Node
{
	private readonly StringName ACTION_PAUSE = "pause";

	[Export]
	private PackedScene pauseMenuScene;

	private Label gameLabel;
	private TextEdit textEdit;

	private string className;
	private string difficultyName;
	private string currentRunText;

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
		//Gameplay
	}


}
