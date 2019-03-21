using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Common;

namespace WodiLib.Test.Common
{
    [TestFixture]
    public class CommonEventVariableAddressExtensionTest
    {
        [TestCase(15000020, 20)]
        [TestCase(15004103, 3)]
        [TestCase(15012899, 99)]
        public static void GetIndexTest(int variableAddress, int answer)
        {
            var varAddress = new CommonEventVariableAddress(variableAddress);
            var result = varAddress.GetIndex();

            // 取得した値が結果と一致すること
            Assert.AreEqual(result, answer);
        }

        [TestCase(15000020, 0)]
        [TestCase(15004103, 41)]
        [TestCase(15012899, 128)]
        public static void GetCommonEventId(int variableAddress, int answer)
        {
            var varAddress = new CommonEventVariableAddress(variableAddress);
            var result = varAddress.GetCommonEventId();

            // 取得した値が結果と一致すること
            Assert.AreEqual(result, (CommonEventId)answer);
        }
    }
}