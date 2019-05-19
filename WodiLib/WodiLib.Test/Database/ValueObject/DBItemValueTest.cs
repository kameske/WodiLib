using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database
{
    [TestFixture]
    public class DBItemValueTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        private static readonly object[] TypeTestCaseSource =
        {
            new object[] {DBItemType.Int},
            new object[] {DBItemType.String},
        };

        [TestCaseSource(nameof(TypeTestCaseSource))]
        public static void TypeTest(DBItemType type)
        {
            var instance = MakeInstance(type);

            DBItemType resultType = null;

            var errorOccured = false;
            try
            {
                resultType = instance.Type;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 項目種別が正しく取得できること
            Assert.AreEqual(resultType, type);
        }

        private static readonly object[] StringValueTestCaseSource =
        {
            new object[] {DBItemType.Int, true},
            new object[] {DBItemType.String, false},
        };

        [TestCaseSource(nameof(StringValueTestCaseSource))]
        public static void StringValueTest(DBItemType type, bool isError)
        {
            var instance = MakeInstance(type);

            var errorOccured = false;
            try
            {
                var _ = instance.StringValue;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [Test]
        public static void ConstructorATest()
        {
            var valueInt = (DBValueInt) 0;

            var errorOccured = false;
            try
            {
                var _ = new DBItemValue(valueInt);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);
        }

        private static readonly object[] ConstructorBTestCaseSource =
        {
            new object[] {(DBValueString) "", false},
            new object[] {null, true},
        };

        [TestCaseSource(nameof(ConstructorBTestCaseSource))]
        public static void ConstructorBTest(DBValueString valueString, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new DBItemValue(valueString);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        private static readonly object[] ToStringTestCaseSource =
        {
            new object[] {new DBItemValue(10)},
            new object[] {new DBItemValue("abc")},
        };

        [TestCaseSource(nameof(ToStringTestCaseSource))]
        public static void ToStringTest(DBItemValue value)
        {
            var _ = value.ToString();

            // エラーが発生しないこと
        }

        private static readonly object[] ToDBValueIntTestCaseSource =
        {
            new object[] {new DBItemValue(10), false},
            new object[] {new DBItemValue("abc"), true},
        };

        [TestCaseSource(nameof(ToDBValueIntTestCaseSource))]
        public static void ToDBValueIntTest(DBItemValue value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = value.ToDBValueInt();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        private static readonly object[] ToDBValueStringTestCaseSource =
        {
            new object[] {new DBItemValue(10), true},
            new object[] {new DBItemValue("abc"), false},
        };

        [TestCaseSource(nameof(ToDBValueStringTestCaseSource))]
        public static void ToDBValueStringTest(DBItemValue value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = value.ToDBValueString();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }


        private static readonly object[] EqualsTestCaseSource =
        {
            new object[] {new DBItemValue(10), new DBItemValue(10), true},
            new object[] {new DBItemValue(10), new DBItemValue("abc"), false},
            new object[] {new DBItemValue(10), null, false},
            new object[] {new DBItemValue("abc"), new DBItemValue(10), false},
            new object[] {new DBItemValue("abc"), new DBItemValue("abc"), true},
            new object[] {new DBItemValue("abc"), null, false},
        };

        [TestCaseSource(nameof(EqualsTestCaseSource))]
        public static void EqualsTest(DBItemValue value, DBItemValue other, bool answerIsEqual)
        {
            var result = false;

            var errorOccured = false;
            try
            {
                result = value.Equals(other);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが発生しないこと
            Assert.IsFalse(errorOccured);

            // 結果が意図した値と一致すること
            Assert.AreEqual(result, answerIsEqual);
        }

        private static readonly object[] CastFromDBValueIntTestCaseSource =
        {
            new object[] {(DBValueInt) 10},
        };

        [TestCaseSource(nameof(CastFromDBValueIntTestCaseSource))]
        public static void CastFromDBValueIntTest(DBValueInt value)
        {
            DBItemValue instance = null;

            var errorOccured = false;
            try
            {
                instance = value;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // キャストした結果が一致すること
            Assert.AreEqual((DBValueInt) instance, value);
        }


        private static readonly object[] CastFromDBValueStringTestCaseSource =
        {
            new object[] {(DBValueString) "abc"},
        };

        [TestCaseSource(nameof(CastFromDBValueStringTestCaseSource))]
        public static void CastFromDBValueStringTest(DBValueString value)
        {
            DBItemValue instance = null;

            var errorOccured = false;
            try
            {
                instance = value;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // キャストした結果が一致すること
            Assert.AreEqual((DBValueString) instance, value);
        }


        [TestCaseSource(nameof(EqualsTestCaseSource))]
        public static void OperatorEqualTest(DBItemValue left, DBItemValue right, bool isEqual)
        {
            Assert.AreEqual(left == right, isEqual);
        }

        [TestCaseSource(nameof(EqualsTestCaseSource))]
        public static void OperatorNotEqualTest(DBItemValue left, DBItemValue right, bool isEqual)
        {
            Assert.AreEqual(left != right, !isEqual);
        }


        private static DBItemValue MakeInstance(DBItemType type)
        {
            if (type == DBItemType.Int)
            {
                return new DBItemValue(0);
            }

            if (type == DBItemType.String)
            {
                return new DBItemValue("");
            }

            throw new ArgumentException();
        }
    }
}