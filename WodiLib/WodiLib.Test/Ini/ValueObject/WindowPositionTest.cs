using System;
using NUnit.Framework;
using WodiLib.Ini;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Ini.ValueObject
{
    [TestFixture]
    public class WindowPositionTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
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
                var _ = new WindowPosition(x, y);
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
        public static void CastTupleToWindowPositionTest(int x, int y, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = (WindowPosition) (x, y);
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
        public static void CastWindowPositionToTupleTest(int x, int y)
        {
            var castValue = (0, 0);

            var instance = new WindowPosition(x, y);

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
        public static void CastValueTupleToWindowPositionTest(int x, int y, bool isError)
        {
            var errorOccured = false;
            var valueTuple = new ValueTuple<int, int>(x, y);
            try
            {
                var _ = (WindowPosition) valueTuple;
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
        public static void CastWindowPositionToValueTupleTest(int x, int y)
        {
            var castValue = new ValueTuple<int, int>(0, 0);

            var instance = new WindowPosition(x, y);

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
            new object[] {(WindowPosition)(0, 0), (WindowPosition)(0, 0), true},
            new object[] {(WindowPosition)(0, 0), (WindowPosition)(0, int.MaxValue), false},
            new object[] {(WindowPosition)(0, 0), (WindowPosition)(int.MaxValue, int.MaxValue), false},
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
