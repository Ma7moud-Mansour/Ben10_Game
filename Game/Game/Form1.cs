using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;


namespace Game
{
    public partial class Form1 : Form
    {
        private IWavePlayer selectOutput;
        private AudioFileReader selectReader;
        Bitmap off;
        Timer IntroTimer = new Timer();
        Timer GameTimer = new Timer();
        int CurrentFrame = 1;
        int IntroFrameCount = 120;
        private System.Media.SoundPlayer introSnd;
        int CurrentMenu = -1;
        bool Space = false;

        public Form1()
        {
            this.Icon = new Icon("Assets/Logo.ico");
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Paint += Form1_Paint;
            this.Load += Form1_Load;
            IntroTimer.Tick += IntroTimer_Tick;
            GameTimer.Tick += GameTimer_Tick;
            this.MouseMove += Form1_MouseMove;
            this.MouseDown += Form1_MouseDown;
            this.KeyDown += Form1_KeyDown;
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            DrawDubb();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && !IntroTimer.Enabled)
            {
                if (GameTimer.Enabled)
                {
                    GameTimer.Stop();
                }
                else
                {
                    GameTimer.Start();
                }
            }
            else if (e.KeyCode == Keys.Space)
            {
                Space = true;
                CurrentMenu = 0;
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            int w = this.ClientSize.Width;
            int h = this.ClientSize.Height;
            int x = e.X;
            int y = e.Y;
            if(CurrentMenu == 1)
            {
                Graphics g = this.CreateGraphics();
                g.DrawImage(new Bitmap("Assets/Menu/Cover.jpg"), 0, 0, this.ClientSize.Width, this.ClientSize.Height);
                System.Threading.Thread.Sleep(1000);
                CurrentMenu = -1;
                GameTimer.Interval = 100;
                GameTimer.Start();
            }
            else if (CurrentMenu == 2)
            {

            }
            else if (CurrentMenu == 3)
            {
                Graphics g = this.CreateGraphics();
                g.DrawImage(new Bitmap("Assets/Menu/Cover.jpg"), 0, 0, this.ClientSize.Width, this.ClientSize.Height);
                System.Threading.Thread.Sleep(2000);
                this.Close();
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (CurrentMenu != -1)
            {
                int w = this.ClientSize.Width;
                int h = this.ClientSize.Height;
                int x = e.X;
                int y = e.Y;
                if (x >= w * 5 / 64 && x <= w * 133 / 384 && y >= h * 35 / 72 && y <= h * 43 / 72)
                {
                    Graphics g = this.CreateGraphics();
                    this.Cursor = Cursors.Hand;
                    if (CurrentMenu != 1)
                    {
                        g.DrawImage(new Bitmap("Assets/Menu/Menu.jpg"), 0, 0, this.ClientSize.Width, this.ClientSize.Height);
                        StopSound();
                        PlaySound("Select");
                    }
                    CurrentMenu = 1;
                    g.DrawImage(new Bitmap("Assets/Menu/Menu_Hover_1.jpg"), 0, 0, this.ClientSize.Width, this.ClientSize.Height);
                    
                }
                else if (x >= w * 5 / 64 && x <= w * 133 / 384 && y >= h * 17 / 27 && y <= h * 3 / 4)
                {
                    Graphics g = this.CreateGraphics();
                    this.Cursor = Cursors.Hand;
                    if (CurrentMenu != 2)
                    {
                        g.DrawImage(new Bitmap("Assets/Menu/Menu.jpg"), 0, 0, this.ClientSize.Width, this.ClientSize.Height);
                        StopSound();
                        PlaySound("Select");
                    }
                    CurrentMenu = 2;
                    g.DrawImage(new Bitmap("Assets/Menu/Menu_Hover_2.jpg"), 0, 0, this.ClientSize.Width, this.ClientSize.Height);
                }
                else if (x >= w * 5 / 64 && x <= w * 133 / 384 && y >= h * 169 / 216 && y <= h * 97 / 108)
                {
                    Graphics g = this.CreateGraphics();
                    this.Cursor = Cursors.Hand;
                    if (CurrentMenu != 3)
                    {
                        g.DrawImage(new Bitmap("Assets/Menu/Menu.jpg"), 0, 0, this.ClientSize.Width, this.ClientSize.Height);
                        StopSound();
                        PlaySound("TimeOut");
                    }
                    CurrentMenu = 3;
                    g.DrawImage(new Bitmap("Assets/Menu/Menu_Hover_3.jpg"), 0, 0, this.ClientSize.Width, this.ClientSize.Height);
                }
                else
                {
                    CurrentMenu = 0;
                    this.Cursor = Cursors.Default;
                    Graphics g = this.CreateGraphics();
                    g.DrawImage(new Bitmap("Assets/Menu/Menu.jpg"), 0, 0, this.ClientSize.Width, this.ClientSize.Height);
                    StopSound();
                }
            }
        }

        private void IntroTimer_Tick(object sender, EventArgs e)
        {
            CurrentFrame++;
            if (CurrentFrame > IntroFrameCount || Space == true)
            {
                IntroTimer.Stop();
                introSnd.Stop();
                CurrentMenu = 0;
                Graphics g = this.CreateGraphics();
                g.DrawImage(new Bitmap("Assets/Menu/Menu.jpg"), 0, 0, this.ClientSize.Width, this.ClientSize.Height);
            }
            else
            {
                Graphics g = this.CreateGraphics();
                g.DrawImage(new Bitmap("Assets/Intro/Intro_Frame_" + CurrentFrame + ".jpg"), 0, 0, this.ClientSize.Width, this.ClientSize.Height);
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            // Create

            // Start background music
            introSnd = new System.Media.SoundPlayer("Assets/Audio/intro_music.wav");
            introSnd.PlayLooping();
            // Start Drawing
            off = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            IntroTimer.Interval = 1;
            IntroTimer.Start();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.Black);
        }

        void DrawDubb()
        {
            Graphics g2 = Graphics.FromImage(off);
            Graphics g = this.CreateGraphics();
            DrawScene(g2);
            g.DrawImage(off, 0, 0);
        }

        void DrawScene(Graphics g)
        {
            g.Clear(Color.Black);
            DrawMap1(g);
        }

        void DrawMap1(Graphics g)
        {
            g.DrawImage(new Bitmap("Assets/Map_1/Sky.png"), 0, 0);
            g.DrawImage(new Bitmap("Assets/Map_1/Mountains.png"), 0, 0);
            g.DrawImage(new Bitmap("Assets/Map_1/Grass.png"), 0, 0);
            g.DrawImage(new Bitmap("Assets/Map_1/Trees.png"), 0, 0);
            g.DrawImage(new Bitmap("Assets/Map_1/Trees_2.png"), 0, 0);
            g.DrawImage(new Bitmap("Assets/Map_1/Ground.png"), 0, 0);
        }

        private void StopSound()
        {
            if (selectOutput != null)
            {
                selectOutput.Stop();
                selectOutput.Dispose();
                selectOutput = null;
            }
            if (selectReader != null)
            {
                selectReader.Dispose();
                selectReader = null;
            }
        }

        private void PlaySound(string s)
        {
            switch (s)
            {
                case "Select":
                    selectReader = new AudioFileReader("Assets/Audio/OmtrixSelect.wav"); 
                    break;
                case "TimeOut":
                    selectReader = new AudioFileReader("Assets/Audio/TimeOut.wav");
                    break;
                default:
                    break;
            }
            selectOutput = new WaveOutEvent();
            selectOutput.Init(selectReader);
            selectOutput.Play();
        }
    }
}
