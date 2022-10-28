using Godot;
using System;

public class menu : Node2D
{
	public override void _Ready()
	{
		
	}
	private void _on_Play_pressed(){
		GetTree().ChangeScene("res://Game.tscn");
	}
	private void _on_Maps_pressed(){
		var changedlabel = "És megváltozott a szoveg.";
		var label = GetNode("text") as Label;
		label.Text = changedlabel;
	}
	private void _on_Exit_pressed(){
		GetTree().Quit();   
	}

}
