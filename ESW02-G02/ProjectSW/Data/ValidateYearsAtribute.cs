using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSW.Data
{
    /// <summary> Classe de validação, usada para validar a data inserida no campo "Data de nascimento"</summary>
    public class ValidateYearsAttribute : ValidationAttribute
    {
        private readonly DateTime _minValue = DateTime.UtcNow.AddYears(-99);
        private readonly DateTime _maxValue = DateTime.UtcNow.AddYears(-16);

        /// <summary> Metodo de validação da data inserida, verifica se a data inserida representa uma data com no minimo de 16 anos</summary>
        /// <param name="value">Objeto passado pelo input da Data de nascimento.</param>
        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            DateTime val = (DateTime)value;
            if (val.Year >= _minValue.Year && val.Year <= _maxValue.Year) {
                return ValidationResult.Success;
            }
            return new ValidationResult(GetErrorMessage());
        }

        /// <summary> Metodo que mostra uma mensagem de erro</summary>
        private string GetErrorMessage()
        {
            return $"O utilizador precisa de ter mais que 16 anos.";
        }
    }
}
