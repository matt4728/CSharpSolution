using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Data.SqlClient;
using System.Drawing.Drawing2D;
using System.Resources;
using System.Drawing.Imaging;

namespace BookTickets
{
    public partial class Form5 : BookTickets.Form1
    {
        static SqlCommand command = new SqlCommand("use Session2DB", conn);
        static SqlDataReader reader;
        int PictureBox_No = 1;

        public Form5()
        {
            InitializeComponent();
            this.Shown += this.Form_Shown;
            axWindowsMediaPlayer1.Update();
        }


        private void Form_Shown(Object sender, EventArgs e)
        {
            command.CommandText = "SELECT * FROM movie";
            using (reader = command.ExecuteReader())
            {
                int count = 0;

                while (reader.Read())
                {
                    byte[] bImage = null;
                    bImage = (byte[])reader[4];

                    PictureBox pictureBox = (PictureBox)Controls.Find("Picturebox" + (count + 1), true)[0];
                    pictureBox.Image = Image.FromStream(new MemoryStream(bImage));
                    count++;
                }
            }

            pictureBox1.Image = SoftEdgeImage(pictureBox1.Image);
            pictureBox2.Image = SoftEdgeImage(pictureBox2.Image);
            pictureBox3.Image = SoftEdgeImage(pictureBox3.Image);
            pictureBox4.Image = SoftEdgeImage(pictureBox4.Image);
            pictureBox5.Image = SoftEdgeImage(pictureBox5.Image);

            Click_Image();
        }

        public void Click_Image()
        {
            command.CommandText = "SELECT * FROM movie";
            using (reader = command.ExecuteReader())
            {
                int count = 0;

                while (reader.Read())
                {
                    if ((count + 1) == PictureBox_No)
                    {
                        textBox1.Text = reader.GetString(3);
                        axWindowsMediaPlayer1.URL = String.Format(@"DataFiles\trailer_{0}.mp4", reader.GetString(2).Trim());
                        axWindowsMediaPlayer1.Refresh();
                        break;
                    }
                    count++;
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            PictureBox_No = 1;
            Click_Image();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            PictureBox_No = 2;
            Click_Image();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            PictureBox_No = 3;
            Click_Image();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            PictureBox_No = 4;
            Click_Image();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            PictureBox_No = 5;
            Click_Image();
        }

        private void Form5_VisibleChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                axWindowsMediaPlayer1.Ctlcontrols.pause();
            }
        }
    }
}
