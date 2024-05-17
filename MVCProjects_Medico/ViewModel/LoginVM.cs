﻿using System.ComponentModel.DataAnnotations;

namespace MVCProjects_Medico.ViewModel
{
    public class LoginVM
    {
        [Required]
        public string UserNameOrEmail { get; set; }
        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsPersistent { get; set; }
    }
}
