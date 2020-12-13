﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Abstractions;

namespace WeatherApp.Entities
{
    public class Cloud : WeathersGraphs
    {
        protected override void DrawImage(Graphics g)
        {
            Image imageCloud = Image.FromFile("Images/cloud.png");
            g.DrawImage(imageCloud, new Rectangle(25, 40, 150, 78));
        }
    }
}
