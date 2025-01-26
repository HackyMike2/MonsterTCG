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
        private static DBConnector db = new DBConnector();
        private const string USER_REQUEST_TEXT = "POST /users";
        private const string SESSION_REQUEST_TEXT = "POST /sessions";
        private const string CREATE_PACKAGE_TEXT = "POST /createpackage";
        private const string SHOW_PACKAGES = "POST /showpackages";
        private const string BUY_PACKAGE = "POST /package";
        private const string SCORE = "POST /score";

        public static string HandleRequest(string request, string body)
        {
            if (request.StartsWith(USER_REQUEST_TEXT))
            { 
                return UserRequest(body);
            }
            else if (request.StartsWith(SESSION_REQUEST_TEXT))
            { 
                return SessionRequest(body);
            }
            else if (request.StartsWith(CREATE_PACKAGE_TEXT)) 
            {
                return CreatePackageRequest(body);
            }
            else if (request.StartsWith(SHOW_PACKAGES)) 
            {
                return ShowPackageRequest(body);
            }
            else if (request.StartsWith(BUY_PACKAGE)) 
            {
                return BuyPackage(body);
            }
            else if (request.StartsWith(SCORE)) 
            {
                return GetScore(body);
            }
            else 
            {
                return "HTTP/1.1 404 - Handler not found";
            }
        }

        public static string UserRequest(string body)
        {
            try
            {
                // Console.WriteLine("we are in User Requests"); //DEBUG
                JsonDocument jsonDocument = JsonDocument.Parse(body);
                JsonElement json = jsonDocument.RootElement;
                string UserName = json.GetProperty("Username").GetString();
                string Password = json.GetProperty("Password").GetString();

                if (UserName != null && Password != null)
                {
                    if (db.DBFindUser(UserName))
                    {
                        Console.WriteLine("The user exists already");
                        return "HTTP/1.1 400 - User already exists";
                    }
                    else
                    {
                        User newUser = new User(UserName, Password);
                        db.DBCreateUser(newUser);
                        Console.WriteLine("The User was Added");
                        return "HTTP/1.1 201 OK";
                    }
                } else { return "HTTP/1.1 400 - wrong Format"; }
                /* OLD CODE FOR LOCAL DB!!!!
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
                */
            }
            catch (Exception ex)
            {
                Console.WriteLine("Handler.UserRequest: Error while parsing user request body. {0}", ex);
                return "HTTP/1.1 500 Internal Server Error";
            }
        }

        public static string SessionRequest(string body)
        {
            try
            {
                //Console.WriteLine("we are in User Requests"); //DEBUG
                JsonDocument jsonDocument = JsonDocument.Parse(body);
                JsonElement json = jsonDocument.RootElement;
                string UserName = json.GetProperty("Username").GetString();
                string Password = json.GetProperty("Password").GetString();
                if (UserName != null && Password != null)
                {
                    if (db.DBFindUser(UserName))
                    {
                        if (db.DBTestPW(UserName, Password))
                        {
                            User userToken = new User(UserName, Password);
                            string token = userToken.GenerateToken();
                            db.DBSetToken(UserName, Password, token);
                            Console.WriteLine("Token has been set");
                            string returnMessage = $"HTTP/1.1 200 -{token}";
                            return returnMessage;
                        }
                        else
                        {//wrong pw
                            return "HTTP/1.1 405 - Login failed, wrong pw";
                        }
                    }
                    else
                    {//no user found
                        return "HTTP/1.1 405 - Login failed from not exiting user";
                    }
                }
                else
                {//Username or PW or Both are empty
                    return "HTTP/1.1 405 - Login failed, wrong Format";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Handler.SessionRequest: Error while parsing user request body. {0}", ex);
                return "HTTP/1.1 500 Internal Server Error";
            }
        }
    
        public static string CreatePackageRequest(string body) //TODO AUTH FOR ADMIN!!!!!
        {
            JsonDocument jsonDocument = JsonDocument.Parse(body);
            JsonElement json = jsonDocument.RootElement;
            string name = json.GetProperty("name").GetString();
            int cost = json.GetProperty("cost").GetInt32();
            // hier noch den array dazubekommen!!
            Console.WriteLine("name:{0}, cost:{1}", name, cost.ToString());
            //zuerst neues pack erstellen, mit namen und kosten
            if (name != null && cost != null)
            {
                int packID = db.DBCreatePack(name, cost);
                if (!(packID == -1))
                {//funktioniert!
                    foreach (JsonElement card in json.GetProperty("cards").EnumerateArray())
                    {
                        //Console.WriteLine("Test test, ich bin in cards. sollte 3 mal kommen");
                        db.DBFillPack(packID, card.GetInt32());
                    }
                    return "HTTP/1.1 201 OK";
                }
                else
                {//something went wrong
                    Console.WriteLine("the DBCreatePack function does not work!");
                    return "HTTP/1.1 500 Internal Server Error";
                }

            }
            //dann die ID von dem ding zurückgeben

            //die ID nehmen, und bei pack-contents neuer entry erstellen, mit neuer id,(auto generated,) die ID die wir zurückbekommen, und die ID von der Karte(funktion machen)
            //wenn mehrere karten, einfach alle durchcyclen.

            return "HTTP/1.1 400 - wrong Format";
        }

        public static string ShowPackageRequest(string body) 
        {
            string AllPackages = db.getAllPacks();
            if (AllPackages.Length > 0)
            {
                return ("HTTP/1.1 200 OK\n" + AllPackages);
            }
            else 
            {
                return "HTTP/1.1 421 No Packages";
            }
        }
    
        public static string BuyPackage(string body) 
        {
            JsonDocument jsonDocument = JsonDocument.Parse(body);
            JsonElement json = jsonDocument.RootElement;
            int Packid = json.GetProperty("id").GetInt32();
            string token = json.GetProperty("token").GetString();
            Console.WriteLine("this is the id:{0} and this is the token:{1}",Packid, token);

            int id = db.DBAuth(token);
            if(id >= 0) 
            {
                Console.WriteLine("this user is authorized!");
                User thisuser = db.getFullUserBytoken(token);
                db.buyPack(thisuser.UserName, Packid, thisuser.Id);
                return "HTTP/1.1 201 OK";
            }
            else
            {
                { return "HTTP/1.1 600 - Authentication Failed"; };
            }
        }
    
        public static string GetScore(string body) 
        {
            string bestPlayers = db.DBGetBestPlayers();
            return "HTTP/1.1 201 OK\n" + bestPlayers;
        }
    
    }
}
