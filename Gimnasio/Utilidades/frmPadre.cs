using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gimnasio.Utilidades
{
    public partial class frmPadre : Form
    {
        public frmPadre()
        {
            InitializeComponent();
            dgvLista.BorderStyle = BorderStyle.None;
            dgvLista.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);
            dgvLista.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvLista.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dgvLista.DefaultCellStyle.SelectionForeColor = Color.WhiteSmoke;
            dgvLista.BackgroundColor = Color.White;
            dgvLista.ReadOnly = false;

            dgvLista.EnableHeadersVisualStyles = false;
            dgvLista.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvLista.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 25, 72);
            dgvLista.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        }

        private void frmPadre_Load(object sender, EventArgs e)
        {

        }

        private void spContenedor_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void cmdModificar_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Business.Excel.ExporterExcel.Export(dgvLista);
        }
    }
}
