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
        public List<Enemy> Enemies = new List<Enemy>();
        public List<int> ScrollingFocus = new List<int>();
        public int MarginGravity = 50;
    }
    public class Character
    {
        public string Name;
        public int Width;
        public int Height;
        public string CurrentMotion = "Stand_Right";
        public int CurrentFrame = 0;
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
        public Rectangle rDst = new Rectangle(50, 820, 0, 0);
        public Rectangle rSrc = new Rectangle();
        public string Character = "Humungousaur";
        public int Index = 0;
        public List<Character> Characters = new List<Character>();
        public Bitmap CurrentBenImg = new Bitmap("Assets/Logo.ico");
        public string BenDirection = "Right";
        public string BenMotion = "Stand_Right";
        public int CurrentSpeed;
        public int CurrentFramesCt;
        public bool BenAcessMove = true;
        public string ReadyMotion = "Not_Ready";
    }
    public class Enemy : Character
    {
        public Rectangle rDst = new Rectangle();
        public Rectangle rSrc = new Rectangle();
        public int Health = 100;
        public int Damage = 10;
        public int Speed = 5;
        public bool IsAlive = true;
    }
    public partial class Form1 : Form
    {
        private IWavePlayer selectOutput;
        private AudioFileReader selectReader;
        private System.Media.SoundPlayer introSnd;
        private Timer IntroTimer = new Timer();
        private Timer GameTimer = new Timer();
        private Ben10 Ben = new Ben10();
        private List<Map> Maps = new List<Map>();
        private Bitmap off;
        private int CurrentMap = 0;
        private int CurrentFrame = 1;
        private int IntroFrameCount = 120;
        private int CurrentMenu = -1;
        private bool Space = false;
        private string Difficulty = "Easy";
        private float SoundVolume = 1.0f;

        public Form1()
        {
            this.Icon = new Icon("Assets/Logo.ico");
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Paint += Form1_Paint;
            this.Load += Form1_Load;
            this.MouseMove += Form1_MouseMove;
            this.MouseDown += Form1_MouseDown;
            this.KeyDown += Form1_KeyDown;
            this.KeyUp += Form1_KeyUp;
            IntroTimer.Tick += IntroTimer_Tick;
            GameTimer.Tick += GameTimer_Tick;
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            BenMotionImg(Ben.BenMotion, Ben.BenDirection);
            BenMotion(Ben.BenMotion, Ben.BenDirection);
            Gravity();
            Scrolling();
            Ben.CurrentBenImg = BenImg();
            ManageBenRectanglesTheme();
            DrawDubb();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(Ben.BenAcessMove)
            {
                if (e.KeyCode == Keys.Right)
                {
                    if(Ben.ReadyMotion != "Run_Right")
                    {
                        Ben.BenMotion = "Walk";
                        if (Ben.ReadyMotion != "Ready_Run_Right" && Ben.ReadyMotion != "Not_Ready_Run_Right")
                        {
                            Ben.ReadyMotion = "Ready_Run_Right";
                        }
                        else
                        {
                            Ben.ReadyMotion = "Not_Ready_Run_Right";
                        }
                    }
                    else
                    {
                        Ben.BenMotion = "Run";
                    }
                    Ben.BenDirection = "Right";
                }
                else if (e.KeyCode == Keys.Left)
                {
                    if (Ben.ReadyMotion != "Run_Left")
                    {
                        Ben.BenMotion = "Walk";
                        if (Ben.ReadyMotion != "Ready_Run_Left" && Ben.ReadyMotion != "Not_Ready_Run_Left")
                        {
                            Ben.ReadyMotion = "Ready_Run_Left";
                        }
                        else
                        {
                            Ben.ReadyMotion = "Not_Ready_Run_Left";
                        }
                    }
                    else
                    {
                        Ben.BenMotion = "Run";
                    }
                    Ben.BenDirection = "Left";
                }
                else if (e.KeyCode == Keys.Escape && !IntroTimer.Enabled)
                {
                    if (GameTimer.Enabled)
                    {
                        CurrentMenu = 22;
                        DrawSettingPage();
                        GameTimer.Stop();
                    }
                    else
                    {
                        CurrentMenu = -1;
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
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if(Ben.BenAcessMove)
            {
                if (e.KeyCode == Keys.Right && Ben.ReadyMotion == "Ready_Run_Right")
                {
                    Ben.ReadyMotion = "Run_Right";
                }
                else if (e.KeyCode == Keys.Left && Ben.ReadyMotion == "Ready_Run_Left")
                {
                    Ben.ReadyMotion = "Run_Left";
                }
                else
                {
                    Ben.ReadyMotion = "Not_Ready";
                }
            }
            else
            {
                Ben.ReadyMotion = "Not_Ready";
            }
            Ben.BenMotion = "Stand";
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            int w = this.ClientSize.Width;
            int h = this.ClientSize.Height;
            int x = e.X;
            int y = e.Y;
            Graphics g = this.CreateGraphics();
            if(CurrentMenu == -2)
            {
                // Init Setting Drawing
                DrawSettingPage();
            }
            if(CurrentMenu == 1)
            {
                // Start Game
                g.DrawImage(new Bitmap("Assets/Menu/Cover.jpg"), 0, 0, this.ClientSize.Width, this.ClientSize.Height);
                System.Threading.Thread.Sleep(1000);
                CurrentMenu = -1;
                GameTimer.Interval = 100;
                GameTimer.Start();
            }
            else if (CurrentMenu == 2 || CurrentMenu == 22 || CurrentMenu == 4 || CurrentMenu == 44)
            {
                // Back From Credits To Settings
                if (CurrentMenu == 4 || CurrentMenu == 44)
                {
                    if (y >= 22 * h / 27 && y <= 69 * h / 72 && x >= 7 * w / 16 && x <= 7 * w / 12)
                    {
                        if (CurrentMenu == 4)
                        {
                            CurrentMenu = 2;
                        }
                        else if (CurrentMenu == 44)
                        {
                            CurrentMenu = 22;
                        }
                        DrawSettingPage();
                        return;
                    }
                }
                // Defficulty Settings
                else if (y >= 71 * h / 216 && y <= 29 * h / 72)
                {
                    if (x >= 179 * w / 384 && x <= 13 * w / 24)
                    {
                        Difficulty = "Easy";
                        PlaySound("Select");
                        DrawSettingPage();
                    }
                    else if (x >= 35 * w / 64 && x <= 121 * w / 192)
                    {
                        Difficulty = "Medium";
                        PlaySound("Select");
                        DrawSettingPage();
                    }
                    else if (x >= 61 * w / 96 && x <= 275 * w / 384)
                    {
                        Difficulty = "Hard";
                        PlaySound("Select");
                        DrawSettingPage();
                    }
                }
                // Cancel Settings
                else if (y >= 1 * h / 9 && y <= 17 * h / 108 && x >= 17 * w / 24 && x <= 47 * w / 64)
                {
                    if (CurrentMenu == 22)
                    {
                        CurrentMenu = -1;
                        GameTimer.Start();
                    }
                    else
                    {
                        CurrentMenu = 0;
                    }
                }
                // Mute Audio Settings
                else if (y >= 13 * h / 27 && y <= 119 * h / 216 && x >= 121 * w / 192 && x <= 271 * w / 384)
                {
                    if (SoundVolume == 0f)
                    {
                        SoundVolume = 1f;
                    }
                    else
                    {
                        SoundVolume = 0f;
                    }
                    PlaySound("Select");
                    DrawSettingPage();
                }
                // Exit Settings
                else if ((CurrentMenu == 2 || CurrentMenu == 22) && (y >= 79 * h / 108 && y <= 23 * h / 27 && x >= 115 * w / 384 && x <= 277 * w / 384))
                {
                    PlaySound("TimeOut");
                    g.DrawImage(new Bitmap("Assets/Menu/Cover.jpg"), 0, 0, this.ClientSize.Width, this.ClientSize.Height);
                    System.Threading.Thread.Sleep(3000);
                    this.Close();
                }
                // Credits Settings
                else if (y >= 43 * h / 72 && y <= 155 * h / 215 && x >= 115 * w / 384 && x <= 277 * w / 384)
                {
                    if (CurrentMenu == 2)
                    {
                        CurrentMenu = 4;
                    }
                    else if (CurrentMenu == 22)
                    {
                        CurrentMenu = 44;
                    }
                    PlaySound("Select");
                    if (CurrentMenu == 4)
                    {
                        g.DrawImage(new Bitmap("Assets/Menu/Credits_Page_Hover.png"), 0, 0, this.ClientSize.Width, this.ClientSize.Height);
                    }
                    else if (CurrentMenu == 44)
                    {
                        g.DrawImage(new Bitmap("Assets/Menu/Credits_NoBg.png"), 0, 0, this.ClientSize.Width, this.ClientSize.Height);
                    }
                }
                // Audio Volume Settings
                else if (y >= 543 * h / 1080 && y <= 575 * h / 1080)
                {
                    int startX = 895 * w / 1920;
                    for (int i = 0; i < 10; i++)
                    {
                        if (x >= startX && x <= startX + (25 * w / 1920))
                        {
                            SoundVolume = (i + 1) / 10f;
                            PlaySound("Select");
                            DrawSettingPage();
                            break;
                        }
                        startX += (5 * w / 1920) + (25 * w / 1920);
                    }
                }
            }
            // Exit Settings
            else if (CurrentMenu == 3)
            {
                g.DrawImage(new Bitmap("Assets/Menu/Cover.jpg"), 0, 0, this.ClientSize.Width, this.ClientSize.Height);
                System.Threading.Thread.Sleep(3000);
                this.Close();
            }
            if (CurrentMenu == -2)
            {
                // Init Setting Opening
                CurrentMenu = 2;
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (CurrentMenu != -1 && CurrentMenu != 2 && CurrentMenu != 4 && CurrentMenu != 22 && CurrentMenu != 44)
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
                    if (CurrentMenu != 2 && CurrentMenu != -2)
                    {
                        g.DrawImage(new Bitmap("Assets/Menu/Menu.jpg"), 0, 0, this.ClientSize.Width, this.ClientSize.Height);
                        StopSound();
                        PlaySound("Select");
                    }
                    CurrentMenu = -2; // It is negative to be flag to first draw of the Setting Page
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
            map.Trees.Add(new Bitmap("Assets/Map_1/Trees_3.png"));
            map.Ground.Add(new Bitmap("Assets/Map_1/Ground.png"));
            DrawMap(map, map.Ground[0].Width, map.Ground[0].Height);
            map.StartX = 0;
            map.StartY = map.img.Height - this.ClientSize.Height;
            map.rDst = new Rectangle(0, 0, this.ClientSize.Width, this.ClientSize.Height);
            map.rSrc = new Rectangle(map.StartX, map.StartY, this.ClientSize.Width, this.ClientSize.Height);
            map.ScrollingFocus.Add(1000);
            map.ScrollingFocus.Add(3000);
            map.ScrollingFocus.Add(4000);
            map.ScrollingFocus.Add(6000);
            Maps.Add(map);

            // Ben10 Characters
            StreamReader SR = new StreamReader("Assets/Characters/Ben10_Characters.txt");
            string line;
            if(!SR.EndOfStream)
                line = SR.ReadLine();
            while (!SR.EndOfStream)
            {
                line = SR.ReadLine();
                string[] temp = line.Split(',');
                int iTemp = 0;
                if(temp.Count() >= 0 && temp.Count() <= 31)
                {
                    Character pnn = new Character();
                    pnn.Name = temp[iTemp++];
                    pnn.StandSpeed = int.Parse(temp[iTemp++]);
                    for (int i = 0; i < int.Parse(temp[iTemp]); i++)
                    {
                        pnn.Stand_Right_Frames.Add(new Bitmap("Assets/Characters/" + pnn.Name + "/Stand_Right/" + pnn.Name + "_Stand_Right_Frame_" + (i + 1) + ".png"));
                    }
                    iTemp++;
                    for (int i = 0; i < int.Parse(temp[iTemp]); i++)
                    {
                        pnn.Stand_Left_Frames.Add(new Bitmap("Assets/Characters/" + pnn.Name + "/Stand_Left/" + pnn.Name + "_Stand_Left_Frame_" + (i + 1) + ".png"));
                    }
                    iTemp++;
                    pnn.WalkSpeed = int.Parse(temp[iTemp++]);
                    for (int i = 0; i < int.Parse(temp[iTemp]); i++)
                    {
                        pnn.Walk_Right_Frames.Add(new Bitmap("Assets/Characters/" + pnn.Name + "/Walk_Right/" + pnn.Name + "_Walk_Right_Frame_" + (i + 1) + ".png"));
                    }
                    iTemp++;
                    for (int i = 0; i < int.Parse(temp[iTemp]); i++)
                    {
                        pnn.Walk_Left_Frames.Add(new Bitmap("Assets/Characters/" + pnn.Name + "/Walk_Left/" + pnn.Name + "_Walk_Left_Frame_" + (i + 1) + ".png"));
                    }
                    iTemp++;
                    pnn.RunSpeed = int.Parse(temp[iTemp++]);
                    for (int i = 0; i < int.Parse(temp[iTemp]); i++)
                    {
                        pnn.Run_Right_Frames.Add(new Bitmap("Assets/Characters/" + pnn.Name + "/Run_Right/" + pnn.Name + "_Run_Right_Frame_" + (i + 1) + ".png"));
                    }
                    iTemp++;
                    for (int i = 0; i < int.Parse(temp[iTemp]); i++)
                    {
                        pnn.Run_Left_Frames.Add(new Bitmap("Assets/Characters/" + pnn.Name + "/Run_Left/" + pnn.Name + "_Run_Left_Frame_" + (i + 1) + ".png"));
                    }
                    iTemp++;
                    pnn.JumpSpeed = int.Parse(temp[iTemp++]);
                    for (int i = 0; i < int.Parse(temp[iTemp]); i++)
                    {
                        pnn.Jump_Right_Frames.Add(new Bitmap("Assets/Characters/" + pnn.Name + "/Jump_Right/" + pnn.Name + "_Jump_Right_Frame_" + (i + 1) + ".png"));
                    }
                    iTemp++;
                    for (int i = 0; i < int.Parse(temp[iTemp]); i++)
                    {
                        pnn.Jump_Left_Frames.Add(new Bitmap("Assets/Characters/" + pnn.Name + "/Jump_Left/" + pnn.Name + "_Jump_Left_Frame_" + (i + 1) + ".png"));
                    }
                    iTemp++;
                    pnn.FlySpeed = int.Parse(temp[iTemp++]);
                    for (int i = 0; i < int.Parse(temp[iTemp]); i++)
                    {
                        pnn.Fly_Right_Frames.Add(new Bitmap("Assets/Characters/" + pnn.Name + "/Fly_Right/" + pnn.Name + "_Fly_Right_Frame_" + (i + 1) + ".png"));
                    }
                    iTemp++;
                    for (int i = 0; i < int.Parse(temp[iTemp]); i++)
                    {
                        pnn.Fly_Left_Frames.Add(new Bitmap("Assets/Characters/" + pnn.Name + "/Fly_Left/" + pnn.Name + "_Fly_Left_Frame_" + (i + 1) + ".png"));
                    }
                    iTemp++;
                    pnn.HitSpeed = int.Parse(temp[iTemp++]);
                    for (int i = 0; i < int.Parse(temp[iTemp]); i++)
                    {
                        pnn.Hit_Right_Frames.Add(new Bitmap("Assets/Characters/" + pnn.Name + "/Hit_Right/" + pnn.Name + "_Hit_Right_Frame_" + (i + 1) + ".png"));
                    }
                    iTemp++;
                    for (int i = 0; i < int.Parse(temp[iTemp]); i++)
                    {
                        pnn.Hit_Left_Frames.Add(new Bitmap("Assets/Characters/" + pnn.Name + "/Hit_Left/" + pnn.Name + "_Hit_Left_Frame_" + (i + 1) + ".png"));
                    }
                    iTemp++;
                    pnn.KickSpeed = int.Parse(temp[iTemp++]);
                    for (int i = 0; i < int.Parse(temp[iTemp]); i++)
                    {
                        pnn.Kick_Right_Frames.Add(new Bitmap("Assets/Characters/" + pnn.Name + "/Kick_Right/" + pnn.Name + "_Kick_Right_Frame_" + (i + 1) + ".png"));
                    }
                    iTemp++;
                    for (int i = 0; i < int.Parse(temp[iTemp]); i++)
                    {
                        pnn.Kick_Left_Frames.Add(new Bitmap("Assets/Characters/" + pnn.Name + "/Kick_Left/" + pnn.Name + "_Kick_Left_Frame_" + (i + 1) + ".png"));
                    }
                    iTemp++;
                    pnn.FallSpeed = int.Parse(temp[iTemp++]);
                    for (int i = 0; i < int.Parse(temp[iTemp]); i++)
                    {
                        pnn.Fall_Right_Frames.Add(new Bitmap("Assets/Characters/" + pnn.Name + "/Fall_Right/" + pnn.Name + "_Fall_Right_Frame_" + (i + 1) + ".png"));
                    }
                    iTemp++;
                    for (int i = 0; i < int.Parse(temp[iTemp]); i++)
                    {
                        pnn.Fall_Left_Frames.Add(new Bitmap("Assets/Characters/" + pnn.Name + "/Fall_Left/" + pnn.Name + "_Fall_Left_Frame_" + (i + 1) + ".png"));
                    }
                    iTemp++;
                    pnn.DamageSpeed = int.Parse(temp[iTemp++]);
                    for (int i = 0; i < int.Parse(temp[iTemp]); i++)
                    {
                        pnn.Damage_Right_Frames.Add(new Bitmap("Assets/Characters/" + pnn.Name + "/Damage_Right/" + pnn.Name + "_Damage_Right_Frame_" + (i + 1) + ".png"));
                    }
                    iTemp++;
                    for (int i = 0; i < int.Parse(temp[iTemp]); i++)
                    {
                        pnn.Damage_Left_Frames.Add(new Bitmap("Assets/Characters/" + pnn.Name + "/Damage_Left/" + pnn.Name + "_Damage_Left_Frame_" + (i + 1) + ".png"));
                    }
                    iTemp++;
                    pnn.DieSpeed = int.Parse(temp[iTemp++]);
                    for (int i = 0; i < int.Parse(temp[iTemp]); i++)
                    {
                        pnn.Die_Right_Frames.Add(new Bitmap("Assets/Characters/" + pnn.Name + "/Die_Right/" + pnn.Name + "_Die_Right_Frame_" + (i + 1) + ".png"));
                    }
                    iTemp++;
                    for (int i = 0; i < int.Parse(temp[iTemp]); i++)
                    {
                        pnn.Die_Left_Frames.Add(new Bitmap("Assets/Characters/" + pnn.Name + "/Die_Left/" + pnn.Name + "_Die_Left_Frame_" + (i + 1) + ".png"));
                    }
                    iTemp++;
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

        private void Scrolling()
        {
            int x = Maps[CurrentMap].rSrc.X + Ben.rDst.X;
            if(Ben.BenDirection == "Right")
            {
                for (int i = 0; i + 1 < Maps[CurrentMap].ScrollingFocus.Count; i++)
                {
                    if(i % 2 == 0 && x >= Maps[CurrentMap].ScrollingFocus[i] && x <= Maps[CurrentMap].ScrollingFocus[i + 1])
                    {
                        if (Ben.rDst.X + Ben.CurrentBenImg.Width >= 3 * this.ClientSize.Width / 4)
                        {
                            if(Maps[CurrentMap].rSrc.X + Ben.CurrentSpeed < Maps[CurrentMap].img.Width - this.ClientSize.Width)
                            {
                                Maps[CurrentMap].rSrc.X += Ben.CurrentSpeed;
                                Ben.rDst.X -= Ben.CurrentSpeed;
                            }
                        }
                        return;
                    }
                }
                if (Ben.BenMotion == "Walk" || Ben.BenMotion == "Run" || Ben.BenMotion == "Fly" || Ben.BenMotion == "Jump")
                {
                    if (Ben.rDst.X + Ben.CurrentBenImg.Width >= this.ClientSize.Width / 4 && Maps[CurrentMap].rSrc.X + Ben.CurrentSpeed + Ben.Characters[Ben.Index].StandSpeed < Maps[CurrentMap].img.Width - this.ClientSize.Width)
                    {
                        Maps[CurrentMap].rSrc.X += Ben.CurrentSpeed + Ben.Characters[Ben.Index].StandSpeed;
                        Ben.rDst.X -= Ben.CurrentSpeed + Ben.Characters[Ben.Index].StandSpeed;
                    }
                    else if (Ben.rDst.X + Ben.CurrentBenImg.Width <= 2 * this.ClientSize.Width / 4 && Maps[CurrentMap].rSrc.X + Ben.CurrentSpeed < Maps[CurrentMap].img.Width - this.ClientSize.Width)
                    {
                        Maps[CurrentMap].rSrc.X += Ben.CurrentSpeed;
                        Ben.rDst.X -= Ben.CurrentSpeed;
                    }
                }
                if (Maps[CurrentMap].rSrc.X > Maps[CurrentMap].img.Width - this.ClientSize.Width)
                {
                    Maps[CurrentMap].rSrc.X = Maps[CurrentMap].img.Width - this.ClientSize.Width;
                }
            }
            else if (Ben.BenDirection == "Left")
            {
                for (int i = 0; i + 1 < Maps[CurrentMap].ScrollingFocus.Count; i++)
                {
                    if (i % 2 == 0 && x >= Maps[CurrentMap].ScrollingFocus[i] && x <= Maps[CurrentMap].ScrollingFocus[i + 1])
                    {
                        if (Ben.rDst.X <= this.ClientSize.Width / 4)
                        {
                            if (Maps[CurrentMap].rSrc.X - Ben.CurrentSpeed > 0)
                            {
                                Maps[CurrentMap].rSrc.X -= Ben.CurrentSpeed;
                                Ben.rDst.X += Ben.CurrentSpeed;
                            }
                        }
                        return;
                    }
                }
                if (Ben.BenMotion == "Walk" || Ben.BenMotion == "Run" || Ben.BenMotion == "Fly" || Ben.BenMotion == "Jump")
                {
                    if (Ben.rDst.X + Ben.CurrentBenImg.Width <= 3 * this.ClientSize.Width / 4 && Maps[CurrentMap].rSrc.X - Ben.CurrentSpeed - Ben.Characters[Ben.Index].StandSpeed > 0)
                    {
                        Maps[CurrentMap].rSrc.X -= Ben.CurrentSpeed + Ben.Characters[Ben.Index].StandSpeed;
                        Ben.rDst.X += Ben.CurrentSpeed + Ben.Characters[Ben.Index].StandSpeed;
                    }
                    else if (Ben.rDst.X + Ben.CurrentBenImg.Width <= 3 * this.ClientSize.Width / 4 && Maps[CurrentMap].rSrc.X - Ben.CurrentSpeed > 0)
                    {
                        Maps[CurrentMap].rSrc.X -= Ben.CurrentSpeed;
                        Ben.rDst.X += Ben.CurrentSpeed;
                    }
                }
                if (Maps[CurrentMap].rSrc.X < 0)
                {
                    Maps[CurrentMap].rSrc.X = 0;
                }
            }
        }

        private void BenMotion(string motion, string direction)
        {
            switch (direction)
            {
                case "Right":
                    {
                        switch (motion)
                        {
                            case "Walk":
                                {
                                    if (Ben.rDst.X + Ben.rDst.Width < this.ClientSize.Width)
                                    {
                                        Ben.rDst.X += Ben.Characters[Ben.Index].WalkSpeed;
                                    }
                                    else
                                    {
                                        Ben.rDst.X = this.ClientSize.Width - Ben.rDst.Width;
                                    }
                                    break;
                                }
                            case "Run":
                                {
                                    if (Ben.rDst.X + Ben.rDst.Width < this.ClientSize.Width)
                                    {
                                        Ben.rDst.X += Ben.Characters[Ben.Index].RunSpeed;
                                    }
                                    else
                                    {
                                        Ben.rDst.X = this.ClientSize.Width - Ben.rDst.Width;
                                    }
                                    break;
                                }
                        }
                        break;
                    }
                case "Left":
                    {
                        switch (motion)
                        {
                            case "Walk":
                                {
                                    if (Ben.rDst.X > 0)
                                    {
                                        Ben.rDst.X -= Ben.Characters[Ben.Index].WalkSpeed;
                                    }
                                    else
                                    {
                                        Ben.rDst.X = 0;
                                    }
                                    break;
                                }
                            case "Run":
                                {
                                    if (Ben.rDst.X > 0)
                                    {
                                        Ben.rDst.X -= Ben.Characters[Ben.Index].RunSpeed;
                                    }
                                    else
                                    {
                                        Ben.rDst.X = 0;
                                    }
                                    break;
                                }
                        }
                        break;
                    }
            }
        }

        private void BenMotionImg(string motion, string direction)
        {
            if (Ben.Characters.Count > 0)
            {
                if (Ben.Characters[Ben.Index].CurrentMotion != motion + '_' + direction)
                {
                    Ben.Characters[Ben.Index].CurrentMotion = motion + '_' + direction;
                    Ben.Characters[Ben.Index].CurrentFrame = 0;
                }
                else
                {
                    Ben.Characters[Ben.Index].CurrentFrame++;
                    if (Ben.Characters[Ben.Index].CurrentFrame >= Ben.CurrentFramesCt)
                    {
                        Ben.Characters[Ben.Index].CurrentFrame = 0;
                    }
                }
            }
        }

        private void Gravity()
        {
            if(Ben.rDst.Y + Ben.rDst.Height - Maps[CurrentMap].MarginGravity + Maps[CurrentMap].Ground[0].Height - this.ClientSize.Height < 0 || Ben.rDst.Y + Ben.rDst.Height - Maps[CurrentMap].MarginGravity + Maps[CurrentMap].Ground[0].Height - this.ClientSize.Height > Maps[CurrentMap].Ground[0].Height)
            {
                int i = Maps[CurrentMap].Ground[0].Height - 1;
                while (Maps[CurrentMap].Ground[0].GetPixel(Ben.rDst.X + Ben.rDst.Width / 2 + Maps[CurrentMap].rSrc.X, i).A != 0)
                {
                    i--;
                }
                Ben.rDst.Y = i - (Maps[CurrentMap].Ground[0].Height - this.ClientSize.Height) + Maps[CurrentMap].MarginGravity - Ben.CurrentBenImg.Height;
                return;
            }
            Color GravityPixel = Maps[CurrentMap].Ground[0].GetPixel(Ben.rDst.X + Ben.rDst.Width / 2 + Maps[CurrentMap].rSrc.X, Ben.rDst.Y + Ben.rDst.Height - Maps[CurrentMap].MarginGravity + Maps[CurrentMap].Ground[0].Height - this.ClientSize.Height);
            if(GravityPixel.A == 0)
            {
                for (int i = 0; GravityPixel.A == 0 && i < Ben.Characters[Ben.Index].FallSpeed; i++)
                {
                    if (Ben.Characters[Ben.Index].CurrentMotion == "Jump_Right" || Ben.Characters[Ben.Index].CurrentMotion == "Jump_Left" || Ben.Characters[Ben.Index].CurrentMotion == "Fly_Right" || Ben.Characters[Ben.Index].CurrentMotion == "Fly_Left")
                    {
                        //if (Ben.rDst.Y + Ben.rDst.Height < this.ClientSize.Height - Maps[CurrentMap].MarginGravity)
                        //{
                        //    Ben.rDst.Y += 5; // Gravity effect
                        //}
                        //else
                        //{
                        //    Ben.rDst.Y = this.ClientSize.Height - Maps[CurrentMap].MarginGravity - Ben.rDst.Height;
                        //    Ben.Characters[Ben.Index].CurrentMotion = "Stand_Right"; // Reset to standing motion
                        //}
                    }
                    else
                    {
                        Ben.rDst.Y++; // Gravity effect
                    }
                    GravityPixel = Maps[CurrentMap].Ground[0].GetPixel(Ben.rDst.X + Ben.rDst.Width / 2 + Maps[CurrentMap].rSrc.X, Ben.rDst.Y + Ben.rDst.Height - Maps[CurrentMap].MarginGravity + Maps[CurrentMap].Ground[0].Height - this.ClientSize.Height);
                }
            }
            else if (Maps[CurrentMap].Ground[0].GetPixel(Ben.rDst.X + Ben.rDst.Width / 2 + Maps[CurrentMap].rSrc.X, Ben.rDst.Y + Ben.rDst.Height - Maps[CurrentMap].MarginGravity + Maps[CurrentMap].Ground[0].Height - this.ClientSize.Height - 1).A != 0)
            {
                int i = Maps[CurrentMap].Ground[0].Height - 1;
                while (Maps[CurrentMap].Ground[0].GetPixel(Ben.rDst.X + Ben.rDst.Width / 2 + Maps[CurrentMap].rSrc.X, i).A != 0)
                {
                    i--;
                }
                Ben.rDst.Y = i - (Maps[CurrentMap].Ground[0].Height - this.ClientSize.Height) + Maps[CurrentMap].MarginGravity - Ben.CurrentBenImg.Height;
            }
        }

        private Bitmap BenImg()
        {
            Bitmap img;
            switch (Ben.Characters[Ben.Index].CurrentMotion)
            {
                case "Stand_Right":
                    img = Ben.Characters[Ben.Index].Stand_Right_Frames[Ben.Characters[Ben.Index].CurrentFrame];
                    Ben.CurrentSpeed = 0;
                    Ben.CurrentFramesCt = Ben.Characters[Ben.Index].Stand_Right_Frames.Count;
                    break;
                case "Stand_Left":
                    img = Ben.Characters[Ben.Index].Stand_Left_Frames[Ben.Characters[Ben.Index].CurrentFrame];
                    Ben.CurrentSpeed = 0;
                    Ben.CurrentFramesCt = Ben.Characters[Ben.Index].Stand_Left_Frames.Count;
                    break;
                case "Walk_Right":
                    img = Ben.Characters[Ben.Index].Walk_Right_Frames[Ben.Characters[Ben.Index].CurrentFrame];
                    Ben.CurrentSpeed = Ben.Characters[Ben.Index].WalkSpeed;
                    Ben.CurrentFramesCt = Ben.Characters[Ben.Index].Walk_Right_Frames.Count;
                    break;
                case "Walk_Left":
                    img = Ben.Characters[Ben.Index].Walk_Left_Frames[Ben.Characters[Ben.Index].CurrentFrame];
                    Ben.CurrentSpeed = Ben.Characters[Ben.Index].WalkSpeed;
                    Ben.CurrentFramesCt = Ben.Characters[Ben.Index].Walk_Left_Frames.Count;
                    break;
                case "Run_Right":
                    img = Ben.Characters[Ben.Index].Run_Right_Frames[Ben.Characters[Ben.Index].CurrentFrame];
                    Ben.CurrentSpeed = Ben.Characters[Ben.Index].RunSpeed;
                    Ben.CurrentFramesCt = Ben.Characters[Ben.Index].Run_Right_Frames.Count;
                    break;
                case "Run_Left":
                    img = Ben.Characters[Ben.Index].Run_Left_Frames[Ben.Characters[Ben.Index].CurrentFrame];
                    Ben.CurrentSpeed = Ben.Characters[Ben.Index].RunSpeed;
                    Ben.CurrentFramesCt = Ben.Characters[Ben.Index].Run_Left_Frames.Count;
                    break;
                case "Jump_Right":
                    img = Ben.Characters[Ben.Index].Jump_Right_Frames[Ben.Characters[Ben.Index].CurrentFrame];
                    break;
                case "Jump_Left":
                    img = Ben.Characters[Ben.Index].Jump_Left_Frames[Ben.Characters[Ben.Index].CurrentFrame];
                    break;
                case "Fly_Right":
                    img = Ben.Characters[Ben.Index].Fly_Right_Frames[Ben.Characters[Ben.Index].CurrentFrame];
                    break;
                case "Fly_Left":
                    img = Ben.Characters[Ben.Index].Fly_Left_Frames[Ben.Characters[Ben.Index].CurrentFrame];
                    break;
                case "Hit_Right":
                    img = Ben.Characters[Ben.Index].Hit_Right_Frames[Ben.Characters[Ben.Index].CurrentFrame];
                    break;
                case "Hit_Left":
                    img = Ben.Characters[Ben.Index].Hit_Left_Frames[Ben.Characters[Ben.Index].CurrentFrame];
                    break;
                case "Kick_Right":
                    img = Ben.Characters[Ben.Index].Kick_Right_Frames[Ben.Characters[Ben.Index].CurrentFrame];
                    break;
                case "Kick_Left":
                    img = Ben.Characters[Ben.Index].Kick_Left_Frames[Ben.Characters[Ben.Index].CurrentFrame];
                    break;
                case "Fall_Right":
                    img = Ben.Characters[Ben.Index].Fall_Right_Frames[Ben.Characters[Ben.Index].CurrentFrame];
                    break;
                case "Fall_Left":
                    img = Ben.Characters[Ben.Index].Fall_Left_Frames[Ben.Characters[Ben.Index].CurrentFrame];
                    break;
                case "Damage_Right":
                    img = Ben.Characters[Ben.Index].Damage_Right_Frames[Ben.Characters[Ben.Index].CurrentFrame];
                    break;
                case "Damage_Left":
                    img = Ben.Characters[Ben.Index].Damage_Left_Frames[Ben.Characters[Ben.Index].CurrentFrame];
                    break;
                case "Die_Right":
                    img = Ben.Characters[Ben.Index].Die_Right_Frames[Ben.Characters[Ben.Index].CurrentFrame];
                    break;
                case "Die_Left":
                    img = Ben.Characters[Ben.Index].Die_Left_Frames[Ben.Characters[Ben.Index].CurrentFrame];
                    break;
                default:
                    img = Ben.Characters[Ben.Index].Stand_Right_Frames[Ben.Characters[Ben.Index].CurrentFrame];
                    Ben.CurrentSpeed = 0;
                    Ben.CurrentFramesCt = Ben.Characters[Ben.Index].Stand_Right_Frames.Count;
                    break;
            }
            return img;
        }

        private void ManageBenRectanglesTheme()
        {
            if (Ben.Characters.Count > 0)
            {
                Ben.rDst.Width = Ben.CurrentBenImg.Width;
                Ben.rDst.Height = Ben.CurrentBenImg.Height;
                Ben.rSrc.Width = Ben.CurrentBenImg.Width;
                Ben.rSrc.Height = Ben.CurrentBenImg.Height;
            }
        }

        private void DrawMap(Map map, int W, int H)
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

        private void DrawSettingPage()
        {
            int w = this.ClientSize.Width;
            int h = this.ClientSize.Height;
            Graphics g = this.CreateGraphics();
            switch (Difficulty)
            {
                case "Easy":
                    {
                        if (CurrentMenu == 2 || CurrentMenu == -2)
                        {
                            g.DrawImage(new Bitmap("Assets/Menu/Setting_Page_E_Hover.png"), 0, 0, this.ClientSize.Width, this.ClientSize.Height);
                        }
                        else if (CurrentMenu == 22)
                        {
                            DrawDubb();
                            g.DrawImage(new Bitmap("Assets/Menu/Setting_NoBg_E_Hover.png"), 0, 0, this.ClientSize.Width, this.ClientSize.Height);
                        }
                        GameTimer.Interval = 100;
                        break;
                    }
                case "Medium":
                    {
                        if (CurrentMenu == 2 || CurrentMenu == -2)
                        {
                            g.DrawImage(new Bitmap("Assets/Menu/Setting_Page_M_Hover.png"), 0, 0, this.ClientSize.Width, this.ClientSize.Height);
                        }
                        else if (CurrentMenu == 22)
                        {
                            DrawDubb();
                            g.DrawImage(new Bitmap("Assets/Menu/Setting_NoBg_M_Hover.png"), 0, 0, this.ClientSize.Width, this.ClientSize.Height);
                        }
                        GameTimer.Interval = 75;
                        break;
                    }
                case "Hard":
                    {
                        if (CurrentMenu == 2 || CurrentMenu == -2)
                        {
                            g.DrawImage(new Bitmap("Assets/Menu/Setting_Page_H_Hover.png"), 0, 0, this.ClientSize.Width, this.ClientSize.Height);
                        }
                        else if (CurrentMenu == 22)
                        {
                            DrawDubb();
                            g.DrawImage(new Bitmap("Assets/Menu/Setting_NoBg_H_Hover.png"), 0, 0, this.ClientSize.Width, this.ClientSize.Height);
                        }
                        GameTimer.Interval = 50;
                        break;
                    }
            }
            if(selectReader != null)
            {
                if (selectReader.Volume <= 0.0f)
                {
                    selectReader.Volume = 0.0f;
                    g.DrawImage(new Bitmap("Assets/Menu/Setting_Page_Mute_Selected.png"), 0, 0, this.ClientSize.Width, this.ClientSize.Height);
                }
                else if (selectReader.Volume > 1.0f)
                {
                    selectReader.Volume = 1.0f;
                }
                else
                {
                    int startX = 895 * w / 1920;
                    int Y = 543 * h / 1080;
                    for (float i = 0.0f; i < selectReader.Volume; i += 0.1f)
                    {
                        g.DrawImage(new Bitmap("Assets/Menu/Sound_Button.png"), startX, Y, 25 * w / 1920, 30 * h / 1080);
                        startX += (5 * w / 1920) + (25 * w / 1920);
                    }
                }
            }
        }

        private void DrawDubb()
        {
            Graphics g2 = Graphics.FromImage(off);
            Graphics g = this.CreateGraphics();
            DrawScene(g2);
            g.DrawImage(off, 0, 0);
        }

        private void DrawScene(Graphics g)
        {
            // Draw Map
            g.DrawImage(Maps[CurrentMap].img, Maps[CurrentMap].rDst, Maps[CurrentMap].rSrc, GraphicsUnit.Pixel);
            // Draw Ben10 Character
            g.DrawImage(Ben.CurrentBenImg, Ben.rDst, Ben.rSrc, GraphicsUnit.Pixel);
            // Scrolling Draft
            for(int i = 0; i < Maps[CurrentMap].ScrollingFocus.Count; i++)
            {
                if(Maps[CurrentMap].ScrollingFocus[i] - Maps[CurrentMap].rSrc.X >= 0)
                {
                    if (i % 2 == 0)
                    {
                        g.DrawLine(Pens.Green, Maps[CurrentMap].ScrollingFocus[i] - Maps[CurrentMap].rSrc.X, 0, Maps[CurrentMap].ScrollingFocus[i] - Maps[CurrentMap].rSrc.X, this.ClientSize.Height);
                    }
                    else
                    {
                        g.DrawLine(Pens.Red, Maps[CurrentMap].ScrollingFocus[i] - Maps[CurrentMap].rSrc.X, 0, Maps[CurrentMap].ScrollingFocus[i] - Maps[CurrentMap].rSrc.X, this.ClientSize.Height);
                    }
                }
            }
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

        private void PlaySound(string sound)
        {
            switch (sound)
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
            selectReader.Volume = SoundVolume;
            selectOutput = new WaveOutEvent();
            selectOutput.Init(selectReader);
            selectOutput.Play();
        }
    }
}