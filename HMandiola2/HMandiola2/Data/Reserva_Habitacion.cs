//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HMandiola2.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Reserva_Habitacion
    {
        public int Reserva_ID_Reserva { get; set; }
        public int Habitacion_ID_Habitacion { get; set; }
    
        public virtual Habitacion Habitacion { get; set; }
    }
}