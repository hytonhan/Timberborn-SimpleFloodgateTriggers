# Timberborn-SimpleFloodgateTriggers
This plugin for Timberborn allows you to automate your Floodgates a little. Currently we offer automatic setting of floodgate height when a Drought starts or ends 
or based on a basic schedule. Pretty neat.

# Usage
The will add a simple UI fragment on the Floodgate UI. Example image below.
![UIFragment](https://raw.githubusercontent.com/hytonhan/Timberborn-SimpleFloodgateTriggers/main/attachments/UIFragment.PNG)

From the UI you can manage the settings:
- Drought
	- Enable setting of height when drought ends
	- The height to set when drought ends
	- Enable setting of height when drought starts
	- The height to set when drought starts
- Schedule
	- Enable Schedule based setting of height
	- Optionally disable schedule during droughts
	- The times and heights for the schedule
		- Normally the height is set at the selected timestamp. If you change the timestamp, then the height might get instantly changed. This should be fixed 
		once the schedule has been running in peace for a while.

![Schedules woo!](https://raw.githubusercontent.com/hytonhan/Timberborn-SimpleFloodgateTriggers/schedule/attachments/ScheduleShowcase.gif)

## Known Limitations
1. The trigger settings are NOT synchronized to neighboring Floodgates. This might be fixed in the future
	- Though if you have a line of Floodgate that are synchronized, you only need to enable the trigger for one of them, as the built in synchronization will take care of
	the neighboring height when one is set.
1. This plugin might cause some lag spikes when a drought ends or starts. They shouldn't be too heavy though
1. If there are lots of active triggers, it might cause general slowness. Be aware, owners of megacolonies!

# Installing
Recommended way to install this mod is through [Thunderstore](https://timberborn.thunderstore.io/). You can install this plugin manually by cloning the repo, building it
and adding the dll to your bepinex plugins folder. This plugin is dependent on the magnificent [TimberAPI](https://github.com/Timberborn-Modding-Central/TimberAPI).

# Changelog

## 0.2.0 - 22.3.2021
- Added Schedule based setting of Floodgate height
	- Schedule takes two times and two heights.
	- When the first time is hit, sets the floodgate height to the first chosen height. Do the same at second time. Repeat the next day.
	- Schedule can optionally be disabled during Droughts.

## 0.1.2 - 20.3.2021
- Fixed Drought related Floodgate UI Fragment to look like fragments from the base game.

## 0.1.1 - 19.3.2021
- Minor tweaks to Readme

## 0.1.0 - 19.3.2021
- First implementation that supports setting of height when Droughts end or start.