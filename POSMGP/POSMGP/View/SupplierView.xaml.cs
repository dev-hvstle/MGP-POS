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
    /// Interaction logic for SupplierView.xaml
    /// </summary>
    public partial class SupplierView : UserControl
    {
        const String connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=db_mgppos";
        SupplierModel supplier;
        public SupplierView()
        {
            InitializeComponent();
            loadSuppliers();
        }

        private void btnResetFields_Click(object sender, RoutedEventArgs e)
        {
            tbSupplierID.Text = "";
            tbSupplierLocation.Text = "";
            tbSupplierName.Text = "";
        }

        void loadSuppliers()
        {
            String query = "SELECT * FROM tbl_supplier";
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
                        //DateTime temp = reader.GetDateTime(3);
                        SupplierModel supplierList = new SupplierModel{ supplierID= reader.GetInt16(0), supplierName= reader.GetString(1), supplierLocation= reader.GetString(2), dateModified= reader.GetDateTime(3).ToString("yyyy-MM-dd") };
                        lvSupplier.Items.Add(supplierList);
                    }
                    databaseConnection.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            lvSupplier.Items.Clear();
            string query = "INSERT INTO tbl_supplier( `supplierName`,`locationCity`,`dateModified`) VALUES (@supplierName, @supplierLocation, @dateModified)";
            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;

            commandDatabase.Parameters.AddWithValue("@supplierName", tbSupplierName.Text);
            commandDatabase.Parameters.AddWithValue("@supplierLocation", tbSupplierLocation.Text);
            commandDatabase.Parameters.AddWithValue("@dateModified", DateTime.Now);

            if (tbSupplierID.Text != "" || (tbSupplierLocation.Text == "" || tbSupplierName.Text == ""))
            {

                MessageBox.Show("Missing Fields or User already exist!");

            }
            else
            {
                try
                {

                    databaseConnection.Open();
                    MySqlDataReader reader = commandDatabase.ExecuteReader();
                    MessageBox.Show("User Successfully Added!");
                    databaseConnection.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    loadSuppliers();
                }
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (tbSupplierID.Text != "" || (tbSupplierLocation.Text == "" || tbSupplierName.Text == ""))
            {

                MessageBox.Show("Missing Fields or User already exist!");
                return;
            }
            string query = "UPDATE `tbl_suppliers` SET `supplierName`=@supplierName,`supplierLocation`=@supplierLocation, `dateModified`=@dateModified WHERE `supplierID` =@supplierID";
            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);

            commandDatabase.Parameters.AddWithValue("@supplierID", Convert.ToInt16(tbSupplierID.Text));
            commandDatabase.Parameters.AddWithValue("@supplierName", tbSupplierName.Text);
            commandDatabase.Parameters.AddWithValue("@supplierLocation", tbSupplierLocation.Text);
            commandDatabase.Parameters.AddWithValue("@dateModified", DateTime.Now);
            commandDatabase.CommandTimeout = 60;
            MySqlDataReader reader;

            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();

                // Succesfully updated

                databaseConnection.Close();
            }
            catch (Exception ex)
            {
                // Ops, maybe the id doesn't exists ?
                MessageBox.Show(ex.Message);
            }
            finally
            {
                loadSuppliers();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void lvSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lvSupplier.SelectedIndex > -1)
            {
                SupplierModel tmp = (SupplierModel)lvSupplier.SelectedItem;
                tbSupplierID.Text = tmp.supplierID.ToString();
                tbSupplierName.Text = tmp.supplierName;
                tbSupplierLocation.Text = tmp.supplierLocation;
            }else
            {
                MessageBox.Show("No selected item!");
            }
        }
    }
}
