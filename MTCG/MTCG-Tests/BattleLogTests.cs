using MonsterTradingCardsGame.Models;

namespace MTCG_Tests
{
    [TestClass]
    public class BattleLogTests
    {
        [TestMethod]
        public void LogRound_AddsRoundToLog()
        {
            var battleLog = new BattleLog();

            battleLog.LogRound("Round 1");

            Assert.AreEqual("\nRound 1", BattleLog.log);
        }

        [TestMethod]
        public void ReturnLog_ReturnsFullLog()
        {
            var battleLog = new BattleLog();
            battleLog.LogRound("Round 1");
            battleLog.LogRound("Round 2");

            var fullLog = battleLog.ReturnLog();

            Assert.AreEqual("\nRound 1\nRound 2", fullLog);
        }
    }
}