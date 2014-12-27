using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomenQueueApp
{
    public partial class CustomerQueueUI : Form
    {
        //Customer aCustomer = new Customer();
     
        public CustomerQueueUI()
        {
        
            InitializeComponent();
        }  
        private string connectionString =
            @"Data Source = (localdb)\v11.0; Database = CustomerQueueDequeueDB; Integrated Security = true";
   private enum Prograss_Status

    {
        Not_Served,
        On_Service,
        Served
    }


        
        private void enqueueButton_Click(object sender, EventArgs e)
        {
            Save();
            ShowCustomerComplain();
        }

        private void ShowCustomerComplain()
        {
            SqlConnection aSqlConnection = new SqlConnection(connectionString);
            aSqlConnection.Open();
            string query2 = "SELECT * FROM t_customer WHERE progress_status = '" + Prograss_Status.Not_Served +
                            "' OR progress_status = '" + Prograss_Status.On_Service + "' OR progress_status = '" +
                            Prograss_Status.Served + "'";
            SqlCommand aSqlCommand = new SqlCommand(query2, aSqlConnection);
            SqlDataReader aSqlDataReader = aSqlCommand.ExecuteReader();

            List<Customer> customers = new List<Customer>();

            while (aSqlDataReader.Read())
            {
                Customer aCustomer = new Customer();
                aCustomer.serialNo = Convert.ToInt32(aSqlDataReader["Id"]);
                aCustomer.name = aSqlDataReader["name"].ToString();
                aCustomer.complain = aSqlDataReader["complain"].ToString();
                aCustomer.status = aSqlDataReader["progress_status"].ToString();
                customers.Add(aCustomer);
            }
            aSqlConnection.Close();

            remainingCustomerListView.Items.Clear();

            foreach (Customer aCustomer in customers)
            {
                ListViewItem aListViewItem = new ListViewItem();
                aListViewItem.Text = aCustomer.serialNo.ToString();
                aListViewItem.SubItems.Add(aCustomer.name);
                aListViewItem.SubItems.Add(aCustomer.complain);
                aListViewItem.SubItems.Add(aCustomer.status);
                remainingCustomerListView.Items.Add(aListViewItem);
            }
            remainingLabel.Text = customers.Count.ToString();
            ShowNoOfCustomerServedToday();
        }
        public void ShowNoOfCustomerServedToday()
        {
            SqlConnection aSqlConnection = new SqlConnection(connectionString);
            aSqlConnection.Open();
            string query2 = "SELECT COUNT(Id) FROM t_customer WHERE progress_status = '" + Prograss_Status.Served + "'";
            SqlCommand aSqlCommand = new SqlCommand(query2, aSqlConnection);
            SqlDataReader aSqlDataReader = aSqlCommand.ExecuteReader();
            aSqlDataReader.Read();
            int noOfServedCustomer = Convert.ToInt32(aSqlDataReader[0]);
            aSqlConnection.Close();
            servedLabel.Text = noOfServedCustomer.ToString();

        }
    

        private void Save()
        {
            SqlConnection aSqlConnection = new SqlConnection(connectionString);
            aSqlConnection.Open();
            string query = "INSERT INTO t_customer VALUES ('" + nameTextBox.Text + "','" + complainTextbox.Text + "','" +
                           Prograss_Status.Not_Served + "') ";
            SqlCommand aSqlCommand = new SqlCommand(query, aSqlConnection);
            aSqlCommand.ExecuteNonQuery();
            aSqlConnection.Close();
        }

        private void CustomerQueueUI_Load(object sender, EventArgs e)
        {
            ShowCustomerComplain();
        }

        private void dequeueButton_Click(object sender, EventArgs e)
        {
            List <Customer> customers = new List<Customer>();
            SqlConnection aSqlConnection = new SqlConnection(connectionString);
            aSqlConnection.Open();

            string minSerialNoQuery = "SELECT MIN(Id) FROM t_customer WHERE progress_status = '" +
                                      Prograss_Status.Not_Served + "'";
            SqlCommand minSerialCommand = new SqlCommand(minSerialNoQuery, aSqlConnection);

            SqlDataReader firstReader = minSerialCommand.ExecuteReader();
            firstReader.Read();

            int minSerial = Convert.ToInt32(firstReader[0]);

            aSqlConnection.Close();
            aSqlConnection.Open();

            string dequeueQuery = "SELECT * FROM t_customer WHERE Id = " + minSerial;
            SqlCommand dequeueCommand = new SqlCommand(dequeueQuery, aSqlConnection);

            SqlDataReader secondReader = dequeueCommand.ExecuteReader();

            while (secondReader.Read())
            {
                Customer aCustomer = new Customer();
                aCustomer.serialNo = Convert.ToInt32(secondReader["Id"]);
                aCustomer.name = secondReader["name"].ToString();
                aCustomer.complain = secondReader["complain"].ToString();
                aCustomer.status = secondReader["progress_status"].ToString();

                customers.Add(aCustomer);
            }

            foreach (Customer aCustomer in customers)
            {
                serialNoTextBox.Text = aCustomer.serialNo.ToString();
                customerNameTextBox.Text = aCustomer.name;
                customerComplainTextBox.Text = aCustomer.complain;
            }

            aSqlConnection.Close();

            UpdateData();
            ShowCustomerComplain();
        }
        private void UpdateData()
        {
            int serial = Convert.ToInt32(serialNoTextBox.Text);

            SqlConnection aSqlConnection = new SqlConnection(connectionString);
            aSqlConnection.Open();

            string firstUpdateQuery = "UPDATE t_customer SET progress_status = '" + Prograss_Status.Served +
                                      "' WHERE progress_status = '" + Prograss_Status.On_Service + "'";
            string secondUpdateQuery = "UPDATE t_customer SET progress_status = '" + Prograss_Status.On_Service +
                                       "' WHERE Id = " + serial;

            SqlCommand firstCommand = new SqlCommand(firstUpdateQuery, aSqlConnection);
            SqlCommand secondCommand = new SqlCommand(secondUpdateQuery, aSqlConnection);

            firstCommand.ExecuteNonQuery();
            secondCommand.ExecuteNonQuery();

            aSqlConnection.Close();
        }
    }
}
