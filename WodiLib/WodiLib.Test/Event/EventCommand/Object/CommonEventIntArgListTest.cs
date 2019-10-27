using NUnit.Framework;
using WodiLib.Event.EventCommand;

namespace WodiLib.Test.Event.EventCommand
{
    [TestFixture]
    public class CommonEventIntArgListTest
    {
        private static readonly object[] TestCaseSource =
        {
            new object[] {0, 100},
            new object[] {1, 100},
            new object[] {2, 100},
            new object[] {3, 100}
        };

        [TestCaseSource(nameof(TestCaseSource))]
        public static void AccessorTest(int index, int value)
        {
            var instance = new CommonEventIntArgList {[index] = value};
            for (var i = 0; i < 4; i++) Assert.AreEqual(instance[i], i == index ? value : 0);
        }
    }
}