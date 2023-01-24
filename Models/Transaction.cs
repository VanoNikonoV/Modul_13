using System;

namespace Modul_13.Models
{
    /// <summary>
    /// Транзакция
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Поступление/выведение денежных средств в данной транзакции
        /// </summary>
        public decimal Amount { get; }
        /// <summary>
        /// Дата и время транзакции
        /// </summary>
        public DateTime Date { get; }
        /// <summary>
        /// Примечание об транзакции
        /// </summary>
        public string Notes { get; }

        public Transaction(decimal amount, DateTime date, string notes)
        {
            Amount = amount;
            Date = date;
            Notes = notes;
        }
    }
}
