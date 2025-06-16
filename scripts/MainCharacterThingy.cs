using Godot;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

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
	private Vector2 cameraVelocity = new Vector2(0, 0);

	public override void _PhysicsProcess(double delta)
	{
		MoveAndSlide();
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
		if (Input.IsActionPressed("shoot")){ 
		}
	}
	public override void _Process(double delta)
	{
		Camera2D camera = GetNode<Camera2D>("Node/Camera2D");
		cameraVelocity += (GlobalPosition - camera.Position)/1000;
		cameraVelocity = cameraVelocity.MoveToward(Vector2.Zero, 0.1f);
		camera.Position += cameraVelocity;
		if ((GlobalPosition - camera.Position).Length() > 300)
		{
			GD.Print("Camera is too far from the player");
			DateTime now = DateTime.Now;
			GD.Print(now);
			//camera.Position += (GlobalPosition-camera.Position)/2;
		camera.Position += (GlobalPosition-camera.Position)/2500 * (GlobalPosition - camera.Position).Length();
			//cameraVelocity = Velocity;
		}
		//camera.Position = GlobalPosition;
		GD.Print(IsOnWall());
		// Sprite2D sprite = GetNode<Sprite2D>("Sprite2D/Node/Sprite2D");
		// GD.Print(sprite);
		// sprite.Position += (GlobalPosition - sprite.Position)/(2 * (1/(float)delta));
	}
	public override void _Ready()
	{
		Camera2D camera = GetNode<Camera2D>("Node/Camera2D");
		camera.Position = GetViewportRect().Size / 16;
	}
	
	public float randomVar;
	private void _on_debug_timer_timeout()
	{
		GD.Print("Debug Happaned");	
		//GD.Print(GetNode<Timer>("bufferTimer").TimeLeft);	
		RichTextLabel fpsLabel = GetNode<RichTextLabel>("Node/Camera2D/RichTextLabel");
		int textInt = int.Parse(fpsLabel.Text);
		fpsLabel.Text = Mathf.Round((Velocity.Length()*2+textInt)/3).ToString(); // I need to make an average so that the shit doesn't constant perma change cause the perma change is kinda ass
		if (IsOnWall() == true) {
			GD.Print("Is on wall");
		}
		if (IsOnWallOnly() == true) {
			GD.Print("Is on wall only");
		}
	}
	private void onBufferTimerTimeout()
	{
		//GD.Print("Buffer Timer Happened");
	}
}
