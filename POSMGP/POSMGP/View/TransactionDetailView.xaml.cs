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
using System.Windows.Shapes;
using POSMGP.Model;

namespace POSMGP.View
{
    /// <summary>
    /// Interaction logic for TransactionDetailView.xaml
    /// </summary>
    public partial class TransactionDetailView : Window
    {
        const String connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=db_mgppos";
        public TransactionDetailView(String customerName, int transactionID, int cashiereID, String transactionDate, Double totalAmount)
        {
            InitializeComponent();
            tbCostumerName.Text = customerName;
            tbTransId.Text = transactionID.ToString();
            tbCashier.Text = cashiereID.ToString();
            tbTransactionDate.Text = transactionDate;
            tbTotalAmount.Text = totalAmount.ToString();
            loadDetails();
        }

        void loadDetails()
        {
            String query = "SELECT tbl_transactiondetails.PID, tbl_products.PName, tbl_transactiondetails.productQuantity, tbl_transactiondetails.productSubtotal FROM tbl_transactiondetails INNER JOIN tbl_products WHERE tbl_products.PID = tbl_transactiondetails.PID AND tbl_transactiondetails.transactionID = @transactionID";

            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            MySqlDataReader reader;

            commandDatabase.Parameters.AddWithValue("@transactionID", Convert.ToInt16(tbTransId.Text));

            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        TransactionDetailsModel tmp = new TransactionDetailsModel {productID = reader.GetInt16("PID"), productName = reader.GetString("PName"), productQuantity = reader.GetInt16("productQuantity"), productSubtotal = reader.GetDouble("productSubtotal") };
                        lvTransactionDetails.Items.Add(tmp);
                    }
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                return;
            }
            this.DragMove();
        }
    }
}
