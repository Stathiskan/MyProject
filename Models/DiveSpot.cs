using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DiveCenterRhodes.Models
{
    public class DiveSpot
    {
        public int Id { get; set; }

        public string Place { get; set; }
        public string Level { get; set; }

    }
}