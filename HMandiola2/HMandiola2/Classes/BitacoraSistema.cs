using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HMandiola2.Data;
using HMandiola2.Models;
using System.Data.Entity.Validation;

namespace HMandiola2.Classes
{
    public class BitacoraSistema
    {
       public const bool habilitarBitacora = true;
        public static void Guardar( TipoBitacoraModel.Tipo tipo, object data, string codigoRegistro, string descripcion)
        {
            if (habilitarBitacora)
            {
                Data.HotelesEntities db = new HotelesEntities();
                var usuarioLogeado = System.Web.HttpContext.Current.Session["usuarioLogueado"] != null ?
                    (sproc_hoteles_LoginUsuario_Result)System.Web.HttpContext.Current.Session["usuarioLogueado"] : null;
                string usuario = "";
                if (usuarioLogeado != null)
                    usuario = usuarioLogeado.cedula;
                DateTime fechaHora = DateTime.Now;
                string detalle = tipo != TipoBitacoraModel.Tipo.Eliminar ? Newtonsoft.Json.JsonConvert.SerializeObject(data) : null;

                db.sproc_hoteles_InsertBitacora(null, usuario, fechaHora, codigoRegistro, tipo.ToString(), descripcion, detalle);
            }
        }
    }
}