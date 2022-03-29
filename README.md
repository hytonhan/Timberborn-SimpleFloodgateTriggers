# Timberborn-SimpleFloodgateTriggers
This plugin for Timberborn allows you to automate your Floodgates a little. Currently we offer automatic setting of floodgate height 
when a Drought starts or ends, based on a basic schedule or based on a Streamgauge depth. Pretty neat.

# Usage
The will add a simple UI fragment on the Floodgate UI. Example images below.

![BasicTab](https://raw.githubusercontent.com/hytonhan/Timberborn-SimpleFloodgateTriggers/main/attachments/BasicTab.PNG?raw=true)
![AdvancedTab](https://raw.githubusercontent.com/hytonhan/Timberborn-SimpleFloodgateTriggers/main/attachments/AdvancedTab.PNG)

Bear in mind that it is possible to create some very janky setups with enabling multiple triggers on the same floodgates. Automation is
nice, but it isn't magic. Be careful, or you'll end up drying/flooding everything!

## Drought
The fist settings on the Basic tab are related to Droughts. The available settings are:
- Enable setting of height when drought ends
- The height to set when drought ends
- Enable setting of height when drought starts
- The height to set when drought starts

## Schedule
The basic tab also contains the settings for automating floodgates based on a schedule.
- Enable Schedule based setting of height
- Optionally disable schedule during droughts
- The times and heights for the schedule
	- Normally the height is set at the selected timestamp. If you change the timestamp, then the height might get instantly changed. This should be fixed 
	once the schedule has been running in peace for a while.

## Linking a StreamGauge
On the Advanced you can link a Floodgate with a StreamGauge. This allows you to control the Floodgate based on the water depth 
recorded by the  StreamGauge. The available settins are:
- Low threshold: Set the floodgate height when water depth is below this value
- Low threshold height: The height to set when the water depth is below the chosen low threshold
- High threshold: Set the floodgate height when water depth is above this value
- High threshold height: The height to set when the water depth is above the chosen high threshold

IMPORTANT! Be aware that if a StreamGauge is linked, then the Floodgate's height will be set always when below low or above high threshold, instead
of triggering once when the thresholds are crossed.

Curretly you can only link a single StreamGauge with a Floodgate. However, there is no limit with how many Floodgates can be connected to a certain StreamGauge.

# Known Limitations
1. The trigger settings are NOT synchronized to neighboring Floodgates. This might be fixed in the future
	- Though if you have a line of Floodgate that are synchronized, you only need to enable the trigger for one of them, as the built in synchronization will take care of
	the neighboring height when one is set.
1. This plugin might cause some lag spikes when a drought ends or starts. They shouldn't be too heavy though
1. If there are lots of active triggers, it might cause general slowness. Be aware, owners of megacolonies!

# Installing
Recommended way to install this mod is through [Thunderstore](https://timberborn.thunderstore.io/). You can install this plugin manually by cloning the repo, building it
and adding the dll to your bepinex plugins folder. This plugin is dependent on the magnificent [TimberAPI](https://github.com/Timberborn-Modding-Central/TimberAPI).

# Changelog

## 1.0.0 - 29.3.2022
- Added option to link Floodgates with StreamGauges
	- This allows to set Floodgate's height when StreamGagues depth is below or above chosen thresholds
- UI Overhaul
	- Added 2 tabs: Basic and Advanced
	- Basic tab contains Drought and Schedule settings
	- Advanced tab contains StreamGague links

## 0.2.0 - 22.3.2022
- Added Schedule based setting of Floodgate height
	- Schedule takes two times and two heights.
	- When the first time is hit, sets the floodgate height to the first chosen height. Do the same at second time. Repeat the next day.
	- Schedule can optionally be disabled during Droughts.

## 0.1.2 - 20.3.2022
- Fixed Drought related Floodgate UI Fragment to look like fragments from the base game.

## 0.1.1 - 19.3.2022
- Minor tweaks to Readme

## 0.1.0 - 19.3.2022
- First implementation that supports setting of height when Droughts end or start.