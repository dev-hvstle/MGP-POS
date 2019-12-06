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
using System.Text.RegularExpressions;

namespace POSMGP.View
{
    /// <summary>
    /// Interaction logic for SupplierView.xaml
    /// </summary>
    public partial class SupplierView : UserControl
    {
        const String connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=db_mgppos";
        List<SupplierModel> supplierList = new List<SupplierModel>();
        int latestSupplierID = 0;
        List<UserModel> users = new List<UserModel>();
        List<ContactNumberModel> contact = new List<ContactNumberModel>();
        List<ContactNumberModel> tmpContact = new List<ContactNumberModel>();


        public SupplierView()
        {
            InitializeComponent();
            if(LoginModel.userRights == "SuperAdmin")
            {
                lblUpdate.Text = "Update/Retrieve";
                cbSearch.Items.Add("Deleted Suppliers");
            }
            loadSuppliers();
            loadContacts();
            loadLatestNumber();
            
        }

        private void btnResetFields_Click(object sender, RoutedEventArgs e)
        {
            tbSupplierID.Text = "";
            tbSupplierLocation.Text = "";
            tbSupplierName.Text = "";
        }

        void loadSuppliers()
        {
            lvSupplier.Items.Clear();
            supplierList.Clear();
            String query = "SELECT * FROM tbl_supplier WHERE isPriority=1";
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
                        SupplierModel supplierList = new SupplierModel{ supplierID= reader.GetInt16(0), supplierName= reader.GetString(1), supplierLocation= reader.GetString(2), modifiedBy=reader.GetInt16(3), dateModified= reader.GetDateTime(4).ToString("yyyy-MM-dd"), timeModified = reader.GetString(5), isPriority = reader.GetInt16(6) };
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

        void loadContacts()
        {
            String query = "SELECT * FROM tbl_contactnumber";

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
                        contact.Add(new ContactNumberModel() { supplierID = reader.GetInt16(0), contactNumber = reader.GetInt64(1) });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void loadLatestNumber()
        {
            String query = "SELECT supplierID FROM tbl_supplier ORDER BY supplierID DESC LIMIT 1";

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
                        latestSupplierID = reader.GetInt16(0);
                    }
                }
                databaseConnection.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void loadDeletedSupplier()
        {
            //lvSupplier.Items.Clear();
            String query = "SELECT * FROM tbl_supplier WHERE isPriority=0";
            tbSearch.IsEnabled = false;
            lvSupplier.Items.Clear();
            supplierList.Clear();

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
                        SupplierModel tmp = new SupplierModel{ supplierID= reader.GetInt16(0), supplierName = reader.GetString(1), supplierLocation = reader.GetString(2), modifiedBy = reader.GetInt16(3), dateModified = reader.GetDateTime(4).ToString("yyyy-MM-dd"), timeModified = reader.GetString(5), isPriority = reader.GetInt16(6) };

                        supplierList.Add(tmp);
                        lvSupplier.Items.Add(tmp);
                    }
                    databaseConnection.Close();
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            lvSupplier.Items.Clear();
            string query = "INSERT INTO tbl_supplier( `supplierName`,`locationCity`, `modifiedBy`,`dateModified`, `timeModified`, `isPriority`) VALUES (@supplierName, @supplierLocation, @userID, @dateModified, @timeModified, @isPriority)";
            String query2 = "INSERT INTO tbl_contactnumber( `supplierID`, `contactNumber`) VALUES (@supplierID, @contactNumber)";
            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;

            commandDatabase.Parameters.AddWithValue("@supplierName", tbSupplierName.Text);
            commandDatabase.Parameters.AddWithValue("@supplierLocation", tbSupplierLocation.Text);
            commandDatabase.Parameters.AddWithValue("@userID", LoginModel.currentUserID);
            commandDatabase.Parameters.AddWithValue("@dateModified", DateTime.Now);
            commandDatabase.Parameters.AddWithValue("@timeModified", DateTime.Now.ToString("hh:mm tt"));
            commandDatabase.Parameters.AddWithValue("@isPriority", 1);

            if (tbSupplierID.Text != "" || (tbSupplierLocation.Text == "" || tbSupplierName.Text == "" || cbContact.Items.Count == 0))
            {

                MessageBox.Show("Missing Fields or User already exist!");
                loadSuppliers();
                return;

            }
            else
            {
                try
                {

                    databaseConnection.Open();
                    commandDatabase.ExecuteNonQuery();
                    MySqlCommand commandDatabase2 = new MySqlCommand(query2, databaseConnection);
                    foreach(ContactNumberModel tmp in tmpContact)
                    {
                        commandDatabase2.Parameters.Clear();
                        commandDatabase2.Parameters.AddWithValue("@supplierID", latestSupplierID + 1);
                        commandDatabase2.Parameters.AddWithValue("@contactNumber", tmp.contactNumber);
                        MySqlDataReader reader =  commandDatabase2.ExecuteReader();
                        reader.Close();
                    }
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
                    loadLatestNumber();
                }
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            //lvSupplier.Items.Clear();
            String query = "UPDATE tbl_supplier SET isPriority=1 WHERE supplierID=@supplierID";

            SupplierModel selectedSupplier = (SupplierModel)lvSupplier.SelectedItem;
            if(selectedSupplier.isPriority == 1)
            {
                query = "UPDATE `tbl_supplier` SET `supplierName`=@supplierName,`locationCity`=@locationCity, `dateModified`=@dateModified, `timeModified`=@timeModified WHERE `supplierID` =@supplierID";
            }

            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);

            if(selectedSupplier.isPriority == 1)
            {
                commandDatabase.Parameters.AddWithValue("@supplierID", Convert.ToInt16(tbSupplierID.Text));
                commandDatabase.Parameters.AddWithValue("@supplierName", tbSupplierName.Text);
                commandDatabase.Parameters.AddWithValue("@locationCity", tbSupplierLocation.Text);
                commandDatabase.Parameters.AddWithValue("@dateModified", DateTime.Now);
                commandDatabase.Parameters.AddWithValue("@timeModified", DateTime.Now.ToString("hh:mm tt"));
                commandDatabase.CommandTimeout = 60;
                MySqlDataReader reader;

                try
                {
                    databaseConnection.Open();
                    commandDatabase.ExecuteNonQuery();
                    //reader.Close();
                    // Succesfully updated


                }
                catch (Exception ex)
                {
                    // Ops, maybe the id doesn't exists ?
                    MessageBox.Show(ex.Message);
                }

                String query2 = "DELETE FROM tbl_contactnumber WHERE supplierID=@supplierID";
                String query3 = "INSERT INTO tbl_contactnumber( `supplierID`, `contactNumber`) VALUES (@supplierID, @contactNumber)";
                MySqlCommand commandDatabase2 = new MySqlCommand(query2, databaseConnection);
                commandDatabase2.CommandTimeout = 60;
                MySqlDataReader reader2;
                commandDatabase2.Parameters.AddWithValue("@supplierID", Convert.ToInt16(tbSupplierID.Text));
                try
                {
                    commandDatabase2.ExecuteNonQuery();
                    MySqlCommand commandDatabase3 = new MySqlCommand(query3, databaseConnection);
                    foreach (ContactNumberModel tmp in tmpContact)
                    {
                        commandDatabase3.Parameters.Clear();
                        commandDatabase3.Parameters.AddWithValue("@supplierID", Convert.ToInt16(tbSupplierID.Text));
                        commandDatabase3.Parameters.AddWithValue("@contactNumber", tmp.contactNumber);
                        MySqlDataReader reader3 = commandDatabase3.ExecuteReader();
                        MessageBox.Show(tmp.contactNumber.ToString());
                        reader3.Close();
                    }
                    MessageBox.Show("Supplier Successfully Updated!");
                    databaseConnection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    lvSupplier.Items.Clear();
                    loadSuppliers();
                    loadContacts();
                }
            }
            else
            {
                commandDatabase.Parameters.AddWithValue("@supplierID", Convert.ToInt16(tbSupplierID.Text));
                commandDatabase.CommandTimeout = 60;
                MySqlDataReader reader;

                try
                {
                    databaseConnection.Open();
                    reader = commandDatabase.ExecuteReader();
                    MessageBox.Show("Supplier Successfully Updated!");
                    databaseConnection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    lvSupplier.Items.Clear();
                    loadSuppliers();
                    loadContacts();
                }
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            String query = "UPDATE tbl_supplier SET isPriority=0 WHERE supplierID=@supplierID";
            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);

            if (tbSupplierID.Text == "")
            {
                MessageBox.Show("Please select a user!");
                return;
            }

            commandDatabase.Parameters.AddWithValue("@supplierID", Convert.ToInt16(tbSupplierID.Text));
            commandDatabase.CommandTimeout = 60;
            MySqlDataReader reader;
            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();
                databaseConnection.Close();
                loadSuppliers();
                MessageBox.Show("Supplier successfully deleted!");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void lvSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tmpContact.Clear();
            cbContact.Items.Clear();
            if(lvSupplier.SelectedIndex > -1)
            {
                SupplierModel tmp = (SupplierModel)lvSupplier.SelectedItem;
                tbSupplierID.Text = tmp.supplierID.ToString();
                tbSupplierName.Text = tmp.supplierName;
                tbSupplierLocation.Text = tmp.supplierLocation;

                foreach(ContactNumberModel tmp2 in contact)
                {
                    if(tmp.supplierID == tmp2.supplierID)
                    {
                        //tmpContact.Add(new ContactNumberModel() { contactNumber = tmp2.contactNumber});
                        cbContact.Items.Add(tmp2.contactNumber);
                        tmpContact.Add(new ContactNumberModel() { supplierID = tmp2.supplierID, contactNumber = tmp2.contactNumber});
                    }
                    //cbContact.Items.Add(tmpContact);
                }
            }else
            {
                //MessageBox.Show("No selected item!");
            }
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            String query = "";

            if (tbSearch.Text == "")
            {
                loadSuppliers();
                return;
            }

            if (cbSearch.Text == "Supplier ID")
            {
                query = "SELECT * FROM tbl_supplier WHERE supplierID LIKE '%" + tbSearch.Text + "%' AND isPriority=1";
            }
            else if (cbSearch.Text == "Supplier Name")
            {
                query = "SELECT * FROM tbl_supplier WHERE supplierName LIKE '%" + tbSearch.Text + "%' AND isPriority=1";
            }
            else if (cbSearch.Text == "Location(City)")
            {
                query = "SELECT * FROM tbl_supplier WHERE locationCity LIKE '%" + tbSearch.Text + "%' AND isPriority=1";
            }
           

            lvSupplier.Items.Clear();

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
                        SupplierModel tmp;
                            tmp = new SupplierModel { supplierID = reader.GetInt16(0), supplierName = reader.GetString(1), supplierLocation = reader.GetString(2), modifiedBy = reader.GetInt16(3), dateModified = reader.GetDateTime(4).ToString("yyyy-MM-dd"), timeModified = reader.GetString(5), isPriority = reader.GetInt16(6) };                       
                        
                        lvSupplier.Items.Add(tmp);
                    }
                    databaseConnection.Close();
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void btnAddContact_Click(object sender, RoutedEventArgs e)
        {
            foreach(ContactNumberModel tmp in tmpContact)
            {
                if(tmp.contactNumber.ToString() == cbContact.Text)
                {
                    MessageBox.Show("Contact Number already exist!");
                    return;
                }
            }
            cbContact.Items.Add(cbContact.Text);
            try
            {
                tmpContact.Add(new ContactNumberModel() { contactNumber = Convert.ToInt64(cbContact.Text) });
                MessageBox.Show("Number successfully added!");
                cbContact.Text = "";
                //MessageBox.Show(tmpContact[0].contactNumber.ToString());
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDeleteContact_Click(object sender, RoutedEventArgs e)
        {
            if(cbContact.Text == "")
            {
                MessageBox.Show("Please select a number to be deleted!");
                return;
            }
            tmpContact.RemoveAt(cbContact.SelectedIndex);
            MessageBox.Show("Number successfully deleted!");
        }

        private void cbSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbSearch.SelectedItem.ToString() == "Deleted Suppliers")
            {
                loadDeletedSupplier();
                tbSearch.IsEnabled = false;
                return;
            }
            tbSearch.IsEnabled = true;
            loadSuppliers();
        }

        private void cbContact_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
