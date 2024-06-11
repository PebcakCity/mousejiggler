namespace PebcakCity.MouseJiggler
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            flpLayout = new FlowLayoutPanel();
            panelMain = new Panel();
            btnAbout = new Button();
            cbSettings = new CheckBox();
            btnTrayify = new Button();
            cbJiggling = new CheckBox();
            labelInterval = new Label();
            upDownInterval = new NumericUpDown();
            panelSettings = new Panel();
            cbMinimize = new CheckBox();
            cbJiggleOnStart = new CheckBox();
            cbZen = new CheckBox();
            jiggleTimer = new System.Windows.Forms.Timer(components);
            niTray = new NotifyIcon(components);
            toolTip1 = new ToolTip(components);
            panelMain.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)upDownInterval ).BeginInit();
            panelSettings.SuspendLayout();
            SuspendLayout();
            // 
            // flpLayout
            // 
            flpLayout.AutoSize = true;
            flpLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpLayout.Location = new Point(1, -1);
            flpLayout.Name = "flpLayout";
            flpLayout.Size = new Size(0, 0);
            flpLayout.TabIndex = 0;
            // 
            // panelMain
            // 
            panelMain.Controls.Add(btnAbout);
            panelMain.Controls.Add(cbSettings);
            panelMain.Controls.Add(btnTrayify);
            panelMain.Controls.Add(cbJiggling);
            panelMain.Location = new Point(12, 3);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(279, 33);
            panelMain.TabIndex = 1;
            // 
            // btnAbout
            // 
            btnAbout.Location = new Point(184, 4);
            btnAbout.Name = "btnAbout";
            btnAbout.Size = new Size(43, 23);
            btnAbout.TabIndex = 3;
            btnAbout.Text = "?";
            toolTip1.SetToolTip(btnAbout, "What's this?");
            btnAbout.UseVisualStyleBackColor = true;
            btnAbout.Click +=  btnAbout_Clicked ;
            // 
            // cbSettings
            // 
            cbSettings.AutoSize = true;
            cbSettings.Location = new Point(92, 7);
            cbSettings.Name = "cbSettings";
            cbSettings.Size = new Size(77, 19);
            cbSettings.TabIndex = 2;
            cbSettings.Text = "Settings...";
            toolTip1.SetToolTip(cbSettings, "Show/hide settings panel");
            cbSettings.UseVisualStyleBackColor = true;
            cbSettings.CheckedChanged +=  cbSettings_CheckedChanged ;
            // 
            // btnTrayify
            // 
            btnTrayify.Location = new Point(233, 4);
            btnTrayify.Name = "btnTrayify";
            btnTrayify.Size = new Size(43, 23);
            btnTrayify.TabIndex = 1;
            btnTrayify.Text = "🔽";
            toolTip1.SetToolTip(btnTrayify, "Minimize to tray");
            btnTrayify.UseVisualStyleBackColor = true;
            btnTrayify.Click +=  btnTrayify_Click ;
            // 
            // cbJiggling
            // 
            cbJiggling.AutoSize = true;
            cbJiggling.Location = new Point(3, 7);
            cbJiggling.Name = "cbJiggling";
            cbJiggling.Size = new Size(72, 19);
            cbJiggling.TabIndex = 0;
            cbJiggling.Text = "Jiggling?";
            toolTip1.SetToolTip(cbJiggling, "Enable/disable jiggling");
            cbJiggling.UseVisualStyleBackColor = true;
            cbJiggling.CheckedChanged +=  cbJiggling_CheckedChanged ;
            // 
            // labelInterval
            // 
            labelInterval.AutoSize = true;
            labelInterval.Location = new Point(145, 7);
            labelInterval.Name = "labelInterval";
            labelInterval.Size = new Size(65, 15);
            labelInterval.TabIndex = 2;
            labelInterval.Text = "Interval (s):";
            // 
            // upDownInterval
            // 
            upDownInterval.Increment = new decimal(new int[] { 60, 0, 0, 0 });
            upDownInterval.Location = new Point(216, 3);
            upDownInterval.Maximum = new decimal(new int[] { 3600, 0, 0, 0 });
            upDownInterval.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            upDownInterval.Name = "upDownInterval";
            upDownInterval.Size = new Size(60, 23);
            upDownInterval.TabIndex = 1;
            toolTip1.SetToolTip(upDownInterval, "Seconds between jiggles");
            upDownInterval.Value = new decimal(new int[] { 1, 0, 0, 0 });
            upDownInterval.ValueChanged +=  upDownInterval_ValueChanged ;
            // 
            // panelSettings
            // 
            panelSettings.Controls.Add(cbMinimize);
            panelSettings.Controls.Add(labelInterval);
            panelSettings.Controls.Add(upDownInterval);
            panelSettings.Controls.Add(cbJiggleOnStart);
            panelSettings.Controls.Add(cbZen);
            panelSettings.Location = new Point(12, 42);
            panelSettings.Name = "panelSettings";
            panelSettings.Size = new Size(279, 66);
            panelSettings.TabIndex = 2;
            panelSettings.Visible = false;
            // 
            // cbMinimize
            // 
            cbMinimize.AutoSize = true;
            cbMinimize.Location = new Point(145, 32);
            cbMinimize.Name = "cbMinimize";
            cbMinimize.Size = new Size(123, 19);
            cbMinimize.TabIndex = 2;
            cbMinimize.Text = "Minimize on start?";
            toolTip1.SetToolTip(cbMinimize, "Minimize to tray on program start");
            cbMinimize.UseVisualStyleBackColor = true;
            cbMinimize.CheckedChanged +=  cbMinimize_CheckedChanged ;
            // 
            // cbJiggleOnStart
            // 
            cbJiggleOnStart.AutoSize = true;
            cbJiggleOnStart.Location = new Point(3, 32);
            cbJiggleOnStart.Name = "cbJiggleOnStart";
            cbJiggleOnStart.Size = new Size(104, 19);
            cbJiggleOnStart.TabIndex = 1;
            cbJiggleOnStart.Text = "Jiggle on start?";
            toolTip1.SetToolTip(cbJiggleOnStart, "Begin jiggling on program start");
            cbJiggleOnStart.UseVisualStyleBackColor = true;
            cbJiggleOnStart.CheckedChanged +=  cbJiggleOnStart_CheckedChanged ;
            // 
            // cbZen
            // 
            cbZen.AutoSize = true;
            cbZen.Location = new Point(3, 7);
            cbZen.Name = "cbZen";
            cbZen.Size = new Size(83, 19);
            cbZen.TabIndex = 0;
            cbZen.Text = "Zen jiggle?";
            toolTip1.SetToolTip(cbZen, "Jiggle invisibly");
            cbZen.UseVisualStyleBackColor = true;
            cbZen.CheckedChanged +=  cbZen_CheckedChanged ;
            // 
            // jiggleTimer
            // 
            jiggleTimer.Interval = 1000;
            jiggleTimer.Tick +=  jiggleTimer_Tick ;
            // 
            // niTray
            // 
            niTray.Icon = (Icon)resources.GetObject("niTray.Icon");
            niTray.Text = "Mouse Jiggler";
            niTray.Visible = true;
            niTray.DoubleClick +=  niTray_DoubleClick ;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(303, 161);
            Controls.Add(panelSettings);
            Controls.Add(panelMain);
            Controls.Add(flpLayout);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "MainForm";
            Text = "Mouse Jiggler";
            Load +=  MainForm_Load ;
            Shown +=  MainForm_Shown ;
            panelMain.ResumeLayout(false);
            panelMain.PerformLayout();
            ( (System.ComponentModel.ISupportInitialize)upDownInterval ).EndInit();
            panelSettings.ResumeLayout(false);
            panelSettings.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private FlowLayoutPanel flpLayout;
        private Panel panelMain;
        private Label labelInterval;
        private NumericUpDown upDownInterval;
        private CheckBox cbJiggling;
        private Panel panelSettings;
        private CheckBox cbZen;
        private CheckBox cbMinimize;
        private CheckBox cbJiggleOnStart;
        private Button btnTrayify;
        private CheckBox cbSettings;
        private System.Windows.Forms.Timer jiggleTimer;
        private NotifyIcon niTray;
        private Button btnAbout;
        private ToolTip toolTip1;
    }
}
