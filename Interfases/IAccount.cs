using Modul_13.Models;
using System;

namespace Modul_13.Interfases
{
    // Используя ковариантный интерфейс, реализуйте методы пополнения счёта по соответствующему типу.
    public interface IAccount<out T> where T : BankAccount
    {
        T Account { get; }
        /// <summary>
        /// Пополнение (открытие нового) счета, начальный баланс должен быть положительным
        /// </summary>
        /// <param name="amount">Начальный баланс</param>
        /// <param name="date">Дата и время создания</param>
        /// <param name="note">Примечание</param>
        //void MakeDeposit(decimal amount, DateTime date, string note);

        /// <summary>
        /// Списание средст со счета, любой вывод не должен создавать отрицательный баланс
        /// </summary>
        /// <param name="amount">Сумма списания</param>
        /// <param name="date">Дата и время операции</param>
        /// <param name="note">Заметка об операции</param>
        //void MakeWithdrawal(decimal amount, DateTime date, string note);
    }
}
