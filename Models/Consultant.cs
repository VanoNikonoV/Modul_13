using Modul_13.Interfases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Modul_13.Models
{
    public class Consultant : IClientDataMonitor
    {
        public Consultant() {  }
      
        /// <summary>
        /// Метод редактирования номера телефона
        /// </summary>
        /// <param name="client">Клиент чей номер необходимо отредактировать</param>
        /// <param name="newData">Новый номер</param>
        /// <returns>Клент с новым номером</returns>
        public Client EditeTelefonClient(string newTelefon, Client client)
        {
            client.Telefon = newTelefon;

            client.DateOfEntry = DateTime.Now;

            client.IsChanged = true;

            return client;
        }

        /// <summary>
        /// Возвращает коллекцию клиентов банка со скрытими данными
        /// </summary>
        /// <returns>IEnumerable<BankAccount></returns>
        public IEnumerable<BankClient> ViewClientsData(IEnumerable<BankClient> clients)
        { 
            List<BankClient> clientsForConsultant = new List<BankClient>();

            foreach (BankClient client in clients)
            {
                string concealment = ConcealmentOfSeriesAndPassportNumber(client.Owner.SeriesAndPassportNumber);

                Client temp = new Client(firstName: client.Owner.FirstName,
                                        middleName: client.Owner.MiddleName,
                                        secondName: client.Owner.SecondName,
                                           telefon: client.Owner.Telefon,
                           seriesAndPassportNumber: concealment,
                                          dateTime: client.Owner.DateOfEntry,
                                         currentId: client.Owner.ID,
                                         isChanged: client.Owner.IsChanged);

                temp.InfoChanges = client.Owner.InfoChanges;

                temp.IsChanged = client.Owner.IsChanged;

                clientsForConsultant.Add(new BankClient(temp, client.Deposit, client.NoDeposit));
            }

            return clientsForConsultant;
        }

        /// <summary>
        /// Сокрыте паспортных данных клиента
        /// </summary>
        /// <param name="number">Паспорные данные</param>
        /// <returns>Скрытые данные либо "нет данных"</returns>
        private string ConcealmentOfSeriesAndPassportNumber(string number)
        {
            if (number.Length > 0 && number != null && number != String.Empty)
            {
                string data = number;

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < number.Length; i++)
                {
                    if (data[i] != ' ')
                    {
                        sb.Append('*');
                    }
                    else sb.Append(data[i]);
                }
                return sb.ToString();
            }

            else return "нет данных";
        }
    }

    
}
