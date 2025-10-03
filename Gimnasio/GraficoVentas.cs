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

namespace Gimnasio
{
    public partial class GraficoVentas : Form
    {
        int total = 0;
        public GraficoVentas()
        {
            InitializeComponent();
            
        }

        private void GraficoVentas_Load(object sender, EventArgs e)
        {

            total = 0;
            comboBox1.Text = DateTime.Now.ToString("yyyy");
            int año = int.Parse(comboBox1.SelectedItem.ToString());
            DateTime fecha = new DateTime(año, 01, 01);
            DateTime fecha_final = new DateTime(año+1, 01, 01);

            string Strdate = fecha.ToString("yyyy/MM/dd");
            string Strdate_final = fecha_final.ToString("yyyy/MM/dd");

            MySqlCommand comando = new MySqlCommand(String.Format("SET lc_time_names = 'es_ES'; Select COUNT(idsocio), MONTHNAME(fechaCreacion) from sociomembresia where fechaCreacion >= '" + Strdate + "'and fechaCreacion <='" + Strdate_final + "' and (idEstado=1 or idEstado=4) GROUP BY WEEK(fechaCreacion)"), conexionbd.ObtenerConexion());
            MySqlDataReader reader = comando.ExecuteReader();

            
            while (reader.Read()) 
            {
                string ventas = reader["COUNT(idsocio)"].ToString();
                string mes = reader["MONTHNAME(fechaCreacion)"].ToString();
                total = total + reader.GetInt16("COUNT(idsocio)");

                chart1.Series["Series1"].LegendText = "TOTAL DE MEMBRESIAS VENDIDAS " + total + " ";
                chart1.Series["Series1"].Points.AddXY(mes, ventas);
                

            } 

        }

        private void button1_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
            total = 0;
            int año = int.Parse(comboBox1.SelectedItem.ToString());
            DateTime fecha = new DateTime(año, 01, 01);
            DateTime fecha_final = new DateTime(año+1, 01, 01);

            string Strdate = fecha.ToString("yyyy/MM/dd");
            string Strdate_final = fecha_final.ToString("yyyy/MM/dd");

            MySqlCommand comando = new MySqlCommand(String.Format("SET lc_time_names = 'es_ES'; Select COUNT(idsocio), MONTHNAME(fechaCreacion) from sociomembresia where fechaCreacion >= '" + Strdate + "'and fechaCreacion <='" + Strdate_final + "' and (idEstado=1 or idEstado=4) GROUP BY WEEK(fechaCreacion)"), conexionbd.ObtenerConexion());
            MySqlDataReader reader = comando.ExecuteReader();


            while (reader.Read())
            {
                string ventas = reader["COUNT(idsocio)"].ToString();
                string mes = reader["MONTHNAME(fechaCreacion)"].ToString();
                total = total + reader.GetInt16("COUNT(idsocio)");
                
                chart1.Series["Series1"].LegendText = "TOTAL DE MEMBRESIAS VENDIDAS " + total + " ";
                chart1.Series["Series1"].Points.AddXY(mes, ventas);


            } 


        }
    }
}
