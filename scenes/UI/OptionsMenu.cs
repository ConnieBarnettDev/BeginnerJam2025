using Godot;
using Game.Autoload;

namespace Game.UI;

public partial class OptionsMenu : CanvasLayer
{
	[Signal]
	public delegate void BackPressedEventHandler();

	//Audio
	private HSlider masterSlider;
	private HSlider musicSlider;
	private HSlider sfxSlider;
	private Button audioResetButton;

	//Display
	private OptionButton windowModeOptionButton;
	private OptionButton resolutionOptionButton;
	private Button displayResetButton;

	//Done
	private Button backButton;

	public override void _Ready()
	{
		//Audio
		masterSlider = GetNode<HSlider>("%MasterSlider");
		musicSlider = GetNode<HSlider>("%MusicSlider");
		sfxSlider = GetNode<HSlider>("%SFXSlider");
		audioResetButton = GetNode<Button>("%AudioResetButton");

		//Display
		windowModeOptionButton = GetNode<OptionButton>("%WindowModeOptionButton");
		resolutionOptionButton = GetNode<OptionButton>("%ResolutionOptionButton");
		displayResetButton = GetNode<Button>("%DisplayResetButton");

		//Done
		backButton = GetNode<Button>("%BackButton");

		AudioHelper.RegisterButtons(new Button[] {
			audioResetButton, resolutionOptionButton, windowModeOptionButton, displayResetButton, backButton
		});

		//Audio
		masterSlider.ValueChanged += OnMasterSliderValueChanged;
		musicSlider.ValueChanged += OnMusicSliderValueChanged;
		sfxSlider.ValueChanged += OnSFXSliderValueChanged;
		audioResetButton.Pressed += OnAudioResetButtonPressed;

		//Display
		windowModeOptionButton.ItemSelected += OnWindowModeItemSelected;
		resolutionOptionButton.ItemSelected += OnResolutionItemSelected;
		displayResetButton.Pressed += OnDisoplayResetButtonPressed;

		//Done
		backButton.Pressed += OnBackButtonPressed;

		UpdateDisplay();
	}

	//Audio
	
	private void OnMasterSliderValueChanged(double value)
	{
		OptionsHelper.SetBusVolumePercent(OptionsHelper.MASTER_BUS, (float)value);
	}

	private void OnMusicSliderValueChanged(double value)
	{
		OptionsHelper.SetBusVolumePercent(OptionsHelper.MUSIC_BUS, (float)value);
	}

	private void OnSFXSliderValueChanged(double value)
	{
		OptionsHelper.SetBusVolumePercent(OptionsHelper.SFX_BUS, (float)value);
	}

	private void OnAudioResetButtonPressed()
	{
		masterSlider.Value = 100;
		musicSlider.Value = 100;
		sfxSlider.Value = 100;
	}

	//Display

	private void OnWindowModeItemSelected(long index)
	{
		OptionsHelper.SetWindowMode(windowModeOptionButton.GetItemText((int)index));
	}

	private void OnResolutionItemSelected(long index)
	{
		OptionsHelper.SetResolution(resolutionOptionButton.GetItemText((int)index));
	}

	private void OnDisoplayResetButtonPressed()
	{
		windowModeOptionButton.Select(0);
		resolutionOptionButton.Select(0);
	}

	//Done

	private void OnBackButtonPressed()
	{
		OptionsHelper.WriteOptionsData(new OptionsData(
			masterSlider.Value,
			musicSlider.Value,
			sfxSlider.Value,
			windowModeOptionButton.GetItemText(windowModeOptionButton.Selected),
			resolutionOptionButton.GetItemText(resolutionOptionButton.Selected)
			));
		EmitSignal(SignalName.BackPressed);
	}

	private void UpdateDisplay()
	{
		masterSlider.Value = OptionsHelper.GetBusVolumePercent(OptionsHelper.MASTER_BUS);
		musicSlider.Value = OptionsHelper.GetBusVolumePercent(OptionsHelper.MUSIC_BUS);
		sfxSlider.Value = OptionsHelper.GetBusVolumePercent(OptionsHelper.SFX_BUS);

		var windowMode = DisplayServer.WindowGetMode();
		switch (windowMode)
		{
			case DisplayServer.WindowMode.Windowed:
				windowModeOptionButton.Select(0);
				break;
			case DisplayServer.WindowMode.Fullscreen:
				windowModeOptionButton.Select(1);
				break;
			case DisplayServer.WindowMode.ExclusiveFullscreen:
				windowModeOptionButton.Select(2);
				break;
		}

		var resolution = DisplayServer.WindowGetSize();
		if (resolution.Equals(new Vector2I(1280, 720)))
		{
			resolutionOptionButton.Select(0);
		}
		else
		{
			resolutionOptionButton.Select(1);
		}
	}

}
