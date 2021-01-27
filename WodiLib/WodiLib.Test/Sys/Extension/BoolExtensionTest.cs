using NUnit.Framework;
using WodiLib.Sys;

namespace WodiLib.Test.Sys
{
    [TestFixture]
    public class BoolExtensionTest
    {
        [TestCase(true, 1)]
        [TestCase(false, 0)]
        public static void ToIntTest(bool target, int answer)
        {
            var result = target.ToInt();
            Assert.AreEqual(result, answer);
        }

        [TestCase(true, "1")]
        [TestCase(false, "0")]
        public static void ToIntString(bool target, string answer)
        {
            var result = target.ToIntString();
            Assert.AreEqual(result, answer);
        }
    }
}
