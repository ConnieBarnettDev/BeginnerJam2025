using Game.Autoload;
using Godot;

namespace Game;

public partial class Ammo : RigidBody2D
{
	public Sprite2D sprite;
	public override void _Ready()
	{
		sprite = GetNode<Sprite2D>("Sprite2D");

		BodyEntered += OnBodyEntered;
	}

    private void OnBodyEntered(Node body)
    {
		//tell GM to play hit scene
		AudioHelper.PlayHit();
		QueueFree();
    }

}
