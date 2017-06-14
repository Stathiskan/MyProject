using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiveCenterRhodes.Models
{
    public class Service
    {
        public int Id { get; set; }
        public string TypeOfDive { get; set; }
        public decimal Price { get; set; }
    }
}