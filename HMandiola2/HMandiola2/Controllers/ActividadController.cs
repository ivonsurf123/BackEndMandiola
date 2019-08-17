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
    public class ActividadController : Controller
    {
        ErrorController manejoDeErrores = new ErrorController();

        // GET: Consecutivos
        public ActionResult Index()
        {
            return View();
        }


        [System.Web.Http.HttpGet]
        public string devuelveActividades()
        {
            Data.HotelesEntities db = new HotelesEntities();
            try
            {
                List<sproc_hoteles_GetActividadList_Result> listaActividad = db.sproc_hoteles_GetActividadList().ToList();

                var responseModel = new ActividadModel()
                {
                    Success = true,
                    Message = "Lista completa",
                    Data = listaActividad
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
        public ActionResult Borrar(int? actividad_id)
        {
            Data.HotelesEntities db = new HotelesEntities();
            db.sproc_hoteles_DeleteActividad(actividad_id);

            BitacoraSistema.Guardar(TipoBitacoraModel.Tipo.Eliminar, actividad_id, actividad_id.ToString(), "Eliminación de Actividad");

            return RedirectToAction("Index", "Actividad");
        }


        public ActionResult ActividadPage(int? actividad_id)
        {
            if (actividad_id == null)
            {
                return View();
            }
            else
            {
                Data.HotelesEntities db = new HotelesEntities();
                List<sproc_hoteles_GetActividad_Result> listaActividades = db.sproc_hoteles_GetActividad(actividad_id).ToList();
                ViewData["listaActividades"] = listaActividades;
                return View();
            }
        }


        [System.Web.Http.HttpPost]
        public string insertarActividad([FromBody]Actividad actividad)
        {
            HotelesEntities db = new HotelesEntities();

            String image = actividad.Imagen;

            if (image != null)
            {
                image = image.Replace("<img src=\"", "");
                image = image.Replace("\">", "");
                actividad.Imagen = image;
            }
            try
            {
                if (actividad.ID_Actividad == 0)
                {
                    db.sproc_hoteles_InsertActividad(actividad.Nombre, actividad.Descripcion, actividad.Cupo, actividad.EspaciosDisponibles, actividad.Imagen);

                    BitacoraSistema.Guardar(TipoBitacoraModel.Tipo.Agregar, actividad, actividad.ID_Actividad.ToString(), "Inserción de Actividad");
                }
                else
                {
                    db.sproc_hoteles_UpdateActividad(actividad.ID_Actividad, actividad.Nombre, actividad.Descripcion, actividad.Cupo, actividad.EspaciosDisponibles, actividad.Imagen);

                    BitacoraSistema.Guardar(TipoBitacoraModel.Tipo.Modificar, actividad, actividad.ID_Actividad.ToString(), "Modificación de Actividad");

                }


                var responseModel = new ResponseModel
                {
                    Success = true,
                    Message = "Actividad registrado"
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