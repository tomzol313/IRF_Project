﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        decimal lat;
        decimal lng;

        public Form1()
        {
            InitializeComponent();

            GetCities();
            FillCitiesSource();
            listBox1.DisplayMember = "varos";
            GetLatLng();
        }

        private void GetDatas()
        {
            var myweather = new WeatherWebReference.ndfdXML();

            //kinyeréshez szükséges paraméterek
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;

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
            listBox2.DataSource = datas;
        }

        private void GetCities()
        {
            using (StreamReader sr = new StreamReader("cities.csv", Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    string[] sor = sr.ReadLine().Split(';');
                    City c = new City();

                    c.varos = sor[0];
                    try
                    {
                        c.lat = decimal.Parse(sor[1]);
                        c.lng = decimal.Parse(sor[2]);
                    }
                    catch { }

                    cities.Add(c);
                    Console.WriteLine(c.lat);
                    Console.WriteLine(c.lng);
                }

                listBox1.DataSource = cities.ToList();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            FillCitiesSource();
        }

        private void FillCitiesSource()
        {
            listBox1.DataSource = (from c in cities
                                   where c.varos.Contains(textBox1.Text)
                                   select c).ToList();
        }

        public void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetLatLng();
        }

        private void GetLatLng()
        {
            City city = (City)listBox1.SelectedItem;

            var latitude = (from k in cities
                            where k == city
                            select k.lat).FirstOrDefault();

            var longitude = (from l in cities
                             where l == city
                             select l.lng).FirstOrDefault();

            lat = latitude;
            lng = longitude;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetDatas();
        }
    }
}
