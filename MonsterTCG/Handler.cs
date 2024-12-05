using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
namespace MonsterTCG
{
    internal class Handler
    {
        public static string HandleRequest(string request, string body) 
        {
            if(request.StartsWith("POST /users")) { return UserRequest(body); }
            else if (request.StartsWith("POST /sessions")) { return SessionRequest(body); }
            else { return "HTTP/1.1 404 - Handler not found"; }
        }



        public static string UserRequest(string body)
        {
           // Console.WriteLine("we are in User Requests"); //DEBUG
            JsonDocument jsonDocument = JsonDocument.Parse(body);
            JsonElement json = jsonDocument.RootElement;
            string UserName = json.GetProperty("Username").GetString();
            string Password = json.GetProperty("Password").GetString();
            User userSet = Database.Temp_Database.FirstOrDefault(u => u.UserName == UserName);
            if (userSet == null) 
            {
                Database.Temp_Database.Add(new User(UserName, Password));
                return "HTTP/1.1 201 OK";
            }
            else 
            {
                return "HTTP/1.1 400 - User already exists";
            }
        }

        public static string SessionRequest(string body) 
        {
            //Console.WriteLine("we are in User Requests"); //DEBUG
            JsonDocument jsonDocument = JsonDocument.Parse(body);
            JsonElement json = jsonDocument.RootElement;
            string UserName = json.GetProperty("Username").GetString();
            string Password = json.GetProperty("Password").GetString();
            User userSet = Database.Temp_Database.FirstOrDefault(u => u.UserName == UserName);
            if (userSet != null) 
            { //den bruder gibts
                if (userSet.Password == Password)
                {//der bruder hat die richtigen daten eingegeben
                    //Console.WriteLine("IN SESSION REQUEST. RICHTIGE DATEN EINGEGEBEN."); //DEBUG
                    string token = userSet.GenerateToken();
                    //Console.WriteLine("TOKEN output:"); //DEBUG
                    string x = $"HTTP/1.1 200 -{token}";
                   // Console.WriteLine(x); //DEBUG
                    return x;
                }
                //er existiert aber falsches PW
                return "HTTP/1.1 405 - Login failed from existing but wrong pw";
            }
            else 
            {
                //diese Person existiert nicht
                return "HTTP/1.1 405 - Login failed from not exiting user";
            }
        }

    }
}
