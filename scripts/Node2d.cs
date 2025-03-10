using Godot;
using System;
using System.Diagnostics;

public partial class Node2d : Node2D
{
	[Export] public int Size {get; set;} = 200;

	public override void _Ready()
	{
		Position = GetViewportRect().Size / 2;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
