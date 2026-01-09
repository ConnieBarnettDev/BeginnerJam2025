using System;
using Game.Autoload;
using Game.Manager;
using Game.Resources;
using Godot;

namespace Game.UI;

public partial class SlotMachine : Control
{
	[Export]
	private AmmoResource[] ammos;
	private Texture2D ammoTexture;
	private TextureRect textureRect;
	private Label nameLabel;
	private AnimationPlayer animationPlayer;
	private AmmoResource spinResult;
	private Random random = new Random();
	
	public override void _Ready()
	{
		textureRect = GetNode<TextureRect>("%TextureRect");
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		nameLabel = GetNode<Label>("NameLabel");

		VisibilityChanged += OnVisibilityChanged;
	}

    private void OnVisibilityChanged()
    {
        if(Visible)
		{
			spinResult = randomAmmo();
			animationPlayer.Play("spin_wheel");
		}
    }

	private AmmoResource randomAmmo()
	{
		return ammos[random.Next(0, ammos.Length-1)];
	}

	private void SetRandomAmmoTexture()
	{
		textureRect.Texture = randomAmmo().ammoSprite;
	}

	private void SetSpinResultTexture()
	{
		textureRect.Texture = spinResult.ammoSprite;
		nameLabel.Text = spinResult.name + "!";
		GameManager.ChangeAmmoSprite(spinResult);
	}

	private void ResetTimer()
	{
		GameManager.ResetAmmoChangeTimer();
	}

	private void PlaySpinAudio()
	{
		AudioHelper.PlaySpin();
	}

	private void PlayFanfareAudio()
	{
		AudioHelper.PlayFanfare();
	}
}
