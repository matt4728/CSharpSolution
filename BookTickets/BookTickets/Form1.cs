 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace BookTickets
{
    public partial class Form1 : Form
    {
        // 이거 sa 접속으로 변경해야 함
        // 나는 비밀번호를 1234가 아니라 다른걸로 했는데 까먹어서 못하고 있음
        public static SqlConnection conn = new SqlConnection("Server=localhost;Integrated security=SSPI;database=master;MultipleActiveResultSets=true");
        // 이렇게 해서 여러 폼에서 불러오면 편하긴 한데 가끔 동기 처리가 안되서 충돌이 일어난다.
        // public static SqlCommand command = new SqlCommand("use Session2DB", conn);
        // public static SqlDataReader reader;

        public static int userNo = -1;
        public Form1()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            this.Hide();
            form4.label1.Font = new Font("맑은 고딕", 27);
            form4.Show();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            this.Hide();
            form3.label1.Font = new Font("맑은 고딕", 27);
            form3.Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Form5 form5 = new Form5();
            this.Hide();
            form5.label1.Font = new Font("맑은 고딕", 27);
            form5.Show();
        }

        public static Image SoftEdgeImage(Image image)
        {
            Bitmap bitmap = new Bitmap(image);
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    if (i >= 93 && i <= bitmap.Width - 93)
                    {
                        break;
                    }

                    if (i < 12 || j < 12 || i > bitmap.Width - 13 || j > bitmap.Height - 13)
                    {
                        bitmap.SetPixel(i, j, Color.FromArgb(0, 0, 0, 0));
                    }

                    if (i >= 12 && i < 20 && j > 12 && j < bitmap.Height - 13)
                    {
                        bitmap.SetPixel(i, j, Color.FromArgb(255 - (19 - i) * 32, bitmap.GetPixel(i, j)));
                    }

                    if (j >= 12 && j < 20 && i > 12 && i < bitmap.Width - 13)
                    {
                        bitmap.SetPixel(i, j, Color.FromArgb(255 - (19 - j) * 32, bitmap.GetPixel(i, j)));
                    }

                    if (i <= bitmap.Width - 13 && i > bitmap.Width - 20 && j > 12 && j < bitmap.Height - 13)
                    {
                        bitmap.SetPixel(i, j, Color.FromArgb((bitmap.Width - 13 - i) * 32, bitmap.GetPixel(i, j)));
                    }

                    if (j <= bitmap.Height - 13 && j > bitmap.Height - 20 && i > 12 && i < bitmap.Width - 13)
                    {
                        bitmap.SetPixel(i, j, Color.FromArgb((bitmap.Height - 13 - j) * 32, bitmap.GetPixel(i, j)));
                    }
                }
            }
            bitmap.SetPixel(12, 12, Color.FromArgb(0, 0, 0, 0));
            bitmap.SetPixel(bitmap.Width - 13, 12, Color.FromArgb(0, 0, 0, 0));
            bitmap.SetPixel(12, bitmap.Height - 13, Color.FromArgb(0, 0, 0, 0));
            bitmap.SetPixel(bitmap.Width - 13, bitmap.Height - 13, Color.FromArgb(0, 0, 0, 0));

            return bitmap;
        }

        public static string getDayOfWeekKor(DayOfWeek day_of_week)
        {
            string dow = "";

            switch (day_of_week)
            {
                case DayOfWeek.Sunday:
                    dow = "일";
                    break;
                case DayOfWeek.Monday:
                    dow = "월";
                    break;
                case DayOfWeek.Tuesday:
                    dow = "화";
                    break;
                case DayOfWeek.Wednesday:
                    dow = "수";
                    break;
                case DayOfWeek.Thursday:
                    dow = "목";
                    break;
                case DayOfWeek.Friday:
                    dow = "금";
                    break;
                case DayOfWeek.Saturday:
                    dow = "토";
                    break;
            }
            return dow;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            if (userNo == -1)
            {
                MessageBox.Show("로그인을 먼저 해주세요", "로그인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Form6 form6 = new Form6();
            this.Hide();
            form6.label1.Font = new Font("맑은 고딕", 27);
            form6.Show();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            if (userNo == -1)
            {
                MessageBox.Show("로그인을 먼저 해주세요", "로그인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Form7 form7 = new Form7();
            this.Hide();
            form7.label1.Font = new Font("맑은 고딕", 27);
            form7.Show();
        }
    }
}
