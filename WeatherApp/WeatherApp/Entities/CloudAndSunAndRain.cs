using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Abstractions;

namespace WeatherApp.Entities
{
    class CloudAndSunAndRain : WeathersGraphs
    {
        protected override void DrawImage(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.Yellow), 115, 45, 50, 50);
            for (int i = 0; i <= 360; i = i + 15)
            {
                g.DrawLine(new Pen(Color.Yellow, 3), 140, 70, Convert.ToInt32(35 * Math.Cos(Math.PI * i / 180) + 140),
                    Convert.ToInt32(35 * Math.Sin(Math.PI * i / 180)) + 70);
            }

            Image imageCloud = Image.FromFile("Images/cloud.png");
            g.DrawImage(imageCloud, new Rectangle(40, 50, 125, 65));

            Image imageRain = Image.FromFile("Images/rain.png");
            g.DrawImage(imageRain, new Rectangle(65, 115, 75, 50));
        }
    }
}
