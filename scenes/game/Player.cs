using Godot;

namespace Game.Player;

public partial class Player : CharacterBody2D
{
	[Export]
	private PackedScene projectileScene;
	const int PLAYER_SPEED = 200;
	const int PROJECTILE_SPEED = 500;
	const int FIRE_RATE = 5;
	private AnimatedSprite2D sprite;
	private Sprite2D wand;
	private int counter = 0;
	public override void _Ready()
	{
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		wand = GetNode<Sprite2D>("Wand");
	}

    public override void _PhysicsProcess(double delta)
    {
        Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		Velocity = direction * PLAYER_SPEED;
		MoveAndSlide();

		switch(direction)
		{
			case (0, 0):
				sprite.Play("idle");
				break;
			case (<0, 0):
				sprite.Play("move_left");
				break;
			case (>0, 0):
				sprite.Play("move_right");
				break;
			case (>0 or <0 or 0, <0):
				sprite.Play("move_up");
				break;
			case (>0 or <0 or 0, >0):
				sprite.Play("move_down");
				break;
		}

		wand.LookAt(GetGlobalMousePosition());

		if (Input.IsActionPressed("left_click"))
		{
			if(counter == 0)
			{
				FireProjectile();
				counter = FIRE_RATE;
			}
			else
			{
				counter--;
			}
			
		}
    }

	public void FireProjectile()
	{
		RigidBody2D projectileInstance = projectileScene.Instantiate<RigidBody2D>();
		projectileInstance.Position = Position;
		projectileInstance.LookAt(GetGlobalMousePosition());
		projectileInstance.ApplyImpulse( 
			new Vector2(PROJECTILE_SPEED, 0).Rotated(projectileInstance.Rotation)
		);
		GetTree().Root.AddChild(projectileInstance);

	}

}
