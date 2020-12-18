using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required(ErrorMessage ="Name is required")]
        [Display(Name = "Full Name")]
        public string Name { get; set; }
        //[Required(ErrorMessage = "Please upload photo")]
        [Display(Name = "Product Photo")]
        public string PhotoName { get; set; }
        [Required(ErrorMessage = "Price is required")]
        [Display(Name = "Product Price")]
        public double Price { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool? IsDeleted { get; set; }
        [NotMapped]
        public IFormFile PhotoFile { get; set; }
    }
}
