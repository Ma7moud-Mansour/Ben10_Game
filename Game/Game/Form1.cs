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
        Timer tt = new Timer();
        public Form1()
        {
            this.WindowState = FormWindowState.Maximized;
            this.Paint += Form1_Paint;
            this.Load += Form1_Load;
            tt.Tick += Tt_Tick;
            tt.Start();
            InitializeComponent();
        }

        private void Tt_Tick(object sender, EventArgs e)
        {
            DrawDubb(this.CreateGraphics());
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Create
            // Draw
            off = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            tt.Interval = 100;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawDubb(this.CreateGraphics());
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
    }
}
