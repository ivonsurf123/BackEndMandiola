using HMandiola2.Data;
using HMandiola2.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HMandiola2.Controllers.Helpers;
using Newtonsoft.Json;
using System.Data.Entity.Validation;

namespace HMandiola2.Controllers
{
    public class BitacoraController : AuditBitacoraController
    {
        private HotelesEntities db = null;
        int id = 1;
        List<sproc_hoteles_GetUsuario_Result> usuario = null;
        ErrorController manejoDeErrores = new ErrorController();
        public BitacoraController()
        {
            //usuario = new List<sproc_hoteles_GetUsuario_Result>();
            //using (db = new HotelesEntities())
            //{
            //    usuario = db.sproc_hoteles_GetUsuario("800700912").ToList();
            //}
            //id += 1;
            //var bita = new Bitacora
            //{
            //    ID_Cambio = id,
            //    Usuario_Cedula = usuario[0].Cedula.ToString(),
            //    Fecha = DateTime.Now,
            //    ID_Registro = id.ToString(),
            //    Descripcion = "Registro de prueba",
            //    Tipo = "Agregar",
            //    Detalles = $"Codigo={id}|Número={id}|Descripción=Prueba|Ilustración=SinVistaPrevia.jpg"
            //};
            //GuardarBitacora(bita);

        }

        //[HttpPost]
        //public JsonResult GetDataBitacora(ConsultaBitacoraModel model)
        //{
        //    var listBitacora = AccionBitacora(model.Usuario, model.Fecha, model.Tipo);
        //    return Json(new { data = listBitacora }, JsonRequestBehavior.AllowGet);
        //}
        //// GET: Bitacora
        //public List<sproc_hoteles_GetDataBitacora_Result> AccionBitacora(string usuario, DateTime fecha, string tipo)
        //{
        //    using (db = new HotelesEntities())
        //    {
        //        try
        //        {
        //            List<sproc_hoteles_GetDataBitacora_Result> result = db.sproc_hoteles_GetDataBitacora(usuario, fecha, tipo).ToList();
        //            return result;
        //        }
        //        catch (Exception ex)
        //        {
        //            //Debe controlar la excepcion
        //            //throw;
        //        }
        //    }
        //    return null;
        //}


        public ActionResult ConsultaBitacora()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }


        [System.Web.Http.HttpGet]
        public string filtrar(string usuario = "", string fechaInicio = "", string fechaFinal = "", string tipo = "")
        {
            Data.HotelesEntities db = new HotelesEntities();
            try
            {
                usuario = string.IsNullOrEmpty(usuario) ? null : usuario;
                tipo = string.IsNullOrEmpty(tipo) || tipo == "0" ? null : tipo;
                DateTime? dtFechaInicio = null;
                DateTime? dtFechaFinal = null;

                if(!string.IsNullOrEmpty(fechaInicio) && !string.IsNullOrEmpty(fechaFinal))
                {
                    dtFechaInicio = Convert.ToDateTime(fechaInicio);
                    dtFechaFinal = Convert.ToDateTime(fechaFinal);
                }

                var listaBitacora = db.sproc_hoteles_GetDataBitacora(usuario, dtFechaInicio, dtFechaFinal, tipo).ToList();

                var responseModel = new
                {
                    Success = true,
                    Message = "Lista completa",
                    Data = listaBitacora
                };
                return JsonConvert.SerializeObject(responseModel);
            }
            catch (DbEntityValidationException e)
            {
                manejoDeErrores.GuardarError(e.ToString());
                return JsonConvert.SerializeObject(manejoDeErrores.errorBaseDeDatos(e));
            }
            catch (Exception ex)
            {
                manejoDeErrores.GuardarError(ex.ToString());
                return JsonConvert.SerializeObject(manejoDeErrores.errorGeneral(ex));
            }
        }
    }
}