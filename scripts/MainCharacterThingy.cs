using Godot;
using System;
using System.ComponentModel.DataAnnotations;

public partial class MainCharacterThingy : CharacterBody2D
{
	public const float SpeedIncrement = 1000.0f;
	public const float SlowIncrement = 500.0f; 
	public const float JumpVelocity = -400.0f;
	public const float dashVelocity = 500.0f;

	public const float MaxVelocityX = 1000.0f ;
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && (IsOnFloor() || IsOnWall()))
		{
			velocity.Y = JumpVelocity;
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
			velocity.X = Mathf.MoveToward(Velocity.X, 0, SlowIncrement * (float)delta);
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
		GD.Print("dashdowntime = " + dashCooldownTime.TimeLeft);
		GD.Print("dashtime = " + dashTime.TimeLeft);

		Velocity = velocity;
		MoveAndSlide();
	}
	private void onDashTimerTimeout()
	{
		GD.Print("Dash Timer Timeout");
	}
}
