using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace ProductManagement.CustomValidations
{
    public class ExtensionValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return true;
            string[] validExtensions = { "JPG", "JPEG", "PNG" };
            var file = (HttpPostedFileBase)value;
            var ext = Path.GetExtension(file.FileName).ToUpper().Replace(".", "");
            return validExtensions.Contains(ext) && file.ContentType.Contains("image");
        }
    }
}