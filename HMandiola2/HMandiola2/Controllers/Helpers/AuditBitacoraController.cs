using HMandiola2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HMandiola2.Controllers.Helpers
{
    public class AuditBitacoraController : Controller
    {
        private HotelesEntities db = null;

        public void GuardarBitacora(Bitacora model)
        {
            try
            {
                using (db = new HotelesEntities())
                {
                    db.sproc_hoteles_InsertBitacora(model.ID_Cambio, model.Usuario_Cedula, model.Fecha, model.ID_Registro, model.Tipo, model.Descripcion, model.Detalles);
                }
            }
            catch (Exception ex)
            {
                //
                //throw;
            }
        }
    }
}