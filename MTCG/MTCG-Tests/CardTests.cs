using MonsterTradingCardsGame.Models;
using static MonsterTradingCardsGame.Models.Card;

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

        [TestMethod]
        public void CheckElementEnum_WithWater_ReturnsWater()
        {
            var card = new Card();
            var checkName = "Water Element";

            var result = card.CheckElementEnum(checkName);

            Assert.AreEqual(ElementEnum.Water, result);
        }

        [TestMethod]
        public void CheckElementEnum_WithFire_ReturnsFire()
        {
            var card = new Card();
            var checkName = "Fire Element";

            var result = card.CheckElementEnum(checkName);

            Assert.AreEqual(ElementEnum.Fire, result);
        }
    }
}