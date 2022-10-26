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
        rotationDir = 0;
        velocity = new Vector2(); 
        if(anchor == true){
            speed = 0;
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
        //GD.Print(speed);
        velocity = new Vector2(+speed, 0).Rotated(Rotation);
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
            if(anchortime > 3){
                    anchor = !anchor;
                anchortime = 0;
            }
        }
        GD.Print(anchortime);
        GD.Print(anchor);
    }
}