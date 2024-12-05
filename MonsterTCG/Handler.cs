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
            else { return "something wrong"; }
        }



        public static string UserRequest(string body)
        {
            Console.WriteLine("we are in User Requests");
            JsonDocument jsonDocument = JsonDocument.Parse(body);
            JsonElement json = jsonDocument.RootElement;
            string UserName = json.GetProperty("Username").GetString();
            string Password = json.GetProperty("Password").GetString();
            User userSet = Database.Temp_Database.FirstOrDefault(u => u.UserName == UserName);
            if (userSet == null) 
            {
                Database.Temp_Database.Add(new User(UserName, Password));
                return "HTTP/1.1 200 OK";
            }
            else 
            {
                return "HTTP/1.1 400 - User already exists";
            }
        }

    }
}
