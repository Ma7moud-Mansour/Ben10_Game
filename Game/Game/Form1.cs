using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;


namespace Game
{
    public class Map
    {
        public Bitmap img;
        public Rectangle rSrc;
        public Rectangle rDst;
        public int StartX;
        public int StartY;
        public List<Bitmap> Sky = new List<Bitmap>();
        public List<Bitmap> Mountains = new List<Bitmap>();
        public List<Bitmap> Grass = new List<Bitmap>();
        public List<Bitmap> Trees = new List<Bitmap>();
        public List<Bitmap> Ground = new List<Bitmap>();
    }
    public class Character
    {
        public string Name;
        public int X;
        public int Y;
        public int StandSpeed;
        public List<Bitmap> Stand_Right_Frames = new List<Bitmap>();
        public List<Bitmap> Stand_Left_Frames = new List<Bitmap>();
        public int WalkSpeed;
        public List<Bitmap> Walk_Right_Frames = new List<Bitmap>();
        public List<Bitmap> Walk_Left_Frames = new List<Bitmap>();
        public int RunSpeed;
        public List<Bitmap> Run_Right_Frames = new List<Bitmap>();
        public List<Bitmap> Run_Left_Frames = new List<Bitmap>();
        public int JumpSpeed;
        public List<Bitmap> Jump_Right_Frames = new List<Bitmap>();
        public List<Bitmap> Jump_Left_Frames = new List<Bitmap>();
        public int FlySpeed;
        public List<Bitmap> Fly_Right_Frames = new List<Bitmap>();
        public List<Bitmap> Fly_Left_Frames = new List<Bitmap>();
        public int HitSpeed;
        public List<Bitmap> Hit_Right_Frames = new List<Bitmap>();
        public List<Bitmap> Hit_Left_Frames = new List<Bitmap>();
        public int KickSpeed;
        public List<Bitmap> Kick_Right_Frames = new List<Bitmap>();
        public List<Bitmap> Kick_Left_Frames = new List<Bitmap>();
        public int FallSpeed;
        public List<Bitmap> Fall_Right_Frames = new List<Bitmap>();
        public List<Bitmap> Fall_Left_Frames = new List<Bitmap>();
        public int DamageSpeed;
        public List<Bitmap> Damage_Right_Frames = new List<Bitmap>();
        public List<Bitmap> Damage_Left_Frames = new List<Bitmap>();
        public int DieSpeed;
        public List<Bitmap> Die_Right_Frames = new List<Bitmap>();
        public List<Bitmap> Die_Left_Frames = new List<Bitmap>();
    }
    public class Ben10
    {
        public int X = 50;
        public int Y = 200;
        public string Character = "Humungousaur";
        public List<Character> Characters = new List<Character>();
    }
    public partial class Form1 : Form
    {
        private IWavePlayer selectOutput;
        private AudioFileReader selectReader;
        private System.Media.SoundPlayer introSnd;
        Bitmap off;
        Timer IntroTimer = new Timer();
        Timer GameTimer = new Timer();
        List<Map> Maps = new List<Map>();
        Ben10 Ben = new Ben10();
        int CurrentMap = 0;
        int CurrentFrame = 1;
        int IntroFrameCount = 120;
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
                if(IntroTimer.Enabled)
                {
                    CurrentMenu = 0;
                }
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

            Map map;
            // Map_1
            map = new Map();
            map.Sky.Add(new Bitmap("Assets/Map_1/Sky.png"));
            map.Mountains.Add(new Bitmap("Assets/Map_1/Mountains.png"));
            map.Grass.Add(new Bitmap("Assets/Map_1/Grass.png"));
            map.Trees.Add(new Bitmap("Assets/Map_1/Trees.png"));
            map.Trees.Add(new Bitmap("Assets/Map_1/Trees_2.png"));
            map.Ground.Add(new Bitmap("Assets/Map_1/Ground.png"));
            DrawMap(map, map.Ground[0].Width, map.Ground[0].Height);
            map.StartX = 0;
            map.StartY = map.img.Height - this.ClientSize.Height;
            map.rDst = new Rectangle(0, 0, this.ClientSize.Width, this.ClientSize.Height);
            map.rSrc = new Rectangle(map.StartX, map.StartY, this.ClientSize.Width, this.ClientSize.Height);
            Maps.Add(map);

