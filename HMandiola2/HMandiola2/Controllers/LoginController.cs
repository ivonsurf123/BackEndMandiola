using HMandiola2.Data;
using HMandiola2.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Validation;

using System.Net.Http;
using System.Web.Http;
using HMandiola2.Classes;
using Newtonsoft.Json;

namespace HMandiola2.Controllers
{
    public class LoginController : Controller
    {
        ErrorController manejoDeErrores = new ErrorController();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Registrar()
        {
            return View();
        }

        [System.Web.Http.HttpPost]
        public string Login(Usuario usuario)
        {
            HotelesEntities db = new HotelesEntities();
            try
            {
                List<sproc_hoteles_LoginUsuario_Result> usuarioLogin = db.sproc_hoteles_LoginUsuario(usuario.Correo, usuario.Contrasena).ToList();
                bool isSuccess = false;
                if (usuarioLogin.Count >= 1)
                {
                    List<sproc_hoteles_GetRol_Usuario_Por_Cedula_Result> rolesUser =
                        db.sproc_hoteles_GetRol_Usuario_Por_Cedula(usuarioLogin[0].cedula).ToList();

                    List<RolParaSeguridad> roles = new List<RolParaSeguridad>();

                    for (int x = 0; x < rolesUser.Count; x++)
                    {
                        RolParaSeguridad roleSeguridad = new RolParaSeguridad();
                        roleSeguridad.Id_Rol = rolesUser[x].Rol_ID_Rol;
                        roleSeguridad.Role_Name = rolesUser[x].Descripcion;
                        roles.Add(roleSeguridad);
                    }

                    System.Web.HttpContext.Current.Session["usuarioLogueado"] = usuarioLogin[0];
                    System.Web.HttpContext.Current.Session["rolesUsuario"] = roles;
					System.Web.HttpContext.Current.Session["Correo"] = usuario.Correo;
                  
                    isSuccess = true;
                }
                else
                {
                    System.Web.HttpContext.Current.Session["usuarioLogueado"] = null;
                }

                var responseModel = new ResponseModel
                {
                    Success = isSuccess
                };

                return JsonConvert.SerializeObject(responseModel);
            }
            catch (Exception ex)
            {
                manejoDeErrores.GuardarError(ex.ToString());
                return JsonConvert.SerializeObject(manejoDeErrores.errorGeneral(ex));
            }
        }

        [System.Web.Http.HttpPost]
        public string RegistrarUsuario([FromBody]UsuarioRegistrarModel usuario)
        {
            HotelesEntities db = new HotelesEntities();


            List<sproc_hoteles_GetUsuario_existente_Result> usuarioExistente = db.sproc_hoteles_GetUsuario_existente(usuario.Cedula, usuario.Correo).ToList();
            if (usuarioExistente.Count > 0)
            {
                manejoDeErrores.GuardarError("Usuario error: el usuario que intenta crear ya se encuentra registrado");
                return JsonConvert.SerializeObject(manejoDeErrores.errorGeneralUsuario("el usuario que intenta crear ya se encuentra registrado"));
            }
            else
            {

                if (usuario.RepetirContrasena == null)
                {
                    usuario.RepetirContrasena = "";
                }
                if (usuario.Contrasena.Equals(usuario.RepetirContrasena))
                {
                    try
                    {
                        db.sproc_hoteles_InsertUsuario(usuario.Cedula, usuario.Nombre, usuario.PrimerApellido, usuario.SegundoApellido, usuario.Correo, usuario.Contrasena, usuario.PreguntaSeguridad, usuario.RespuestaSeguridad);

                        List<sproc_hoteles_GetRolList_Result> listRoles = db.sproc_hoteles_GetRolList(1).ToList();
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
                        manejoDeErrores.GuardarError(e.ToString());
                        return JsonConvert.SerializeObject(manejoDeErrores.errorBaseDeDatos(e));
                    }
                    catch (Exception ex)
                    {
                        manejoDeErrores.GuardarError(ex.ToString());
                        return JsonConvert.SerializeObject(manejoDeErrores.errorGeneral(ex));
                    }
                }
                else
                {
                    manejoDeErrores.GuardarError("Usuario error: Las contraseñas ingresadas no coinciden");
                    return JsonConvert.SerializeObject(manejoDeErrores.errorGeneralUsuario("Las contraseñas ingresadas no coinciden"));
                }

            }
        }
    }
}
