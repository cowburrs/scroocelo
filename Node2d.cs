using Godot;
using System;

public partial class Node2d : Node2D
{
	[Export] public int Size {get; set;} = 200;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Line2D line = GetNode<Line2D>("Line2D");
		Position = GetViewportRect().Size / 2;
		for(int i = 0; i < 7; i++){
			line.AddPoint(new Vector2(0, Size).Rotated(Mathf.Pi / (6 / 2) * i));
			GD.Print(new Vector2(0, Size).Rotated(Mathf.Pi / (6 / 2) * i));
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
