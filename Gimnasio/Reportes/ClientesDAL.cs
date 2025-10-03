using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Gimnasio.Reportes
{
    class ClientesDAL
    {
                public static int Agregar(Cliente pCliente)
        {

            int retorno = 0;
            MySqlCommand comando = new MySqlCommand(string.Format("Insert into socio (Nombre, Paterno, clave, Observaciones) values ('{0}','{1}','{2}', '{3}')",
                pCliente.Nombre, pCliente.Paterno, pCliente.Materno, pCliente.Telefono), conexionbd.ObtenerConexion());
            retorno = comando.ExecuteNonQuery();
            return retorno;
                   
        }
        public static List<Cliente> Buscar(string pNombre, string pApellido)
        {
            List<Cliente> _lista = new List<Cliente>();

            MySqlCommand _comando = new MySqlCommand(String.Format(
                "SELECT idsocio, Nombre, Paterno, Materno, Telefono FROM socio  where Nombre ='{0}' or Paterno='{1}'", pNombre, pApellido), conexionbd.ObtenerConexion());
            MySqlDataReader _reader = _comando.ExecuteReader();


                while (_reader.Read())
                {

                    string idsocio = _reader["idsocio"].ToString() ;
                    MySqlCommand _comando2 = new MySqlCommand(String.Format(
                    "SELECT idUsuarioCreo FROM sociomembresia where idSocio='" + idsocio + "' "), conexionbd.ObtenerConexion());

                    MySqlDataReader _reader2 = _comando2.ExecuteReader();
                    Cliente pCliente = new Cliente();
                    pCliente.Id = _reader.GetInt32(0);

                    while (_reader2.Read())
                    {
                        string idusuario = _reader2["idUsuarioCreo"].ToString();

                        MySqlCommand _comando3 = new MySqlCommand(String.Format(
                        "SELECT Usuario FROM usuario where idUsuario='" + idusuario + "' "), conexionbd.ObtenerConexion());

                        MySqlDataReader _reader3 = _comando3.ExecuteReader();

                        while (_reader3.Read())
                        {
                            Console.Write("LLEGA A READER 3");
                            pCliente.Asesor = _reader3.GetString(0);
                            Console.WriteLine(_reader3[0]);
                            Console.WriteLine("PASO POR AQUI 11111111");
                        }
                    }

                    pCliente.Nombre = _reader.GetString(1);
                    pCliente.Paterno = _reader.GetString(2);
                    pCliente.Materno = _reader.GetString(3);
                    pCliente.Telefono = _reader.GetString(4);


                    _lista.Add(pCliente);
                }
            


            return _lista;
        }

        public static List<Cliente> mostrar(string pNombre, string pApellido)
        {
            List<Cliente> _lista = new List<Cliente>();

            MySqlCommand _comando = new MySqlCommand(String.Format(
                "SELECT idsocio, Nombre, Paterno, Materno, Telefono FROM socio "), conexionbd.ObtenerConexion());
            MySqlDataReader _reader = _comando.ExecuteReader();


            while (_reader.Read())
            {

                string idsocio = _reader["idsocio"].ToString();
                MySqlCommand _comando2 = new MySqlCommand(String.Format(
                "SELECT idUsuarioCreo FROM sociomembresia where idSocio='" + idsocio + "' "), conexionbd.ObtenerConexion());

                MySqlDataReader _reader2 = _comando2.ExecuteReader();
                Cliente pCliente = new Cliente();
                pCliente.Id = _reader.GetInt32(0);

                while (_reader2.Read())
                {
                    string idusuario = _reader2["idUsuarioCreo"].ToString();

                    MySqlCommand _comando3 = new MySqlCommand(String.Format(
                    "SELECT Usuario FROM usuario where idUsuario='" + idusuario + "' "), conexionbd.ObtenerConexion());

                    MySqlDataReader _reader3 = _comando3.ExecuteReader();

                    while (_reader3.Read())
                    {
                        Console.Write("LLEGA A READER 3");
                        pCliente.Asesor = _reader3.GetString(0);
                        Console.WriteLine(_reader3[0]);
                        Console.WriteLine("PASO POR AQUI 11111111");
                    }
                }

                pCliente.Nombre = _reader.GetString(1);
                pCliente.Paterno = _reader.GetString(2);
                pCliente.Materno = _reader.GetString(3);
                pCliente.Telefono = _reader.GetString(4);


                _lista.Add(pCliente);
            }



            return _lista;
        }


        public static List<reporte> mostrar2(string fecha1, string fecha2, string combo1)
        {
            
            List<reporte> _lista = new List<reporte>();

            MySqlCommand comando01 = new MySqlCommand(String.Format("Select idUsuario FROM usuario where Usuario = '" + combo1 + "' "), conexionbd.ObtenerConexion());
            MySqlDataReader reader01 = comando01.ExecuteReader();
            while (reader01.Read())
            {
                string usuario1 = reader01.GetString(0);
                Console.WriteLine(usuario1);

                MySqlCommand comando = new MySqlCommand(String.Format("Select idsocio from sociomembresia where fechaCreacion >= '" + fecha1 + "'and fechaCreacion <='" + fecha2 + "' and idUsuarioCreo ='" + usuario1 + "'and (idEstado=1 or idEstado=4)  "), conexionbd.ObtenerConexion());
                MySqlDataReader reader = comando.ExecuteReader();

   
                while (reader.Read())
                {
                   
                string idsocio1 = reader["idsocio"].ToString();

                MySqlCommand _comando = new MySqlCommand(String.Format("SELECT idsocio, Nombre, Paterno, Materno, Telefono FROM socio where idSocio='" + idsocio1 + "' and idSocio>1000  "), conexionbd.ObtenerConexion());
                MySqlDataReader _reader = _comando.ExecuteReader();

                
                while (_reader.Read())
                {
                    string idsocio = _reader["idsocio"].ToString();
                    MySqlCommand _comando2 = new MySqlCommand(String.Format("SELECT estadoMembresia,idMembresia,idUsuarioCreo,idSocioMembresia, fechaCreacion FROM sociomembresia where idSocio='" + idsocio + "'  "), conexionbd.ObtenerConexion());
                    MySqlDataReader _reader2 = _comando2.ExecuteReader();
                 
                    reporte pCliente = new reporte();

                    while (_reader2.Read())
                    {
                        pCliente.Estado = _reader2.GetString(0);
                        pCliente.fechapago = _reader2.GetString(4);

                        // GUARDO LAS VARIABLES USUARIO QUE CREO LA MEMBRESIA , ID MEMBRESIA Y SOCIO MEMBRESIA
                        string usuariocreo = _reader2["idUsuarioCreo"].ToString();
                        string idmembresia = _reader2["idMembresia"].ToString();
                        string idsociomembresia = _reader2["idSocioMembresia"].ToString();

                        MySqlCommand _comando3 = new MySqlCommand(String.Format("SELECT Nombre,meses,precio FROM membresia where idMembresia='" + idmembresia + "'and idEstado=1"), conexionbd.ObtenerConexion());
                        MySqlDataReader _reader3 = _comando3.ExecuteReader();
                        
                        while (_reader3.Read())
                        {
                            
                            pCliente.Membresia = _reader3.GetString(0);
                            pCliente.Meses = _reader3.GetString(1);
                            pCliente.precio = _reader3.GetString(2);

                        }
                        _reader3.Close();
                        
                        MySqlCommand _comando4 = new MySqlCommand(String.Format("SELECT Nombre FROM usuario where idusuario='" + usuariocreo + "'and idEstado=1"), conexionbd.ObtenerConexion());
                        MySqlDataReader _reader4 = _comando4.ExecuteReader();

                        while (_reader4.Read())
                        {
                            pCliente.Asesor = _reader4.GetString(0);
                        }
                        _reader4.Close();
                        MySqlCommand _comando5 = new MySqlCommand(String.Format("SELECT sum(importe)  FROM sociomembresia_pago  where idSocioMembresia ='" + idsociomembresia + "' and idEstado= '" + 1 + "'  "), conexionbd.ObtenerConexion());
                        MySqlDataReader _reader5 = _comando5.ExecuteReader();

                        while (_reader5.Read())
                        {

                            pCliente.Pago = _reader5["sum(importe)"].ToString();

                        }
                        _reader5.Close();
                        MySqlCommand _comando6 = new MySqlCommand(String.Format("SELECT sum(importe)-(SELECT Precio FROM membresia where idMembresia = '" + idmembresia + "')  FROM sociomembresia_pago  where idSocioMembresia ='" + idsociomembresia + "' and idEstado= '" + 1 + "'  "), conexionbd.ObtenerConexion());
                        MySqlDataReader _reader6 = _comando6.ExecuteReader();

                        while (_reader6.Read())
                        {

                            pCliente.Deuda = _reader6["sum(importe)-(SELECT Precio FROM membresia where idMembresia = '" + idmembresia + "')"].ToString();

                        }

                        MySqlConnection.ClearAllPools();
                        _reader6.Close();
                    }

                    _reader2.Close();
                    


                    pCliente.Id = _reader.GetInt32(0);
                    pCliente.Nombre = _reader.GetString(1);
                    pCliente.Paterno = _reader.GetString(2);
                    pCliente.Materno = _reader.GetString(3);
                    // pCliente.Estado = _reader.GetString(4);


                    _lista.Add(pCliente);


                }
                
                _reader.Close();

            }
            reader.Close();
            
           //return _lista;
            }
            
            reader01.Close();
            return _lista;
        }

        public static List<reporte> mostrar3(string fecha1, string fecha2)
        {
            List<reporte> _lista = new List<reporte>();


                MySqlCommand comando = new MySqlCommand(String.Format("Select idsocio from sociomembresia where fechaCreacion >= '" + fecha1 + "'and fechaCreacion <='" + fecha2 + "' and idEstado=1 or idEstado=4  "), conexionbd.ObtenerConexion());
                MySqlDataReader reader = comando.ExecuteReader();

                while (reader.Read())
                {

                    string idsocio1 = reader["idsocio"].ToString();

                    MySqlCommand _comando = new MySqlCommand(String.Format("SELECT idsocio, Nombre, Paterno, Materno, Telefono FROM socio where idSocio='" + idsocio1 + "' and idSocio>1000  "), conexionbd.ObtenerConexion());
                    MySqlDataReader _reader = _comando.ExecuteReader();

                    while (_reader.Read())
                    {
                        string idsocio = _reader["idsocio"].ToString();
                        MySqlCommand _comando2 = new MySqlCommand(String.Format("SELECT estadoMembresia,idMembresia,idUsuarioCreo,idSocioMembresia, fechaCreacion FROM sociomembresia where idSocio='" + idsocio + "' "), conexionbd.ObtenerConexion());
                        MySqlDataReader _reader2 = _comando2.ExecuteReader();

                        reporte pCliente = new reporte();

                        while (_reader2.Read())
                        {
                            pCliente.Estado = _reader2.GetString(0);
                            pCliente.fechapago = _reader2.GetString(4);

                            // GUARDO LAS VARIABLES USUARIO QUE CREO LA MEMBRESIA , ID MEMBRESIA Y SOCIO MEMBRESIA
                            string usuariocreo = _reader2["idUsuarioCreo"].ToString();
                            string idmembresia = _reader2["idMembresia"].ToString();
                            string idsociomembresia = _reader2["idSocioMembresia"].ToString();

                            MySqlCommand _comando3 = new MySqlCommand(String.Format("SELECT Nombre,meses,precio FROM membresia where idMembresia='" + idmembresia + "'and idEstado=1"), conexionbd.ObtenerConexion());
                            MySqlDataReader _reader3 = _comando3.ExecuteReader();

                            while (_reader3.Read())
                            {
                                pCliente.Membresia = _reader3.GetString(0);
                                pCliente.Meses = _reader3.GetString(1);
                                pCliente.precio = _reader3.GetString(2);
                                
                            }
                            _reader3.Close();
                            MySqlCommand _comando4 = new MySqlCommand(String.Format("SELECT Nombre FROM usuario where idusuario='" + usuariocreo + "'and idEstado=1"), conexionbd.ObtenerConexion());
                            MySqlDataReader _reader4 = _comando4.ExecuteReader();

                            while (_reader4.Read())
                            {
                                pCliente.Asesor = _reader4.GetString(0);
                            }
                            _reader4.Close();
                            MySqlCommand _comando5 = new MySqlCommand(String.Format("SELECT sum(importe)  FROM sociomembresia_pago  where idSocioMembresia ='" + idsociomembresia + "' and idEstado= '" + 1 + "'  "), conexionbd.ObtenerConexion());
                            MySqlDataReader _reader5 = _comando5.ExecuteReader();

                            while (_reader5.Read())
                            {
                                pCliente.Pago = _reader5["sum(importe)"].ToString();
                            }
                            _reader5.Close();

                            MySqlCommand _comando6 = new MySqlCommand(String.Format("SELECT sum(importe)-(SELECT Precio FROM membresia where idMembresia = '" + idmembresia + "')  FROM sociomembresia_pago  where idSocioMembresia ='" + idsociomembresia + "' and idEstado= '" + 1 + "'  "), conexionbd.ObtenerConexion());
                            MySqlDataReader _reader6 = _comando6.ExecuteReader();

                            while (_reader6.Read())
                            {
                                pCliente.Deuda = _reader6["sum(importe)-(SELECT Precio FROM membresia where idMembresia = '" + idmembresia + "')"].ToString(); 
                            }
                            MySqlConnection.ClearAllPools();
                            _reader6.Close();
                        }
                        _reader2.Close();
                        
                        pCliente.Id = _reader.GetInt32(0);
                        pCliente.Nombre = _reader.GetString(1);
                        pCliente.Paterno = _reader.GetString(2);
                        pCliente.Materno = _reader.GetString(3);
                        _lista.Add(pCliente);

                    }
                    _reader.Close();
                } 
                reader.Close();
            return _lista;
        }

        public static List<usuario> Obtenerusuarios()
       {
           List<usuario> _lista = new List<usuario>();

               string cadql2 = "SELECT idUsuario, Usuario FROM usuario where idEstado = 1 ";
               MySqlCommand comando2 = new MySqlCommand(cadql2, conexionbd.ObtenerConexion());

               MySqlDataReader leer2 = comando2.ExecuteReader();
               while (leer2.Read())
               {
                   usuario usuario = new usuario();

                   usuario.Id = leer2.GetInt32(0);
                   usuario.Nombre = leer2.GetString(1);

                   _lista.Add(usuario);
               }
               leer2.Close();
           return _lista;
       }

   }    



     }// aqui termina la clase entera
    

