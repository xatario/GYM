using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MySql.Data.MySqlClient;


using System.Collections;



namespace Gimnasio.Registro
{
    public partial class frmRegistro : Form, DPFP.Capture.EventHandler
    {
        clsRegistro oRegistro;

        public int id = 0;
        //huella
        Bitmap img = null;
        private DPFP.Verification.Verification verificador;
        DPFP.Capture.SampleConversion sp;
        DPFP.Capture.Capture cp;

        //delegados
        public delegate void dBuscarSocio(int clave);
        public event dBuscarSocio OnBuscarSocio;

        public frmRegistro()
        {
            try
            {
                cp = new DPFP.Capture.Capture();
                sp = new DPFP.Capture.SampleConversion();

                cp.StartCapture();
                cp.EventHandler = this;
                verificador = new DPFP.Verification.Verification();
                this.OnBuscarSocio += BuscaSocio;
            }
            catch { }

            InitializeComponent();
            timer2.Interval = 4000;
            timer2.Tick += timer2_Tick;
          
        }

        private void frmRegistro_Load(object sender, EventArgs e)
        {
            conexionbd conexionbd = new conexionbd();
            

            Utilidades.clsGrafico.centraX(this, panelContenedor);
            Utilidades.clsGrafico.centraX(this, panelEncabezado);
            Utilidades.clsGrafico.centraX(this, panelFoot);
            cargaDatos();
            CheckForIllegalCrossThreadCalls = false;
         // this.SetStyle(ControlStyles.SupportsTransparentBackColor, false);


        }

