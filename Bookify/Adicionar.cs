using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bookify
{
    public partial class Adicionar : Form
    {
        OleDbConnection conn;
        string caminhoFoto = "";
        private string connStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Livros.accdb";

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("O nome não pode estar vazio.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(cbTipo.Text))
            {
                MessageBox.Show("Tipo Invalido.");
                return false;
            }

            return true;
        }
        

        public Adicionar()
        {
            InitializeComponent();
            conn = new OleDbConnection(connStr);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
        }

        private void AbrirLigacao()
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
        }

        private void FecharLigacao()
        {
            if (conn.State != ConnectionState.Closed)
                conn.Close();
        }

        // ─── ADICIONAR CAPA ───────────────────────────────────────────
        private void BtnaddCapa_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Escolhe uma imagem";
                dlg.Filter = "Imagens|*.jpg;*.jpeg;*.png;*.bmp;*.gif";

                if (dlg.ShowDialog() == DialogResult.OK)  // ← só este, apaga o segundo
                {
                    byte[] fotoBytes = File.ReadAllBytes(dlg.FileName);

                    MemoryStream ms = new MemoryStream(fotoBytes);
                    pictureBox1.Image?.Dispose();
                    pictureBox1.Image = Image.FromStream(ms);

                    try
                    {
                        AbrirLigacao();
                        string sql = "INSERT INTO Livros (Capa) VALUES (?)";
                        OleDbCommand cmd = new OleDbCommand(sql, conn);
                        cmd.Parameters.Add("?", OleDbType.Binary).Value = fotoBytes;
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Capa adicionada com sucesso!", "Sucesso",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao guardar: " + ex.Message, "Erro",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        FecharLigacao();
                    }
                }
            }
        }

        // ─── VER CAPA ─────────────────────────────────────────────────
        private void btnVerCapa_Click(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox(
                "Introduz o ID da capa:", "Ver Capa", "1");

            if (!int.TryParse(input, out int id)) return;

            try
            {
                AbrirLigacao();
                string sql = "SELECT Capa FROM Livros WHERE Id = ?";
                OleDbCommand cmd = new OleDbCommand(sql, conn);
                cmd.Parameters.AddWithValue("?", id);
                byte[] bytes = cmd.ExecuteScalar() as byte[];

                if (bytes != null)
                {
                    using (MemoryStream ms = new MemoryStream(bytes))
                        pictureBox1.Image = Image.FromStream(ms);
                }
                else
                {
                    MessageBox.Show("Nenhuma capa encontrada com esse ID.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                FecharLigacao();
            }
        }

        // ─── GUARDAR PRODUTO ──────────────────────────────────────────
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            try
            {
                AbrirLigacao();

                string query = "INSERT INTO Livros " +
                               "(Nome,Tipo) " +
                               "VALUES (?, ?,)";

                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("?", txtNome.Text);
                cmd.Parameters.AddWithValue("?", Convert.ToDouble(cbTipo.Text));
                cmd.Parameters.AddWithValue("?", caminhoFoto);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Livro guardado com sucesso!", "Sucesso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao guardar livro:\n" + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                FecharLigacao();
            }

            txtNome.Clear();
            cbTipo.Text = "";
            caminhoFoto = "";
            pictureBox1.Image = null;
            txtNome.Focus();
        }
    }
}