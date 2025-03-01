using Godot;
using System;

public partial class Bat : Node2D
{
	private Node2D _player;
	private float _speed = 50.0f; 
	private Vector2 _targetDirection; 

	public override void _Ready()
	{
		_player = GetNode<Node2D>("/root/RootScene/Player");
		if (_player == null)
		{
			GD.PrintErr("Player node not found!");
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_player == null) return;

		var playerCollisionShape = _player.GetNode<CollisionShape2D>("CollisionShape2D");

		if (playerCollisionShape != null)
		{
			Vector2 targetPosition = playerCollisionShape.GlobalPosition;

			Vector2 direction = targetPosition - GlobalPosition;

			if (direction.Length() > 0)
			{
				_targetDirection = direction.Normalized();
				Position += _targetDirection * _speed * (float)delta;
				UpdateSpriteFlip(direction);
			}
		}
	}
	
	private void UpdateSpriteFlip(Vector2 direction)
	{
		var animatedSprite = GetNode<AnimatedSprite2D>("AnimatedBatSprite2D");

		if (animatedSprite != null)
		{
			animatedSprite.FlipH = direction.X < 0;
		}
	}

	private void _on_Area2D_body_entered(Node body)
	{
		if (body is PlayerController)
		{
            var playerData = GetNode<PlayerData>("/root/PlayerData");

            playerData.DecreaseHealth(1);

            if( playerData.Health <= 0) 
            {
                GD.Print("Game OVER!");
            }
			QueueFree();  // Remove the bat
		}
	}
}
