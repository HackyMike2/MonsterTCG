using MonsterTCG;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTCG_Test
{
    public class BattleLogic_Test
    {
        [Fact]
        public void BATTLELOGIC_QUEUE()
        {
            //Arrange
            BattleLogic.SetQueue();
            ConcurrentQueue<int> queue = new ConcurrentQueue<int>();
            int ID = 5;
            //Act
            BattleLogic.AddPlayerToQueue(ID);
            queue = BattleLogic.GetQueue();
            //Assert
            Assert.True(queue.Count == 1);
        }
        [Fact]
        public void START_BATTLE_TEST_NOT_ENOUGH_PLAYER() 
        {
            //Arrange
            int ID = 5;
            BattleLogic.SetQueue();
            //Act
            BattleLogic.AddPlayerToQueue(ID);
            var getreturn = BattleLogic.StartBattle();
            //Assert
            Assert.True(getreturn == null);
        }
        [Fact]
        public void START_BATTLE_TEST_ENOUGH_PLAYER() 
        {
            //Arrange
            int ID1 = 5;
            int ID2 = 10;
            BattleLogic.SetQueue();
            //Act
            BattleLogic.AddPlayerToQueue(ID1);
             BattleLogic.AddPlayerToQueue(ID2);
            (int,int)? getreturn = BattleLogic.StartBattle();
            //Assert
            Assert.True(getreturn.Value.Item1 == ID1 && getreturn.Value.Item2 == ID2);
        }

        [Fact]
        public void FIGHT_NO_CARDS()
        {
            //Arrange
            User User1 = new User("user1", "passwort1");
            User User2 = new User("user2", "passwort2");
            tempCard[] User1TempCards = new tempCard[0];
            tempCard[] User2TempCards = new tempCard[0];
            //Act
            (int,string) answer = BattleLogic.Fight(User1, User2, User1TempCards, User2TempCards);
            //Assert
            Assert.True(answer.Item1 == 0);
        }
        [Fact]
        public void FIGHT_DRAW() 
        {
            //Arrange
            User User1 = new User("user1", "passwort1");
            User User2 = new User("user2", "passwort2");
            tempCard[] User1TempCards = new tempCard[1];
            tempCard[] User2TempCards = new tempCard[1];

            User1TempCards[0] = new tempCard { id = 0, Name = "monster1", Species = 1, Damage = 50, Type = 0 };
            User2TempCards[0] = new tempCard { id = 0, Name = "monster1", Species = 1, Damage = 50, Type = 0 };
            //Act
            (int,string) answer = BattleLogic.Fight(User1,User2,User1TempCards,User2TempCards);
            //Assert
            Assert.True(answer.Item1 == 0);

        }

        [Fact]
        public void FIGHT_WIN()
        {
            //Arrange
            User User1 = new User("user1", "passwort1");
            User User2 = new User("user2", "passwort2");
            tempCard[] User1TempCards = new tempCard[1];
            tempCard[] User2TempCards = new tempCard[1];

            User1TempCards[0] = new tempCard { id = 0, Name = "monster1", Species = 1, Damage = 60, Type = 0 };
            User2TempCards[0] = new tempCard { id = 0, Name = "monster1", Species = 1, Damage = 50, Type = 0 };
            //Act
            (int, string) answer = BattleLogic.Fight(User1, User2, User1TempCards, User2TempCards);
            //Assert
            Assert.True(answer.Item1 == 1);
        }
        [Fact]
        public void FIGHT_LOGIC_SPECIAL_MONSTER1()
        {
            //Arrange
            tempCard User1 = new tempCard { id = 0, Name = "monster1", Species = 0, Damage = 60, Type = 0 };
            tempCard User2 = new tempCard { id = 0, Name = "monster1", Species = 7, Damage = 50, Type = 0 };
            //Act
             int answer = BattleLogic.FightLogic(User1, User2);
            //Assert
            Assert.True(answer == 0);
        }
        [Fact]
        public void FIGHT_LOGIC_SPECIAL_MONSTER2()
        {
            //Arrange
            tempCard User1 = new tempCard { id = 0, Name = "monster1", Species = 2, Damage = 60, Type = 0 };
            tempCard User2 = new tempCard { id = 0, Name = "monster1", Species = 1, Damage = 50, Type = 0 };
            //Act
            int answer = BattleLogic.FightLogic(User1, User2);
            //Assert
            Assert.True(answer == 0);
        }

        [Fact]
        public void FIGHT_LOGIC_SPECIAL_MONSTER3()
        {
            //Arrange
            tempCard User1 = new tempCard { id = 0, Name = "monster1", Species = 7, Damage = 60, Type = 0 };
            tempCard User2 = new tempCard { id = 0, Name = "monster1", Species = 6, Damage = 50, Type = 0 };
            //Act
            int answer = BattleLogic.FightLogic(User1, User2);
            //Assert
            Assert.True(answer == 0);
        }

        [Fact]
        public void FIGHT_LOGIC_MONSTER_STANDARD()
        {
            //Arrange
            tempCard User1 = new tempCard { id = 0, Name = "monster1", Species = 5, Damage = 60, Type = 0 };
            tempCard User2 = new tempCard { id = 0, Name = "monster1", Species = 4, Damage = 50, Type = 0 };
            //Act
            int answer = BattleLogic.FightLogic(User1, User2);
            //Assert
            Assert.True(answer == User1.Damage);
        }

        [Fact]
        public void FIGHT_LOGIC_NON_CREATURE_CRIT()
        {
            //Arrange
            tempCard User1 = new tempCard { id = 0, Name = "monster1", Species = 0, Damage = 60, Type = 0 };
            tempCard User2 = new tempCard { id = 0, Name = "Zauber1", Species = 8, Damage = 50, Type = 2 };
            //Act
            int answer = BattleLogic.FightLogic(User1, User2);
            //Assert
            Assert.True(answer == User1.Damage*2);
        }

        [Fact]
        public void FIGHT_LOGIC_NON_CREATURE_NOT_EFFECTIV()
        {
            //Arrange
            tempCard User1 = new tempCard { id = 0, Name = "monster1", Species = 0, Damage = 60, Type = 2 };
            tempCard User2 = new tempCard { id = 0, Name = "Zauber1", Species = 8, Damage = 50, Type = 0 };
            //Act
            int answer = BattleLogic.FightLogic(User1, User2);
            //Assert
            Assert.True(answer == User1.Damage /2);
        }

        [Fact]
        public void START_BATTLE_TEST_PLAYER_TWICE()
        {
            //Arrange
            int ID1 = 5;
            int ID2 = 10;
            BattleLogic.SetQueue();
            //Act
            BattleLogic.AddPlayerToQueue(ID1);
            BattleLogic.AddPlayerToQueue(ID1);
            var getreturn = BattleLogic.StartBattle();
            //Assert
            Assert.True(getreturn == null);
        }

    }
}
