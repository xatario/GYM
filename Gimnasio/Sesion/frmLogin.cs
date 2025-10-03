using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gimnasio.Sesion
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
           
            //txtUsuario.Text = "admin";
            //txtPassword.Text = "a";
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string password = txtPassword.Text.Trim();
            //VALIDACIONES
            if (usuario.Equals("") || password.Equals(""))
            {
                MessageBox.Show("Usuario y password son obligatorios");
                return;
            }
            //PROCESO
            if (Utilidades.clsUsuario.login(usuario, password))
            {
                this.Close();
            }
            else
            {
                MessageBox.Show(Utilidades.clsUsuario.error);
                txtUsuario.Text = "USUARIO";
                txtPassword.UseSystemPasswordChar = false;
                txtPassword.Text = "CONTRASEÑA";
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            
        }



        private void txtUsuario_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                txtPassword.Focus();
                e.Handled = true;
            }
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {

                string usuario = txtUsuario.Text.Trim();
                string password = txtPassword.Text.Trim();
                //VALIDACIONES
                if (usuario.Equals("") || password.Equals(""))
                {
                    MessageBox.Show("Usuario y password son obligatorios");
                    e.Handled = true;
                    return;

                }
                //PROCESO
                if (Utilidades.clsUsuario.login(usuario, password))
                {
                    e.Handled = true;
                    this.Close();
                }
                else
                {

                    MessageBox.Show(Utilidades.clsUsuario.error);
                    txtUsuario.Text = "USUARIO";
                    txtPassword.UseSystemPasswordChar = false;
                    txtPassword.Text = "CONTRASEÑA";
                    
                }

            }
            
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }



        private void btncerrar_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void txtUsuario_Enter(object sender, EventArgs e)
        {
            if (txtUsuario.Text == "USUARIO")
            {
                txtUsuario.Text = "";
                txtUsuario.ForeColor = Color.LightGray;
            }
        }

        private void txtUsuario_Leave(object sender, EventArgs e)
        {
            if (txtUsuario.Text == "")
            {
                txtUsuario.Text = "USUARIO";
                txtUsuario.ForeColor = Color.DimGray;
            }
        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            if (txtPassword.Text == "CONTRASEÑA")
            {
                txtPassword.Text = "";
                txtPassword.UseSystemPasswordChar = true;
                txtPassword.ForeColor = Color.LightGray;
            }
        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            if (txtPassword.Text == "")
            {
                txtPassword.Text = "CONTRASEÑA";
                txtPassword.UseSystemPasswordChar = false;
                txtPassword.ForeColor = Color.DimGray;
            }
        }

        private void btnminimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }




    }
}
