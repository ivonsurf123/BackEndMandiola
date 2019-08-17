using HMandiola2.Data;
using HMandiola2.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace HMandiola2.Controllers
{
    public class ErrorController : Controller
    {

        public string errorBaseDeDatos(DbEntityValidationException e)
        {
            foreach (var eve in e.EntityValidationErrors)
            {
                Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                foreach (var ve in eve.ValidationErrors)
                {
                    Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                        ve.PropertyName, ve.ErrorMessage);
                }
            }

            var responseModel = new ResponseModel
            {
                Success = false,
                Message = "Error"
            };

            return JsonConvert.SerializeObject(responseModel);
        }

        public string errorGeneral(Exception e)
        {
            var responseModel = new ResponseModel
            {
                Success = false,
                Message = e.ToString()
            };

            return JsonConvert.SerializeObject(responseModel);
        }

        public string errorGeneralUsuario(String e)
        {
            var responseModel = new ResponseModel
            {
                Success = false,
                Message = e
            };

            return JsonConvert.SerializeObject(responseModel);
        }

        // GET: Consecutivos
        public ActionResult Index()
        {
            return View();
        }

        [System.Web.Http.HttpGet]
        public string devuelveErrores(DateTime? fechainicio, DateTime? fechaFin)
        {
            Data.HotelesEntities db = new HotelesEntities();

            if (fechainicio != null)
            {
                fechainicio = ((DateTime)fechainicio).Date;
            }

            if (fechaFin != null)
            {
                fechaFin = ((DateTime)fechaFin).Date;
            }

            List<sproc_hoteles_GetErrorList_Result> listaErrores = db.sproc_hoteles_GetErrorList(fechainicio, fechaFin).ToList();

            var responseModel = new ErrorModel()
            {
                Success = true,
                Message = "Lista completa",
                Data = listaErrores
            };
            return JsonConvert.SerializeObject(responseModel);
        }


        public void GuardarError(String description)
        {
            HotelesEntities db = new HotelesEntities();

            try
            {
                db.sproc_hoteles_InsertError(DateTime.Now, description);
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
        }


        [System.Web.Http.HttpGet]
        public string guardarErrorAjax(string error)
        {
            HotelesEntities db = new HotelesEntities();

            try
            {
                GuardarError(error);
                var responseModel = new ResponseModel
                {
                    Success = true,
                    Message = ""
                };

                return JsonConvert.SerializeObject(responseModel);
            }
            catch (DbEntityValidationException e)
            {
                GuardarError(e.ToString());
                return JsonConvert.SerializeObject(errorBaseDeDatos(e));
            }
            catch (Exception ex)
            {
                GuardarError(ex.ToString());
                return JsonConvert.SerializeObject(errorGeneral(ex));
            }
        }
    }
}