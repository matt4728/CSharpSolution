using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace MakeBookTicketsDB
{
    class Program
    {
        static SqlConnection conn = new SqlConnection("Server=localhost;INtegrated security=SSPI;database=master");
        static SqlCommand command = new SqlCommand("DROP DATABASE IF EXISTS Session2DB", conn);
        static void Main(string[] args)
        {

            using (conn)
            {
                try
                {
                    conn.Open();
                    execQuery("DROP DATABASE IF EXISTS Session2DB");
                    execQuery("CREATE DATABASE Session2DB");
                    execQuery(@"USE [Session2DB]
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[movie](
	[m_no] [int] IDENTITY(1,1) NOT NULL,
	[m_title] [nchar](30) NULL,
	[m_EngTitle] [nchar](30) NULL,
	[m_description] [nchar](500) NULL,
	[m_image] [image] NULL,
	[m_startDate] [date] NULL,
	[m_endDate] [date] NULL,
	[m_price] [int] NULL,
 CONSTRAINT [PK_movie] PRIMARY KEY CLUSTERED 
(
	[m_no] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
");
                    execQuery(@"USE [Session2DB]
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[user](
	[u_no] [int] IDENTITY(1,1) NOT NULL,
	[u_id] [nchar](12) NULL,
	[u_pw] [nchar](16) NULL,
	[u_name] [nchar](10) NULL,
	[u_birthdate] [date] NULL,
	[u_phone] [nchar](13) NULL,
 CONSTRAINT [PK_user] PRIMARY KEY CLUSTERED 
(
	[u_no] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
");
                    execQuery(@"USE [Session2DB]
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[theater](
	[t_no] [int] IDENTITY(1,1) NOT NULL,
	[t_rowCount] [int] NULL,
	[t_colCount] [int] NULL,
 CONSTRAINT [PK_Theater] PRIMARY KEY CLUSTERED 
(
	[t_no] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
");
                    execQuery(@"USE [Session2DB]
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[reservation](
	[r_no] [int] IDENTITY(1,1) NOT NULL,
	[m_no] [int] NULL,
	[u_no] [int] NULL,
	[t_no] [int] NULL,
	[r_date] [date] NULL,
	[r_price] [int] NULL,
 CONSTRAINT [PK_reservation] PRIMARY KEY CLUSTERED 
(
	[r_no] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[reservation]  WITH CHECK ADD  CONSTRAINT [FK_reservation_movie] FOREIGN KEY([m_no])
REFERENCES [dbo].[movie] ([m_no])
ON UPDATE CASCADE
ON DELETE CASCADE

ALTER TABLE [dbo].[reservation] CHECK CONSTRAINT [FK_reservation_movie]

ALTER TABLE [dbo].[reservation]  WITH CHECK ADD  CONSTRAINT [FK_reservation_theater] FOREIGN KEY([t_no])
REFERENCES [dbo].[theater] ([t_no])
ON UPDATE CASCADE
ON DELETE CASCADE

ALTER TABLE [dbo].[reservation] CHECK CONSTRAINT [FK_reservation_theater]

ALTER TABLE [dbo].[reservation]  WITH CHECK ADD  CONSTRAINT [FK_reservation_user] FOREIGN KEY([u_no])
REFERENCES [dbo].[user] ([u_no])
ON UPDATE CASCADE
ON DELETE CASCADE

ALTER TABLE [dbo].[reservation] CHECK CONSTRAINT [FK_reservation_user]
");
                    execQuery(@"USE [Session2DB]
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[reservationSeat](
	[rs_no] [int] IDENTITY(1,1) NOT NULL,
	[r_no] [int] NULL,
	[rs_row] [int] NULL,
	[rs_col] [int] NULL,
 CONSTRAINT [PK_reservationSeat_1] PRIMARY KEY CLUSTERED 
(
	[rs_no] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[reservationSeat]  WITH CHECK ADD  CONSTRAINT [FK_ReservationSeat_reservation] FOREIGN KEY([r_no])
REFERENCES [dbo].[reservation] ([r_no])
ON UPDATE CASCADE
ON DELETE CASCADE

ALTER TABLE [dbo].[reservationSeat] CHECK CONSTRAINT [FK_ReservationSeat_reservation]
");

                    insertMovieData();
                    insertUser();
                    insertTheater();
                    Console.WriteLine("DB 생성 완료");
                    Console.ReadLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + "\n");
                    Console.WriteLine(ex.StackTrace);
                    Console.ReadLine();
                }

            }
        }

        public static void execQuery(string sql)
        {
            command.CommandText = sql;
            command.ExecuteNonQuery();
        }

        public static void insertMovieData()
        {
            string[] movieName = "caribbean,the_mummy,wonder_woman,galaxy,transformers".Split(',');

            command.CommandText = "INSERT movie (m_title, m_EngTitle, m_description, m_image, m_startDate, m_endDate, m_price) values(@title, @EngTitle, @description, @image, @sDate, @eDate, @price)";

            command.Parameters.Add("@title", SqlDbType.NVarChar);
            command.Parameters.Add("@EngTitle", SqlDbType.NVarChar);
            command.Parameters.Add("@description", SqlDbType.NVarChar);
            command.Parameters.Add("@image", SqlDbType.Image);
            command.Parameters.Add("@sDate", SqlDbType.Date);
            command.Parameters.Add("@eDate", SqlDbType.Date);
            command.Parameters.Add("@price", SqlDbType.Int);

            for (int i = 0; i < movieName.Length; i++)
            {
                command.Parameters[0].Value = File.ReadAllText(String.Format(@"DataFiles\title_{0}.txt", movieName[i]));
                command.Parameters[1].Value = movieName[i];
                command.Parameters[2].Value = File.ReadAllText(String.Format(@"DataFiles\contents_{0}.txt", movieName[i]));
                command.Parameters[3].Value = ResizeImage(new Bitmap(String.Format(@"DataFiles\poster_{0}.jpg", movieName[i])));
                command.Parameters[4].Value = DateTime.Now.AddDays(-14).ToString("yyyy-MM-dd");
                command.Parameters[5].Value = DateTime.Now.AddDays(14).ToString("yyyy-MM-dd");
                command.Parameters[6].Value = 10000;
                command.ExecuteNonQuery();
            }
        }

        public static void insertUser()
        {
            string[] id = "hong.kd\tlee.ss".Split('\t');
            string[] pw = "1234,1234".Split(',');
            string[] name = "홍길동,이순신".Split(',');
            DateTime[] birthdate = { new DateTime(1988, 1, 1), new DateTime(1999, 12, 31) };
            string[] phone = "010-1234-5678,010-3333-4444".Split(',');

            command.Parameters.Clear();
            command.CommandText = "INSERT [user] (u_id, u_pw, u_name, u_birthdate, u_phone) values(@id, @pw, @name, @birthdate, @phone)";

            command.Parameters.Add("@id", SqlDbType.NVarChar);
            command.Parameters.Add("@pw", SqlDbType.NVarChar);
            command.Parameters.Add("@name", SqlDbType.NVarChar);
            command.Parameters.Add("@birthdate", SqlDbType.Date);
            command.Parameters.Add("@phone", SqlDbType.NVarChar);

            for (int i = 0; i < id.Length; i++)
            {
                command.Parameters[0].Value = id[i];
                command.Parameters[1].Value = pw[i];
                command.Parameters[2].Value = name[i];
                command.Parameters[3].Value = birthdate[i].ToString("yyyy-MM-dd");
                command.Parameters[4].Value = phone[i];
                command.ExecuteNonQuery();
            }
        }

        public static void insertTheater()
        {
            command.Parameters.Clear();
            command.CommandText = "INSERT theater (t_rowCount, t_colCount) values(@row, @col)";

            command.Parameters.Add("@row", SqlDbType.Int);
            command.Parameters.Add("@col", SqlDbType.Int);

            for (int i = 0; i < 5; i++)
            {
                command.Parameters[0].Value = 8;
                command.Parameters[1].Value = 10;
                command.ExecuteNonQuery();
            }
        }

        public static byte[] ResizeImage(Bitmap image)
        {
            Bitmap resize = new Bitmap(image, 185, 260);
            MemoryStream stream = new MemoryStream();
            resize.Save(stream, ImageFormat.Jpeg);
            return stream.ToArray();
        }
    }

}

