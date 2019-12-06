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

namespace POSMGP.View
{
    /// <summary>
    /// Interaction logic for UserView.xaml
    /// </summary>
    public partial class UserView : UserControl
    {

        List<UserModel> userList = new List<UserModel>();
        UserModel user;
        const String connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=db_mgppos";

        public UserView()
        {
            InitializeComponent();
            if (LoginModel.userRights == "SuperAdmin")
            {
                btnUpdateText.Text = "Update/Retrieve";
                cbSearch.Items.Add("Deleted User");

            }
            loadUsers();
            loadUserType();
        }


        void loadUserType()
        {
            String query = "SELECT * FROM tbl_usertype";

            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand databaseCommand = new MySqlCommand(query, databaseConnection);
            databaseCommand.CommandTimeout = 60;
            MySqlDataReader reader;

            try
            {
                databaseConnection.Open();
                reader = databaseCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (LoginModel.userRights == "SuperUser") { 
                            UserTypeModel tmp = new UserTypeModel { userType = reader.GetString(0) };
                            cbUserType.Items.Add(tmp.userType);
                        }
                        else
                        {
                            UserTypeModel tmp = new UserTypeModel { userType = reader.GetString(0) };
                            if (tmp.userType != "SuperAdmin")
                            {
                                cbUserType.Items.Add(tmp.userType);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void loadDeletedUser()
        {
            String query = "SELECT * FROM tbl_users WHERE isPriority=0";
            tbSearch.IsEnabled = false;
            lvUser.Items.Clear();

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
                        UserModel tmp = new UserModel { userID = reader.GetInt16(0), userType = reader.GetString(1), userName = reader.GetString(2), userPass = reader.GetString(3), userFName = reader.GetString(4), userMName = reader.GetString(5), userLName = reader.GetString(6), dateModified = reader.GetDateTime(7).ToString("yyyy-MM-dd"), timeModified = reader.GetString(8) ,isPriority = reader.GetInt16(9) };

                        userList.Add(tmp);
                        lvUser.Items.Add(tmp);
                    }
                    databaseConnection.Close();
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void loadUsers()
        {
            lvUser.Items.Clear();
            String query = "SELECT * FROM tbl_users WHERE isPriority=1 AND userType<>'SuperAdmin'";

            if (LoginModel.userRights == "SuperAdmin")
            {
                query = "SELECT * FROM tbl_users";
            }

            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query,databaseConnection);
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
                        UserModel tmp;
                        if (LoginModel.userRights == "SuperAdmin")
                        {
                            tmp = new UserModel { userID = reader.GetInt16(0), userType = reader.GetString(1), userName = reader.GetString(2), userPass = reader.GetString(3), userFName = reader.GetString(4), userMName = reader.GetString(5), userLName = reader.GetString(6), dateModified = reader.GetDateTime(7).ToString("yyyy-MM-dd"), timeModified = reader.GetString(8), isPriority = reader.GetInt16(9) };
                        }else
                        {
                            tmp = new UserModel { userID = reader.GetInt16(0), userType = reader.GetString(1), userName = reader.GetString(2), userPass = reader.GetString(3), userFName = reader.GetString(4), userMName = reader.GetString(5), userLName = reader.GetString(6), dateModified = reader.GetDateTime(7).ToString("yyyy-MM-dd"), timeModified = reader.GetString(8) };
                        }
                           
                        userList.Add(tmp);
                        lvUser.Items.Add(tmp);
                    }
                        databaseConnection.Close();
                }

            }catch(Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void lvUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lvUser.SelectedIndex > -1)
            {

                user = (UserModel)lvUser.SelectedItem;
                tbUserID.Text = user.userID.ToString();
                tbUserName.Text = user.userName;
                tbPassword.Text = user.userPass;
                cbUserType.Text = user.userType;
                tbFirstName.Text = user.userFName;
                tbMiddleName.Text = user.userMName;
                tbLastName.Text = user.userLName;

            }else
            {

            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            lvUser.Items.Clear();
            string query = "INSERT INTO tbl_users(`userFirstName`, `userMiddleName`,`userLastName`,`userPassword`,`userName`,`userType`, `dateModified`, `timeModified`) VALUES (@userFName, @userMName, @userLName, @userPass, @userName, @userType, @dateModified, @timeModified)";
            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;

            commandDatabase.Parameters.AddWithValue("@userFName", tbFirstName.Text);
            commandDatabase.Parameters.AddWithValue("@userMName", tbMiddleName.Text);
            commandDatabase.Parameters.AddWithValue("@userLName", tbLastName.Text);
            commandDatabase.Parameters.AddWithValue("@userPass", encryptText.ComputeSha256Hash(tbPassword.Text));
            commandDatabase.Parameters.AddWithValue("@userName", tbUserName.Text);
            commandDatabase.Parameters.AddWithValue("@userType", cbUserType.Text);
            commandDatabase.Parameters.AddWithValue("@dateModified", DateTime.Today);
            commandDatabase.Parameters.AddWithValue("@timeModified", DateTime.Now.ToString("hh:mm tt"));

            //foreach(UserModel tmp in userList)
            //{
            //    if (tmp.userID == Convert.ToInt16(tbUserID.Text))     
            //        MessageBox.Show("User Already Exist!"); return;
            //}

            if (tbUserID.Text != "" || (tbFirstName.Text == "" || tbLastName.Text == "" || tbUserName.Text == "" || tbPassword.Text == "" || cbUserType.Text == "" ))
            {

                MessageBox.Show("Missing fields or user already exists!");
                return;

            }else
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
                    loadUsers();
                }
            }
            
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            String query = "";
            //int retriveOrNot = 0;
            MySqlConnection databaseConnection = new MySqlConnection(connectionString);

