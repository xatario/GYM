using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Runtime.InteropServices; // para mover el frame con el mose
using System.Configuration;


namespace Gimnasio
{
    public partial class FrmMain : Form
    {
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        public FrmMain()
        {
            InitializeComponent();
            
            timer1.Enabled = true;
            
        }


        private void Form1_Load(object sender, EventArgs e)
        {
           // this.WindowState = FormWindowState.Maximized;
            sinSesion();

            string mes = DateTime.Now.ToString("MMMM");
            label8.Text = mes.ToUpper();
            label9.Text = mes.ToUpper();

            string fecha_actual = DateTime.Now.ToString("yyyy/MM/01");
            string fecha_mes = DateTime.Now.ToString("MM");



            int año = int.Parse(DateTime.Now.ToString("yyyy"));
            string Strdate;
            if (fecha_mes == "12") 
            {
                DateTime fecha_final = new DateTime(año+1, 01, 01);
                 Strdate = fecha_final.ToString("yyyy/MM/dd");
                
            }
            else
            {
                DateTime fecha_final = new DateTime(año, Convert.ToInt32(fecha_mes)+1, 01);
                 Strdate = fecha_final.ToString("yyyy/MM/dd");
                
            }

            

            MySqlCommand comando = new MySqlCommand(String.Format("Select COUNT(idsocio) from sociomembresia where fechaCreacion >= '" + fecha_actual + "'and fechaCreacion <='" + Strdate + "' and (idEstado=1 or idEstado=4) "), conexionbd.ObtenerConexion());
            MySqlDataReader reader = comando.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine(fecha_actual);
                Console.WriteLine(Strdate);

                label6.Text = reader["COUNT(idsocio)"].ToString();
            }

            reader.Close();

            MySqlCommand comando1 = new MySqlCommand(String.Format("SELECT SUM(importe) from vwsociomembresia_pago where fecha >= '" + fecha_actual + "' and fecha <='" + Strdate + "' and (idEstado=1 or idEstado=4) "), conexionbd.ObtenerConexion());
            MySqlDataReader reader1 = comando1.ExecuteReader();

            while (reader1.Read())
            {
                label7.Text = reader1["SUM(importe)"].ToString();
                
                
            }
            reader1.Close();

            MySqlCommand comando2 = new MySqlCommand(String.Format("SELECT idSocioMembresia from vwsociomembresia_pago WHERE importe-Precio<>0 and idEstado=(1 or 4) and estadoMembresia='Sin pagar'"), conexionbd.ObtenerConexion());
            MySqlDataReader reader2 = comando2.ExecuteReader();
            int deudores = 0;
            while (reader2.Read())
            {
             //   int id = reader2.GetInt32(0);
                //MessageBox.Show(Convert.ToString(id));

            //    MySqlCommand comando3 = new MySqlCommand(String.Format("SELECT idSocioMembresia FROM vwsociomembresias WHERE idSocioMembresia='"+id+"' and idEstado=(1 or 4) "), conexionbd.ObtenerConexion());
            //    MySqlDataReader reader3 = comando3.ExecuteReader();
                
              //  while (reader3.Read())
               // {
                    deudores = deudores+1;
                    
                //}

              //  reader3.Close();
                label11.Text = deudores.ToString();
            }

            reader2.Close();

            
        }
        /// <summary>
        /// metodo que abre la ventana de login
        /// </summary>
        private void sinSesion()
        {
            panel1.Enabled = false;
            cerrarFormuarios();

            Sesion.frmLogin frmL = new Sesion.frmLogin();
            frmL.ShowDialog();

            if(Utilidades.clsUsuario.existeSesion)
            panel1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel11.Visible = false;
            if (!detectarFormularioAbierto("frmUsuarios"))
            {
                Usuarios.frmUsuarios frmU = new Usuarios.frmUsuarios();
                frmU.MdiParent = this;
                frmU.Show();
                frmU.WindowState = FormWindowState.Maximized;
            }
        }

        

        private void button3_Click(object sender, EventArgs e)
        {
            panel11.Visible = false;
            if (!detectarFormularioAbierto("frmMembresias"))
            {
                Membresias.frmMembresias frmM = new Membresias.frmMembresias();
                frmM.MdiParent = this;
                frmM.Show();
                frmM.WindowState = FormWindowState.Maximized;
            }
        }

