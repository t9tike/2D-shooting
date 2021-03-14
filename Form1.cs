using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Media;
using System.Windows.Media;
using NPOI.SS.Formula.Functions;
//using System.Windows.Media;


namespace Perkelesimu
{ 
    
    
    public partial class formPelialue : Form
    {
        public bool goLeft, goRight, goUp, goDown, gameOver;
        public string facing = "up";
        public static int playerHealth = 100;
        public static int playerSpeed = 10;
        public int ammo = 10;
        public int zombieSpeed = 3;
        public static int score;
        public int sprintAmount = 99;
        public bool healthpackMaassa = false;
      
        public static int shootCount = 0;
        public static bool superAmmo = false;

        public Random randNum = new Random();
        Enemies enemiesZombi2 = new Enemies();

        public bool tauko = false;

        public List<PictureBox> zombiesList = new List<PictureBox>();

        SoundPlayer haulikko = new SoundPlayer(@"C:\Users\keijo\Desktop\Windows ohjelmointi\Harjoittelua\Perkelesimu\dsshotgn.wav");
        SoundPlayer raketinheitin = new SoundPlayer(@"C:\Users\keijo\Desktop\Windows ohjelmointi\Harjoittelua\Perkelesimu\raketinheitin.wav");

        SoundPlayer kuolinaani = new SoundPlayer(@"C:\Users\keijo\Desktop\Windows ohjelmointi\Harjoittelua\Perkelesimu\Kuoli.wav");
        SoundPlayer zombinkuolinaani = new SoundPlayer(@"C:\Users\keijo\Desktop\Windows ohjelmointi\Harjoittelua\Perkelesimu\Zombinkuolinaani.wav");
        SoundPlayer zombinkuolinaani2 = new SoundPlayer(@"C:\Users\keijo\Desktop\Windows ohjelmointi\Harjoittelua\Perkelesimu\Zombinkuolinaani2.wav");

        SoundPlayer tavaranpoiminta = new SoundPlayer(@"C:\Users\keijo\Desktop\Windows ohjelmointi\Harjoittelua\Perkelesimu\tavaranpoiminta.wav");
        SoundPlayer panoksinepoiminta = new SoundPlayer(@"C:\Users\keijo\Desktop\Windows ohjelmointi\Harjoittelua\Perkelesimu\ammopickup.wav");

        //SoundPlayer victoryMusic = new SoundPlayer(@"C:\Users\keijo\Desktop\Windows ohjelmointi\Harjoittelua\Perkelesimu\victorymusic.wav");

        //public static WMPLib.WindowsMediaPlayer mplayerHaulikko = new WMPLib.WindowsMediaPlayer();
        //public static WMPLib.WindowsMediaPlayer mplayerHaulikko2 = new WMPLib.WindowsMediaPlayer();
        Records records = new Records();

        public formPelialue()
        {
            InitializeComponent();
            RestartGame();
            //axWindowsMediaPlayerVictory.URL =  (@"C:\Users\keijo\Desktop\Windows ohjelmointi\Harjoittelua\Perkelesimu\victorymusic.wav");
            //labelShowRecord.Text = records.record.ToString();
            records.readRecord();
            labelShowRecord.Text = Records.ennatys.ToString();
        }

        private void MainTimerEvent(object sender, EventArgs e)
        {
            if (playerHealth <0)
            {
                gameOver = true;
                records.saveRecord(score);
                records.readRecord();
                labelShowRecord.Text = Records.ennatys.ToString();
                //victoryMusic.Stop();
            }
            
            //progressBarSprint.Maximum = sprintAmount;
            if (sprintAmount < 100 && sprintAmount > 0)
            {
                progressBarSprint.Value = sprintAmount;
            }  

            if (playerHealth > 1 && playerHealth < 101)
            {
                progressBarHealthBar.Value = playerHealth;
            }
            if (playerHealth > 100)
            {
                playerHealth = 100;
            }

            if (gameOver == true)
            {
                pictureBoxPlayer.Image = Properties.Resources.dead;
                timerGameTimer.Stop();
                kuolinaani.Play();
            }

            if (playerHealth <= 20)
            {
                DropHealth();
            }


            labelAmmo.Text = "Ammo: " + ammo;
            labelScore.Text = "Pisteet: " + score;

            if (goLeft == true && pictureBoxPlayer.Left > 0)
            {
                pictureBoxPlayer.Left -= playerSpeed;
            }
            if (goRight == true && pictureBoxPlayer.Left + pictureBoxPlayer.Width < this.ClientSize.Width)
            {
                pictureBoxPlayer.Left += playerSpeed;
            }
            if (goUp == true && pictureBoxPlayer.Top > 50)
            {
                pictureBoxPlayer.Top -= playerSpeed;
            }
            if (goDown == true && pictureBoxPlayer.Top + pictureBoxPlayer.Height < this.ClientSize.Height)
            {
                pictureBoxPlayer.Top += playerSpeed; 
            }

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "ammo")
                {
                    if (pictureBoxPlayer.Bounds.IntersectsWith(x.Bounds))
                    {
                        this.Controls.Remove(x);
                        ((PictureBox)x).Dispose();
                        ammo += 5;
                        tavaranpoiminta.Play();
                        panoksinepoiminta.Play();
                    }
                }

