using HMandiola2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HMandiola2.Models
{
    public class RolesModel : ResponseModel
    {
        public List<sproc_hoteles_GetRolList_Result> Data { get; set; }
    }

    public class RolParaSeguridad
    {
        public int Id_Rol { get; set; }
        public string Role_Name { get; set; }
    }
}