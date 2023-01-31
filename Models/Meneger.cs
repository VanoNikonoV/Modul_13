using Modul_13.Interfases;
using System;
using System.Collections.ObjectModel;

namespace Modul_13.Models
{
    public class Meneger:Consultant, IClientDataMonitor
    {
        /// <summary>
        /// Возвращает коллекцию клиентов
        /// </summary>
        /// <returns>ObservableCollection<Client></returns>
        public new ObservableCollection<Client> ViewClientsData(ObservableCollection<Client> clients)
        {
            return clients;
        }

        /// <summary>
        /// Метод редактирования номера телефона
        /// </summary>
        /// <param name="client">Клиент чей номер необходимо отредактировать</param>
        /// <param name="newData">Новый номер</param>
        /// <returns>Клент с новым номером</returns>
        public new Client EditeTelefonClient(string newTelefon, Client client )
        {
            base.EditeTelefonClient(newTelefon, client);

            var x = client.InfoChanges[client.InfoChanges.Count-1];

            x.WhoChangedIt = nameof(Meneger);

            return client;
                
        }

        /// <summary>
        /// Метод редактирования имени
        /// </summary>
        /// <param name="client">Клент чьё имя необходимо отредактировать</param>
        /// <param name="newName">Новое имя</param>
        /// <returns>Клиент с новым именем</returns>
        public Client EditNameClient(Client client, string newName)
        {
            string whatChanges = string.Format(client.FirstName + @" на " + newName);

            Client changeClient = new Client( firstName: newName,
                                             middleName: client.MiddleName,
                                             secondName: client.SecondName,
                                                telefon: client.Telefon,
                                seriesAndPassportNumber: client.SeriesAndPassportNumber,
                                              currentId: client.ID,
                                               dateTime: DateTime.Now,
                                              isChanged: true);
           
            changeClient.InfoChanges = client.InfoChanges; //копирую старую информацию

            changeClient.InfoChanges.Add(new InformationAboutChanges(DateTime.Now, whatChanges, "замена", nameof(Meneger)));

            return changeClient;
        }

        /// <summary>
        /// Метод редактирования отчества
        /// </summary>
        /// <param name="client">Клиент чьё отчетсво необходимо отредактировать</param>
        /// <param name="newMiddleName">Новое отчетсво</param>
        /// <returns>Клиент с новым отчеством</returns>
        public Client EditMiddleNameClient(Client client, string newMiddleName)
        {
            string whatChanges = string.Format(client.MiddleName + @" на " + newMiddleName);

            Client changeClient = new Client( firstName: client.FirstName,
                                             middleName: newMiddleName,
                                             secondName: client.SecondName,
                                                telefon: client.Telefon,
                                seriesAndPassportNumber: client.SeriesAndPassportNumber,
                                              currentId: client.ID,
                                               dateTime: DateTime.Now,
                                              isChanged: true);

            changeClient.InfoChanges = client.InfoChanges; //копирую старую информацию

            changeClient.InfoChanges.Add(new InformationAboutChanges(DateTime.Now, whatChanges, "замена", nameof(Meneger)));

            return changeClient;
        }

        /// <summary>
        /// Метод редактирования фамилии
        /// </summary>
        /// <param name="client">Клиент чьё фамилию необходимо отредактировать</param>
        /// <param name="newSecondName">Новое отчетсво</param>
        /// <returns>Клиент с новой фамилией</returns>
        public Client EditSecondNameClient(Client client, string newSecondName)
        {
            string whatChanges = string.Format(client.SecondName + @" на " + newSecondName);

            Client changeClient = new Client( firstName: client.FirstName,
                                             middleName: client.MiddleName,
                                             secondName: newSecondName,
                                                telefon: client.Telefon,
                                seriesAndPassportNumber: client.SeriesAndPassportNumber,
                                              currentId: client.ID,
                                               dateTime: DateTime.Now,
                                              isChanged: true);

            changeClient.InfoChanges = client.InfoChanges; //копирую старую информацию

            changeClient.InfoChanges.Add(new InformationAboutChanges(DateTime.Now, whatChanges, "замена", nameof(Meneger)));

            return changeClient;
        }
        /// <summary>
        /// Метод редактирования паспортных данных
        /// </summary>
        /// <param name="client">Клиент чьи паспортные данные необходимо отредактировать</param>
        /// <param name="newSeriesAndPassportNumber">Новые паспортные данные</param>
        /// <returns>Клиент с новыи паспортными данными</returns>
        public Client EditSeriesAndPassportNumberClient(Client client, string newSeriesAndPassportNumber)
        {
            string whatChanges = string.Format(client.SeriesAndPassportNumber + @" на " + newSeriesAndPassportNumber);

            Client changeClient = new Client( firstName: client.FirstName,
                                             middleName: client.MiddleName,
                                             secondName: client.SecondName,
                                                telefon: client.Telefon,
                                seriesAndPassportNumber: newSeriesAndPassportNumber,
                                              currentId: client.ID,
                                               dateTime: DateTime.Now,
                                              isChanged: true);

            changeClient.InfoChanges = client.InfoChanges; //копирую старую информацию

            changeClient.InfoChanges.Add(new InformationAboutChanges(DateTime.Now, whatChanges, "замена", nameof(Meneger)));

            return changeClient;
        }
    }
}
