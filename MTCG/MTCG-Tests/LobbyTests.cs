using MonsterTradingCardsGame.Models;

namespace MTCG_Tests
{
    [TestClass]
    public class LobbyTests
    {
        [TestMethod]
        public void AddUserToLobby_WhenCalled_AddsUserToLobby()
        {
            var user = new User("John", "Testpassword", 20, "BattleKiller", "Empty", "--");
            var initialCount = Lobby.CheckCountOfLobby();

            Lobby.AddUserToLobby(user);

            Assert.AreEqual(initialCount + 1, Lobby.CheckCountOfLobby());
            Assert.IsTrue(Lobby.GetFirstUserInLobby() == user);
        }

        [TestMethod]
        public void RemoveUserFromLobby_WhenCalled_RemovesUserFromLobby()
        {
            var user1 = new User("John", "Testpassword", 20, "BattleKiller", "Empty", "--");
            var user2 = new User("Eric", "Passw0rd", 15, "CoolGuy", "HiHi", ":-)");
            Lobby.AddUserToLobby(user1);
            Lobby.AddUserToLobby(user2);
            var initialCount = Lobby.CheckCountOfLobby();

            Lobby.RemoveUserFromLobby(user1);

            Assert.AreEqual(initialCount - 1, Lobby.CheckCountOfLobby());
        }

        [TestMethod]
        public void ClearLobby_RemovesAllUsersFromLobby()
        {
            var user1 = new User("John", "Testpassword", 20, "BattleKiller", "Empty", "--");
            var user2 = new User("Eric", "Passw0rd", 15, "CoolGuy", "HiHi", ":-)");
            var user3 = new User("Yasmin", "1234", 10, "Gamer", "easy", "::");
            Lobby.AddUserToLobby(user1);
            Lobby.AddUserToLobby(user2);
            Lobby.AddUserToLobby(user3);

            Lobby.ClearLobby();

            Assert.AreEqual(0, Lobby.CheckCountOfLobby());
        }
    }
}