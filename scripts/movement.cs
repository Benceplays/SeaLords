using Godot;
using System;

class InputConstans{
    public const string move_right = "move_right";
    public const string move_forward = "move_forward";
    public const string move_backward = "move_backward";
    public const string move_left = "move_left";

}

public class movement : Sprite
{
    public override void _Ready()
    {
        
    }
    public override void _PhysicsProcess(float delta){
        Vector2 Move = new Vector2(0, 0);
        float speed = 50;
        if (Input.IsActionPressed(InputConstans.move_left)){Move.x -= 1; Rotation = 9.44f;}
        if (Input.IsActionPressed(InputConstans.move_backward)){Move.y += 1; Rotation = 7.85f;}
        if (Input.IsActionPressed(InputConstans.move_forward)){Move.y -= 1; Rotation = 4.7f;}
        if (Input.IsActionPressed(InputConstans.move_right)){Move.x += 1; Rotation = 6.3f;}
        /*
        probalkozas
        if (Input.IsActionPressed(InputConstans.move_left)){Rotation -= 0.0001f * speed;}
        if (Input.IsActionPressed(InputConstans.move_backward)){Move.y += 0.2f;}
        if (Input.IsActionPressed(InputConstans.move_forward)){Move.y -= 1;}
        if (Input.IsActionPressed(InputConstans.move_right)){Rotation += 0.0001f * speed;}
        */
                GlobalPosition += Move*speed*delta;
    }

}
