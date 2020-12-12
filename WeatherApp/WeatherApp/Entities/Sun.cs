using System;
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
            Width = 100;
            Height = Width;
            Paint += Sun_Paint;
        }

        private void Sun_Paint(object sender, PaintEventArgs e)
        {
            DrawImage(e.Graphics);
        }

        protected void DrawImage(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.Yellow), 0, 0, Width, Height);
        }
    }
}
