using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Lab5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            openFileDialog1.Filter = "Image Files(*.BMP;*.JPG;*GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*";
            comboBox1.Items.AddRange(new string[] { "rus", "eng", "chs", "fre", "ger", "kor", "jpn"});
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pictureBox1.Image = new Bitmap(openFileDialog1.FileName);

                }
                catch
                {
                    MessageBox.Show("Невозможно открыть выбранный файл", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string token = "85398df20188957";
            string path = openFileDialog1.FileName;
            string url = $"https://api.ocr.space/parse/image/";
            Console.WriteLine(path);

            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            request.AddHeader("apikey", token);
            request.AddFile("url", path);
            if (comboBox1.Text == "Выбрать язык")
            {
                textBox1.Text = "Не выбран язык";
            }
            else
            {
                request.AddParameter("language", $"{comboBox1.Text}");
            }
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            Console.WriteLine(response.StatusCode);


            JObject j = JObject.Parse(response.Content);
            JToken item = j.SelectToken("$.ParsedResults")[0].SelectToken(".ParsedText");

            textBox1.Text = item.ToString();

        }
    }


}