        private void cargaDatos()
        {
            Configuracion.clsConfiguracion.getDatos();
            lblNombreGimnacio.Text = Configuracion.clsConfiguracion.datos.NombreGimnacio.ToUpper();
            lblDomicilio.Text = Configuracion.clsConfiguracion.datos.Domicilio;
            lblTelefono.Text = Configuracion.clsConfiguracion.datos.Telefono;
            lblRFC.Text = Configuracion.clsConfiguracion.datos.RFC.ToUpper();
            lblMensaje.Text = Configuracion.clsConfiguracion.datos.Mensaje;
            if (Configuracion.clsConfiguracion.datos.Logo != null)
            {

                MemoryStream stream = new MemoryStream(Configuracion.clsConfiguracion.datos.Logo);
                Bitmap image = new Bitmap(stream);
                pbLogo.Image = image;
            }

            lblFecha.Text = DateTime.Now.ToLongDateString();
            lblHora.Text = DateTime.Now.ToLongTimeString();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void frmRegistro_SizeChanged(object sender, EventArgs e)
        {
            Utilidades.clsGrafico.centraX(this, panelContenedor);
            Utilidades.clsGrafico.centraX(this, panelEncabezado);
            Utilidades.clsGrafico.centraX(this, panelFoot);
        }

        private void txtClave_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtNombre.Text = "";
                txtMaterno.Text = "";
                txtPaterno.Text = "";
                label13.Text = "";
                label6.Text = "";
                label4.Text = "";
                label6.Text = "";
                label9.Text = "";
                lblVencimiento.Text = "";

                if (!ExpresionesRegulares.RegEX.isNumber(txtClave.Text.ToString()))
                {
                    MessageBox.Show("La clave es numerica, debes introducir solo numeros");
                    return;

                }

                int clave = int.Parse(txtClave.Text.ToString());
                BuscaSocio(clave);

                

                string cadql1 = "SELECT idsocio, Nombre, Paterno, Materno, Telefono FROM socio  where Nombre ='" + txtNombre.Text + "'and Paterno='" + txtPaterno.Text + "'and Materno='"+ txtMaterno.Text +"' ";
                MySqlCommand comando = new MySqlCommand(cadql1, conexionbd.ObtenerConexion());

                MySqlDataReader leer = comando.ExecuteReader();

                if (leer.Read() == true)
                {


                    string idsocio = leer["idSocio"].ToString();

                    string cadql2 = "SELECT idSocioMembresia, idMembresia, Precio, idMembresia FROM sociomembresia  where idSocio ='" + idsocio + "' and idEstado= '" + 1 + "'and fechaCreacion = (SELECT MAX(fechaCreacion) FROM sociomembresia WHERE idSocio ='" + idsocio + "' and idEstado= '" + 1 + "') ";
                    MySqlCommand comando2 = new MySqlCommand(cadql2, conexionbd.ObtenerConexion());

                    MySqlDataReader leer2 = comando2.ExecuteReader();


                    if (leer2.Read() == true)
                    {

                        // CONSULTAMOS A LA TABLA VWULTIMAMEMBRESIADETALLADA LA FECHA DEL VENCIMIENTO DEL USUARIO
                        string nombre_membresia = leer2.GetString(3);
                        int precio = leer2.GetInt32(2);
                        string membresiaven = leer2["idSocioMembresia"].ToString();

                        MySqlCommand obetener_nombreMembresia = new MySqlCommand(String.Format("SELECT Nombre FROM membresia WHERE idMembresia = '" + nombre_membresia + "' "), conexionbd.ObtenerConexion());
                        MySqlDataReader datareader = obetener_nombreMembresia.ExecuteReader();

                        while (datareader.Read()) { label4.Text = datareader.GetString(0).ToUpper();  }
                        datareader.Close();

                        MySqlCommand comando10 = new MySqlCommand(String.Format("SELECT vencimiento FROM vwultimamembresiadetallada WHERE idSocioMembresia = '" + membresiaven + "' "), conexionbd.ObtenerConexion());
                        MySqlDataReader reader = comando10.ExecuteReader();

                        while (reader.Read()) 
                        { 
                       
                            DateTime vencimiento = reader.GetDateTime(0);
                           
                        
                                string idMembresia = leer2["idMembresia"].ToString();
                                string idSocioMembresia = leer2["idSocioMembresia"].ToString();

                                string cadql3 = "SELECT importe, observacion, fecha FROM sociomembresia_pago  where idSocioMembresia ='" + idSocioMembresia + "' and idEstado= '" + 1 + "'and fecha=(SELECT max(fecha) FROM sociomembresia_pago WHERE idSocioMembresia ='" + idSocioMembresia + "' )  ";
                                MySqlCommand comando3 = new MySqlCommand(cadql3,conexionbd.ObtenerConexion());

                                MySqlDataReader leer3 = comando3.ExecuteReader();

                                if (leer3.Read() == true)
                                {
                                    label13.Text = leer3["importe"].ToString();
                                   // label6.Text = leer3["observacion"].ToString();
                                    label9.Text = leer3["fecha"].ToString();
                                }
                                leer3.Close();
                                string cadql4 = "SELECT sum(importe) FROM sociomembresia_pago  where idSocioMembresia ='" + idSocioMembresia + "' and idEstado= '" + 1 + "'  ";
                                MySqlCommand comando4 = new MySqlCommand(cadql4, conexionbd.ObtenerConexion());
                                MySqlDataReader leer4 = comando4.ExecuteReader();
                                if (leer4.Read() == true)
                                {
                                    if (leer4["sum(importe)"].ToString() == "")
                                    {
                                        label13.Text = "0";
                                        int deuda = precio ;
                                        //MessageBox.Show(leer4.GetString(0));
                                        label6.Text = deuda.ToString();
                                    }

                                    else
                                    {
                                        label13.Text = leer4["sum(importe)"].ToString();
                                        int deuda = precio - leer4.GetInt32(0);
                                        //MessageBox.Show(leer4.GetString(0));
                                        //MessageBox.Show("ENTRA AL ELSE");
                                        label6.Text = deuda.ToString();
                                    }

                                    
                                  //  string cadql5 = "SELECT sum(importe) - (SELECT Precio FROM membresia where idMembresia = '"+ idMembresia +"') FROM sociomembresia_pago  where idSocioMembresia ='" + idSocioMembresia + "' and idEstado= '" + 1 + "'  ";
                                    //MySqlCommand comando5 = new MySqlCommand(cadql5, conexionbd.ObtenerConexion());
                                    //MySqlDataReader leer5 = comando5.ExecuteReader();
                                }
                                leer4.Close();

                         }
                        reader.Close();
                    }
                    leer2.Close();

                }
                leer.Close();
                MySqlConnection.ClearAllPools();
                timer2.Start();
            }
        }

