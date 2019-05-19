using NUnit.Framework;
using WodiLib.Database;

namespace WodiLib.Test.Database
{
    [TestFixture]
    public class DatabaseItemDescListTest
    {
        [Test]
        public static void GetMaxCapacityTest()
        {
            var instance = new DatabaseItemDescList();
            var maxCapacity = instance.GetMaxCapacity();

            // 取得した値が容量最大値と一致すること
            Assert.AreEqual(maxCapacity, DatabaseItemDescList.MaxCapacity);
        }

        [Test]
        public static void GetMinCapacityTest()
        {
            var instance = new DatabaseItemDescList();
            var maxCapacity = instance.GetMinCapacity();

            // 取得した値が容量最大値と一致すること
            Assert.AreEqual(maxCapacity, DatabaseItemDescList.MinCapacity);
        }
    }
}