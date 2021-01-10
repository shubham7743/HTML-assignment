using ProductManagement.CustomValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProductManagement.Models
{
    public class Product
    {
        public int ID { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        [RegularExpression(@"^[a-zA-Z]+(([ ][a-zA-Z ])?[a-zA-Z]*)*$")]
        public string Name { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [StringLength(250)]
        [RegularExpression(@"^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$")]
        [Display(Name = "Short Description")]
        public string ShortDescription { get; set; }

        [StringLength(500)]
        [RegularExpression(@"^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$")]
        [Display(Name = "Large Description")]
        public string LongDescription { get; set; }

        [Display(Name = "Small Image")]
        public string SmallImagePath { get; set; }

        [Display(Name = "Large Image")]
        public string LargeImagePath { get; set; }

        [NotMapped]
        [ExtensionValidation(ErrorMessage = "Only .jpg, .jpeg, .png files are allowed in Small Image")]
        [FileSizeValidation(MaxSize = 3, ErrorMessage = "Maximum file size allowed is 3 MB")]
        [Display(Name = "Small Image")]
        public HttpPostedFileBase SmallImage { get; set; }

        [NotMapped]
        [ExtensionValidation(ErrorMessage = "Only .jpg, .jpeg, .png files are allowed in Large Image")]
        [FileSizeValidation(MaxSize = 3, ErrorMessage = "Maximum file size allowed is 3 MB")]
        [Display(Name = "Large Image")]
        public HttpPostedFileBase LargeImage { get; set; }
    }

    public class ProductDBContext : DbContext
    {
        public ProductDBContext() { }
        public DbSet<Product> Products { get; set; }
    }
}