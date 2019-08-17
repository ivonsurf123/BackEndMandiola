using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HMandiola2.Models
{
    public class ResponseModel
    {
        public Boolean Success { get; set; }
        public String Message { get; set; }
        public String Error { get; set; }
    }
}