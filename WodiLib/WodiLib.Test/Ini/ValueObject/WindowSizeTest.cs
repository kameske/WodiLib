using System;
using Commons;
using NUnit.Framework;
using WodiLib.Ini;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Ini.ValueObject
{
    [TestFixture]
    public class WindowSizeTest
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
                var _ = new WindowSize(x, y);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
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
        public static void CastTupleToWindowSizeTest(int x, int y, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = (WindowSize) (x, y);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(0, 0)]
        [TestCase(int.MaxValue, int.MaxValue)]
        public static void CastWindowSizeToTupleTest(int x, int y)
        {
            var castValue = (0, 0);

            var instance = new WindowSize(x, y);

            var errorOccured = false;
            try
            {
                castValue =  instance;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 元の値と一致すること
            Assert.AreEqual(castValue, (x, y));
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
        public static void CastValueTupleToWindowSizeTest(int x, int y, bool isError)
        {
            var errorOccured = false;
            var valueTuple = new ValueTuple<int, int>(x, y);
            try
            {
                var _ = (WindowSize) valueTuple;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(0, 0)]
        [TestCase(int.MaxValue, int.MaxValue)]
        public static void CastWindowSizeToValueTupleTest(int x, int y)
        {
            var castValue = new ValueTuple<int, int>(0, 0);

            var instance = new WindowSize(x, y);

            var errorOccured = false;
            try
            {
                castValue =  instance;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            var argValue = new ValueTuple<int, int>(x, y);

            // 元の値と一致すること
            Assert.IsTrue(castValue.Equals(argValue));
        }

        private static readonly object[] EqualTestCaseSource =
        {
            new object[] {(WindowSize)(0, 0), (WindowSize)(0, 0), true},
            new object[] {(WindowSize)(0, 0), (WindowSize)(0, int.MaxValue), false},
            new object[] {(WindowSize)(0, 0), (WindowSize)(int.MaxValue, int.MaxValue), false},
        };

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualTest(WindowSize left, WindowSize light, bool isEqual)
        {
            Assert.AreEqual(left == light, isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorNotEqualTest(WindowSize left, WindowSize right, bool isEqual)
        {
            Assert.AreEqual(left != right, !isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualsTest(WindowSize left, WindowSize right, bool isEqual)
        {
            Assert.AreEqual(left.Equals(right), isEqual);
        }

        [Test]
        public static void SerializeTest()
        {
            var target = (WindowSize) (640, 480);
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }
    }
}
