﻿using Graduation_project.Enums;
using System.ComponentModel.DataAnnotations;

namespace Graduation_project.DTO.Service
{
    public class ServiceUpdateDTO
    {

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Title { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }
        public ServiceType Type { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        public decimal Budget { get; set; }
        public IFormFile? ImageFile { get; set; }

    }


}
