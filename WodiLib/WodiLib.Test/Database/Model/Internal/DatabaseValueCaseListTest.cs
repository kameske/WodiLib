using System;
using System.Collections.Generic;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Internal
{
    [TestFixture]
    public class DatabaseValueCaseListTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [Test]
        public static void GetMaxCapacityTest()
        {
            var instance = new DatabaseValueCaseList();
            var maxCapacity = instance.GetMaxCapacity();

            // 取得した値が容量最大値と一致すること
            Assert.AreEqual(maxCapacity, DatabaseValueCaseList.MaxCapacity);
        }

        [Test]
        public static void GetMinCapacityTest()
        {
            var instance = new DatabaseValueCaseList();
            var maxCapacity = instance.GetMinCapacity();

            // 取得した値が容量最大値と一致すること
            Assert.AreEqual(maxCapacity, DatabaseValueCaseList.MinCapacity);
        }

        [TestCase(0, 3, "0")]
        [TestCase(-1, 3, null)]
        public static void GetDescriptionForCaseNumberTest(int caseNumber, int initLength, string resultDescription)
        {
            var initList = MakeInitList(initLength, false);
            var instance = new DatabaseValueCaseList(initList);

            var result = "";

            var errorOccured = false;
            try
            {
                result = instance.GetDescriptionForCaseNumber(caseNumber);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 取得した値が意図した値であること
            Assert.AreEqual(result, resultDescription);
        }

        [TestCase(0, 3, "0")]
        [TestCase(-1, 3, null)]
        public static void GetForCaseNumberTest(int caseNumber, int initLength, string resultDescription)
        {
            var initList = MakeInitList(initLength, false);
            var instance = new DatabaseValueCaseList(initList);

            DatabaseValueCase result = null;

            var errorOccured = false;
            try
            {
                result = instance.GetForCaseNumber(caseNumber);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 取得した値が意図した値であること
            var resultCase = resultDescription == null
                ? null
                : new DatabaseValueCase(caseNumber, resultDescription);
            Assert.AreEqual(result, resultCase);
        }

        private static IReadOnlyList<DatabaseValueCase> MakeInitList(int length, bool hasNullItem)
        {
            if (length == -1) return null;

            var result = new List<DatabaseValueCase>();
            for (var i = 0; i < length; i++)
            {
                result.Add(hasNullItem && i == length / 2
                    ? null
                    : new DatabaseValueCase(i, i.ToString()));
            }

            return result;
        }
    }
}