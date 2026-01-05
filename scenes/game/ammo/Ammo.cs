using Game.Autoload;
using Game.Manager;
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
		GameManager.ExplodeAmmo(this);
		AudioHelper.PlayHit();
		QueueFree();
    }

}
