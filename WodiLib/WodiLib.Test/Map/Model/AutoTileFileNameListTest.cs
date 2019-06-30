using NUnit.Framework;
using WodiLib.Map;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class AutoTileFileNameListTest
    {
        [Test]
        public static void GetMaxCapacityTest()
        {
            var instance = new AutoTileFileNameList();
            var maxCapacity = instance.GetMaxCapacity();

            // 取得した値が容量最大値と一致すること
            Assert.AreEqual(maxCapacity, AutoTileFileNameList.MaxCapacity);
        }

        [Test]
        public static void GetMinCapacityTest()
        {
            var instance = new AutoTileFileNameList();
            var maxCapacity = instance.GetMinCapacity();

            // 取得した値が容量最大値と一致すること
            Assert.AreEqual(maxCapacity, AutoTileFileNameList.MinCapacity);
        }
    }
}