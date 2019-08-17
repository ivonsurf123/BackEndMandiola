using HMandiola2.Classes;
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
    public class ConsecutivosController : Controller
    {
        ErrorController manejoDeErrores = new ErrorController();

        // GET: Consecutivos
        public ActionResult Index()
        {
            return View();
        }


        [System.Web.Http.HttpGet]
        public string devuelveConsecutivos()
        {
            Data.HotelesEntities db = new HotelesEntities();
            try
            {
                List<sproc_hoteles_GetConsecutivoList_Result> listaConsecutivos = db.sproc_hoteles_GetConsecutivoList().ToList();

                var responseModel = new ConsecutivoModel()
                {
                    Success = true,
                    Message = "Lista completa",
                    Data = listaConsecutivos
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

        [System.Web.Http.HttpGet]
        public ActionResult Borrar(int? consecutivo_id)
        {
            Data.HotelesEntities db = new HotelesEntities();
            db.sproc_hoteles_DeleteConsecutivo(consecutivo_id);

            BitacoraSistema.Guardar(TipoBitacoraModel.Tipo.Eliminar, consecutivo_id, consecutivo_id.ToString(), "Eliminación de Consecutivo");


            return RedirectToAction("Index", "Consecutivos");
        }


        public ActionResult ConsecutivoPage(int? consecutivo_id)
        {
            if (consecutivo_id == null)
            {
                return View();
            }
            else
            {
                Data.HotelesEntities db = new HotelesEntities();
                List<sproc_hoteles_GetConsecutivo_Result> listaConsecutivos = db.sproc_hoteles_GetConsecutivo(consecutivo_id).ToList();
                ViewData["listaConsecutivos"] = listaConsecutivos;
                return View();
            }
        }


        [System.Web.Http.HttpPost]
        public string insertarConsecutivo([FromBody]Consecutivo consecutivo)
        {
            HotelesEntities db = new HotelesEntities();

            try
            {
                if (consecutivo.ID_Consecutivo == 0)
                {
                    db.sproc_hoteles_InsertConsecutivo(consecutivo.Num_Consecutivo,
                  consecutivo.Posee_Prefijo, consecutivo.Prefijo, consecutivo.Descripcion, consecutivo.Posee_Rango,
                  consecutivo.Rango_Inicial, consecutivo.Rango_Final);

                    BitacoraSistema.Guardar(TipoBitacoraModel.Tipo.Agregar, consecutivo, consecutivo.ID_Consecutivo.ToString(), "Inserción de Consecutivo");

                }
                else
                {
                    db.sproc_hoteles_UpdateConsecutivo(consecutivo.ID_Consecutivo, consecutivo.Num_Consecutivo,
                  consecutivo.Posee_Prefijo, consecutivo.Prefijo, consecutivo.Posee_Rango,
                  consecutivo.Rango_Inicial, consecutivo.Rango_Final, consecutivo.Descripcion);

                    BitacoraSistema.Guardar(TipoBitacoraModel.Tipo.Modificar, consecutivo, consecutivo.ID_Consecutivo.ToString(), "Modificación de Consecutivo");

                }


                var responseModel = new ResponseModel
                {
                    Success = true,
                    Message = "Consecutivo registrado"
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