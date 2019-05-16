using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Restopedia.Models
{
    public class RestaurantViewModel
    {
        public int RestaurantId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string Image { get; set; }

        public string Description { get; set; }
        public int CountOfReviews { get; set; }
        public decimal? AverageRating { get; set; }
    }
}