        /// <summary>
        /// busca al socio
        /// </summary>
        /// <param name="clave"></param>
        private void BuscaSocio(int clave)
        {
            oRegistro = new clsRegistro(clave);
            if (oRegistro.buscaDatos())
            {
                //cargar datos
                txtNombre.Invoke(new MethodInvoker(delegate { 
                    txtNombre.Text = oRegistro.datos.NombreSocio;
                    txtPaterno.Text = oRegistro.datos.Paterno;
                    txtMaterno.Text = oRegistro.datos.Materno;
                    lblVencimiento.Text = oRegistro.datos.Vencimiento.ToLongDateString();
                }));


                //cargar foto
                pbFoto.Image = null;
                if (oRegistro.datos.foto != null)
                {
                    MemoryStream stream = new MemoryStream(oRegistro.datos.foto);
                    Bitmap image = new Bitmap(stream);
                    pbFoto.Image = image;
                }

                try { txtClave.Text = ""; } catch { }
                if (DateTime.Compare(oRegistro.datos.Vencimiento, DateTime.Now) < 0)
                {
                    lblVencimiento.ForeColor = Color.Red;
                    MessageBox.Show("Se a términado tu membresia");

                }
                //si no se a vencido la registramos REGISTRO
                else
                {
                    //si le quedanm pocos dias se le avisa de que ya mero se le acaba su membresia
                    Configuracion.clsConfiguracion.getDatos();
                    if (Configuracion.clsConfiguracion.datos.mensajeVencimiento != null && Configuracion.clsConfiguracion.datos.mensajeVencimiento > 0)//si tiene abilitado el mensaje
                        if (DateTime.Compare(oRegistro.datos.Vencimiento, DateTime.Now.AddDays(Configuracion.clsConfiguracion.datos.mensajeVencimiento)) < 0)//si ya mero se termina
                        {
                            DateTime newDate = oRegistro.datos.Vencimiento;
                            DateTime oldDate = DateTime.Now;
                            TimeSpan ts = newDate - oldDate;
                            int diasFaltantes = ts.Days;

                            MessageBox.Show("Quedan " + (diasFaltantes) + " dias de tu membresia");
                        }

                    lblVencimiento.ForeColor = Color.Black;

                    if (!oRegistro.addVisita())
                    {
                        MessageBox.Show(oRegistro.error);
                    }
                }
            }
            else
            {
                MessageBox.Show("No existe un socio con esa clave ó su membresia a expirado");
            }
        }

        private void txtClave_TextChanged(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblFecha.Text = DateTime.Now.ToLongDateString();
            lblHora.Text = DateTime.Now.ToLongTimeString();
        }

