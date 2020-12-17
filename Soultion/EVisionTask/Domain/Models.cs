﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain
{
    public class Models
    {}
    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }
        [Required(ErrorMessage ="Name is required")]
        [Display(Name = "Full Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please upload photo")]
        [Display(Name = "Product Photo")]
        public string Photo { get; set; }
        [Required(ErrorMessage = "Price is required")]
        [Display(Name = "Product Price")]
        public double Price { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
