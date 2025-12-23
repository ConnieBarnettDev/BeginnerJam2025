using Game.Manager;
using Godot;

namespace Game.Player;

public partial class Player : CharacterBody2D
{
	[Export]
	private PackedScene projectileScene;
	const int PROJECTILE_SPEED = 500;
	private AnimatedSprite2D sprite;
	private Sprite2D shooter;
	private int counter = 0;
	public int playerSpeed = 200;
	public int fireRate = 5;
	public override void _Ready()
	{
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		shooter = GetNode<Sprite2D>("Shooter");
	}

    public override void _PhysicsProcess(double delta)
    {
        Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		Velocity = direction * playerSpeed;
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

		if(!GameManager.isPaused)
		{
			shooter.LookAt(GetGlobalMousePosition());
		}
		
		if (Input.IsActionPressed("left_click"))
		{
			if(counter == 0)
			{
				FireProjectile();
				counter = fireRate;
			}
			else
			{
				counter--;
			}
			
		}
    }

	public void FireProjectile()
	{
		Projectile projectileInstance = projectileScene.Instantiate<Projectile>();
		//projectileInstance.ChangeSprite();
		projectileInstance.Position = Position;
		projectileInstance.LookAt(GetGlobalMousePosition());
		projectileInstance.ApplyImpulse( 
			new Vector2(PROJECTILE_SPEED, 0).Rotated(projectileInstance.Rotation)
		);
		GetTree().Root.AddChild(projectileInstance);

	}

}