            UserModel tmp = (UserModel)lvUser.SelectedItem;
            if (tmp.isPriority == 1)
            {
                //query = "UPDATE tbl_users SET isPriority=@isPriority";
                //retriveOrNot = 1;
                query = "UPDATE `tbl_users` SET `userFirstName`=@userFName,`userMiddleName`=@userMName, `userLastName`=@userLName, `userType`=@userType, `userName`=@userName, `userPassword`=@userPass, `dateModified`=@dateModified, `timeModified`=@timeModified WHERE userID = @userID";
            }
            else
            {
                query = "UPDATE tbl_users SET isPriority=@isPriority";
                //query = "UPDATE `tbl_users` SET `userFirstName`=@userFName,`userMiddleName`=@userMName, `userLastName`=@userLName, `userType`=@userType, `userName`=@userName, `userPassword`=@userPass, `dateModified`=@dateModified, `timeModified`=@timeModified WHERE userID = @userID";
            }

            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);

            if (tmp.isPriority == 1)
            {
                //commandDatabase.Parameters.AddWithValue("@isPriority", 1);
                commandDatabase.Parameters.AddWithValue("@userID", Convert.ToInt16(tbUserID.Text));
                commandDatabase.Parameters.AddWithValue("@userFName", tbFirstName.Text);
                commandDatabase.Parameters.AddWithValue("@userMName", tbMiddleName.Text);
                commandDatabase.Parameters.AddWithValue("@userLName", tbLastName.Text);
                commandDatabase.Parameters.AddWithValue("@userType", cbUserType.Text);
                commandDatabase.Parameters.AddWithValue("@userName", tbUserName.Text);
                commandDatabase.Parameters.AddWithValue("@userPass", encryptText.ComputeSha256Hash(tbPassword.Text));
                commandDatabase.Parameters.AddWithValue("@dateModified", DateTime.Today);
                commandDatabase.Parameters.AddWithValue("@timeModified", DateTime.Now.ToString("hh:mm tt"));

            }
            else
            {
                commandDatabase.Parameters.AddWithValue("@isPriority", 1);
            }

            commandDatabase.CommandTimeout = 60;
            MySqlDataReader reader;

            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();
                MessageBox.Show("Details Successfully Updated!");

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
                if (cbSearch.Text == "Deleted User")
                {
                    loadDeletedUser();

                }
                else
                {
                    loadUsers();
                }
            }
        }

        private void btnResetFields_Click(object sender, RoutedEventArgs e)
        {
            tbUserID.Text = "";
            tbUserName.Text = "";
            tbPassword.Text = "";
            cbUserType.Text = "";
            tbFirstName.Text = "";
            tbMiddleName.Text = "";
            tbLastName.Text = "";
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            String query = "UPDATE tbl_users SET isPriority=0 WHERE userID=@userID";
            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);

            if (tbUserID.Text == "")
            {
                MessageBox.Show("Please select a user!");
                return;
            }

            commandDatabase.Parameters.AddWithValue("@userID", Convert.ToInt16(tbUserID.Text));
            commandDatabase.CommandTimeout = 60;
            MySqlDataReader reader;
            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();
                databaseConnection.Close();
                loadUsers();
                MessageBox.Show("User successfully deleted!");

            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            String query = "";

            if(tbSearch.Text == "")
            {
                loadUsers();
                return;
            }

            if (cbSearch.Text == "First Name")
            {
                query = "SELECT * FROM tbl_users WHERE userFirstName LIKE '%" + tbSearch.Text + "%' AND isPriority=1";
            }
            else if(cbSearch.Text == "Last Name")
            {
                query = "SELECT * FROM tbl_users WHERE userLastName LIKE '%" + tbSearch.Text + "%' AND isPriority=1";
            }
            else if (cbSearch.Text == "User Name")
            {
                query = "SELECT * FROM tbl_users WHERE userName LIKE '%" + tbSearch.Text + "%' AND isPriority=1";
            }
            else if(cbSearch.Text == "Deleted User")
            {
                query = "SELECT * FROM tbl_users WHERE isPriority=0";
            }

            lvUser.Items.Clear();

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
                        UserModel tmp;
                        
                        tmp = new UserModel { userID = reader.GetInt16(0), userType = reader.GetString(1), userName = reader.GetString(2), userPass = reader.GetString(3), userFName = reader.GetString(4), userMName = reader.GetString(5), userLName = reader.GetString(6), dateModified = reader.GetDateTime(7).ToString("yyyy-MM-dd"), isPriority = reader.GetInt16(8) };
                        userList.Add(tmp);
                        lvUser.Items.Add(tmp);
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
            if(cbSearch.SelectedItem.ToString() == "Deleted User")
            {
                loadDeletedUser();
            }
            else
            {
                loadUsers();
                tbSearch.IsEnabled = true;
            }
        }
    }
}
