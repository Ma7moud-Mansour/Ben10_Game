using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game
{
    public partial class Form1 : Form
    {
        Bitmap off;
        Timer IntroTimer = new Timer();
        int CurrentFrame = 0;
        int IntroFrameCount = 330;

        public Form1()
        {
            this.Icon = new Icon("Assets/Logo.ico");
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Paint += Form1_Paint;
            this.Load += Form1_Load;
            IntroTimer.Tick += IntroTimer_Tick;
            this.MouseMove += Form1_MouseMove;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void IntroTimer_Tick(object sender, EventArgs e)
        {
            CurrentFrame++;
            if (CurrentFrame > IntroFrameCount)
            {
                IntroTimer.Stop();
                player.Stop();
                Menu();
            }
            else
            {
                Graphics g = this.CreateGraphics();
                g.DrawImage(new Bitmap("Assets/Intro/Intro_Frame_" + CurrentFrame + ".jpg"), 0, 0, this.ClientSize.Width, this.ClientSize.Height);
            }
        }

        private System.Media.SoundPlayer player;

        private void Form1_Load(object sender, EventArgs e)
        {
            // Create
            // Start

            // Start background music
            player = new System.Media.SoundPlayer("Assets/Audio/intro_music.wav");
            player.PlayLooping(); // يشغلها في لوب طول فترة الانترو


            off = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            IntroTimer.Interval = 1;
            IntroTimer.Start();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = this.CreateGraphics();
            g.DrawImage(new Bitmap("Assets/Intro/Intro_Frame_1.jpg"), 0, 0, this.ClientSize.Width, this.ClientSize.Height);
        }

        void DrawDubb(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            DrawScene(g2);
            g.DrawImage(off, 0, 0);
        }

        void DrawScene(Graphics g)
        {
            // Drawing logic goes here
            g.Clear(Color.Black);
            // Example: draw a simple rectangle
            g.FillRectangle(Brushes.Red, 50, 50, 100, 100);
        }


        void Menu()
        {
            Graphics g = this.CreateGraphics();
            g.DrawImage(new Bitmap("Assets/Intro/Menu.jpg"), 0, 0, this.ClientSize.Width, this.ClientSize.Height);
        }
    }
}
