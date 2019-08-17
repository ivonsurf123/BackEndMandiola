using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HMandiola2.Classes
{
    public class General
    {

        public bool verificarRole(string roleName)
        {
            List<HMandiola2.Models.RolParaSeguridad> rolesUsuario =
                (List<HMandiola2.Models.RolParaSeguridad>)System.Web.HttpContext.Current.Session["rolesUsuario"];

            for (int x = 0; x < rolesUsuario.Count; x++)
            {
                if (roleName == rolesUsuario[x].Role_Name || "Administrador" == rolesUsuario[x].Role_Name)
                {
                    return true;
                }
            }

            return false;
        }
    }
}