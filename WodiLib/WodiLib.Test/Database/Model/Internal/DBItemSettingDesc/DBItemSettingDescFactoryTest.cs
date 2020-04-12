using System;
using Commons;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Internal.DBItemSettingDesc
{
    [TestFixture]
    public class DBItemSettingDescFactoryTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        private static readonly object[] CreateTestCaseSource =
        {
            new object[] {null, -1, true},
            new object[] {null, 0, true},
            new object[] {null, 1, true},
            new object[] {null, 2, true},
            new object[] {null, 99, true},
            new object[] {DBItemSpecialSettingType.Normal, -1, false},
            new object[] {DBItemSpecialSettingType.Normal, 0, false},
            new object[] {DBItemSpecialSettingType.Normal, 1, false},
            new object[] {DBItemSpecialSettingType.Normal, 2, false},
            new object[] {DBItemSpecialSettingType.Normal, 99, false},
            new object[] {DBItemSpecialSettingType.LoadFile, -1, false},
            new object[] {DBItemSpecialSettingType.LoadFile, 0, true},
            new object[] {DBItemSpecialSettingType.LoadFile, 1, false},
            new object[] {DBItemSpecialSettingType.LoadFile, 2, true},
            new object[] {DBItemSpecialSettingType.LoadFile, 99, true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, -1, false},
            new object[] {DBItemSpecialSettingType.ReferDatabase, 0, false},
            new object[] {DBItemSpecialSettingType.ReferDatabase, 1, false},
            new object[] {DBItemSpecialSettingType.ReferDatabase, 2, false},
            new object[] {DBItemSpecialSettingType.ReferDatabase, 99, false},
            new object[] {DBItemSpecialSettingType.Manual, -1, false},
            new object[] {DBItemSpecialSettingType.Manual, 0, false},
            new object[] {DBItemSpecialSettingType.Manual, 1, false},
            new object[] {DBItemSpecialSettingType.Manual, 2, false},
            new object[] {DBItemSpecialSettingType.Manual, 99, false},
        };

        [TestCaseSource(nameof(CreateTestCaseSource))]
        public static void CreateTest(DBItemSpecialSettingType type, int caseListLength, bool isError)
        {
            var caseList = MakeDBValueCaseList(caseListLength);

            IDBItemSettingDesc result = null;

            var errorOccured = false;
            try
            {
                result = DBItemSettingDescFactory.Create(type, caseList);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // 設定種別に応じた型が取得できていること
            if (type == DBItemSpecialSettingType.Normal)
            {
                Assert.IsTrue(result is DBItemSettingDescNormal);
            }
            else if (type == DBItemSpecialSettingType.LoadFile)
            {
                Assert.IsTrue(result is DBItemSettingDescLoadFile);
            }
            else if (type == DBItemSpecialSettingType.ReferDatabase)
            {
                Assert.IsTrue(result is DBItemSettingDescDatabase);
            }
            else if (type == DBItemSpecialSettingType.Manual)
            {
                Assert.IsTrue(result is DBItemSettingDescManual);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public static void CreateNormalTest()
        {
            var errorOccured = false;
            try
            {
                var _ = DBItemSettingDescFactory.CreateNormal();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);
        }

        [TestCase(-1, false)]
        [TestCase(0, true)]
        [TestCase(1, false)]
        [TestCase(2, true)]
        public static void CreateLoadFileTest(int caseListLength, bool isError)
        {
            var caseList = MakeDBValueCaseList(caseListLength);

            IDBItemSettingDesc result = null;

            var errorOccured = false;
            try
            {
                result = DBItemSettingDescFactory.CreateLoadFile(caseList);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // 選択肢の要素数が1であること
            Assert.AreEqual(result.GetAllSpecialCase().Count, 1);
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(2)]
        public static void CreateReferDatabaseTest(int caseListLength)
        {
            var caseList = MakeDBValueCaseList(caseListLength);

            IDBItemSettingDesc result = null;

            var errorOccured = false;
            try
            {
                result = DBItemSettingDescFactory.CreateReferDatabase(caseList);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 選択肢の数が0であること
            Assert.AreEqual(result.GetAllSpecialCase().Count, 0);
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public static void CreateManualTest(int caseListLength)
        {
            var caseList = MakeDBValueCaseList(caseListLength);

            IDBItemSettingDesc result = null;

            var errorOccured = false;
            try
            {
                result = DBItemSettingDescFactory.CreateManual(caseList);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 選択肢が意図した数であること
            var answerResultLength = caseListLength != -1
                ? caseListLength
                : 0;
            Assert.AreEqual(result.GetAllSpecialCase().Count, answerResultLength);
        }


        private static DatabaseValueCaseList MakeDBValueCaseList(int length)
        {
            if (length == -1) return null;

            var result = new DatabaseValueCaseList();

            for (var i = 0; i < length; i++)
            {
                result.Add((i, i.ToString()));
            }

            return result;
        }
    }
}