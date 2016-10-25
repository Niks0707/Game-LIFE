using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Life
{
    public partial class Form1 : Form
    {
        Life l;
        public Form1()
        {
            InitializeComponent();
            numericUpDown1.Value = timer1.Interval;
            //Camalon.Drawing.Shapes.Pie pie = new Camalon.Drawing.Shapes.Pie();
            //pie.Size=new Size(100,100);            
            //Graphics g=Graphics.FromHdc(//.FromHwndInternal(this.Handle);
            //g.DrawPolygon(new Pen(Brushes.Brown),new Point[]{
            //    new Point(20,20),
            //    new Point(30,20),
            //    new Point(30,30),
            //    new Point(40,20),
            //    new Point(50,20),
            //});
            //g.DrawPie(new Pen(Brushes.Black), new Rectangle(10, 10, 100, 100), 1, 30);
            //pie.Paint();
        }

        public void New()
        {
            l = new Life(new Point(30, 30),250);
        }

        private void show()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.ColumnCount = l.Size().Y;
            dataGridView1.RowCount = l.Size().X;
            for (int i = 0; i < l.Size().Y; i++)
            {
                dataGridView1.Columns[i].Width = 25;
                for (int j = 0; j < l.Size().X; j++)
                    if (l.a[j][i] == 1)
                    {
                        dataGridView1[i, j].Style.BackColor = Color.Red;
                    }
            }
            
        }

        private void repickToolStripMenuItem_Click(object sender, EventArgs e)
        {
            New();
            show();
        }

        private void nextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            l.Next();
            show();
        }

        private void gLAYDERToolStripMenuItem_Click(object sender, EventArgs e)
        {
            l = new Life(Samples.Glayder);
            show();
        }

        private void rabbitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            l = new Life(Samples.Rabbit);
            show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //if (toolStripMenuItem1.CheckState==CheckState.Checked)
            l.Next();
            //if (toolStripMenuItem3.CheckState==CheckState.Checked)
            //    for (int i = 0; i < 10; i++)
            //        l.Next();
            show();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            timer1.Interval = (int)numericUpDown1.Value;
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }
    }



    class Life
    {
        public int[][] a;
        Point size;
        Random rand = new Random(unchecked((int)(DateTime.Now.Ticks)));
        struct neighbors
        {
            public int xleft, ytop, xright, ydown;
        }

        public Life()
        {
            size.X = rand.Next(5, 30);
            size.Y = rand.Next(5, 30);
            a = new int[size.X][];
            for (int i = 0; i < size.X; i++)
            {
                a[i] = new int[size.Y];
                for (int j = 0; j < size.Y; j++)
                    if (rand.Next(1, 50) == 1)
                        a[i][j] = 1;
            }
        }

        public Life(Point Size)
        {
            size = Size;
            a = new int[size.X][];
            for (int i = 0; i < size.X; i++)
            {
                a[i] = new int[size.Y];
                for (int j = 0; j < size.Y; j++)
                    if (rand.Next(1, 50) == 1)
                        a[i][j] = 1;
            }
        }

        public Life(Point Size, byte frequent)
        {
            size = Size;
            a = new int[size.X][];
            for (int i = 0; i < size.X; i++)
            {
                a[i] = new int[size.Y];
                for (int j = 0; j < size.Y; j++)
                    if (rand.Next(1, 256 - frequent) == 1)
                        a[i][j] = 1;
            }
        }

        public Life(int[][] m)
        {
            size.X = m.Length;
            size.Y = m[0].Length;
            a = m;
        }

        public Point Size()
        {
            return size;
        }

        public int[][] life()
        {
            return a;
        }

        private void Neighbors(out neighbors neighbs, int I, int J)
        {
            neighbs = new neighbors();
            neighbs.xleft = I - 1;
            neighbs.xright = I + 1;
            neighbs.ytop = J - 1;
            neighbs.ydown = J + 1;
            if (I - 1 < 0)
                neighbs.xleft = size.X - 1;
            if (I + 1 > size.X - 1)
                neighbs.xright = 0;
            if (J - 1 < 0)
                neighbs.ytop = size.Y - 1;
            if (J + 1 > size.Y - 1)
                neighbs.ydown = 0;
        }

        public void Next()
        {
            int[][] temp = a;
            int count;
            for (int i = 0; i < size.X; i++)
                for (int j = 0; j < size.Y; j++)
                {
                    neighbors neighbors;
                    Neighbors(out neighbors, i, j);
                    count = 0;
                    if (a[neighbors.xleft][neighbors.ytop] > 0)
                        count++;
                    if (a[i][neighbors.ytop] > 0)
                        count++;
                    if (a[neighbors.xright][neighbors.ytop] > 0)
                        count++;
                    if (a[neighbors.xleft][j] > 0)
                        count++;
                    if (a[neighbors.xright][j] > 0)
                        count++;
                    if (a[neighbors.xleft][neighbors.ydown] > 0)
                        count++;
                    if (a[i][neighbors.ydown] > 0)
                        count++;
                    if (a[neighbors.xright][neighbors.ydown] > 0)
                        count++;
                    if (count < 2 || count > 3)
                        temp[i][j] = 0;
                    if (count == 3 && a[i][j] == 0)
                        temp[i][j] = 1;
                }
            a = temp;
        }
    }

    static class Samples
    {
        public static readonly int[][] Glayder = new int[][]
            {
                new int[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                new int[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                new int[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                new int[]{0,0,0,1,1,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                new int[]{0,0,0,1,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                new int[]{0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0},
                new int[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0},
                new int[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                new int[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                new int[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                new int[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                new int[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                new int[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                new int[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}};
        
        public static readonly int[][] Rabbit = new int[][]
            {        
                new int[]{0,1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0},
                new int[]{0,1,1,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0},
                new int[]{0,1,1,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0},
                new int[]{0,0,1,1,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0},
                new int[]{0,0,1,1,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0},
                new int[]{0,0,0,1,1,1,1,0,0,0,1,1,1,1,1,1,0,0,0,0},
                new int[]{0,0,1,1,1,1,0,0,0,1,1,1,1,1,1,1,1,0,0,0},
                new int[]{0,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,0,0},
                new int[]{1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                new int[]{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                new int[]{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                new int[]{0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                new int[]{0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                new int[]{0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                new int[]{0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                new int[]{0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                new int[]{0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                new int[]{0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                new int[]{0,0,0,0,0,0,1,1,1,1,1,0,1,1,1,1,1,0,0,0},
                new int[]{0,0,0,0,0,1,1,1,1,0,0,1,1,1,1,1,0,0,0,0}};

    }

    /*
    class Rectangles
    {
        Rectangle[,] rect;
        int width = 0;
        int height = 0;
        int i = 0;
        int j = 0;
        int N;
        int M;
        int zoom;
        int k = 0;
        int l = 0;

        public Rectangles(int n, int m)
        {
            N = n;
            M = m;
            rect = new Rectangle[n, m];
        }

        public void DrawRectangle(int w, int h, Color color, int beginX, int beginY, int size)
        {
            width = w / M;
            height = h / N;
            if (height > width)
                zoom = width;
            else
                zoom = height;
            if (j == M)
            {
                i++;
                if (i == N)
                    i = 0;
                j = 0;
                l = 0;
            }
            if (i == 0 && j == 0)
            {
                k = 0;
                l = 0;
            }
            for (int I = size; I < N; I += 5)
                if (i == I && j == 0)
                    k += 3;
            for (int J = 0; J < M; J++)
                if (j == J * 5 + size)
                {
                    l += 3;
                }
            rect[i, j] = new Rectangle(beginX + j * zoom + 10 + l,
                beginY + i * zoom + 10 + k, zoom - 3, zoom - 3);
        }

        public Rectangle Rectan(int I, int J)
        {
            return rect[I, J];
        }
    }
     */
}

