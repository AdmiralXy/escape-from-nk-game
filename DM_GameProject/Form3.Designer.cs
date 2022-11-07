namespace DM_GameProject
{
    partial class Form3
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form3));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.playerBulletTimer = new System.Windows.Forms.Timer(this.components);
            this.playerMoveTimer = new System.Windows.Forms.Timer(this.components);
            this.mineMoveTimer = new System.Windows.Forms.Timer(this.components);
            this.totalSecondsTimer = new System.Windows.Forms.Timer(this.components);
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.S_Background = new AxWMPLib.AxWindowsMediaPlayer();
            this.GliderTimer = new System.Windows.Forms.Timer(this.components);
            this.PersonHitTimer = new System.Windows.Forms.Timer(this.components);
            this.CrosshairTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.S_Background)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 20;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // playerBulletTimer
            // 
            this.playerBulletTimer.Enabled = true;
            this.playerBulletTimer.Interval = 20;
            this.playerBulletTimer.Tick += new System.EventHandler(this.playerBulletTimer_Tick);
            // 
            // playerMoveTimer
            // 
            this.playerMoveTimer.Enabled = true;
            this.playerMoveTimer.Interval = 15;
            this.playerMoveTimer.Tick += new System.EventHandler(this.playerMoveTimer_Tick);
            // 
            // mineMoveTimer
            // 
            this.mineMoveTimer.Enabled = true;
            this.mineMoveTimer.Interval = 30;
            this.mineMoveTimer.Tick += new System.EventHandler(this.mineMoveTimer_Tick);
            // 
            // totalSecondsTimer
            // 
            this.totalSecondsTimer.Enabled = true;
            this.totalSecondsTimer.Interval = 1000;
            this.totalSecondsTimer.Tick += new System.EventHandler(this.totalSecondsTimer_Tick);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox2.Location = new System.Drawing.Point(391, 157);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(16, 20);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // S_Background
            // 
            this.S_Background.Enabled = true;
            this.S_Background.Location = new System.Drawing.Point(12, 12);
            this.S_Background.Name = "S_Background";
            this.S_Background.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("S_Background.OcxState")));
            this.S_Background.Size = new System.Drawing.Size(38, 33);
            this.S_Background.TabIndex = 7;
            this.S_Background.Visible = false;
            this.S_Background.Enter += new System.EventHandler(this.S_Background_Enter);
            // 
            // GliderTimer
            // 
            this.GliderTimer.Interval = 7;
            this.GliderTimer.Tick += new System.EventHandler(this.GliderTimer_Tick);
            // 
            // PersonHitTimer
            // 
            this.PersonHitTimer.Tick += new System.EventHandler(this.PersonHitTimer_Tick);
            // 
            // CrosshairTimer
            // 
            this.CrosshairTimer.Interval = 5;
            this.CrosshairTimer.Tick += new System.EventHandler(this.CrosshairTimer_Tick);
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.S_Background);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form3";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Escape from North Korea 2D";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form3_FormClosed);
            this.Load += new System.EventHandler(this.Form3_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form3_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form3_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form3_KeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form3_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form3_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.S_Background)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer playerBulletTimer;
        private System.Windows.Forms.Timer playerMoveTimer;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Timer mineMoveTimer;
        private System.Windows.Forms.Timer totalSecondsTimer;
        public AxWMPLib.AxWindowsMediaPlayer S_Background;
        private System.Windows.Forms.Timer GliderTimer;
        private System.Windows.Forms.Timer PersonHitTimer;
        private System.Windows.Forms.Timer CrosshairTimer;
    }
}