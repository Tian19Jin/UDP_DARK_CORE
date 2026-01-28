using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UDP_CLIENT
{
    public partial class frmClient : Form
    {
        public frmClient()
        {
            InitializeComponent();
            dtg.ColumnCount = 2;
            dtg.Columns[0].Name = "Valor Y";
            dtg.Columns[1].Name = "Valor X";
        }
        private UdpClient udpClient;
        private IPEndPoint remoteEndPoint;

        int y = 1;
        int max = 20;

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (y > max)
            {
                timer1.Stop();
                udpClient.Close();
                btnStart.Enabled = true;
                MessageBox.Show("Se han enviado los 20 datos.");
            }

            int x = (2 * y) + 1;

            string mensaje = $"{y},{x}";
            byte[] data = Encoding.UTF8.GetBytes(mensaje);

            try
            {
                // 3. Enviar datos
                udpClient.Send(data, data.Length, remoteEndPoint);


                dtg.Rows.Add(y, x);

            }
            catch (Exception ex)
            {
                timer1.Stop();
                MessageBox.Show("Error al enviar: " + ex.Message);
            }

            y++;
        }
    
        private void btnStart_Click(object sender, EventArgs e)
        {
            string ip = txtIP.Text;
            int port = int.Parse(txtPPort.Text);

            try
            {
                udpClient = new UdpClient();
                remoteEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                y = 1;
                dtg.Rows.Clear(); 

                btnStart.Enabled = false;
                timer1.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
