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
using WeatherApp.Entities;

namespace WeatherApp
{
    public partial class Form1 : Form
    {
        BindingList<City> cities = new BindingList<City>();
        decimal lat;
        decimal lng;
        WeatherWebReference.unitType unit;
        int felhoErtek = -1;
        int csapadekP = -1;

        public Form1()
        {
            InitializeComponent();

            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, true);
            }
            comboBox1.SelectedItem = "Celsius";
            GetCities();
            FillCitiesSource();
            listBox1.DisplayMember = "varos";
            GetLatLng();
        }

        private void GetDatas()
        {
            var myweather = new WeatherWebReference.ndfdXML();

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
                
                if (data.ToString().Contains("Temperature"))
                {
                    foreach (XmlElement element in xml.GetElementsByTagName("value"))
                    {
                        listBox2.Items.Add("Aktuális hőmérséklet: " + element.InnerText);
                    }
                }
                else
                {
                    MessageBox.Show("A kiválasztott városhoz nem tartozik aktuális hőmérséklet");
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
                        listBox2.Items.Add("Napi max. hőmérséklet: " + element.InnerText);
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
                        listBox2.Items.Add("Napi min. hőmérséklet: " + element.InnerText);
                    }
                }
                else
                {
                    MessageBox.Show("A kiválasztott városhoz nem tartozik napi minimum érték");
                }
            }

            if (checkedListBox1.CheckedItems.Contains("Felhőtakaró"))
            {
                var parameters = new WeatherWebReference.weatherParametersType();
                {
                    parameters.sky = true;
                };

                var data = myweather.NDFDgen(lat, lng, WeatherWebReference.productType.timeseries,
                                             DateTime.Now, DateTime.Now, unit, parameters);
                
                var xml = new XmlDocument();
                xml.LoadXml(data);

                if (data.ToString().Contains("Cloud Cover Amount"))
                {
                    foreach (XmlElement element in xml.GetElementsByTagName("value"))
                    {
                        felhoErtek = Convert.ToInt32(element.InnerText);

                        if (felhoErtek <= 25)
                        {
                            listBox2.Items.Add("Felhőtakaró: tiszta");
                        }
                        else
                        {
                            if (felhoErtek > 50)
                            {
                                listBox2.Items.Add("Felhőtakaró: felhős");
                            }
                            else
                            {
                                listBox2.Items.Add("Felhőtakaró: néhol felhős");
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("A kiválasztott városhoz nem tartozik felhőtakaróra vonatkozó érték");
                }
            }

            if (checkedListBox1.CheckedItems.Contains("Csapadék valószínűsége"))
            {
                var parameters = new WeatherWebReference.weatherParametersType();
                {
                    parameters.pop12 = true;
                };

                var data = myweather.NDFDgen(lat, lng, WeatherWebReference.productType.timeseries,
                                             DateTime.Now, DateTime.Now, unit, parameters);
                
                var xml = new XmlDocument();
                xml.LoadXml(data);

                if (data.ToString().Contains("12 Hourly Probability of Precipitation"))
                {
                    foreach (XmlElement element in xml.GetElementsByTagName("value"))
                    {
                        listBox2.Items.Add("Csapadék valószínűsége: " + element.InnerText + "%");
                        csapadekP = Convert.ToInt32(element.InnerText);
                    }
                }
                else
                {
                    MessageBox.Show("A kiválasztott városhoz nem tartozik valószínűség");
                }
            }

            if (checkedListBox1.CheckedItems.Contains("Páratartalom"))
            {
                var parameters = new WeatherWebReference.weatherParametersType();
                {
                    parameters.rh = true;
                };

                var data = myweather.NDFDgen(lat, lng, WeatherWebReference.productType.timeseries,
                                             DateTime.Now, DateTime.Now, unit, parameters);
                
                var xml = new XmlDocument();
                xml.LoadXml(data);

                if (data.ToString().Contains("Relative Humidity"))
                {
                    foreach (XmlElement element in xml.GetElementsByTagName("value"))
                    {
                        listBox2.Items.Add("Páratartalom: " + element.InnerText + "%");
                    }
                }
                else
                {
                    MessageBox.Show("A kiválasztott városhoz nem tartozik páratartalomra vonatkozó adat");
                }
            }

            if (checkedListBox1.CheckedItems.Contains("Szélerősség"))
            {
                var parameters = new WeatherWebReference.weatherParametersType();
                {
                    parameters.wspd = true;
                };

                var data = myweather.NDFDgen(lat, lng, WeatherWebReference.productType.timeseries,
                                             DateTime.Now, DateTime.Now, unit, parameters);
                
                var xml = new XmlDocument();
                xml.LoadXml(data);

                if (data.ToString().Contains("Wind Speed"))
                {
                    foreach (XmlElement element in xml.GetElementsByTagName("value"))
                    {
                        listBox2.Items.Add("Szélerősség: " + element.InnerText + " m/s");
                    }
                }
                else
                {
                    MessageBox.Show("A kiválasztott városhoz nem tartozik szélerősségre vonatkozó adat");
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

        private void GetGraph()
        {
            var s = new Sun();
            var c = new Cloud();
            var cs = new CloudAndSun();
            var cr = new CloudAndRain();
            var csr = new CloudAndSunAndRain();

            if (felhoErtek != -1)
            {
                if (felhoErtek <= 25) { mainPanel.Controls.Add(s); }
                else
                {
                    if (csapadekP != -1)
                    {
                        if (csapadekP < 50 && felhoErtek > 50) { mainPanel.Controls.Add(c); }
                        if (csapadekP < 50 && felhoErtek <= 50 && felhoErtek > 25) { mainPanel.Controls.Add(cs); }
                        if (csapadekP >= 50 && felhoErtek > 50) { mainPanel.Controls.Add(cr); }
                        if (csapadekP >= 50 && felhoErtek <= 50 && felhoErtek > 25) { mainPanel.Controls.Add(csr); }
                    }
                    else
                    {
                        if (felhoErtek > 50) { mainPanel.Controls.Add(c); }
                        if (felhoErtek <= 50 && felhoErtek > 25) { mainPanel.Controls.Add(cs); }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mainPanel.Controls.Clear();
            listBox2.Items.Clear();
            GetDatas();
            GetGraph();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /*SaveFileDialog sfd = new SaveFileDialog();

            sfd.InitialDirectory = Application.StartupPath;
            sfd.Filter = "Comma Seperated Values (*.csv)|*.csv";
            sfd.DefaultExt = "csv";
            sfd.AddExtension = true;

            if (sfd.ShowDialog() != DialogResult.OK) return;

            using (StreamWriter sw = new StreamWriter(sfd.FileName, false, Encoding.UTF8))
            {
                sw.Write(DateTime.Now);
                sw.Write(";");

                foreach (var i in listBox1.Items)
                {
                    foreach (var item in collection)
                    {

                    }
                    sw.Write(i);
                    sw.Write(";");
                    sw.WriteLine();
                }
            }*/
        }
    }
}
