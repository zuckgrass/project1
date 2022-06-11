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
    /// Interaction logic for Zvit1.xaml
    /// </summary>
    public partial class Zvit1 : Window
    {
        public Zvit1()
        {
            InitializeComponent();
            InitDatatable();
        }
        static string Connection = @"Data Source=DESKTOP-EN4LANA; initial Catalog=List of bands; Integrated Security=True";
        static DataTable dT = new DataTable("Виконавці");
        static DataTable dT1 = new DataTable("Репертуар");

        public void InitDatatable()
        {
            SqlConnection sqlConn = new SqlConnection(Connection);
            sqlConn.Open();
            if (sqlConn.State == System.Data.ConnectionState.Open)
            {
                SqlDataAdapter Data = new SqlDataAdapter("SELECT MemberName as [Виконавець] FROM Members;", sqlConn);
                dT.Clear();
                Data.Fill(dT);
                dataGrid.ItemsSource = dT.DefaultView;
                SqlDataAdapter Data1 = new SqlDataAdapter("SELECT Name as [Пісня] FROM Songs;", sqlConn);
                dT1.Clear();
                Data1.Fill(dT1);
                dataGrid1.ItemsSource = dT1.DefaultView;
            }
            sqlConn.Close();
        }

        private void Main_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            Hide();
            mw.Show();
        }

        private void Filter1_Click(object sender, RoutedEventArgs e)
        {
            if (Group.Text != "")
            {
                SqlConnection sqlConn = new SqlConnection(Connection);
                sqlConn.Open();
                string strQ1;
                strQ1 = "SELECT IDGroup FROM Groups WHERE Name='" + Group.Text + "';";
                SqlCommand Com = new SqlCommand(strQ1, sqlConn);
                Com.ExecuteNonQuery();
                if (Com.ExecuteScalar() != null)
                {
                    int id = int.Parse(Com.ExecuteScalar().ToString());
                    SqlDataAdapter Data = new SqlDataAdapter("SELECT MemberName as [Виконавець] FROM Members WHERE IDGroup='" + id + "';", sqlConn);
                    dT.Clear();
                    Data.Fill(dT);
                    dataGrid.ItemsSource = dT.DefaultView;
                    SqlDataAdapter Data1 = new SqlDataAdapter("SELECT Name as [Пісня] FROM Songs WHERE IDGroup='" + id + "';", sqlConn);
                    dT1.Clear();
                    Data1.Fill(dT1);
                    dataGrid1.ItemsSource = dT1.DefaultView;
                }
                else
                    MessageBox.Show("Неправильно введена назва гурту або такого гурту немає у базі!");
            }
            else
                MessageBox.Show("Введіть назву групи!");
        }

        private void Filter2_Click(object sender, RoutedEventArgs e)
        {
            if (Author.Text != "")
            {
                SqlConnection sqlConn = new SqlConnection(Connection);
                sqlConn.Open();
                string strQ1;
                strQ1 = "SELECT IDMember FROM Members WHERE MemberName='" + Author.Text + "';";
                SqlCommand Com = new SqlCommand(strQ1, sqlConn);
                Com.ExecuteNonQuery();
                if (Com.ExecuteScalar() != null)
                {
                    int id = int.Parse(Com.ExecuteScalar().ToString());
                    SqlDataAdapter Data = new SqlDataAdapter("SELECT MemberName as [Виконавець] FROM Members WHERE IDMember='" + id + "';", sqlConn);
                    dT.Clear();
                    Data.Fill(dT);
                    dataGrid.ItemsSource = dT.DefaultView;
                    SqlDataAdapter Data1 = new SqlDataAdapter("SELECT Name as [Пісня] FROM Songs WHERE Composer='" + id + "' OR SongWriter='" + id + "';", sqlConn);
                    dT1.Clear();
                    Data1.Fill(dT1);
                    dataGrid1.ItemsSource = dT1.DefaultView;
                }
                else
                    MessageBox.Show("Неправильно введена ім'я музиканта або такого виконавця немає у базі!");
            }
            else
                MessageBox.Show("Введіть ім'я та прізвище музиканта!");
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            InitDatatable();
        }
    }
}