                if (x is PictureBox && (string)x.Tag == "health")
                {
                    if (pictureBoxPlayer.Bounds.IntersectsWith(x.Bounds))
                    {
                        this.Controls.Remove(x);
                        ((PictureBox)x).Dispose();
                        playerHealth += 20;
                        sprintAmount += 50;
                        tavaranpoiminta.Play();
                        healthpackMaassa = false;
                    }
                }

                // pistetään zombit seuraamaan pelaajaa

                if (x is PictureBox && (string)x.Tag == "zombie" || x is PictureBox && (string)x.Tag == "zombie2")
                {
                    if (pictureBoxPlayer.Bounds.IntersectsWith(x.Bounds))
                    {
                        playerHealth -= 10;
                        this.Controls.Remove(x);
                        ((PictureBox)x).Dispose();
                        zombiesList.Remove(((PictureBox)x));
                        MakeZombies();

                    }

                    if (x.Bounds.IntersectsWith(x.Bounds) && x.Tag == "zombie")
                    {
                        
                        
                        // tee tänne jotain
                        //x.Left += zombieSpeed;
                    }


                    if (x.Left > pictureBoxPlayer.Left)
                    {
                        x.Left -= zombieSpeed;
                        ((PictureBox)x).Image = Properties.Resources.zleft;
                    }
                    if (x.Left < pictureBoxPlayer.Left)
                    {
                        x.Left += zombieSpeed;
                        ((PictureBox)x).Image = Properties.Resources.zright;
                    }
                    if (x.Top > pictureBoxPlayer.Top)
                    {
                        x.Top -= zombieSpeed;
                        ((PictureBox)x).Image = Properties.Resources.zup;
                    }
                    if (x.Top < pictureBoxPlayer.Top)
                    {
                        x.Top += zombieSpeed;
                        ((PictureBox)x).Image = Properties.Resources.zdown;
                    }
                }

                if (x is PictureBox && (string)x.Tag == "zombie2")
                {
                    if (pictureBoxPlayer.Bounds.IntersectsWith(x.Bounds))
                    {
                        playerHealth -= 10;
                        
                        this.Controls.Remove(x);
                        ((PictureBox)x).Dispose();
                        zombiesList.Remove(((PictureBox)x));
                        MakeZombies();

                    }

                    if (x.Left > pictureBoxPlayer.Left)
                    {
                        x.Left -= zombieSpeed;
                        ((PictureBox)x).Image = Properties.Resources.zleft_1_png;
                    }
                    if (x.Left < pictureBoxPlayer.Left)
                    {
                        x.Left += zombieSpeed;
                        ((PictureBox)x).Image = Properties.Resources.zright_1_png;
                    }
                    if (x.Top > pictureBoxPlayer.Top)
                    {
                        x.Top -= zombieSpeed;
                        ((PictureBox)x).Image = Properties.Resources.zup_1_png;
                    }
                    if (x.Top < pictureBoxPlayer.Top)
                    {
                        x.Top += zombieSpeed;
                        ((PictureBox)x).Image = Properties.Resources.zdown_1_png;
                    }
                }

