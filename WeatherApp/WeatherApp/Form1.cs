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

            Designer();
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

                try
                {
                    var data = myweather.NDFDgen(lat, lng, WeatherWebReference.productType.timeseries,
                                             DateTime.Now, DateTime.Now, unit, parameters);

                    var xml = new XmlDocument();
                    xml.LoadXml(data);

                    if (data.ToString().Contains("Temperature"))
                    {
                        foreach (XmlElement element in xml.GetElementsByTagName("value"))
                        {
                            ListViewItem item = new ListViewItem("Aktuális hőmérséklet:");
                            item.SubItems.Add(element.InnerText);
                            listView1.Items.Add(item);
                        }
                    }
                    else
                    {
                        MessageBox.Show("A kiválasztott városhoz nem tartozik aktuális hőmérséklet");
                    }
                }
                catch (Exception e)
                {

                    MessageBox.Show(e.Message);
                }
            }

            if (checkedListBox1.CheckedItems.Contains("Napi max. hőmérséklet"))
            {
                var parameters = new WeatherWebReference.weatherParametersType();
                {
                    parameters.maxt = true;
                };

                try
                {
                    var data = myweather.NDFDgen(lat, lng, WeatherWebReference.productType.timeseries,
                                             DateTime.Now, DateTime.Now, unit, parameters);
                    Console.WriteLine(data);
                    var xml = new XmlDocument();
                    xml.LoadXml(data);

                    if (data.ToString().Contains("Daily Maximum Temperature"))
                    {
                        foreach (XmlElement element in xml.GetElementsByTagName("value"))
                        {
                            ListViewItem item = new ListViewItem("Napi max. hőmérséklet:");
                            item.SubItems.Add(element.InnerText);
                            listView1.Items.Add(item);
                        }
                    }
                    else
                    {
                        MessageBox.Show("A kiválasztott városhoz nem tartozik napi maximum érték");
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }

            if (checkedListBox1.CheckedItems.Contains("Napi min. hőmérséklet"))
            {
                var parameters = new WeatherWebReference.weatherParametersType();
                {
                    parameters.mint = true;
                };

                try
                {
                    var data = myweather.NDFDgen(lat, lng, WeatherWebReference.productType.timeseries,
                                             DateTime.Now, DateTime.Now, unit, parameters);

                    var xml = new XmlDocument();
                    xml.LoadXml(data);

                    if (data.ToString().Contains("Daily Minimum Temperature"))
                    {
                        foreach (XmlElement element in xml.GetElementsByTagName("value"))
                        {
                            ListViewItem item = new ListViewItem("Napi min. hőmérséklet:");
                            item.SubItems.Add(element.InnerText);
                            listView1.Items.Add(item);
                        }
                    }
                    else
                    {
                        MessageBox.Show("A kiválasztott városhoz nem tartozik napi minimum érték");
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }

            if (checkedListBox1.CheckedItems.Contains("Felhőtakaró"))
            {
                var parameters = new WeatherWebReference.weatherParametersType();
                {
                    parameters.sky = true;
                };

                try
                {
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
                                ListViewItem item = new ListViewItem("Felhőtakaró:");
                                item.SubItems.Add("tiszta");
                                listView1.Items.Add(item);
                            }
                            else
                            {
                                if (felhoErtek > 50)
                                {
                                    ListViewItem item = new ListViewItem("Felhőtakaró:");
                                    item.SubItems.Add("felhős");
                                    listView1.Items.Add(item);
                                }
                                else
                                {
                                    ListViewItem item = new ListViewItem("Felhőtakaró:");
                                    item.SubItems.Add("néhol felhős");
                                    listView1.Items.Add(item);
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("A kiválasztott városhoz nem tartozik felhőtakaróra vonatkozó érték");
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }

            if (checkedListBox1.CheckedItems.Contains("Csapadék valószínűsége"))
            {
                var parameters = new WeatherWebReference.weatherParametersType();
                {
                    parameters.pop12 = true;
                };

                try
                {
                    var data = myweather.NDFDgen(lat, lng, WeatherWebReference.productType.timeseries,
                                             DateTime.Now, DateTime.Now, unit, parameters);

                    var xml = new XmlDocument();
                    xml.LoadXml(data);

                    if (data.ToString().Contains("12 Hourly Probability of Precipitation"))
                    {
                        foreach (XmlElement element in xml.GetElementsByTagName("value"))
                        {
                            ListViewItem item = new ListViewItem("Csapadék valószínűsége:");
                            item.SubItems.Add(element.InnerText + "%");
                            listView1.Items.Add(item);
                            csapadekP = Convert.ToInt32(element.InnerText);
                        }
                    }
                    else
                    {
                        MessageBox.Show("A kiválasztott városhoz nem tartozik valószínűség");
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }

            if (checkedListBox1.CheckedItems.Contains("Páratartalom"))
            {
                var parameters = new WeatherWebReference.weatherParametersType();
                {
                    parameters.rh = true;
                };

                try
                {
                    var data = myweather.NDFDgen(lat, lng, WeatherWebReference.productType.timeseries,
                                             DateTime.Now, DateTime.Now, unit, parameters);

                    var xml = new XmlDocument();
                    xml.LoadXml(data);

                    if (data.ToString().Contains("Relative Humidity"))
                    {
                        foreach (XmlElement element in xml.GetElementsByTagName("value"))
                        {
                            ListViewItem item = new ListViewItem("Páratartalom:");
                            item.SubItems.Add(element.InnerText + "%");
                            listView1.Items.Add(item);
                        }
                    }
                    else
                    {
                        MessageBox.Show("A kiválasztott városhoz nem tartozik páratartalomra vonatkozó adat");
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }

            if (checkedListBox1.CheckedItems.Contains("Szélerősség"))
            {
                var parameters = new WeatherWebReference.weatherParametersType();
                {
                    parameters.wspd = true;
                };

                try
                {
                    var data = myweather.NDFDgen(lat, lng, WeatherWebReference.productType.timeseries,
                                             DateTime.Now, DateTime.Now, unit, parameters);

                    var xml = new XmlDocument();
                    xml.LoadXml(data);

                    if (data.ToString().Contains("Wind Speed"))
                    {
                        foreach (XmlElement element in xml.GetElementsByTagName("value"))
                        {
                            ListViewItem item = new ListViewItem("Szélerősség:");
                            item.SubItems.Add(element.InnerText + " m/s");
                            listView1.Items.Add(item);
                        }
                    }
                    else
                    {
                        MessageBox.Show("A kiválasztott városhoz nem tartozik szélerősségre vonatkozó adat");
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        private void GetCities()
        {
            using (StreamReader sr = new StreamReader("Varosok/cities.csv", Encoding.Default))
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
            listBox1.SelectedIndex = -1;

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
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Az adatok listázásához válassz egy várost a listából! \n" +
                    "Előfordulhat, hogy a keresett város nincs benne a listában. \n" +
                    "Kereséskor ügyelj a kis- és nagybetűkre!");
            }
            else
            {
                mainPanel.Controls.Clear();
                listView1.Items.Clear();
                GetDatas();
                GetGraph();
                button2.Visible = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.InitialDirectory = Application.StartupPath;
            sfd.Filter = "Comma Seperated Values (*.csv)|*.csv";
            sfd.DefaultExt = "csv";
            sfd.AddExtension = true;

            if (sfd.ShowDialog() != DialogResult.OK) return;

            using (StreamWriter sw = new StreamWriter(sfd.FileName, false, Encoding.UTF8))
            {
                City c = (City)listBox1.SelectedItem;

                sw.Write(c.varos);
                sw.Write(";");
                sw.Write(DateTime.Now);
                sw.WriteLine();
                sw.Write("Megnevezés;");
                sw.Write("Érték;");
                sw.WriteLine();

                foreach (ListViewItem item in listView1.Items)
                {
                    sw.Write(item.Text);
                    sw.Write(";");
                    for (int i = 1; i < item.SubItems.Count; i++)
                    {
                        sw.Write(item.SubItems[i].Text);
                        sw.Write(";");
                        sw.WriteLine();
                    }
                }
            }
        }

        private void Designer()
        {
            this.Icon = new Icon("Images/icon.ico");
            this.BackColor = Color.FromArgb(193, 209, 219);
            this.ForeColor = Color.FromArgb(48, 71, 94);
            listView1.BackColor = Color.FromArgb(193, 209, 219);
            listView1.BorderStyle = BorderStyle.None;
            listBox1.BackColor = Color.FromArgb(147, 179, 207);
            checkedListBox1.BackColor = Color.FromArgb(193, 209, 219);
            textBox1.BackColor = Color.FromArgb(147, 179, 207);
            comboBox1.BackColor = Color.FromArgb(147, 179, 207);
            button1.BackColor = Color.FromArgb(122, 160, 203);
            button1.FlatAppearance.BorderColor = Color.FromArgb(122, 160, 203);
            button2.BackColor = Color.FromArgb(122, 160, 203);
            button2.FlatAppearance.BorderColor = Color.FromArgb(122, 160, 203);

            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, true);
            }

            listView1.View = View.Details;
            listView1.Columns.Add("Megnevezés", 150);
            listView1.Columns.Add("Érték", 90);
        }
    }
}
