using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Data.SqlClient;
using System.IO;
using System.Diagnostics;

namespace BookTickets
{
    public partial class Form2 : BookTickets.Form1
    {

        static SqlCommand command = new SqlCommand("use Session2DB", conn);
        static SqlDataReader reader;
        static object Lock = new object();

        int imageNum = -1;

        Rectangle leftBtn = new Rectangle(250, 280, 70, 70);
        Rectangle rightBtn = new Rectangle(650, 280, 70, 70);
        
        public Form2()
        {
            this.MouseDown += new MouseEventHandler(this.Form_MouseDown);
            this.Shown += new EventHandler(this.Form_Shown);
            InitializeComponent();
        }


        public void ThreadProc()
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
                command.ExecuteNonQuery();
            }
            
            while (true)
            {
                ChangeImage(true);
                Thread.Sleep(3000);
            }
        }


        public void ChangeImage(Boolean isNext)
        {
            lock (Lock)
            {
                imageNum = isNext ? ++imageNum : imageNum > 0 ? --imageNum : 4;
                
                command.CommandText = "SELECT * FROM movie ORDER BY m_startDate DESC OFFSET " + (imageNum % 5) + " ROWS FETCH NEXT 5 ROWS ONLY";
                using (reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        byte[] bImage = null;
                        bImage = (byte[])reader[4];
                        pictureBox1.Image = Image.FromStream(new MemoryStream(bImage));
                    }
                }
            }
            
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            base.OnPaint(e);
            e.Graphics.FillEllipse(new SolidBrush(Color.Black), leftBtn);
            e.Graphics.DrawString("<", new Font("Segoe UI", 54, FontStyle.Bold), new SolidBrush(Color.White), leftBtn.X - 5, leftBtn.Y - 20);
            e.Graphics.FillEllipse(new SolidBrush(Color.Black), rightBtn);
            e.Graphics.DrawString(">", new Font("Segoe UI", 54, FontStyle.Bold), new SolidBrush(Color.White), rightBtn.X - 5, rightBtn.Y - 20);
        }


        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            if (leftBtn.Contains(e.Location))
            {
                ChangeImage(false);
            }
            else if (rightBtn.Contains(e.Location))
            {
                ChangeImage(true);
            }
        }

        private void Form_Shown(Object sender, EventArgs e)
        {
            new Thread(new ThreadStart(ThreadProc)).Start();
        }

        private void Form2_VisibleChanged(object sender, EventArgs e)
        {

        }
    }
}
