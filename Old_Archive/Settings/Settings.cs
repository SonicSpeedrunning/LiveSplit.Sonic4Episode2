using System;
using System.Xml;
using System.Windows.Forms;

namespace LiveSplit.Sonic4Episode2
{
    public partial class Settings : UserControl
    {
        public bool RunStart { get; set; }
        public bool SC1 { get; set; }
        public bool SC2 { get; set; }
        public bool SC3 { get; set; }
        public bool SCB { get; set; }
        public bool WP1 { get; set; }
        public bool WP2 { get; set; }
        public bool WP3 { get; set; }
        public bool WPB { get; set; }
        public bool OD1 { get; set; }
        public bool OD2 { get; set; }
        public bool OD3 { get; set; }
        public bool ODB { get; set; }
        public bool SF1 { get; set; }
        public bool SF2 { get; set; }
        public bool SF3 { get; set; }
        public bool SFB { get; set; }
        public bool DE1 { get; set; }
        public bool DEB { get; set; }
        public bool EM1 { get; set; }
        public bool EM2 { get; set; }
        public bool EM3 { get; set; }
        public bool EM4 { get; set; }

        public Settings()
        {
            InitializeComponent();

            // General settings
            this.chkRunStart.DataBindings.Add("Checked", this, "RunStart", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkSC1.DataBindings.Add("Checked", this, "SC1", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkSC2.DataBindings.Add("Checked", this, "SC2", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkSC3.DataBindings.Add("Checked", this, "SC3", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkSCB.DataBindings.Add("Checked", this, "SCB", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkWP1.DataBindings.Add("Checked", this, "WP1", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkWP2.DataBindings.Add("Checked", this, "WP2", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkWP3.DataBindings.Add("Checked", this, "WP3", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkWPB.DataBindings.Add("Checked", this, "WPB", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkOD1.DataBindings.Add("Checked", this, "OD1", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkOD2.DataBindings.Add("Checked", this, "OD2", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkOD3.DataBindings.Add("Checked", this, "OD3", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkODB.DataBindings.Add("Checked", this, "ODB", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkSF1.DataBindings.Add("Checked", this, "SF1", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkSF2.DataBindings.Add("Checked", this, "SF2", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkSF3.DataBindings.Add("Checked", this, "SF3", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkSFB.DataBindings.Add("Checked", this, "SFB", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkDE1.DataBindings.Add("Checked", this, "DE1", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkDEB.DataBindings.Add("Checked", this, "DEB", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkEM1.DataBindings.Add("Checked", this, "EM1", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkEM2.DataBindings.Add("Checked", this, "EM2", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkEM3.DataBindings.Add("Checked", this, "EM3", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkEM4.DataBindings.Add("Checked", this, "EM4", false, DataSourceUpdateMode.OnPropertyChanged);

            // Default Values
            this.RunStart = true;
            SC1 = SC2 = SC3 = SCB = true;
            WP1 = WP2 = WP3 = WPB = true;
            OD1 = OD2 = OD3 = ODB = true;
            SF1 = SF2 = SF3 = SFB = true;
            DE1 = DEB = true;
            EM1 = EM2 = EM3 = EM4 = true;
        }

        public XmlNode GetSettings(XmlDocument doc)
        {
            XmlElement settingsNode = doc.CreateElement("Settings");
            settingsNode.AppendChild(ToElement(doc, "RunStart", this.RunStart));
            settingsNode.AppendChild(ToElement(doc, "SC1", this.SC1));
            settingsNode.AppendChild(ToElement(doc, "SC2", this.SC2));
            settingsNode.AppendChild(ToElement(doc, "SC3", this.SC3));
            settingsNode.AppendChild(ToElement(doc, "SCB", this.SCB));
            settingsNode.AppendChild(ToElement(doc, "WP1", this.WP1));
            settingsNode.AppendChild(ToElement(doc, "WP2", this.WP2));
            settingsNode.AppendChild(ToElement(doc, "WP3", this.WP3));
            settingsNode.AppendChild(ToElement(doc, "WPB", this.WPB));
            settingsNode.AppendChild(ToElement(doc, "OD1", this.OD1));
            settingsNode.AppendChild(ToElement(doc, "OD2", this.OD2));
            settingsNode.AppendChild(ToElement(doc, "OD3", this.OD3));
            settingsNode.AppendChild(ToElement(doc, "ODB", this.ODB));
            settingsNode.AppendChild(ToElement(doc, "SF1", this.SF1));
            settingsNode.AppendChild(ToElement(doc, "SF2", this.SF2));
            settingsNode.AppendChild(ToElement(doc, "SF3", this.SF3));
            settingsNode.AppendChild(ToElement(doc, "SFB", this.SFB));
            settingsNode.AppendChild(ToElement(doc, "DE1", this.DE1));
            settingsNode.AppendChild(ToElement(doc, "DEB", this.DEB));
            settingsNode.AppendChild(ToElement(doc, "EM1", this.EM1));
            settingsNode.AppendChild(ToElement(doc, "EM2", this.EM2));
            settingsNode.AppendChild(ToElement(doc, "EM3", this.EM3));
            settingsNode.AppendChild(ToElement(doc, "EM4", this.EM4));
            return settingsNode;
        }

        public void SetSettings(XmlNode settings)
        {
            this.RunStart = ParseBool(settings, "RunStart", true);
            this.SC1 = ParseBool(settings, "SC1", true);
            this.SC2 = ParseBool(settings, "SC2", true);
            this.SC3 = ParseBool(settings, "SC3", true);
            this.SCB = ParseBool(settings, "SCB", true);
            this.WP1 = ParseBool(settings, "WP1", true);
            this.WP2 = ParseBool(settings, "WP2", true);
            this.WP3 = ParseBool(settings, "WP3", true);
            this.WPB = ParseBool(settings, "WPB", true);
            this.OD1 = ParseBool(settings, "OD1", true);
            this.OD2 = ParseBool(settings, "OD2", true);
            this.OD3 = ParseBool(settings, "OD3", true);
            this.ODB = ParseBool(settings, "ODB", true);
            this.SF1 = ParseBool(settings, "SF1", true);
            this.SF2 = ParseBool(settings, "SF2", true);
            this.SF3 = ParseBool(settings, "SF3", true);
            this.SFB = ParseBool(settings, "SFB", true);
            this.DE1 = ParseBool(settings, "DE1", true);
            this.DEB = ParseBool(settings, "DEB", true);
            this.EM1 = ParseBool(settings, "EM1", true);
            this.EM2 = ParseBool(settings, "EM2", true);
            this.EM3 = ParseBool(settings, "EM3", true);
            this.EM4 = ParseBool(settings, "EM4", true);
        }

        static bool ParseBool(XmlNode settings, string setting, bool default_ = false)
        {
            bool val;
            return settings[setting] != null ? (Boolean.TryParse(settings[setting].InnerText, out val) ? val : default_) : default_;
        }

        static XmlElement ToElement<T>(XmlDocument document, string name, T value)
        {
            XmlElement str = document.CreateElement(name);
            str.InnerText = value.ToString();
            return str;
        }
    }
}
