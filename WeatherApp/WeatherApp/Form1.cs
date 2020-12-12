using System;
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
        //BindingList<string> datas = new BindingList<string>();
        BindingList<City> cities = new BindingList<City>();
        decimal lat;
        decimal lng;
        WeatherWebReference.unitType unit;
        //WeatherWebReference.weatherParametersType parameters;

        public Form1()
        {
            InitializeComponent();

            comboBox1.SelectedItem = "Celsius";
            GetCities();
            FillCitiesSource();
            listBox1.DisplayMember = "varos";
            GetLatLng();
        }

        private void GetDatas()
        {
            var myweather = new WeatherWebReference.ndfdXML();

            //kinyeréshez szükséges paraméterek
            /*DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;*/

            if ((string)comboBox1.SelectedItem == "Celsius")
            {
                unit = WeatherWebReference.unitType.m;
            }
            else
            {
                unit = WeatherWebReference.unitType.e;
            }

            if (checkedListBox1.CheckedItems.Contains("Jelenlegi hőmérséklet"))
            {
                var parameters = new WeatherWebReference.weatherParametersType();
                {
                    parameters.temp = true;
                };

                var data = myweather.NDFDgen(lat, lng, WeatherWebReference.productType.timeseries, 
                                             DateTime.Now, DateTime.Now, unit, parameters);

                var xml = new XmlDocument();
                xml.LoadXml(data);

                foreach (XmlElement element in xml.GetElementsByTagName("value"))
                {
                    listBox2.Items.Add("Temperature: " + element.InnerText);
                }
            }

            if (checkedListBox1.CheckedItems.Contains("Napi max. hőmérséklet"))
            {
                var parameters = new WeatherWebReference.weatherParametersType();
                {
                    parameters.maxt = true;
                };

                var data = myweather.NDFDgen(lat, lng, WeatherWebReference.productType.timeseries,
                                             DateTime.Now, DateTime.Now, unit, parameters);
                Console.WriteLine(data); 
                var xml = new XmlDocument();
                xml.LoadXml(data);

                if (data.ToString().Contains("Daily Maximum Temperature"))
                {
                    foreach (XmlElement element in xml.GetElementsByTagName("value"))
                    {
                        listBox2.Items.Add("Max temperature: " + element.InnerText);
                    }
                }
                else
                {
                    MessageBox.Show("A kiválasztott városhoz nem tartozik napi maximum érték");
                }
            }

            if (checkedListBox1.CheckedItems.Contains("Napi min. hőmérséklet"))
            {
                var parameters = new WeatherWebReference.weatherParametersType();
                {
                    parameters.mint = true;
                };

                var data = myweather.NDFDgen(lat, lng, WeatherWebReference.productType.timeseries,
                                             DateTime.Now, DateTime.Now, unit, parameters);

                var xml = new XmlDocument();
                xml.LoadXml(data);

                if (data.ToString().Contains("Daily Minimum Temperature"))
                {
                    foreach (XmlElement element in xml.GetElementsByTagName("value"))
                    {
                        listBox2.Items.Add("Min temperature: " + element.InnerText);
                    }
                }
                else
                {
                    MessageBox.Show("A kiválasztott városhoz nem tartozik napi minimum érték");
                }
            }
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
            listBox2.Items.Clear();
            GetDatas();
        }
    }
}
