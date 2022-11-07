using System;
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
    public partial class Form2 : Form
    {

        public Form2(int k)
        {
            InitializeComponent();
            k++;
            if (k == 1)
                BackgroundImage = Image.FromFile("..\\..\\Resources\\5kfMUeO.jpg");
            if (k == 2)
                BackgroundImage = Image.FromFile("..\\..\\Resources\\back1.jpg");
            if (k == 3)
                BackgroundImage = Image.FromFile("..\\..\\Resources\\back2.jpg");
            BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form3 F3 = new Form3();
            F3.Show();
            Hide();
        }
    }
}
