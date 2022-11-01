using Godot;
using System;
using System.Collections.Generic;
public class readsettings : Node
{
	public string filepath = "res://options.ini";
	public List<string> keybinds = new List<string>();
	public override void _Ready()
	{
		ConfigFile optionfile = new ConfigFile();
		optionfile.Load(filepath);
		foreach(var key in optionfile.GetSectionKeys("keybinds")){
			var key_value = optionfile.GetValue("keybinds", key);
			GD.Print(key, " : ", OS.GetScancodeString(Convert.ToUInt32(key_value)));
			keybinds.Add(OS.GetScancodeString(Convert.ToUInt32(key_value)));
			//GD.Print(request_key("keys"));
		}
	}
	/*public string request_key (string returnkey){
		var actions = InputMap.GetActionList(returnkey);
		foreach(var i in actions)
		{
			InputEventKey key = i as InputEventKey;
			if(key != null)
			{
				return OS.GetScancodeString(key.Scancode);
			}
		}
		return "No key is assigned";
	}*/
}