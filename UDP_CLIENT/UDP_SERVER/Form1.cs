using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace UDP_SERVER
{
    public partial class Form1 : Form
    {

        private UdpClient udpServer;
        private Thread listenThread;
        private bool listening = false;

        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;


            ChCoordenades.Series.Clear();
            var series = new Series("Coordinates");
            series.ChartType = SeriesChartType.Line;
            ChCoordenades.Series.Add(series);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                int portLocal = int.Parse(txtPort.Text);
                udpServer = new UdpClient(portLocal);
                listening = true;

                listenThread = new Thread(ListenForMessages);
                listenThread.IsBackground = true;
                listenThread.Start();

                MessageBox.Show($"Servidor escoltant al port {portLocal}...");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            listening = false;

            if (udpServer != null)
            {
                udpServer.Close();
                udpServer = null;
            }

            if (listenThread != null)
            {
                listenThread.Join();
                listenThread = null;
            }

            ChCoordenades.Series["Coordinates"].Points.Clear();

            MessageBox.Show("Servidor desconnectat.");
        }

        private void ListenForMessages()
        {
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);

            while (listening)
            {
                try
                {
                    byte[] data = udpServer.Receive(ref remoteEP);
                    string message = Encoding.UTF8.GetString(data);

                    string[] parts = message.Split(',');
                    if (parts.Length == 2 &&
                        int.TryParse(parts[0], out int x) &&
                        int.TryParse(parts[1], out int y))
                    {
                        ChCoordenades.Series["Coordinates"].Points.AddXY(y, x);
                    }
                }
                catch
                {
                }
            }
        }
    }
}
