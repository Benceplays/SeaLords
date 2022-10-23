using Godot;
using System;

public class movement : Sprite
{
    public override void _Ready()
    {
        
    }
    public override void _PhysicsProcess(float delta){
        Vector2 Move = new Vector2(0, 0);
        float speed = 50;
        if (Input.IsActionPressed(InputConstans.move_left)){Move.x -= 1;}
        if (Input.IsActionPressed(InputConstans.move_backward)){Move.y += 1;}
        if (Input.IsActionPressed(InputConstans.move_forward)){Move.y -= 1;}
        if (Input.IsActionPressed(InputConstans.move_right)){Move.x += 1;}
        GlobalPosition += Move*speed*delta;
    }

}
