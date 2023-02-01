using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modul_13.Models
{
    public class AccountsRepository:ObservableCollection<BankAccount> 
    {
        public AccountsRepository(string path)
        {

        }

       
    }
}
