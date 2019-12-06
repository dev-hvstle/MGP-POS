using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using POSMGP.Model;
using POSMGP.View;

namespace POSMGP.View
{
    /// <summary>
    /// Interaction logic for TransactionView.xaml
    /// </summary>
    public partial class TransactionView : UserControl
    {
        List<TransactionModel> transactions = new List<TransactionModel>();
        
        const string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=db_mgppos;";
        string queryAll = "SELECT * FROM tbl_transactionmaster WHERE isPriority=1";
        public TransactionView()
        {
            InitializeComponent();
            //dateFirst.cus
            //string queryAll = "SELECT * FROM tbl_transactionmaster";
            if(LoginModel.userRights == "SuperAdmin")
            {
                cbSearch.Items.Add("Deleted Transactions");
                //btnRetrieveRecord.Visibility = Visibility.Visible;
            }
            loadTransaction(queryAll);
        }

        void loadTransaction(String query)
        {
            //string query = "SELECT * FROM tbl_transactionmaster";
            transactions.Clear();
            lvTransactions.Items.Clear();
            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            MySqlDataReader reader;


            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();
                // Success, now list 

                // If there are available rows
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        TransactionModel tmp = new TransactionModel {transactionID = reader.GetInt16(0), customerName = reader.GetString(1), userID = reader.GetInt16(2), totalItems = reader.GetInt16(3), totalAmount = reader.GetDouble(4), paymentAmount = reader.GetDouble(5), paymentChange = reader.GetDouble(6), transactionDate = reader.GetDateTime(7).ToString("yyyy-MM-dd"), transactionTime = reader.GetString(8), isPriority = reader.GetInt16(9) };
                        transactions.Add(tmp);
                        lvTransactions.Items.Add(tmp);
                    }
                }
                else
                {
                    //Console.WriteLine("There are no available products");
                    
                }

                databaseConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void loadDeletedTransaction()
        {
            String query = "SELECT * FROM tbl_transactionmaster WHERE isPriority=0";
            transactions.Clear();
            lvTransactions.Items.Clear();
            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            MySqlDataReader reader;


            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();
                // Success, now list 

                // If there are available rows
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        TransactionModel tmp = new TransactionModel { transactionID = reader.GetInt16(0), customerName = reader.GetString(1), userID = reader.GetInt16(2), totalItems = reader.GetInt16(3), totalAmount = reader.GetDouble(4), paymentAmount = reader.GetDouble(5), paymentChange = reader.GetDouble(6), transactionDate = reader.GetDateTime(7).ToString("yyyy-MM-dd"), transactionTime = reader.GetString(8), isPriority = reader.GetInt16(9) };
                        transactions.Add(tmp);
                        lvTransactions.Items.Add(tmp);
                    }
                }
                else
                {
                    //Console.WriteLine("There are no available products");

                }

                databaseConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void lvTransactions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lvTransactions.SelectedIndex > -1)
            {
                //TransactionModel tmp = (TransactionModel)lvTransactions.SelectedItem;
                //tbTransId.Text = tmp.transactionID.ToString();
                //tbCostumerName.Text = tmp.customerName;
                //tbCashier.Text = tmp.userID.ToString();
                //tbTransactionDate.Text = tmp.transactionDate;
                //tbTotalAmount.Text = tmp.totalAmount.ToString();
            }else
            {
                MessageBox.Show("No selected item!");
            }
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            String queryWithOneDate = "SELECT * FROM tbl_transactionmaster WHERE transactionDate BETWEEN CAST('" + calendarGeneral.SelectedDate.Value.ToString("yyyy-MM-dd") + "' AS DATE) AND CAST('" + calendarGeneral.SelectedDate.Value.ToString("yyyy-MM-dd") + "' AS DATE)";
            //MessageBox.Show(calendarGeneral.DisplayDate.ToString("yyyy-MM-dd"));
            loadTransaction(queryWithOneDate);
            Double tmpTotal = 0;
            foreach(TransactionModel tmp in transactions)
            {
                tmpTotal += tmp.totalAmount;
            }
            tbTotalSales.Text = tmpTotal.ToString();
        }

        private void dateSecond_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            
            
            if (dateFirst.Text == "" || dateFirst.Text == null)
            {
                //MessageBox.Show("Please enter first date before second date!");
                return;
            }

            String queryDateBetween = "SELECT * FROM tbl_transactionmaster WHERE transactionDate BETWEEN CAST('" + dateFirst.SelectedDate.Value.ToString("yyyy-MM-dd") + "' AS DATE) AND CAST('" + dateSecond.SelectedDate.Value.ToString("yyyy-MM-dd") + "' AS DATE)";
            MessageBox.Show(dateFirst.ToString());
            loadTransaction(queryDateBetween);
            Double tmpTotal = 0;
            foreach (TransactionModel tmp in transactions)
            {
                tmpTotal += tmp.totalAmount;
            }
            tbTotalSales.Text = tmpTotal.ToString();
        }

        private void dateFirst_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dateSecond.Text == "" || dateSecond.Text == null)
            {
                //MessageBox.Show("Please enter first date before second date!");
                return;
            }

            String queryDateBetween = "SELECT * FROM tbl_transactionmaster WHERE transactionDate BETWEEN CAST('" + dateFirst.SelectedDate.Value.ToString("yyyy-MM-dd") + "' AS DATE) AND CAST('" + dateSecond.SelectedDate.Value.ToString("yyyy-MM-dd") + "' AS DATE)";
            MessageBox.Show(dateFirst.ToString());
            loadTransaction(queryDateBetween);
            Double tmpTotal = 0;
            foreach (TransactionModel tmp in transactions)
            {
                tmpTotal += tmp.totalAmount;
            }
            tbTotalSales.Text = tmpTotal.ToString();
        }

        private void btnViewTransaction_Click(object sender, RoutedEventArgs e)
        {
            TransactionModel tmp = (TransactionModel)lvTransactions.SelectedItem;
            TransactionDetailView openView = new TransactionDetailView(tmp.customerName, Convert.ToInt16(tmp.transactionID), Convert.ToInt16(tmp.userID), tmp.transactionDate, Convert.ToDouble(tmp.totalAmount));
            openView.Show();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            String query = "UPDATE tbl_transactionmaster SET isPriority=0 WHERE transactionID=@transactionID";

            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;

            try
            {
                databaseConnection.Open();
                commandDatabase.ExecuteReader();
                MessageBox.Show("Transaction successfully deleted!");
                databaseConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                loadTransaction(queryAll);
            }
        }

        private void btnRetrieveRecord_Click(object sender, RoutedEventArgs e)
        {
            String query = "UPDATE tbl_transactionmaster SET isPriority=1 WHERE transactionID=@transactionID";

            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;

            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            String query = "";

            if (tbSearch.Text == "")
            {
                loadTransaction(queryAll);
                return;
            }

            if (cbSearch.Text == "Transaction ID")
            {
                query = "SELECT * FROM tbl_transactionmaster WHERE transactionID LIKE '%" + tbSearch.Text + "%' AND isPriority=1";
            }
            else if (cbSearch.Text == "Product Name")
            {
                query = "SELECT * FROM tbl_transactionmaster WHERE customerName LIKE '%" + tbSearch.Text + "%' AND isPriority=1";
            }
            else if (cbSearch.Text == "Supplier ID")
            {
                query = "SELECT * FROM tbl_transactionmaster WHERE userID LIKE '%" + tbSearch.Text + "%' AND isPriority=1";
            }


            lvTransactions.Items.Clear();

            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            MySqlDataReader reader;


            try
            {

                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        TransactionModel tmp;
                        tmp = new TransactionModel { transactionID = reader.GetInt16(0), customerName = reader.GetString(1), userID = reader.GetInt16(2), totalItems = reader.GetInt16(3), totalAmount = reader.GetDouble(4), paymentAmount = reader.GetDouble(5), paymentChange = reader.GetDouble(6), transactionDate = reader.GetDateTime(7).ToString("yyyy-MM-dd"), transactionTime = reader.GetString(8), isPriority = reader.GetInt16(9) };

                        lvTransactions.Items.Add(tmp);
                    }
                    databaseConnection.Close();
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void cbSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbSearch.SelectedItem.ToString() == "Deleted Transactions")
            {
                loadDeletedTransaction();
                tbSearch.IsEnabled = false;
                btnRetrieveRecord.Visibility = Visibility.Visible;
                return;
            }
            loadTransaction(queryAll);
            btnRetrieveRecord.Visibility = Visibility.Collapsed;
            tbSearch.IsEnabled = true;
        }
    }
}
