using System;
using Godot;
using Newtonsoft.Json;

namespace Game.Autoload;

public partial class LevelManager : Node
{
	private static readonly string SAVE_FILE_PATH = "user://save.json";

	[Export(PropertyHint.File, "*.tscn")]
	private string mainMenuScenePath;
	[Export(PropertyHint.File, "*.tscn")]
	private string gameScenePath;

	private static LevelManager Instance;

	public override void _Notification(int what)
	{
		if (what == NotificationSceneInstantiated)
		{
			Instance = this;
		}
	}

	public static ClassData currentClass { get; private set; }
	public static DifficultyData currentDifficulty { get; private set; }
	public static SaveData saveData { get; private set; }

	public static void ChangeToMainMenu()
	{
		Instance.GetTree().ChangeSceneToFile(Instance.mainMenuScenePath);
	}

	public static void ContinueRun()
	{
		saveData = LoadSaveData();
		currentClass = saveData.classData;
		currentDifficulty = saveData.difficultyData;
		Instance.GetTree().ChangeSceneToFile(Instance.gameScenePath);
	}

	public static void StartRun(ClassData classData, DifficultyData difficultyData)
	{
		saveData = null;
		if (FileAccess.FileExists(SAVE_FILE_PATH))
		{
			DirAccess.RemoveAbsolute(SAVE_FILE_PATH);
		}
		currentClass = classData;
		currentDifficulty = difficultyData;
		Instance.GetTree().ChangeSceneToFile(Instance.gameScenePath);
	}

	public static bool SaveDataExists()
	{
		return FileAccess.FileExists(SAVE_FILE_PATH);
	}

	public static void WriteSaveData(SaveData saveData)
	{
		var dataString = JsonConvert.SerializeObject(saveData);
		using var saveFile = FileAccess.Open(
			SAVE_FILE_PATH,
			FileAccess.ModeFlags.Write
		);
		saveFile.StoreLine(dataString);
	}

	public static SaveData LoadSaveData()
	{
		if (!FileAccess.FileExists(SAVE_FILE_PATH)) return null;

		using var saveFile = FileAccess.Open(
			SAVE_FILE_PATH,
			FileAccess.ModeFlags.Read
		);

		var dataString = saveFile.GetLine();
		try
		{
			var saveDataObj = JsonConvert.DeserializeObject<SaveData>(dataString);
			return saveDataObj;
		}
		catch (Exception)
		{
			GD.PushWarning("Save File Json was corrupted");
			return null;
		}
	}

}
