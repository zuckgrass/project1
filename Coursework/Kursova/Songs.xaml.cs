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
    /// Interaction logic for Songs.xaml
    /// </summary>
    public partial class Songs : Window
    {
        string GroupID;
        public Songs()
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
            lmem.Content = "Репертуар гурту " + Com.ExecuteScalar().ToString();
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
                SqlDataAdapter Data = new SqlDataAdapter("SELECT Son.Name as [Назва пісні], mem.MemberName as [Композитор], mem1.MemberName as [Автор слів пісні], CONVERT(varchar, Release, 104) as [Дата релізу] FROM Songs " +
                                                        "Son Left JOIN Members mem ON Son.Composer=mem.IDMember " +
                                                        "Left JOIN Members mem1 ON Son.SongWriter=mem1.IDMember " +
                                                        "WHERE Son.IDGroup='" + Group + "'; ", sqlConn);
                dT.Clear();
                Data.Fill(dT);
                dataGrid.ItemsSource = dT.DefaultView;
            }
            sqlConn.Close();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (SongName.Text != "" && Composer.Text != "" && SongWriter.Text != "" && Realise.Text != "")
            {
                SqlConnection sqlConn = new SqlConnection(Connection);
                sqlConn.Open();
                if (sqlConn.State == System.Data.ConnectionState.Open)
                {
                    string strQ1;
                    strQ1 = "SELECT IDMember FROM Members WHERE MemberName='" + Composer.Text + "';";
                    SqlCommand Com = new SqlCommand(strQ1, sqlConn);
                    Com.ExecuteNonQuery();
                    int idcomposer = int.Parse(Com.ExecuteScalar().ToString());
                    string strQ2;
                    strQ2 = "SELECT IDMember FROM Members WHERE MemberName='" + SongWriter.Text + "';";
                    SqlCommand Com1 = new SqlCommand(strQ2, sqlConn);
                    Com1.ExecuteNonQuery();
                    int idsongwr = int.Parse(Com1.ExecuteScalar().ToString());
                    if (idsongwr != null && idcomposer != null)
                    {
                        SqlDataAdapter Data = new SqlDataAdapter("IF '"+ SongName.Text + "' NOT IN(Select Name FROM Songs)" +
                                                                "INSERT INTO Songs(IDSong, IDGroup, Name, Composer, SongWriter, Release) " +
                                                                "VALUES((SELECT Max(IDSong)+1 FROM Songs)," + GroupID + ", '" + SongName.Text + "' , " + idcomposer + ", " + idsongwr + ", '" + Realise.Text + "');", sqlConn);
                        Data.Fill(dT);
                        dataGrid.ItemsSource = dT.DefaultView;
                    }
                    else
                        MessageBox.Show("Виконавця не існує, спочатку додайте про нього інформацію!");
                }
                sqlConn.Close();
                InitDataTable(GroupID);
            }
            else
                MessageBox.Show("Введіть усі дані!");
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (SongName.Text != "")
            {
                SqlConnection sqlConn = new SqlConnection(Connection);
                sqlConn.Open();
                if (sqlConn.State == System.Data.ConnectionState.Open)
                {
                    SqlDataAdapter Data = new SqlDataAdapter("DELETE FROM Songs WHERE Name ='" + SongName.Text + "' AND IDGroup=" + GroupID + ";", sqlConn);
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
    }
}
