using System;
using System.Windows.Forms;
using System.Xml;
using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;

namespace LiveSplit.Sonic4Episode2
{
    class Sonic4Episode2Component : LogicComponent
    {
        public override string ComponentName => "Sonic 4 Episode 2 - Autosplitter";
        private Settings Settings { get; set; }
        private readonly TimerModel timer;
        private readonly Timer update_timer;
        private readonly SplittingLogic SplittingLogic;

        public Sonic4Episode2Component(LiveSplitState state)
        {
            timer = new TimerModel { CurrentState = state };
            Settings = new Settings();

            SplittingLogic = new SplittingLogic();
            SplittingLogic.OnStartTrigger += OnStartTrigger;
            SplittingLogic.OnSplitTrigger += OnSplitTrigger;

            update_timer = new Timer() { Interval = 15, Enabled = true };
            update_timer.Tick += delegate { update_timer.Enabled = false; SplittingLogic.Update(); update_timer.Start(); };
        }

        void OnStartTrigger(object sender, EventArgs e)
        {
            if (timer.CurrentState.CurrentPhase == TimerPhase.NotRunning && Settings.RunStart) timer.Start();
        }

        void OnSplitTrigger(object sender, Acts type)
        {
            if (timer.CurrentState.CurrentPhase != TimerPhase.Running) return;
            switch (type)
            {
                case Acts.SylvanyaCastleAct1: if (Settings.SC1) timer.Split(); break;
                case Acts.SylvanyaCastleAct2: if (Settings.SC2) timer.Split(); break;
                case Acts.SylvanyaCastleAct3: if (Settings.SC3) timer.Split(); break;
                case Acts.SylvanyaCastleBoss: if (Settings.SCB) timer.Split(); break;
                case Acts.WhiteParkAct1: if (Settings.WP1) timer.Split(); break;
                case Acts.WhiteParkAct2: if (Settings.WP2) timer.Split(); break;
                case Acts.WhiteParkAct3: if (Settings.WP3) timer.Split(); break;
                case Acts.WhiteParkBoss: if (Settings.WPB) timer.Split(); break;
                case Acts.OilDesertAct1: if (Settings.OD1) timer.Split(); break;
                case Acts.OilDesertAct2: if (Settings.OD2) timer.Split(); break;
                case Acts.OilDesertAct3: if (Settings.OD3) timer.Split(); break;
                case Acts.OilDesertBoss: if (Settings.ODB) timer.Split(); break;
                case Acts.SkyFortressAct1: if (Settings.SF1) timer.Split(); break;
                case Acts.SkyFortressAct2: if (Settings.SF2) timer.Split(); break;
                case Acts.SkyFortressAct3: if (Settings.SF3) timer.Split(); break;
                case Acts.SkyFortressBoss: if (Settings.SFB) timer.Split(); break;
                case Acts.DeathEggAct1: if (Settings.DE1) timer.Split(); break;
                case Acts.DeathEggBoss: if (Settings.DEB) timer.Split(); break;
                case Acts.EpisodeMetalAct1: if (Settings.EM1) timer.Split(); break;
                case Acts.EpisodeMetalAct2: if (Settings.EM2) timer.Split(); break;
                case Acts.EpisodeMetalAct3: if (Settings.EM3) timer.Split(); break;
                case Acts.EpisodeMetalAct4: if (Settings.EM4) timer.Split(); break;
            }
        }

        public override void Dispose()
        {
            Settings.Dispose();
            update_timer?.Dispose();
        }

        public override XmlNode GetSettings(XmlDocument document) { return this.Settings.GetSettings(document); }

        public override Control GetSettingsControl(LayoutMode mode) { return this.Settings; }

        public override void SetSettings(XmlNode settings) { this.Settings.SetSettings(settings); }

        public override void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode) { }
    }
}
