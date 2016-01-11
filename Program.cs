using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ConsoleApplication1
{
    public interface IPeriode
    {
         DateTime StartDate { get; set; }

         DateTime EndDate { get; set; }
    }

    [PeriodeContrainte (typeof(Periode), typeof(IPeriode))]
    public class Periode 
    {
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        
        public DateTime EndDate { get; set; }
    }
    public class Commande
    {
        [Required, ValidateObject]
        public Periode periode { get; set; }

    }
    public class ValidateObjectAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(value, null, null);

            Validator.TryValidateObject(value, context, results, true);

            if (results.Count != 0)
            {
                var compositeResults = new CompositeValidationResult(String.Format("Validation for {0} failed!", validationContext.DisplayName));
                results.ForEach(compositeResults.AddResult);

                return compositeResults;
            }

            return ValidationResult.Success;
        }
    }

    public class CompositeValidationResult : ValidationResult
    {
        private readonly List<ValidationResult> _results = new List<ValidationResult>();

        public IEnumerable<ValidationResult> Results
        {
            get
            {
                return _results;
            }
        }

        public CompositeValidationResult(string errorMessage) : base(errorMessage) { }
        public CompositeValidationResult(string errorMessage, IEnumerable<string> memberNames) : base(errorMessage, memberNames) { }
        protected CompositeValidationResult(ValidationResult validationResult) : base(validationResult) { }

        public void AddResult(ValidationResult validationResult)
        {
            _results.Add(validationResult);
        }
    }
    public class PeriodeContrainteAttribute : ValidationAttribute
    {
        public PeriodeContrainteAttribute(Type MyClasse, Type MyInterface)
            : base("{0} doit implémenter l'interface {1}")
        {
            myClasse = MyClasse;
            myInterface = MyInterface;
        }
        public Type myClasse { get; set; }
        public Type myInterface { get; set; }
        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name, myInterface);
        }

        protected override ValidationResult
            IsValid(object firstValue, ValidationContext validationContext)
        {
            var isValidClasse = ImplementsInterface(myClasse, myInterface);
            if (isValidClasse)
                return ValidationResult.Success;
            else
                return new ValidationResult(
                        FormatErrorMessage(validationContext.DisplayName));
        }
        public static bool ImplementsInterface( Type type, Type ifaceType)
        {
            Type[] intf = type.GetInterfaces();
            for (int i = 0; i < intf.Length; i++)
            {
                if (intf[i] == ifaceType)
                {
                    return true;
                }
            }
            return false;
        }
    }

        public class GreaterThan3Attribute : ValidationAttribute
    {
        public GreaterThan3Attribute(string otherProperty)
            : base("{0} doit etre superieur à {1}")
        {
            OtherProperty = otherProperty;
        }

        public string OtherProperty { get; set; }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name, OtherProperty);
        }

        protected override ValidationResult
            IsValid(object firstValue, ValidationContext validationContext)
        {
            var firstComparable = firstValue as IComparable;
            var secondComparable = GetSecondComparable(validationContext);

            if (firstComparable != null && secondComparable != null)
            {
                if (firstComparable.CompareTo(secondComparable) < 1)
                {
                    return new ValidationResult(
                        FormatErrorMessage(validationContext.DisplayName));
                }
            }

            return ValidationResult.Success;
        }
        protected IComparable GetSecondComparable(
            ValidationContext validationContext)
        {
            var propertyInfo = validationContext
                                  .ObjectType
                                  .GetProperty(OtherProperty);
            if (propertyInfo != null)
            {
                var secondValue = propertyInfo.GetValue(
                    validationContext.ObjectInstance, null);
                return secondValue as IComparable;
            }
            return null;
        }
    }

    public class programTest
    {
        private static void PrintResults(IEnumerable<ValidationResult> results, Int32 indentationLevel)
        {
            foreach (var validationResult in results)
            {
                SetIndentation(indentationLevel);

                Console.WriteLine(validationResult.ErrorMessage);
                Console.WriteLine();

                if (validationResult is CompositeValidationResult)
                {
                    PrintResults(((CompositeValidationResult)validationResult).Results, indentationLevel + 1);
                }
            }
        }
        private static void SetIndentation(int indentationLevel)
        {
            Console.CursorLeft = indentationLevel * 4;
        }
        public static void Main()
        {
            Periode PeriodeTest = new Periode();
            Commande commande = new Commande();
            commande.periode = PeriodeTest;
            var context = new ValidationContext(commande, null, null);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(commande, context, results, true);
            PrintResults(results, 0);
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
