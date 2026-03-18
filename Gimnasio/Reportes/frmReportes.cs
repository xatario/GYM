using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Gimnasio.Socios;

namespace Gimnasio.Reportes
{
    public partial class frmReportes : Form
    {
        Productos.clsProducto oProducto = new Productos.clsProducto();
        Membresias.clsMembresia oMembresia = new Membresias.clsMembresia();
        Socios.clsMembresiaPago oMembresiaPago = new Socios.clsMembresiaPago();
        Socios.clsSocio oSocio = new Socios.clsSocio();

   
        private DataTable tablaSocio;


        public frmReportes()
        {
            InitializeComponent();

            CargarDatosAutocompletado();

            button16_Click(this, EventArgs.Empty);

            dataGridView2.CellDoubleClick += dataGridView2_CellDoubleClick;


        }


        private void frmReportes_Load(object sender, EventArgs e)
        {
            refrescaListaInventario();
            refrescaListaMembresias();
            refrescaListaSocios();
            refrescaListaRegistro();
            refrescaListaVisitas();
            refrescaListaVentaProductos();
            refrescaListaPagos();
            interfaz();

            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;

            comboBox1.DataSource = ClientesDAL.Obtenerusuarios();
            comboBox1.DisplayMember = "Nombre";
            comboBox1.ValueMember = "Id";

            button17_Click(this, EventArgs.Empty);

        }

        #region metodos refrescadores
        private void refrescaListaInventario()
        {
            if (!oProducto.getDatosRptInventario(dgvListaInventario))
            {
                MessageBox.Show(oProducto.getError());
            }


        }
        private void refrescaListaMembresias()
        {
            DateTime fecha1 = dtpFecha1Membresia.Value;
            DateTime fecha2 = dtpFecha2Membresia.Value;

            if (!oMembresia.getDatosRptMembresias(dgvListaMembresias,fecha1,fecha2))
            {
                MessageBox.Show(oMembresia.getError());
            }

            //sacar el total
            lblTotalMembresia.Text = "$ " + getTotalMembresia().ToString();
        }

        private void refrescaListaSocios()
        {
            if (!oSocio.getDatosRptSocios(dgvListaSocios))
            {
                MessageBox.Show(oSocio.getError());
            }


        }

        private void refrescaListaRegistro()
        {
            DateTime fecha = dtpFecha1Registro.Value;


            if (!oSocio.getDatosRptRegistro(dgvListaRegistro, fecha))
            {
                MessageBox.Show(oSocio.getError());
            }

        }

        private void refrescaListaVisitas()
        {
            DateTime fecha = dtpFecha1Visitas.Value;


            if (!oSocio.getDatosRptVisitas(dgvListaVisitas, fecha))
            {
                MessageBox.Show(oSocio.getError());
            }

            //sacar el total
            lblTotalVisitas.Text = "$ " + getTotalVisitas().ToString();
        }

        private void refrescaListaVentaProductos()
        {
            DateTime fecha1 = dtpFecha1VtaProductos.Value;
            DateTime fecha2 = dtpFecha2VtaProductos.Value;

            if (!oProducto.getDatosRptVentaProductos(dgvListaVentaProductos, fecha1, fecha2))
            {
                MessageBox.Show(oProducto.getError());
            }

            //sacar el total
            lblTotalVentaProductos.Text = "$ " + getTotalVentaProductos().ToString();
        }

        private void refrescaListaPagos()
        {
            DateTime fecha1 = dtpFechaPagos1.Value;
            DateTime fecha2 = dtpFechaPagos2.Value;

            if (!oMembresiaPago.getDatosRptPagos(dgvListaPagos, fecha1, fecha2))
            {
                MessageBox.Show(oMembresia.getError());
            }

            //sacar el total
            lblTotalPagos.Text = "$ " + getTotalPagos().ToString();
        }
        #endregion

        #region interfaces

