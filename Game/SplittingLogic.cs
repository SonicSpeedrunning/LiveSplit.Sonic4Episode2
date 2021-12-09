using System;
using System.Threading.Tasks;

namespace LiveSplit.Sonic4Episode2
{
    class SplittingLogic
    {
        private Watchers watchers;

        public event EventHandler OnStartTrigger;
        public event EventHandler<Acts> OnSplitTrigger;

        public void Update()
        {
            if (!VerifyOrHookGameProcess()) return;
            watchers.Update();
            Start();
            Split();
        }

        void Start()
        {
            if (watchers.RunStarted) this.OnStartTrigger?.Invoke(this, EventArgs.Empty);
        }

        void Split()
        {
            if (watchers.Act.Current == Acts.DeathEggBoss && watchers.EggHeartHitsLeft.Current == 0 && watchers.EggHeartHitsLeft.Old == 0 && watchers.EggHeartHitsLeft.VeryOld == 1) this.OnSplitTrigger?.Invoke(this, Acts.DeathEggBoss);
            else if (watchers.Act.Current != Acts.DeathEggBoss && watchers.StageCompleted.Current && !watchers.StageCompleted.Old) this.OnSplitTrigger?.Invoke(this, watchers.Act.Current);
        }

        bool VerifyOrHookGameProcess()
        {
            if (watchers != null && watchers.IsGameHooked) return true;
            try { watchers = new Watchers(); } catch { Task.Delay(500); return false; }
            return true;
        }
    }
}
