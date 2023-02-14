using Modul_13.Models;
using System.Collections.Generic;

namespace Modul_13.Interfases
{
    public interface IClientDataMonitor
    {
        IEnumerable<BankAccount> ViewClientsData(IEnumerable<BankAccount>clients);

        Client EditeTelefonClient( string newData, Client client);
    }
}
