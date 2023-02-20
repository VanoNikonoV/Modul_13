using Modul_13.Models;
using System.Collections.Generic;

namespace Modul_13.Interfases
{
    public interface IClientDataMonitor
    {
        IEnumerable<BankClient<Client>> ViewClientsData(IEnumerable<BankClient<Client>> clients);

        Client EditeTelefonClient( string newData, Client client);
    }
}
