using Newtonsoft.Json;
using Points;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private Point[] points = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCreate_Click(object sender, System.EventArgs e)
        {
            points = new Point[5];

            var rnd = new Random();

            for (int i = 0; i < points.Length; i++)
                points[i] = rnd.Next(3) % 2 == 0 ? new Point() : new Point3D();

            listBox.DataSource = points;
        }

        private void btnSort_Click(object sender, EventArgs e)
        {
            if (points == null)
                return;

            Array.Sort(points);

            listBox.DataSource = null;
            listBox.DataSource = points;

        }

        private void btnSerialize_Click(object sender, EventArgs e)
        {
            var dlg = new SaveFileDialog();
            dlg.Filter = "SOAP|*.soap|XML|*.xml|JSON|*.json|Binary|*.bin";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                using (var fs =
                    new FileStream(dlg.FileName, FileMode.Create, FileAccess.Write))
                {
                    switch (Path.GetExtension(dlg.FileName))
                    {
                        case ".bin":
                            var bf = new BinaryFormatter();
                            bf.Serialize(fs, points);
                            break;
                        case ".xml":
                            var xf = new XmlSerializer(typeof(Point[]), new[] { typeof(Point3D) });
                            xf.Serialize(fs, points);
                            break;
                        case ".json":
                            var jf = new JsonSerializer();
                            using (var w = new StreamWriter(fs))
                                jf.Serialize(w, points);
                            break;


                    }
                }

            }

        }

        private void btnDeserialize_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "SOAP|*.soap|XML|*.xml|JSON|*.json|Binary|*.bin";

            if (dlg.ShowDialog() == DialogResult.OK)
            {

                using (var fs =
                    new FileStream(dlg.FileName, FileMode.Open, FileAccess.Read))
                {
                    switch (Path.GetExtension(dlg.FileName))
                    {
                        case ".bin":
                            var bf = new BinaryFormatter();
                            points = (Point[])bf.Deserialize(fs);
                            break;
                        case ".xml":
                            var xf = new XmlSerializer(typeof(Point[]), new[] { typeof(Point3D) });
                            points = (Point[])xf.Deserialize(fs);
                            break;
                        case ".json":
                            var jf = new JsonSerializer();
                            using (var r = new StreamReader(fs))
                                points = (Point[])jf.Deserialize(r, typeof(Point[]));
                            break;

                    }
                }

                listBox.DataSource = null;
                listBox.DataSource = points;

            }

        }
    }
}
