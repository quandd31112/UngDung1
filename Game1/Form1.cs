using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game1
{
    public partial class Form1 : Form
    {
        int randomNumber;
        Random random = new Random();
        int score = 0;
        int lives = 6;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            randomNumber = random.Next(1, 11);
            UpdateScorePanel();
        }
        private void button1_Click(object sender, EventArgs e)
        {
           
        }
        public void ButtonClick(object sender,EventArgs e)
        {
            Button btn = sender as Button;
            int guessedNumber = int.Parse(btn.Text);
            btn.Enabled = false;

            panelIn4.Controls.Clear();
            Label lbl = new Label();
            lbl.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lbl.Dock = DockStyle.Fill;
            lbl.TextAlign = ContentAlignment.MiddleCenter;

            if (guessedNumber == randomNumber)
            {
                score += 10;
                SaveScoreToDatabase(score); // gọi hàm lưu DB

                lbl.Text = $"Chính xác! Số đúng là {randomNumber}";
                panelIn4.BackColor = Color.LightGreen;

                ResetRound(); // bắt đầu vòng mới
            }
            else
            {
                lives--;
                if (lives <= 0)
                {
                    lbl.Text = $"Bạn đã hết mạng!\nĐiểm của bạn: {score}";
                    panelIn4.BackColor = Color.LightCoral;

                    score = 0;
                    lives = 6;
                    ResetRound();
                }
                else if (guessedNumber < randomNumber)
                {
                    panelIn4.BackColor = Color.LightCoral;
                    lbl.Text = $"Sai rồi! Số đúng lớn hơn {guessedNumber}.";
                }
                else
                {
                    panelIn4.BackColor = Color.LightCoral;
                    lbl.Text = $"Sai rồi! Số đúng nhỏ hơn {guessedNumber}.";
                }
            }

            panelIn4.Controls.Add(lbl);
            UpdateScorePanel();
        }
        private void UpdateScorePanel()
        {
            panelScore.Controls.Clear();

            Label lbl = new Label();
            lbl.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lbl.Dock = DockStyle.Fill;
            lbl.TextAlign = ContentAlignment.MiddleCenter;

            string hearts = new string('❤', lives) + new string('♡', 6 - lives);
            lbl.Text = $"Điểm: {score}\nMạng: {hearts}";

            panelScore.Controls.Add(lbl);
        }

        private void ResetRound()
        {
            randomNumber = random.Next(1, 11);

            foreach (Button b in panel1.Controls)
                b.Enabled = true;
        }

        private void SaveScoreToDatabase(int currentScore)
        {
            try
            {
                string connectionString = "Data Source=QUAN\\SQLEXPRESS;Initial Catalog=GameDB;Integrated Security=True";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO PlayerScore (PlayerName, Score, DatePlayed) VALUES (@name, @score, @date)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@name", "Player1");
                    cmd.Parameters.AddWithValue("@score", currentScore);
                    cmd.Parameters.AddWithValue("@date", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu điểm: " + ex.Message);
            }
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }
    }
}
