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
    public class ClientesActivosController : Controller
    {
        // GET: ClientesActivos
        ErrorController manejoDeErrores = new ErrorController();

        public ActionResult Index()
        {
            return View();
        }


        [System.Web.Http.HttpGet]
        public string devuelveClientes(string tipo = "")
        {
            Data.HotelesEntities db = new HotelesEntities();
            try
            {
                tipo = string.IsNullOrEmpty(tipo) ? null : tipo;
                var listaCliente = db.sproc_hoteles_GetClientesActivos(tipo).ToList();

                var responseModel = new
                {
                    Success = true,
                    Message = "Lista completa",
                    Data = listaCliente
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