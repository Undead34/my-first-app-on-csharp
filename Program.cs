using System;
using System.Data.SQLite;
using System.IO;


namespace PassManager
{
  class Program
  {
    static int menuSelect = 0;
    static string[] menuOptions = { "Login", "Register", "Manage Accounts", "Exit" };

    static void Main(string[] args)
    {
      ShowMenu();
      MainMenu();
      Console.CursorVisible = true;
    }
    static void ShowMenu()
    {
      Console.Clear();
      Console.CursorVisible = false;
      Console.WriteLine("!Welcome to KryptoPass¡");
      Console.Write("Use the keyboard to select an option.\n");

      for (int i = 0; i < menuOptions.Length; i++)
      {
        if (menuSelect == i)
        {
          Console.BackgroundColor = ConsoleColor.White;
          Console.WriteLine("[*] {0}", menuOptions[i]);
          Console.BackgroundColor = ConsoleColor.Black;
        }
        else
        {
          Console.WriteLine("[{1}] {0}", menuOptions[i], (i + 1));
        }
      }
    }


    static void MainMenu()
    {
      while (true)
      {
        ShowMenu();
        ConsoleKeyInfo keyPressed = Console.ReadKey();
        if (keyPressed.Key == ConsoleKey.DownArrow && menuSelect != menuOptions.Length - 1) menuSelect++;
        else if (keyPressed.Key == ConsoleKey.UpArrow && menuSelect >= 1) menuSelect--;
        else if (keyPressed.Key == ConsoleKey.Enter)
        {
          MainOptions();
          break;
        };
      }
    }

    static void MainOptions()
    {
      Console.CursorVisible = true;
      switch (menuSelect)
      {
        case 0:
          LoginUser();
          break;
        case 1:
          RegisterUser();
          break;
        case 2:
          DashboardAccounts();
          break;
        case 3:
          System.Console.WriteLine("Leaving the app...");
          Environment.Exit(0);
          break;
      }
    }

    static void LoginUser()
    {
      Console.WriteLine("connecting to database...");
      SQLiteConnection sqlite_conn = CreateConnection();
      Console.WriteLine("successful");
      
      if(ExistsTable(sqlite_conn, "USERS")) {
        System.Console.WriteLine("Yess");
      } else {
        Console.Clear();
        System.Console.WriteLine("");
      }

      // ReadData(sqlite_conn);
    }
    static void RegisterUser() { }

    static void DashboardAccounts() { }

    static SQLiteConnection CreateConnection()
    {
      string dirPath = "C:\\Users\\Undead34\\KryptoPass";
      string dbPath = Path.Join(dirPath, "\\database.db");
      if (!Directory.Exists(dirPath))
      {
        Directory.CreateDirectory(dirPath);
      }

      SQLiteConnection sqlite_conn;
      // Create a new database connection:
      sqlite_conn = new SQLiteConnection($"DataSource={dbPath}; Version=3; New=True; Compress=True;");
      try
      {
        sqlite_conn.Open();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex);
      }
      return sqlite_conn;
    }

    static void CreateTable(SQLiteConnection conn)
    {

      SQLiteCommand sqlite_cmd;
      string Createsql = "CREATE TABLE USERS (username VARCHAR(200), id INT)";
      string Createsql1 = "CREATE TABLE DATA_USERS (password VARCHAR(200), note VARCHAR(200))";
      sqlite_cmd = conn.CreateCommand();
      sqlite_cmd.CommandText = Createsql;
      sqlite_cmd.ExecuteNonQuery();
      sqlite_cmd.CommandText = Createsql1;
      sqlite_cmd.ExecuteNonQuery();
    }

    static void InsertData(SQLiteConnection conn)
    {
      SQLiteCommand sqlite_cmd;
      sqlite_cmd = conn.CreateCommand();
      sqlite_cmd.CommandText = "INSERT INTO USERS (username, id) VALUES('Test Text ', 1); ";
      sqlite_cmd.ExecuteNonQuery();
      sqlite_cmd.CommandText = "INSERT INTO USERS (username, id) VALUES('Test1 Text1 ', 2); ";
      sqlite_cmd.ExecuteNonQuery();
      sqlite_cmd.CommandText = "INSERT INTO USERS (username, id) VALUES('Test2 Text2 ', 3); ";
      sqlite_cmd.ExecuteNonQuery();

      sqlite_cmd.CommandText = "INSERT INTO DATA_USERS (password, note) VALUES('Test3 Text3 ', 'Hola');";
      sqlite_cmd.ExecuteNonQuery();

    }

    static void ReadData(SQLiteConnection conn)
    {
      SQLiteDataReader sqlite_datareader;
      SQLiteCommand sqlite_cmd;
      sqlite_cmd = conn.CreateCommand();
      sqlite_cmd.CommandText = "SELECT * FROM SampleTable";

      sqlite_datareader = sqlite_cmd.ExecuteReader();
      while (sqlite_datareader.Read())
      {
        string myreader = sqlite_datareader.GetString(0);
        Console.WriteLine(myreader);
      }
      conn.Close();
    }

    public static bool ExistsTable(SQLiteConnection conn, string table)
    {
      string querySQL = "SELECT name FROM sqlite_master WHERE type='table' AND name='" + table + "';";
      SQLiteDataReader sqlite_datareader;
      SQLiteCommand sqlite_cmd = conn.CreateCommand();
      sqlite_cmd.CommandText = querySQL;
      sqlite_datareader = sqlite_cmd.ExecuteReader();
      if (sqlite_datareader.HasRows)
      {
        return true;
      }

      return false;
      // if (conn.State == System.Data.ConnectionState.Open) {}
    }
  }
}
