using MonsterTCG;
using System.Runtime.InteropServices.Marshalling;
using Moq;
namespace MonsterTCG_Test
{
    public class HTTP_Test
    {
        [Fact]
        public void HTTP_START_SERVER_SERVER_SHOULD_BE_STARTED_AFTER_5_SEC()
        {
            // Arrange
            HTTP http = new HTTP();

            // Act
            Thread handleClientThread = new Thread(() => http.StartServer());
            handleClientThread.Start();

            Thread.Sleep(5000);

            // Assert
            Assert.True(http.IsStarted);
        }
        [Fact]
        public void HTTP_TEST_CONNECTION_TO_DB()
        {
            // Arrange
            DBConnector db = new DBConnector();

            // Act
            string answer = db.DBTestConnection();
            // Assert
            Assert.True(answer == "Connection to the database is sucessful!");
        }
        [Fact]
        public void HTTP_GETCONTENTLENGTH_SHOULD_RETURN_RIGHT_CONTENT_LENGTH()
        {
            // Arrange

            string command = "curl -i -X POST http://localhost:10001/showpackages --header \"Content-Type: application/json\" -d \"\"";

            var mockStreamReader = new Mock<StreamReader>(Stream.Null);
            var setupSequence = mockStreamReader.SetupSequence(reader => reader.ReadLine());


            // Act
            int result = HTTP.GetContentLength(mockStreamReader.Object);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void USER_CREATE_USER_BASE_STATS()
        {
            // Arrange & Act
            User user = new User("Arnold", "extremelySecureUnhackablePassword");

            // Assert
            Assert.Equal(100,user.Elo);
            Assert.Equal(20,user.Coins);
        }


        [Fact]
        public void USER_CREATE_USER_GENERATE_TOKEN_TOKEN_SHOULD_BE_GENERATED()
        {
            // Arrange
            User user = new User("Joschi", "extremelySecureUnhackablePassword");

            // Act
            string result = user.GenerateToken();

            // Assert
            Assert.Equal("Joschi-mtcgToken", result);
        }

        [Fact]
        public void HANDLER_GETALLPACKS_SHOULD_RETURN_PACKS()
        {
            // Arrange

            Handler handler = new Handler();

            var mockDB = new Mock<DBConnector>();
            mockDB.Setup(db => db.getAllPacks()).Returns("Package1, Package2, Package3");

            // Act

            string result = Handler.ShowPackageRequest(mockDB.Object);

            // Assert

            Assert.Equal("HTTP/1.1 200 OK\nPackage1, Package2, Package3", result);
        }

        [Fact]
        public void HANDLER_GETALLPACKS_RETURN_NO_PACKS()
        {
            // Arrange

            Handler handler = new Handler();

            var mockDB = new Mock<DBConnector>();
            mockDB.Setup(db => db.getAllPacks()).Returns(string.Empty);

            // Act

            string result = Handler.ShowPackageRequest(mockDB.Object);

            // Assert

            Assert.Equal("HTTP/1.1 421 No Packages", result);
        }
        /*
        [Fact]
        public void HANDLER_BUYPACKAGE_SHOULD_BUY_PACKAGE()
        {
            // Arrange
            string body =
            """
            {
                "Id": 1,
                "Name": "WaterGoblin",
                "Damage": 10.0
            },
            {
                "Id": 2,
                "Name": "Dragon",
                "Damage": 50.0
            },
            {
                "Id": 3,
                "Name": "WaterSpell",
                "Damage": 20.0
            },
            {
                "Id": 4,
                "Name": "Ork",
                "Damage": 45.0
            },
            {
                "Id": 5,
                "Name": "FireSpell",
                "Damage": 25.0
            }
            """;



            Handler handler = new Handler();

            var mockDB = new Mock<DBConnector>();
            mockDB.Setup(db => db.DBAuth("Token")).Returns(1);

            // Act

            string result = Handler.BuyPackage(body,mockDB);

            // Assert

            Assert.Equal("HTTP/1.1 201 OK", result);
        }
        */
    }
}