using System;
using Godot;

namespace Game;

public partial class Projectile : RigidBody2D
{
	private Sprite2D sprite;
	public override void _Ready()
	{
		sprite = GetNode<Sprite2D>("Sprite2D");

		BodyEntered += OnBodyEntered;
	}


    public void ChangeSprite(Texture2D newSprite)
	{
		sprite.Texture = newSprite;
	}

    private void OnBodyEntered(Node body)
    {
		//hit effect
		QueueFree();
    }

}
