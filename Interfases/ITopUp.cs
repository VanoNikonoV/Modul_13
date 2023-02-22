using Modul_13.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modul_13.Interfases
{
    public interface ITopUp<out C> where C : BankAccount
    {
        C TopUpAccount (decimal amount);
    }
}
