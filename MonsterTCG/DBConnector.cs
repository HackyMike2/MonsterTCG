﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace MonsterTCG
{
    public class DBConnector
    {
        private string connectionstring = "";
        
        
        public DBConnector() 
        {
            connectionstring = "Host=localhost;Port=9009;Username=postgres;Password=postgres;Database=Monster_TCG";
        }

        public string DBTestConnection()
        {
            try
            {
                NpgsqlConnection connection = new NpgsqlConnection(connectionstring);
                connection.Open();
                Console.WriteLine("Connection to the database is sucessful!");
                connection.Close();
                return "Connection to the database is sucessful!";
            }
            catch
            {
                Console.WriteLine("an error occured while trying to connect to the database");
                return "an error occured while trying to connect to the database";
            }


        }
        public bool DBTestPW(string Username,string pw) 
        {
            bool PwCorrect = false;
            string querystring = "SELECT password FROM tcguser WHERE username = @username;";
            NpgsqlConnection connection = new NpgsqlConnection(connectionstring);
            connection.Open();
            using (var cmd = new NpgsqlCommand(querystring, connection)) 
            {
                cmd.Parameters.AddWithValue("username", Username);
                var result = cmd.ExecuteScalar();
                string resultpw = Convert.ToString(result);
                Console.WriteLine("The Resultpw is: {0}",resultpw);
                if (result != null && pw == resultpw) 
                {
                    PwCorrect = true;
                }
            }
                return PwCorrect;
        }

        public void DBSetToken(string Username, string pw,string token) 
        {
            string querystring = @"UPDATE tcguser SET securitytoken = @token WHERE username = @username;";
            NpgsqlConnection connection = new NpgsqlConnection(connectionstring);
            connection.Open();
            using (var cmd = new NpgsqlCommand(querystring, connection))
            {
                cmd.Parameters.AddWithValue("username", Username);
                cmd.Parameters.AddWithValue("token", token);
                cmd.ExecuteNonQuery();
            }
        }

        public bool DBFindUser(string username) 
        {
            bool userExists = false;
            string querystring = @"SELECT COUNT(*) FROM tcguser WHERE username = @username";
            NpgsqlConnection connection = new NpgsqlConnection(connectionstring);
            connection.Open();
            using (var cmd = new NpgsqlCommand(querystring, connection))
            {
                cmd.Parameters.AddWithValue("username", username);

                int count = Convert.ToInt32(cmd.ExecuteScalar());

                if (count > 0)
                {
                    userExists = true;
                }
            }
            connection.Close();
            return userExists;
        }

        public void DBCreateUser(User benutzer) 
        {

            string querystring = @"INSERT INTO tcguser (userName, password, coins, elo) VALUES (@userName, @password, @coins, @elo)";
            NpgsqlConnection connection = new NpgsqlConnection(connectionstring);
            connection.Open();
            using (var cmd = new NpgsqlCommand(querystring, connection)) 
            {
                string token = benutzer.GenerateToken();
                cmd.Parameters.AddWithValue("userName", benutzer.UserName);
                cmd.Parameters.AddWithValue("password", benutzer.Password); 
                cmd.Parameters.AddWithValue("coins", 20);
                cmd.Parameters.AddWithValue("elo", 100);
                cmd.ExecuteNonQuery();

            }
            connection.Close();
        }

        public void DBLoginUser(User benutzer)
        {
            bool userexists = false;
            userexists = DBFindUser(benutzer.UserName);
            if (userexists) 
            {

                benutzer.Password = benutzer.UserName;

                //test if password is correct / ask for password (mit stärnchen ;))
                string querystring = @"INSERT INTO tcguser(securitytoken) VALUES (@securitytoken)";
                NpgsqlConnection connection = new NpgsqlConnection(connectionstring);
                connection.Open();
                using (var cmd = new NpgsqlCommand(querystring, connection))
                {
                    string token = benutzer.GenerateToken();
                    cmd.Parameters.AddWithValue("securitytoken", token);
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
                Console.WriteLine("User has been tokenized");
            } 
            
        }

        public int DBgetCardCount() 
        {
            string querystring = "SELECT COUNT(*) FROM Card";
            int count = 0;
            NpgsqlConnection connection = new NpgsqlConnection(connectionstring);
            connection.Open();
            using (var cmd = new NpgsqlCommand(querystring, connection))
            {
                count = Convert.ToInt32(cmd.ExecuteScalar());
            }
            connection.Close();
            //Console.WriteLine("The card count is:"); DEBUG
            //Console.WriteLine(count.ToString()); DEBUG
            return count;
        }

       /* public int DBGetCardIdAt(int CardAT) 
        {
            
            return CardAT;
        }*/

        public int DBGetIDFromRandomCard() //add card count to cardcollection to make it easier
        {
            int count = DBgetCardCount();
            Random rnd = new Random();
            int cardAT = rnd.Next(0,count);
            Console.WriteLine("the Card AT number is:"); //DEBUG
            Console.WriteLine(cardAT.ToString()); //DEBUG
            int ID = 0; //DEBUG!

            string querystring = @"SELECT * FROM Card ORDER BY ID ASC OFFSET @n - 1 LIMIT 1";
            NpgsqlConnection connection = new NpgsqlConnection(connectionstring);
            connection.Open();
            using (var cmd = new NpgsqlCommand(querystring, connection))
            {
                cmd.Parameters.AddWithValue("n", cardAT);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ID = reader.GetInt32(reader.GetOrdinal("ID"));
                        Console.WriteLine("This is the ID:"); //DEBUG
                        Console.WriteLine(ID.ToString()); //DEBUG!
                    }
                }

            }
            connection.Close();
            return ID;
        }

        public void DBFillPack(int PackID, int CardID)
        {
            string querystring = "INSERT INTO packcontents(packid, cardid) VALUES (@packid, @cardid)";
            NpgsqlConnection connection = new NpgsqlConnection(connectionstring);
            connection.Open();
            using (var cmd = new NpgsqlCommand(querystring, connection))
            {
                cmd.Parameters.AddWithValue("packid", PackID);
                cmd.Parameters.AddWithValue("cardid", CardID);
                cmd.ExecuteNonQuery();
            }
            connection.Close();

        }

        public virtual string getAllPacks() 
        {
            string answer = "";
            string queryString = "SELECT name, cost FROM packs;";
            NpgsqlConnection connection = new NpgsqlConnection(connectionstring);
            connection.Open();
            using (var cmd = new NpgsqlCommand(queryString, connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = reader.GetString(reader.GetOrdinal("name"));
                        int cost = reader.GetInt32(reader.GetOrdinal("cost"));
                        answer += ("Name: " + name+", cost: "+cost.ToString()+"\n");
                    }
                }
                connection.Close();
                return answer;
            }
        }

        public int DBAuth(string token) // wenn >= 0, dann ist die Auth. durchgegangen!
        {
            int id = -1;
            string querystring = @"SELECT id FROM tcguser WHERE securitytoken = @token";
            NpgsqlConnection connection = new NpgsqlConnection(connectionstring);
            connection.Open();
            using (var cmd = new NpgsqlCommand(querystring, connection))
            {
                cmd.Parameters.AddWithValue("token", token);

                var result = cmd.ExecuteScalar();
                if(result != null) 
                {
                    id = Convert.ToInt32(result);
                }
            }
            connection.Close();
            return id;
        }

        public void buyPack(string PlayerUsername, int packid,int userid, User user)
        {
            Random rnd = new Random();
            int PackCount = DBgetCountOfPack(packid);
            //Console.WriteLine("the Count of Pack is:{0}", PackCount);
            if (PackCount > 0 && user.Coins > 10)
            {
                //random number from 0 to Count-1
                int Cardnumber = rnd.Next(0,PackCount);
                //Console.WriteLine("The Cardnumber is {0}", Cardnumber.ToString());
                //get the id of a random card from that pack
                
                int CardID = DBGetCardIdFromPack(packid, Cardnumber);
                //Console.WriteLine("The CardID is:{0}", CardID);
                DBAddCardToPlayerCollection(PlayerUsername, CardID);
                //Console.WriteLine("The player with the username:{0} and the id of {1} has gotten the card with the id of {2}", PlayerUsername, userid.ToString(), CardID.ToString());
                DBChangeCoins(user.Id);
            } 
        }

        public string DBGetBestPlayers() 
        {
            string bestPlayers = "";
            string queryString = @"SELECT username, elo FROM tcguser ORDER BY elo DESC LIMIT 10";
            NpgsqlConnection connection = new NpgsqlConnection(connectionstring);
            connection.Open();
            using (var cmd = new NpgsqlCommand(queryString, connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        string username = reader.GetString(0);
                        int elo = reader.GetInt32(1);
                        bestPlayers += "Player: " + username.ToString() + " with the elo of: " + elo + "\n";
                    }
                }
            }
                return bestPlayers;
        }

        public void DBChangeName(int id,string newUsername) 
        {
            string querystring = "UPDATE tcguser SET username = @newname WHERE id = @id";
            NpgsqlConnection connection = new NpgsqlConnection(connectionstring);
            connection.Open();
            //Console.WriteLine("The id is{0}, and the new Username should be:{1}",id,newUsername);
            using (var cmd = new NpgsqlCommand(querystring, connection))
            {
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("newname",newUsername);
                cmd.ExecuteNonQuery();
            }
        }

        public void DBChangeCoins(int id) 
        {
            string querystring = "UPDATE tcguser SET coins = coins - 10 WHERE id = @id";
            NpgsqlConnection connection = new NpgsqlConnection(connectionstring);
            connection.Open();
            using (var cmd = new NpgsqlCommand(querystring, connection))
            {
                cmd.Parameters.AddWithValue("id", id);
                cmd.ExecuteNonQuery();
            }
        }

        public string DBShowDeck(int id)
        {
            string deck = "Your cards in your deck are:\n";
            string querystring = "SELECT userid, cardid FROM deck WHERE userid = @id";
            NpgsqlConnection connection = new NpgsqlConnection(connectionstring);
            connection.Open();
            using (var cmd = new NpgsqlCommand(querystring, connection))
            {
                cmd.Parameters.AddWithValue("id", id);
                var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        int UserID = reader.GetInt32(reader.GetOrdinal("userid"));
                        int CardID = reader.GetInt32(reader.GetOrdinal("cardid"));
                        deck += DBGetCardNameByID(CardID) + "\n";
                    }
            }
            connection.Close();
            return deck;
        }

        public tempCard[] DBReturnAllDeckCards(int id) //needs userID!
        {
           // Console.WriteLine("i am in DBReturnAllDeckCards"); //DEBUG!
            tempCard[] DeckCards = new tempCard[4];
            int i = 0;
            string querystring = "SELECT userid, cardid FROM deck WHERE userid = @id";
            NpgsqlConnection connection = new NpgsqlConnection(connectionstring);
            connection.Open();
            using (var cmd = new NpgsqlCommand(querystring, connection))
            {
                cmd.Parameters.AddWithValue("id", id);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int deckCardID = reader.GetInt32(reader.GetOrdinal("cardid"));
                    DeckCards[i] = DBGetFullCardByID(deckCardID);
                    i++;
                }
            }
            connection.Close();
            return DeckCards;
        }

        public string DBGetCardNameByID(int id)
        {
            string cardname = "";
            string queryString = "SELECT name FROM card WHERE id = @cardid;";
            NpgsqlConnection connection = new NpgsqlConnection(connectionstring);
            connection.Open();
            using (var cmd = new NpgsqlCommand(queryString, connection))
            {
                cmd.Parameters.AddWithValue("cardid", id);
                var reader = cmd.ExecuteReader();
                if (reader.Read()) 
                {
                    cardname = reader.GetString(reader.GetOrdinal("name"));
                }
            }
            connection.Close();
            return cardname;
        }

        public tempCard DBGetFullCardByID(int id) //for Fight to get all informations of a Card.
        {
           // Console.WriteLine("I am in DBGetFullCardByID!"); //DEBUG!!
            tempCard tpCard = new tempCard();
            string queryString = "SELECT * FROM card WHERE id = @cardid;";
            NpgsqlConnection connection = new NpgsqlConnection(connectionstring);
            connection.Open();
            using (var cmd = new NpgsqlCommand(queryString, connection))
            {
                cmd.Parameters.AddWithValue("cardid", id);
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                   tpCard.id  = reader.GetInt32(reader.GetOrdinal("id"));
                   tpCard.Name = reader.GetString(reader.GetOrdinal("name"));
                   tpCard.Damage = reader.GetInt32(reader.GetOrdinal("Damage"));
                   tpCard.Type = reader.GetInt32(reader.GetOrdinal("Type"));
                   tpCard.Species = reader.GetInt32(reader.GetOrdinal("Species"));

                }
            }
            connection.Close();
            return tpCard;
        }

        public void DBChangePassword(int id, string newPW)
        {
            string querystring = "UPDATE tcguser SET password = @newPW WHERE id = @id";
            NpgsqlConnection connection = new NpgsqlConnection(connectionstring);
            connection.Open();
            using (var cmd = new NpgsqlCommand(querystring, connection))
            {
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("newPW", newPW);
                cmd.ExecuteNonQuery();
            }
        }

        public int DBGetCardIdFromPack(int packid, int randomNumber)
        {
            string querystring = "SELECT cardid FROM packcontents WHERE packid = @packId LIMIT 1 OFFSET @offset";
            NpgsqlConnection connection = new NpgsqlConnection(connectionstring);
            connection.Open();
            using (var cmd = new NpgsqlCommand(querystring, connection))
            {
                cmd.Parameters.AddWithValue("packId", packid);
                cmd.Parameters.AddWithValue("offset", randomNumber);
                var result = cmd.ExecuteScalar();
                if (result != null) 
                {
                    connection.Close();
                    return Convert.ToInt32(result);
                }
            }
            connection.Close();
            return 0;
        }

        public int DBgetCountOfPack(int packid) 
        {
            string queryString = "SELECT COUNT(*) FROM packcontents WHERE packid = @packid";
            NpgsqlConnection connection = new NpgsqlConnection(connectionstring);
            connection.Open();
            using (var cmd = new NpgsqlCommand(queryString, connection))
            {
                cmd.Parameters.AddWithValue("packid", packid);
                var result = cmd.ExecuteScalar();
                if (result != null) 
                {
                    return Convert.ToInt32(result);
                }
            }
            connection.Close();
            return 0;
        }

        public User DBgetFullUserByName(string name) //-1 return if something went wrong
        {
            int id = -1;
            string Username = "";
            string password = "";
            string sectoken = "";
            int elo = 0;
            int coins = 0;
            string namequery = "SELECT * FROM tcguser WHERE username = @username";
            NpgsqlConnection connection = new NpgsqlConnection(connectionstring);
            connection.Open();
            using (var cmd = new NpgsqlCommand(namequery, connection)) 
            {
                cmd.Parameters.AddWithValue("username", name);
                using (var reader = cmd.ExecuteReader()) 
                {
                    if (reader.Read()) 
                    {
                        id = reader.GetInt32(reader.GetOrdinal("id"));
                        Username = reader.GetString(reader.GetOrdinal("username"));
                        password = reader.GetString(reader.GetOrdinal("password"));
                        sectoken = reader.GetString(reader.GetOrdinal("securitytoken"));
                        elo = reader.GetInt32(reader.GetOrdinal("elo"));
                        coins = reader.GetInt32(reader.GetOrdinal("coins"));
                    }
                    else 
                    {
                        Console.WriteLine("reader failed");
                    }
                }

            }
            User user = new User(id,Username,password,coins,elo,sectoken);
            connection.Close();
            return user;
        }

        public User DBgetFullUserByID(int id) //-1 return if something went wrong
        {
            string Username = "";
            string password = "";
            string sectoken = "";
            int elo = 0;
            int coins = 0;
            string namequery = "SELECT * FROM tcguser WHERE id = @id";
            NpgsqlConnection connection = new NpgsqlConnection(connectionstring);
            connection.Open();
            using (var cmd = new NpgsqlCommand(namequery, connection))
            {
                cmd.Parameters.AddWithValue("id", id);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        id = reader.GetInt32(reader.GetOrdinal("id"));
                        Username = reader.GetString(reader.GetOrdinal("username"));
                        password = reader.GetString(reader.GetOrdinal("password"));
                        sectoken = reader.GetString(reader.GetOrdinal("securitytoken"));
                        elo = reader.GetInt32(reader.GetOrdinal("elo"));
                        coins = reader.GetInt32(reader.GetOrdinal("coins"));
                    }
                    else
                    {
                        Console.WriteLine("reader failed");
                    }
                }

            }
            User user = new User(id, Username, password, coins, elo, sectoken);
            connection.Close();
            return user;
        }

        public User getFullUserBytoken(string token) //-1 return if something went wrong
        {
            int id = -1;
            string Username = "";
            string password = "";
            string sectoken = "";
            int elo = 0;
            int coins = 0;
            string namequery = "SELECT * FROM tcguser WHERE securitytoken = @token";
            NpgsqlConnection connection = new NpgsqlConnection(connectionstring);
            connection.Open();
            using (var cmd = new NpgsqlCommand(namequery, connection))
            {
                cmd.Parameters.AddWithValue("token", token);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        id = reader.GetInt32(reader.GetOrdinal("id"));
                        Username = reader.GetString(reader.GetOrdinal("username"));
                        password = reader.GetString(reader.GetOrdinal("password"));
                        sectoken = reader.GetString(reader.GetOrdinal("securitytoken"));
                        elo = reader.GetInt32(reader.GetOrdinal("elo"));
                        coins = reader.GetInt32(reader.GetOrdinal("coins"));
                    }
                    else
                    {
                        Console.WriteLine("reader failed");
                    }
                }

            }
            User user = new User(id, Username, password, coins, elo, sectoken);
            connection.Close();
            return user;
        }

        //EditDeck
        public int DBEditDeck(int id, int[] cards)
        {
            string querystring = "SELECT COUNT(*) FROM collection WHERE userid = @userid";
            int cardcount = 0;
            NpgsqlConnection connection = new NpgsqlConnection(connectionstring);
            connection.Open();
            using (var cmd = new NpgsqlCommand(querystring, connection))
            {
                cmd.Parameters.AddWithValue("userid", id);
                cardcount = Convert.ToInt32(cmd.ExecuteScalar());
            }
            if (cardcount > cards.Length && cardcount >= cards.Max())
            {
                Console.WriteLine("the cardcount was bigger than the length AND cardcount was bigger than the cards.Max ");
                //step 2: delete all old tables
                querystring = "DELETE FROM deck WHERE userid = @userid;";

                using (var cmd = new NpgsqlCommand(querystring, connection))
                {
                    cmd.Parameters.AddWithValue("userid", id);
                    cmd.ExecuteNonQuery();
                }

                //step 3: retrieve card ids.

                List<int> cardIds = new List<int>();
                querystring = "SELECT cardid FROM collection WHERE userid = @userid ORDER BY cardid LIMIT 1 OFFSET @offset";
                foreach (int index in cards)
                {
                    using (var cmd = new NpgsqlCommand(querystring, connection))
                    {
                        cmd.Parameters.AddWithValue("userid", id);
                        cmd.Parameters.AddWithValue("offset", index - 1);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                cardIds.Add(reader.GetInt32(0));
                            }
                        }
                    }
                }
                //step 3: add cards to deck
                querystring = "INSERT INTO deck (userid, cardid) VALUES (@userId, @cardId)";
                using (var cmd = new NpgsqlCommand(querystring, connection))
                {
                    cmd.Parameters.AddWithValue("userId", id);
                    foreach(int cardId in cardIds) 
                    {
                        cmd.Parameters.AddWithValue("cardId", cardId);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.RemoveAt("cardId");
                    }
                }
                connection.Close();
                return 1;

            }
            else { connection.Close(); return 0; }

         return 0;
        }

        public void DBChangeElo(int id, int EloChange) 
        {
            string querystring = "UPDATE tcguser SET elo = GREATEST(0, elo + @eloChange) WHERE id = @userId";
            NpgsqlConnection connection = new NpgsqlConnection(connectionstring);
            connection.Open();
            using (var cmd = new NpgsqlCommand(querystring, connection))
            {
                cmd.Parameters.AddWithValue("userId", id);
                cmd.Parameters.AddWithValue("eloChange", EloChange);
                cmd.ExecuteNonQuery();
            }
        }

        public int DBCreatePack(string name,int cost) 
        {
            int? count;
            int id = 0;
            NpgsqlConnection connection = new NpgsqlConnection(connectionstring);
            connection.Open();
            //first check if the Name is not yet used
            string namequery = "SELECT COUNT(*) FROM packs WHERE name = @name";
            using (var cmd = new NpgsqlCommand(namequery, connection)) 
            {
                cmd.Parameters.AddWithValue("name", name);
                count = Convert.ToInt32(cmd.ExecuteScalar());
            }
                //end of check if name is not used
            if(count != null) 
            {
                string querystring = "INSERT INTO packs (name, cost) VALUES (@name, @cost) RETURNING packid";


                using (var cmd = new NpgsqlCommand(querystring, connection))
                {
                    cmd.Parameters.AddWithValue("name", name);
                    cmd.Parameters.AddWithValue("cost", cost);
                    id = Convert.ToInt32(cmd.ExecuteScalar());
                    Console.WriteLine("the id is :{0}", id);
                }
                connection.Close();
                return id;
            }
            else { id = -1; connection.Close(); return id; }
        }

        public void DBAddCardToPlayerCollection(string username, int cardID)
        {
            //check for the ID of the USER
            string querystring = @"SELECT id FROM tcguser WHERE username = @userName;";
            int userID = 0;
            NpgsqlConnection connection = new NpgsqlConnection(connectionstring);
            connection.Open();
            using (var getUserCmd = new NpgsqlCommand(querystring, connection))
            {
                getUserCmd.Parameters.AddWithValue("userName", username);
                var result = getUserCmd.ExecuteScalar();

                if(result == null)
                {
                    Console.WriteLine("User:{0} does not exist", username);
                    return;
                }
                else 
                {
                    userID = Convert.ToInt32(result);
                    Console.WriteLine("User:{0}, does exist and hast the ID: {1}",username, userID.ToString());
                }
            }
            //check if the card has been set once
            string checkCardQuery = "SELECT count FROM collection WHERE userid = @userID AND cardid = @cardID;";
            int? currentCount = null;
            using (var checkCardCmd = new NpgsqlCommand(checkCardQuery, connection))
            {
                checkCardCmd.Parameters.AddWithValue("userID", userID);
                checkCardCmd.Parameters.AddWithValue("cardID", cardID);
                var result = checkCardCmd.ExecuteScalar();
                if (result != null) 
                {
                    currentCount = Convert.ToInt32(result);
                    Console.WriteLine("There is Here to be updated");
                }
                else { Console.WriteLine("New Table has to be Made"); }
            }
            if (currentCount.HasValue) 
            {
                string updateCardQuery = "UPDATE collection SET count = count + 1 WHERE UserID = @userID AND CardID = @cardID;";
                using (var updateCardCmd = new NpgsqlCommand(updateCardQuery, connection))
                {
                    updateCardCmd.Parameters.AddWithValue("userID", userID);
                    updateCardCmd.Parameters.AddWithValue("cardID", cardID);
                    updateCardCmd.ExecuteNonQuery();
                    Console.WriteLine("The Count has been updated");
                }
            }
            else 
            {
                string insertCardQuery = "INSERT INTO collection (userid, cardid, count) VALUES (@userID, @cardID, 1);";
                using (var insertCardCmd = new NpgsqlCommand(insertCardQuery, connection)) 
                {
                    insertCardCmd.Parameters.AddWithValue("userID", userID);
                    insertCardCmd.Parameters.AddWithValue("cardID", cardID);
                    insertCardCmd.ExecuteNonQuery();
                    Console.WriteLine("The Count has been set to 1");
                }
            }
                connection.Close();
        }
    }
}