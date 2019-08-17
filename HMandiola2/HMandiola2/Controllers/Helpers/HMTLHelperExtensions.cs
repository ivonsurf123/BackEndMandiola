using HMandiola2.Data;
using HMandiola2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HMandiola2
{
    public static class HMTLHelperExtensions
    {
        public const string accionConsulta = "consulta";
        public const string accionModificar = "modificar";
        public const string accionCrear = "crear";
        public const string accionEliminar = "eliminar";
        public const string accionMenu = "menu";
        public const string accionSubMenu = "submenu";

        public const string moduloUsuario = "usuario";
        public const string moduloAdmin = "admin";
        public const string moduloConsulta = "consulta";

        public const string cntrlConsecutivo = "consecutivo";

        public static bool MostrarOpcion(this HtmlHelper html, string accion = "", string modulo = "", string controller = "")
        {
            bool respuesta = false;



            var rolesUsuario = System.Web.HttpContext.Current.Session["rolesUsuario"] != null ?
                (List<RolParaSeguridad>)System.Web.HttpContext.Current.Session["rolesUsuario"] : null;

            if (rolesUsuario != null)
            {
                var rol = rolesUsuario.FirstOrDefault();

                switch (rol.Role_Name)
                {
                    case "Administrador":
                        respuesta = true;
                        break;
                    case "Seguridad":
                        if (accion == accionCrear ||
                            accion == accionConsulta ||
                            accion == accionMenu &&
                            modulo == moduloUsuario)
                        {
                            respuesta = true;
                        }
                        break;
                    case "Consecutivo":
                        if (modulo == moduloAdmin && (controller == cntrlConsecutivo || accion == accionMenu))
                        {
                            respuesta = true;
                        }
                        break;
                    case "Mantenimiento":
                        if ((accion == accionCrear ||
                            accion == accionModificar ||
                            accion == accionEliminar ||
                            accion == accionMenu ||
                            accion == accionSubMenu) &&
                            modulo == moduloAdmin)
                        {
                            respuesta = true;
                        }
                        break;
                    case "Consulta":
                        if (modulo == moduloConsulta)
                        {
                            respuesta = true;
                        }
                        break;
                    default:
                        respuesta = false;
                        break;
                }
            }

            return respuesta;

        }
    }
}