using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Gimnasio
{
    public partial class Deudores : Form
    {

        public Deudores()
        {
            InitializeComponent();
        }

        private void Deudores_Load(object sender, EventArgs e)
        {
            MySqlCommand comando2 = new MySqlCommand(String.Format("SELECT idSocioMembresia from vwsociomembresia_pago WHERE importe-Precio<>0 and idEstado=(1 or 4) and estadoMembresia='Sin pagar'"), conexionbd.ObtenerConexion());
           // MySqlDataReader reader2 = comando2.ExecuteReader();
            MySqlDataAdapter con = new MySqlDataAdapter(comando2);
            DataSet ds = new DataSet();

            con.Fill(ds);

            //dataGridView1.DataSource = ds.Tables[0];

            
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MySqlCommand comando2 = new MySqlCommand(String.Format("SELECT idSocioMembresia from vwsociomembresia_pago WHERE importe-Precio<>0 and idEstado=(1 or 4) and estadoMembresia='Sin pagar'"), conexionbd.ObtenerConexion());
            MySqlDataReader reader2 = comando2.ExecuteReader();

            List<int> resultado = new List<int>();

            while (reader2.Read())
            {
                int id = reader2.GetInt32(0);
                MySqlCommand comando3 = new MySqlCommand(String.Format("SELECT idSocio,NombreSocio,Paterno,Materno,Precio,estadoMembresia,Observaciones,meses  FROM vwsociomembresias WHERE idSocioMembresia='" + id + "' and idEstado=(1 or 4) "), conexionbd.ObtenerConexion());

                MySqlDataAdapter tabla = new MySqlDataAdapter(comando3);
                

                DataTable dt = new DataTable();
                DataRow row = dt.NewRow();

                resultado.Add(Convert.ToInt32(reader2[0]));



                tabla.Fill(dt);
                dataGridView1.DataSource = dt;

               
            }

            
            reader2.Close();
        }
    }
}