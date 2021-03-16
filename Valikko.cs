using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Perkelesimu
{
    public partial class Valikko : Form
    {
        public static WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();
        public Valikko()
        {
            InitializeComponent();

            wplayer.URL = (@"***");
            wplayer.controls.play();
            axWindowsMediaPlayer1.Hide();
        }

       

        private void pictureBoxOptions_Click(object sender, EventArgs e)
        {
            //FormOptionPage option = new FormOptionPage();
            //option.ShowDialog();
        }

        private void pictureBoxStart_MouseHover_1(object sender, EventArgs e)
        {
            pictureBoxStart.Image = Properties.Resources.start2;
            System.Media.SoundPlayer sound = new System.Media.SoundPlayer(@"***");
            sound.Play();
        }

        private void pictureBoxStart_MouseLeave_1(object sender, EventArgs e)
        {
            pictureBoxStart.Image = Properties.Resources.start;
        }

        private void pictureBoxOptions_MouseHover_1(object sender, EventArgs e)
        {
            pictureBoxOptions.Image = Properties.Resources.settings2;
            System.Media.SoundPlayer sound = new System.Media.SoundPlayer(@"***");
            sound.Play();
        }

        private void pictureBoxOptions_MouseLeave_1(object sender, EventArgs e)
        {
            pictureBoxOptions.Image = Properties.Resources.settings;
        }

        private void pictureBoxQuit_MouseHover_1(object sender, EventArgs e)
        {
            pictureBoxQuit.Image = Properties.Resources.quit2;
            System.Media.SoundPlayer sound = new System.Media.SoundPlayer(@"***");
            sound.Play();
        }

        private void pictureBoxQuit_MouseLeave_1(object sender, EventArgs e)
        {
            pictureBoxQuit.Image = Properties.Resources.quit;
        }

        private void pictureBoxQuit_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBoxStart_Click_1(object sender, EventArgs e)
        {
            formPelialue form2 = new formPelialue();
            wplayer.controls.stop();
            this.Hide();
              
            form2.Closed += (s, args) => this.Close();
            form2.Show();   
        }
    }
}
