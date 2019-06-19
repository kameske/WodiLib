using System;
using NUnit.Framework;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Sys
{
    [TestFixture]
    public class TypeSafeEnumTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [Test]
        public static void IdTest()
        {
            var errorOccured = false;
            try
            {
                var _ = EnumTestClass.Item1.Id;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);
        }

        private static readonly object[] OperatorEqualTestCaseSource =
        {
            new object[] {EnumTestClass.Item1, EnumTestClass.Item1, true},
            new object[] {EnumTestClass.Item1, EnumTestClass.Item2, false},
            new object[] {EnumTestClass.Item1, null, false},
        };

        [TestCaseSource(nameof(OperatorEqualTestCaseSource))]
        public static void OperatorEqualTestA(EnumTestClass left, EnumTestClass right, bool answer)
        {
            var result = false;

            var errorOccured = false;
            try
            {
                result = left == right;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 結果が意図した値と一致すること
            Assert.AreEqual(result, answer);
        }

        [Test]
        public static void OperatorEqualTestB()
        {
            var result = false;

            var errorOccured = false;
            try
            {
                result = EnumTestClass.Item1 == null;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 比較結果がfalseであること
            Assert.IsFalse(result);
        }

        [TestCaseSource(nameof(OperatorEqualTestCaseSource))]
        public static void OperatorNotEqualTestA(EnumTestClass left, EnumTestClass right, bool answer)
        {
            var result = false;

            var errorOccured = false;
            try
            {
                result = left != right;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 結果が意図した値と一致すること
            Assert.AreEqual(result, !answer);
        }

        [Test]
        public static void OperatorNotEqualTestB()
        {
            var result = false;

            var errorOccured = false;
            try
            {
                result = EnumTestClass.Item1 != null;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 比較結果がtrueであること
            Assert.IsTrue(result);
        }

        [TestCaseSource(nameof(OperatorEqualTestCaseSource))]
        public static void EqualsTestA(EnumTestClass left, EnumTestClass right, bool answer)
        {
            var result = false;

            var errorOccured = false;
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

            // 結果が意図した値と一致すること
            Assert.AreEqual(result, answer);
        }

        private static readonly object[] EqualsTestBTestCaseSource =
        {
            new object[] {EnumTestClass.Item1, EnumTestClass.Item1, true},
            new object[] {EnumTestClass.Item1, EnumTestClass.Item2, false},
            new object[] {EnumTestClass.Item1, 999, false},
            new object[] {EnumTestClass.Item1, EnumTestClass2.Item1, false},
            new object[] {EnumTestClass.Item1, EnumTestClass2.Item2, false},
            new object[] {EnumTestClass.Item1, null, false},
        };

        [TestCaseSource(nameof(EqualsTestBTestCaseSource))]
        public static void EqualsTestB(EnumTestClass left, object right, bool answer)
        {
            var result = false;

            var errorOccured = false;
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

            // 結果が意図した値と一致すること
            Assert.AreEqual(result, answer);
        }

        public class EnumTestClass : TypeSafeEnum<EnumTestClass>
        {
            public static readonly EnumTestClass Item1;
            public static readonly EnumTestClass Item2;

            private EnumTestClass(string id) : base(id)
            {
            }

            static EnumTestClass()
            {
                Item1 = new EnumTestClass(nameof(Item1));
                Item2 = new EnumTestClass(nameof(Item2));
            }
        }

        public class EnumTestClass2 : TypeSafeEnum<EnumTestClass2>
        {
            public static readonly EnumTestClass2 Item1;
            public static readonly EnumTestClass2 Item2;

            private EnumTestClass2(string id) : base(id)
            {
            }

            static EnumTestClass2()
            {
                Item1 = new EnumTestClass2(nameof(Item1));
                Item2 = new EnumTestClass2(nameof(Item2));
            }
        }
    }
}