        #region HUELLA
        public void OnComplete(object Capture, string ReaderSerialNumber, DPFP.Sample Sample)
        {
            // Extraemos las caracteristicas de la plantilla
            DPFP.FeatureSet features = ExtractFeatures(Sample, DPFP.Processing.DataPurpose.Verification);

            //vamos por todos los socios que tengan huella
              dsGimnasioTableAdapters.vwultimamembresiadetalladaTableAdapter ta = new dsGimnasioTableAdapters.vwultimamembresiadetalladaTableAdapter();
              dsGimnasio.vwultimamembresiadetalladaDataTable lstSocios = ta.GetData();

             foreach(dsGimnasio.vwultimamembresiadetalladaRow oSocioHuella in lstSocios.Rows)
             {
                 //comparamos socio por socio con huella
                 if (oSocioHuella.huella != null)
                 {
                     MemoryStream fingerprintData = new MemoryStream(oSocioHuella.huella);
                     // Creamos una plantilla a partir de esos bytes...
                     DPFP.Template templateOriginal = new DPFP.Template(fingerprintData);
                     
                     // Verificamos que las caracteristicas sean buenas
                     if (features != null)
                     {
                         // Compare the feature set with our template
                         DPFP.Verification.Verification.Result result = new DPFP.Verification.Verification.Result();
                         verificador.Verify(features, templateOriginal, ref result);
                         if (result.Verified)
                         {
                             OnBuscarSocio(oSocioHuella.idSocio);


                             //AQUI EMPIEZA LA MODIFICACION DEL PROGRAMA HUELLA REGION

                             string cadql1 = "SELECT idsocio, Nombre, Paterno, Materno, Telefono FROM socio  where Nombre ='" + txtNombre.Text + "'and Paterno='" + txtPaterno.Text + "'and Materno='" + txtMaterno.Text + "' ";
                             MySqlCommand comando = new MySqlCommand(cadql1, conexionbd.ObtenerConexion());

                             MySqlDataReader leer = comando.ExecuteReader();

                             if (leer.Read() == true)
                             {


                                 string idsocio = leer["idSocio"].ToString();

                                 string cadql2 = "SELECT idSocioMembresia, idMembresia, Precio, idMembresia FROM sociomembresia  where idSocio ='" + idsocio + "' and idEstado= '" + 1 + "'and fechaCreacion = (SELECT MAX(fechaCreacion) FROM sociomembresia WHERE idSocio ='" + idsocio + "' and idEstado= '" + 1 + "') ";
                                 MySqlCommand comando2 = new MySqlCommand(cadql2, conexionbd.ObtenerConexion());

                                 MySqlDataReader leer2 = comando2.ExecuteReader();


                                 if (leer2.Read() == true)
                                 {

                                     // CONSULTAMOS A LA TABLA VWULTIMAMEMBRESIADETALLADA LA FECHA DEL VENCIMIENTO DEL USUARIO
                                     string nombre_membresia = leer2.GetString(3);
                                     int precio = leer2.GetInt32(2);
                                     string membresiaven = leer2["idSocioMembresia"].ToString();

                                     MySqlCommand obetener_nombreMembresia = new MySqlCommand(String.Format("SELECT Nombre FROM membresia WHERE idMembresia = '" + nombre_membresia + "' "), conexionbd.ObtenerConexion());
                                     MySqlDataReader datareader = obetener_nombreMembresia.ExecuteReader();

                                     while (datareader.Read()) { label4.Text = datareader.GetString(0).ToUpper(); }
                                     datareader.Close();

                                     MySqlCommand comando10 = new MySqlCommand(String.Format("SELECT vencimiento FROM vwultimamembresiadetallada WHERE idSocioMembresia = '" + membresiaven + "' "), conexionbd.ObtenerConexion());
                                     MySqlDataReader reader = comando10.ExecuteReader();

                                     while (reader.Read())
                                     {

                                         DateTime vencimiento = reader.GetDateTime(0);



                                         string idMembresia = leer2["idMembresia"].ToString();
                                         string idSocioMembresia = leer2["idSocioMembresia"].ToString();

                                         string cadql3 = "SELECT importe, observacion, fecha FROM sociomembresia_pago  where idSocioMembresia ='" + idSocioMembresia + "' and idEstado= '" + 1 + "'and fecha=(SELECT max(fecha) FROM sociomembresia_pago WHERE idSocioMembresia ='" + idSocioMembresia + "' )  ";
                                         MySqlCommand comando3 = new MySqlCommand(cadql3, conexionbd.ObtenerConexion());

                                         MySqlDataReader leer3 = comando3.ExecuteReader();

                                         if (leer3.Read() == true)
                                         {
                                             label13.Text = leer3["importe"].ToString();
                                             // label6.Text = leer3["observacion"].ToString();
                                             label9.Text = leer3["fecha"].ToString();
                                         }
                                         leer3.Close();
                                         string cadql4 = "SELECT sum(importe) FROM sociomembresia_pago  where idSocioMembresia ='" + idSocioMembresia + "' and idEstado= '" + 1 + "'  ";
                                         MySqlCommand comando4 = new MySqlCommand(cadql4, conexionbd.ObtenerConexion());
                                         MySqlDataReader leer4 = comando4.ExecuteReader();
                                         if (leer4.Read() == true)
                                         {
                                             if (leer4["sum(importe)"].ToString() == "")
                                             {
                                                 label13.Text = "0";
                                                 int deuda = precio;
                                                 //MessageBox.Show(leer4.GetString(0));
                                                 label6.Text = deuda.ToString();
                                             }

                                             else
                                             {
                                                 label13.Text = leer4["sum(importe)"].ToString();
                                                 int deuda = precio - leer4.GetInt32(0);
                                                 //MessageBox.Show(leer4.GetString(0));
                                                 //MessageBox.Show("ENTRA AL ELSE");
                                                 label6.Text = deuda.ToString();
                                             }

                                            // string cadql5 = "SELECT sum(importe) - (SELECT Precio FROM membresia where idMembresia = '" + idMembresia + "') FROM sociomembresia_pago  where idSocioMembresia ='" + idSocioMembresia + "' and idEstado= '" + 1 + "'  ";
                                            // MySqlCommand comando5 = new MySqlCommand(cadql5, conexionbd.ObtenerConexion());
                                            // MySqlDataReader leer5 = comando5.ExecuteReader();
                                         }
                                         leer4.Close();

                                     }
                                     reader.Close();
                                 }
                                 leer2.Close();

                             }
                             leer.Close();
                             MySqlConnection.ClearAllPools();


                             // AQUI TERMINA LA MODIFICACION
                             return;
                         }
                        
                     }
                 }

             }
             MessageBox.Show("No existe un socio con esta huella");

        }

