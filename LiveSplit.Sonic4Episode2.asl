state ("Sonic") {}

startup
{
    string[,] Settings =
    {
        { null, "newGame",      "Autostart - new game (clean save)",    "true" },
        { null, "newGamePlus",  "Autostart - New Game +",               "false" },
        { null, "newGameMetal", "Autostart - Episode Metal",            "true" },

        { null, "episode2", "Autosplitting - Episode 2", "true" },

        { "episode2", "SC1", "Sylvania Castle - Act 1", "true" },
        { "episode2", "SC2", "Sylvania Castle - Act 2", "true" },
        { "episode2", "SC3", "Sylvania Castle - Act 3", "true" },
        { "episode2", "SCB", "Sylvania Castle - Boss", "true" },

        { "episode2", "WP1", "White Park - Act 1", "true" },
        { "episode2", "WP2", "White Park - Act 2", "true" },
        { "episode2", "WP3", "White Park - Act 3", "true" },
        { "episode2", "WPB", "White Park - Boss", "true" },

        { "episode2", "OD1", "Oil Desert - Act 1", "true" },
        { "episode2", "OD2", "Oil Desert - Act 2", "true" },
        { "episode2", "OD3", "Oil Desert - Act 3", "true" },
        { "episode2", "ODB", "Oil Desert - Boss", "true" },

        { "episode2", "SF1", "Sky Fortress - Act 1", "true" },
        { "episode2", "SF2", "Sky Fortress - Act 2", "true" },
        { "episode2", "SF3", "Sky Fortress - Act 3", "true" },
        { "episode2", "SFB", "Sky Fortress - Boss", "true" },

        { "episode2", "DE1", "Death Egg Mk2 - Act 1", "true" },
        { "episode2", "DEB", "Death Egg Mk2 - Boss", "true" },

        { null, "episodeM", "Autosplitting - Episode Metal", "true" },
        { "episodeM", "EM1", "Act 1", "true" },
        { "episodeM", "EM2", "Act 2", "true" },
        { "episodeM", "EM3", "Act 3", "true" },
        { "episodeM", "EM4", "Act 4", "true" }
    };
    for (int i = 0; i < Settings.GetLength(0); i++) settings.Add(Settings[i, 1], bool.Parse(Settings[i, 3]), Settings[i, 2], Settings[i, 0]);

    // Bitmask check function
    vars.BitCheck = (Func<byte, int, bool>)((plotEvent, b) => { return (plotEvent & (1 << b)) != 0; } );

    // Workaround for Egg Heart health sometimes jumping to zero
    vars.EggHeartHitsLeft = new ExpandoObject();
    vars.EggHeartHitsLeft.Current = 0;
    vars.EggHeartHitsLeft.Old = 0;
    vars.EggHeartHitsLeft.VeryOld = 0;

    vars.Acts = new Dictionary<string, byte>{
        { "SC1", 0 },
        { "SC2", 1 },
        { "SC3", 2 },
        { "SCB", 3 },
        { "WP1", 4 },
        { "WP2", 5 },
        { "WP3", 6 },
        { "WPB", 7 },
        { "OD1", 8 },
        { "OD2", 9 },
        { "OD3", 10 },
        { "ODB", 11 },
        { "SF1", 12 },
        { "SF2", 13 },
        { "SF3", 14 },
        { "SFB", 15 },
        { "DE1", 16 },
        { "DEB", 17 },
        { "EM1", 31 },
        { "EM2", 30 },
        { "EM3", 29 },
        { "EM4", 28 }
    };
}

