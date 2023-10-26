# Against The Storm Zoom Mod

More zoom! Zoom out further and see more of the map. Extends the normal zoom mechanism, so just keep scrolling with your mouse wheel.

Tested with game version Early Access 0.62.1R on 10/26/23.

The compiled mod is meant to be loaded into the game with [BepInEx](https://github.com/BepInEx/BepInEx). 
I tested with BepInEx_x64_5.4.22.0 on Windows 11 (https://github.com/BepInEx/BepInEx/releases/download/v5.4.22/BepInEx_x64_5.4.22.0.zip). 
Simply download the loader and unzip it into your game directory. 
For the Steam version of the game, this will likely be `C:\Program Files (x86)\Steam\steamapps\common\Against the Storm\`. 

The `.dll` file created by this mod (`Josiwe.ATS.Zoom.dll` by default) can then be dropped into the `BepInEx\plugins` subdirectory 
inside the game directory. You might have to make this directory yourself, or run the game once to have it made automatically.

You'll also need the json configuration file Josiwe.ATS.Zoom.Config.json in the same directory. This file can be edited
to adjust the zoom limit multiplier. The default value of 3 seems good to me. I wouldn't set it much higher than 5.