using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace WeatherApp
{
    public partial class Form1 : Form
    {
        BindingList<string> datas = new BindingList<string>();

        public Form1()
        {
            InitializeComponent();

            GetDatas();
        }

        private void GetDatas()
        {
            var myweather = new WeatherServiceReference.ndfdXMLPortTypeClient();

            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;

            decimal lat = 32.7452M;
            decimal lng = -117.1979M;

            var productT = WeatherServiceReference.productType.timeseries;
            var unit = WeatherServiceReference.unitType.m;

            var parameters = new WeatherServiceReference.weatherParametersType();
            {
                parameters.temp = true;
            };

            var data = myweather.NDFDgen(lat, lng, productT,
                startDate, endDate, unit, parameters);

            var xml = new XmlDocument();
            xml.LoadXml(data);

            foreach (XmlElement element in xml.GetElementsByTagName("value"))
            {
                datas.Add(element.InnerText);
            }

            listBox1.DataSource = datas;
        }
    }
}
