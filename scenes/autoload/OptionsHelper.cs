using System;
using Godot;
using Newtonsoft.Json;

namespace Game.Autoload;

public partial class OptionsHelper : Node
{
	private static readonly string OPTIONS_FILE_PATH = "user://options.json";

	public const string MASTER_BUS = "Master";
	public const string MUSIC_BUS = "Music";
	public const string SFX_BUS = "SFX";

	public static void SetBusVolumePercent(string busName, float volumePercent)
	{
		var busIndex = AudioServer.GetBusIndex(busName);
		AudioServer.SetBusVolumeDb(busIndex, Mathf.LinearToDb(volumePercent));
	}

	public static float GetBusVolumePercent(string busName)
	{
		var busIndex = AudioServer.GetBusIndex(busName);
		return AudioServer.GetBusVolumeLinear(busIndex);
	}

	public static void SetWindowMode(string windowMode)
	{
		switch (windowMode)
		{
			case "Windowed":
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
				break;
			case "Windowed Borderless":
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
				break;
			case "Fullscreen":
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.ExclusiveFullscreen);
				break;
		}
	}

	public static void SetResolution(string resolution)
	{
		switch (resolution)
		{
			case "1280x720":
				DisplayServer.WindowSetSize(new Vector2I(1280, 720), 0);
				break;
			case "1920x1080":
				DisplayServer.WindowSetSize(new Vector2I(1920, 1080), 0);
				break;
		}
	}

	public static void WriteOptionsData(OptionsData optionsData)
	{
		var dataString = JsonConvert.SerializeObject(optionsData);
		using var optionsFile = FileAccess.Open(
			OPTIONS_FILE_PATH,
			FileAccess.ModeFlags.Write
		);
		optionsFile.StoreLine(dataString);
	}

	public static OptionsData LoadOptionsData()
	{
		if (!FileAccess.FileExists(OPTIONS_FILE_PATH)) return null;

		using var saveFile = FileAccess.Open(
			OPTIONS_FILE_PATH,
			FileAccess.ModeFlags.Read
		);

		var dataString = saveFile.GetLine();
		try
		{
			var optionsDataObj = JsonConvert.DeserializeObject<OptionsData>(dataString);
			return optionsDataObj;
		}
		catch (Exception)
		{
			GD.PushWarning("Save File Json was corrupted");
			return null;
		}
	}

	public static void ApplyOptionsData()
	{
		OptionsData optionsData = LoadOptionsData();

		if (optionsData == null) return;

		SetBusVolumePercent(MASTER_BUS, (float)optionsData.masterVolume);
		SetBusVolumePercent(MUSIC_BUS, (float)optionsData.musicVolume);
		SetBusVolumePercent(SFX_BUS, (float)optionsData.sfxVolume);

		SetWindowMode(optionsData.windowMode);
		SetResolution(optionsData.resolution);
	}

}
