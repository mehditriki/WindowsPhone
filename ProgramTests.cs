using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsoleApplication1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ConsoleApplication1.Tests
{
    public class BeforeEndDateAttribute : ValidationAttribute
    {
        public string EndDatePropertyName { get; set; }
        public string StartDate { get; set; }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            System.Reflection.PropertyInfo endDateProperty = validationContext.ObjectType.GetProperty(EndDatePropertyName);

            DateTime endDate = (DateTime)endDateProperty.GetValue(validationContext.ObjectInstance, null);

            var startDate = DateTime.Parse(StartDate);
            int result = DateTime.Compare(startDate, endDate);
            if (result <= 0)
                return ValidationResult.Success;
            // Do comparison
            // return ValidationResult.Success; // if success
            return new ValidationResult("Error"); // if fail
        }

    }
    public class Post
    {
        [BeforeEndDate(EndDatePropertyName = "EndDate", StartDate = "2000/01/01")]
        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }

    }
    [TestClass()]
    public class ProgramTests
    {
        [TestMethod()]
        public void testTest()
        {
            Post Post1 = new Post();
            Post1.BeginDate = DateTime.Parse("05/01/2000");
            Post1.EndDate = DateTime.Parse("04/01/1999");
            var validationResult = ValidationHelper.ValidateEntity<Post>(Post1);

            // Assert
            Assert.IsFalse(validationResult.HasError);

            Assert.Fail();
        }
    }
    public class EntityValidationResult
    {
        public IList<validationresult> Errors { get; private set; }
        public bool HasError
        {
            get { return Errors.Count > 0; }
        }
        public EntityValidationResult(IList<validationresult> errors = null)
        {
            Errors = errors ?? new List<validationresult>();
        }
    }

    public class EntityValidator<t> where T : class
    {
        public EntityValidationResult Validate(T entity)
        {
            var validationResults = new List<validationresult>();
            var vc = new ValidationContext(entity, null, null);
            var isValid = Validator.TryValidateObject
                    (entity, vc, validationResults, true);

            return new EntityValidationResult(validationResults);
        }
    }
    public class ValidationHelper
    {
        public static EntityValidationResult ValidateEntity<t>(T entity)
            where T : class
        {
            return new EntityValidator<t>().Validate(entity);
        }
    }
}