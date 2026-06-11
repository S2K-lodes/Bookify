using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bookify
{
    public partial class CCt : Form
    {
        string connectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={Path.Combine(Application.StartupPath, "Livros.accdb")}";
        
        public CCt()
        {
            InitializeComponent();
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string user = txtUser.Text.Trim();
            string pass = txtPass.Text.Trim();
            string confirm = txtConfirmPassword.Text.Trim();

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass) || string.IsNullOrEmpty(confirm))
            {
                MessageBox.Show("Preencha todos os campos!");
                return;
            }

            if (pass != confirm)
            {
                MessageBox.Show("As senhas não coincidem!");
                return;
            }

            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    conn.Open();
                    using (OleDbTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            // 1. Verificar se já existe
                            string sqlCheck = "SELECT COUNT(*) FROM Utilizadores WHERE UserName = ?";
                            using (OleDbCommand cmdCheck = new OleDbCommand(sqlCheck, conn, trans))
                            {
                                cmdCheck.Parameters.AddWithValue("?", user);
                                int existente = (int)cmdCheck.ExecuteScalar();
                                if (existente > 0)
                                {
                                    MessageBox.Show("Usuário já existe!");
                                    return;
                                }
                            }

                            // 2. Inserir na INFORegistro
                            string sqlInsert = "INSERT INTO Utilizadores (UserName, [PassWord]) VALUES (?, ?)";
                            using (OleDbCommand cmdInsert = new OleDbCommand(sqlInsert, conn, trans))
                            {
                                cmdInsert.Parameters.AddWithValue("?", user);
                                cmdInsert.Parameters.AddWithValue("?", pass);
                                cmdInsert.ExecuteNonQuery();
                            }

                            // 3. Inserir na INFOLogin
                            string sqlLog = "INSERT INTO Utilizadores (UserName, [PassWord], Ativo) VALUES (?, ?, ?)";
                            using (OleDbCommand cmdLog = new OleDbCommand(sqlLog, conn, trans))
                            {
                                cmdLog.Parameters.AddWithValue("?", user);
                                cmdLog.Parameters.AddWithValue("?", pass);
                                cmdLog.Parameters.AddWithValue("?", true);
                                cmdLog.ExecuteNonQuery();
                            }

                            // FINALIZA A TRANSAÇÃO (APENAS UMA VEZ)
                            trans.Commit();

                            MessageBox.Show("Usuário criado! Agora, complete o seu perfil.", "Sucesso");

                            // ABRE A PRÓXIMA TELA (FORA DOS BLOCOS DE COMANDO)
                            Bookify telaDados = new Bookify();
                            telaDados.Show();
                            this.Hide();
                        }
                        catch (Exception ex)
                        {
                            // Se der erro, tenta fazer o Rollback
                            try { trans.Rollback(); } catch { }
                            MessageBox.Show("Erro ao gravar: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro de conexão: " + ex.Message);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CCt_Load(object sender, EventArgs e)
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
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Bookify f4 = new Bookify();
            f4.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
