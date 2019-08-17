using HMandiola2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HMandiola2.Models
{
    public class PrecioModel : ResponseModel
    {
        public List<sproc_hoteles_GetPrecioList_Result> Data { get; set; }
    }
}