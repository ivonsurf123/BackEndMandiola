using HMandiola2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HMandiola2.Models
{
    public class UsuariosModel : ResponseModel
    {

        public List<sproc_hoteles_GetUsuarios_List_Result> Data { get; set; }
    }


    public class RolUsuariosModel : ResponseModel
    {
        public List<sproc_hoteles_GetRol_Usuario_Por_Cedula_Result> Data { get; set; }
    }
}