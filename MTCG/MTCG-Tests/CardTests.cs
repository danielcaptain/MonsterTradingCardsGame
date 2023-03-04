using MonsterTradingCardsGame.Models;

namespace MTCG_Tests
{
    [TestClass]
    public class CardTests
    {
        [TestMethod]
        public void MonsterCardConstructorSetsPropertiesTest()
        {
            Guid id = Guid.NewGuid();
            string name = "Test Card";
            double damage = 10.5;
            Card.ElementEnum element = Card.ElementEnum.Fire;
            MonsterCard.MonsterEnum monster = MonsterCard.MonsterEnum.Goblin;

            MonsterCard card = new MonsterCard(id, name, damage, element, monster);

            Assert.AreEqual(id, card.Id);
            Assert.AreEqual(name, card.Name);
            Assert.AreEqual(damage, card.Damage);
            Assert.AreEqual(element, card.Element);
            Assert.AreEqual(monster, card.Monster);
        }

        [TestMethod]
        public void MonsterCard_SetOnceTest()
        {
            Guid id = Guid.NewGuid();
            string name = "Test Card";
            double damage = 10.5;
            Card.ElementEnum element = Card.ElementEnum.Fire;
            MonsterCard.MonsterEnum monster = MonsterCard.MonsterEnum.Goblin;
            MonsterCard card = new MonsterCard(id, name, damage, element, monster);

            card.Monster = MonsterCard.MonsterEnum.Dragon;

            Assert.AreNotEqual(monster, card.Monster);
        }

        [TestMethod]
        public void SpellCardConstructorSetsPropertiesTest()
        {
            Guid id = Guid.NewGuid();
            string name = "Test Card";
            double damage = 10.5;
            Card.ElementEnum element = Card.ElementEnum.Fire;

            SpellCard card = new SpellCard(id, name, damage, element);

            Assert.AreEqual(id, card.Id);
            Assert.AreEqual(name, card.Name);
            Assert.AreEqual(damage, card.Damage);
            Assert.AreEqual(element, card.Element);
        }
    }
}