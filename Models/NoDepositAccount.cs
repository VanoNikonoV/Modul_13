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
    public class NoDepositAccount:BankAccount
    {
        public NoDepositAccount(Client owner, decimal initialBalance) : base(owner, initialBalance) { }

    }
}
