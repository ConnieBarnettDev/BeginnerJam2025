using Godot;
using System.Collections.Generic;

namespace Game.Autoload;

public partial class AudioHelper : Node
{
	private static AudioHelper Instance;

	private AudioStreamPlayer2D clickAudioStreamPlayer;
	private AudioStreamPlayer2D musicAudioStreamPlayer;

	public override void _Notification(int what)
	{
		if (what == NotificationSceneInstantiated)
		{
			Instance = this;
		}
	}

	public override void _Ready()
	{
		clickAudioStreamPlayer = GetNode<AudioStreamPlayer2D>("ClickAudioStreamPlayer");
		musicAudioStreamPlayer = GetNode<AudioStreamPlayer2D>("MusicAudioStreamPlayer");

		musicAudioStreamPlayer.Finished += OnMusicFinished;
	}

	public static void RegisterButtons(IEnumerable<Button> buttons)
	{
		foreach (var button in buttons)
		{
			button.Pressed += Instance.OnButtonPressed;
		}
	}

	private void OnButtonPressed()
	{
		clickAudioStreamPlayer.Play();
	}

	private void OnMusicFinished()
	{
		GetTree().CreateTimer(5).Timeout += OnMusicDelayTimerTimout;
	}

	private void OnMusicDelayTimerTimout()
	{
		musicAudioStreamPlayer.Play();
	}
}
