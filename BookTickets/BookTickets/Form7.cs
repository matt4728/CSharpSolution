using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace BookTickets
{
    public partial class Form7 : BookTickets.Form1
    {
        static SqlCommand command = new SqlCommand("use Session2DB", conn);
        static SqlDataReader reader;

        int r_no = -1;

        public Form7()
        {
            InitializeComponent();
        }

        private void Form7_Shown(object sender, EventArgs e)
        {
            updateInfo();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("예매를 취소하시겠습니까?", "예매 취소", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                command.Parameters.Clear();
                command.CommandText = "DELETE FROM reservation where r_no = @r_no";
                command.Parameters.Add("@r_no", SqlDbType.Int);
                command.Parameters[0].Value = r_no;
                command.ExecuteNonQuery();
                MessageBox.Show("예매가 취소되었습니다.", "예매 취소", MessageBoxButtons.OK, MessageBoxIcon.Information);
                updateInfo();
            }
        }

        public void updateInfo()
        {
            panel3.Visible = true;

            command.Parameters.Clear();
            command.CommandText = "SELECT * FROM reservation inner join movie on reservation.m_no = movie.m_no WHERE u_no = @u_no ORDER BY r_no DESC";

            command.Parameters.Add("@u_no", SqlDbType.Int);
            command.Parameters[0].Value = userNo;

            using (reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    label9.Text = "영화 : " + reader.GetString(7);
                    label10.Text = "날짜 : " + reader.GetDateTime(4).ToString("yyyy-MM-dd") + " (" + getDayOfWeekKor(reader.GetDateTime(4).DayOfWeek) + ")";
                    label11.Text = "상영관 : " + reader.GetInt32(3) + "관";
                    label12.Text = "좌석 : ";
                    label13.Text = "금액 : " + String.Format("{0:#,###}", reader.GetInt32(5));

                    byte[] bImage = (byte[])reader[10];
                    pictureBox1.Image = SoftEdgeImage(Image.FromStream(new MemoryStream(bImage)));
                    r_no = reader.GetInt32(0);
                }
                else
                {
                    panel3.Visible = false;
                }
            }
            if (panel3.Visible == true)
            {
                command.Parameters.Clear();
                command.CommandText = "SELECT * FROM reservation inner join reservationSeat on reservation.r_no = reservationSeat.r_no where reservation.r_no = @r_no and u_no = @u_no";
                command.Parameters.Add("r_no", SqlDbType.Int);
                command.Parameters.Add("u_no", SqlDbType.Int);

                command.Parameters[0].Value = r_no;
                command.Parameters[1].Value = userNo;

                using (reader = command.ExecuteReader())
                {
                    List<string> seatList = new List<string>();

                    while (reader.Read())
                    {
                        seatList.Add(String.Format("{0}{1}", (char)(reader.GetInt32(8) + 63), reader.GetInt32(9)));
                    }

                    label12.Text = "좌석 : " + string.Join(", ", seatList);
                }
            }

            panel4.Visible = true;

            command.Parameters.Clear();
            command.CommandText = "SELECT * FROM reservation inner join movie on reservation.m_no = movie.m_no WHERE u_no = @u_no ORDER BY r_no DESC";

            command.Parameters.Add("@u_no", SqlDbType.Int);
            command.Parameters[0].Value = userNo;


            using (reader = command.ExecuteReader())
            {
                int count = 0;

                while (reader.Read())
                {
                    if (count == 0)
                    {
                        count++;
                        continue;
                    }
                    int reservationNo = reader.GetInt32(0);

                    Label lblSeat;

                    if (count == 1)
                    {
                        pictureBox2.Image = SoftEdgeImage(Image.FromStream(new MemoryStream((byte[])reader[10])));
                        label16.Text = reader.GetString(7);
                        label17.Text = reader.GetDateTime(4).ToString("yyyy-MM-dd") + "(" + getDayOfWeekKor(reader.GetDateTime(4).DayOfWeek) + ")";
                        label18.Text = "상영관 : " + reader.GetInt32(3) + "관";
                        label20.Text = "금액 : " + String.Format("{0:#,###}", reader.GetInt32(5));

                        lblSeat = label19;
                    }
                    else
                    {
                        Panel panel = new Panel();
                        panel.Size = panel5.Size;
                        panel.Location = new Point(panel5.Bounds.X, 120 * (count - 1));

                        PictureBox pictureBox = new PictureBox();
                        pictureBox.SizeMode = pictureBox2.SizeMode;
                        pictureBox.Bounds = pictureBox2.Bounds;
                        pictureBox.Image = SoftEdgeImage(Image.FromStream(new MemoryStream((byte[])reader[10])));
                        panel.Controls.Add(pictureBox);

                        Label lblTitle = new Label();
                        lblTitle.Text = reader.GetString(7);
                        lblTitle.Bounds = label16.Bounds;
                        lblTitle.Font = label16.Font;
                        panel.Controls.Add(lblTitle);

                        Label lblDate = new Label();
                        lblDate.Text = reader.GetDateTime(4).ToString("yyyy-MM-dd") + "(" + getDayOfWeekKor(reader.GetDateTime(4).DayOfWeek) + ")";
                        lblDate.Bounds = label17.Bounds;
                        lblDate.Font = label17.Font;
                        panel.Controls.Add(lblDate);

                        Label lblTheater = new Label();
                        lblTheater.Text = "상영관 : " + reader.GetInt32(3) + "관";
                        lblTheater.Bounds = label18.Bounds;
                        lblTheater.Font = label18.Font;
                        panel.Controls.Add(lblTheater);

                        lblSeat = new Label();
                        lblSeat.Bounds = label19.Bounds;
                        lblSeat.Font = label19.Font;
                        panel.Controls.Add(lblSeat);

                        Label lblPrice = new Label();
                        lblPrice.Text = "금액 : " + String.Format("{0:#,###}", reader.GetInt32(5));
                        lblPrice.Bounds = label20.Bounds;
                        lblPrice.Font = label20.Font;
                        panel.Controls.Add(lblPrice);

                        panel4.Controls.Add(panel);
                    }

                    
                    SqlCommand command2 = new SqlCommand("SELECT * FROM reservation inner join reservationSeat on reservation.r_no = reservationSeat.r_no WHERE u_no = @u_no and reservation.r_no = @r_no", conn);

                    command2.Parameters.Add("@u_no", SqlDbType.Int);
                    command2.Parameters.Add("@r_no", SqlDbType.Int);
                    command2.Parameters[0].Value = userNo;
                    command2.Parameters[1].Value = reservationNo;

                    using (SqlDataReader reader2 = command2.ExecuteReader())
                    {
                        List<string> SeatList = new List<string>();

                        while (reader2.Read())
                        {
                            SeatList.Add("" + (char)(reader2.GetInt32(8) + 63) + reader2.GetInt32(9));
                        }
                        lblSeat.Text = String.Join(", ", SeatList);
                    }

                    count++;
                }
                if (count == 0)
                {
                    panel4.Visible = false;
                }
            }
        }
    }
}
