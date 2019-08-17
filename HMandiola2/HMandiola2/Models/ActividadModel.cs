using HMandiola2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HMandiola2.Models
{
    public class ActividadModel : ResponseModel
    {
        public List<sproc_hoteles_GetActividadList_Result> Data { get; set; }
    }
}