using HMandiola2.Data;
using HMandiola2.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HMandiola2.Controllers
{
    public class HabitacionesListasController : Controller
    {
        ErrorController manejoDeErrores = new ErrorController();

        // GET: Consecutivos
        public ActionResult Index()
        {
            return View();
        }


        [System.Web.Http.HttpGet]
        public string devuelveHabitaciones()
        {
            Data.HotelesEntities db = new HotelesEntities();
            try
            {
                List<sproc_hoteles_GetHabitacionList_Result> listaHabitacion = db.sproc_hoteles_GetHabitacionList().ToList();

                var responseModel = new HabitacionModel()
                {
                    Success = true,
                    Message = "Lista completa",
                    Data = listaHabitacion
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