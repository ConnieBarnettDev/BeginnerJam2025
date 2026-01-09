using Godot;
using System.Collections.Generic;

namespace Game.Autoload;

public partial class AudioHelper : Node
{
	private static AudioHelper Instance;

	private AudioStreamPlayer2D clickAudioStreamPlayer;
	private AudioStreamPlayer2D shootAudioStreamPlayer;
	private AudioStreamPlayer2D hitAudioStreamPlayer;
	private AudioStreamPlayer2D footstepAudioStreamPlayer;
	private AudioStreamPlayer2D musicAudioStreamPlayer;
	private AudioStreamPlayer2D spinAudioStreamPlayer;
	private AudioStreamPlayer2D fanfareAudioStreamPlayer;

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
		shootAudioStreamPlayer = GetNode<AudioStreamPlayer2D>("ShootAudioStreamPlayer");
		hitAudioStreamPlayer = GetNode<AudioStreamPlayer2D>("HitAudioStreamPlayer");
		footstepAudioStreamPlayer = GetNode<AudioStreamPlayer2D>("FootstepAudioStreamPlayer");
		musicAudioStreamPlayer = GetNode<AudioStreamPlayer2D>("MusicAudioStreamPlayer");
		spinAudioStreamPlayer = GetNode<AudioStreamPlayer2D>("SpinAudioStreamPlayer");
		fanfareAudioStreamPlayer = GetNode<AudioStreamPlayer2D>("FanfareAudioStreamPlayer");

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

	public static void PlayShoot()
	{
		Instance.shootAudioStreamPlayer.Play();
	}
	
	public static void PlayHit()
	{
		Instance.hitAudioStreamPlayer.Play();
	}

	public static void PlaySpin()
	{
		Instance.spinAudioStreamPlayer.Play();
	}

	public static void PlayFanfare()
	{
		Instance.fanfareAudioStreamPlayer.Play();
	}

	public static void PlayFootsteps()
	{
		if (!Instance.footstepAudioStreamPlayer.Playing)
		{
			Instance.footstepAudioStreamPlayer.Play();
		}
	}

	public static void StopFootsteps()
	{
		Instance.footstepAudioStreamPlayer.Stop();
	}
}
