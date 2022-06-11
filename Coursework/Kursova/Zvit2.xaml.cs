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
    /// Interaction logic for Zvit2.xaml
    /// </summary>
    public partial class Zvit2 : Window
    {
        public Zvit2()
        {
            InitializeComponent();
            InitDatatable();
        }
        static string Connection = @"Data Source=DESKTOP-EN4LANA; initial Catalog=List of bands; Integrated Security=True";
        static DataTable dT = new DataTable("Гастролі");
        static DataTable dT1 = new DataTable("Міста");
        static DataTable dT2 = new DataTable("Репертуар");
        public void InitDatatable()
        {
            SqlConnection sqlConn = new SqlConnection(Connection);
            sqlConn.Open();
            if (sqlConn.State == System.Data.ConnectionState.Open)
            {
                SqlDataAdapter Data = new SqlDataAdapter("SELECT Name as [Назва гастрольної програми], DATEDIFF(day, Start, Finish) as [Тривалість(у днях)], " +
                                                         "AvPrice as [Середня ціна], SUM(con.AmountTickets) as [К-сть проданих квитків], " +
                                                         "AvPrice*SUM(con.AmountTickets) as [Виручка] FROM Gastroli gas " +
                                                         "LEFT JOIN GasConCit con ON con.IDGastroli=gas.IDGastroli " +
                                                         "GROUP BY Name, Start, Finish, AvPrice;", sqlConn);
                dT.Clear();
                Data.Fill(dT);
                dataGrid.ItemsSource = dT.DefaultView;
                SqlDataAdapter Data1 = new SqlDataAdapter("SELECT Name as [Місто] FROM City;", sqlConn);
                dT1.Clear();
                Data1.Fill(dT1);
                dataGrid1.ItemsSource = dT1.DefaultView;
                SqlDataAdapter Data2 = new SqlDataAdapter("SELECT Son.Name as [Назва пісні], mem.MemberName as [Композитор], mem1.MemberName as [Автор слів пісні] FROM Songs " +
                                                        "Son Left JOIN Members mem ON Son.Composer=mem.IDMember " +
                                                        "Left JOIN Members mem1 ON Son.SongWriter=mem1.IDMember;", sqlConn);
                dT2.Clear();
                Data2.Fill(dT2);
                dataGrid2.ItemsSource = dT2.DefaultView;
            }
            sqlConn.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            Hide();
            mw.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            InitDatatable();
        }

        private void GroupFilter_Click(object sender, RoutedEventArgs e)
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
                    SqlDataAdapter Data = new SqlDataAdapter("SELECT Name as [Назва гастрольної програми], DATEDIFF(day, Start, Finish) as [Тривалість(у днях)], " +
                                                         "AvPrice as [Середня ціна], SUM(con.AmountTickets) as [К-сть проданих квитків], " +
                                                         "AvPrice*SUM(con.AmountTickets) as [Виручка] FROM Gastroli gas " +
                                                         "LEFT JOIN GasConCit con ON con.IDGastroli=gas.IDGastroli " +
                                                         "WHERE IDGRoup=" + id + " GROUP BY Name, Start, Finish, AvPrice;", sqlConn);
                    dT.Clear();
                    Data.Fill(dT);
                    dataGrid.ItemsSource = dT.DefaultView;
                    SqlDataAdapter Data1 = new SqlDataAdapter("SELECT cit.Name as [Місто] FROM Gastroli " +
                                                          "gas LEFT JOIN GasConCit gcc " +
                                                          "ON gas.IDGastroli=gcc.IDGastroli " +
                                                          "Left JOIN City cit " +
                                                          "ON cit.IDCity=gcc.IDCity " +
                                                          "Where gas.IDGroup='" + id + "';", sqlConn);
                    dT1.Clear();
                    Data1.Fill(dT1);
                    dataGrid1.ItemsSource = dT1.DefaultView;
                    SqlDataAdapter Data2 = new SqlDataAdapter("SELECT Son.Name as [Назва пісні], mem.MemberName as [Композитор], mem1.MemberName as [Автор слів пісні] FROM Songs " +
                                                        "Son Left JOIN Members mem ON Son.Composer=mem.IDMember " +
                                                        "Left JOIN Members mem1 ON Son.SongWriter=mem1.IDMember WHERE mem.IDGroup=" + id + ";", sqlConn);
                    dT2.Clear();
                    Data2.Fill(dT2);
                    dataGrid2.ItemsSource = dT2.DefaultView;
                }
                else
                    MessageBox.Show("Неправильно введена назва гурту або такого гурту немає у базі!");
            }
            else
                MessageBox.Show("Введіть назву групи!");
        }

        private void CityFilter_Click(object sender, RoutedEventArgs e)
        {
            if (City.Text != "")
            {
                SqlConnection sqlConn = new SqlConnection(Connection);
                sqlConn.Open();
                string strQ1;
                strQ1 = "SELECT IDCity FROM City WHERE Name='" + City.Text + "';";
                SqlCommand Com = new SqlCommand(strQ1, sqlConn);
                Com.ExecuteNonQuery();
                if (Com.ExecuteScalar() != null)
                {
                    int id = int.Parse(Com.ExecuteScalar().ToString());
                    SqlDataAdapter Data = new SqlDataAdapter("SELECT gr.Name as [Гурт], gas.Name as [Назва гастрольної програми] FROM Gastroli gas " +
                                                             "Left Join Groups gr ON gr.IDGroup=gas.IDGroup " +
                                                             "LEFT JOIN GasConCit con ON con.IDGastroli=gas.IDGastroli WHERE con.IDCity = " + id + "; ", sqlConn);
                    dT.Clear();
                    Data.Fill(dT);
                    dataGrid.ItemsSource = dT.DefaultView;
                }
                else
                    MessageBox.Show("Неправильно введена назва міста або такого міста немає у базі!");
            }
            else
                MessageBox.Show("Введіть назву міста!");
        }

        private void PriceFilter_Click(object sender, RoutedEventArgs e)
        {
            if (FirstPrice.Text != "" && LastPrice.Text != "")
            {
                SqlConnection sqlConn = new SqlConnection(Connection);
                sqlConn.Open();
                SqlDataAdapter Data = new SqlDataAdapter("SELECT Name as [Назва гастрольної програми], DATEDIFF(day, Start, Finish) as [Тривалість(у днях)], " +
                                                        "AvPrice as [Середня ціна], SUM(con.AmountTickets) as [К-сть проданих квитків], " +
                                                        "AvPrice*SUM(con.AmountTickets) as [Виручка] FROM Gastroli gas " +
                                                        "LEFT JOIN GasConCit con ON con.IDGastroli=gas.IDGastroli " +
                                                        "WHERE AvPrice>" + FirstPrice.Text + " AND AvPrice<" + LastPrice.Text + " GROUP BY Name, Start, Finish, AvPrice;", sqlConn);
                dT.Clear();
                Data.Fill(dT);
                dataGrid.ItemsSource = dT.DefaultView;
            }
            else
                MessageBox.Show("Введіть усі дані!");
        }

        private void TicketsNum_Click(object sender, RoutedEventArgs e)
        {
            if (FirstNum.Text != "" && LastNum.Text != "")
            {
                SqlConnection sqlConn = new SqlConnection(Connection);
                sqlConn.Open();
                SqlDataAdapter Data = new SqlDataAdapter("SELECT Name as [Назва гастрольної програми], DATEDIFF(day, Start, Finish) as [Тривалість(у днях)], " +
                                                        "AvPrice as [Середня ціна], SUM(con.AmountTickets) as [К-сть проданих квитків], " +
                                                        "AvPrice*SUM(con.AmountTickets) as [Виручка] FROM Gastroli gas " +
                                                        "LEFT JOIN GasConCit con ON con.IDGastroli=gas.IDGastroli " +
                                                        "GROUP BY Name, Start, Finish, AvPrice Having SUM(con.AmountTickets)>" + FirstNum.Text + " AND SUM(con.AmountTickets)<" + LastNum.Text + ";", sqlConn);
                dT.Clear();
                Data.Fill(dT);
                dataGrid.ItemsSource = dT.DefaultView;
            }
            else
                MessageBox.Show("Введіть усі дані!");
        }
    }
}
