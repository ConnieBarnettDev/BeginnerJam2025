using Godot;
using Game.Autoload;

namespace Game.UI;

public partial class Credits : CanvasLayer
{
	[Signal]
	public delegate void BackPressedEventHandler();

	private Button backButton;

	public override void _Ready()
	{
		backButton = GetNode<Button>("%BackButton");

		AudioHelper.RegisterButtons(new Button[] {backButton});

		backButton.Pressed += OnBackButtonPressed;
	}

    private void OnBackButtonPressed()
    {
		EmitSignal(SignalName.BackPressed);
    }
}
