﻿namespace School.Web.Areas.Administration.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using School.Models;
    using School.Services.Interfaces;
    using School.Web.Validators;
    
    // [YearShoudNotExistInDb]
    public class AcademicYearCreateSubmitModel : IValidatableObject
    {
        public Guid Id { get; set; }

        [Required]
        [FutureDate(ErrorMessage = "The entered date should be current or future date.")]
        [Display(Name = "Start date")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime StartDate { get; set; }

        [Required]
        [FutureDate(ErrorMessage = "The entered date should be current or future date.")]
        [Display(Name = "End date")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime EndDate { get; set; }
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (this.StartDate >= this.EndDate)
            {
                errors.Add(new ValidationResult("Start date should not be equal or greater than end date."));
            }

            IAcademicYearService academicYearService = DependencyResolver.Current.GetService<IAcademicYearService>();

            bool yearExistsInDb = 
                academicYearService.AcademicYearExistsInDb(this.StartDate, this.EndDate) ? true : false;

            if (yearExistsInDb)
            {
                errors.Add(new ValidationResult(
                    "Academic Year with the entered start year or end year already exists."));
            }

            if (academicYearService.All().Any())
            {
                AcademicYear latestAcademicYear = 
                    academicYearService.All().OrderByDescending(ay => ay.StartDate).First();
                
                if (latestAcademicYear != null && this.StartDate.Year > latestAcademicYear.StartDate.Year + 1)
                {
                    errors.Add(new ValidationResult(
                        "New academic year cannot be more than 1 year later than latest academic year."));
                }
            }

            if (this.EndDate.Year > this.StartDate.Year + 1)
            {
                errors.Add(new ValidationResult("Academic Year may not last more than 1 astronomical year."));
            }

            return errors;
        }
    }
}