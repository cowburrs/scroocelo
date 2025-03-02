using Godot;
using System;
using System.Diagnostics;

public partial class Node2d : Node2D
{
	[Export] public int Size {get; set;} = 200;

	public override void _Ready()
	{
		Position = GetViewportRect().Size / 2;
		Line2D line = GetNode<Line2D>("Line2D");
		for(int i = 0; i < 7; i++){
			line.AddPoint(new Vector2(0, Size).Rotated(Mathf.Pi / (6 / 2) * i));
		}
		CollisionPolygon2D collision = GetNode<CollisionPolygon2D>("Area2D/CollisionPolygon2D");
		Vector2[] colArray = new Vector2[6];	
		for(int i = 0; i < 6; i++){
			colArray[i] = new Vector2(0, Size).Rotated(Mathf.Pi / (6 / 2) * i);
		}
		collision.Polygon = colArray;

		HexMap hex_map = new HexMap(3, 3);
		hex_map.Block(4);
		hex_map.PrintHexAdjMap();
		hex_map.Update();
		hex_map.PrintHexAdjMap();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void onArea2dInputEvent(Node viewport, InputEvent input, int shapeIdx) // This is it nikolai, the signal connected to mouse clicks
	{
		if (input.GetType() == typeof(InputEventMouseButton)) // Check if the input is compatible
		{
			var mouseEvent = (InputEventMouseButton)input;  // It's a cast apprently but I like to think of it more as getting a value from a variable with lots of information
			if (mouseEvent.Pressed) // Obvious...
			{
				if (mouseEvent.ButtonIndex == MouseButton.Left) // Also very obvious... I used a lot of gpt here btw
				{
					GD.Print("the click shit"); //  put whatever you want in here
				}
			}
		}
	}
}
