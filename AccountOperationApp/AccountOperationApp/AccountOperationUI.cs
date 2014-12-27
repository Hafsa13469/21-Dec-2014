using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccountOperationApp
{
    public partial class AccountOperationUI : Form
    {
        Account aAccount = new Account();
        public AccountOperationUI()
        {
            InitializeComponent();
        }

        private void createButton_Click(object sender, EventArgs e)
        {
           

            aAccount.accountNumber = accountNumberTextBox.Text;
            aAccount.customerName = customerNameTextBox.Text;
            MessageBox.Show("Account has been created");

        }

        private void depositButton_Click(object sender, EventArgs e)
        {
            //int totalAmount = 0;
           aAccount.givenAmount = Convert.ToInt32(amountTextBox.Text);
            aAccount.DepositToAccont();
            MessageBox.Show("Amount is deposited");
        }

        private void withdrawButton_Click(object sender, EventArgs e)
        {
            aAccount.withdrawAmount = Convert.ToInt32(amountTextBox.Text);
            aAccount.WithdrawFromAccount();
            MessageBox.Show("Amount is withdrawed");
        }

        private void reportButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show(aAccount.GetReport());
        }
    }
}
