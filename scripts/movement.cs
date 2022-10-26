using Godot;
using System;

public class movement : KinematicBody2D
{
    [Export] public int speed = 0;
    [Export] public float rotationSpeed = 1.5f;

    public Vector2 velocity = new Vector2();    
    public int rotationDir = 0;

    public void GetInput()
    {
        rotationDir = 0;
        velocity = new Vector2(); 

        if (Input.IsActionPressed("right"))
            rotationDir += 1;

        if (Input.IsActionPressed("left"))
            rotationDir -= 1;

        if (Input.IsActionPressed("down"))
        {
            if(speed > 0){
                speed -= 25;
            }
        }
        if (Input.IsActionPressed("up"))
        {
            if(speed < 250){
                speed += 25;
            }
        }
        GD.Print(speed);
        velocity = new Vector2(+speed, 0).Rotated(Rotation);
        velocity = velocity.Normalized() * speed;
    }

    public override void _PhysicsProcess(float delta)
    {
        GetInput();
        Rotation += rotationDir * rotationSpeed * delta;
        velocity = MoveAndSlide(velocity);
    }
}