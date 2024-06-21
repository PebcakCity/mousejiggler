using System.Reflection;

namespace PebcakCity.MouseJiggler
{
    public partial class MainForm : Form
    {
        private Config? Config;

        public MainForm() : this(null) { }

        public MainForm(Config? config)
        {
            InitializeComponent();
            this.Config = config;
        }

        #region MainForm events

        private void MainForm_Load(object sender, EventArgs e)
        {
            if ( Config is not null )
            {
                this.cbJiggleOnStart.Checked = this.cbJiggling.Checked = Config.JiggleOnStartup;
                this.cbMinimize.Checked = Config.MinimizeOnStartup;
                this.upDownInterval.Value = Config.JiggleInterval;
                this.cbZen.Checked = Config.ZenJiggle;
            }
            CreateTrayContextMenu();
        }

        private bool FirstShown = true;

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if ( this.FirstShown && this.Config is not null && Config.MinimizeOnStartup )
                this.MinimizeToTray();
            this.FirstShown = false;
        }

        private const int WM_SYSCOMMAND = 0x0112;
        private const int SC_MINIMIZE = 0xF020;

        protected override void WndProc( ref Message m )
        {
            if (m.Msg == WM_SYSCOMMAND)
            {
                if (m.WParam.ToInt32() ==  SC_MINIMIZE)
                {
                    this.MinimizeToTray();
                }
            }
            base.WndProc(ref m);
        }

        private void btnSaveSettings_Clicked(object sender, EventArgs e)
        {
            if (Config is not null && Config.Write())
            {
                MessageBox.Show("Settings saved.", "Mouse Jiggler",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnAbout_Clicked(object sender, EventArgs e)
        {
            string title = "Mouse Jiggler";
            string version = Assembly.GetExecutingAssembly().GetName().Version!.ToString();
            object [] attr = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
            string desc = (attr.Length > 0) ? ((AssemblyDescriptionAttribute) attr[0]).Description : "";
            MessageBox.Show($"{title} {version} - {desc}", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        #region jiggleTimer

        protected bool Zig = true;

        private void jiggleTimer_Tick(object sender, EventArgs e)
        {
            if ( this.cbZen.Checked )
                Helpers.Jiggle(0);
            else if ( this.Zig )
                Helpers.Jiggle(4);
            else
                Helpers.Jiggle(-4);

            this.Zig = !this.Zig;
        }

        private void cbJiggling_CheckedChanged(object sender, EventArgs e)
        {
            this.jiggleTimer.Enabled = this.cbJiggling.Checked;
        }

        #endregion

        #region Settings & config sync

        private void cbSettings_CheckedChanged(object sender, EventArgs e)
        {
            this.panelSettings.Visible = this.cbSettings.Checked;
        }

        private void cbJiggleOnStart_CheckedChanged(object sender, EventArgs e)
        {
            if ( Config is not null )
                Config.JiggleOnStartup = this.cbJiggleOnStart.Checked;
        }

        private void cbMinimize_CheckedChanged(object sender, EventArgs e)
        {
            if ( Config is not null )
                Config.MinimizeOnStartup = this.cbMinimize.Checked;
        }

        private void cbZen_CheckedChanged(object sender, EventArgs e)
        {
            if (Config is not null)
                Config.ZenJiggle = this.cbZen.Checked;
        }

        private void upDownInterval_ValueChanged(object sender, EventArgs e)
        {
            // Handle up button increasing time by one minute (60) when at the minimum (1), reset to 60
            if ( Int32.TryParse(((UpDownBase)sender).Text, out int oldValue))
            {
                if (Int32.TryParse(((NumericUpDown)sender).Value.ToString(), out int newValue))
                {
                    if ( oldValue == 1 && newValue == 61 )
                        ( (NumericUpDown)sender ).Value = 60;
                }
            }
            if ( Config is not null )
                Config.JiggleInterval = (int)( (NumericUpDown)sender ).Value;

            this.jiggleTimer.Interval = (int)( (NumericUpDown)sender ).Value * 1000;
        }

        #endregion

        #region NotifyIcon

        // Checkbox in NotifyIcon tray menu that allows quick enable/disable
        private ToolStripMenuItem? niTrayCbJiggleEnabled;

        /// <summary>
        /// Create a context menu and attach it to the NotifyIcon niTray:
        /// Show - Same as doubleclicking on the icon
        /// Jiggling - Quick enable/disable checkbox
        /// Exit - Exit application
        /// </summary>
        private void CreateTrayContextMenu()
        {
            this.niTray.ContextMenuStrip = new();
            this.niTray.ContextMenuStrip.ShowCheckMargin = true;
            this.niTray.ContextMenuStrip.Items.Add("Show", null, this.niTray_DoubleClick);
            this.niTrayCbJiggleEnabled = new ToolStripMenuItem()
            {
                Text = "Jiggling",
                Checked = this.jiggleTimer.Enabled
            };
            this.niTrayCbJiggleEnabled.Click += this.niTray_ToggleJiggleEnabled;
            this.niTray.ContextMenuStrip.Items.Add(this.niTrayCbJiggleEnabled);
            this.niTray.ContextMenuStrip.Items.Add("Exit", null, this.niTray_ExitClick);
        }

        private void niTray_DoubleClick(object? sender, EventArgs e)
        {
            this.RestoreFromTray();
        }

        private void niTray_ExitClick(object? sender, EventArgs e)
        {
            Application.Exit();
        }

        private void niTray_ToggleJiggleEnabled(object? sender, EventArgs e)
        {
            this.cbJiggling.Checked = !this.cbJiggling.Checked;
            this.niTrayCbJiggleEnabled!.Checked = this.jiggleTimer.Enabled;
            this.UpdateNotificationAreaText();
        }


        public void RestoreFromTray()
        {
            this.Visible        = true;
            this.ShowInTaskbar  = true;
            this.niTray.Visible = false;
        }

        private void MinimizeToTray()
        {
            this.Visible        = false;
            this.ShowInTaskbar  = false;
            this.niTray.Visible = true;
            this.UpdateNotificationAreaText();
            if (this.niTrayCbJiggleEnabled is not null)
                this.niTrayCbJiggleEnabled.Checked = this.jiggleTimer.Enabled;
        }

        private void UpdateNotificationAreaText()
        {
            if ( !this.cbJiggling.Checked )
            {
                this.niTray.Text = "Not jiggling the mouse.";
            }
            else
            {
                string? ww = this.cbZen.Checked ? "with" : "without";
                this.niTray.Text = $"Jiggling mouse every {this.upDownInterval.Value} s, {ww} Zen.";
            }
        }

        private void btnTrayify_Click(object sender, EventArgs e)
        {
            this.MinimizeToTray();
        }

        #endregion
    }
}
