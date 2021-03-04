Locator - Valheim

AUTHOR: Purps (https://steamcommunity.com/id/PurpsQQ/)

## Requirements
BepInEx for Valheim

## Features
- Automatically pins various locations and entities during your explration in the world of Valheim
     Mineable rocks (copper, tin, etc.)
     Plants & Fungi (mushrooms, dandelions, thistles, berries, etc.)
     Locations (dungeons / crypts, caves, runestones, boss altars, merchant, etc.)
     Spawners
     Leviathans (Karekens)
- Pin Filtering
     Allows you to filter your Minimap pins using keywords.
- Highly configurable. You can basically configure the plugin to pin any item / location that would be considered important.
     Can be done via console / chat (runtime) or the plugin's configuration file (requires restart)
     See [How To Use] section for more inforamation about configuration.
- Provides various QoL commands to ...
     /locatemerchant => Pins the closest Merchant on the minimap.
     /locatebosses => Pins every boss

### Default Pins
Here is what the Plugin automatically Pins by default
- Locations / Structures
     Crypt
     Grave
     Dragon Egg
     Runestone
     Beehive
- Ores
     Copper
     Tin
     Obsidian
     Silver
     Flametal (Meteorites)
- Berries
     Blueberry Bush
     Raspberry Bush
     Cloudberry Bush
- Fungi
     Mushrooms
- Herbs / Plants
     Thistle
     Carrot
     Dandelion
     Turnip
     Flax
     Barley
- Bosses / NPCs
     Eikthyr
     The Elder
     Bonemass
     Moder
     Yagluth
     Merchant
- Spawners
     All

## Commands
### [Pins]
- /pinfilters [string[]: keywords] => Filters your minimap pins using the provided names.
- /listpins [string[]: name] => Lists all your pins in the Console.
- /clearpins => Clears all your pins. Careful!

### [AutoPin] Can be configured in configuration file.
- /debug => Prints useful information to configure your own pinnable item types.
- /autopin => Toggles entity auto-pinning.
- /pindistance [float: value] => The allowed distance between two entities for auto-pinning.
- /pinraydistance [float: value] => How close to the entity the player must be for it to be auto-pinned. 
- /pindestructibles => Toggles the pinning of big ores veins.
- /pinminerocks => Toggles the pinning of mineable rocks.
- /pinlocations => Toggles the pinning of dungeons, caves, altars, runestones, etc.
- /pinpickables => Toggles the pinning of plants and fungi.
- /pinspawners => Toggles the pinning of spawners.
- /pinleviathans => Toggles the pinning of leviathans.
- /pindistances => The allowed distance between two entities for auto-pinning.

### [Locate]  
- /locatemerchant => Pins the BlackForest Merchant on your Minimap.
- /locatebosses => Pins all boss altars on your Minimap.

### [Other]  
- /listlocations [string: name] => Lists ALL World locations in the Console. Does not work on servers.

## Planned Features
New pin icons (need to do some research for this, new to modding).
Add drop-down to minimap screen (m) to allow pin filtering.
Allow pin creation at current x,y,z using key-binds.

## Manual Installation
1. Extract DLL from zip file into "<GameDirectory>\Bepinex\plugins"
2. Start the game

## Changelog
1. 1.0.1 => Initial Release (Server fix)
2. 1.0.2 => Skipped
3. 1.0.3 => Allow commands to run via chatbox. Adds auto-pinning and config system.
4. 1.0.4 => Add new auto-pin categories. Improved config system. Optimized the code.
5. 1.0.5 => Optimizations. Fix TrollCave pin name. Add debug text (top left) if enabled.
6. 1.0.6 => Handle Multiple item_ids in a single command. Change berry bush categories. Fix shouldTrack property.
7. 1.0.7 => Add a pin filtering system. Add new config options.

## How To Use
### General
Open the Console (F5) or chat (Enter) and type in the desired commands. 
You can view the list by typing in /locator-commands

### Configuration
The configuration file found under **BepInEx\config\purps.valheim.locator.cfg** is pretty self explanatory for all categories other than the [Inclusions] section. You will have to enter properly formatted text for the plugin to load it.

### [Inclusions] parameters:
You will notice different configuration parameters under the [Inclusions] section. These are the item types in Valheim. Right now, the plugin tracks the following categories:
- Destructible
     Any item in Valheim that can be destroyed. Trees, ore veins, etc.
     Oddly enough, most relevant ore veins that you will discover in the world will be under this type.
     A lot of other types can fall under this category. i.e. BerryBushes are both Pickable and Destructible.
- MineRock
     Mineable ore veins. Haven't figure out what's useful in here other than Flametal (Meteorites). 
     Most relevant ore related items are under the Destructible type.
- Location
     Various locations in Valheim, i.e. boss altars, runestones, crypts, caves, etc.
- Pickable
     Items that you can pickup in the enviroment while exploring, i.e. berries, barely, flax, thistle, etc.
- SpawnArea
     Enemeny / item spawners.
- Vegvisir
     These are the boss runestones.
- Leviathan
     The floating "islands" you find in the Ocean. Often referred to as Krakens.

### Format:
{item_id, pin_name, shouldPin}
- [string: item_id] 
     the name identifier that is used by Valheim.
     You can enter partial names i.e. "Crypt" will pin "Crypt1", "Crypt2", "Crypt3" etc.
- [string: pin_name]
     The name that you want to show up on the minimap for that specific pin.
- [bool: shouldTag]
     True or false defining whether this should get tagged or not.
     Useful if you use partial names and you want to exclude a specific item_id.

### Examples
- pickableInclusions = {Pickable_Barley,Barley,true}{Pickable_Thistle,Thistle,true}
     This will **ONLY** pin Barley and Thistle on your minimap for the Pickable type.
- pickableInclusions = {Pickable,Plant,true}{Pickable_Thistle,"",false}
     This will pin **EVERY** pickable items (Bareley, Dandelion, Mushroom, Carrot, etc.) but **NOT** Thistles.

### Default [Inclusions] parameters
Since they would clutter the parameter comments, I've excluded the default values for these from appearing in the config file. Here they are for reference:
- destructibleInclusions
     {silvervein,Silver,true}{rock3_silver,Silver,true}{MineRock_Tin,Tin,true}{rock4_copper,Copper,true}{MineRock_Obsidian,Obsidian,true}
- mineRockInclusions
     {MineRock_Meteorite,Meteorite,true}
- locationInclusions
     {DrakeLorestone,Runestone,true}{TrollCave,BlueBerry,true}{Crypt,Crypt,true}{SunkenCrypt,Crypt,true}{Grave,Grave,true}{DrakeNest,Egg,true}{Runestone,Runestone,true}{Eikthyrnir,Eikthyr,true}{GDKing,The Elder,true}{Bonemass,Bonemass,true}{Dragonqueen,Moder,true}{GoblinKing,Yagluth,true}
- pickableInclusions
     {BlueberryBush,BlueBerry,true}{CloudberryBush,Cloudberry,true}{RaspberryBush,Raspberry,true}{Pickable_Barley,Barley,true}{Pickable_Flax,Flax,true}{Pickable_Thistle,Thistle,true}{Pickable_Mushroom,Mushroom,true}{Pickable_SeedCarrot,Carrot,true}{Pickable_Dandelion,Dandelion,true}{Pickable_SeedTurnip,Turnip,true}
- spawnerInclusions
     {Spawner,Spawner,true}
- vegvisirInclusions
     {Vegvisir,Vegvisir,true}
- leviathanInclusions
     {Leviathan,Leviathan,true}

### How To Find ItemIDs And Type
1. While in-game, enable the Plugin's debug mode by entering the console (F5) command **/debug**.
     You should now see debug text at the top right of your screen when location at most items. Try it our with bushes or mushrooms!
2. Use the information provided to configure the plugin to your liking.