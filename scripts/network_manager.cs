using Godot;
using System;

public partial class network_manager : Node
{
	[Export]
	public Button StartButton {get;set;}= null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if(StartButton != null)
		{
			GD.Print(StartButton == null);
			GD.Print(StartButton.GetInstanceId());

			GD.Print("The button is correctly configured!");
		} 
		else {

			GD.Print("Start button is not added.");

		}
		//GD.Print("What?"); //This works : D
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
