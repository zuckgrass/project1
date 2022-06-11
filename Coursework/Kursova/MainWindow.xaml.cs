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
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace Kursova
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            UpdateDataTable();
        }

        string Connection = @"Data Source=DESKTOP-EN4LANA; initial Catalog=List of bands; Integrated Security=True";
        DataTable dT = new DataTable("Музичні групи");
        DataTable dT1 = new DataTable("Ювілеї груп");
        DataTable dT2 = new DataTable("Наймолодший вокаліст з усіх груп");
        DataTable dT3 = new DataTable("Групи з середнім віком меншим за 31");
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        public void UpdateDataTable()
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
                SqlDataAdapter Data1 = new SqlDataAdapter("SELECT Name as [Назва гурту], [Foundation year] as [Рік заснування], Year(getdate())-[Foundation year] as [Років виповнюється] FROM Groups " +
                                                       "WHERE Year(getdate())-[Foundation year] in (5,10,15,25, 30, 35, 40, 45, 50, 55, 60);", sqlConn);
                dT1.Clear();
                Data1.Fill(dT1);
                dataGrid1.ItemsSource = dT1.DefaultView;
                SqlDataAdapter Data2 = new SqlDataAdapter("SELECT TOP (1) MemberName as [Виконавець], Age as [Вік], gr.Name as [Назва гурту] From Members " +
                                                       "mem LEFT JOIN Amplua am ON mem.IDAmplua=am.IDAmplua " +
                                                       "LEFT JOIN Groups gr ON mem.IDGroup=gr.IDGroup " +
                                                       "WHERE mem.IDAmplua='1' " +
                                                       "ORDER BY mem.Age; ", sqlConn);
                dT2.Clear();
                Data2.Fill(dT2);
                dataGrid2.ItemsSource = dT2.DefaultView;
                SqlDataAdapter Data3 = new SqlDataAdapter("SELECT Name as [Назва гурту], sum(mem.Age)/COUNT (*) as [Середній вік] FROM Groups gr " +
                                                       "LEFT JOIN Members mem ON mem.IDGroup = gr.IDGroup " +
                                                       "GROUP BY Name " +
                                                       "HAVING sum(mem.Age)/COUNT (*)<30 " +
                                                       "ORDER BY sum(mem.Age)/COUNT (*); ", sqlConn);
                dT3.Clear();
                Data3.Fill(dT3);
                dataGrid3.ItemsSource = dT3.DefaultView;
            }
            sqlConn.Close();
        }

        private void Choice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Choice.SelectedIndex == 0)
            {
                Nhitparad.IsEnabled = false;
                lhit.Visibility = Visibility.Hidden;
                GroupName.IsEnabled = true;
                lgroup.Visibility = Visibility.Visible;
            }
            else
            {
                GroupName.IsEnabled = false;
                lgroup.Visibility = Visibility.Hidden;
                Nhitparad.IsEnabled = true;
                lhit.Visibility = Visibility.Visible;
            }
        }

        private void Members_Click(object sender, RoutedEventArgs e)
        {
            if (NuminFile() == true)
            {
                Members mem = new Members();
                Hide();
                mem.Show();
            }
        }

        private void Repertuar_Click(object sender, RoutedEventArgs e)
        {
            if (NuminFile() == true)
            {
                Songs rep = new Songs();
                Hide();
                rep.Show();
            }
        }

        private void Gastroli_Click(object sender, RoutedEventArgs e)
        {
            if (NuminFile() == true)
            {
                Gastroli gas = new Gastroli();
                Hide();
                gas.Show();
            }
        }

        public bool NuminFile()
        {
            if (Choice.SelectedIndex == 0)
            {
                if (GroupName.Text != "")
                {
                    SqlConnection sqlConn = new SqlConnection(Connection);
                    sqlConn.Open();
                    string strQ1;
                    strQ1 = "SELECT IDGroup FROM Groups WHERE Name = '" + GroupName.Text + "'; ";
                    SqlCommand Com = new SqlCommand(strQ1, sqlConn);
                    Com.ExecuteNonQuery();
                    if (Com.ExecuteScalar() == null || Com.ExecuteScalar().ToString() == "")
                    {
                        MessageBox.Show("Групи з такою назвою немає у базі!");
                        return false;
                    }
                    else
                    {
                        string num = Com.ExecuteScalar().ToString();
                        sqlConn.Close();
                        StreamWriter group = new StreamWriter("Group");
                        group.WriteLine(num);
                        group.Close();
                        return true;
                    }
                }
                else
                {
                    MessageBox.Show("Введіть назву гурту");
                    return false;
                }
            }
            else
            {
                if (Nhitparad.Text != "")
                {
                    SqlConnection sqlConn = new SqlConnection(Connection);
                    sqlConn.Open();
                    string strQ1;
                    strQ1 = "SELECT IDGroup FROM GroupsinParad WHERE Nhitparad = '" + Nhitparad.Text + "'; ";
                    SqlCommand Com = new SqlCommand(strQ1, sqlConn);
                    Com.ExecuteNonQuery();
                    if (Com.ExecuteScalar() == null || Com.ExecuteScalar().ToString() == "")
                    {
                        MessageBox.Show("Виберіть групу у межах ТОП-50!");
                        return false;
                    }
                    else
                    {
                        string num = Com.ExecuteScalar().ToString();
                        sqlConn.Close();
                        StreamWriter group = new StreamWriter("Group");
                        group.WriteLine(num);
                        group.Close();
                        return true;
                    }
                }
                else
                {
                    MessageBox.Show("Введіть назву гурту");
                    return false;
                }
            }
        }

        private void Discover_Click(object sender, RoutedEventArgs e)
        {
            if (com1.SelectedIndex == -1 || com2.SelectedIndex == -1)
                MessageBox.Show("Ви не обрали умову в одному з комбобоксів або в обох!");
            else
            {
                SqlConnection sqlConn = new SqlConnection(Connection);
                sqlConn.Open();
                if (sqlConn.State == System.Data.ConnectionState.Open)
                {
                    string Compare, yearold;
                    if (com1.SelectedIndex == 0)
                        Compare = "<";
                    else
                        Compare = ">";
                    yearold = com2.Text;
                    SqlDataAdapter Data3 = new SqlDataAdapter("SELECT Name as [Назва гурту], sum(mem.Age)/COUNT (*) as [Середній вік] FROM Groups gr " +
                                                       "LEFT JOIN Members mem ON mem.IDGroup = gr.IDGroup " +
                                                       "GROUP BY Name " +
                                                       "HAVING sum(mem.Age)/COUNT (*)" + Compare + yearold +
                                                       "ORDER BY sum(mem.Age)/COUNT (*); ", sqlConn);
                    dT3.Clear();
                    Data3.Fill(dT3);
                    dataGrid3.ItemsSource = dT3.DefaultView;
                }
                sqlConn.Close();
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddGroup add = new AddGroup();
            Hide();
            add.Show();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (Choice.SelectedIndex == 0 && GroupName.Text != "")
            {
                SqlConnection sqlConn = new SqlConnection(Connection);
                sqlConn.Open();
                if (sqlConn.State == System.Data.ConnectionState.Open)
                {
                    string strQ1;
                    strQ1 = "SELECT Nhitparad FROM GroupsinParad WHERE IDGroup=(Select IDGroup FROM Groups WHERE Name='" + GroupName.Text + "');";
                    SqlCommand Com = new SqlCommand(strQ1, sqlConn);
                    Com.ExecuteNonQuery();
                    int idparad = int.Parse(Com.ExecuteScalar().ToString());
                    SqlDataAdapter Data = new SqlDataAdapter("DELETE FROM Groups WHERE Name='" + GroupName.Text + "' " +
                                                             "Update GroupsinParad Set Nhitparad=Nhitparad-1 Where Nhitparad>'" + idparad + "'", sqlConn);
                    dT.Clear();
                    Data.Fill(dT);
                    dataGrid.ItemsSource = dT.DefaultView;
                }
                sqlConn.Close();
                UpdateDataTable();
            }
            else if (Choice.SelectedIndex == 1 && Nhitparad.Text != "")
            {
                SqlConnection sqlConn = new SqlConnection(Connection);
                sqlConn.Open();
                if (sqlConn.State == System.Data.ConnectionState.Open)
                {
                    SqlDataAdapter Data = new SqlDataAdapter("DELETE FROM Groups WHERE IDGroup =" +
                                                             "(SELECT IDGroup FROM GroupsinParad WHERE Nhitparad='" + Nhitparad.Text + "')" +
                                                             "Update GroupsinParad Set Nhitparad=Nhitparad-1 Where Nhitparad>'" + Nhitparad.Text + "'", sqlConn);
                    dT.Clear();
                    Data.Fill(dT);
                    dataGrid.ItemsSource = dT.DefaultView;
                }
                sqlConn.Close();
                UpdateDataTable();
            }
            else
            {
                MessageBox.Show("Введіть дані про групу!");
            }
        }

        private void Zvit1_Click(object sender, RoutedEventArgs e)
        {
            Zvit1 add = new Zvit1();
            Hide();
            add.Show();
        }

        private void Zvit2_Click(object sender, RoutedEventArgs e)
        {
            Zvit2 add = new Zvit2();
            Hide();
            add.Show();
        }
    }
}
