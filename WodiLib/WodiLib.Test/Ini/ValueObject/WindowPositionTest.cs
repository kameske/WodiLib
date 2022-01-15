using System;
using Commons;
using NUnit.Framework;
using WodiLib.Ini;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Ini.ValueObject
{
    [TestFixture]
    public class WindowPositionTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [TestCase(-1, -1, true)]
        [TestCase(-1, 0, true)]
        [TestCase(-1, int.MaxValue, true)]
        [TestCase(0, -1, true)]
        [TestCase(0, 0, false)]
        [TestCase(0, int.MaxValue, false)]
        [TestCase(int.MaxValue, -1, true)]
        [TestCase(int.MaxValue, 0, false)]
        [TestCase(int.MaxValue, int.MaxValue, false)]
        public static void ConstructorTest(int x, int y, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new WindowPosition { X = x, Y = y };
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        private static readonly object[] EqualTestCaseSource =
        {
            new object[] { new WindowPosition { X = 0, Y = 0 }, new WindowPosition { X = 0, Y = 0 }, true },
            new object[] { new WindowPosition { X = 0, Y = 0 }, new WindowPosition { X = 0, Y = int.MaxValue }, false },
            new object[]
            {
                new WindowPosition { X = 0, Y = 0 }, new WindowPosition { X = int.MaxValue, Y = int.MaxValue }, false
            }
        };

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualTest(WindowPosition left, WindowPosition light, bool isEqual)
        {
            Assert.AreEqual(left == light, isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorNotEqualTest(WindowPosition left, WindowPosition right, bool isEqual)
        {
            Assert.AreEqual(left != right, !isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualsTest(WindowPosition left, WindowPosition right, bool isEqual)
        {
            Assert.AreEqual(left.Equals(right), isEqual);
        }
    }
}
