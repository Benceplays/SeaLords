using Godot;
using System;
public class movement : KinematicBody2D
{
	[Export] public int speed = 50;

	[Export] public float rotationSpeed = 0.1f;

	public Vector2 velocity = new Vector2();    
	public int rotationDir = 0;
	public int sailspeed = 1;
	public bool sail = false;
	public float sailtime = 0;
	public bool anchor = false;
	public float anchortime = 0;
	public float rotationtime = 0;
	public static double ConvertRadiansToDegrees(double radians)
	{
		double degrees = (180 / Math.PI) * radians;
		return (degrees);
	}

	public void GetInput()
	{
		var compass = GetNode("Ship/HUD/Compass") as Sprite;
		var sail_texutre = GetNode("Ship/HUD/HUD_Sail") as Sprite;
		var pause_menu_panel = GetNode("Ship/Pause_Menu/Panel") as Panel; 
		var hud_anchor = GetNode("Ship/HUD/HUD_Anchor") as Sprite; 
		var anchor_disabled = ResourceLoader.Load("res://anchor_disabled.png") as Texture;
		var anchor_enabled = ResourceLoader.Load("res://anchor_enabled.png") as Texture;
		var sail_disabled = ResourceLoader.Load("res://assets/sail_disabled.png") as Texture;
		var sail_enabled = ResourceLoader.Load("res://assets/sail_enabled.png") as Texture;

		velocity = new Vector2();
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
		if(anchor == true){
			speed = 0;
			rotationSpeed = 0;
			hud_anchor.Texture = anchor_disabled;
		}
		else{
			speed = 50;
			hud_anchor.Texture = anchor_enabled;
			rotationSpeed = 0.1f;
		}
		if(sail == true){
			sail_texutre.Texture = sail_enabled;
			sailspeed = 2;
		}
		else{
			sail_texutre.Texture = sail_disabled;
			sailspeed = 1;
		}
		if(anchor == true && sail == true){
			rotationSpeed = 0.1f;
		}
	
		velocity = velocity.Normalized() * speed * sailspeed;
	}
	public override void _PhysicsProcess(float delta)
	{
		GetInput();
		var hud_wheel_rotationspeed = GetNode("Ship/HUD/HUD_Pirate_Wheel/HUD_Wheel_RotationSpeed") as Label;
		var hud_compass_arrow = GetNode("Ship/HUD/Compass/Arrow") as Sprite;
		var hud_minicompass_text = GetNode("Ship/HUD/HUD_Compass/HUD_Compass_Label") as Label;
		if (Input.IsActionPressed("right"))
		{
			rotationtime += delta;
			if(rotationtime > 0.8 && rotationDir < 4){
				rotationDir += 2;
				rotationtime = 0;
			}
		}
		if (Input.IsActionPressed("left"))
		{
			rotationtime += delta;
			if(rotationtime > 0.8 && rotationDir >  -4){
				rotationDir -= 2;
				rotationtime = 0;
			}
		}
		if(!Input.IsActionPressed("left") && !Input.IsActionPressed("right")){
			rotationtime = 0;
		}
		hud_wheel_rotationspeed.Text = Convert.ToString(rotationDir);
		Rotation += rotationDir * rotationSpeed * delta;
		velocity = MoveAndSlide(velocity);
		hud_compass_arrow.RotationDegrees = (float) Math.Round(ConvertRadiansToDegrees(Rotation), 0);
		string quarter = getQuarter((float) ConvertRadiansToDegrees(Rotation));
		hud_minicompass_text.Text = quarter;
		if (Input.IsActionPressed("space"))
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
		if (Input.IsActionPressed("alt"))
		{
			sailtime += delta;
			if(sailtime> 2){
				sail = !sail;
				sailtime = 0;
			}
		}
		else{
			sailtime = 0;
		}
	}
	public string getQuarter (float rotation){
		string quarter = "E";
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