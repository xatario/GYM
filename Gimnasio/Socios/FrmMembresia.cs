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
    public partial class FrmMembresia : Form
    {
        public int id = 0;
        clsSocio oSocio = new clsSocio();
        Membresias.clsMembresia oMembresia = new Membresias.clsMembresia();
        clsSocioMembresia oSocioMembresia = new clsSocioMembresia();
        public FrmMembresia()
        {
            InitializeComponent();
            btnEditarFecha.Click += btnEditarFecha_Click;
            btnHistorial.Click += btnHistorial_Click;
        }
        public FrmMembresia(int idSocio)
        {
            InitializeComponent();
            this.id = idSocio;
            btnEditarFecha.Click += btnEditarFecha_Click;
            btnHistorial.Click += btnHistorial_Click;
        }

        private void FrmMembresia_Load(object sender, EventArgs e)
        {
            if (oSocio.getDatos(id))
            {
                lblNombre.Text = oSocio.datos.Nombre + " " + oSocio.datos.Paterno + " " + oSocio.datos.Materno;
                lblTelefono.Text = oSocio.datos.Telefono;
                txtObservaciones.Text = oSocio.datos.Observaciones;

                if (oSocio.datos.foto != null)
                {
                    System.IO.MemoryStream stream = new System.IO.MemoryStream(oSocio.datos.foto);
                    Bitmap image = new Bitmap(stream);
                    pbFoto.Image = image;
                }
            }
            else
            {
                MessageBox.Show(oSocio.getError());
            }
            //llenado de combo
            Membresias.clsMembresia.getCboMembresias(cboMembresia);
            if (cboMembresia.Items.Count <= 0)
            {
                MessageBox.Show("No existen tipos de membresias agregadas al sistema, por favor ve al modulo de membresias y agregar una para poder ser asignada a los socios");
                this.Close();
            }

           
            refrescaLista();
            interfaz();
        }

        private void interfaz()
        {
            try
            {
                dgvLista.Columns[0].Visible = false;
                dgvLista.Columns[dgvLista.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                if(dgvLista.Rows.Count>0)
                    dgvLista.CurrentCell = dgvLista[1, 0];

                Utilidades.OperacionesFormulario.quitaOrdenamientoGridView(dgvLista);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de sistema " + ex.Message);
            }

        }


        private void cboMembresia_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboMembresia.Items.Count > 0)
                {
                    id = int.Parse(cboMembresia.SelectedValue.ToString());

                    if (oMembresia.getDatos(id))
                    {
                        lblPrecio.Text = oMembresia.datos.Precio.ToString();
                        lblMeses.Text = oMembresia.datos.meses.ToString();
                        lblHoraInicial.Text = oMembresia.datos.horaInicio.ToString();
                        lblHoraFinal.Text = oMembresia.datos.horaFinal.ToString();
                    }
                }
                //con esto evito la falla al abrir el formulario
            }catch{}

        }

        private void refrescaLista()
        {
            if (!oSocioMembresia.getDatos(dgvLista, oSocio.datos.idSocio))
            {
                MessageBox.Show(oSocioMembresia.getError());
            }
            else
            {
                if (dgvLista.Rows.Count > 0)
                {
                 //   dtpFechaInicio.Enabled = false;
                    dtpFechaInicio.MinDate = DateTime.Parse(Utilidades.OperacionesFormulario.getValorCelda(dgvLista, 2, 0));
                    dtpFechaInicio.Value = DateTime.Parse(Utilidades.OperacionesFormulario.getValorCelda(dgvLista, 2, 0));
                }
                else
                {
                   // dtpFechaInicio.Enabled = true;
                    dtpFechaInicio.MinDate = DateTime.Parse("01/01/1753");
                    dtpFechaInicio.Value = DateTime.Now;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            oSocioMembresia.idMembresia = oMembresia.datos.idMembresia;
            oSocioMembresia.idSocio = oSocio.datos.idSocio;
            oSocioMembresia.Precio = oMembresia.datos.Precio;
            oSocioMembresia.fechaInicioMembresia = dtpFechaInicio.Value;
            oSocioMembresia.idUsuarioLog = Utilidades.clsUsuario.idUsuario;
             oSocioMembresia.idEstado = 1; //activa
            

            if (oSocioMembresia.add())
            {
                refrescaLista();
            }
            else
            {
                MessageBox.Show(oSocioMembresia.getError());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dgvLista.Rows.Count > 0)
            {
                int id = int.Parse(Utilidades.OperacionesFormulario.getValorCelda(dgvLista, 0, 0));
                if (id > 0)
                {
                    if (MessageBox.Show("Estas seguro de eliminar el ultimo registro agregado de membresia", "Confirm delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (oSocioMembresia.changeState(3, id))
                        {
                            refrescaLista();
                        }
                        else
                        {
                            MessageBox.Show("Ocurrio un error " + oSocioMembresia.getError());
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
                MessageBox.Show("No existen membresias para ser eliminadas");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dgvLista.Rows.Count > 0)
            {
                int id= Utilidades.OperacionesFormulario.getId(dgvLista);

               // int id = int.Parse(Utilidades.OperacionesFormulario.getValorCelda(dgvLista, 0, 0));
                if (id > 0)
                {
                    frmMembresiaPagos frm = new frmMembresiaPagos();
                    frm.id = id;
                    frm.ShowDialog();
                    refrescaLista();
                }
                else
                {
                    MessageBox.Show("Debe existir una fila seleccionada");
                }
            }
            else
            {
                MessageBox.Show("No existen membresias para agregar pago");
            }


          
          
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnEditarFecha_Click(object sender, EventArgs e)
        {
            // Verificar que hay una fila seleccionada en el grid
            if (dgvLista.Rows.Count == 0 || dgvLista.CurrentRow == null)
            {
                MessageBox.Show("Selecciona una membresía de la lista.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Obtener idSocioMembresia (columna 0 según tu grid)
            int rowIndex = dgvLista.CurrentRow.Index;
            int idSocioMembresia;
            try
            {
                idSocioMembresia = int.Parse(Utilidades.OperacionesFormulario.getValorCelda(dgvLista, 0, rowIndex));
            }
            catch
            {
                MessageBox.Show("No se pudo obtener el ID de la membresía seleccionada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Obtener la fecha actual de inicio (columna 2 según tu refrescaLista)
            DateTime fechaInicioActual;
            try
            {
                fechaInicioActual = DateTime.Parse(Utilidades.OperacionesFormulario.getValorCelda(dgvLista, 2, rowIndex));
            }
            catch
            {
                fechaInicioActual = DateTime.Now;
            }

            // Mostrar selector de fecha simple en modal
            DateTimePicker picker = new DateTimePicker();
            picker.Format = DateTimePickerFormat.Short;
            picker.Value = fechaInicioActual;

            Form dlg = new Form()
            {
                Width = 300,
                Height = 130,
                Text = "Seleccionar nueva fecha de inicio",
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MinimizeBox = false,
                MaximizeBox = false
            };

            picker.Dock = DockStyle.Top;
            dlg.Controls.Add(picker);

            Button ok = new Button() { Text = "Aceptar", Dock = DockStyle.Bottom, Height = 28 };
            ok.Click += (s, ev) => { dlg.DialogResult = DialogResult.OK; dlg.Close(); };
            dlg.Controls.Add(ok);

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                // -- Aquí está el cambio: combinamos la fecha seleccionada con la hora actual --
                DateTime fechaSeleccionada = picker.Value.Date;                 // sólo la fecha elegida (00:00:00)
                TimeSpan horaActual = DateTime.Now.TimeOfDay;                   // hora del sistema ahora
                DateTime nuevaFechaInicio = fechaSeleccionada + horaActual;    // fecha seleccionada + hora actual

                if (MessageBox.Show($"¿Confirmas cambiar la fecha de inicio a {nuevaFechaInicio:dd/MM/yyyy HH:mm:ss}?\n\nEsto cambiará la fecha de vencimiento calculada por el sistema.",
                                    "Confirmar cambio", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        // Llama al método que actualizará la fecha de inicio en BD
                        bool actualizado = oSocioMembresia.UpdateFechaInicio(idSocioMembresia, nuevaFechaInicio);

                        if (actualizado)
                        {
                            MessageBox.Show("Fecha de inicio actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            refrescaLista();
                        }
                        else
                        {
                            MessageBox.Show("No se pudo actualizar la fecha: " + oSocioMembresia.getError(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al actualizar la fecha: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnHistorial_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(oSocio.datos.clave))
                {
                    MessageBox.Show("El socio no tiene un DNI registrado o no se ha cargado correctamente.",
                                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string dni = oSocio.datos.clave;

                string consulta = @"
            SELECT 
                v.fechaCreacion AS 'FECHA DE INGRESO',
                CASE 
                    WHEN v.estado = 1 THEN 'ACTIVA'
                    WHEN v.estado = 0 THEN 'VENCIDA'
                    ELSE 'DESCONOCIDO'
                END AS 'ESTADO'
            FROM visita v
            INNER JOIN socio s ON s.idSocio = v.idSocio
            WHERE s.clave = @dni
            ORDER BY v.fechaCreacion DESC;";

                using (var conn = conexionbd.ObtenerConexion())
                {
                    MySql.Data.MySqlClient.MySqlCommand comando =
                        new MySql.Data.MySqlClient.MySqlCommand(consulta, conn);

                    comando.Parameters.AddWithValue("@dni", dni);

                    MySql.Data.MySqlClient.MySqlDataAdapter adaptador =
                        new MySql.Data.MySqlClient.MySqlDataAdapter(comando);

                    DataTable tablaVisita = new DataTable();
                    adaptador.Fill(tablaVisita);

                    if (tablaVisita.Rows.Count == 0)
                    {
                        MessageBox.Show($"No se encontraron registros de ingreso para el socio con DNI {dni}.",
                                        "Sin registros", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    Form frmHistorial = new Form()
                    {
                        Text = $"Historial de Ingresos - DNI {dni}",
                        StartPosition = FormStartPosition.CenterParent,
                        Width = 600,
                        Height = 400
                    };

                    DataGridView dgv = new DataGridView()
                    {
                        Dock = DockStyle.Fill,
                        ReadOnly = true,
                        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                        DataSource = tablaVisita
                    };

                    // 🔥 OPCIONAL PERO RECOMENDADO: colorear filas
                    dgv.CellFormatting += (s, ev) =>
                    {
                        if (dgv.Columns[ev.ColumnIndex].Name == "ESTADO")
                        {
                            if (ev.Value != null)
                            {
                                if (ev.Value.ToString() == "ACTIVA")
                                    dgv.Rows[ev.RowIndex].DefaultCellStyle.ForeColor = Color.DarkGreen;

                                if (ev.Value.ToString() == "VENCIDA")
                                    dgv.Rows[ev.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
                            }
                        }
                    };

                    frmHistorial.Controls.Add(dgv);
                    frmHistorial.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener el historial: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
