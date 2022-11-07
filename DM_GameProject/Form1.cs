using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DM_GameProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            byte[] b = Properties.Resources.GameMenu;
            FileInfo fileInfo = new FileInfo("GameMenu.mp3");
            FileStream fs = fileInfo.OpenWrite();
            fs.Write(b, 0, b.Length);
            fs.Close();
            axWindowsMediaPlayer1.URL = fileInfo.Name;
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }

        int k = 3;

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 F2 = new Form2(k - 1);
            F2.Show();
            axWindowsMediaPlayer1.Ctlcontrols.pause();
            Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            k++;
            if (k > 3)
                k = 1;
            if (k == 1)
                BackgroundImage = Image.FromFile("..\\..\\Resources\\5kfMUeO.jpg");
            if (k == 2)
                BackgroundImage = Image.FromFile("..\\..\\Resources\\back1.jpg");
            if (k == 3)
                BackgroundImage = Image.FromFile("..\\..\\Resources\\back2.jpg");
            BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsStopped)
            {
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }
        }
    }
}
