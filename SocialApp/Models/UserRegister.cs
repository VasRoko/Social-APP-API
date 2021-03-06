﻿using System;
using System.ComponentModel.DataAnnotations;

namespace SocialApp.Models
{
    public class UserRegister
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 8, ErrorMessage = "Your password must be between 8 and 255 characters long")]
        public string Password { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }

        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }

        public UserRegister()
        {
            Created = DateTime.Now;
            LastActive = DateTime.Now;
        }
    }
}
