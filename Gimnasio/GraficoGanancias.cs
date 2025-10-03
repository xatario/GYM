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
    public partial class GraficoGanancias : Form
    {
        int total = 0;
        public GraficoGanancias()
        {
            InitializeComponent();
        }

        private void GraficoGanancias_Load(object sender, EventArgs e)
        {

            comboBox1.Text = DateTime.Now.ToString("MMMM");
            comboBox2.Text = DateTime.Now.ToString("yyyy");
            string nombre_mes = comboBox1.Text;
            int año = int.Parse(comboBox2.SelectedItem.ToString());
            int mes1;

            switch (nombre_mes)
            {
                case "Enero":
                    nombre_mes = "1";
                    break;
                case "Febrero":
                    nombre_mes = "2";
                    break;
                case "Marzo":
                    nombre_mes = "3";
                    break;
                case "Abril":
                    nombre_mes = "4";
                    break;
                case "Mayo":
                    nombre_mes = "5";
                    break;
                case "Junio":
                    nombre_mes = "6";
                    break;
                case "Julio":
                    nombre_mes = "7";
                    break;
                case "Agosto":
                    nombre_mes = "8";
                    break;
                case "Septiembre":
                    nombre_mes = "9";
                    break;
                case "Octubre":
                    nombre_mes = "10";
                    break;
                case "Noviembre":
                    nombre_mes = "11";
                    break;
                case "Diciembre":
                    nombre_mes = "12";
                    break;

            }

            mes1 = Convert.ToInt32(nombre_mes);
            DateTime fecha = new DateTime(año, mes1, 01);
            DateTime fecha_final = new DateTime(año, mes1 + 1, 01);

            string Strdate = fecha.ToString("yyyy/MM/dd");
            string Strdate_final = fecha_final.ToString("yyyy/MM/dd");

            MySqlCommand comando = new MySqlCommand(String.Format("SELECT SUM(importe), membresia,COUNT(idSocio),Precio from vwsociomembresia_pago where fecha >= '" + Strdate + "'and fecha <='" + Strdate_final + "' and (idEstado=1 or idEstado=4) GROUP BY(Precio)"), conexionbd.ObtenerConexion());
            MySqlDataReader reader = comando.ExecuteReader();

            while (reader.Read())
            {
                string ventas = reader["SUM(importe)"].ToString();
                string mes = reader["precio"].ToString();
                total = total + reader.GetInt16("SUM(importe)");

                chart1.Series["Series1"].LegendText = "TOTAL DE GANANCIA DE " + comboBox1.Text.ToUpper() + " " + total;
                chart1.Series["Series1"].Points.AddXY(mes, ventas);


            } 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            total = 0;
            chart1.Series[0].Points.Clear();
            string nombre_mes = comboBox1.Text;
            int año = int.Parse(comboBox2.SelectedItem.ToString());
            int mes1;

            switch (nombre_mes)
            {
                case "Enero":
                    nombre_mes = "1";
                    break;
                case "Febrero":
                    nombre_mes = "2";
                    break;
                case "Marzo":
                    nombre_mes = "3";
                    break;
                case "Abril":
                    nombre_mes = "4";
                    break;
                case "Mayo":
                    nombre_mes = "5";
                    break;
                case "Junio":
                    nombre_mes = "6";
                    break;
                case "Julio":
                    nombre_mes = "7";
                    break;
                case "Agosto":
                    nombre_mes = "8";
                    break;
                case "Septiembre":
                    nombre_mes = "9";
                    break;
                case "Octubre":
                    nombre_mes = "10";
                    break;
                case "Noviembre":
                    nombre_mes = "11";
                    break;
                case "Diciembre":
                    nombre_mes = "12";
                    break;
                    
            }

            mes1 = Convert.ToInt32(nombre_mes);
            DateTime fecha = new DateTime(año, mes1, 01);
            DateTime fecha_final = new DateTime(año, mes1, 01);

            if (mes1 == 12)
            {
                fecha_final = new DateTime(año, mes1, 01);
            }
            else { fecha_final = new DateTime(año, mes1 + 1, 01); }

            MessageBox.Show(fecha_final.ToString("yyyy/MM/dd"));
            
            string Strdate = fecha.ToString("yyyy/MM/dd");
            string Strdate_final = fecha_final.ToString("yyyy/MM/dd");

            MySqlCommand comando = new MySqlCommand(String.Format("SELECT SUM(importe), membresia,COUNT(idSocio),Precio from vwsociomembresia_pago where fecha >= '" + Strdate + "'and fecha <='" + Strdate_final + "' and (idEstado=1 or idEstado=4) GROUP BY(Precio)"), conexionbd.ObtenerConexion());
            MySqlDataReader reader = comando.ExecuteReader();

            while (reader.Read())
            {
                string ventas = reader["SUM(importe)"].ToString();
                string mes = reader["precio"].ToString();
                total = total + reader.GetInt32("SUM(importe)");

                chart1.Series["Series1"].LegendText = "TOTAL DE GANANCIA DE " + comboBox1.Text.ToUpper() +" " + total ;
                chart1.Series["Series1"].Points.AddXY(mes, ventas);


            } 

        }
    }

}
