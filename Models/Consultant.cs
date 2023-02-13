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
        /// Возвращает коллекцию клиентов со скрытими данными
        /// </summary>
        /// <returns>ObservableCollection<Client></returns>
        public IEnumerable<Client> ViewClientsData(IEnumerable<Client> clients)
        { 
            List<Client> clientsForConsultant = new List<Client>();

            foreach (Client client in clients)
            {
                string concealment = ConcealmentOfSeriesAndPassportNumber(client.SeriesAndPassportNumber);

                Client temp = new Client(firstName: client.FirstName,
                                        middleName: client.MiddleName,
                                        secondName: client.SecondName,
                                           telefon: client.Telefon,
                           seriesAndPassportNumber: concealment,
                                          dateTime: client.DateOfEntry,
                                         currentId: client.ID,
                                         isChanged: client.IsChanged);

                temp.InfoChanges = client.InfoChanges;

                temp.IsChanged = client.IsChanged;

                clientsForConsultant.Add(temp);
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
