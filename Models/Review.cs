using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DiveCenterRhodes.Models
{
    public class Review
    {
        public int Id { get; set; }

        
        public string Email { get; set; }
        [ForeignKey("Email")]
        public User User { get; set; }

        public string ReviewText { get; set; }

        // stars must be in range of 5.
        public int Star { get; set; }

        public int DiveSpotId { get; set; }
        [ForeignKey("DiveSpotId")]
        public DiveSpot DiveSpot { get; set; }
    }
}