﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WeatherApp.Entities
{
    class Sun : Label
    {
        public Sun()
        {
            AutoSize = false;
            Width = 200;
            Height = Width;
            Paint += Sun_Paint;
        }

        private void Sun_Paint(object sender, PaintEventArgs e)
        {
            DrawImage(e.Graphics);
        }

        protected void DrawImage(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.Yellow), 50, 50, 100, 100);
            for (int i = 0; i <= 360; i = i + 15)
            {
                g.DrawLine(new Pen(Color.Yellow, 3), 100, 100, Convert.ToInt32(75*Math.Cos(Math.PI * i / 180)+100), 
                    Convert.ToInt32(75*Math.Sin(Math.PI * i /180))+100);
            }
        }
    }
}