                foreach (Control j in this.Controls)
                {
                    if (j is PictureBox && (string)j.Tag == "bullet" && x is PictureBox && (string)x.Tag == "zombie"
                        || j is PictureBox && (string)j.Tag == "bullet" && x is PictureBox && (string)x.Tag == "zombie2") 
                    {
                        if (x.Bounds.IntersectsWith(j.Bounds))
                        {
                            score++;
                            playerHealth += 2;

                            if (superAmmo == false)
                            {
                                this.Controls.Remove(j);
                                ((PictureBox)j).Dispose();
                                x.BackgroundImage = Properties.Resources.dead;
                                ((PictureBox)x).Dispose();
                                zombiesList.Remove(((PictureBox)x));
                                MakeZombies();
                            }
                            if (superAmmo == true)
                            {
                                this.Controls.Remove(x);
                                ((PictureBox)x).Dispose();
                                zombiesList.Remove(((PictureBox)x));
                                MakeZombies();     
                            }                

                            if (score == 10)
                            {
                                MakeZombies();
                                enemiesZombi2.MakeZombies(this);
                                //zombieSpeed = 4;
                            }
                            else if (score == 20)
                            {
                                
                                MakeZombies();
                                //enemiesZombi2.MakeZombies(this);
                                //zombieSpeed = 5;
                            }
                            else if (score == 30)
                            {
                               
                                MakeZombies();
                                //enemiesZombi2.MakeZombies(this);
                                //zombieSpeed = 6;
                            }
                            else if (score == 40)
                            {
                               
                                MakeZombies();
                                //enemiesZombi2.MakeZombies(this);
                                //zombieSpeed = 7;
                            }
                            else if (score == 50)
                            {
                                //axWindowsMediaPlayerVictory.Ctlcontrols.play();
                                //timerGameTimer.Stop();
                                //MessageBox.Show("Voitit pelin jee");
                                //RestartGame();
                                MakeZombies();

                            }
                            else if (score == 60)
                            {

                                MakeZombies();
                                //enemiesZombi2.MakeZombies(this);
                                //zombieSpeed = 5;
                            }
                            else if (score == 70)
                            {

                                MakeZombies();
                                //enemiesZombi2.MakeZombies(this);
                                //zombieSpeed = 6;
                            }
                            else if (score == 80)
                            {

                                MakeZombies();
                                //enemiesZombi2.MakeZombies(this);
                                //zombieSpeed = 7;
                            }
                            else if (score == 90)
                            {

                                MakeZombies();
                                //enemiesZombi2.MakeZombies(this);
                                //zombieSpeed = 6;
                            }
                            else if (score == 100)
                            {

                                timerGameTimer.Stop();
                                MessageBox.Show("Voitit pelin jee");
                                RestartGame();
                                //records.saveRecord();
                            }

                            //arvotaan soitetaanko zombi ääni
                            int soitetaankoZombi = randNum.Next(1, 7);

                            if (soitetaankoZombi == 1)
                            {
                                zombinkuolinaani.Play();
                            }
                            if (soitetaankoZombi == 2)
                            {
                                zombinkuolinaani2.Play();
                            } 
                        }
                    }
                }
            }
        }
        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (gameOver == true)
            {
                return; 
            }

            if (e.KeyCode == Keys.Left)
            {
                goLeft = true;
                facing = "left";
                pictureBoxPlayer.Image = Properties.Resources.left;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = true;
                facing = "right";
                pictureBoxPlayer.Image = Properties.Resources.right;
            }
            if (e.KeyCode == Keys.Up)
            {
                goUp = true;
                facing = "up";
                pictureBoxPlayer.Image = Properties.Resources.up;
            }
            if (e.KeyCode == Keys.Down)
            {
                goDown = true;
                facing = "down";
                pictureBoxPlayer.Image = Properties.Resources.down;
            }

            if (e.KeyCode == Keys.Q)
            {
                if (sprintAmount <= 10)
                {
                    return;
                }
                else
                {
                    timerSprintTimer.Enabled = true;
                    timerSprintTimer.Start();
                }


            }
            if (e.KeyCode == Keys.Escape)
            {
                
                tauko = true;
                taukoPaikalla();
            }
            if (e.KeyCode == Keys.Space)
            {            
                    if (ammo > 0 )
                    {
                        shootCount++;
                        if (shootCount % 3 == 0)
                        {
                            raketinheitin.Play();
                            superAmmo = true;
                        }
                        else
                        {
                            haulikko.Play();
                            superAmmo = false;
                        }


                        
                        //mplayerHaulikko.controls.play();
                        //axWindowsMediaPlayerHaulikko.Ctlcontrols.play();
                        //soitetaankoMika = false;
                        Console.WriteLine("haulikko1");
                    }
                }
                
            }

        private void taukoPaikalla()
        {
            if (tauko == true)
            {
                timerGameTimer.Stop();
                panelTauko.Show();
                Console.WriteLine("tauko");
                panelTauko.BringToFront();
                Valikko.wplayer.controls.play();
                
            }
        }

        private void timerSprintEvent(object sender, EventArgs e)
        {
            Console.WriteLine("sprint interval", sprintAmount);
            sprintAmount -= 10;
            playerSpeed = 40;

            if (sprintAmount <=10)
            {
                timerSprintTimer.Enabled = false;
                timerSprintTimer.Stop();
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
               
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
             
            }
            if (e.KeyCode == Keys.Up)
            {
                goUp = false;
         
            }
            if (e.KeyCode == Keys.Down)
            {
                goDown = false;
                Console.WriteLine("keydown");
            }

            if (e.KeyCode == Keys.Space && ammo > 0 && !gameOver)
            {
                ammo--;
                ShootBullet(facing);


                if (ammo < 2)
                {
                    DropAmmo();
                }
            }

            if (e.KeyCode == Keys.Enter)
            {
                RestartGame();
            }

            if (e.KeyCode == Keys.Q)
            {
                timerSprintTimer.Enabled = false;
                timerSprintTimer.Stop();
                playerSpeed = 10;
            }
        }

        private void ShootBullet(string direction)
        {
            Bullet shootBullet = new Bullet();
            shootBullet.direction = direction;
            shootBullet.bulletLeft = pictureBoxPlayer.Left + (pictureBoxPlayer.Width / 2);
            shootBullet.bulletTop = pictureBoxPlayer.Top + (pictureBoxPlayer.Height / 2);
            shootBullet.MakeBullet(this);       // miksi lähetetään formi classii?????
        }

        private void timerSprintinLataaja(object sender, EventArgs e)
        {
            if (sprintAmount <100)
            {
                sprintAmount++;
            }
        }

        private void TaukoStartKlikattu(object sender, EventArgs e)
        {
            tauko = false;

            
            timerGameTimer.Start();
            panelTauko.Hide();
            Valikko.wplayer.controls.stop();
            labelShowRecord.Text = Records.ennatys.ToString();
        }

        private void pictureBoxQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MakeZombies()
        {
            
            
                PictureBox pictureBoxZombie = new PictureBox();
                pictureBoxZombie.Tag = "zombie";

             int randomColor = randNum.Next(1, 3);
            if (randomColor == 1)
            {
                pictureBoxZombie.Image = Properties.Resources.zup_1_png;
            }
            else
            {
                pictureBoxZombie.Image = Properties.Resources.zup;
            }

            pictureBoxZombie.Left = randNum.Next(0, 900);
                pictureBoxZombie.Top = randNum.Next(0, 800);
                pictureBoxZombie.SizeMode = PictureBoxSizeMode.AutoSize;
                zombiesList.Add(pictureBoxZombie);
                this.Controls.Add(pictureBoxZombie);
                pictureBoxPlayer.BringToFront();
                pictureBoxZombie.BringToFront();
            
            
           
        }

        private void DropAmmo()
        {
            PictureBox pictureboxAmmo = new PictureBox();
            pictureboxAmmo.Image = Properties.Resources.ammo_Image;
            pictureboxAmmo.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureboxAmmo.Left = randNum.Next(60, this.ClientSize.Width - pictureboxAmmo.Width);
            pictureboxAmmo.Top = randNum.Next(45, this.ClientSize.Height - pictureboxAmmo.Height);
            pictureboxAmmo.Tag = "ammo";
            this.Controls.Add(pictureboxAmmo);

            pictureboxAmmo.BringToFront();
            pictureBoxPlayer.BringToFront();
        }

        private void DropHealth()
        {
            if (healthpackMaassa == false)
            {
                PictureBox pictureBoxHealth = new PictureBox();
                pictureBoxHealth.Image = Properties.Resources.healthpack2;
                pictureBoxHealth.SizeMode = PictureBoxSizeMode.AutoSize;
                pictureBoxHealth.Left = randNum.Next(60, this.ClientSize.Width - pictureBoxHealth.Width);
                pictureBoxHealth.Top = randNum.Next(45, this.ClientSize.Height - pictureBoxHealth.Height);
                pictureBoxHealth.Tag = "health";
                this.Controls.Add(pictureBoxHealth);

                pictureBoxHealth.BringToFront();
                pictureBoxPlayer.BringToFront();
                healthpackMaassa = true;
            }
        }

        private void RestartGame()
        {
            pictureBoxPlayer.Image = Properties.Resources.up;

            foreach (PictureBox i in zombiesList)   // poistetaan zombit
            {
                this.Controls.Remove(i);
            }

            zombiesList.Clear();

            foreach (Control x in this.Controls)    // poistetaan ammuslaatikot
            {
                if (x is PictureBox && (string)x.Tag == "ammo")
                {
                    this.Controls.Remove(x);
                    ((PictureBox)x).Dispose();
                }     
            }

            foreach (Control x in this.Controls)    // poistetaan elkkapakit
            {
                if (x is PictureBox && (string)x.Tag == "health")
                {
                    this.Controls.Remove(x);
                    ((PictureBox)x).Dispose();
                }
            }

            for (int i = 0; i < 3; i++) // luodaa x zombia alus
            {
                MakeZombies();
                
            }
            
            goDown = false;
            goUp = false;
            goLeft = false;
            goRight = false;
            healthpackMaassa = false;
            zombieSpeed = 3;

            playerHealth = 100;
            score = 0;

            timerGameTimer.Start();
            timerSprintLataaja.Start();

            gameOver = false;
            ammo = 10;
        }
    }
}
