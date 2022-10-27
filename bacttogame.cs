using Godot;
using System;

public class bacttogame : Godot.TextureButton
{
    public void _on_bacttogame_pressed(){
        var pause_menu_panel = GetNode("../../Panel") as Panel; 
        pause_menu_panel.Visible = false;
        GetTree().Paused = false;       
    }

}