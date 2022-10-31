using Godot;
using System;
public class movement : KinematicBody2D
{
	[Export] public int speed = 50;

	[Export] public float rotationSpeed = 0.1f;

	public Vector2 velocity = new Vector2();    
	public int rotationDir = 0;
	public float sailspeed = 1;
	public bool sail = false;
	public float sailtime = 0;
	public bool anchor = true;
	public float anchortime = 0;
	public float rotationtime = 0;
	public int windrotation = 0;
	public int windmax = 0;
	public int windmin = 0;
	public int windmax_infrontof = 0;
	public int windmin_infrontof = 0;
	public bool interaction_bool_anchor = false;
	public bool interaction_bool_sail = false;
		public override void _Ready()
	{
		// Szél irányát random generálja a játék elindulásánál
		Random random = new Random();
		windrotation = random.Next(0, 360);
		var wind = GetNode("Ship/Wind_CanvasLayer/Wind") as Sprite;
		// A szembeszél és a hátszél kiszámításához kellenek
		wind.RotationDegrees = windrotation;
		if((windrotation + 30) > 359){
			windmax = (windrotation + 30) - 360;
		}
		else{
			windmax = windrotation + 30;
		}
		if((windrotation - 30) < 0){
			windmin= (windrotation - 30) + 360;
		}
		else{
			windmin = windrotation - 30;
		}
		if((windrotation - 150) < 0){
			windmax_infrontof = (windrotation - 150) + 360;
		}
		else{
			windmax_infrontof = windrotation - 150;
		}
		if((windrotation - 210) < 0){
			windmin_infrontof = (windrotation - 210) + 360;
		}
		else{
			windmin_infrontof = windrotation - 210;
		}
	}
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
			hud_anchor.Modulate = Color.Color8(255, 255, 255, 255);
		}
		else{
			speed = 50;
			hud_anchor.Modulate = Color.Color8(255, 255, 255, 125);
			rotationSpeed = 0.1f;
		}
		if(sail == true){
			sail_texutre.Modulate = Color.Color8(255, 255, 255, 255);
		}
		else{
			sail_texutre.Modulate = Color.Color8(255, 255, 255, 125);
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
		// Vitorlás cucc (ha szembeszél lassít, ha mögötte van a szél gyorsít)
		int ship_rotation = (int) Math.Round(ConvertRadiansToDegrees(Rotation), 0);
		sailspeed = 1;
		if (speed > 0 && sail == true) 
		{
			if (ship_rotation < 0){
				ship_rotation += 360;
			}
			if(((windrotation + 30) > 359 || (windrotation - 30) < 0) && (windmax >= ship_rotation && 0 <= ship_rotation || windmin <= ship_rotation && 359 >= ship_rotation)){
				sailspeed = 2;
			}
			if(((windrotation + 30) < 359 && (windrotation - 30) > 0) && (windmax >= ship_rotation && windmin <= ship_rotation)){
				sailspeed = 2;
			}
			if(((windrotation - 150) < 0 && (windrotation - 210) < 0) && (windmax_infrontof >= ship_rotation && windmin_infrontof <= ship_rotation)){
				sailspeed = 0.5f;
			}
			if(((windrotation - 150) > 0 && (windrotation - 210) < 0) && (windmax_infrontof >= ship_rotation && 0 <= ship_rotation || windmin_infrontof <= ship_rotation && 359 >= ship_rotation)){
				sailspeed = 0.5f;
			}
			if(((windrotation - 150) > 0 && (windrotation - 210) > 0) && (windmax_infrontof >= ship_rotation && windmin_infrontof <= ship_rotation)){
				sailspeed = 0.5f;
			}
		}
		// Texture csere speed alapján
		var hud_speed = GetNode("Ship/HUD/HUD_Speed") as Sprite;
		switch(sailspeed){
			case 0.5f:
				hud_speed.Texture =  ResourceLoader.Load("res://assets/speed1.png") as Texture;
				break;
			case 1:
				hud_speed.Texture =  ResourceLoader.Load("res://assets/speed2.png") as Texture;
				break;
			case 2:
				hud_speed.Texture =  ResourceLoader.Load("res://assets/speed3.png") as Texture;
				break;
		}
		if(speed == 0){
			hud_speed.Texture = ResourceLoader.Load("res://assets/speed0.png") as Texture;
		}
		var hud_compass_arrow = GetNode("Ship/HUD/Compass/Arrow") as Sprite;
		var hud_wind_directionlabel = GetNode("Ship/HUD/HUD_Wind_Direction/HUD_Wind_Label") as Label;
		//Jobb és balra kormányzás
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
		Rotation += rotationDir * rotationSpeed * delta;
		velocity = MoveAndSlide(velocity);
		// Texture csere kormányzás iránya alapján
		var hud_wheel = GetNode("Ship/HUD/HUD_WheelLine") as Sprite;
		switch(rotationDir){
			case 0:
				hud_wheel.Texture =  ResourceLoader.Load("res://assets/rotation0.png") as Texture;
				break;
			case 2:
				hud_wheel.Texture =  ResourceLoader.Load("res://assets/rotation1.png") as Texture;
				break;
			case 4:
				hud_wheel.Texture =  ResourceLoader.Load("res://assets/rotation2.png") as Texture;
				break;
			case -2:
				hud_wheel.Texture =  ResourceLoader.Load("res://assets/rotation-1.png") as Texture;
				break;
			case -4:
				hud_wheel.Texture =  ResourceLoader.Load("res://assets/rotation-2.png") as Texture;
				break;
		}
		// Iránytű
		hud_compass_arrow.RotationDegrees = (float) Math.Round(ConvertRadiansToDegrees(Rotation), 0);
		// A szél irányának kiíratása
		string quarter = getQuarter(windrotation);
		hud_wind_directionlabel.Text = quarter;
		// Interakciók (vitorla, horgony)
		string anchor_keyinfo = "space";
		var anchor_keyinfo_label = GetNode("Ship/HUD/HUD_Anchor_Keyinfo") as Label;
		var	interaction_anchor = GetNode("Ship/HUD_Interaction/Anchor_ProgressBar/Anchor_Label") as Label;
		var	interaction_sail = GetNode("Ship/HUD_Interaction/Sail_Progressbar/Sail_Label") as Label;
		var	interaction_sail_progressbar = GetNode("Ship/HUD_Interaction/Sail_Progressbar") as ProgressBar;
		var	interaction_anchor_progressbar = GetNode("Ship/HUD_Interaction/Anchor_ProgressBar") as ProgressBar;
		anchor_keyinfo_label.Text = anchor_keyinfo;
		if(!interaction_bool_sail){
			if (Input.IsActionPressed(anchor_keyinfo))
			{
				interaction_bool_anchor = true;
				anchortime += delta;
				interaction_anchor_progressbar.Visible = true;
				if(anchor){interaction_anchor.Text = "Pulling up anchor";}
				else{interaction_anchor.Text = "Dropping anchor";}
				interaction_anchor_progressbar.Value = anchortime;
				if(anchortime > 3){
					anchor = !anchor;
					anchortime = 0;
				}
			}
			else{
				anchortime = 0;
				interaction_bool_anchor = false;
				interaction_anchor_progressbar.Visible = false;
			}
		}
		string sail_keyinfo = "alt";
		var sail_keyinfo_label = GetNode("Ship/HUD/HUD_Sail_KeyInfo") as Label;
		sail_keyinfo_label.Text = sail_keyinfo;
		interaction_sail.Text = "";
		if(!interaction_bool_anchor){
			if (Input.IsActionPressed(sail_keyinfo))
			{
				interaction_bool_sail = true;
				sailtime += delta;
				interaction_sail_progressbar.Visible = true;
				if(sail){interaction_sail.Text = "Lowering the sail";}
				else{interaction_sail.Text = "Raising the sail";}
				interaction_sail_progressbar.Value = sailtime;
				if(sailtime> 3){
					sail = !sail;
					sailtime = 0;
				}
			}
			else{
				sailtime = 0;
				interaction_bool_sail = false;
				interaction_sail_progressbar.Visible = false;
			}
		}
	}
	// Szél irányának kiszámításához használt metódus
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