        private void interfaz()
        {
            this.SuspendLayout();
          
            try
            {
                dgvListaInventario.Columns[0].Visible = false;
                dgvListaInventario.Columns[dgvListaInventario.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de sistema " + ex.Message);
            }

            try
            {
                dgvListaMembresias.Columns[0].Visible = false;
                dgvListaMembresias.Columns[dgvListaInventario.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dtpFecha1Membresia.Value = DateTime.Now.Date;
                dtpFecha2Membresia.Value = DateTime.Now.Date;

                dtpFecha1VtaProductos.Value = DateTime.Now.Date;
                dtpFecha2VtaProductos.Value = DateTime.Now.Date;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de sistema " + ex.Message);
            }

            try
            {
                dgvListaVentaProductos.Columns[0].Visible = false;
                dgvListaVentaProductos.Columns[dgvListaInventario.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dtpFecha1VtaProductos.Value = DateTime.Now.Date;
                dtpFecha2VtaProductos.Value = DateTime.Now.Date;

                dtpFecha1Registro.Value = DateTime.Now.Date;
                dtpFecha1Visitas.Value = DateTime.Now.Date;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de sistema " + ex.Message);
            }

            try
            {
                dgvListaPagos.Columns[0].Visible = false;
                dgvListaPagos.Columns[dgvListaPagos.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dtpFechaPagos1.Value = DateTime.Now.Date;
                dtpFechaPagos2.Value = DateTime.Now.Date;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de sistema " + ex.Message);
            }

            this.ResumeLayout();
        }

 
        #endregion

        #region metodos del formulario
        private decimal getTotalMembresia()
        {
            decimal total = 0;

            foreach (DataGridViewRow dr in dgvListaMembresias.Rows)
            {
                total+=decimal.Parse(dr.Cells[dr.Cells.Count-1].Value.ToString());
            }
            return total;
        }

        private decimal getTotalVisitas()
        {
            decimal total = 0;

            foreach (DataGridViewRow dr in dgvListaVisitas.Rows)
            {
                total += decimal.Parse(dr.Cells[dr.Cells.Count - 1].Value.ToString());
            }
            return total;
        }

        private decimal getTotalVentaProductos()
        {
            decimal total = 0;

            foreach (DataGridViewRow dr in dgvListaVentaProductos.Rows)
            {
                total += decimal.Parse(dr.Cells[dr.Cells.Count - 1].Value.ToString());
            }
            return total;
        }
        private decimal getTotalPagos()
        {
            decimal total = 0;

            foreach (DataGridViewRow dr in dgvListaPagos.Rows)
            {
                total += decimal.Parse(dr.Cells[dr.Cells.Count - 1].Value.ToString());
            }
            return total;
        }
        #endregion

        private void frmReportes_Enter(object sender, EventArgs e)
        {
            refrescaListaInventario();
            refrescaListaMembresias();
            refrescaListaSocios();
            refrescaListaRegistro();
            refrescaListaVisitas();
            refrescaListaVentaProductos();
            refrescaListaPagos();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            refrescaListaMembresias();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            refrescaListaRegistro();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            refrescaListaVisitas();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            refrescaListaVentaProductos();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Business.Excel.ExporterExcel.Export(dgvListaInventario);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Business.Excel.ExporterExcel.Export(dgvListaMembresias);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Business.Excel.ExporterExcel.Export(dgvListaSocios);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Business.Excel.ExporterExcel.Export(dgvListaRegistro);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Business.Excel.ExporterExcel.Export(dgvListaVentaProductos);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Business.Excel.ExporterExcel.Export(dgvListaVisitas);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Business.Excel.ExporterExcel.Export(dgvListaPagos);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            refrescaListaPagos();
        }

        private void splitContainer2_Panel2_Paint(object sender, PaintEventArgs e)
        {
                   }

        private void button13_Click(object sender, EventArgs e)
        {

            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy-MM-dd";

            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "yyyy-MM-dd";

            dataGridView1.DataSource = ClientesDAL.mostrar2(dateTimePicker1.Text, dateTimePicker2.Text,comboBox1.Text);

            MySqlConnection.ClearAllPools();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Business.Excel.ExporterExcel.Export(dataGridView1);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy-MM-dd";

            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "yyyy-MM-dd";

            dataGridView1.DataSource = ClientesDAL.mostrar3(dateTimePicker1.Text, dateTimePicker2.Text);
            MySqlConnection.ClearAllPools();
        }

        private void splitContainer3_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CargarDatosAutocompletado()
        {
            try
            {
                // Realizar la consulta para obtener los datos de la tabla socio
                string consulta = "SELECT idSocio,Nombre,Paterno,Materno,clave FROM socio WHERE idEstado = 1";
                MySqlCommand comando = new MySqlCommand(consulta, conexionbd.ObtenerConexion());
                MySqlDataAdapter adaptador = new MySqlDataAdapter(comando);
                tablaSocio = new DataTable();
                adaptador.Fill(tablaSocio);

                // Configurar el autocompletado del TextBox para que muestre los nombres y claves de socio
                AutoCompleteStringCollection autoCompletado = new AutoCompleteStringCollection();
                foreach (DataRow row in tablaSocio.Rows)
                {
                    autoCompletado.Add(row["Nombre"].ToString());
                    autoCompletado.Add(row["clave"].ToString());
                }

                textBoxBusqueda.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                textBoxBusqueda.AutoCompleteSource = AutoCompleteSource.CustomSource;
                textBoxBusqueda.AutoCompleteCustomSource = autoCompletado;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos de autocompletado: " + ex.Message);
            }
        }

        private void ResaltarFechasEnCalendario()
        {
            if (dataGridView3.Rows.Count > 0 && dataGridView3.Columns.Contains("fechaCreacion"))
            {
                // Obtener todas las fechas de la columna "fechaCreacion" del DataGridView
                List<DateTime> fechasVisita = new List<DateTime>();
                foreach (DataGridViewRow row in dataGridView3.Rows)
                {
                    // Verificar si el valor en la columna "fechaCreacion" es una fecha válida
                    if (row.Cells["fechaCreacion"].Value is DateTime fecha)
                    {
                        fechasVisita.Add(fecha);
                    }
                }

                // Resaltar las fechas en el calendario solo si se encontraron fechas válidas
                if (fechasVisita.Count > 0)
                {
                    monthCalendar1.RemoveAllBoldedDates();
                    foreach (DateTime fecha in fechasVisita)
                    {
                        monthCalendar1.AddBoldedDate(fecha);
                       

                    }
                    
                    monthCalendar1.UpdateBoldedDates();
                    
                }



            }

        }

        private void button16_Click(object sender, EventArgs e)
        {
            string sql3 = "SELECT mensajeVencimiento FROM configuracion WHERE idConfiguracion = 1";
            MySqlCommand comando3 = new MySqlCommand(sql3, conexionbd.ObtenerConexion());
            MySqlDataAdapter DP3 = new MySqlDataAdapter(comando3);
            DataTable dt3 = new DataTable();
            DP3.Fill(dt3);

            // Verificar si se obtuvieron resultados
            if (dt3.Rows.Count > 0)
            {
                // Obtener el valor del campo "mensajeVencimiento"
                string mensajeVencimiento = dt3.Rows[0]["mensajeVencimiento"].ToString();

                // Asignar el valor al TextBox
                textBox1.Text = mensajeVencimiento;
            }
            else
            {
                // No se encontraron resultados
                textBox1.Text = "No se encontraron resultados.";
            }

            int vencimiento = int.Parse(textBox1.Text);

            // 🔹 Agregamos idSocio al final del SELECT
            string sql2 = @"
            SELECT 
              DATE(DATE_ADD(sm.fechaInicioMembresia, INTERVAL m.meses MONTH)) AS `DATE(Vencimiento)`,
              s.Nombre AS NombreSocio,
              s.Paterno,
              s.Materno,
              s.clave,
              m.Nombre AS NombreMembresia,
              GROUP_CONCAT(DISTINCT p.observacion SEPARATOR '; ') AS Observaciones,
              DATE(sm.fechaInicioMembresia) AS `DATE(fechaInicioMembresia)`,
              m.Precio AS Precio,
              IFNULL(SUM(p.importe), 0) AS Importe,
              CASE 
                  WHEN IFNULL(SUM(p.importe),0) >= m.Precio THEN 'Pagado'
                  WHEN IFNULL(SUM(p.importe),0) > 0 AND IFNULL(SUM(p.importe),0) < m.Precio THEN 'Pago Parcial'
                  ELSE 'Pendiente'
              END AS EstadoPago,
              s.idSocio
            FROM sociomembresia sm
            INNER JOIN socio s ON s.idSocio = sm.idSocio
            INNER JOIN membresia m ON m.idMembresia = sm.idMembresia
            LEFT JOIN sociomembresia_pago p ON p.idSocioMembresia = sm.idSocioMembresia
            WHERE sm.idEstado = 1
            GROUP BY sm.idSocioMembresia
            ORDER BY `DATE(Vencimiento)` DESC;";

            MySqlCommand comando2 = new MySqlCommand(sql2, conexionbd.ObtenerConexion());
            MySqlDataAdapter DP = new MySqlDataAdapter(comando2);

            DataTable dt = new DataTable();
            DP.Fill(dt);
            dataGridView2.DataSource = dt;

            DateTime fechaActual = DateTime.Now;

            foreach (DataGridViewRow fila in dataGridView2.Rows)
            {
                if (fila.Cells["DATE(Vencimiento)"].Value != null)
                {
                    DateTime fechaVencimiento = Convert.ToDateTime(fila.Cells["DATE(Vencimiento)"].Value);

                    int diasRestantes = (fechaVencimiento - fechaActual).Days;

                    if (diasRestantes < 0)
                    {
                        fila.DefaultCellStyle.BackColor = Color.Firebrick;
                        fila.DefaultCellStyle.ForeColor = Color.White;
                    }
                    else if (diasRestantes < vencimiento)
                    {
                        fila.DefaultCellStyle.BackColor = Color.LightYellow;
                        fila.DefaultCellStyle.ForeColor = Color.Black;
                    }
                }
            }

            dataGridView2.Columns[0].HeaderText = "FECHA TERMINO";
            dataGridView2.Columns[1].HeaderText = "NOMBRE";
            dataGridView2.Columns[2].HeaderText = "PATERNO";
            dataGridView2.Columns[3].HeaderText = "MATERNO";
            dataGridView2.Columns[4].HeaderText = "DNI";
            dataGridView2.Columns[5].HeaderText = "MEMBRESIA";
            dataGridView2.Columns[6].HeaderText = "OBSERVACIONES";
            dataGridView2.Columns[7].HeaderText = "FECHA INICIO";
            dataGridView2.Columns[8].HeaderText = "PRECIO";
            dataGridView2.Columns[9].HeaderText = "IMPORTE";
            dataGridView2.Columns[10].HeaderText = "ESTADO PAGO";

            // 🔹 idSocio no necesita encabezado visible
            dataGridView2.Columns[11].Visible = false;

            dataGridView2.Rows.RemoveAt(0);

            // CONVIERTO TODO EL REGISTRO DEL DATAGRIDVIEW2 EN MAYUSCULAS
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView2.Columns.Count; j++)
                {
                    if (dataGridView2.Rows[i].Cells[j].Value != null)
                    {
                        string texto = dataGridView2.Rows[i].Cells[j].Value.ToString();
                        dataGridView2.Rows[i].Cells[j].Value = texto.ToUpper();
                    }
                }
            }
        }
        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verificar que la fila seleccionada sea válida
            if (e.RowIndex >= 0 && dataGridView2.Rows[e.RowIndex].Cells["idSocio"].Value != null)
            {
                // Obtener el idSocio de la fila seleccionada
                int idSocio = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells["idSocio"].Value);

                // Crear una nueva instancia del formulario de membresía
                FrmMembresia frm = new FrmMembresia(idSocio);

                // Mostrar el formulario
                frm.ShowDialog();
            }
        }


