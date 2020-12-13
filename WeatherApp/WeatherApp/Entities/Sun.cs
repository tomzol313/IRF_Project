using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeatherApp.Abstractions;

namespace WeatherApp.Entities
{
    public class Sun : WeathersGraphs
    {
        protected override void DrawImage(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.Yellow), 50, 40, 100, 100);
            for (int i = 0; i <= 360; i = i + 15)
            {
                g.DrawLine(new Pen(Color.Yellow, 3), 100, 90, Convert.ToInt32(75*Math.Cos(Math.PI * i / 180)+100), 
                    Convert.ToInt32(75*Math.Sin(Math.PI * i /180))+90);
            }
        }
    }
}
