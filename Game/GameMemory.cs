using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using LiveSplit.ComponentUtil;

namespace LiveSplit.Sonic4Episode2
{
    class Watchers : MemoryWatcherList
    {
        // Game process
        private readonly Process game;
        public bool IsGameHooked => !(game == null || game.HasExited);

        // Imported game data
        // private MemoryWatcher<int> InternalIGT { get; }
        private MemoryWatcher<byte> LevelID { get; }
        private MemoryWatcher<byte> GameFlags { get; }
        private MemoryWatcher<byte> RunStart1 { get; }
        private MemoryWatcher<int> RunStart2 { get; }
        private MemoryWatcher<byte> RunStart3 { get; }
        private MemoryWatcher<byte> EggHeartsHits { get; }

        // Fake MemoryWatchers: used to convert game data into more easily readable formats
        public FakeMemoryWatcher<bool> StageCompleted => new FakeMemoryWatcher<bool>(bitCheck(this.GameFlags.Old, 2) && bitCheck(this.GameFlags.Old, 4), bitCheck(this.GameFlags.Current, 2) && bitCheck(this.GameFlags.Current, 4));
        // public FakeMemoryWatcher<double> IGT => new FakeMemoryWatcher<double>(Convert.ToDouble(this.InternalIGT.Old) / 60, Convert.ToDouble(this.InternalIGT.Current) / 60);
        public bool RunStarted => this.RunStart1.Current == 3 && this.RunStart1.Changed && this.RunStart2.Changed && this.RunStart2.Current != 0 && this.RunStart2.Old != 0 && this.RunStart3.Old == 0;
        public FakeMemoryWatcher<Acts> Act => new FakeMemoryWatcher<Acts>((Acts)this.LevelID.Old, (Acts)this.LevelID.Current);
        public TripleMemoryWatcher<byte> EggHeartHitsLeft { get; set; }

        // Useful functions and internal variables
        public double AccumulatedIGT = 0d;
        // public bool IGThasReset => this.InternalIGT.Current < this.InternalIGT.Old;


        public Watchers()
        {
            game = Process.GetProcessesByName("Sonic").Where(x => x.MainWindowTitle == "SONIC THE HEDGEHOG 4 Episode II").OrderByDescending(x => x.StartTime).FirstOrDefault(x => !x.HasExited);
            if (game == null) throw new Exception("Couldn't connect to the game!");

            IntPtr baseAddress = game.MainModuleWow64Safe().BaseAddress;
            // this.InternalIGT = new MemoryWatcher<int>(new DeepPointer(baseAddress + 0x4A2804));
            this.LevelID = new MemoryWatcher<byte>(new DeepPointer(baseAddress + 0x4A3544));
            this.GameFlags = new MemoryWatcher<byte>(new DeepPointer(baseAddress + 0x4A27F0));
            this.RunStart1 = new MemoryWatcher<byte>(new DeepPointer(baseAddress + 0x4A027C));
            this.RunStart2 = new MemoryWatcher<int>(new DeepPointer(baseAddress + 0x4A4E00, 0x38)) { FailAction = MemoryWatcher.ReadFailAction.SetZeroOrNull };
            this.RunStart3 = new MemoryWatcher<byte>(new DeepPointer(baseAddress + 0x4A4E00, 0x38, 0x54)) { FailAction = MemoryWatcher.ReadFailAction.SetZeroOrNull };
            this.EggHeartsHits = new MemoryWatcher<byte>(new DeepPointer(baseAddress + 0x4A3428, 0xA10, 0x38C)) { FailAction = MemoryWatcher.ReadFailAction.SetZeroOrNull };

            this.EggHeartHitsLeft = new TripleMemoryWatcher<byte>(this.EggHeartsHits.Old, this.EggHeartsHits.Current);
            this.AddRange(this.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(p => !p.GetIndexParameters().Any()).Select(p => p.GetValue(this, null) as MemoryWatcher).Where(p => p != null));
        }

        public void Update()
        {
            this.UpdateAll(game);
            this.EggHeartHitsLeft.Update(this.EggHeartsHits.Current);
        }

        private bool bitCheck(byte plotEvent, int b)
        {
            return (plotEvent & (1 << b)) != 0;
        }
    }

    class FakeMemoryWatcher<T>
    {
        public T Current { get; set; }
        public T Old { get; set; }
        public bool Changed => !this.Old.Equals(this.Current);
        public FakeMemoryWatcher(T old, T current)
        {
            this.Old = old;
            this.Current = current;
        }
    }

    class TripleMemoryWatcher<T>
    {
        public T VeryOld { get; set; }
        public T Old { get; set; }
        public T Current { get; set; }

        public TripleMemoryWatcher(T old, T current)
        {
            this.VeryOld = this.Old = old;
            this.Current = current;
        }

        public void Update(T current)
        {
            this.VeryOld = this.Old;
            this.Old = this.Current;
            this.Current = current;
        }
    }

    enum Acts : byte
    {
        SylvanyaCastleAct1 = 0,
        SylvanyaCastleAct2 = 1,
        SylvanyaCastleAct3 = 2,
        SylvanyaCastleBoss = 3,
        WhiteParkAct1 = 4,
        WhiteParkAct2 = 5,
        WhiteParkAct3 = 6,
        WhiteParkBoss = 7,
        OilDesertAct1 = 8,
        OilDesertAct2 = 9,
        OilDesertAct3 = 10,
        OilDesertBoss = 11,
        SkyFortressAct1 = 12,
        SkyFortressAct2 = 13,
        SkyFortressAct3 = 14,
        SkyFortressBoss = 15,
        DeathEggAct1 = 16,
        DeathEggBoss = 17,
        SpecialStage1 = 18,
        SpecialStage2 = 19,
        SpecialStage3 = 20,
        SpecialStage4 = 21,
        SpecialStage5 = 22,
        SpecialStage6 = 23,
        SpecialStage7 = 24,
        EpisodeMetalAct1 = 31,
        EpisodeMetalAct2 = 30,
        EpisodeMetalAct3 = 29,
        EpisodeMetalAct4 = 28
    }
}
