using Godot;
using System;

public class movement : KinematicBody2D
{
    [Export] public int speed = 0;

    [Export] public float rotationSpeed = 0.5f;

    public Vector2 velocity = new Vector2();    
    public int rotationDir = 0;
    public bool anchor = false;
    public float anchortime = 0;

    public void GetInput()
    {
        var pause_menu_panel = GetNode("Ship/Pause_Menu/Panel") as Panel; 
        var hud_anchor = GetNode("Ship/HUD/HUD_Anchor") as Sprite; 
	    var anchor_disabled = ResourceLoader.Load("res://anchor_disabled.png") as Texture;
	    var anchor_enabled = ResourceLoader.Load("res://anchor_enabled.png") as Texture;

        rotationDir = 0;
        velocity = new Vector2(); 
        if(anchor == true){
            speed = 0;
            hud_anchor.Texture = anchor_disabled;
        }
        else{
            hud_anchor.Texture = anchor_enabled;
        }
        if (Input.IsActionPressed("right"))
            rotationDir += 1;

        if (Input.IsActionPressed("left"))
            rotationDir -= 1;

        if (Input.IsActionPressed("down"))
        {
            if(speed > 0){
                speed -= 1;
            }
        }
        if (Input.IsActionPressed("up"))
        {
            if(speed < 75){
                speed += 1;
            }
        }
        velocity = new Vector2(+speed, 0).Rotated(Rotation);
            velocity = new Vector2(speed, 0).Rotated(Rotation);

        if (Input.IsActionPressed("escape"))
        {
            pause_menu_panel.Visible = true;
            GetTree().Paused = true;
        }
    
        velocity = velocity.Normalized() * speed;
    }
    public override void _PhysicsProcess(float delta)
    {
        GetInput();
        Rotation += rotationDir * rotationSpeed * delta;
        velocity = MoveAndSlide(velocity);
        if (Input.IsActionPressed("E"))
        {
            anchortime += delta;
            if(anchortime > 2){
                    anchor = !anchor;
                anchortime = 0;
            }
        }
        else{
            anchortime = 0;
        }
    }
}