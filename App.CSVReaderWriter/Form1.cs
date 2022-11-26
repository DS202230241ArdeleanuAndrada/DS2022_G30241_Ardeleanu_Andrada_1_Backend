using CsvHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App.CSVReaderWriter
{
    public partial class Form1 : Form
    {
        string csvPath = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Open CSV file",
                Filter = "csv files (*.csv)|*.csv",
                CheckFileExists = true,
                CheckPathExists = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show(openFileDialog.FileName);
                csvPath = openFileDialog.FileName;
            }
        }

        public class Foo
        {
            public string Email { get; set; }
            public string Name { get; set; }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            IEnumerable<Foo> records = null;
            List<Foo> recordsList = null;

            using (var reader = new StreamReader(csvPath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                records = csv.GetRecords<Foo>();
                recordsList = records.ToList();
                MessageBox.Show("Total Records: " + recordsList.Count().ToString());
                foreach (Foo record in recordsList)
                {
                    MessageBox.Show(record.Email);
                }
            }
        }
    }
}
