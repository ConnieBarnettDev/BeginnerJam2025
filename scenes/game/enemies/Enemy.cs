using System;
using Game.Autoload;
using Game.Manager;
using Godot;

public partial class Enemy : RigidBody2D
{
	[Export]
	private int damage = 5;
	[Export]
	private int speed = 25;
	[Export]
	private int maxHealth = 2;
	[Export]
	private int scoreReward = 1;
	private int currentHealth;
	
	private Area2D area2D;
	private ProgressBar healthBar;


	public override void _Ready()
	{
		area2D = GetNode<Area2D>("Area2D");
		healthBar = GetNode<ProgressBar>("HealthBar");
		healthBar.MaxValue = maxHealth;
		
		currentHealth = maxHealth;
		UpdateHealthBar();
		
		area2D.BodyEntered += OnAreaBodyEntered;
		LockRotation = true;
	}

    private void OnAreaBodyEntered(Node body)
    {
        if(body.Name == "Player")
		{
			GameManager.DamagePlayer(damage);
		}
		if(body.Name == "Ammo")
		{
			TakeDamage(GameManager.GetAmmoDamage());
		}
    }

    public override void _PhysicsProcess(double delta)
	{
		Vector2 direction = (GameManager.playerPosition - Position).Normalized();
		LinearVelocity = direction * speed;
	}

	public void TakeDamage(int damage)
	{
		currentHealth = currentHealth - damage;
		UpdateHealthBar();
		if (currentHealth <= 0)
		{
			Die();
		}
	}

	private void UpdateHealthBar()
	{
		healthBar.Value = currentHealth;
	}

	private void Die()
	{
		AudioHelper.PlayEnemyDeathSound();
		GameManager.AddScore(scoreReward);
		QueueFree();
	}
    
}
