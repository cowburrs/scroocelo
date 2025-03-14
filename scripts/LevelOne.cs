using Godot;
using System;

public partial class LevelOne : Node2D
{
    public override void _Ready()
    {
		Vector2 screensize = (Vector2)GetViewportRect().Size; // Get the size of the screen.
		Position = new Vector2(screensize.X/2, screensize.Y/2); // Set the position of the node to the center of the screen.
    }

    public override void _Process(double delta)
    {
    }
}
