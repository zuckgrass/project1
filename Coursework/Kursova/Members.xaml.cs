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
using System.IO;

namespace Kursova
{
    /// <summary>
    /// Interaction logic for Members.xaml
    /// </summary>
    public partial class Members : Window
    {
        string GroupID;
        public Members()
        {
            InitializeComponent();
            StreamReader group = new StreamReader("Group");
            string Group = group.ReadLine().Trim();
            SqlConnection sqlConn = new SqlConnection(Connection);
            sqlConn.Open();
            string strQ1;
            strQ1 = "SELECT Name FROM Groups WHERE IDGroup = '" + Group + "'; ";
            SqlCommand Com = new SqlCommand(strQ1, sqlConn);
            Com.ExecuteNonQuery();
            lmem.Content = "Склад гурту " + Com.ExecuteScalar().ToString();
            group.Dispose();
            group.Close();
            GroupID = Group;
            image.Source = BitmapFrame.Create(new Uri(@"C:\Users\38099\Desktop\Роботи\Основи програмування\Курсач\Картинки гурту\" + Group + ".jpg"));
            InitDataTable(Group);
        }
        string Connection = @"Data Source=DESKTOP-EN4LANA; initial Catalog=List of bands; Integrated Security=True";
        DataTable dT = new DataTable("Музичні групи");
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            Hide();
            mw.Show();
        }
        public void InitDataTable(string Group)
        {
            SqlConnection sqlConn = new SqlConnection(Connection);
            sqlConn.Open();
            if (sqlConn.State == System.Data.ConnectionState.Open)
            {
                SqlDataAdapter Data = new SqlDataAdapter("SELECT MemberName as [Виконавець], Age as [Вік], Am.Name as [Амплуа] FROM Members " +
                                                         "Mem LEFT JOIN Amplua Am " +
                                                         "ON Mem.IDAmplua = Am.IDAmplua " +
                                                         "WHERE Mem.IDGroup= '" + Group + "'; ", sqlConn);
                dT.Clear();
                Data.Fill(dT);
                dataGrid.ItemsSource = dT.DefaultView;
            }
            sqlConn.Close();
        }

        private void AddMember_Click(object sender, RoutedEventArgs e)
        {
            int ampl = Amplua.SelectedIndex + 1;
            if (ampl != 0 && Member.Text != "" && Age.Text != "")
            {
                SqlConnection sqlConn = new SqlConnection(Connection);
                sqlConn.Open();
                if (sqlConn.State == System.Data.ConnectionState.Open)
                {
                    SqlDataAdapter Data = new SqlDataAdapter("INSERT INTO Members(IDMember, IDGroup, MemberName, Age, IDAmplua)" +
                                                            "VALUES((SELECT Max(IDMember) FROM Members)+1, '" + GroupID + "', '" + Member.Text + "', " + Age.Text + ", " + ampl + ");", sqlConn);
                    dT.Clear();
                    Data.Fill(dT);
                    dataGrid.ItemsSource = dT.DefaultView;
                }
                sqlConn.Close();
                InitDataTable(GroupID);
            }
            else
                MessageBox.Show("Введіть усі дані!");
        }

        private void DeleteMember_Click(object sender, RoutedEventArgs e)
        {
            if (Member.Text != "")
            {
                SqlConnection sqlConn = new SqlConnection(Connection);
                sqlConn.Open();
                if (sqlConn.State == System.Data.ConnectionState.Open)
                {
                    SqlDataAdapter Data = new SqlDataAdapter("DELETE FROM Members WHERE MemberName ='" + Member.Text + "' AND IDGroup=" + GroupID + ";", sqlConn);
                    dT.Clear();
                    Data.Fill(dT);
                    dataGrid.ItemsSource = dT.DefaultView;
                }
                sqlConn.Close();
                InitDataTable(GroupID);
            }
            else
                MessageBox.Show("Введіть ім'я виконавця!");
        }
    }
}
