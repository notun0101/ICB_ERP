﻿using System;
using System.ComponentModel.DataAnnotations;

namespace OPUSERP.Areas.API.Models
{
    public class loginViewModel
    {
        [Required]
        //[StringLength(100, ErrorMessage = "The {0} Must be at least {2} and at most {1} characters long.", MinimumLength = 3)]
        [Display(Name = "User Name")]
        public string ID { get; set; }

        [Required]
        //[StringLength(100, ErrorMessage = "The {0} Must be at least {2} and at most {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        public string fcmToken { get; set; }

    }
}
