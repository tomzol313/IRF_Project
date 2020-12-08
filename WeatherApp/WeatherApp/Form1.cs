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
        BindingList<City> cities = new BindingList<City>();

        public Form1()
        {
            InitializeComponent();

            GetDatas();
        }

        private void GetDatas()
        {
            var myweather = new WeatherWebReference.ndfdXML();

            //kinyeréshez szükséges paraméterek
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;

            decimal lat = 18.2319M;
            decimal lng = -66.0388M;

            var productT = WeatherWebReference.productType.timeseries;
            var unit = WeatherWebReference.unitType.m;

            var parameters = new WeatherWebReference.weatherParametersType();
            {
                parameters.temp = true;
            };

            //releváns adatok kinyerése
            var data = myweather.NDFDgen(lat, lng, productT, startDate, endDate, unit, parameters);

            //adatok megjelenítése
            var xml = new XmlDocument();
            xml.LoadXml(data);

            foreach (XmlElement element in xml.GetElementsByTagName("value"))
            {
                datas.Add(element.InnerText);
            }

            listBox1.DataSource = datas;
        }

        private void GetCities()
        {
            var myweather = new WeatherWebReference.ndfdXML();

            var city = myweather.LatLonListCityNames("1234");


        }
    }
}
