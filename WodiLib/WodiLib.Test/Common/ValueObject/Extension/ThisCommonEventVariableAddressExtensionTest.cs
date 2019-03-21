using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Common;

namespace WodiLib.Test.Common
{
    [TestFixture]
    public class ThisCommonEventVariableAddressExtensionTest
    {
        [TestCase(1600020, 20)]
        [TestCase(1600003, 3)]
        [TestCase(1600099, 99)]
        public static void GetIndexTest(int variableAddress, int answer)
        {
            var varAddress = new ThisCommonEventVariableAddress(variableAddress);
            var result = varAddress.GetIndex();

            // 取得した値が結果と一致すること
            Assert.AreEqual(result, answer);
        }
    }
}