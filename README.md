# Timberborn-SimpleFloodgateTriggers
This plugin for Timberborn allows you to automate your Floodgates a little. Currently we offer automatic setting of floodgate height when a Drought starts or ends. Pretty neat.

# Usage
The will add a simple UI fragment on the Floodgate UI. Example image below.
![UIFragment](https://raw.githubusercontent.com/hytonhan/Timberborn-SimpleFloodgateTriggers/main/attachments/UIFragment.PNG)

From the UI you can manage the settings:
- Enable setting of height when drought ends
- The height to set when drought ends
- Enable setting of height when drought starts
- The height to set when drought starts

The settings should be pretty self explanatory.

## Known Limitations
1. The trigger settings are NOT synchronized to neighboring Floodgates. This might be fixed in the future
	- Though if you have a line of Floodgate that are synchronized, you only need to enable the trigger for one of them, as the built in synchronization will take care of
	the neighboring height when one is set.
1. This plugin might cause some lag spikes when a drought ends or starts. They shouldn't be too heavy though

# Installing
Recommended way to install this mod is through [Thunderstore](https://timberborn.thunderstore.io/). You can install this plugin manually by cloning this repo, building it
and adding the dll to your bepinex plugins folder. This plugin is dependent on the magnificent [TimberAPI](https://github.com/Timberborn-Modding-Central/TimberAPI).
