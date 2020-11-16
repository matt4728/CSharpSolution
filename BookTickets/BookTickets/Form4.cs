using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace BookTickets
{
    public partial class Form4 : BookTickets.Form1
    {
        static SqlCommand command = new SqlCommand("use Session2DB", conn);
        static SqlDataReader reader;
        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("공백란이 있습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            command.Parameters.Clear();
            command.CommandText = "SELECT * FROM [user] where u_id = '" + textBox1.Text + "' and u_pw = '" + textBox2.Text + "'";

            using (reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    MessageBox.Show("로그인에 성공했습니다.", "로그인 성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    userNo = reader.GetInt32(0);
                    Form2 form2 = new Form2();
                    this.Hide();
                    form2.Show();
                }
                else
                {
                    MessageBox.Show("아이디 혹은 비밀번호를 확인해주세요.", "로그인 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
