using NUnit.Framework;
using WodiLib.Map;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class TilePathSettingListTest
    {
        [Test]
        public static void GetMaxCapacityTest()
        {
            var instance = new TilePathSettingList();
            var maxCapacity = instance.GetMaxCapacity();

            // 取得した値が容量最大値と一致すること
            Assert.AreEqual(maxCapacity, TilePathSettingList.MaxCapacity);
        }

        [Test]
        public static void GetMinCapacityTest()
        {
            var instance = new TilePathSettingList();
            var maxCapacity = instance.GetMinCapacity();

            // 取得した値が容量最大値と一致すること
            Assert.AreEqual(maxCapacity, TilePathSettingList.MinCapacity);
        }
    }
}
