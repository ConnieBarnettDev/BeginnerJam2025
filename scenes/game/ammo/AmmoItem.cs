using Game.Manager;
using Game.Resources;
using Godot;

namespace Game;

public partial class AmmoItem : Node2D
{
	[Export]
	private AmmoResource ammoResource;
	private Area2D pickupCollision;
	private Sprite2D sprite;
	public override void _Ready()
	{
		pickupCollision = GetNode<Area2D>("PickupCollision");
		sprite = GetNode<Sprite2D>("ItemSprite");

		sprite.Texture = ammoResource.ammoSprite;
		pickupCollision.BodyEntered += OnBodyEntered;
	}

    private void OnBodyEntered(Node2D body)
    {
        if(body.Name == "Player")
		{
			GameManager.ChangeAmmoSprite(ammoResource.ammoSprite);
		}
    }

}
