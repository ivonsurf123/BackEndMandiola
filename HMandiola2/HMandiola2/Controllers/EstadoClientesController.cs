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
    public class EstadoClientesController : Controller
    {
        ErrorController manejoDeErrores = new ErrorController();

        // GET: EstadoClientes
        public ActionResult Index()
        {
            return View();
        }

        [System.Web.Http.HttpGet]
        public string devuelveClientes()
        {
            Data.HotelesEntities db = new HotelesEntities();
            try
            {
                var listaClientes = db.sproc_hoteles_GetClienteList().ToList();

                var responseModel = new ClienteModel()
                {
                    Success = true,
                    Message = "Lista completa",
                    Data = listaClientes
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

        public ActionResult ConsultaClienteEstado(string cedula)
        {
            if (string.IsNullOrEmpty(cedula))
            {
                return View();
            }
            else
            {
                Data.HotelesEntities db = new HotelesEntities();
                var listaClienteEstado = db.sproc_hoteles_GetClienteEstado(cedula).ToList();
                ViewData["listaClienteEstado"] = listaClienteEstado;
                return View();
            }
        }
    }
}