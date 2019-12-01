using NUnit.Framework;
using WodiLib.Map;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class MapEventPageListTest
    {
        [Test]
        public static void GetMaxCapacityTest()
        {
            var instance = new MapEventPageList(new []
            {
                new MapEventPage(),
            });
            var maxCapacity = instance.GetMaxCapacity();

            // 取得した値が容量最大値と一致すること
            Assert.AreEqual(maxCapacity, MapEventPageList.MaxCapacity);
        }

        [Test]
        public static void GetMinCapacityTest()
        {
            var instance = new MapEventPageList(new []
            {
                new MapEventPage(),
            });
            var maxCapacity = instance.GetMinCapacity();

            // 取得した値が容量最大値と一致すること
            Assert.AreEqual(maxCapacity, MapEventPageList.MinCapacity);
        }

        [Test]
        public static void SerializeTest()
        {
            var target = new MapEventPageList(new []
            {
                new MapEventPage
                {
                    ShadowGraphicId = 2,
                },
            });
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }
    }
}