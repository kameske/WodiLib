using System;
using System.Linq;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database
{
    [TestFixture]
    public class DBTypeSettingTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        private static readonly object[] TypeNameTestCaseSource =
        {
            new object[] {(TypeName) "", false},
            new object[] {null, true},
        };

        [TestCaseSource(nameof(TypeNameTestCaseSource))]
        public static void TypeNameTest(TypeName typeName, bool isError)
        {
            var instance = new DBTypeSetting();

            var errorOccured = false;
            try
            {
                instance.TypeName = typeName;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            var setValue = instance.TypeName;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.Equals(typeName));
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void DataNameListTest(bool isSetNull, bool isError)
        {
            var instance = new DBTypeSetting();

            var dataNameList = isSetNull ? null : new DataNameList();

            var errorOccured = false;
            try
            {
                instance.DataNameList = dataNameList;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            var setValue = instance.DataNameList;

            Assert.NotNull(setValue);
            Assert.NotNull(dataNameList);

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.SequenceEqual(dataNameList));
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void ItemSettingListTest(bool isSetNull, bool isError)
        {
            var instance = new DBTypeSetting();

            var itemSettingList = isSetNull ? null : new DBItemSettingList();

            var errorOccured = false;
            try
            {
                instance.ItemSettingList = itemSettingList;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            var setValue = instance.ItemSettingList;

            Assert.NotNull(setValue);
            Assert.NotNull(itemSettingList);

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.SequenceEqual(itemSettingList));
        }

        private static readonly object[] MemoTestCaseSource =
        {
            new object[] {(DatabaseMemo) "", false},
            new object[] {null, true},
        };

        [TestCaseSource(nameof(MemoTestCaseSource))]
        public static void MemoTest(DatabaseMemo memo, bool isError)
        {
            var instance = new DBTypeSetting();

            var errorOccured = false;
            try
            {
                instance.Memo = memo;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            var setValue = instance.Memo;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.Equals(memo));
        }
    }
}