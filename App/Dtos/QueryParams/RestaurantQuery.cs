using System.ComponentModel.DataAnnotations;

namespace App.Dtos.QueryParams
{
    public class RestaurantQuery
    {
        public bool? IncludeReviews { get; set; }
        public string? CategoryName { get; set; }
        public string? RestaurantName { get; set; }

        public int PageSize { get; set; }
        public int PageNumber { get; set; }

    }
}
