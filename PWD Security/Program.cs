using System.IO;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

class Program
{
    public static bool debug = false;
    static public string userdata_path = null;
    static public string[] user_data;
    static public bool user_data_available = false;
    static int number2;
    static string assembly_pwd = "";
    static int result;
    static public int free_line = -1;
    static bool user_exists = false;
    static int user_id;
    static void Main(string[] args)
    {
        string current_path;
        var rnd = new Random();
        if (debug)
        {
            current_path = "E:\\C#testing\\pwd_security\\";  //DEBUG
        }
        else
        {
            current_path = Directory.GetCurrentDirectory(); //Release
        }
        try
        {
            userdata_path = Path.Combine(current_path, "data\\user_data.bin");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Couldn't acces user data");
            Console.ReadKey();
        }
        try
        {
            if (userdata_path != null)
            {
                user_data = File.ReadAllLines(userdata_path);
                user_data_available = true;
            }
            
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("error");
            Console.ReadKey();
        }
        int character;
        int key_action;
        for (key_action = 0; key_action == 0;)
        {
            Console.Clear();
            Console.ResetColor();
            Console.WriteLine("[1] Sign up");
            Console.WriteLine("[2] Log in");
            char readkey = Console.ReadKey().KeyChar;
            int number;
            if (int.TryParse(readkey.ToString(), out number))
            {
                if ((number < 3) && (number > 0))
                {
                    key_action = number;
                }
            }
        }
        switch(key_action)
        {
            case 1:
                int sucesscode = 0;
                while (sucesscode == 0)
                {
                    Console.Clear();
                    Console.Write("Enter username: ");
                    string created_username = Console.ReadLine().ToLower();
                    Console.WriteLine();
                    Console.Write("Enter password: ");
                    string created_password = Console.ReadLine();
                    Console.WriteLine();
                    Console.Write("Confirm password: ");
                    string created_password_confirm = Console.ReadLine();
                    Console.Clear();
                    bool created_username_available = true;
                    for (int i = 0; i <= 999; i++)
                    {
                        if (user_data[i] == created_username)
                        {
                            created_username_available = false;
                            Console.WriteLine("Username already chosen");
                            Console.ReadKey();
                            break;
                        }
                    }
                    if (created_username_available == true)
                    {
                        if (created_password == created_password_confirm)
                        {

                            for (int i = 0; i <= 999; i++)
                            {
                                if (user_data[i] == "")
                                {
                                    free_line = i;
                                    i = 1000;
                                }
                                if (free_line >= 0)
                                {
                                    try
                                    {
                                        user_data[free_line] = created_username; //Name
                                        string salt = GenerateRandomString(4);
                                        user_data[2000 + free_line] = salt; //Salt
                                        user_data[1000 + free_line] = CreateHash(created_password, userdata_path, salt); //Hash PWD
                                        sucesscode++;
                                        Console.Clear();
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.WriteLine("Success!");
                                        Console.ResetColor();
                                        Console.WriteLine("Restart program to sign in");
                                        Console.ReadKey();

                                    }
                                    catch (Exception ex)
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Critical error: {0}", ex);
                                        Console.WriteLine("Could not save data");
                                        Console.ResetColor();
                                        Console.ReadKey();
                                    }
                                    if (sucesscode > 0)
                                    {
                                        File.WriteAllLines(userdata_path, user_data);
                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Could not save data due do internal error");
                                        Console.ResetColor();
                                        Console.ReadKey();
                                    }

                                }
                            }
                        }
                    }
                }
                
                break;
            case 2:
                Console.Clear();
                Console.Write("Enter your username: ");
                string trueinput_username = Console.ReadLine();
                user_data = File.ReadAllLines(userdata_path);
                string input_username = trueinput_username.ToLower();
                for(int i = 0; i < 999; i++)
                {
                    if(input_username == user_data[i])
                    {
                        user_exists = true;
                        user_id = i;
                    }
                }
                if (user_exists)
                {
                    Console.Clear();
                    Console.WriteLine("Welcome back!");
                    Console.WriteLine();
                    Console.ResetColor();
                    Console.Write("Please enter your password: ");
                    string input_password = Console.ReadLine();
                    Console.WriteLine();
                    string user_salt = user_data[user_id + 2000];
                    string password_probe = CreateHash(input_password, userdata_path, user_salt);
                    string password_saved = user_data[user_id + 1000];
                    if (debug)
                    {
                        Console.WriteLine("User_Salt: " + user_salt);
                        Console.WriteLine("User saved_pwd: " + password_saved);
                        Console.WriteLine("User input_pwd: " + password_probe);
                        if(password_probe == password_saved)
                        {
                            Console.WriteLine("The passwords match");
                        }
                        Console.ReadKey();
                    }
                    if (password_probe == password_saved)
                    {
                        Console.Clear();
                        Console.ForegroundColor= ConsoleColor.Green;
                        Console.WriteLine("Sucess!");
                        Console.ResetColor();
                        Console.Write("Welcome back, ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(trueinput_username);
                        Console.ResetColor();
                        Console.ReadKey();

                    }
                    else if(password_probe != password_saved)
                    {
                        Console.Clear();
                        Console.ForegroundColor= ConsoleColor.Red;
                        Console.WriteLine("Password is not correct");
                        Console.ResetColor();
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Internal error");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("User does not exist");
                    Console.ResetColor();
                    Console.ReadKey();
                }
                
                break;
            default:
                Console.Clear ();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Critical error");
                Console.ResetColor ();
                Thread.Sleep(5000);
                break;
        }

    }
    static string GenerateRandomString(int length)
    {
        var rnd = new Random();
        string characters = "abcdefgjijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        string return_string = "";
        for(int i = 0; i < length; i++)
        {
            char randomchar = characters[rnd.Next(0, characters.Length)];
            return_string += randomchar;
        }
        return return_string;
    }

    static string CreateHash(string pwd, string path, string salt)
    {
        string[] userdata = File.ReadAllLines(path);
        Thread.Sleep(1000);
        Console.Clear();
        if (debug)
        {
            Console.WriteLine("Password: " + pwd);
            Console.WriteLine("Salt: " + salt);
            Console.WriteLine("Combinde: " + salt + pwd);
        }
        pwd = salt + pwd;
        byte[] bytes = Encoding.UTF8.GetBytes(pwd);
        Thread.Sleep(1000);
        string hexString = BitConverter.ToString(bytes).Replace("-", string.Empty);
        if (debug)
        {
            Console.WriteLine("Combined HEX: " + hexString);
        }
        if (hexString.Length > 8)
        {
            Thread.Sleep(1000);
            hexString = hexString.Substring(0, 8);
        }
        ulong hexValue = Convert.ToUInt64(hexString, 16);
        Thread.Sleep(1000);
        string hexValueString = Convert.ToString(hexValue);
        int max_length = 10;
        if (hexValueString.Length > 8)
        {
            max_length = 8;
        }
        else
        {
            max_length = hexValueString.Length;
        }
        for (int i = 0; i < max_length; i++)
        {
            Thread.Sleep(30);
            int number = Convert.ToInt32(hexValueString[i]);

            try
            {
                number2 = Convert.ToInt32(hexValueString[i + 1]);
            }
            catch (Exception)
            {

            }
            result = number * number2;
            assembly_pwd = assembly_pwd + Convert.ToString(result);
        }
        if(debug)
        {
            Console.WriteLine("Result: " + assembly_pwd);
            Console.ReadKey();
        }
        Thread.Sleep(1000);
        return assembly_pwd;
    }
}