        #region METODOS PRIVADOS
        private bool detectarFormularioAbierto(string formulario)
        {
            bool abierto = false;

            if (Application.OpenForms[formulario] != null)
            {
                abierto = true;
                Application.OpenForms[formulario].Activate();
                Application.OpenForms[formulario].WindowState = FormWindowState.Maximized;
            }
            return abierto;
        }

        private void cerrarFormuarios()
        {
            if (this.MdiChildren.Length > 0)   
            {
                foreach (Form childForm in this.MdiChildren)
                    childForm.Close();

              
            }
        }

        #endregion

        private void button4_Click(object sender, EventArgs e)
        {
            if (!detectarFormularioAbierto("frmProductos"))
            {
                Productos.frmProductos frmM = new Productos.frmProductos();
                frmM.MdiParent = this;
                frmM.Show();
                frmM.WindowState = FormWindowState.Maximized;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel11.Visible = false;
            if (!detectarFormularioAbierto("frmSocios"))
            {
               Socios.frmSocios frmM = new Socios.frmSocios();
                frmM.MdiParent = this;
                frmM.Show();
                frmM.WindowState = FormWindowState.Maximized;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            panel11.Visible = false;
            if (!detectarFormularioAbierto("frmRegistro"))
            {
                Registro.frmRegistro frmM = new Registro.frmRegistro();
                frmM.Show();
                frmM.WindowState = FormWindowState.Maximized;
            }
        }

        private void FrmMain_KeyDown(object sender, KeyEventArgs e)
        {
            //SE ABRE REGISTRO CON F12
            if (e.KeyCode == Keys.F12)
            {
                if (!detectarFormularioAbierto("frmRegistro"))
                {
                    Registro.frmRegistro frmM = new Registro.frmRegistro();
                    frmM.Show();
                    frmM.WindowState = FormWindowState.Maximized;
                }
            }
        }

        private void FrmMain_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void FrmMain_KeyUp(object sender, KeyEventArgs e)
        {
           
        }

        private void button7_Click(object sender, EventArgs e)
        {
            panel11.Visible = false;
            if (!detectarFormularioAbierto("frmReportes"))
            {
                Reportes.frmReportes frmM = new Reportes.frmReportes();
                frmM.MdiParent = this;
                frmM.Show();
                frmM.WindowState = FormWindowState.Maximized;
            }
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void cerrarSesiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Utilidades.clsUsuario.salir())
            {
                sinSesion();
            }
            else
            {
                MessageBox.Show(Utilidades.clsUsuario.error);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (!detectarFormularioAbierto("frmConfiguracion"))
            {
                Configuracion.frmConfiguracion frmM = new Configuracion.frmConfiguracion();
                frmM.MdiParent = this;
                frmM.Show();
                frmM.WindowState = FormWindowState.Maximized;
            }
        }

        private void iniciarSesiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Utilidades.clsUsuario.existeSesion)
                sinSesion();
            else
                MessageBox.Show("Cierra la sesión primero");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (!detectarFormularioAbierto("frmEntradas"))
            {
                Entradas.frmEntradas frmM = new Entradas.frmEntradas();
                frmM.MdiParent = this;
                frmM.Show();
                frmM.WindowState = FormWindowState.Maximized;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!detectarFormularioAbierto("frmSalidas"))
            {
                Salidas.frmSalidas frmM = new Salidas.frmSalidas();
                frmM.MdiParent = this;
                frmM.Show();
                frmM.WindowState = FormWindowState.Maximized;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
      
        }

        private void button11_Click(object sender, EventArgs e)
        {
           
        }

        private void button12_Click(object sender, EventArgs e)
        {

         
            SaveFileDialog sFileDialog1 = new SaveFileDialog();

            sFileDialog1.InitialDirectory = "c:\\";
            sFileDialog1.Filter = "Archivos de sql (*.sql)|*.sql";
            sFileDialog1.FilterIndex = 1;
            sFileDialog1.RestoreDirectory = true;

            if (sFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //se guarda el respaldo
                    try
                    {
                        string constr = "server=localhost;User Id=root;Persist Security Info=True;database=gym";
                        string file = sFileDialog1.FileName;
                        MySqlBackup mb = new MySqlBackup(constr);
                        mb.ExportInfo.FileName = file;
                        mb.ExportInfo.ExportViews = false;

                        if (System.IO.File.Exists(file))
                            System.IO.File.Delete(file);
                        mb.Export();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ocurrio un error " + ex.Message);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error del Sistema: " + ex.Message);
                }
            }

            MessageBox.Show("Información guardada con exito");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Estas seguro de abrir un archivo de respaldo? se remplazara toda la información en el sistema con lo que existe en el archivo de respaldo", "Confirm delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                //se abre la ventana para seleccionar acrhivo

                OpenFileDialog openFileDialog1 = new OpenFileDialog();

                openFileDialog1.InitialDirectory = "c:\\";
                openFileDialog1.Filter = "Archivos de sql (*.sql)|*.sql";
                openFileDialog1.FilterIndex = 1;
                openFileDialog1.RestoreDirectory = true;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                            //se abre el respaldo
                            try
                            {
                                string constr = "server=localhost;User Id=root;Persist Security Info=True;database=gym";
                                string file = openFileDialog1.FileName;
                                MySqlBackup mb = new MySqlBackup(constr);
                                mb.ImportInfo.FileName = file;
                                mb.ImportInfo.SetTargetDatabase("gym", ImportInformations.CharSet.utf8);
                                mb.Import();
                            }catch(Exception ex){
                                MessageBox.Show("Ocurrio un error "+ex.Message);
                            }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error del Sistema: " + ex.Message);
                    }
                }

                MessageBox.Show("Información restaurada con exito");

                //se cierran formularios
                cerrarFormuarios();
                //se cierra sesion
                if (Utilidades.clsUsuario.salir())
                {
                    sinSesion();
                }
                else
                {
                    MessageBox.Show(Utilidades.clsUsuario.error);
                }
               
            }
        }

        private void autorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Programa hecho por Kevin huamani https://www.paypal.me/kevinjho.");
        }

        private void btncerrar_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnmax_Click(object sender, EventArgs e)
        {
            btnmax.Visible = false;
            btnrestaurar.Visible = true;
            this.WindowState = FormWindowState.Maximized;
        }

        private void btnmin_Click(object sender, EventArgs e)
        {

            this.WindowState = FormWindowState.Minimized;
        }

        private void btnrestaurar_Click(object sender, EventArgs e)
        {
            btnrestaurar.Visible = false;
            btnmax.Visible = true;
            this.WindowState = FormWindowState.Normal;
        }

        private void menuStrip1_MouseMove(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void tmOcultarMenu_Tick(object sender, EventArgs e)
        {
            if (panel1.Width <= 60)
            {
                tmOcultarMenu.Enabled = false;
            }
            else 
            {
                panel1.Width = panel1.Width - 20;
            }
        }

        private void tmMostrarMenu_Tick(object sender, EventArgs e)
        {
            if (panel1.Width >= 260)
            {
                tmMostrarMenu.Enabled = false;
            }
            else
            {
                panel1.Width = panel1.Width + 20;
            }
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            if (panel1.Width == 260)
            {
                tmOcultarMenu.Enabled = true;
            }
            else if (panel1.Width == 60)
            {
                tmMostrarMenu.Enabled = true;
            }
        }

        private void button10_Click_1(object sender, EventArgs e)
        {
            panel11.Visible = false;
            if (panel5.Visible == true)
            {
                button11.Location = new Point(0, 495);
                panel6.Location = new Point(55, 545);
                panel10.Location = new Point(0, 495);
                panel5.Visible = false;
            }
            else if (panel5.Visible == false)
            {
                button11.Location = new Point(0, 595);
                panel6.Location = new Point(55, 645);
                panel10.Location = new Point(0, 595);
                panel5.Visible = true;

            }

        }

        private void button11_Click_1(object sender, EventArgs e)
        {
            panel11.Visible = false;
            if (panel5.Visible == true)
            {
                panel6.Location = new Point(55, 645);
            }
            else { panel6.Location = new Point(55, 545); }

            if (panel6.Visible == false)
            {
                //panel6.Location = new Point(55, 645);
                panel6.Visible = true;
            }
            else if (panel6.Visible == true)
            {
                panel6.Visible = false;
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (panel11.Visible == true) { panel11.Visible = false; }
            else if (panel11.Visible == false) { panel11.Visible = true; }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = DateTime.Now.ToLongTimeString();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            GraficoVentas cargar = new GraficoVentas();
            cargar.ShowDialog();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            GraficoVentas cargar = new GraficoVentas();
            cargar.ShowDialog();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            GraficoGanancias cargar = new GraficoGanancias();
            cargar.ShowDialog();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Deudores cargar = new Deudores();
            cargar.ShowDialog();
        }
    }
}
