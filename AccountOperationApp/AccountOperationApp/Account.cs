using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountOperationApp
{
    class Account
    {
        public string accountNumber;
        public string customerName;
        public int givenAmount;
        public int withdrawAmount;
        public int totalAmount;


        public int DepositToAccont()
        {
            return totalAmount += givenAmount;

        }

        public int WithdrawFromAccount()
        {
            return totalAmount -= withdrawAmount;
        }

        public string GetReport()
        {
            return customerName + ", your account number:" + accountNumber + " and it's balance: " + totalAmount +
                   "taka";
        }


    }
}
