using Godot;
using System;

public class menu : Node
{
    public override void _Ready()
    {
        
    }
    private void _on_play_pressed(){
        GetTree().ChangeScene("res://Game.tscn");
    }
    private void _on_hazi_pressed(){
        GD.Print("asd");
        var changedlabel = "És megváltozott a szoveg.";
        var label = GetNode("../text") as Label;
        label.Text = changedlabel;
    }
    private void _on_exit_pressed(){
        GD.Print("exit");
        GetTree().Quit();
    }

}
