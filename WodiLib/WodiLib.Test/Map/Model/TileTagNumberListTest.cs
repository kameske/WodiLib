using NUnit.Framework;
using WodiLib.Map;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class TileTagNumberListTest
    {
        [Test]
        public static void GetMaxCapacityTest()
        {
            var instance = new TileTagNumberList();
            var maxCapacity = instance.GetMaxCapacity();

            // 取得した値が容量最大値と一致すること
            Assert.AreEqual(maxCapacity, TileTagNumberList.MaxCapacity);
        }

        [Test]
        public static void GetMinCapacityTest()
        {
            var instance = new TileTagNumberList();
            var maxCapacity = instance.GetMinCapacity();

            // 取得した値が容量最大値と一致すること
            Assert.AreEqual(maxCapacity, TileTagNumberList.MinCapacity);
        }

        [Test]
        public static void SerializeTest()
        {
            var target = new TileTagNumberList();
            target.AdjustLength(30);
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }
    }
}