using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Abstractions;

namespace WeatherApp.Entities
{
    class CloudAndRain : WeathersGraphs
    {
        protected override void DrawImage(Graphics g)
        {
            Image imageCloud = Image.FromFile("Images/cloud.png");
            g.DrawImage(imageCloud, new Rectangle(37, 40, 125, 65));

            Image imageRain = Image.FromFile("Images/rain.png");
            g.DrawImage(imageRain, new Rectangle(58, 105, 83, 55));
        }
    }
}
