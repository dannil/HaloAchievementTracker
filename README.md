# HaloAchievementTracker

Enables you to track misaligned achievements on Steam and Halo Waypoint (Xbox Live) for Halo: The Master Chief Collection. Some achievements (such as "Big Green Style") 
are known to unlock on Halo Waypoint and not Steam, while others simply either unlocks themself on Steam or the wrong achievement is unlocked, 
such as the par scores on early Halo: CE campaign maps. This makes for a complete mess if you for example try to 100% and don't know which achievements 
you actually have unlocked on each platform.

## Usage

Note that your Steam profile needs to be set to public in order to be able to read the achievements.

Save the Halo Waypoint MCC service record page as a HTML file named ```service-record.html``` and put it in folder ```Resources\HaloWaypointData``` relative to the executable, 
then run the command below.

```
HaloAchievementTracker.exe <steamId64>
```

## Example output

![Example output](HaloAchievementTracker.ConsoleApp/Resources/Examples/example_output.png?raw=true "Title")
