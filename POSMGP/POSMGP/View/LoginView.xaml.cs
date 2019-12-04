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
using POSMGP;
using POSMGP.Viewmodel;

namespace POSMGP.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }

     public String getUsername{
            get { return tbUserName.Text; }
            set { tbUserName.Text = value; }
        }
     public String getPassword
        {
            get{ return tbPassword.Text; }
            set{ tbPassword.Text =  value;}
        } 
    }
}
