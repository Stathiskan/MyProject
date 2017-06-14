using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DiveCenterRhodes.Models
{
    public class User
    {
        [Key]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        public string Level { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Country { get; set; }
        public string Address { get; set; }
        public int PostalCode { get; set; }
        public string MedicalHistory { get; set; }

        public bool Admin { get; set; }

        public int? CouponId { get; set; }
        [ForeignKey("CouponId")]
        public Coupon Coupon { get; set; }
    }
}