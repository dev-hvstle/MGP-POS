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
using MySql.Data.MySqlClient;

namespace POSMGP.View
{
    /// <summary>
    /// Interaction logic for PosView.xaml
    /// </summary>
    public partial class PosView : UserControl
    {
        const string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=db_mgppos;";
        int tag = 0;
        
        List<PosModel> itemData = new List<PosModel>();
        List<PosModel> basketData = new List<PosModel>();
        //List<TransactionModel> transactions = new List<TransactionModel>();
        int latestTransactionID;

        public PosView()
        {
            InitializeComponent();
            lblCashiereDisplay.Content = POSMGP.Model.LoginModel.currentUserID.ToString();
            loadItems();
            loadTransaction();
        }

        void computation()
        {
            int totalItems = 0;
            Double totalAmount = 0;
            if(basketData.Count == 0)
            {
                lblTotalItemsDisplay.Content = null;
                //lblSubtotalDisplay.Content = null;
                lblTotalCostDisplay.Content = null;
            }
            else
            {
                foreach (PosModel item in basketData)
                {
                    totalItems += item.productQuantity;
                    totalAmount += item.productSubtotal;
                    //lblSubtotalDisplay.Content = subtotalAmount.ToString();
                }
                lblTotalItemsDisplay.Content = totalItems.ToString();
                lblTotalCostDisplay.Content = totalAmount.ToString();
            }
        }

        void loadItems()
        {
            lvPurchaseList.Items.Clear();

            string query = "SELECT * FROM tbl_products";

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
                        

                        itemData.Add(new PosModel() {productID = reader.GetInt16(0), productName = reader.GetString(1), productPrice = reader.GetDouble(2)});
                        
                        Button itemButton = new Button();
                        itemButton.Content = reader.GetString(1);
                        itemButton.Tag = tag;
                        itemButton.Width = 100;
                        itemButton.Height = 100;
                        itemButton.Background = (Brush)(new BrushConverter().ConvertFrom("#B6B5B5"));
                        itemButton.BorderBrush = Brushes.Aquamarine;
                        itemButton.Click += ItemButton_Click;

                        Grid productGrid = new Grid();
                        productGrid.Height = 120;
                        productGrid.Width = 120;
                        productGrid.Children.Add(itemButton);
                        //productGrid.Children.Add(itemPanel);

                        itemCollectionPanel.Children.Add(productGrid);
                        tag++;
                    }
                }
                else
                {
                    Console.WriteLine("There are no available products");
                }

                databaseConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void loadTransaction()
        {
            //lvPurchaseList.Items.Clear();

            string query = "SELECT transactionID FROM tbl_transactionmaster ORDER BY transactionID DESC LIMIT 1";

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
                        latestTransactionID = reader.GetInt16(0);
                    }
                }
                else
                {
                    //Console.WriteLine("There are no available products");
                    latestTransactionID = 0;
                }

                databaseConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (tbQuantity.Text == "")
            {
                MessageBox.Show("Please Enter Product Quantity!");
                return;
            }
            
            PosModel item = (PosModel)itemData.ElementAt(Convert.ToInt16(((Button)sender).Tag));
            Double subtotalProcess = item.productPrice * Convert.ToDouble(tbQuantity.Text);

            lvPurchaseList.ItemsSource = null; 
            foreach(PosModel tmp in basketData)
            {
                if(tmp.productID == item.productID)
                {
                    tmp.productQuantity += Convert.ToInt16(tbQuantity.Text);
                    tmp.productSubtotal = Convert.ToDouble(tmp.productPrice) * tmp.productQuantity;
                    lvPurchaseList.ItemsSource = basketData;
                    computation();
                    return;
                }
            }
            basketData.Add(new PosModel() { productID = item.productID, productName = item.productName, productPrice = item.productPrice, productQuantity = Convert.ToInt16(tbQuantity.Text), productSubtotal = subtotalProcess});
            lvPurchaseList.ItemsSource = basketData;

            computation();
        }

        private void btnPayment_Click(object sender, RoutedEventArgs e)
        {
            if (tbPayment.Text == "" || Convert.ToDouble(tbPayment.Text) < Convert.ToDouble(lblTotalCostDisplay.Content))
            {

                MessageBox.Show("Insufficient Payment!");
                return;

            }
            string query = "INSERT INTO tbl_transactionmaster(`customerName`, `userID`,`totalItems`,`totalAmount`,`paymentAmount`, `paymentChange`, `transactionDate`) VALUES (@customerName, @userID, @totalItems, @totalAmount, @paymentAmount, @paymentChange, @transactionDate)";
            string query2 = "INSERT INTO tbl_transactiondetails(`transactionID`, `PID`, `productQuantity`, `productSubtotal`) VALUES (@transactionID, @PID, @productQuantity, @productSubtotal)";
            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            //MySqlDataReader reader;
            commandDatabase.CommandTimeout = 60;

            commandDatabase.Parameters.AddWithValue("@customerName", tbCustomer.Text);
            commandDatabase.Parameters.AddWithValue("@userID", Convert.ToInt16(lblCashiereDisplay.Content));
            commandDatabase.Parameters.AddWithValue("@totalItems", Convert.ToInt16(lblTotalItemsDisplay.Content));
            commandDatabase.Parameters.AddWithValue("@totalAmount", Convert.ToDouble(lblTotalCostDisplay.Content));
            commandDatabase.Parameters.AddWithValue("@paymentAmount", Convert.ToDouble(tbPayment.Text));
            commandDatabase.Parameters.AddWithValue("@paymentChange", Convert.ToDouble(tbPayment.Text) - Convert.ToDouble(lblTotalCostDisplay.Content));
            commandDatabase.Parameters.AddWithValue("@transactionDate", DateTime.Now);

           
                try
                {
                    databaseConnection.Open();
                    commandDatabase.ExecuteNonQuery();
                    MySqlCommand commandDatabase2 = new MySqlCommand(query2, databaseConnection);
                    foreach (PosModel tmp in basketData)
                    {

                        commandDatabase2.Parameters.Clear();
                        commandDatabase2.Parameters.AddWithValue("@transactionID", latestTransactionID + 1);
                        commandDatabase2.Parameters.AddWithValue("@PID", tmp.productID);
                        commandDatabase2.Parameters.AddWithValue("@productQuantity", tmp.productQuantity);
                        commandDatabase2.Parameters.AddWithValue("@productSubtotal", tmp.productSubtotal);
                        MySqlDataReader reader = commandDatabase2.ExecuteReader();
                        reader.Close();

                    }
                    databaseConnection.Close();
                    MessageBox.Show("Payment Successful!");
                    lvPurchaseList.ItemsSource = null;
                    lblTotalCostDisplay.Content = "";
                    lblTotalItemsDisplay.Content = "";
                    tbPayment.Text = "";
                    tbQuantity.Text = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    //loadProducts();
                    loadTransaction();
                }
        }

        private void btnDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if(lvPurchaseList.SelectedIndex == -1)
            {
                MessageBox.Show("Please Select an Item!");
                return;
            }

            PosModel item = (PosModel)lvPurchaseList.SelectedItem;

            lvPurchaseList.ItemsSource = null;
            basketData.Remove(item);
            lvPurchaseList.ItemsSource = basketData;

            computation();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            lvPurchaseList.ItemsSource = null;
            basketData.Clear();
            lvPurchaseList.ItemsSource = basketData;
            lblTotalItemsDisplay.Content = null;
            //lblSubtotalDisplay.Content = null;
            lblTotalCostDisplay.Content = null;
        }

        private void btnBill_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
 