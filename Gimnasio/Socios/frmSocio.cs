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

namespace Gimnasio.Socios
{
    public partial class frmSocio : Form, DPFP.Capture.EventHandler
    {
        public int id = 0;
        clsSocio oSocio = new clsSocio();

        //huella
        Bitmap img = null;
        DPFP.Capture.SampleConversion sp;
        DPFP.Capture.Capture cp;
        DPFP.Sample sample;
        private DPFP.Processing.Enrollment Enroller;//sirve para crear el template
        private DPFP.Template plantilla; //sirve para tener la plantilla

        public frmSocio()
        {
            try
            {
                cp = new DPFP.Capture.Capture();
                sp = new DPFP.Capture.SampleConversion();
                sample = new DPFP.Sample();
                  //inicializar lector
                cp.StartCapture();
                cp.EventHandler = this;
                Enroller = new DPFP.Processing.Enrollment();
            }
            catch { }

            InitializeComponent();
          
        }

        private void frmSocio_Load(object sender, EventArgs e)
        {
            if (id > 0)
            {
                txtClave.Focus();
                cargaDatos();
            }

        }

        private void cargaDatos()
        {
            if (oSocio.getDatos(id))
            {

                txtNombre.Text = oSocio.datos.Nombre;
                txtPaterno.Text = oSocio.datos.Paterno;
                txtMaterno.Text = oSocio.datos.Materno;
                txtTelefono.Text = oSocio.datos.Telefono;
                txtObservaciones.Text = oSocio.datos.Observaciones;
                txtClave.Text = oSocio.datos.clave;

                if(oSocio.datos.foto!=null)
                {
                    MemoryStream stream = new MemoryStream(oSocio.datos.foto);
                    Bitmap image = new Bitmap(stream);
                    pbFoto.Image = image;
                }
                if (oSocio.datos.huella != null)
                {                  
                    //pbHuella.Image = image;

                    pbHuella.Image = Properties.Resources.huella;
                }

            }
            else
            {
                MessageBox.Show("Ocurrio un problema al cargar los datos " + oSocio.getError());
                this.Close();
            }
        }

        /// <summary>
        /// aqui abrimos la captura de imagen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbFoto_Click(object sender, EventArgs e)
        {
            frmFoto frmF = new frmFoto();
            frmF.pbFotoSocio = pbFoto;
            frmF.Show();
            frmF.WindowState = FormWindowState.Maximized;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (id <= 0)
            {
                agrega();
            }
            else
            {
                modifica();
            }
        }

        private void agrega()
        {
            try
            {
                //validaciones
                if (txtNombre.Text.Trim().Equals("") || txtPaterno.Text.Trim().Equals("") || txtMaterno.Text.Equals(""))
                {
                    MessageBox.Show("Nombre, Apellido Paterno y Apellido Materno son obligatorios");
                    return;
                }

                string consulta = "SELECT clave FROM socio  where clave ='" + txtClave.Text + "' AND idEstado = 1 ";
                MySqlCommand comando = new MySqlCommand(consulta, conexionbd.ObtenerConexion());
                MySqlDataReader leer = comando.ExecuteReader();

                if (leer.Read() == true)
                {
                    MessageBox.Show("EL DNI YA SE ENCUENTRA REGISTRADO ");
                    return;
                }


                //asignacion de datos
                oSocio.Nombre = txtNombre.Text.Trim();
                oSocio.Paterno = txtPaterno.Text.Trim();
                oSocio.Materno = txtMaterno.Text.Trim();
                oSocio.Telefono = txtTelefono.Text.Trim();
                oSocio.Observaciones = txtObservaciones.Text.Trim();
                oSocio.clave = txtClave.Text.Trim();
                if(pbFoto.Image!=null)
                oSocio.foto = Utilidades.OperacionesFormulario.conviertePicBoxImageToByte(pbFoto);
                if (plantilla != null)
                    oSocio.huella = plantilla.Bytes;
                oSocio.idUsuarioLog = Utilidades.clsUsuario.idUsuario;
                if (oSocio.add())
                {
                    MessageBox.Show("Registro agregado con exito");
                    this.Close();
                }
                else
                    MessageBox.Show(oSocio.getError());

            }
            catch (Exception EX)
            {
                MessageBox.Show("Ocurrio un error de sistema " + EX.Message);
            }
        }

