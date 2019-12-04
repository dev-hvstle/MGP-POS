using POSMGP.Viewmodel;
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
using MySql.Data.MySqlClient;
using POSMGP.View;
using POSMGP.Model;

namespace POSMGP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const String connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=db_mgppos";
        Rectangle darkDockPanel = new Rectangle();
        //LoginView uvUserLogin;

        public MainWindow()
        {
            //uvUserLogin = new LoginView();
            InitializeComponent();
            darkDockPanel.Width = 116;
            darkDockPanel.Height = 720;
            darkDockPanel.Fill = Brushes.Black;
            darkDockPanel.Opacity = 0.4;
            //canvasDockPanel.Children.Add(darkDockPanel);
            //DataContext = new LoginViewModel();
            
        }


        private void MainFrame_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.RightButton == MouseButtonState.Pressed)
            {
                return;
            }
            this.DragMove();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void btnUser_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new UserViewModel();
        }

        private void btnPurchase_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new PosViewModel();
        }


        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            
            if (tbUsername.Text == "admin" && pbPass.Password == "admin")
            {
                MessageBox.Show("Login Successful!");
                btnUser.IsEnabled = true;
                btnPurchase.IsEnabled = true;
                btnSell.IsEnabled = true;
                btnProduct.IsEnabled = true;
                btnLogOut.IsEnabled = true;
                btnSupplier.IsEnabled = true;
                tbUsername.Text = "";
                pbPass.Password = "";
                btnLogin.Visibility = Visibility.Collapsed;
                lblLogin.Visibility = Visibility.Collapsed;
                lblPos.Visibility = Visibility.Collapsed;
                lblUserName.Visibility = Visibility.Collapsed;
                lblPassword.Visibility = Visibility.Collapsed;
                pbPass.Visibility = Visibility.Collapsed;
                tbUsername.Visibility = Visibility.Collapsed;
                lblLogNotif.Visibility = Visibility.Collapsed;
                //canvasDockPanel.Children.Remove(darkDockPanel);
                DataContext = new PosViewModel();
                return;
            }
               
            String dbQuery = "SELECT * FROM tbl_users";
            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand databaseCommand = new MySqlCommand(dbQuery, databaseConnection);
            MySqlDataReader dbReader;

            try
            {
                databaseConnection.Open();
                dbReader = databaseCommand.ExecuteReader();

                if(dbReader.HasRows)
                {
                    //String userName = "", passWord = "";
                    while (dbReader.Read())
                        //MessageBox.Show( " " + uvUserLogin.getUsername);
                    {
                        if((dbReader.GetString(2) == tbUsername.Text && dbReader.GetString(3) == encryptText.ComputeSha256Hash(pbPass.Password)) && (dbReader.GetString(1) == "Admin" || dbReader.GetString(1) == "SuperAdmin"))
                        {
                           // MessageBox.Show(dbReader.GetString(5) + " " + uvUserLogin.tbUserName);   
                            MessageBox.Show("MAHAL NA MAHAL KITA HARVEY SANA MAPAGTAGUMPAYAN MO ANG PAG COCODE PARA SA GRUPO MO. GOODLUCK I LOVE YOU! -Ken");
                            btnUser.IsEnabled = true;
                            btnPurchase.IsEnabled = true;
                            btnSell.IsEnabled = true;
                            btnProduct.IsEnabled = true;
                            btnLogOut.IsEnabled = true;
                            btnSupplier.IsEnabled = true;
                            tbUsername.Text = "";
                            pbPass.Password = "";
                            btnLogin.Visibility = Visibility.Collapsed;
                            lblLogin.Visibility = Visibility.Collapsed;
                            lblPos.Visibility = Visibility.Collapsed;
                            lblUserName.Visibility = Visibility.Collapsed;
                            lblPassword.Visibility = Visibility.Collapsed; 
                            pbPass.Visibility = Visibility.Collapsed;
                            tbUsername.Visibility = Visibility.Collapsed;
                            lblLogNotif.Visibility = Visibility.Collapsed;
                            //canvasDockPanel.Children.Remove(darkDockPanel);
                            LoginModel.currentUser = dbReader.GetString(5);
                            LoginModel.currentUserID = dbReader.GetInt16(0);
                            LoginModel.userRights = dbReader.GetString(1);
                            databaseConnection.Close();
                            DataContext = new PosViewModel();
                            return;
                        }else if((dbReader.GetString(2) == tbUsername.Text && dbReader.GetString(3) == encryptText.ComputeSha256Hash(pbPass.Password)) && (dbReader.GetString(1) == "Parametric"))
                        {

                            MessageBox.Show("MAHAL NA MAHAL KITA HARVEY SANA MAPAGTAGUMPAYAN MO ANG PAG COCODE PARA SA GRUPO MO. GOODLUCK I LOVE YOU! -Ken");
                            //btnUser.IsEnabled = true;
                            btnPurchase.IsEnabled = true;
                            btnSell.IsEnabled = true;
                            //btnProduct.IsEnabled = true;
                            btnLogOut.IsEnabled = true;
                            //btnSupplier.IsEnabled = true;
                            tbUsername.Text = "";
                            pbPass.Password = "";
                            btnLogin.Visibility = Visibility.Collapsed;
                            lblLogin.Visibility = Visibility.Collapsed;
                            lblPos.Visibility = Visibility.Collapsed;
                            lblUserName.Visibility = Visibility.Collapsed;
                            lblPassword.Visibility = Visibility.Collapsed;
                            pbPass.Visibility = Visibility.Collapsed;
                            tbUsername.Visibility = Visibility.Collapsed;
                            lblLogNotif.Visibility = Visibility.Collapsed;
                            //canvasDockPanel.Children.Remove(darkDockPanel);
                            LoginModel.currentUser = dbReader.GetString(5);
                            LoginModel.currentUserID = dbReader.GetInt16(0);
                            LoginModel.userRights = dbReader.GetString(1);
                            databaseConnection.Close();
                            DataContext = new PosViewModel();
                            return;
                        }
                    }
                }
                lblLogNotif.Content = "Invalid Credentials!";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            Rectangle bg = new Rectangle();
            bg.Width = 1280;
            bg.Height = 720;
            bg.Fill = Brushes.Black;
            bg.Opacity = 0.5;

            darkDockPanel.Width = 116;
            darkDockPanel.Height = 720;
            darkDockPanel.Fill = Brushes.Black;

            MainCanvas.Children.Add(bg);
            switch (MessageBox.Show("Do you want to Log out?","Logout", MessageBoxButton.YesNo, MessageBoxImage.Warning))
            {
                case MessageBoxResult.Yes:
                    DataContext = null;
                    btnUser.IsEnabled = false;
                    btnPurchase.IsEnabled = false;
                    btnSell.IsEnabled = false;
                    btnProduct.IsEnabled = false;
                    btnLogOut.IsEnabled = false;
                    btnSupplier.IsEnabled = false;
                    btnLogin.Visibility = Visibility.Visible;
                    lblLogin.Visibility = Visibility.Visible;
                    lblPos.Visibility = Visibility.Visible;
                    lblUserName.Visibility = Visibility.Visible;
                    lblPassword.Visibility = Visibility.Visible;
                    pbPass.Visibility = Visibility.Visible;
                    tbUsername.Visibility = Visibility.Visible;
                    MainCanvas.Children.Remove(bg);
                    //MainCanvas.Children.Add(darkDockPanel);
                    break;

                case MessageBoxResult.No:
                    MainCanvas.Children.Remove(bg);
                    break;
            }
            
        }

        private void btnSell_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new TransactionViewModel();
        }

        private void btnProduct_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new ProductsViewModel();
        }

        private void btnSupplier_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new SupplierViewModel();
        }
    }
}
