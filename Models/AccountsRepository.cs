using Modul_13.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Modul_13.Models
{
    public class AccountsRepository<T>:List<T> 
    {
        public ICollectionView CollectionView { get; set; }

        public AccountsRepository(string path)
        {
            LoadData(path);

            CollectionView = CollectionViewSource.GetDefaultView(this);
        }

        private void LoadData(string path)
        {
            throw new NotImplementedException();
        }
    }
}
