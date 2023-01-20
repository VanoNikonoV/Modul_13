using Modul_13.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modul_13.Interfases
{
    public interface IClientDataMonitor
    {
        ObservableCollection<Client> ViewClientsData(ObservableCollection<Client>clients);

        Client EditeTelefonClient( string newData, Client client);
    }
}
