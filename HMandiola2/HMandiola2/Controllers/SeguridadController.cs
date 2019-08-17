using HMandiola2.Classes;
using System.Data.Objects;
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
    public class SeguridadController : Controller
    {
        ErrorController manejoDeErrores = new ErrorController();
        // GET: Seguridad
        public ActionResult VerUsuarios()
        {
            return View();
        }

        public ActionResult CrearUsuario()
        {
            return View();
        }

        [System.Web.Http.HttpGet]
        public string devuelveUsuarios()
        {
            HotelesEntities db = new HotelesEntities();
            try
            {
                List<sproc_hoteles_GetUsuarios_List_Result> listUsers = db.sproc_hoteles_GetUsuarios_List().ToList();

                var responseModel = new UsuariosModel
                {
                    Success = true,
                    Message = "Lista completa",
                    Data = listUsers
                };
                return JsonConvert.SerializeObject(responseModel);
            }
            catch (DbEntityValidationException e)
            {
                return JsonConvert.SerializeObject(manejoDeErrores.errorBaseDeDatos(e));
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(manejoDeErrores.errorGeneral(ex));
            }
        }




        [System.Web.Http.HttpGet]
        public string devuelveIdRoles()
        {
            HotelesEntities db = new HotelesEntities();
            try
            {
                List<sproc_hoteles_GetRolList_Result> listUsers = db.sproc_hoteles_GetRolList(1).ToList();

                var responseModel = new RolesModel
                {
                    Success = true,
                    Message = "Lista completa",
                    Data = listUsers
                };
                return JsonConvert.SerializeObject(responseModel);
            }
            catch (DbEntityValidationException e)
            {
                return JsonConvert.SerializeObject(manejoDeErrores.errorBaseDeDatos(e));
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(manejoDeErrores.errorGeneral(ex));
            }
        }

        [System.Web.Http.HttpPost]
        public string devuelveRolesPorUsuario(Rol_Usuario rol_Usuario)
        {
            HotelesEntities db = new HotelesEntities();
            try
            {
                List<sproc_hoteles_GetRol_Usuario_Por_Cedula_Result> listUsers = db.sproc_hoteles_GetRol_Usuario_Por_Cedula(rol_Usuario.Usuario_Cedula).ToList();

                var responseModel = new RolUsuariosModel
                {
                    Success = true,
                    Message = "Lista completa",
                    Data = listUsers
                };
                return JsonConvert.SerializeObject(responseModel);
            }
            catch (DbEntityValidationException e)
            {
                return JsonConvert.SerializeObject(manejoDeErrores.errorBaseDeDatos(e));
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(manejoDeErrores.errorGeneral(ex));
            }
        }

        [System.Web.Http.HttpPost]
        public string eliminarRolesDeUsuario(Rol_Usuario rol_Usuario_eliminar)
        {
            HotelesEntities db = new HotelesEntities();
            try
            {
                db.sproc_hoteles_DeleteRol_Usuario(rol_Usuario_eliminar.Usuario_Cedula);

                BitacoraSistema.Guardar(TipoBitacoraModel.Tipo.Eliminar, rol_Usuario_eliminar, rol_Usuario_eliminar.Rol_ID_Rol.ToString(), "Eliminación de Rol");


                var responseModel = new RolUsuariosModel
                {
                    Success = true
                };
                return JsonConvert.SerializeObject(responseModel);
            }
            catch (DbEntityValidationException e)
            {
                return JsonConvert.SerializeObject(manejoDeErrores.errorBaseDeDatos(e));
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(manejoDeErrores.errorGeneral(ex));
            }
        }


        [System.Web.Http.HttpPost]
        public string insertaRolesPorUsuario(Rol_Usuario rol_Usuario)
        {
            HotelesEntities db = new HotelesEntities();
            try
            {
                db.sproc_hoteles_InsertRol_Usuario(rol_Usuario.Rol_ID_Rol, rol_Usuario.Usuario_Cedula);

                BitacoraSistema.Guardar(TipoBitacoraModel.Tipo.Agregar, rol_Usuario, rol_Usuario.ID_Rol_Usuario.ToString(), "Inserción de Rol");


                var responseModel = new RolUsuariosModel
                {
                    Success = true
                };
                return JsonConvert.SerializeObject(responseModel);
            }
            catch (DbEntityValidationException e)
            {
                return JsonConvert.SerializeObject(manejoDeErrores.errorBaseDeDatos(e));
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(manejoDeErrores.errorGeneral(ex));
            }
        }


        [System.Web.Http.HttpPost]
        public string RegistrarUsuario([FromBody]Usuario usuario, string repetir_contrasena)
        {
            HotelesEntities db = new HotelesEntities();

            try
            {
                db.sproc_hoteles_InsertUsuario(usuario.Cedula, usuario.Nombre, usuario.PrimerApellido, usuario.SegundoApellido, usuario.Correo, usuario.Contrasena, usuario.PreguntaSeguridad, usuario.RespuestaSeguridad);

                BitacoraSistema.Guardar(TipoBitacoraModel.Tipo.Agregar, usuario, usuario.Cedula, "Inserción de Usuario");


                List<Rol> listRoles = db.Rols.ToList();
                int idRoleConsulta = 0;
                for (int x = 0; x < listRoles.Count; x++)
                {
                    if (listRoles[x].Descripcion == "Consulta")
                    {
                        idRoleConsulta = listRoles[x].ID_Rol;
                        break;
                    }
                }

                db.sproc_hoteles_InsertRol_Usuario(idRoleConsulta, usuario.Cedula);

                var responseModel = new ResponseModel
                {
                    Success = true,
                    Message = "Usuario registrado"
                };

                return JsonConvert.SerializeObject(responseModel);
            }
            catch (DbEntityValidationException e)
            {
                return JsonConvert.SerializeObject(manejoDeErrores.errorBaseDeDatos(e));
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(manejoDeErrores.errorGeneral(ex));
            }
        }
        [System.Web.Http.HttpPost]
        public JsonResult validarContrasena(String contrasenaActual)
        {
            String correo = System.Web.HttpContext.Current.Session["Correo"].ToString();
            String resultado = String.Empty;
            String mensaje = String.Empty;
            Boolean valido;

            try
            {
                using (HotelesEntities db = new HotelesEntities())
                {
                    ObjectParameter IsPasswordValid = new ObjectParameter("IsPasswordValid", typeof(Boolean));
                    var result = db.sproc_hoteles_ValidarContrasena(correo, contrasenaActual, IsPasswordValid);
                    valido = (Boolean)IsPasswordValid.Value;
                }

                if (valido)
                {
                    resultado = "OK";
                    mensaje = String.Empty;
                }
                else
                {
                    resultado = "ERROR";
                    mensaje = "La contraseña no coincide con la contraseña actual";
                }

                return Json(new { Result = resultado, Message = mensaje });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [System.Web.Http.HttpPost]
        public JsonResult cambiarContrasena(String contrasenaActual, String contrasenaNueva)
        {
            String correo = System.Web.HttpContext.Current.Session["Correo"].ToString();
            String resultado = String.Empty;
            String mensaje = String.Empty;
            Boolean cambiada;

            try
            {
                using (HotelesEntities db = new HotelesEntities())
                {
                    ObjectParameter IsPasswordChanged = new ObjectParameter("IsPasswordChanged", typeof(Boolean));
                    var result = db.sproc_hoteles_CambiarContrasena(correo, contrasenaActual, contrasenaNueva, IsPasswordChanged);
                    cambiada = (Boolean)IsPasswordChanged.Value;

                    BitacoraSistema.Guardar(TipoBitacoraModel.Tipo.Modificar, new { ContrasenaActual = contrasenaActual, ContrasenaNueva = contrasenaNueva }, correo, "Modificación de Contraseña");

                }

                if (cambiada)
                {
                    resultado = "OK";
                    mensaje = String.Empty;
                }
                else
                {
                    resultado = "ERROR";
                    mensaje = "No se pudo cambiar la contraseña.";
                }

                return Json(new { Result = resultado, Message = mensaje });
                
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }


        }

        [System.Web.Http.HttpGet]
        public ActionResult CambiarContrasenia()
        {
            return View();
        }
    }
}