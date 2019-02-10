using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AimprosoftWebAPI.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string KindOfWork { get; set; }
        [Required]
        public string City { get; set; }

        public bool HasOfficialEmployment { get; set; }

        public List<Employee> Employees { get; set; }
    }
}