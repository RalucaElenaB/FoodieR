﻿using System.ComponentModel.DataAnnotations;

namespace FoodieR.Models;

public class ContactFormModel
{
    [Required]
    [Display(Name = "Your Name")]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    [Display(Name = "Your Email")]
    public string Email { get; set; }

    [Required]
    [Display(Name = "Subject")]
    public string Subject { get; set; }

    [Required]
    [Display(Name = "Message")]
    public string Message { get; set; }
}



