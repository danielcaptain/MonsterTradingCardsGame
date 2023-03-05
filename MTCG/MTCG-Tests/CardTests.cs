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
            var checkName = "Water Element";

            var result = Card.CheckElementEnum(checkName);

            Assert.AreEqual(ElementEnum.Water, result);
        }

        [TestMethod]
        public void CheckElementEnum_WithFire_ReturnsFire()
        {
            var checkName = "Fire Element";

            var result = Card.CheckElementEnum(checkName);

            Assert.AreEqual(ElementEnum.Fire, result);
        }

        [TestMethod]
        public void CheckMonsterEnum_WithGoblin()
        {
            var checkName = "WaterGoblin";

            var result = MonsterCard.CheckMonsterEnum(checkName);

            Assert.AreEqual(MonsterCard.MonsterEnum.Goblin, result);
        }


        [TestMethod]
        public void CheckMonsterEnum_WithDragon()
        {
            var checkName = "FireDragon";

            var result = MonsterCard.CheckMonsterEnum(checkName);

            Assert.AreEqual(MonsterCard.MonsterEnum.Dragon, result);
        }

        [TestMethod]
        public void CheckMonsterEnum_WithTroll()
        {
            var checkName = "NormalTroll";

            var result = MonsterCard.CheckMonsterEnum(checkName);

            Assert.AreEqual(MonsterCard.MonsterEnum.Troll, result);
        }
    }
}