init
{
    if (game.ReadString(modules.First().BaseAddress + 0x33D774, 31) != "SONIC THE HEDGEHOG 4 Episode II") throw new Exception();

    vars.watchers = new MemoryWatcherList
    {
        new MemoryWatcher<byte>(new DeepPointer(modules.First().BaseAddress + 0x4A3544)) { Name = "LevelID" },
        new MemoryWatcher<byte>(new DeepPointer(modules.First().BaseAddress + 0x4A27F0)) { Name = "GameFlags" },
        new MemoryWatcher<byte>(new DeepPointer(modules.First().BaseAddress + 0x4A27F2)) { Name = "GameFlags2" },
        new MemoryWatcher<byte>(new DeepPointer(modules.First().BaseAddress + 0x4A3428, 0xA10, 0x38C)) { Name = "EggHeartHits" },
        new MemoryWatcher<byte>(new DeepPointer(modules.First().BaseAddress + 0x4A3580)) { Name = "UnlockFlags" },
        new MemoryWatcher<byte>(new DeepPointer(modules.First().BaseAddress + 0x38CE75)) { Name = "PlayerInputs" },
        new MemoryWatcher<int>(new DeepPointer(modules.First().BaseAddress + 0x4A4E00)) { Name = "MainMenuSelectionPointer" },
        new MemoryWatcher<byte>(new DeepPointer(modules.First().BaseAddress + 0x4A4E00, 0x38, 0x54)) { Name = "MainMenuSelection" },
        new MemoryWatcher<byte>(new DeepPointer(modules.First().BaseAddress + 0x4A4DD4, 0x4DF8, 0x4DB8, 0x4C70, 0x5C)) { Name = "ZoneSelect" },
        new MemoryWatcher<byte>(new DeepPointer(modules.First().BaseAddress + 0x4A4DD4, 0x4DF8, 0x4DB8, 0x4C70, 0x5820)) { Name = "ActSelect" },
        new MemoryWatcher<byte>(new DeepPointer(modules.First().BaseAddress + 0x4A4DD4, 0x4DF8, 0x4DB8, 0x4C70, 0x7E80)) { Name = "MenuSpawnConfirmationPlayThisAct" },
        new MemoryWatcher<byte>(new DeepPointer(modules.First().BaseAddress + 0x4A4DD4, 0x4DF8, 0x4DB8, 0x4C70, 0x7ED8)) { Name = "ConfirmationPlayThisActSelection" },
    };

    // Defining current variables in order to avoid triggering exceptions
    current.StageCompleted = false;
    current.GoalPostReached = false;
}

update
{
    // Update watchers
    vars.watchers.UpdateAll(game);

    // Fix for Egg Heart
    vars.EggHeartHitsLeft.VeryOld = vars.EggHeartHitsLeft.Old;
    vars.EggHeartHitsLeft.Old = vars.EggHeartHitsLeft.Current;
    vars.EggHeartHitsLeft.Current = vars.watchers["EggHeartHits"].Current;

    // General purpose variables
    current.StageCompleted = vars.BitCheck(vars.watchers["GameFlags"].Current, 2) && vars.BitCheck(vars.watchers["GameFlags"].Current, 4);
    current.GoalPostReached = vars.BitCheck(vars.watchers["GameFlags2"].Current, 4);
    vars.AButtonPressed = vars.BitCheck(vars.watchers["PlayerInputs"].Current, 4) && !vars.BitCheck(vars.watchers["PlayerInputs"].Old, 4);
    vars.IsConfirmationActStartScreen = vars.watchers["MenuSpawnConfirmationPlayThisAct"].Current == 1 && vars.watchers["ConfirmationPlayThisActSelection"].Current == 0;
}

split
{
    if (vars.watchers["LevelID"].Current == vars.Acts["DEB"])
    {
        if (vars.EggHeartHitsLeft.Current == 0 && vars.EggHeartHitsLeft.Old == 0 && vars.EggHeartHitsLeft.VeryOld == 1) return settings["DEB"];
    }
    else if (vars.watchers["LevelID"].Current == vars.Acts["EM4"])
    {
        if (!old.GoalPostReached && current.GoalPostReached) return settings["EM4"];
    }
    else
    {
        if (current.StageCompleted && !old.StageCompleted)
        {
            foreach (var entry in vars.Acts) { if (vars.watchers["LevelID"].Current == entry.Value) return settings[entry.Key]; }
        }
    }
}

start
{
    bool startingNew = vars.watchers["UnlockFlags"].Current == 0 &&
                       vars.watchers["MainMenuSelectionPointer"].Old != 0 && !vars.watchers["MainMenuSelectionPointer"].Changed &&
                       vars.watchers["MainMenuSelection"].Old == 0 && vars.watchers["MainMenuSelection"].Current == 0 && // Selected first menu option on the main menu
                       vars.AButtonPressed;

    bool startingNewGamePlus = vars.watchers["ZoneSelect"].Current == 0 && vars.watchers["ActSelect"].Current == 0 && vars.IsConfirmationActStartScreen && vars.AButtonPressed;

    bool startingEpisodeMetal = vars.watchers["ZoneSelect"].Current == 6 && vars.watchers["ActSelect"].Current == 0 && vars.IsConfirmationActStartScreen && vars.AButtonPressed;

    return
        (settings["newGame"] && startingNew) ||
        (settings["newGamePlus"] && startingNewGamePlus) ||
        (settings["newGameMetal"] && startingEpisodeMetal);
}