        public void OnFingerGone(object Capture, string ReaderSerialNumber)
        {
            
        }

        public void OnFingerTouch(object Capture, string ReaderSerialNumber)
        {
            img = null;
        }

        public void OnReaderConnect(object Capture, string ReaderSerialNumber)
        {
            
        }

        public void OnReaderDisconnect(object Capture, string ReaderSerialNumber)
        {
            
        }

        public void OnSampleQuality(object Capture, string ReaderSerialNumber, DPFP.Capture.CaptureFeedback CaptureFeedback)
        {
            
        }

        protected DPFP.FeatureSet ExtractFeatures(DPFP.Sample Sample, DPFP.Processing.DataPurpose Purpose)
        {
            DPFP.Processing.FeatureExtraction Extractor = new DPFP.Processing.FeatureExtraction();	// Create a feature extractor
            DPFP.Capture.CaptureFeedback feedback = DPFP.Capture.CaptureFeedback.None;
            DPFP.FeatureSet features = new DPFP.FeatureSet();
            Extractor.CreateFeatureSet(Sample, Purpose, ref feedback, ref features);			// TODO: return features as a result?
            if (feedback == DPFP.Capture.CaptureFeedback.Good)
                return features;
            else
                return null;
        }
        #endregion

        private void frmRegistro_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(cp!=null)
            cp.StopCapture();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            txtNombre.Text = "";
            txtMaterno.Text = "";
            txtPaterno.Text = "";
            label13.Text = "";
            label6.Text = "";
            label4.Text = "";
            label6.Text = "";
            label9.Text = "";
            lblVencimiento.Text = "";
            pbFoto.Image = null;
            timer2.Stop();
        }
    }
}
