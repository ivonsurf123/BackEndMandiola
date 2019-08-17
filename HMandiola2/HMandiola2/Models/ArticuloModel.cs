using HMandiola2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HMandiola2.Models
{
    public class ArticuloModel : ResponseModel
    {
        public List<sproc_hoteles_GetArticuloList_Result> Data { get; set; }
    }
}