            // Ben10 Characters
            StreamReader SR = new StreamReader("Read.txt");
            string line;
            if(!SR.EndOfStream)
                line = SR.ReadLine();
            while (!SR.EndOfStream)
            {
                line = SR.ReadLine();
                string[] temp = line.Split(',');
                if(temp.Count() >= 0 && temp.Count() <= 31)
                {
                    Character pnn = new Character();
                    pnn.Name = temp[0];
                    pnn.StandSpeed = int.Parse(temp[1]);
                    for (int i = 0; i < int.Parse(temp[2]); i++)
                    {
                        pnn.Walk_Right_Frames.Add(new Bitmap("Assets/Characters/" + pnn.Name + "/Walk_Right_" + (i + 1) + ".png"));
                    }
                    pnn.WalkSpeed = int.Parse(temp[4]);
                    for (int i = 0; i < int.Parse(temp[2]); i++)
                    {
                        pnn.Walk_Right_Frames.Add(new Bitmap("Assets/Characters/" + pnn.Name + "/Walk_Right_" + (i + 1) + ".png"));
                    }
                    pnn.RunSpeed = int.Parse(temp[7]);
                    for (int i = 0; i < int.Parse(temp[2]); i++)
                    {
                        pnn.Walk_Right_Frames.Add(new Bitmap("Assets/Characters/" + pnn.Name + "/Walk_Right_" + (i + 1) + ".png"));
                    }
                    pnn.JumpSpeed = int.Parse(temp[10]);
                    for (int i = 0; i < int.Parse(temp[2]); i++)
                    {
                        pnn.Walk_Right_Frames.Add(new Bitmap("Assets/Characters/" + pnn.Name + "/Walk_Right_" + (i + 1) + ".png"));
                    }
                    pnn.FlySpeed = int.Parse(temp[13]);
                    for (int i = 0; i < int.Parse(temp[2]); i++)
                    {
                        pnn.Walk_Right_Frames.Add(new Bitmap("Assets/Characters/" + pnn.Name + "/Walk_Right_" + (i + 1) + ".png"));
                    }
                    pnn.HitSpeed = int.Parse(temp[16]);
                    for (int i = 0; i < int.Parse(temp[2]); i++)
                    {
                        pnn.Walk_Right_Frames.Add(new Bitmap("Assets/Characters/" + pnn.Name + "/Walk_Right_" + (i + 1) + ".png"));
                    }
                    pnn.KickSpeed = int.Parse(temp[19]);
                    for (int i = 0; i < int.Parse(temp[2]); i++)
                    {
                        pnn.Walk_Right_Frames.Add(new Bitmap("Assets/Characters/" + pnn.Name + "/Walk_Right_" + (i + 1) + ".png"));
                    }
                    pnn.FallSpeed = int.Parse(temp[22]);
                    for (int i = 0; i < int.Parse(temp[2]); i++)
                    {
                        pnn.Walk_Right_Frames.Add(new Bitmap("Assets/Characters/" + pnn.Name + "/Walk_Right_" + (i + 1) + ".png"));
                    }
                    pnn.DamageSpeed = int.Parse(temp[25]);
                    for (int i = 0; i < int.Parse(temp[2]); i++)
                    {
                        pnn.Walk_Right_Frames.Add(new Bitmap("Assets/Characters/" + pnn.Name + "/Walk_Right_" + (i + 1) + ".png"));
                    }
                    pnn.DieSpeed = int.Parse(temp[28]);
                    for (int i = 0; i < int.Parse(temp[2]); i++)
                    {
                        pnn.Walk_Right_Frames.Add(new Bitmap("Assets/Characters/" + pnn.Name + "/Walk_Right_" + (i + 1) + ".png"));
                    }
                    Ben.Characters.Add(pnn);
                }
            }


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

        void DrawMap(Map map, int W, int H)
        {
            map.img = new Bitmap(W, H);
            Graphics g = Graphics.FromImage(map.img);
            for (int i = 0; i < map.Sky.Count; i++)
            {
                g.DrawImage(map.Sky[i], 0, 0, map.img.Width, map.img.Height);
            }
            for (int i = 0; i < map.Mountains.Count; i++)
            {
                g.DrawImage(map.Mountains[i], 0, 0, map.img.Width, map.img.Height);
            }
            for (int i = 0; i < map.Grass.Count; i++)
            {
                g.DrawImage(map.Grass[i], 0, 0, map.img.Width, map.img.Height);
            }
            for (int i = 0; i < map.Trees.Count; i++)
            {
                g.DrawImage(map.Trees[i], 0, 0, map.img.Width, map.img.Height);
            }
            for (int i = 0; i < map.Ground.Count; i++)
            {
                g.DrawImage(map.Ground[i], 0, 0, map.img.Width, map.img.Height);
            }
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
            g.DrawImage(Maps[CurrentMap].img, Maps[CurrentMap].rDst, Maps[CurrentMap].rSrc, GraphicsUnit.Pixel);
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
