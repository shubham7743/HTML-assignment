﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductManagement.CustomValidations
{
    public class FileSizeValidation : ValidationAttribute, IClientValidatable
    {
        public int MaxSize { get; set; }

        public override bool IsValid(object value)
        {
            if (value == null)
                return true;
            var file = (HttpPostedFileBase)value;
            return file.ContentLength <= MaxSize * 1048576;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule();
            rule.ErrorMessage = base.ErrorMessage;
            rule.ValidationType = "filesize";
            rule.ValidationParameters.Add("maxsize", MaxSize);
            return new ModelClientValidationRule[] { rule };
        }
    }
}