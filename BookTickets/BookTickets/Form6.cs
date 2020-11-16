using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BookTickets
{
    public partial class Form6 : BookTickets.Form1
    {
        static SqlCommand command = new SqlCommand("use Session2DB", conn);
        static SqlDataReader reader;

        Dictionary<int, int[]> dictionary = new Dictionary<int, int[]>();
        int movieNo = -1;
        DateTime date = DateTime.Now.Date.AddDays(-1);
        int theaterNo = -1;


        public Form6()
        {
            InitializeComponent();
            updateInfo();
        }

        private void Form6_Shown(object sender, EventArgs e)
        {
            command.CommandText = "SELECT * FROM movie";
            using (reader = command.ExecuteReader())
            {
                int count = 0;

                while (reader.Read())
                {
                    listBox1.Items.Add(reader.GetString(1));
                    count++;
                }
            }

            command.CommandText = "SELECT * FROM theater";
            using (reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    listBox3.Items.Add(reader.GetInt32(0) + "관");
                }
            }

            DateTime today = DateTime.Now.Date;
            DateTime lastDay = today.AddDays(1 - today.Day).AddMonths(1).AddDays(-1);

            label12.Text = today.ToString("yyyy-MM");
            for (int i = today.Day; i < lastDay.Day; i++)
            {
                string dow = getDayOfWeekKor(today.AddDays(i - today.Day).DayOfWeek);

                listBox2.Items.Add(i + " " + dow);
            }
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index > -1)
            {
                e.DrawBackground();
                Graphics g = e.Graphics;

                g.FillRectangle(new SolidBrush(Color.White), e.Bounds);
                ListBox lb = (ListBox)sender;
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    g.DrawString(lb.Items[e.Index].ToString(), e.Font, new SolidBrush(Color.Red), new PointF(e.Bounds.X, e.Bounds.Y));
                } else
                {
                    g.DrawString(lb.Items[e.Index].ToString(), e.Font, new SolidBrush(Color.Black), new PointF(e.Bounds.X, e.Bounds.Y));
                }
                
                e.DrawFocusRectangle();
            }
        }

        private void label110_MouseClick(object sender, MouseEventArgs e)
        {
            if (listBox2.SelectedIndex == -1 || listBox3.SelectedIndex == -1)
            {
                MessageBox.Show("날짜와 상영관을 선택해주세요", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Label lbl = (Label)sender;

            for (int i = 21; i <= 110; i++)
            {
                if (lbl.Name == "label" + i)
                {
                    if (lbl.BackColor == Color.Black)
                    {
                        MessageBox.Show("해당 자리는 이미 예약되었습니다.", "예약", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (lbl.BackColor == Color.Red)
                    {
                        dictionary.Remove(i);
                        lbl.BackColor = Color.White;
                    } 
                    else
                    {
                        int[] value = new int[2];
                        value[0] = i / 10 - 1;
                        value[1] = i % 10;
                        dictionary.Add(i, value);
                        lbl.BackColor = Color.Red;
                    }
                    
                }
            }
            updateInfo();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            date = DateTime.Now.Date.AddDays(listBox2.SelectedIndex);
            SeatUpdate();
            updateInfo();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                command.Parameters.Clear();
                command.CommandText = "SELECT * FROM MOVIE WHERE m_title = @title";
                command.Parameters.Add("@title", SqlDbType.NVarChar).Value = listBox1.SelectedItem;

                command.Parameters[0].Value = listBox1.SelectedItem;
                using (reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        movieNo = reader.GetInt32(0);
                    }
                }
            }
            updateInfo();
        }

        public void updateInfo()
        {
            label111.Text = "영화 : " + (movieNo != -1 ? listBox1.SelectedItem : "");
            label112.Text = "날짜 : " + (date != DateTime.Now.Date.AddDays(-1) ? date.ToString("yyyy-MM-dd") : "") + " (" + getDayOfWeekKor(date.DayOfWeek) + ")";
            label113.Text = "상영관 : " + (theaterNo != -1 ? listBox3.SelectedItem : "");
            label114.Text = "좌석 : ";

            int count = 0;

            List<string> list = new List<string>();

            foreach (KeyValuePair<int, int[]> items in dictionary)
            {
                list.Add(String.Format("{0}{1}", (char)(items.Value[0] + 63), items.Value[1]));
                
                count++;
            }
            label114.Text += String.Join(", ", list);

            label114.Text += String.Format(" ({0}석)", count);
            label115.Text = "금액 : " + String.Format("{0:#,####}", count * 10000);
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            SeatUpdate();
            updateInfo();
        }

        public void SeatUpdate()
        {
            if (listBox2.SelectedIndex == -1 || listBox3.SelectedIndex == -1)
            {
                return;
            }

            theaterNo = listBox3.SelectedIndex + 1;

            dictionary.Clear();
            for (int i = 21; i <= 110; i++)
            {
                Label lbl = (Label)Controls.Find("label" + i, true).FirstOrDefault();
                lbl.BackColor = Color.White;
            }

            command.Parameters.Clear();
            command.CommandText = "select rs_row, rs_col from reservation inner join reservationSeat on reservation.r_no = reservationSeat.r_no where t_no = @theater and r_date = @date";

            command.Parameters.Add("@theater", SqlDbType.Int);
            command.Parameters.Add("date", SqlDbType.Date);

            command.Parameters[0].Value = theaterNo;
            command.Parameters[1].Value = DateTime.Now.Date.AddDays(listBox2.SelectedIndex);

            using (reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Label lbl = (Label)Controls.Find("label" + ((reader.GetInt32(0) + 1) * 10 + reader.GetInt32(1)), true).FirstOrDefault();
                    lbl.BackColor = Color.Black;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (movieNo == -1 || theaterNo == -1 || date == DateTime.Now.Date.AddDays(-1) ||  dictionary.Count == 0)
            {
                MessageBox.Show("항목을 선택해주세요", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int price = 0;

            command.Parameters.Clear();
            command.CommandText = "SELECT * FROM movie WHERE m_no = @movie";
            command.Parameters.Add("@movie", SqlDbType.Int);

            command.Parameters[0].Value = movieNo;

            using (reader = command.ExecuteReader())
            {
                if (reader.Read()) {
                   price = reader.GetInt32(7);
                }
            }

            command.Parameters.Clear();
            command.CommandText = "INSERT INTO reservation (m_no, u_no, t_no, r_date, r_price) values(@movie, @user, @theater, @date, @price);SELECT SCOPE_IDENTITY();";
            
            command.Parameters.Add("@movie", SqlDbType.Int);
            command.Parameters.Add("@user", SqlDbType.Int);
            command.Parameters.Add("@theater", SqlDbType.Int);
            command.Parameters.Add("@date", SqlDbType.Date);
            command.Parameters.Add("@price", SqlDbType.Int);

            command.Parameters[0].Value = movieNo;
            command.Parameters[1].Value = userNo;
            command.Parameters[2].Value = theaterNo;
            command.Parameters[3].Value = date;
            command.Parameters[4].Value = price * dictionary.Count;

            int id = Convert.ToInt32(command.ExecuteScalar());

            command.Parameters.Clear();
            command.CommandText = "INSERT INTO reservationSeat (r_no, rs_row, rs_col) values(@r_no, @row, @col)";
            command.Parameters.Add("@r_no", SqlDbType.Int);
            command.Parameters.Add("@row", SqlDbType.Int);
            command.Parameters.Add("@col", SqlDbType.Int);

            command.Parameters[0].Value = id; 

            foreach(KeyValuePair<int, int[]> seat in dictionary)
            {
                command.Parameters[1].Value = seat.Value[0];
                command.Parameters[2].Value = seat.Value[1];
                command.ExecuteNonQuery();
            }

            MessageBox.Show("예약이 완료되었습니다.", "예약", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Form7 form7 = new Form7();
            this.Hide();
            form7.Show();
        }
    }
}
