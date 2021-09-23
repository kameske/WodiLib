using NUnit.Framework;
using WodiLib.Event.EventCommand;

namespace WodiLib.Test.Event.EventCommand
{
    [TestFixture]
    public class ChoiceForkFlagsTest
    {
        private static readonly object[] TestCaseSource =
        {
            new object[] { new[] { false, false, false }, (byte)0 },
            new object[] { new[] { false, false, true }, (byte)4 },
            new object[] { new[] { false, true, false }, (byte)2 },
            new object[] { new[] { false, true, true }, (byte)6 },
            new object[] { new[] { true, false, false }, (byte)1 },
            new object[] { new[] { true, false, true }, (byte)5 },
            new object[] { new[] { true, true, false }, (byte)3 },
            new object[] { new[] { true, true, true }, (byte)7 }
        };

        [TestCaseSource(nameof(TestCaseSource))]
        public static void CreateTest(bool[] flags, byte flagByte)
        {
            var instance = new ChoiceForkFlags(flagByte);
            Assert.AreEqual(instance.IsForkLeftKey, flags[0]);
            Assert.AreEqual(instance.IsForkRightKey, flags[1]);
            Assert.AreEqual(instance.IsStopForce, flags[2]);
        }

        [TestCaseSource(nameof(TestCaseSource))]
        public static void ToByteTest(bool[] flags, byte flagByte)
        {
            var instance = new ChoiceForkFlags
            {
                IsForkLeftKey = flags[0],
                IsForkRightKey = flags[1],
                IsStopForce = flags[2]
            };
            Assert.AreEqual(instance.ToByte(), flagByte);
        }
    }
}
