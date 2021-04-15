using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace mSquares
{
    public partial class Form1 : Form
    {
        StreamReader file;
        float[,] scalarfield;
        int[,] binaryfield;
        string[,] grid;
        List<LineMS> il;
        string id;
        int resol;
        int step;
        PointF[] lin;
        PointF[] linV;

        public Form1()
        {
            step = 0;
            resol = 60;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                file = new StreamReader(textBox1.Text);
                scalarfield = new float[int.Parse(file.ReadLine()), int.Parse(file.ReadLine())];
                for (int i = 0; i < scalarfield.GetUpperBound(0) + 1; i++)
                {
                    for (int j = 0; j < scalarfield.GetUpperBound(1) + 1; j++)
                    {
                        scalarfield[i, j] = float.Parse(file.ReadLine());
                    }
                }
                binaryfield = new int[scalarfield.GetUpperBound(0) + 1, scalarfield.GetUpperBound(1) + 1];
                for (int i = 0; i < scalarfield.GetUpperBound(0) + 1; i++)
                {
                    for (int j = 0; j < scalarfield.GetUpperBound(1) + 1; j++)
                    {
                        if (scalarfield[i, j] > 0) binaryfield[i, j] = 1;
                        else binaryfield[i, j] = 0;
                    }
                }
                grid = new string[scalarfield.GetUpperBound(0), scalarfield.GetUpperBound(1)];
                for (int i = 0; i < scalarfield.GetUpperBound(0); i++)
                {
                    for (int j = 0; j < scalarfield.GetUpperBound(1); j++)
                    {
                        grid[i, j] = "";
                        grid[i, j] += binaryfield[i, j];
                        grid[i, j] += binaryfield[i, j + 1];
                        grid[i, j] += binaryfield[i + 1, j + 1];
                        grid[i, j] += binaryfield[i + 1, j];
                    }
                }
                id = "";
                il = new List<LineMS>();
                for (int i = 0; i < grid.GetUpperBound(0)+1; i++)
                {
                    for (int j = 0/*c = 0*/; j < grid.GetUpperBound(1)+1; j++)
                    {
                        id = "";
                        //for (int o = 0; o < 4; o++) if (grid[i, j][o] == 1) c++;
                        grid[i, j] += grid[i, j][0];
                        for (int o = 0; o < 4; o++) if (grid[i, j][o] != grid[i, j][o + 1]) id += o;
                        if (id != "")
                        {
                            if (id.Length > 2)
                            {
                                il.Add(new LineMS(j, i, id[0].ToString() + id[1].ToString(), resol));
                                il.Add(new LineMS(j, i, id[2].ToString() + id[3].ToString(), resol));
                            }
                            //else if (c == 3)
                            //{
                            //    il.Add(new LineMS(j, i, id[0].ToString() + "4", resol));
                            //    il.Add(new LineMS(j, i, id[1].ToString() + "4", resol));
                            //}
                            else il.Add(new LineMS(j, i, id[0].ToString() + id[1].ToString(), resol));
                        }
                    }
                }

                textBox1.Enabled = false;
                textBox1.Visible = false;
                button1.Enabled = false;
                button1.Visible = false;
                label1.Visible = false;

                Height = resol * (scalarfield.GetUpperBound(0) + 2) + 10;
                Width = resol * (scalarfield.GetUpperBound(1) + 2) - 15;

                step = 1;
                BackColor = Color.White;
                //Refresh();

                //последовательный массив точек
                List<int> chs = new List<int>();
                lin = new PointF[il.Count + 1];
                lin[0] = il[0].A;
                lin[1] = il[0].B;
                for (int i = 0; i < lin.Length-1; i++)
                {
                    for (int j = 0; j < il.Count; j++)
                    {
                        try
                        {
                            for (int o = 0; o < chs.Count; o++)
                            {
                                if ((j == chs[o]) && (j < il.Count))
                                {
                                    j++;
                                    o = 0;
                                }
                            }
                            if (i != 0)
                            {
                                if (lin[i] == il[j].A)
                                {
                                    lin[i + 1] = il[j].B;
                                    chs.Add(j);
                                }
                                else if (lin[i] == il[j].B)
                                {
                                    lin[i + 1] = il[j].A;
                                    chs.Add(j);
                                }
                            }
                        }
                        catch { }
                    }
                }
                linV = new PointF[lin.Length];
                lin.CopyTo(linV, 0);
                for (int i = 0; i < lin.Length; i++)
                {
                    linV[i].X = lin[i].X * resol + 15;
                    linV[i].Y = lin[i].Y * resol + 15;
                }
                Refresh();
            }
            catch
            {
                label1.Text = "Что-то пошло не так";
                label1.Visible = true;
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (step > 0)
            {
                for (int i = 0; i <= scalarfield.GetUpperBound(0) + 1; i++) e.Graphics.DrawLine(new Pen(Color.Black), 15, 15 + resol * i, Width - 30, 15 + resol * i);
                for (int i = 0; i <= scalarfield.GetUpperBound(1) + 1; i++) e.Graphics.DrawLine(new Pen(Color.Black), 15 + resol * i, 15, 15 + resol * i, Height - 55);
                for (int i = 0; i < il.Count; i++) e.Graphics.DrawLine(new Pen(Color.MediumVioletRed, 6), il[i].Av, il[i].Bv);
                e.Graphics.DrawLines(new Pen(Color.DodgerBlue, 4), linV);
            }
        }
    }
}
