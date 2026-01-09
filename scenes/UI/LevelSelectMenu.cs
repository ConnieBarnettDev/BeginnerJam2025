using Godot;
using Game.Autoload;
using System.Collections.Generic;

namespace Game.UI;

//INACTIVE

public partial class LevelSelectMenu : CanvasLayer
{
	[Signal]
	public delegate void BackPressedEventHandler();

	//Class
	private Button classLeftButton;
	private Button classRightButton;
	private TextureRect classTextureRect;
	private Label classNameLabel;
	private Label classDescriptionLabel;

	//Difficulty
	private Button difficultyLeftButton;
	private Button difficultyRightButton;
	private TextureRect difficultyTextureRect;
	private Label difficultyNameLabel;
	private Label difficultyDescriptionLabel;

	//Buttons
	private Button backButton;
	private Button continueButton;
	private Button abandonRunButton;
	private Button startRunButton;

	private const string PLACEHOLDER_IMAGE_PATH = "res://icon.svg";

	private List<ClassData> classes = new List<ClassData>();
	private List<DifficultyData> difficulties = new List<DifficultyData>();

	private int classIndex = 0;
	private int difficultyIndex = 0;

	public override void _Ready()
	{
		//Class
		classLeftButton = GetNode<Button>("%ClassLeftButton");
		classRightButton = GetNode<Button>("%ClassRightButton");
		classTextureRect = GetNode<TextureRect>("%ClassTextureRect");
		classNameLabel = GetNode<Label>("%ClassNameLabel");
		classDescriptionLabel = GetNode<Label>("%ClassDescriptionLabel");

		//Difficulty
		difficultyLeftButton = GetNode<Button>("%DifficultyLeftButton");
		difficultyRightButton = GetNode<Button>("%DifficultyRightButton");
		difficultyTextureRect = GetNode<TextureRect>("%DifficultyTextureRect");
		difficultyNameLabel = GetNode<Label>("%DifficultyNameLabel");
		difficultyDescriptionLabel = GetNode<Label>("%DifficultyDescriptionLabel");

		//Buttons
		backButton = GetNode<Button>("%BackButton");
		continueButton = GetNode<Button>("%ContinueButton");
		startRunButton = GetNode<Button>("%StartRunButton");

		AudioHelper.RegisterButtons(new Button[] {
			classLeftButton, classRightButton,
			difficultyLeftButton, difficultyRightButton,
			backButton, continueButton, startRunButton
		});

		//Class
		classLeftButton.Pressed += OnClassLeftButtonPressed;
		classRightButton.Pressed += OnClassRightButtonPressed;

		//Description
		difficultyLeftButton.Pressed += OnDiffcultyLeftButtonPressed;
		difficultyRightButton.Pressed += OnDifficultyRightButtonPressed;

		//Buttons
		backButton.Pressed += OnBackButtonPressed;
		continueButton.Pressed += OnContinueButtonPressed;
		startRunButton.Pressed += OnStartRunButtonPressed;

		//Add Classes
		classes.Add(new ClassData(
			"Class 1",
			"Description of Class 1",
			PLACEHOLDER_IMAGE_PATH,
			Colors.Red
		));
		classes.Add(new ClassData(
			"Class 2",
			"Description of Class 2",
			PLACEHOLDER_IMAGE_PATH,
			Colors.Yellow
		));
		classes.Add(new ClassData(
			"Class 3",
			"Description of Class 3",
			PLACEHOLDER_IMAGE_PATH,
			Colors.Blue
		));

		//Add Difficulties
		difficulties.Add(new DifficultyData(
			"Difficulty 1",
			"Description of Difficulty 1",
			PLACEHOLDER_IMAGE_PATH,
			Colors.Green
		));
		difficulties.Add(new DifficultyData(
			"Difficulty 2",
			"Description of Difficulty 2",
			PLACEHOLDER_IMAGE_PATH,
			Colors.Purple
		));
		difficulties.Add(new DifficultyData(
			"Difficulty 3",
			"Description of Difficulty 3",
			PLACEHOLDER_IMAGE_PATH,
			Colors.Orange
		));

		UpdateClassIndex();
		UpdateDifficultyIndex();

		if (LevelManager.SaveDataExists())
		{
			continueButton.Disabled = false;
		}
		else
		{
			continueButton.Disabled = true;
		}
	}

	//Class

	private void UpdateClassIndex()
	{
		classTextureRect.Texture = GD.Load<Texture2D>(classes[classIndex].imagePath);
		classTextureRect.Modulate = classes[classIndex].color;
		classNameLabel.Text = classes[classIndex].name;
		classDescriptionLabel.Text = classes[classIndex].description;

		if (classIndex <= 0)
		{
			classLeftButton.Disabled = true;
			classRightButton.Disabled = false;
		}
		else if (classIndex >= classes.Count - 1)
		{
			classLeftButton.Disabled = false;
			classRightButton.Disabled = true;
		}
		else
		{
			classLeftButton.Disabled = false;
			classRightButton.Disabled = false;
		}
	}

	private void OnClassLeftButtonPressed()
	{
		if (classIndex <= 0) return;

		classIndex--;
		UpdateClassIndex();
	}


	private void OnClassRightButtonPressed()
	{
		if (classIndex >= classes.Count - 1) return;

		classIndex++;
		UpdateClassIndex();
	}

	//Difficulty

	private void UpdateDifficultyIndex()
	{
		difficultyTextureRect.Texture = GD.Load<Texture2D>(difficulties[difficultyIndex].imagePath);
		difficultyTextureRect.Modulate = difficulties[difficultyIndex].color;
		difficultyNameLabel.Text = difficulties[difficultyIndex].name;
		difficultyDescriptionLabel.Text = difficulties[difficultyIndex].description;

		if (difficultyIndex <= 0)
		{
			difficultyLeftButton.Disabled = true;
			difficultyRightButton.Disabled = false;
		}
		else if (difficultyIndex >= difficulties.Count - 1)
		{
			difficultyLeftButton.Disabled = false;
			difficultyRightButton.Disabled = true;
		}
		else
		{
			difficultyLeftButton.Disabled = false;
			difficultyRightButton.Disabled = false;
		}
	}

	private void OnDiffcultyLeftButtonPressed()
	{
		if (difficultyIndex <= 0) return;

		difficultyIndex--;
		UpdateDifficultyIndex();
	}

	private void OnDifficultyRightButtonPressed()
	{
		if (difficultyIndex >= difficulties.Count - 1) return;

		difficultyIndex++;
		UpdateDifficultyIndex();
	}

	//Buttons
	private void OnBackButtonPressed()
	{
		EmitSignal(SignalName.BackPressed);
	}

	private void OnContinueButtonPressed()
	{
		LevelManager.ContinueRun();
	}

	private void OnStartRunButtonPressed()
	{
		LevelManager.StartRun(
			classes[classIndex],
			difficulties[difficultyIndex]
		);
	}


}
