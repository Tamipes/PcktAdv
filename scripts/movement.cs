using Godot;
using System;

public partial class movement : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float SprintMult = 2.5f;
	private bool sprinting = false;
	public const float JumpVelocity = 4.5f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	Node3D neck;
	Node3D cam;

	public override void _Ready()
	{
		neck = GetTree().Root.GetNode("default scene").GetNode("Character").GetNode<Node3D>("Neck");
		cam = neck.GetNode<Node3D>("Cam");
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton)
		{
			Input.MouseMode = Input.MouseModeEnum.Captured;
		}
		else if (@event.IsActionPressed("ui_cancel"))
		{
			Input.MouseMode = Input.MouseModeEnum.Visible;
		}

		if (Input.MouseMode == Input.MouseModeEnum.Captured)
		{
			if (@event is InputEventMouseMotion eventMouseMotion)
			{
				neck.RotateY(-eventMouseMotion.Relative.X * 0.005f);

				cam.RotateX(-eventMouseMotion.Relative.Y * 0.005f);
				cam.Rotation = new Vector3(Math.Clamp(cam.Rotation.X, Mathf.DegToRad(-89.5f), Mathf.DegToRad(89.5f)), cam.Rotation.Y, cam.Rotation.Z);
			}
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;
		sprinting = Input.IsActionPressed("Sprint");

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("Left", "Right", "Up", "Down");
		Vector3 direction = (neck.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = Mathf.MoveToward(Velocity.X, direction.X * Speed * (sprinting ? SprintMult : 1f), (float)delta * 15f);
			velocity.Z = Mathf.MoveToward(Velocity.Z, direction.Z * Speed * (sprinting ? SprintMult : 1f), (float)delta * 15f);
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, (float)delta * Speed * 3f);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, (float)delta * Speed * 3f);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
