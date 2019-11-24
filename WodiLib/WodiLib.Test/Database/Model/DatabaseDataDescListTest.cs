using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database
{
    [TestFixture]
    public class DatabaseDataDescListTest
    {
        [Test]
        public static void GetMaxCapacityTest()
        {
            var instance = new DatabaseDataDescList();
            var maxCapacity = instance.GetMaxCapacity();

            // 取得した値が容量最大値と一致すること
            Assert.AreEqual(maxCapacity, DatabaseDataDescList.MaxCapacity);
        }

        [Test]
        public static void GetMinCapacityTest()
        {
            var instance = new DatabaseDataDescList();
            var maxCapacity = instance.GetMinCapacity();

            // 取得した値が容量最大値と一致すること
            Assert.AreEqual(maxCapacity, DatabaseDataDescList.MinCapacity);
        }

        [Test]
        public static void SerializeTest()
        {
            var target = new DatabaseDataDescList();
            target.AdjustLength(2);
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }
    }
}