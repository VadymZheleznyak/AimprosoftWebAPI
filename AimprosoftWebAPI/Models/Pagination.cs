using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AimprosoftWebAPI.Models
{
    public class Pagination
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int? TotalRecords { get; set; }
        public int? TotalPages { get; set; }
    }
}
