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
using System.IO;
using System.Data;
using System.Data.SqlClient;

namespace Kursova
{
    /// <summary>
    /// Interaction logic for Gastroli.xaml
    /// </summary>
    public partial class Gastroli : Window
    {
        string GroupID;
        int idgastr = 0;
        public Gastroli()
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
            lmem.Content = "Гастролі гурту " + Com.ExecuteScalar().ToString();
            group.Dispose();
            group.Close();
            GroupID = Group;
            image.Source = BitmapFrame.Create(new Uri(@"C:\Users\38099\Desktop\Роботи\Основи програмування\Курсач\Картинки гурту\" + Group + ".jpg"));
            InitDataTable(Group);
        }
        string Connection = @"Data Source=DESKTOP-EN4LANA; initial Catalog=List of bands; Integrated Security=True";
        DataTable dT = new DataTable("Гастролі групи");
        DataTable dT1 = new DataTable("Міста гастролів групи");
        public void InitDataTable(string Group)
        {
            SqlConnection sqlConn = new SqlConnection(Connection);
            sqlConn.Open();
            if (sqlConn.State == System.Data.ConnectionState.Open)
            {
                SqlDataAdapter Data = new SqlDataAdapter("SELECT Name as [Назва гастрольної програми], CONVERT(varchar, Start, 104) as [Початок гастролів], CONVERT(varchar, Finish, 104) as [Кінець гастролів], AvPrice as [Середня ціна квитка] FROM Gastroli WHERE IDGroup='" + Group + "';", sqlConn);
                dT.Clear();
                Data.Fill(dT);
                dataGrid.ItemsSource = dT.DefaultView;
                SqlDataAdapter Data1 = new SqlDataAdapter("SELECT cit.Name as [Місто] FROM Gastroli " +
                                                          "gas LEFT JOIN GasConCit gcc " +
                                                          "ON gas.IDGastroli=gcc.IDGastroli " +
                                                          "Left JOIN City cit " +
                                                          "ON cit.IDCity=gcc.IDCity " +
                                                          "Where gas.IDGroup='" + Group + "'; ", sqlConn);
                dT1.Clear();
                Data1.Fill(dT1);
                Cities.ItemsSource = dT1.DefaultView;
            }
            sqlConn.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            Hide();
            mw.Show();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (GastroliName.Text != "" && Start.Text != "" && Finish.Text != "" && AvPrice.Text != "")
            {
                SqlConnection sqlConn = new SqlConnection(Connection);
                sqlConn.Open();
                if (sqlConn.State == System.Data.ConnectionState.Open)
                {
                    SqlDataAdapter Data = new SqlDataAdapter("INSERT INTO Gastroli (IDGastroli, IDGroup, Name, Start, Finish, AvPrice) " +
                                                            "VALUES((SELECT Max(IDGastroli) FROM Gastroli)+1," + GroupID + ", '" + GastroliName.Text + "', '" + Start.Text + "', '" + Finish.Text + "', " + AvPrice.Text + ");", sqlConn);
                    string strQ1;
                    strQ1 = "SELECT Max(IDGastroli) FROM Gastroli;";
                    SqlCommand Com = new SqlCommand(strQ1, sqlConn);
                    Com.ExecuteNonQuery();
                    idgastr = int.Parse(Com.ExecuteScalar().ToString());
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

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (GastroliName.Text != "")
            {
                SqlConnection sqlConn = new SqlConnection(Connection);
                sqlConn.Open();
                if (sqlConn.State == System.Data.ConnectionState.Open)
                {
                    SqlDataAdapter Data = new SqlDataAdapter("DELETE FROM Gastroli WHERE Name ='" + GastroliName.Text + "' AND IDGroup=" + GroupID + ";", sqlConn);
                    dT.Clear();
                    Data.Fill(dT);
                    dataGrid.ItemsSource = dT.DefaultView;
                }
                sqlConn.Close();
                InitDataTable(GroupID);
            }
            else
                MessageBox.Show("Введіть назву гастрольної програми!");
        }

        private void AddCity_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection sqlConn1 = new SqlConnection(Connection);
            sqlConn1.Open();
            if (idgastr == 0)
            {
                string strQ1;
                strQ1 = "SELECT IDGastroli FROM Gastroli WHERE IDGroup=" + GroupID + ";";
                SqlCommand Com = new SqlCommand(strQ1, sqlConn1);
                Com.ExecuteNonQuery();
                idgastr = int.Parse(Com.ExecuteScalar().ToString());
            }
            if (GastroliName.Text != "" && City.Text != "")
            {
                SqlConnection sqlConn = new SqlConnection(Connection);
                sqlConn.Open();
                if (sqlConn.State == System.Data.ConnectionState.Open)
                {
                    string strQ = "SELECT IDCity FROM City Where City.Name='" + City.Text + "';";
                    SqlCommand Com1 = new SqlCommand(strQ, sqlConn);
                    Com1.ExecuteNonQuery();
                    if (Com1.ExecuteScalar() == null)
                    {
                        string strQ2 = "INSERT INTO City(IDCity, Name) VALUES((SELECT Max(IDCity) FROM City)+1, '" + City.Text + "');";
                        SqlCommand Com2 = new SqlCommand(strQ2, sqlConn);
                        Com2.ExecuteNonQuery();
                    }
                    string strQ1;
                    strQ1 = "SELECT IDCity FROM City Where City.Name='" + City.Text + "' AND IDCity not in(SELECT IDCity FROM GasConCit WHERE IDGastroli = '" + idgastr + "');";
                    SqlCommand Com = new SqlCommand(strQ1, sqlConn);
                    Com.ExecuteNonQuery();
                    if (Com.ExecuteScalar() != null)
                    {
                        string citynum = Com.ExecuteScalar().ToString();
                        SqlDataAdapter Data = new SqlDataAdapter("INSERT INTO GasConCit(IDGastroli, IDConcert, IDCity) " +
                                                                "VALUES(" + idgastr + " , (SELECT Max(IDConcert) FROM GasConCit) + 1," +
                                                                citynum + "); ", sqlConn);
                        //dT.Clear();
                        Data.Fill(dT1);
                        Cities.ItemsSource = dT1.DefaultView;
                    }
                }
                sqlConn.Close();
                InitDataTable(GroupID);
            }
            else
                MessageBox.Show("Введіть усі дані!");
        }

        private void DeleteCity_Click(object sender, RoutedEventArgs e)
        {
            if (GastroliName.Text != "" && City.Text != "")
            {
                SqlConnection sqlConn = new SqlConnection(Connection);
                sqlConn.Open();
                if (sqlConn.State == System.Data.ConnectionState.Open)
                {
                    if (idgastr == 0)
                    {
                        string strQ1;
                        strQ1 = "SELECT IDGastroli FROM Gastroli WHERE IDGroup=" + GroupID + ";";
                        SqlCommand Com = new SqlCommand(strQ1, sqlConn);
                        Com.ExecuteNonQuery();
                        idgastr = int.Parse(Com.ExecuteScalar().ToString());
                    }
                    string strQ = "SELECT IDCity FROM City Where City.Name='" + City.Text + "';";
                    SqlCommand Com1 = new SqlCommand(strQ, sqlConn);
                    Com1.ExecuteNonQuery();
                    if (Com1.ExecuteNonQuery() != null)
                    {
                        string idcit = Com1.ExecuteScalar().ToString();
                        SqlDataAdapter Data = new SqlDataAdapter("DELETE FROM GasConCit WHERE IDGastroli=" + idgastr + " AND IDCity=" + idcit + ";", sqlConn);
                        dT.Clear();
                        Data.Fill(dT);
                        dataGrid.ItemsSource = dT.DefaultView;
                    }
                }
                sqlConn.Close();
                InitDataTable(GroupID);
            }
            else
                MessageBox.Show("Введіть назву гастрольної програми!");
        }
    }
}
