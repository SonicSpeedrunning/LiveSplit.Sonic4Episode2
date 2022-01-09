using LiveSplit.Sonic4Episode2;
using LiveSplit.Model;
using LiveSplit.UI.Components;
using System;
using System.Reflection;

[assembly: ComponentFactory(typeof(Factory))]

namespace LiveSplit.Sonic4Episode2
{
    public class Factory : IComponentFactory
    {
        public string ComponentName => "Sonic 4 Episode 2 - Autosplitter";
        public string Description => "Automatic splitting";
        public ComponentCategory Category => ComponentCategory.Control;
        public string UpdateName => this.ComponentName;
        public string UpdateURL => "https://raw.githubusercontent.com/SonicSpeedrunning/LiveSplit.Sonic4Episode2/master/";
        public Version Version => Assembly.GetExecutingAssembly().GetName().Version;
        public string XMLURL => this.UpdateURL + "Components/LiveSplit.Sonic4Episode2.xml";
        public IComponent Create(LiveSplitState state) { return new Sonic4Episode2Component(state); }
    }
}
