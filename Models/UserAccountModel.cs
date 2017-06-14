using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiveCenterRhodes.Models
{
    public class UserAccountModel
    {
        public User user;
        public List<Booking> userBookings;

        public UserAccountModel(User u, List<Booking> bookings)
        {
            user = u;
            userBookings = bookings;
        }

        public decimal TotalPrice()
        {
            decimal price = 0;
            foreach (var p in userBookings)
            {
                if (p.Paid != true)
                {
                    price += p.TotalPrice;
                }
                
            }
            return price;
        }
    }
}