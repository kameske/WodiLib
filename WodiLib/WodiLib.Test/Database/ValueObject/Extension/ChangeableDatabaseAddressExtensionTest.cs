using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Extension
{
    [TestFixture]
    public class ChangeableDatabaseAddressExtensionTest
    {
        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
        }

        [TestCase(1100000000, 0)]
        [TestCase(1135002410, 35)]
        [TestCase(1199999999, 99)]
        public static void GetTypeIdTest(int variableAddress, int answer)
        {
            var varAddress = (ChangeableDatabaseAddress) variableAddress;
            var answerTypeId = (TypeId) answer;

            Assert.AreEqual(varAddress.GetTypeId(), answerTypeId);
        }

        [TestCase(1100000000, 0)]
        [TestCase(1135002410, 24)]
        [TestCase(1199999999, 9999)]
        public static void GetDataIdTest(int variableAddress, int answer)
        {
            var varAddress = (ChangeableDatabaseAddress) variableAddress;
            var answerDataId = (DataId) answer;

            Assert.AreEqual(varAddress.GetDataId(), answerDataId);
        }

        [TestCase(1100000000, 0)]
        [TestCase(1135002410, 10)]
        [TestCase(1199999999, 99)]
        public static void GetItemIdTest(int variableAddress, int answer)
        {
            var varAddress = (ChangeableDatabaseAddress) variableAddress;
            var answerItemId = (ItemId) answer;

            Assert.AreEqual(varAddress.GetItemId(), answerItemId);
        }
    }
}