        private void button17_Click(object sender, EventArgs e)
        {
            string sql3 = "SELECT mensajeVencimiento FROM configuracion WHERE idConfiguracion = 1";
            MySqlCommand comando3 = new MySqlCommand(sql3, conexionbd.ObtenerConexion());
            MySqlDataAdapter DP3 = new MySqlDataAdapter(comando3);
            DataTable dt3 = new DataTable();
            DP3.Fill(dt3);

            // Verificar si se obtuvieron resultados
            if (dt3.Rows.Count > 0)
            {
                // Obtener el valor del campo "mensajeVencimiento"
                string mensajeVencimiento = dt3.Rows[0]["mensajeVencimiento"].ToString();

                // Asignar el valor al TextBox
                textBox1.Text = mensajeVencimiento;
            }
            else
            {
                // No se encontraron resultados, puedes manejar esta situación según tus necesidades
                textBox1.Text = "No se encontraron resultados.";
            }

            int vencimiento = int.Parse(textBox1.Text);


            DateTime fechaActual = DateTime.Now;
            foreach (DataGridViewRow fila in dataGridView2.Rows)
            {
                if (fila.Cells["DATE(Vencimiento)"].Value != null)
                {
                    DateTime fechaVencimiento = Convert.ToDateTime(fila.Cells["DATE(Vencimiento)"].Value);

                    int diasRestantes = (fechaVencimiento - fechaActual).Days;

                    if (diasRestantes < 0)
                    {
                        // Cambia el color de la fila a rojo
                        fila.DefaultCellStyle.BackColor = Color.Firebrick;
                        fila.DefaultCellStyle.ForeColor = Color.White;
                    }

                    else if (diasRestantes < vencimiento)
                    {
                        // Cambia el color de la fila a amarillo
                        fila.DefaultCellStyle.BackColor = Color.LightYellow;
                        fila.DefaultCellStyle.ForeColor = Color.Black;
                    }
                }


            }
        }

