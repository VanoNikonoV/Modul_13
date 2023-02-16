using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modul_13.Models
{
    public class BankClient
    {
        public Client Bank_Client { get; set; }

        public DepositAccount Deposit { get; set; }

        public NoDepositAccount NoDeposit { get; set; }

        /// <summary>
        /// Конструктор клиента банка, с возможностью завести два счета
        /// </summary>
        /// <param name="bankClient">Базованя информация о клиенте</param>
        /// <param name="deposit">Депозитный счет</param>
        /// <param name="noDeposit">Не депозитный счет</param>
        public BankClient(Client bankClient, DepositAccount deposit, NoDepositAccount noDeposit)
        {
            this.Bank_Client = bankClient;
            this.Deposit = deposit;
            this.NoDeposit = noDeposit;
        }
    }
}
