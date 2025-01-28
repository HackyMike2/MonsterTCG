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
        private static DBConnector _db = new DBConnector();
        //some could have been get...
        private const string USER_REQUEST_TEXT = "POST /users";
        private const string SESSION_REQUEST_TEXT = "POST /sessions";
        private const string CREATE_PACKAGE_TEXT = "POST /createpackage";
        private const string SHOW_PACKAGES = "POST /showpackages";
        private const string BUY_PACKAGE = "POST /package";
        private const string SCORE = "POST /score";
        private const string USER_DATA = "POST /change";
        private const string SHOW_DECK = "POST /showdeck";
        private const string PROFILE = "POST /profile";
        private const string EDIT_DECK = "POST /editdeck";
        private const string QUEUE = "POST /queue";

        public static string HandleRequest(string request, string body)
        {
            if (request.StartsWith(USER_REQUEST_TEXT))
            { 
                return UserRequest(body,_db);
            }
            else if (request.StartsWith(SESSION_REQUEST_TEXT))
            { 
                return SessionRequest(body, _db);
            }
            else if (request.StartsWith(CREATE_PACKAGE_TEXT)) 
            {
                return CreatePackageRequest(body, _db);
            }
            else if (request.StartsWith(SHOW_PACKAGES)) 
            {
                return ShowPackageRequest(_db);
            }
            else if (request.StartsWith(BUY_PACKAGE)) 
            {
                return BuyPackage(body, _db);
            }
            else if (request.StartsWith(SCORE)) 
            {
                return GetScore(_db);
            }
            else if (request.StartsWith(USER_DATA)) 
            {
                return ChangeUserData(body, _db);
            }
            else if (request.StartsWith(SHOW_DECK)) 
            {
                return ShowDeck(body, _db);
            }
            else if (request.StartsWith(PROFILE)) 
            {
                return ShowProfile(body, _db);
            }
            else if (request.StartsWith(EDIT_DECK)) 
            {
                return EditDeck(body, _db);
            }
            else if (request.StartsWith(QUEUE)) 
            {
                return Queue(body, _db);
            }
            else 
            {
                return "HTTP/1.1 404 - Handler not found";
            }
        }

        public static string UserRequest(string body, DBConnector db)
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
            }
            catch (Exception ex)
            {
                Console.WriteLine("Handler.UserRequest: Error while parsing user request body. {0}", ex);
                return "HTTP/1.1 500 Internal Server Error";
            }
        }

        public static string SessionRequest(string body, DBConnector db)
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
    
        public static string CreatePackageRequest(string body, DBConnector db) //TODO AUTH FOR ADMIN!!!!!
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

        public static string ShowPackageRequest(DBConnector db) 
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
    
        public static string BuyPackage(string body, DBConnector db) 
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
    
        public static string GetScore(DBConnector db) 
        {
            string bestPlayers = db.DBGetBestPlayers();
            return "HTTP/1.1 201 OK\n" + bestPlayers;
        }

        public static string ChangeUserData(string body, DBConnector db) 
        {
            JsonDocument jsonDocument = JsonDocument.Parse(body);
            JsonElement json = jsonDocument.RootElement;
            string Token = json.GetProperty("token").GetString();
            int id = db.DBAuth(Token);
            if (id >= 0)
            {
                try
                {
                    string UserName = json.GetProperty("name").GetString();
                    db.DBChangeName(id,UserName);
                }
                catch { }
                try
                {
                    string pw = json.GetProperty("pw").GetString();
                    db.DBChangePassword(id, pw);
                }
                catch { }
                return "HTTP/1.1 201 OK\n";
            }
            else 
            {
                return "return \"HTTP/1.1 600 - Authentication Failed";
            }
        }

        public static string ShowProfile(string body, DBConnector db)
        {
            JsonDocument jsonDocument = JsonDocument.Parse(body);
            JsonElement json = jsonDocument.RootElement;
            string Token = json.GetProperty("token").GetString();
            int id = db.DBAuth(Token);
            if (id >= 0) //Authed
            {
                User thisUser = db.DBgetFullUserByID(id);
                Console.WriteLine("This user is: {0}, has:{1} elo, and {2} coins.", thisUser.UserName, thisUser.Elo.ToString(), thisUser.Coins.ToString());
                return "HTTP/1.1 201 OK";
            }
            return "HTTP/1.1 600 - Authentication Failed";
        }

        public static string ShowDeck(string body, DBConnector db) 
        {
            JsonDocument jsonDocument = JsonDocument.Parse(body);
            JsonElement json = jsonDocument.RootElement;
            string Token = json.GetProperty("token").GetString();
            int id = db.DBAuth(Token);
            if (id >= 0) //Authed
            {
                string deck = db.DBShowDeck(id);
                return "HTTP/1.1 201 OK\n" + deck;
            }
            else 
            {
                return "HTTP/1.1 600 - Authentication Failed";
            }
        }
    
        public static string EditDeck(string body, DBConnector db)
        {
            JsonDocument jsonDocument = JsonDocument.Parse(body);
            JsonElement json = jsonDocument.RootElement;
            string Token = json.GetProperty("token").GetString();
            int id = db.DBAuth(Token);
            if (id >= 0) //Authed
            {
                try 
                {
                    Console.WriteLine("i am in editdeck-autzorized");
                    JsonElement cardsElement = json.GetProperty("cards");
                    int[] deck_cards = cardsElement.EnumerateArray().Select(card=>card.GetInt32()).ToArray();
                    int answer = db.DBEditDeck(id, deck_cards);
                    if(answer == 1) { return "HTTP/1.1 201 OK"; }
                    else { return "HTTP/1.1 500 Internal Server Error"; }
                    
                } 
                catch
                {
                    return "HTTP/1.1 500 Internal Server Error";
                }
            }

            return "HTTP/1.1 600 - Authentication Failed";
        }
    
        public static string Queue(string body, DBConnector db) 
        {
            JsonDocument jsonDocument = JsonDocument.Parse(body);
            JsonElement json = jsonDocument.RootElement;
            string Token = json.GetProperty("token").GetString();
            int id = db.DBAuth(Token);
            if (id >= 0) //Authed
            {
                //check if atleast 1 Card is in Cards with id
                BattleLogic.AddPlayerToQueue(id);
                (int,int)? result = BattleLogic.StartBattle();
                if(result != null) 
                {
                    User user1 = db.DBgetFullUserByID(result.Value.Item1);
                    User user2 = db.DBgetFullUserByID(result.Value.Item2);
                    tempCard[] user1Cards = db.DBReturnAllDeckCards(user1.Id);
                    tempCard[] user2Cards = db.DBReturnAllDeckCards(user2.Id);
                    (int, string) battleinfo =BattleLogic.Fight(user1,user2,user1Cards,user2Cards); //return 2 = player 1 won, return 1 = player 1 won, return 0 = draw.
                    if(battleinfo.Item1 == 1) //ELO anpassen!
                    {//player 1 won
                        db.DBChangeElo(user1.Id, 5);
                        db.DBChangeElo(user2.Id, -3);
                        return "HTTP/1.1 201 OK \n" + battleinfo.Item2; 
                    } 
                    else if(battleinfo.Item1 == 2)
                    { //player 2 won
                        db.DBChangeElo(user2.Id, 5);
                        db.DBChangeElo(user1.Id, -3);
                        return "HTTP/1.1 201 OK \n" + battleinfo.Item2; //warum kann ich kein \n mitgeben????
                    } 
                    else { return "HTTP/1.1 201 OK "; }
                }
                return "HTTP/1.1 201 OK - no second player yet";

            }
                return "HTTP/1.1 600 - Authentication Failed";
        }
    }
}
