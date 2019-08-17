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
    public class ArticuloController : Controller
    {
        ErrorController manejoDeErrores = new ErrorController();

        // GET: Consecutivos
        public ActionResult Index()
        {
            return View();
        }


        [System.Web.Http.HttpGet]
        public string devuelveArticulo()
        {
            Data.HotelesEntities db = new HotelesEntities();
            try
            {
                List<sproc_hoteles_GetArticuloList_Result> listaArticulo = db.sproc_hoteles_GetArticuloList().ToList();

                var responseModel = new ArticuloModel()
                {
                    Success = true,
                    Message = "Lista completa",
                    Data = listaArticulo
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
        public ActionResult Borrar(int? articulo_id)
        {
            Data.HotelesEntities db = new HotelesEntities();
            db.sproc_hoteles_DeleteArticulo(articulo_id);

            BitacoraSistema.Guardar(TipoBitacoraModel.Tipo.Eliminar, articulo_id, articulo_id.ToString(), "Eliminacionde Articulo");

            return RedirectToAction("Index", "Articulo");
        }


        public ActionResult ArticuloPage(int? articulo_id)
        {
            if (articulo_id == null)
            {
                return View();
            }
            else
            {
                Data.HotelesEntities db = new HotelesEntities();
                List<sproc_hoteles_GetArticulo_Result> listaArticulos = db.sproc_hoteles_GetArticulo(articulo_id).ToList();
                ViewData["listaArticulos"] = listaArticulos;
                return View();
            }
        }


        [System.Web.Http.HttpPost]
        public string insertarArticulo([FromBody]Articulo articulo)
        {
            HotelesEntities db = new HotelesEntities();

            String image = articulo.Imagen;

            if (image != null)
            {
                image = image.Replace("<img src=\"", "");
                image = image.Replace("\">", "");
                articulo.Imagen = image;
            }
            try
            {
                if (articulo.ID_Articulo == 0)
                {
                    db.sproc_hoteles_InsertArticulo(articulo.Nombre, articulo.Descripcion, articulo.Cantidad, articulo.Imagen);

                    BitacoraSistema.Guardar(TipoBitacoraModel.Tipo.Agregar, articulo, articulo.ID_Articulo.ToString(), "insertar Articulo");
                }
                else
                {
                    db.sproc_hoteles_UpdateArticulo(articulo.ID_Articulo, articulo.Nombre, articulo.Descripcion, articulo.Cantidad, articulo.Imagen);

                    BitacoraSistema.Guardar(TipoBitacoraModel.Tipo.Modificar, articulo, articulo.ID_Articulo.ToString(), "actualizar Articulo");
                }


                var responseModel = new ResponseModel
                {
                    Success = true,
                    Message = "Articulo registrado"
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
