using Source_Control_Final_Assignment.Custom_Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Source_Control_Final_Assignment.Models
{
    public class Person
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }

        public string Email { get; set; }

        [Required]
        [Range(18, 60, ErrorMessage = "Age must be between 18 and 60")]
        public int Age { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [RegularExpression(@"^(([',. -][a-zA-Z0-9 ])?[a-zA-Z]*)*$")]
        public string About { get; set; }

        [FileExtensions(Extensions = "jpg,jpeg,png")]
        public string ProfileImagePath { get; set; }

        [Required]
        [ExtensionValidation]
        [NotMapped]
        public HttpPostedFileBase ProfilePhoto { get; set; }
    }

    public class PersonDBContext : DbContext
    {
        public PersonDBContext() { }

        public DbSet<Person> Persons { get; set; }
    }
}