using Game.Manager;
using Game.Autoload;
using Godot;

namespace Game;

public partial class Player : CharacterBody2D
{
	private AnimatedSprite2D sprite;
	private Sprite2D shooter;
	private Area2D hitBox;
	private int counter = 0;
	private int playerSpeed = 200;
	private int fireRate = 5;
	public override void _Ready()
	{
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		shooter = GetNode<Sprite2D>("Shooter");
		hitBox = GetNode<Area2D>("HitBox");
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

		if (direction != Vector2.Zero)
		{
			AudioHelper.PlayFootsteps();
		}
		else
		{
			AudioHelper.StopFootsteps();
		}

		if(!GameManager.isPaused)
		{
			shooter.LookAt(GetGlobalMousePosition());
		}
		
		if (Input.IsActionPressed("left_click"))
		{
			if(counter == 0)
			{
				GameManager.FireAmmo(this);
				counter = fireRate;
			}
			else
			{
				counter--;
			}
		}

		GameManager.playerPosition = Position;
    }


	public void ChangeFireRate(int newRate)
	{
		fireRate = newRate;
	}

	public void ChangeSpeed(int newSpeed)
	{
		playerSpeed = newSpeed;
	}

}
