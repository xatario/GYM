using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gimnasio.Socios
{
    class clsMembresiaPago : Utilidades.clsModulo
    {
        dsGimnasio.sociomembresia_pagoRow datos;
        public int idUsuarioLog = 0;
        public int idSocioMembresia = 0, folio = 0;
        public decimal importe = 0;
        public string observacion = "";

        public override bool getDatos(System.Windows.Forms.DataGridView dgv)
        {
            clear();
            bool exito = false;
            try
            {
                dsGimnasioTableAdapters.vwsociomembresia_pagoTableAdapter ta = new dsGimnasioTableAdapters.vwsociomembresia_pagoTableAdapter();
                dsGimnasio.vwsociomembresia_pagoDataTable dt = ta.GetData();
                dgv.DataSource = dt;
                exito = true;
            }
            catch (Exception ex)
            {
                error.Add(ex.Message);
            }

            return exito;
        }

        public bool getDatos(System.Windows.Forms.DataGridView dgv, int idSocioMembresia)
        {
            clear();
            bool exito = false;
            try
            {
                dsGimnasioTableAdapters.vwsociomembresia_pagoTableAdapter ta = new dsGimnasioTableAdapters.vwsociomembresia_pagoTableAdapter();
                dsGimnasio.vwsociomembresia_pagoDataTable dt = ta.GetDataByIdSocioMembresia(idSocioMembresia);
                dgv.DataSource = dt;
                exito = true;
            }
            catch (Exception ex)
            {
                error.Add(ex.Message);
            }

            return exito;
        }

        public override bool getDatos(int id)
        {
            throw new NotImplementedException();
        }

        public override bool add()
        {
            clear();
            bool exito = false;
            try
            {
                dsGimnasioTableAdapters.sociomembresia_pagoTableAdapter ta = new dsGimnasioTableAdapters.sociomembresia_pagoTableAdapter();
                ta.add(observacion,folio,idSocioMembresia,idUsuarioLog,importe);

                exito = true;
            }
            catch (Exception ex)
            {
                error.Add(ex.Message);
            }

            return exito;
        }

        public override bool changeState(int newState, int id)
        {
            clear();
            bool exito = false;
            try
            {
                dsGimnasioTableAdapters.sociomembresia_pagoTableAdapter ta = new dsGimnasioTableAdapters.sociomembresia_pagoTableAdapter();
                ta.cambiaEstado(newState, id);

                exito = true;
            }
            catch (Exception ex)
            {
                error.Add(ex.Message);
            }

            return exito;
        }

       

        public override bool edit(int id)
        {
            throw new NotImplementedException();
        }

        public override bool search(System.Windows.Forms.DataGridView dgv, int campo, string valor)
        {
            throw new NotImplementedException();
        }


        #region metodos de reportes
        public bool getDatosRptPagos(System.Windows.Forms.DataGridView dgv, DateTime fecha1, DateTime fecha2)
        {
            clear();
            bool exito = false;
            try
            {
                dsGimnasioTableAdapters.rpt_vwsociomembresia_pagoTableAdapter ta = new dsGimnasioTableAdapters.rpt_vwsociomembresia_pagoTableAdapter();
                dsGimnasio.rpt_vwsociomembresia_pagoDataTable dt = ta.GetDataByFecha(fecha1, fecha2);
                dgv.DataSource = dt;
                exito = true;
            }
            catch (Exception ex)
            {
                error.Add(ex.Message);
            }

            return exito;
        }

        #endregion
    }
}
