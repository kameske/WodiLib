using NUnit.Framework;
using WodiLib.Map;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class MapTreeNodeListTest
    {
        [Test]
        public static void GetMaxCapacityTest()
        {
            var instance = new MapTreeNodeList();
            var maxCapacity = instance.GetMaxCapacity();

            // 取得した値が容量最大値と一致すること
            Assert.AreEqual(maxCapacity, MapTreeNodeList.MaxCapacity);
        }

        [Test]
        public static void GetMinCapacityTest()
        {
            var instance = new MapTreeNodeList();
            var maxCapacity = instance.GetMinCapacity();

            // 取得した値が容量最大値と一致すること
            Assert.AreEqual(maxCapacity, MapTreeNodeList.MinCapacity);
        }

        [Test]
        public static void SerializeTest()
        {
            var target = new MapTreeNodeList();
            target.AdjustLength(3);
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }
    }
}