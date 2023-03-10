using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modul_13.Models
{
    /// <summary>
    /// Счет для начисления процентов
    /// </summary>
    public class InterestEarningAccount:BankAccount
    {
        public InterestEarningAccount(Client owner, decimal initialBalance) : base(owner, initialBalance) { }

        public override void PerformMonthEndTransactions()
        {
            if (Balance > 500m)
            {
                decimal interest = Balance * 0.05m;
                MakeDeposit(interest,
                    DateTime.Now,
                    "Начислены ежемесячные проценты");
            }
        }
    }
}
