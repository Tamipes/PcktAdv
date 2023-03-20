using Godot;
using System;

public partial class movement : CharacterBody2D
{
	[Export]
	public float speed { get; set; } = 400f;

	public void GetInput()
	{
		Vector2 inputDirection = Input.GetVector("Left", "Right", "Up", "Down");
		Velocity = inputDirection * speed;
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		GetInput();
		MoveAndSlide();
	}
}
