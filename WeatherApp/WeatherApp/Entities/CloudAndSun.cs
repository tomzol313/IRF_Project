using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Abstractions;

namespace WeatherApp.Entities
{
    public class CloudAndSun : WeathersGraphs
    {
        protected override void DrawImage(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.Yellow), 115, 45, 50, 50);
            for (int i = 0; i <= 360; i = i + 15)
            {
                g.DrawLine(new Pen(Color.Yellow, 3), 140, 70, Convert.ToInt32(35 * Math.Cos(Math.PI * i / 180)+140),
                    Convert.ToInt32(35 * Math.Sin(Math.PI * i / 180))+70);
            }

            Image imageFile = Image.FromFile("Images/cloud.png");
            g.DrawImage(imageFile, new Rectangle(25, 50, 150, 78));
        }
    }
}
