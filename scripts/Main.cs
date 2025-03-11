using Godot;
using System;

public partial class Main : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Hellow"); // Hellowo
		Vector2 screensize = (Vector2)GetViewportRect().Size; // Get the size of the screen.
		Position = new Vector2(screensize.X/2, screensize.Y/2); // Set the position of the node to the center of the screen.


	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("restart")) // restart the thingy mabobby
		{
			GetTree().ReloadCurrentScene();
		}
	}
}
