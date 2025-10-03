using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace Gimnasio.Registro
{
    class conexionbd {

        public static MySqlConnection ObtenerConexion()
        {
            MySqlConnection conectar = new MySqlConnection("server=localhost;database=gym;Uid=root;pwd=;");
            try { conectar.Open(); Console.WriteLine("SE HA HABIERTO UNA CONEXION A LA BASE DE DATOS"); }
            catch (Exception ex) { Console.WriteLine("error " + ex.Message); }
            return conectar;

        }

    
    }
    class clsRegistro
    {
         public dsGimnasio.vwultimamembresiadetalladaRow datos;
         public string error = "";

         int clave = 0;
         public clsRegistro(int clave_)
         {
             clave = clave_;
         }

         public bool buscaDatos()
         {
             bool exito = false;
             try
             {
                 dsGimnasioTableAdapters.vwultimamembresiadetalladaTableAdapter ta = new dsGimnasioTableAdapters.vwultimamembresiadetalladaTableAdapter();   
                 dsGimnasio.vwultimamembresiadetalladaDataTable dt = ta.GetDataByIdSocio(clave);
                 if (dt.Rows.Count > 0)
                 {
                     datos = (dsGimnasio.vwultimamembresiadetalladaRow)dt.Rows[0];
                     exito = true;
                 }
             }
             catch { }
             return exito;
         }

         public bool addVisita()
         {
            
             bool exito = false;
             try
             {
                 dsGimnasioTableAdapters.visitaTableAdapter ta = new dsGimnasioTableAdapters.visitaTableAdapter();
                 decimal precioVisita = 0;
                //si es visita se agrega el precio de la visita
                 if (datos.idSocio <= 1000) precioVisita = obtenerPrecioVisita();

                 ta.add(this.datos.idSocio, DateTime.Now,precioVisita);

                 exito = true;
             }
             catch (Exception ex)
             {
                 error=ex.Message;
             }

             return exito;
         }

         private decimal obtenerPrecioVisita()
         {
             decimal precio = 0;

             dsGimnasioTableAdapters.membresiaTableAdapter ta = new dsGimnasioTableAdapters.membresiaTableAdapter();
             dsGimnasio.membresiaDataTable dt = ta.GetDataByIdMembresia(1);
             dsGimnasio.membresiaRow dr = (dsGimnasio.membresiaRow)dt.Rows[0];

             precio = dr.Precio;
             
             return precio;
         }


    }
}
