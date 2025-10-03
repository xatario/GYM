using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gimnasio.Reportes
{
    class Cliente
    {
        public int Id { get; set; }
        public string Asesor { get; set; }
        public string Nombre { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; }
        public string Telefono { get; set; }




        public Cliente() { }

        public Cliente(int pId, string Nombre, string paterno, string Materno, string Telefono, string Asesor)
        {
            this.Id = pId;
            this.Asesor = Asesor;
            this.Nombre = Nombre;
            this.Paterno = paterno;
            this.Materno = Materno;
            this.Telefono = Telefono;
        }



    }
    class reporte
    {
        public int Id { get; set; }

        public string Asesor { get; set; }
        public string Membresia { get; set; }
        public string Meses { get; set; }

        public string Nombre { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; }
        public string Estado { get; set; }

        public string Pago { get; set; }
        public string Deuda { get; set; }
        public string fechapago { get; set; }
        public string precio { get; set; }

        public reporte() { }

        public reporte(int pId, string Nombre, string paterno, string Materno, string Estado, string Asesor, string Membresia, string Meses, string pago, string deuda,
                        string fechapago, string precio)
        {
            this.Membresia = Membresia;
            this.Meses = Meses;

            this.Id = pId;
            this.Asesor = Asesor;
            this.Nombre = Nombre;
            this.Paterno = paterno;
            this.Materno = Materno;
            this.Estado = Estado;

            this.Pago = pago;
            this.Deuda = deuda;
            this.fechapago = fechapago;
            this.precio = precio;
        }



    }
    class usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public usuario() { }

        public usuario(int pId, string Nombre, string paterno)
        {
            this.Id = pId;
            this.Nombre = Nombre;

        }
    }
}
