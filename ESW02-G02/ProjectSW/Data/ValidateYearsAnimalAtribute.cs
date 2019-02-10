using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSW.Data
{
    /// <summary> Classe de validação, usada para validar a data inserida no campo "Data de nascimento"</summary>
    public class ValidateYearsAnimalAttribute : ValidationAttribute
    {
        private readonly DateTime _minValue = DateTime.UtcNow.AddYears(-20);
        private readonly DateTime _maxValue = DateTime.UtcNow.AddYears(0);

        /// <summary> Metodo de validação da data inserida, verifica se a data inserida representa uma data com no minimo de 16 anos</summary>
        /// <param name="value">Objeto passado pelo input da Data de nascimento.</param>
        public override bool IsValid(object value)
        {
            DateTime val = (DateTime)value;
            return val >= _minValue && val <= _maxValue;
        }

        /// <summary> Metodo que mostra uma mensagem de erro</summary>
        /// <param name="name">Mensagem de erro passada caso necessário.</param>
        public override string FormatErrorMessage(string name)
        {
            return string.Format("O valor da sua data é invalida, tem que estar entre {0} ### {1}", _minValue, _maxValue);
        }
    }
}
