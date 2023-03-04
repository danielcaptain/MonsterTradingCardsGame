using MonsterTradingCardsGame.Models;

namespace MTCG_Tests
{
    [TestClass]
    public class UserTests
    {
        [TestMethod]
        public void UserEmptyConstructorTest()
        {
            User userTest1 = new User();

            Assert.IsTrue(userTest1.Username == "" && userTest1.Coins == 0);
        }

        [TestMethod]
        public void UserConstructorWithoutTokenTest()
        {
            User userTest2 = new User("Testname", "Testpassword", 20, "Testalias", "Testbio", "Testimage");

            Assert.IsTrue(userTest2.Username == "Testname" && userTest2.Token == "Testname-mtcgToken");
        }

        [TestMethod]
        public void UserConstructorWithTokenTest()
        {
            User userTest2 = new User("Testname", "Testpassword", 20, "Testalias", "Testbio", "Testimage", "Testname-mtcgToken");

            Assert.IsTrue(userTest2.Username == "Testname" && userTest2.Token == "Testname-mtcgToken");
        }

        [TestMethod]
        public void CreateTokenTest()
        {
            string testToken = User.CreateToken("RandomUser");

            Assert.IsTrue(testToken == "RandomUser-mtcgToken");
        }

        [TestMethod]
        public void CheckIfTokenIsMissingOrInvalidTrueTest()
        {
            string testToken1 = "RandomUser-mtcgToken";
            bool testBool1 = User.CheckIfTokenIsMissingOrInvalid(testToken1);
            Assert.IsTrue(testBool1);
        }

        public void CheckIfTokenIsMissingOrInvalidAdminTest()
        {
            string testToken3 = "admin-mtcgToken";
            bool testBool3 = User.CheckIfTokenIsMissingOrInvalid(testToken3);
            Assert.IsTrue(testBool3);
        }

        [TestMethod]
        public void CheckIfTokenIsMissingOrInvalidFailTest()
        {
            string testToken2 = "RandomUser-Token";
            bool testBool2 = User.CheckIfTokenIsMissingOrInvalid(testToken2);
            Assert.IsFalse(testBool2);
        }


        [TestMethod]
        public void CheckIfTokenIsMissingOrInvalidEmptyTest()
        {
            string testToken2 = "";
            bool testBool2 = User.CheckIfTokenIsMissingOrInvalid(testToken2);
            Assert.IsFalse(testBool2);
        }
    }
}