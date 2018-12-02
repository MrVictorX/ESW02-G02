using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSW.Areas.Identity.Data
{
    public class ValidateYearsAttribute : ValidationAttribute
    {
        private readonly DateTime _minValue = DateTime.UtcNow.AddYears(-99);
        private readonly DateTime _maxValue = DateTime.UtcNow.AddYears(-16);

        public override bool IsValid(object value)
        {
            DateTime val = (DateTime)value;
            return val >= _minValue && val <= _maxValue;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("O valor da sua data é invalida", _minValue, _maxValue);
        }
    }
}
