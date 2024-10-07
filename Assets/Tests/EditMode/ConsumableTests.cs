using NUnit.Framework;
using Tests.Utils;

namespace Tests.EditMode
{
    public class ConsumableTests
    {
        [Test]
        public void ExtraLifeDefaultValues()
        {
            var consumable = TestUtils.TestableObjectFactory.Create<ExtraLife>();
            Assert.AreEqual("Life", consumable.GetConsumableName());
            Assert.AreEqual(Consumable.ConsumableType.EXTRALIFE, consumable.GetConsumableType());
            Assert.AreEqual(2000, consumable.GetPrice());
            Assert.AreEqual(5, consumable.GetPremiumCost());
        }

        [Test]
        public void CoinMagnetDefaultValues()
        {
            var consumable = TestUtils.TestableObjectFactory.Create<CoinMagnet>();
            Assert.AreEqual("Magnet", consumable.GetConsumableName());
            Assert.AreEqual(Consumable.ConsumableType.COIN_MAG, consumable.GetConsumableType());
            Assert.AreEqual(750, consumable.GetPrice());
            Assert.AreEqual(0, consumable.GetPremiumCost());
        }

        [Test]
        public void InvincibilityDefaultValues()
        {
            var consumable = TestUtils.TestableObjectFactory.Create<Invincibility>();
            Assert.AreEqual("Invincible", consumable.GetConsumableName());
            Assert.AreEqual(Consumable.ConsumableType.INVINCIBILITY, consumable.GetConsumableType());
            Assert.AreEqual(1500, consumable.GetPrice());
            Assert.AreEqual(5, consumable.GetPremiumCost());
        }

        [Test]
        public void Score2MultiplierDefaultValues()
        {
            var consumable = TestUtils.TestableObjectFactory.Create<Score2Multiplier>();
            Assert.AreEqual("x2", consumable.GetConsumableName());
            Assert.AreEqual(Consumable.ConsumableType.SCORE_MULTIPLAYER, consumable.GetConsumableType());
            Assert.AreEqual(750, consumable.GetPrice());
            Assert.AreEqual(0, consumable.GetPremiumCost());
        }
    }
}