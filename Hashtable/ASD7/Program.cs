using static System.Console;
using System.Collections.Generic;
using System.Linq;

namespace Lab7
{
    public class HashTable
    {
        public double loadness { get; set; }
        public int size { get; set; }
        public Entry[] table { get; set; }
        public HashTable(Entry[] Table, double Loadness, int Size)
        {
            table = Table;
            loadness = Loadness;
            size = Size;
        }
    }
    public class Key
    {
        public string firstName;
        public string Surname;
        public Key(string FirstName, string surname)
        {
            firstName = FirstName;
            Surname = surname;
        }
    }
    public class Value
    {
        public string password;
        public string emailAdress;
        public LinkedList<Key> friends;
        public Value(string Password, string EmailAdress, LinkedList<Key> Friends)
        {
            password = Password;
            emailAdress = EmailAdress;
            friends = Friends;
        }
    }
    public class Entry
    {
        public Key key;
        public Value value;
        public Entry(Key Key, Value Value)
        {
            key = Key;
            value = Value;
        }
    }
    class Program
    {
        static double loadness = 0;
        static int size = 0;
        static HashTable hashtable;
        static Entry[] table = new Entry[10];
        static int N = 26;
        static void Main()
        {
            while (true)
            {
                WriteLine("Do you want to fill the hashtable by yourself?");
                string an = ReadLine();
                hashtable = new HashTable(table, loadness, size);
                if (an == "No" || an == "no")
                {
                    Key key1 = new Key("Sophia", "Pisotska");
                    LinkedList<Key> frie1 = new LinkedList<Key>();
                    Value value1 = new Value("1111", "sofiapisotska@gmail.com", frie1);
                    Key key2 = new Key("Mark", "Peterson");
                    LinkedList<Key> frie2 = new LinkedList<Key>();
                    Value value2 = new Value("mark1234", "markpeterson@gmail.com", frie2);
                    Key key3 = new Key("Olga", "Sulema");
                    LinkedList<Key> frie3 = new LinkedList<Key>();
                    Value value3 = new Value("os2022", "olgasulema@gmail.com", frie3);
                    Key key4 = new Key("James", "Bond");
                    LinkedList<Key> frie4 = new LinkedList<Key>();
                    Value value4 = new Value("jamesbond03", "jamesbond@gmail.com", frie4);
                    Key key5 = new Key("Kevin", "Markov");
                    LinkedList<Key> frie5 = new LinkedList<Key>();
                    Value value5 = new Value("kevinmak", "kevinmarkov@gmail.com", frie5);
                    Key key6 = new Key("Sasha", "Ivanov");
                    LinkedList<Key> frie6 = new LinkedList<Key>();
                    Value value6 = new Value("asasas", "sasha@gmail.com", frie6);
                    Key key7 = new Key("Dmytro", "Kuleba");
                    LinkedList<Key> frie7 = new LinkedList<Key>();
                    Value value7 = new Value("dymdym", "dymakul@gmail.com", frie7);
                    Key key8 = new Key("Lola", "Larris");
                    LinkedList<Key> frie8 = new LinkedList<Key>();
                    Value value8 = new Value("Lorris", "lorris@gmail.com", frie8);
                    Key key9 = new Key("Alexa", "Smith");
                    LinkedList<Key> frie9 = new LinkedList<Key>();
                    Value value9 = new Value("asmith0", "alexa@gmail.com", frie9);
                    Key key10 = new Key("David", "Cole");
                    LinkedList<Key> frie10 = new LinkedList<Key>();
                    Value value10 = new Value("davidcol22", "david@gmail.com", frie10);
                    insertEntry(key1, value1);
                    insertEntry(key2, value2);
                    insertEntry(key3, value3);
                    insertEntry(key4, value4);
                    insertEntry(key5, value5);
                    insertEntry(key6, value6);
                    insertEntry(key7, value7);
                    insertEntry(key8, value8);
                    insertEntry(key9, value9);
                    insertEntry(key10, value10);
                    addFriend("Olga", "Sulema", key1);
                    addFriend("Olga", "Sulema", key2);
                    addFriend("Sophia", "Pisotska", key2);
                    addFriend("Sophia", "Pisotska", key3);
                    addFriend("Sophia", "Pisotska", key4);
                    addFriend("Sophia", "Pisotska", key5);
                    addFriend("Kevin", "Markov", key4);
                    addFriend("Kevin", "Markov", key3);
                    break;
                }
                else if (an == "Yes" || an == "yes")
                    break;
                else
                    WriteLine("Please! Give the answer!");
            }
            int slot = 0;
            string username, usersurname;
            while (true)
            {
                WriteLine("Welcome to our social net! Do you want to authorize,  to register, to see hashtable, to remove user or to find user?");
                string answer = ReadLine();
                while (true)
                {
                    if (answer == "authorize")
                    {
                        while (true)
                        {
                            string[] Line;
                            while (true)
                            {
                                Write("Input your name and surname: ");
                                Line = ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                                if (Line.Length == 2)
                                    break;
                            }
                            Key key = new Key(Line[0], Line[1]);
                            slot = findEntry(key);
                            if (slot == -1)
                                Write("There's no user with this name and surname. ");
                            else
                            {
                                username = Line[0];
                                usersurname = Line[1];
                                while(true)
                                {
                                    Write("Input password: ");
                                    string pass = ReadLine();
                                    if (HashPassword(pass) == hashtable.table[slot].value.password)
                                    {
                                        WriteLine("Welcome!");
                                        break;
                                    }
                                    else
                                        Write("Incorrect password: ");
                                }
                                break;
                            }
                        }
                        userMode(username, usersurname, slot);
                        break;
                    }
                    else if (answer == "register")
                    {
                        LinkedList<Key> friends = new LinkedList<Key>();
                        while (true)
                        {
                            Write("Input name: ");
                            string name = ReadLine();
                            Write("Input surname: ");
                            string surname = ReadLine();
                            Write("Input password: ");
                            string password = ReadLine();
                            Write("Input email: ");
                            string emailAdress = ReadLine();
                            Key Key = new Key(name, surname);
                            Value value = new Value(password, emailAdress, friends);
                            if (name != "" && surname != "" && password != "" && emailAdress != "")
                            {
                                insertEntry(Key, value);
                                username = name;
                                usersurname = surname;
                                break;
                            }
                            else
                                WriteLine("Don't leave free spaces");
                        }
                        Key key = new Key(username, usersurname);
                        int sl = findEntry(key);
                        userMode(username, usersurname, sl);
                        break;
                    }
                    else if(answer== "see hashtable")
                    {
                        for(int i=0; i<hashtable.table.Length; i++)
                        {
                            if (hashtable.table[i] == null)
                            {
                                WriteLine("[" + i + "]" + "-");
                            }
                            else
                            {
                                WriteLine("[" + i + "]" + hashtable.table[i].key.firstName + " " + hashtable.table[i].key.Surname + ", " + hashtable.table[i].value.password + ", " + hashtable.table[i].value.emailAdress + ";");
                                if (hashtable.table[i].value.friends != null)
                                {
                                    foreach (Key item in hashtable.table[i].value.friends)
                                        Write(item.firstName + " " + item.Surname + "; ");
                                    WriteLine();
                                }
                                else
                                    WriteLine();
                            }
                        }
                        break;
                    }
                    else if (answer == "remove user")
                    {
                        string[] Line;
                        while (true)
                        {
                            Write("Input name and surname: ");
                            Line = ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                            if (Line.Length == 2)
                                break;
                            else
                                Write("Incorrect!");
                        }
                        Key key = new Key(Line[0], Line[1]);
                        removeEntry(key);
                        break;
                    }
                    else if (answer == "find user")
                    {
                        string[] Line;
                        while (true)
                        {
                            Write("Input name and surname: ");
                            Line = ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                            if (Line.Length == 2)
                                break;
                            else
                                Write("Incorrect!");
                        }
                        Key key = new Key(Line[0], Line[1]);
                        slot = findEntry(key);
                        if (slot != -1)
                        {
                            WriteLine("Name and surname of the user: " + hashtable.table[slot].key.firstName + " " + hashtable.table[slot].key.Surname);
                            WriteLine("EmailAdress of the user: " + hashtable.table[slot].value.emailAdress);
                            Write("Friends list of the user: ");
                            foreach (Key item in hashtable.table[slot].value.friends)
                                Write(item.firstName + " " + item.Surname + "; ");
                            WriteLine();
                        }
                        else
                        {
                            WriteLine("There is no user with this name and surname");
                        }
                        break;
                    }
                    else
                    {
                        Write("Input authorize or register: ");
                        answer = ReadLine();
                    }
                }
            }
        }
        public static void insertEntry(Key key, Value value)
        {
            int slot = getHash(key, hashtable);
            while (slot==-1)
            {
                WriteLine("Table is full. Rehashing starts!");
                rehashing();
                slot = getHash(key, hashtable);
            }
            value.password=HashPassword(value.password);
            Entry entry = new Entry(key, value);
            hashtable.table[slot] = entry;
            hashtable.size++;
            hashtable.loadness = Convert.ToDouble(hashtable.size / hashtable.table.Length);
            if (hashtable.loadness == 1)
            {
                WriteLine("Table is full. Rehashing starts!");
                rehashing();
            }
        }
        public static int getHash(Key key, HashTable Hashtable)
        {
            int k = HashCode(key);
            int hashzond;
            hashzond = k % Hashtable.table.Length;
            Entry entry;
            while (true)
            {
                if (hashzond == Hashtable.table.Length)
                {
                    hashzond = -1;
                    break;
                }
                entry = Hashtable.table[hashzond];
                if (entry == null || entry.key.firstName == "DELETED")
                {
                    entry = Hashtable.table[hashzond];
                    break;
                }
                else
                    hashzond++;
            }
            return hashzond;
        }
        public static int HashCode(Key key)
        {
            int sum = 0;
            double n = key.firstName.Length - 1;
            for (int i = 0; i < key.firstName.Length; i++)
            {
                sum += (int)((int)key.firstName[i] * Math.Pow(N, n));
                n--;
            }
            n = key.Surname.Length - 1;
            for (int i = 0; i < key.Surname.Length; i++)
            {
                sum += (int)((int)key.Surname[i] * Math.Pow(N, n));
                n--;
            }
            sum = Math.Abs(sum);
            return sum;
        }
        public static void removeEntry(Key key)
        {
            int slot = findEntry(key);
            if (slot == -1)
                WriteLine("There is no user with this name and surname");
            else
            {
                for (int i = 0; i < hashtable.table.Length; i++)
                {
                    if(hashtable.table[i]!=null && hashtable.table[i].key.firstName!="DELETED")
                        removeFriend(key.firstName, key.Surname, hashtable.table[i].key);
                }
                Entry entry = hashtable.table[slot];
                entry.key.firstName = "DELETED";
                entry.key.Surname = "DELETED";
                entry.value.friends=null;
                entry.value.password = "DELETED";
                entry.value.emailAdress = "DELETED";
                hashtable.table[slot] = entry;
                hashtable.size--;
                hashtable.loadness = Convert.ToDouble(hashtable.size / hashtable.table.Length);
            }
        }
        public static int findEntry(Key key)
        {
            int slot = -1;
            Entry entry;
            for(int i = 0; i < hashtable.table.Length; i++)
            {
                entry = hashtable.table[i];
                if(entry !=null && key.firstName == entry.key.firstName && key.Surname == entry.key.Surname)
                {
                    slot = i;
                    break;
                }
            }
            return slot;
        }
        public static void rehashing()
        {
            Entry[] Table = new Entry[hashtable.table.Length * 2];
            int Size = hashtable.size;
            double Loadness = Convert.ToDouble(Size / Table.Length);
            HashTable Hashtable = new HashTable(Table, Loadness, Size);
            Entry el;
            for (int i = 0; i < hashtable.table.Length; i++)
            {
                el = hashtable.table[i];
                if(el != null && el.key.firstName!="DELETED")
                {
                    Key keyy = el.key;
                    int slot = getHash(keyy,Hashtable);
                    Hashtable.table[slot] = el;
                }
            }
            hashtable = Hashtable;
        }
        public static void addFriend(string firstName, string surname, Key keyy)
        {
            Key key = new Key(firstName, surname);
            int slot = findEntry(key);
            bool isFriend = false;
            if (slot != -1)
            {
                foreach (Key item in hashtable.table[findEntry(keyy)].value.friends)
                {
                    if (item.firstName == key.firstName && item.Surname==key.Surname)
                    {
                        isFriend = true;
                        break;
                    }
                }
                if (isFriend == false)
                    hashtable.table[findEntry(keyy)].value.friends.AddLast(key);
                else
                    WriteLine("You have this user in your friends list");
            }
            else
                WriteLine("There is no user with this name and surname");
        }
        public static void removeFriend(string firstName, string surname, Key keyy)
        {
            Key key = new Key(firstName, surname);
            int slot = findEntry(key);
            if (slot != -1)
            {
                foreach (Key item in hashtable.table[findEntry(keyy)].value.friends)
                {
                    if (item.firstName == key.firstName && item.Surname == key.Surname)
                    {
                        hashtable.table[findEntry(keyy)].value.friends.Remove(item);
                        break;
                    }
                }
            }
            else
                WriteLine("There is no user with this name and surname");
        }
        public static string HashPassword(string password)
        {
            int sum = 0;
            int n=password.Length-1;
            for (int i = 0; i <= n; i++)
            {
                sum += (int)((int)password[i] * Math.Pow(N, n));
                n--;
            }
            sum = Math.Abs(sum);
            return sum.ToString();
        }
        public static void userMode(string username, string usersurname, int slot)
        {
            while (true)
            {
                WriteLine("Input command FriendsList, AddFriend, DeleteFriend, DeleteThisUser, Exit: ");
                string command = ReadLine();
                if (command == "FriendsList")
                {
                    LinkedList<Key> list = hashtable.table[slot].value.friends;
                    foreach (Key item in list)
                        Write(item.firstName + " " + item.Surname + "; ");
                    WriteLine();
                }
                else if (command == "AddFriend")
                {
                    while (true)
                    {
                        Write("Input name of your friend: ");
                        string name = ReadLine();
                        Write("Input surname of your friend: ");
                        string surname = ReadLine();
                        Key friend = new Key(name, surname);
                        int index = findEntry(friend);
                        if (index != -1)
                        {
                            Key key = new Key(username, usersurname);
                            addFriend(name, surname, key);
                            break;
                        }
                        else
                            Write("There is no user, try more. ");
                    }
                }
                else if (command == "DeleteFriend")
                {
                    while (true)
                    {
                        Write("Input name of your friend: ");
                        string name = ReadLine();
                        Write("Input surname of your friend: ");
                        string surname = ReadLine();
                        Key friend = new Key(name, surname);
                        int index = findEntry(friend);
                        if (index != -1)
                        {
                            removeFriend(name, surname, hashtable.table[slot].key);
                            break;
                        }
                        else
                            WriteLine("There is no user, try more. ");
                    }
                }
                else if (command == "DeleteThisUser")
                {
                    Key key = new Key(username, usersurname);
                    removeEntry(key);
                    break;
                }
                else if (command == "Exit")
                    break;
                else
                    WriteLine("Try another command: ");
            }
        }
    }
}