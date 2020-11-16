using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BookTickets
{
    public partial class Form3 : BookTickets.Form1
    {
        static SqlCommand command = new SqlCommand("use Session2DB", conn);

        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals("") || textBox2.Text.Equals("") || textBox3.Text.Equals("") || textBox4.Text.Equals("") || textBox5.Text.Equals("") || textBox6.Text.Equals(""))
            {
                MessageBox.Show("공란이 존재합니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            } else if (!textBox2.Text.Equals(textBox3.Text))
            {
                MessageBox.Show("비밀번호 확인을 해주세요", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            command.Parameters.Clear();

            command.CommandText = "INSERT [user] (u_id, u_pw, u_name, u_birthdate, u_phone) values(@id, @pw, @name, @birthdate, @phone)";

            command.Parameters.Add("@id", SqlDbType.NVarChar).Value = textBox1.Text;
            command.Parameters.Add("@pw", SqlDbType.NVarChar).Value = textBox2.Text;
            command.Parameters.Add("@name", SqlDbType.NVarChar).Value = textBox4.Text;
            command.Parameters.Add("@birthdate", SqlDbType.NVarChar).Value = textBox5.Text;
            command.Parameters.Add("@phone", SqlDbType.NVarChar).Value = textBox6.Text;

            command.ExecuteNonQuery();

            MessageBox.Show("회원가입이 완료되었습니다.", "회원가입", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Form4 form4 = new Form4();
            this.Hide();
            form4.Show();
        }
    }
}
