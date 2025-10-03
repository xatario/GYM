using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gimnasio.Socios
{
    public partial class frmMembresiaPagos : Form
    {
        public int id = 0;
        
        clsMembresiaPago oMembresiaPago = new clsMembresiaPago();
        clsSocioMembresia oSocioMembresia = new clsSocioMembresia();
        public frmMembresiaPagos()
        {
            InitializeComponent();
        }

        private void frmMembresiaPagos_Load(object sender, EventArgs e)
        {
            refrescaLista();
            interfaz();
            ActualizaDatosInicio();
        }

        #region HELPERS
        public void ActualizaDatosInicio()
        {
            oSocioMembresia.getDatos(id);
            dsGimnasioTableAdapters.QueriesTableAdapter ta = new dsGimnasioTableAdapters.QueriesTableAdapter();
            decimal? totalPagado = (decimal?)ta.GetTotalPagadoMembresia(id);
            if (totalPagado == null) totalPagado = 0;


            lblFecha.Text = oSocioMembresia.datos.fechaInicioMembresia.ToString("dd-MM-yyyy");
            lblPrecio.Text = "$ "+oSocioMembresia.datos.Precio.ToString();
            lblPagada.Text = oSocioMembresia.datos.estadoMembresia;
            if (oSocioMembresia.datos.estadoMembresia.Equals("Pagada")) lblPagada.ForeColor = Color.Green;
            else lblPagada.ForeColor = Color.Red;
            lblTotalPagado.Text = "$ " + totalPagado;

        }
        private void refrescaLista()
        {
            if (!oMembresiaPago.getDatos(dgvLista, id))
            {
                MessageBox.Show(oMembresiaPago.getError());
            }
            
        }
        private void interfaz()
        {
            try
            {
                dgvLista.Columns[0].Visible = false;
                dgvLista.Columns[dgvLista.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                Utilidades.OperacionesFormulario.quitaOrdenamientoGridView(dgvLista);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de sistema " + ex.Message);
            }

        }

        public void limpiaInput()
        {
            txtFolio.Text = "";
            txtImporte.Text = "";
            txtObservacion.Text = "";
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            oMembresiaPago.idSocioMembresia = id;
            oMembresiaPago.observacion = txtObservacion.Text.Trim();
            try { oMembresiaPago.importe = decimal.Parse(txtImporte.Text.Trim()); }
            catch { MessageBox.Show("Captura un importe valido"); return; }
            oMembresiaPago.idUsuarioLog = Utilidades.clsUsuario.idUsuario;
            try { oMembresiaPago.folio= int.Parse(txtFolio.Text.Trim()); }
            catch { MessageBox.Show("Captura un folio númerico valido"); return; }

            //validamos vengan los datos obligatorios
            if (oMembresiaPago.importe <= 0M) {
                MessageBox.Show("El importe es obligatorio");
                return;
            }


            //validamos que el importe total no supere la membresia
            dsGimnasioTableAdapters.QueriesTableAdapter ta= new dsGimnasioTableAdapters.QueriesTableAdapter();
            decimal? totalPagado = (decimal?)ta.GetTotalPagadoMembresia(id);
            if (totalPagado == null) totalPagado = 0;

            //si el total de pagos es menor o igual al precio de la membresia permitimos agregarlo
            if ((totalPagado + oMembresiaPago.importe) <= oSocioMembresia.datos.Precio)
            {
                if (oMembresiaPago.add())
                {
                    //si el total ya cubre el pago modificamos a pagado
                    if ((totalPagado + oMembresiaPago.importe) == oSocioMembresia.datos.Precio)
                    {
                        oSocioMembresia.changeStateStatePayment("Pagada", id);
                       
                    }

                    ActualizaDatosInicio();
                    refrescaLista();
                    limpiaInput();
                }
                else
                {
                    MessageBox.Show(oMembresiaPago.getError());
                }
            }
            else
            {
                MessageBox.Show("Los pagos no pueden ser mayores a lo que cuesta la membresia");
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dgvLista.Rows.Count > 0)
            {
                int idMembresiaPago = Utilidades.OperacionesFormulario.getId(dgvLista);
                
                if (idMembresiaPago > 0)
                {
                    if (MessageBox.Show("Estas seguro de eliminar el pago de la membresia", "Confirm delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (oMembresiaPago.changeState(3, idMembresiaPago))
                        {
                            //ponemos sin pagar
                            oSocioMembresia.changeStateStatePayment("Sin Pagar", id);
                            ActualizaDatosInicio();
                            refrescaLista();
                        }
                        else
                        {
                            MessageBox.Show("Ocurrio un error " + oMembresiaPago.getError());
                        }
                    }

                }
                else
                {
                    MessageBox.Show("Debe existir una fila seleccionada");
                }
            }
            else
            {
                MessageBox.Show("No existen pagos para ser eliminados");
            }
        }
    }
}
