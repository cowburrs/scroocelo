using Godot;
using System;
using System.ComponentModel.DataAnnotations;

public partial class MainCharacterThingy : CharacterBody2D
{
	[Export]
	public float SpeedIncrement;
	[Export]
	public float SlowIncrement; 
	[Export]
	public float JumpVelocity;
	[Export]
	private float dashVelocity;
	[Export]
	public float MaxVelocityX;
	[Export]
	public float SlowAlwaysIncrement;

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && (IsOnFloor()))
		{
			velocity.Y = JumpVelocity;
		}
		if (Input.IsActionJustPressed("jump") && (IsOnWallOnly()) && Input.IsActionPressed("right") && !Input.IsActionPressed("left"))
		{
			velocity.Y = JumpVelocity;
			velocity.X += JumpVelocity;
		}
		if (Input.IsActionJustPressed("jump") && (IsOnWallOnly()) && Input.IsActionPressed("left") && !Input.IsActionPressed("right"))
		{
			velocity.Y = JumpVelocity;
			velocity.X -= JumpVelocity;
		}
		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("left", "right", "up", "down");
		if (direction != Vector2.Zero)
		{
			float ImpulseX = direction.X * SpeedIncrement * (float)delta; 
			if (velocity.X + ImpulseX < MaxVelocityX)
			{
				velocity.X += ImpulseX;
			}
		}
		else
		{
			if (IsOnFloorOnly())
			{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, SlowIncrement * (float)delta);
			}
		}
		Timer dashTime = GetNode<Timer>("dashTimer");
		Timer dashCooldownTime = GetNode<Timer>("dashCooldownTimer");
		if (Input.IsActionJustPressed("right") && dashTime.TimeLeft > 0){
			velocity.X += dashVelocity;
			dashCooldownTime.Start();
			dashTime.Stop();
		}
		if (Input.IsActionJustPressed("right") && dashTime.TimeLeft == 0 && dashCooldownTime.TimeLeft == 0)
		{
			dashTime.Start();
		}
		if (Input.IsActionJustPressed("left") && dashTime.TimeLeft > 0){
			velocity.X -= dashVelocity;
			dashCooldownTime.Start();
			dashTime.Stop();
		}
		if (Input.IsActionJustPressed("left") && dashTime.TimeLeft == 0 && dashCooldownTime.TimeLeft == 0)
		{
			dashTime.Start();
		}
		Line2D staminaBar = GetNode<Line2D>("staminaBar");
		staminaBar.ClearPoints();
		staminaBar.AddPoint(new Vector2(20, 20));
		staminaBar.AddPoint(new Vector2(20, -20+40 * (float)(dashCooldownTime.TimeLeft/dashCooldownTime.WaitTime)));
		// instead of changing the point i fucking delete it and add it back very bad implementation by me lol

		Velocity = velocity;
		if (Input.IsActionPressed("sprint")){
			MoveAndSlide();
		}
		MoveAndSlide();
		if (Input.IsActionPressed("shoot")){
			
		}
	}
}
