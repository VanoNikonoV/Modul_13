using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace Modul_13.Models

/// <summary>
/// Класс описывающий модель клиента
/// </summary>
{
    public partial class Client : INotifyPropertyChanged, IDataErrorInfo, IEquatable<Client>
    {
        private string error;
        public string Error 
        { 
            get { return error; }
            set { error = value; }
        }

        string IDataErrorInfo.this[string propertyName]
        {
            get { return this.GetValidationError(propertyName); }
        }

        /// <summary>
        /// Метод информиует о наличии/отсутствии ощибок в данных 
        /// </summary>
        public bool IsValid
        {
            get
            {
                foreach (string property in ValidatedProperties)
                    if (GetValidationError(property) != null)
                        return false;

                return true;
            }
        }
        /// <summary>
        /// Массив свойст для медода IsValid
        /// </summary>
        static readonly string[] ValidatedProperties =
        {
            "FirstName",
            "MiddleName",
            "SecondName",
            "Telefon",
            "SeriesAndPassportNumber",
            "DateOfEntry"
        };

        /// <summary>
        /// Проеверка введенных пользователем даных
        /// </summary>
        /// <param name="propertyName">Наименование проеряемого свойства</param>
        /// <returns>null - если данные корректны, либо информацию об ощибке</returns>
        string GetValidationError(string propertyName)
        {
            if (Array.IndexOf(ValidatedProperties, propertyName) < 0)
                return null;

            string error = null;

            switch (propertyName)
            {
                case nameof(FirstName):
                    error = this.ValidateFirstName();
                    break;

                case nameof(MiddleName):
                    error = this.ValidateMiddleName();
                    break;

                case nameof(SecondName):
                    error = this.ValidateSecondName();
                    break;

                case nameof(Telefon):
                    error = this.ValidateTelefon();
                    break;

                case nameof(SeriesAndPassportNumber):
                    error = this.ValidateSeriesAndPassportNumber();
                    break;

                default:
                    //Debug.Fail("Unexpected property being validated on Client: " + propertyName);
                    break;
            }

            this.Error = error;
            return error;
        }

        /// <summary>
        /// Проверка строки
        /// </summary>
        /// <param name="value"></param>
        /// <returns>true - если строка корректна
        /// false - если строка пустая</returns>
        static bool IsStringMissing(string value)
        {
            return
                String.IsNullOrEmpty(value) ||
                value.Trim() == String.Empty;
        }

        string ValidateFirstName()
        {
            if (IsStringMissing(this.FirstName))
            {
                return "Нужно заполнить поле с именем";
            }
            return null;
        }

        string ValidateMiddleName()
        {
            if (IsStringMissing(this.MiddleName))
            {
                return "Нужно заполнить поле с отчеством";
            }
            return null;
        }

        string ValidateSecondName()
        {
            if (IsStringMissing(this.SecondName))
            {
                return "Нужно заполнить поле с фамилией";
            }
            return null;
        }

        string ValidateSeriesAndPassportNumber()
        {
            if (IsStringMissing(this.SeriesAndPassportNumber))
            {
                return "Нужно заполнить поле с паспортными данными";
            }
            return null;
        }

        string ValidateTelefon()
        {
            if (IsStringMissing(this.Telefon))
            {
                return "Нужно заполнить поле с номером телефона";
            }
            else if (!decimal.TryParse(this.Telefon, out decimal number))
            {
                return "Номер должен состоять из чисел";
            }

            else if (this.Telefon.Length > 11 || this.Telefon.Length < 11)
            {
                return "Номер должен состоять из 11 цифр";
            }
            return null;
        }

        public bool Equals(Client other)
        {
            if (this.FirstName == other.FirstName
                && this.SecondName == other.SecondName
                && this.MiddleName == other.MiddleName
                && this.SeriesAndPassportNumber == other.SeriesAndPassportNumber
                && this.Telefon == other.Telefon)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}

