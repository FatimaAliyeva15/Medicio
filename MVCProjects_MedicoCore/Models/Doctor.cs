﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCProjects_MedicoCore.Models
{
    public class Doctor: BaseEntity
    {
        [Required]
        [MinLength(10)]
        [MaxLength(50)]
        public string FullName { get; set; }
        [Required]
        [MinLength(10)]
        [MaxLength(50)]
        public string Position { get; set; }
        public string? ImgUrl { get; set; }
        [NotMapped]
        public IFormFile? ImgFile { get; set; }=null!;
    }
}
