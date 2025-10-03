using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Gimnasio
{
    public class conexionbd
    {
        public static int x = 1;
        public static MySqlConnection ObtenerConexion()
        {

            MySqlConnection conectar = new MySqlConnection("server=localhost;database=gym;Uid=root;pwd=;");

            
            Console.WriteLine(x++);
            

            try { conectar.Open(); Console.WriteLine("SE HA HABIERTO UNA CONEXION A LA BASE DE DATOS");}

                 
            catch (Exception ex) { Console.WriteLine("error " + ex.Message); }
            return conectar;

        }
    }
}
