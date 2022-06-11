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
using System.Data;
using System.Data.SqlClient;

namespace Kursova
{
    /// <summary>
    /// Interaction logic for AddGroup.xaml
    /// </summary>
    public partial class AddGroup : Window
    {
        public AddGroup()
        {
            InitializeComponent();
            Update();
        }
        DataTable dT = new DataTable("Музичні групи");
        string Connection = @"Data Source=DESKTOP-EN4LANA; initial Catalog=List of bands; Integrated Security=True";
        private void GoToMain_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            Hide();
            mw.Show();
        }

        private void AddGroup1_Click(object sender, RoutedEventArgs e)
        {
            if (GroupName.Text != "" && Year.Text != "" && Country.Text != "")
            {
                SqlConnection sqlConn = new SqlConnection(Connection);
                sqlConn.Open();
                if (sqlConn.State == System.Data.ConnectionState.Open)
                {
                    SqlDataAdapter Data = new SqlDataAdapter("INSERT INTO Groups (IDGroup, Name,[Foundation year],IDCountry) " +
                                                            "SELECT a,'" + GroupName.Text + "', '" + Year.Text + "', IDCountry FROM Country " +
                                                            "CROSS JOIN (SELECT Max(IDGroup)+1 a FROM Groups) b " +
                                                            "WHERE Country.Name='" + Country.Text + "' " +
                                                            "AND '" + GroupName.Text + "' NOT IN(SeLECT Name FROM Groups); " +
                                                            "INSERT INTO GroupsinParad (IDGroup, Nhitparad) " +
                                                            "SELECT gr.IDGroup, a FROM Groups gr " +
                                                            "CROSS JOIN (SELECT Max(Nhitparad)+1 a FROM GroupsinParad) b " +
                                                            "WHERE gr.Name='" + GroupName.Text + "'" +
                                                            "AND gr.IDGroup NOT IN(SeLECT IDGroup FROM GroupsinParad)", sqlConn);
                    dT.Clear();
                    Data.Fill(dT);
                    dataGrid.ItemsSource = dT.DefaultView;
                }
                sqlConn.Close();
                Update();
            }
            else
                MessageBox.Show("Заповніть пропуски");
        }
        private void Update()
        {
            SqlConnection sqlConn = new SqlConnection(Connection);
            sqlConn.Open();
            if (sqlConn.State == System.Data.ConnectionState.Open)
            {
                SqlDataAdapter Data = new SqlDataAdapter("SELECT hit.NhitParad as [№ у хіт-параді], gr.Name  as [Назва гурту], [Foundation year] as [Рік заснування], con.Name as [Країна походження] FROM Groups " +
                                                        "gr LEFT JOIN GroupsinParad hit ON gr.IDGroup=hit.IDGroup " +
                                                        "Left join Country con ON gr.IDCountry=con.IDCountry; ", sqlConn);
                dT.Clear();
                Data.Fill(dT);
                dataGrid.ItemsSource = dT.DefaultView;
            }
            sqlConn.Close();
        }
    }
}
