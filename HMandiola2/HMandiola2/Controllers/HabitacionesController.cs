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
    public class HabitacionesController : Controller
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

        [System.Web.Http.HttpGet]
        public ActionResult Borrar(int? habitacion_id)
        {
            Data.HotelesEntities db = new HotelesEntities();
            db.sproc_hoteles_DeleteHabitacion(habitacion_id);

            BitacoraSistema.Guardar(TipoBitacoraModel.Tipo.Eliminar, habitacion_id, habitacion_id.ToString(), "Eliminación de Habitación");

            return RedirectToAction("Index", "Habitaciones");
        }


        public ActionResult HabitacionesPage(int? habitacion_id)
        {
            if (habitacion_id == null)
            {
                return View();
            }
            else
            {
                Data.HotelesEntities db = new HotelesEntities();
                List<sproc_hoteles_GetHabitacion_Result> listaHabitaciones = db.sproc_hoteles_GetHabitacion(habitacion_id).ToList();
                ViewData["listaHabitaciones"] = listaHabitaciones;
                return View();
            }
        }


        [System.Web.Http.HttpPost]
        public string insertarHabitacion([FromBody]Habitacion habitacion)
        {
            HotelesEntities db = new HotelesEntities();
            String image = habitacion.Imagen;

            if (image != null)
            {
                image = image.Replace("<img src=\"", "");
                image = image.Replace("\">", "");
                habitacion.Imagen = image;
            }

            try
            {
                throw new Exception("prueba");
                if (habitacion.ID_Habitacion == 0)
                {
                    db.sproc_hoteles_InsertHabitacion(habitacion.NumeroHabitacion, habitacion.Estado, habitacion.CantidadPersonas, habitacion.CamasIndividuales, habitacion.CamasMatrimoniales, habitacion.Observaciones, habitacion.Imagen, habitacion.Tipo_Habitacion);
                    BitacoraSistema.Guardar(TipoBitacoraModel.Tipo.Agregar, habitacion, habitacion.ID_Habitacion.ToString(), "Inserción de Habitación");
                }
                else
                {
                    db.sproc_hoteles_UpdateHabitacion(habitacion.ID_Habitacion, habitacion.NumeroHabitacion, habitacion.Estado, habitacion.Tipo_Habitacion, habitacion.CantidadPersonas, habitacion.CamasIndividuales, habitacion.CamasMatrimoniales, habitacion.Observaciones, habitacion.Imagen);
                    BitacoraSistema.Guardar(TipoBitacoraModel.Tipo.Modificar, habitacion, habitacion.ID_Habitacion.ToString(), "Modificación de Habitación");
                }


                var responseModel = new ResponseModel
                {
                    Success = true,
                    Message = "habitacion registrado"
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