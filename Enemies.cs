using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Media;
using System.Windows.Media;
using NPOI.SS.Formula.Functions;

namespace Perkelesimu
{
    class Enemies 
    {
        public List<PictureBox> zombiesList2 = new List<PictureBox>();
        public void MakeZombies(Form pForm)
        {
            formPelialue form1 = new formPelialue();
            PictureBox pictureBoxZombie = new PictureBox();
            pictureBoxZombie.Tag = "zombie2";
            pictureBoxZombie.Image = Properties.Resources.zup_1_png;
            pictureBoxZombie.Left = form1.randNum.Next(0, 900);
            pictureBoxZombie.Top = form1.randNum.Next(0, 800);
            pictureBoxZombie.SizeMode = PictureBoxSizeMode.AutoSize;
            zombiesList2.Add(pictureBoxZombie);
            pForm.Controls.Add(pictureBoxZombie);
        }
    }   
}