        private void BuscadorMembresia_TextChanged(object sender, EventArgs e)
        {
            // Obtener el texto del TextBox
            string filtro = BuscadorMembresia.Text;

            // Filtrar los datos del DataGridView según el texto ingresado
            if (!string.IsNullOrEmpty(filtro))
            {
                (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"NombreSocio LIKE '%{filtro}%' OR clave LIKE '%{filtro}%'"; // Reemplaza Columna1 y Columna2 con los nombres de las columnas que deseas filtrar
            }
            else
            {
                (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = "";
            }

            int vencimiento = int.Parse(textBox1.Text);

            DateTime fechaActual = DateTime.Now;
            foreach (DataGridViewRow fila in dataGridView2.Rows)
            {
                if (fila.Cells["DATE(Vencimiento)"].Value != null)
                {
                    DateTime fechaVencimiento = Convert.ToDateTime(fila.Cells["DATE(Vencimiento)"].Value);

                    int diasRestantes = (fechaVencimiento - fechaActual).Days;

                    if (diasRestantes < 0)
                    {
                        // Cambia el color de la fila a rojo
                        fila.DefaultCellStyle.BackColor = Color.Firebrick;
                        fila.DefaultCellStyle.ForeColor = Color.White;
                    }

                    else if (diasRestantes < vencimiento)
                    {
                        // Cambia el color de la fila a amarillo
                        fila.DefaultCellStyle.BackColor = Color.LightYellow;
                        fila.DefaultCellStyle.ForeColor = Color.Black;
                    }
                }


            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            int vencimiento = int.Parse(textBox1.Text);
            // Definir la consulta SQL con el valor a actualizar
            string sqlInsert = "UPDATE configuracion SET mensajeVencimiento = @Vencimiento WHERE idConfiguracion = 1";

            // Crear el comando con parámetros
            MySqlCommand comandoInsert = new MySqlCommand(sqlInsert, conexionbd.ObtenerConexion());
            comandoInsert.Parameters.AddWithValue("@Vencimiento", vencimiento);

            // Abrir la conexión y ejecutar la consulta
            conexionbd.ObtenerConexion();
            comandoInsert.ExecuteNonQuery();
            

            // Mostrar un mensaje de éxito
            MessageBox.Show("Valor insertado correctamente en la tabla.");

        }

        private void textBoxBusqueda_TextChanged(object sender, EventArgs e)
        {

            string textoBusqueda = textBoxBusqueda.Text;
            DataRow[] resultado = tablaSocio.Select($"Nombre = '{textoBusqueda}' OR clave = '{textoBusqueda}'");

            if (resultado.Length > 0)
            {
                int idSocioSeleccionado = Convert.ToInt32(resultado[0]["idSocio"]);
                labelnombre.Text = Convert.ToString(resultado[0]["Nombre"]).ToUpper();
                labelpaterno.Text = Convert.ToString(resultado[0]["Paterno"]).ToUpper();
                labelmaterno.Text = Convert.ToString(resultado[0]["Materno"]).ToUpper();

                // Realizar la consulta en la tabla visita para obtener los datos requeridos
                string consultaVisita = $"SELECT fechaCreacion FROM visita WHERE idSocio = {idSocioSeleccionado} ORDER BY fechaCreacion DESC";
                MySqlCommand comandoVisita = new MySqlCommand(consultaVisita, conexionbd.ObtenerConexion());
                MySqlDataAdapter adaptadorVisita = new MySqlDataAdapter(comandoVisita);
                DataTable tablaVisita = new DataTable();
                adaptadorVisita.Fill(tablaVisita);

                // Mostrar los datos en el DataGridView
                dataGridView3.DataSource = tablaVisita;
                dataGridView3.Columns[0].HeaderText = "HISTORIAL DE INGRESO DD-MM-AAAA ";
                dataGridView3.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                ResaltarFechasEnCalendario();
                
            }
            else
            {
                dataGridView3.DataSource = null;
                labelnombre.Text = "";
                labelpaterno.Text = "";
                labelmaterno.Text = "";
                monthCalendar1.RemoveAllBoldedDates();
            }

            if(textBoxBusqueda.Text == "")
            {
                dataGridView3.DataSource = null;
                labelnombre.Text = "";
                labelpaterno.Text = "";
                labelmaterno.Text = "";
                monthCalendar1.RemoveAllBoldedDates();
            }
        }


    }
}
