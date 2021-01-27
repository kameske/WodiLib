using System;
using Commons;
using NUnit.Framework;
using WodiLib.Map;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class PositionTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [TestCase(-1, -1, true)]
        [TestCase(0, -1, true)]
        [TestCase(9999, -1, true)]
        [TestCase(10000, -1, true)]
        [TestCase(-1, 0, true)]
        [TestCase(0, 0, false)]
        [TestCase(9999, 0, false)]
        [TestCase(10000, 0, true)]
        [TestCase(-1, 9999, true)]
        [TestCase(0, 9999, false)]
        [TestCase(9999, 9999, false)]
        [TestCase(10000, 9999, true)]
        [TestCase(-1, 10000, true)]
        [TestCase(0, 10000, true)]
        [TestCase(9999, 10000, true)]
        [TestCase(10000, 10000, true)]
        public static void ConstructorTest(int x, int y, bool isError)
        {
            var instance = default(Position);

            var errorOccured = false;
            try
            {
                instance = new Position(x, y);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // セットした値と一致すること
            Assert.AreEqual(instance.X, x);
            Assert.AreEqual(instance.Y, y);
        }

        private static readonly object[] EqualsTestCaseSource =
        {
            new object[] {new Position(3, 5), new Position(3, 5), true},
            new object[] {new Position(3, 5), new Position(5, 5), false},
            new object[] {new Position(3, 5), new Position(3, 3), false},
            new object[] {new Position(3, 5), new Position(5, 3), false},
        };

        [TestCaseSource(nameof(EqualsTestCaseSource))]
        public static void EqualsTest(Position left, Position right, bool answer)
        {
            Assert.AreEqual(left.Equals(right), answer);
        }
    }
}