        private void modifica()
        {

            try
            {
                //validaciones
                if (txtNombre.Text.Trim().Equals("") || txtPaterno.Text.Trim().Equals("") || txtMaterno.Text.Equals(""))
                {
                    MessageBox.Show("Nombre, Apellido Paterno y Apellido Materno son obligatorios");
                    return;
                }

                //asignacion de datos
                oSocio.Nombre = txtNombre.Text.Trim();
                oSocio.Paterno = txtPaterno.Text.Trim();
                oSocio.Materno = txtMaterno.Text.Trim();
                oSocio.Telefono = txtTelefono.Text.Trim();
                oSocio.Observaciones = txtObservaciones.Text.Trim();
                oSocio.clave = txtClave.Text.Trim();
                if (pbFoto.Image != null)
                    oSocio.foto = Utilidades.OperacionesFormulario.conviertePicBoxImageToByte(pbFoto);
                if (plantilla != null)
                    oSocio.huella = plantilla.Bytes;
                else
                    oSocio.huella = oSocio.datos.huella; //asignamos la ya guardada

                oSocio.idUsuarioLog = Utilidades.clsUsuario.idUsuario;
                if (oSocio.edit(id))
                {
                    MessageBox.Show("Registro modificado con exito");
                    this.Close();
                }
                else
                    MessageBox.Show(oSocio.getError());

            }
            catch (Exception EX)
            {
                MessageBox.Show("Ocurrio un error de sistema " + EX.Message);
            }
        }


        #region HUELLA

        public void OnComplete(object Capture, string ReaderSerialNumber, DPFP.Sample Sample)
        {

            //solo muestra la huella
            sp.ConvertToPicture(Sample, ref img);
            pbHuella.Image = img;


            DPFP.FeatureSet features = ExtractFeatures(Sample, DPFP.Processing.DataPurpose.Enrollment);

            if (features != null)
            {
                Enroller.AddFeatures(features);

                //agregamos al progressbar 
                pbarHuella.Invoke(new MethodInvoker(delegate { pbarHuella.Value += 25; }));
                

                switch (Enroller.TemplateStatus)
                {
                    case DPFP.Processing.Enrollment.Status.Ready:	// report success and stop capturing
                        plantilla = Enroller.Template;
                        cp.StopCapture();
                       
                        break;

                    case DPFP.Processing.Enrollment.Status.Failed:	// report failure and restart capturing
                        Enroller.Clear();
                        cp.StopCapture();
                        plantilla = Enroller.Template;
                        cp.StartCapture();
                        break;
                }
            }
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


        // crea feature con magia
        protected DPFP.FeatureSet ExtractFeatures(DPFP.Sample Sample, DPFP.Processing.DataPurpose Purpose)
        {
            DPFP.Processing.FeatureExtraction Extractor = new DPFP.Processing.FeatureExtraction();
            DPFP.Capture.CaptureFeedback feedback = DPFP.Capture.CaptureFeedback.None;
            DPFP.FeatureSet features = new DPFP.FeatureSet();
            Extractor.CreateFeatureSet(Sample, Purpose, ref feedback, ref features);		
            if (feedback == DPFP.Capture.CaptureFeedback.Good)
                return features;
            else
                return null;
        }
        #endregion

        private void frmSocio_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(cp!=null)
                cp.StopCapture();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmFoto frmF = new frmFoto();
            frmF.pbFotoSocio = pbFoto;
            frmF.Show();
            frmF.WindowState = FormWindowState.Maximized;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "Archivos jpg (*.jpg)|*.jpg|Archivos png (*.png)|*.png";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            pbFoto.Image = Image.FromStream(myStream);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error del Sistema: " + ex.Message);
                }
            }
        }

        private void txtClave_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                txtNombre.Focus();
                e.Handled = true;
            }
        }
        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                txtPaterno.Focus();
                e.Handled = true;
            }
        }

        private void txtPaterno_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                txtMaterno.Focus();
                e.Handled = true;
            }
        }
        private void txtMaterno_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                txtTelefono.Focus();
                e.Handled = true;
            }
        }
        private void txtTelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                txtObservaciones.Focus();
                e.Handled = true;
            }
        }

    }
}
