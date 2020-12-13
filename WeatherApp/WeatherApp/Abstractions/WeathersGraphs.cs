using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WeatherApp.Abstractions
{
    public abstract class WeathersGraphs : Label
    {
        public WeathersGraphs()
        {
            AutoSize = false;
            Width = 200;
            Height = Width;
            Paint += WeathersGraphs_Paint;
        }

        private void WeathersGraphs_Paint(object sender, PaintEventArgs e)
        {
            DrawImage(e.Graphics);
        }

        protected abstract void DrawImage(Graphics g);
    }
}
