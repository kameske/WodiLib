using System;
using Commons;
using NUnit.Framework;
using WodiLib.Sys;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Sys
{
    [TestFixture]
    public class SingleIntValueRecordTest
    {
        private static Logger logger;
        private static Logger loggerLocked;

        private static readonly object ConstructorTestLockObject = "";

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
            lock (ConstructorTestLockObject)
            {
                loggerLocked = Logger.GetInstance();
            }
        }

        [TestCase(30, 0, 29, true)]
        [TestCase(30, 0, 30, false)]
        [TestCase(0, 0, 20, false)]
        [TestCase(0, 1, 20, true)]
        [TestCase(-30, -30, 5, false)]
        [TestCase(-30, -29, 5, true)]
        [TestCase(0, -20, -1, true)]
        [TestCase(0, -20, 0, false)]
        [TestCase(8, 8, 8, false)]
        public static void ConstructorTest(int value, int min, int max, bool isError)
        {
            lock (ConstructorTestLockObject)
            {
                ValidationOption.MaxValue = max;
                ValidationOption.MinValue = min;

                var errorOccured = false;

                try
                {
                    var _ = new ValidationTestRecord(value);
                }
                catch (Exception ex)
                {
                    loggerLocked.Exception(ex);
                    errorOccured = true;
                }

                // エラーフラグが一致すること
                Assert.AreEqual(errorOccured, isError);
            }
        }

        [TestCase(20, -20)]
        [TestCase(20, 0)]
        [TestCase(20, 20)]
        [TestCase(20, 21)]
        public static void GetHashCodeTest(int leftSrc, int rightSrc)
        {
            var left = new MethodTestRecord(leftSrc);
            var right = new MethodTestRecord(rightSrc);

            var leftHashCode = 0;
            var rightHashCode = 0;

            var errorOccured = false;

            try
            {
                leftHashCode = left.GetHashCode();
                rightHashCode = right.GetHashCode();
            }
            catch (Exception ex)
            {
                loggerLocked.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // left と right が等しいとき、HashCode が一致すること
            if (left == right)
            {
                Assert.IsTrue(leftHashCode == rightHashCode);
            }
        }

        [TestCase(-127)]
        [TestCase(0)]
        [TestCase(33)]
        public static void ToStringTest(int src)
        {
            var instance = new MethodTestRecord(src);
            var instanceStr = instance.ToString();
            Assert.IsTrue(instanceStr.Equals(src.ToString()));
        }

        [TestCase(-127)]
        [TestCase(0)]
        [TestCase(33)]
        public static void ToIntTest(int src)
        {
            var instance = new MethodTestRecord(src);
            var instanceStr = instance.ToInt();
            Assert.IsTrue(instanceStr == src);
        }

        [TestCase(0, 0)]
        [TestCase(0, 21)]
        [TestCase(32, 2)]
        [TestCase(-5, 0)]
        [TestCase(-8, 8)]
        [TestCase(2, -6)]
        [TestCase(5, null)]
        public static void CompareToTest(int leftSrc, int? rightSrc)
        {
            var left = new MethodTestRecord(leftSrc);
            var right = rightSrc is null ? null : new MethodTestRecord(rightSrc.Value);

            var errorOccured = false;
            var result = 0;
            try
            {
                result = left.CompareTo(right);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            var answer = leftSrc.CompareTo(rightSrc);
            logger.Info($"result: {result}, answer: {answer}");
            Assert.IsTrue(result == answer);
        }

        private static readonly object[] EqualsOtherTestCaseSource =
        {
            new object[] {new MethodTestRecord(13), new MethodTestRecord(-13), false},
            new object[] {new MethodTestRecord(13), new MethodTestRecord(12), false},
            new object[] {new MethodTestRecord(13), new MethodTestRecord(13), true},
            new object[] {new MethodTestRecord(13), new MethodTestRecord(14), false},
            new object[] {new MethodTestRecord(13), null, false},
            new object[] {new MethodTestRecord(-13), new MethodTestRecord(-14), false},
            new object[] {new MethodTestRecord(-13), new MethodTestRecord(-13), true},
            new object[] {new MethodTestRecord(-13), new MethodTestRecord(-12), false},
            new object[] {new MethodTestRecord(-13), new MethodTestRecord(13), false},
            new object[] {new MethodTestRecord(0), new MethodTestRecord(0), true},
            new object[] {new MethodTestRecord(0), null, false},
        };

        [TestCaseSource(nameof(EqualsOtherTestCaseSource))]
        public static void EqualsOtherTest(MethodTestRecord left, MethodTestRecord right, bool answer)
        {
            var errorOccured = false;
            var result = false;
            try
            {
                result = left.Equals(right);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 結果が意図した値であること
            Assert.AreEqual(result, answer);
        }

        private static readonly object[] EqualsObjectTestCaseSource =
        {
            new object[] {new MethodTestRecord(13), new MethodTestRecord(-13), false},
            new object[] {new MethodTestRecord(13), new MethodTestRecord(12), false},
            new object[] {new MethodTestRecord(13), new MethodTestRecord(13), true},
            new object[] {new MethodTestRecord(13), new MethodTestRecord(14), false},
            new object[] {new MethodTestRecord(13), new MethodTestRecord(0), false},
            new object[] {new MethodTestRecord(13), null, false},
            new object[] {new MethodTestRecord(13), 13, false},
            new object[] {new MethodTestRecord(13), "13", false},
            new object[] {new MethodTestRecord(13), new EqualsTestRecord(13), false},
        };

        [TestCaseSource(nameof(EqualsObjectTestCaseSource))]
        public static void EqualsObjectTest(MethodTestRecord left, object right, bool answer)
        {
            var errorOccured = false;
            var result = false;
            try
            {
                result = left.Equals(right);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 結果が意図した値であること
            Assert.AreEqual(result, answer);
        }

        /// <summary>
        /// Initializeの検証テスト用
        /// </summary>
        /// <remarks>
        /// 検証オプションは static である <see cref="ValidationOption"/> から取得。
        /// </remarks>
        public record ValidationTestRecord : SingleIntValueRecord<ValidationTestRecord>
        {
            protected override int _MaxValue => ValidationOption.MaxValue;
            protected override int _MinValue => ValidationOption.MinValue;

            public ValidationTestRecord(int value) : base(value)
            {
            }
        }

        /// <summary>
        /// コンストラクタテスト用検証オプション
        /// </summary>
        private static class ValidationOption
        {
            public static int MaxValue { get; set; }
            public static int MinValue { get; set; }
        }

        /// <summary>
        /// メソッドテスト用クラス
        /// </summary>
        public record MethodTestRecord : SingleIntValueRecord<MethodTestRecord>
        {
            protected override int _MaxValue => int.MaxValue;
            protected override int _MinValue => int.MinValue;

            public MethodTestRecord(int value) : base(value)
            {
            }
        }

        /// <summary>
        /// Equals メソッドテスト用比較対象クラス
        /// </summary>
        public record EqualsTestRecord : SingleIntValueRecord<EqualsTestRecord>
        {
            protected override int _MaxValue => int.MaxValue;
            protected override int _MinValue => int.MinValue;

            public EqualsTestRecord(int value) : base(value)
            {
            }
        }
    }
}
