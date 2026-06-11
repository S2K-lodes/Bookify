using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bookify
{
    public partial class Bookify : Form
    {
        public Bookify()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {

            string username = txtNome.Text;
            string password = txtPass.Text;

            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Livros.accdb";

            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT COUNT(*) FROM Utilizadores WHERE UserName=@user AND PassWord=@pass";

                    OleDbCommand cmd = new OleDbCommand(query, conn);
                    cmd.Parameters.AddWithValue("@user", username);
                    cmd.Parameters.AddWithValue("@pass", password);

                    int count = (int)cmd.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show("Login com sucesso!");

                        Center menu = new Center();
                        menu.Show();

                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Usuário ou senha incorretos!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CCt t4 = new CCt();
            t4.Show();
            this.Hide();

        }

        private void Bookify_Load(object sender, EventArgs e)
        {
            int raio = 20;

            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, raio, raio, 180, 90);
            path.AddArc(panel1.Width - raio, 0, raio, raio, 270, 90);
            path.AddArc(panel1.Width - raio, panel1.Height - raio, raio, raio, 0, 90);
            path.AddArc(0, panel1.Height - raio, raio, raio, 90, 90);
            path.CloseFigure();

            panel1.Region = new Region(path);

            Panel sombra = new Panel();
            sombra.Size = panel1.Size;
            sombra.Location = new Point(panel1.Left + 8, panel1.Top + 8);
            sombra.BackColor = Color.Pink;

            this.Controls.Add(sombra);

            sombra.SendToBack();
            panel1.BringToFront();
            /////////////
            int rao = 20;

            GraphicsPath pat = new GraphicsPath();
            pat.AddArc(0, 0, rao, rao, 180, 90);
            pat.AddArc(panel2.Width - rao, 0, rao, rao, 270, 90);
            pat.AddArc(panel2.Width - rao, panel2.Height - rao, rao, rao, 0, 90);
            pat.AddArc(0, panel2.Height - rao, rao, rao, 90, 90);
            pat.CloseFigure();

            panel2.Region = new Region(pat);

            Panel sombr = new Panel();
            sombr.Size = panel2.Size;
            sombr.Location = new Point(panel2.Left + 8, panel2.Top + 8);
            sombr.BackColor = Color.Pink;

            this.Controls.Add(sombr);

            sombr.SendToBack();
            panel2  .BringToFront();

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Criado Por: Simão Barreto");
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Application.Exit();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}