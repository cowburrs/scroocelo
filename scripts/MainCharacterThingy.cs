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

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}
		Timer buffer = GetNode<Timer>("bufferTimer");
		if (Input.IsActionJustPressed("jump") && !IsOnFloor() && buffer.TimeLeft == 0)
		{
			buffer.Start();
		}
		if (buffer.TimeLeft > 0 && IsOnFloor())
		{
			Input.ActionRelease("jump");
			Input.ActionPress("jump");
			Input.ActionRelease("jump");
			buffer.Stop();
		}
		GD.Print(buffer.TimeLeft);
		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && (IsOnFloor()))
		{
			velocity.Y = JumpVelocity;
		}
		else if (Input.IsActionJustPressed("jump") && (IsOnWallOnly()) && Input.IsActionPressed("right") && !Input.IsActionPressed("left"))
		{
			velocity.Y = JumpVelocity;
			velocity.X += JumpVelocity;
		}
		else if (Input.IsActionJustPressed("jump") && (IsOnWallOnly()) && Input.IsActionPressed("left") && !Input.IsActionPressed("right"))
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
			if (Math.Abs(velocity.X + ImpulseX) < MaxVelocityX) // this bugs the game wall jump. ??? why though I don't understand. do we has to scrap this shit
			{
				velocity.X += ImpulseX;
			}
			else if (IsOnFloorOnly()) 
			{
				velocity.X = Mathf.MoveToward(Velocity.X, 0, SlowIncrement * (float)delta);
			}
		}
		else if (IsOnFloorOnly())
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
		Line2D staminaBar = GetNode<Line2D>("staminaBar");
		staminaBar.ClearPoints();
		staminaBar.AddPoint(new Vector2(20, 20));
		staminaBar.AddPoint(new Vector2(20, -20+40 * (float)(dashCooldownTime.TimeLeft/dashCooldownTime.WaitTime)));
		// instead of changing the point i fucking delete it and add it back very bad implementation by me lol

		Velocity = velocity;
		MoveAndSlide();
		if (Input.IsActionPressed("shoot")){ 
		}

	}
	public float randomVar;
	private void _on_debug_timer_timeout()
	{
		//GD.Print("Debug Happaned");	
		//GD.Print(GetNode<Timer>("bufferTimer").TimeLeft);	
	}
	private void onBufferTimerTimeout()
	{
		//GD.Print("Buffer Timer Happened");
	}
}
