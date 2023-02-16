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
    public class BankRepository: ObservableCollection<BankClient> 
    {
        public ObservableCollection<BankClient> Collection { get; set; }

        public BankRepository(string path = "")
        {
            //LoadData(path);

            GetClientsRep(20);

            Collection = this;
        }

        private void LoadData(string path)
        {
            throw new NotImplementedException();
        }

        public void ReplaceClient(int index, Client editClient)
        {
            
        }

        #region Автогенерация данных
        private void GetClientsRep(int count)
        {
            long telefon = 79020000000;
            long passport = 6650565461;

            Random random = new Random();

            for (long i = 0; i < count; i++)
            {
                telefon += i;

                passport += random.Next(1, 500);

                Client _c = new Client(firstNames[BankRepository.randomize.Next(BankRepository.firstNames.Length)],
                    middleNames[BankRepository.randomize.Next(BankRepository.middleNames.Length)],
                    secondNames[BankRepository.randomize.Next(BankRepository.secondNames.Length)],
                    telefon.ToString(),
                    passport.ToString());

                this.Add(new BankClient (_c));
            }

        }

        static readonly string[] firstNames;

        static readonly string[] middleNames;

        static readonly string[] secondNames;

        /// <summary>
        /// Генератор псевдослучайных чисел
        /// </summary>
        static Random randomize;

        /// <summary>
        /// Статический конструктор, в котором "хранятся"
        /// данные о именах и фамилиях баз данных firstNames и lastNames
        /// </summary>
        static BankRepository()
        {
            randomize = new Random();

            firstNames = new string[] {
                "Агата",
                "Агнес",
                "Мария",
                "Аделина",
                "Ольга",
                "Людмила",
                "Аманда",
                "Татьяна",
                "Вероника",
                "Жанна",
                "Крестина",
                "Анжела",
                "Маргарита"
            };

            middleNames = new string[]
            {
                "Ивановна",
                "Петровна",
                "Васильевна",
                "Сергеевна",
                "Дмитриевна",
                "Владимировна",
                "Александровна",
                "Тимофеевна"

            };

            secondNames = new string[]
            {
                "Иванова",
                "Петрова",
                "Васильева",
                "Кузнецова",
                "Ковалёва",
                "Попова",
                "Пономарёва",
                "Дьячкова",
                "Коновалова",
                "Соколова",
                "Лебедева",
                "Соловьёва",
                "Козлова",
                "Волкова",
                "Зайцева",
                "Ершова",
                "Карпова",
                "Щукина",
                "Виноградова",
                "Цветкова",
                "Калинина"
            };

        }
        #endregion
    }
}
