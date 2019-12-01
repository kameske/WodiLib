using NUnit.Framework;
using WodiLib.Map;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class MapTreeOpenStatusListTest
    {
        [Test]
        public static void GetMaxCapacityTest()
        {
            var instance = new MapTreeOpenStatusList();
            var maxCapacity = instance.GetMaxCapacity();

            // 取得した値が容量最大値と一致すること
            Assert.AreEqual(maxCapacity, MapTreeOpenStatusList.MaxCapacity);
        }

        [Test]
        public static void GetMinCapacityTest()
        {
            var instance = new MapTreeOpenStatusList();
            var maxCapacity = instance.GetMinCapacity();

            // 取得した値が容量最大値と一致すること
            Assert.AreEqual(maxCapacity, MapTreeOpenStatusList.MinCapacity);
        }

        [Test]
        public static void SerializeTest()
        {
            var target = new MapTreeOpenStatusList();
            target.AdjustLength(3);
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }
    }
}