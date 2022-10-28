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
	public static double ConvertRadiansToDegrees(double radians)
	{
		double degrees = (180 / Math.PI) * radians;
		return (degrees);
	}

	public void GetInput()
	{
		var compass = GetNode("Ship/HUD/Compass") as Sprite;
		var pause_menu_panel = GetNode("Ship/Pause_Menu/Panel") as Panel; 
		var hud_anchor = GetNode("Ship/HUD/HUD_Anchor") as Sprite; 
		var anchor_disabled = ResourceLoader.Load("res://anchor_disabled.png") as Texture;
		var anchor_enabled = ResourceLoader.Load("res://anchor_enabled.png") as Texture;

		rotationDir = 0;
		velocity = new Vector2();
		if(anchor == true){
			speed = 0;
			rotationSpeed = 0;
			hud_anchor.Texture = anchor_disabled;
		}
		else{
			hud_anchor.Texture = anchor_enabled;
			rotationSpeed = 0.5f;
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
		if (Input.IsActionPressed("tab"))
		{
			compass.Visible = true;
		}
		else{
			compass.Visible = false;
		}
	
		velocity = velocity.Normalized() * speed;
	}
	public override void _PhysicsProcess(float delta)
	{
		GetInput();
		var hud_compass_arrow = GetNode("Ship/HUD/Compass/Arrow") as Sprite;
		var hud_minicompass_text = GetNode("Ship/HUD/HUD_Compass/HUD_Compass_Label") as Label;  
		Rotation += rotationDir * rotationSpeed * delta;
		velocity = MoveAndSlide(velocity);
		hud_compass_arrow.RotationDegrees = (float) Math.Round(ConvertRadiansToDegrees(velocity.Angle()), 0);
		string quarter = getQuarter((float) ConvertRadiansToDegrees(velocity.Angle()));
		hud_minicompass_text.Text = quarter;
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
	public string getQuarter (float rotation){
		string quarter = "";
		if (rotation > 0){
			rotation -= 360;
		}
		if (rotation < 0 && rotation > -90){
			if(rotation < -60){
				quarter = "N";
			}
			else if(rotation > -30){
				quarter = "E";
			}
			else{
				quarter = "NE";
			}
		}
		if (rotation < -90 && rotation > -180){
			if(rotation < -150){
				quarter = "W";
			}
			else if(rotation > -120){
				quarter = "N";
			}
			else{
				quarter = "NW";
			}
		}
		if (rotation < -180 && rotation > -270){
			if(rotation < -240){
				quarter = "S";
			}
			else if(rotation > -210){
				quarter = "W";
			}
			else{
				quarter = "SW";
			}
		}
		if (rotation < -270 && rotation > -360){
			if(rotation < -330){
				quarter = "E";
			}
			else if(rotation > -300){
				quarter = "S";
			}
			else{
				quarter = "SE";
			}
		}
		return quarter;
	}
	public void _on_backtomenu_pressed(){
		GD.Print("mukodik");
		GetTree().ChangeScene("res://Menu.tscn");
		GetTree().Paused = false;
	}
    public void _on_fullscreenbutton_pressed(){
        OS.WindowFullscreen = !OS.WindowFullscreen;
    }
    public void _on_optionsbutton_pressed(){
        GetTree().ChangeScene("res://Options.tscn");
        GetTree().Paused = false;
    }
}