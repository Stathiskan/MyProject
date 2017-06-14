using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DiveCenterRhodes.Models
{
    public class Booking
    {
        public int Id { get; set; }


        
        public string Email { get; set; }
        [ForeignKey("Email")]
        public User User { get; set; }


        
        public int ServiceId { get; set; }
        [ForeignKey("ServiceId")]
        public Service Service { get; set; }

        
        public int DiveSpotId { get; set; }
        [ForeignKey("DiveSpotId")]
        public DiveSpot DiveSpot { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public int NumOfDives { get; set; }

        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public decimal ShoeNumber { get; set; }

        public decimal TotalPrice { get; set; }

        // Added by the admin
        public bool? Paid { get; set; } // nullable FK

        // Instructor is added by the admin
        public int? InstructorId { get; set; } // nullable FK
        [ForeignKey("InstructorId")]
        public Instructor Instructor { get; set; }
    }
}