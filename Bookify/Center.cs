using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bookify
{
    public partial class Center : Form
    {
        public Center()
        {
            InitializeComponent();
            this.IsMdiContainer = true;

        }
        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void catalogoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Adicionar f1 = new Adicionar();
            f1.MdiParent = this; 
            f1.Show();


        }
    }
}
