using System;
using System.Collections.Generic;
using NUnit.Framework;
using WodiLib.Common;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Common.Internal
{
    [TestFixture]
    public class CommonEventSpecialArgCaseListTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [TestCase(1)]
        [TestCase(10)]
        public static void ConstructorTest(int initLength)
        {
            var instance = new CommonEventSpecialArgCaseList(MakeSpecialArgCaseArray(initLength));

            // 要素数が初期化した数と一致すること
            Assert.AreEqual(instance.Count, initLength);
        }

        [TestCase(1, -1, true)]
        [TestCase(1, 0, false)]
        [TestCase(1, 1, true)]
        [TestCase(4, -1, true)]
        [TestCase(4, 0, false)]
        [TestCase(4, 3, false)]
        [TestCase(4, 4, true)]
        public static void GetDescriptionForCaseNumberTest(int initLength, int caseNumber, bool isNull)
        {
            var instance = new CommonEventSpecialArgCaseList(MakeSpecialArgCaseArray(initLength));

            var errorOccured = false;
            try
            {
                instance.GetDescriptionForCaseNumber(caseNumber);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.AreEqual(errorOccured, false);

            // 取得結果が意図した値であること
            var description = instance.GetDescriptionForCaseNumber(caseNumber);
            Assert.AreEqual(description == null, isNull);
        }

        [TestCase(1, -1, true)]
        [TestCase(1, 0, false)]
        [TestCase(1, 1, true)]
        [TestCase(4, -1, true)]
        [TestCase(4, 0, false)]
        [TestCase(4, 3, false)]
        [TestCase(4, 4, true)]
        public static void GetForCaseNumberTest(int initLength, int caseNumber, bool isNull)
        {
            var instance = new CommonEventSpecialArgCaseList(MakeSpecialArgCaseArray(initLength));

            var errorOccured = false;
            try
            {
                instance.GetForCaseNumber(caseNumber);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.AreEqual(errorOccured, false);

            // 取得結果が意図した値であること
            var argCase = instance.GetForCaseNumber(caseNumber);
            Assert.AreEqual(argCase == null, isNull);
        }

        [Test]
        public static void GetAllCaseTest()
        {
            const int initLength = 10;
            var instance = new CommonEventSpecialArgCaseList(MakeSpecialArgCaseArray(initLength));

            var errorOccured = false;
            try
            {
                instance.GetAllCase();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 取得件数がセットした件数と一致すること
            var caseList = instance.GetAllCase();
            Assert.AreEqual(caseList.Count, initLength);
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void AddTest(bool isNull, bool isError)
        {
            var instance = new CommonEventSpecialArgCaseList(MakeSpecialArgCaseArray(1));

            var errorOccured = false;
            try
            {
                var argCase = isNull
                    ? null
                    : new CommonEventSpecialArgCase(0, "");
                instance.Add(argCase);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(-1, true)]
        [TestCase(0, false)]
        [TestCase(1, false)]
        [TestCase(999, false)]
        public static void AddRangeTest(int addLength, bool isError)
        {
            var instance = new CommonEventSpecialArgCaseList(MakeSpecialArgCaseArray(1));

            var errorOccured = false;
            try
            {
                var addArgCases = MakeSpecialArgCaseArray(addLength);
                instance.AddRange(addArgCases);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(1, -1, false, true)]
        [TestCase(1, -1, true, true)]
        [TestCase(1, 0, false, false)]
        [TestCase(1, 0, true, true)]
        [TestCase(1, 1, false, false)]
        [TestCase(1, 1, true, true)]
        [TestCase(1, 2, false, true)]
        [TestCase(1, 2, true, true)]
        [TestCase(4, -1, false, true)]
        [TestCase(4, -1, true, true)]
        [TestCase(4, 0, false, false)]
        [TestCase(4, 0, true, true)]
        [TestCase(4, 4, false, false)]
        [TestCase(4, 4, true, true)]
        [TestCase(4, 5, false, true)]
        [TestCase(4, 5, true, true)]
        public static void InsertTest(int initLength, int index, bool isNull, bool isError)
        {
            var instance = new CommonEventSpecialArgCaseList(MakeSpecialArgCaseArray(initLength));

            var errorOccured = false;
            try
            {
                var item = isNull
                    ? null
                    : new CommonEventSpecialArgCase(999, "");
                instance.Insert(index, item);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(1, -1, -1, true)]
        [TestCase(1, -1, 0, true)]
        [TestCase(1, -1, 1, true)]
        [TestCase(1, 0, -1, true)]
        [TestCase(1, 0, 0, false)]
        [TestCase(1, 0, 1, false)]
        [TestCase(1, 1, -1, true)]
        [TestCase(1, 1, 0, false)]
        [TestCase(1, 1, 1, false)]
        [TestCase(1, 2, -1, true)]
        [TestCase(1, 2, 0, true)]
        [TestCase(1, 2, 1, true)]
        [TestCase(4, -1, -1, true)]
        [TestCase(4, -1, 0, true)]
        [TestCase(4, -1, 1, true)]
        [TestCase(4, 0, -1, true)]
        [TestCase(4, 0, 0, false)]
        [TestCase(4, 0, 1, false)]
        [TestCase(4, 4, -1, true)]
        [TestCase(4, 4, 0, false)]
        [TestCase(4, 4, 1, false)]
        [TestCase(4, 5, -1, true)]
        [TestCase(4, 5, 0, true)]
        [TestCase(4, 5, 1, true)]
        public static void InsertRangeTest(int initLength, int index, int count, bool isError)
        {
            var instance = new CommonEventSpecialArgCaseList(MakeSpecialArgCaseArray(initLength));

            var errorOccured = false;
            try
            {
                instance.InsertRange(index, MakeSpecialArgCaseArray(count));
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(1, -1, false, true)]
        [TestCase(1, -1, true, true)]
        [TestCase(1, 0, false, false)]
        [TestCase(1, 0, true, true)]
        [TestCase(1, 1, false, true)]
        [TestCase(1, 1, true, true)]
        [TestCase(4, -1, false, true)]
        [TestCase(4, -1, true, true)]
        [TestCase(4, 0, false, false)]
        [TestCase(4, 0, true, true)]
        [TestCase(4, 3, false, false)]
        [TestCase(4, 3, true, true)]
        [TestCase(4, 4, false, true)]
        [TestCase(4, 4, true, true)]
        public static void UpdateTest(int initLength, int index, bool isUpdateEmpty, bool isError)
        {
            var item = isUpdateEmpty ? null : new CommonEventSpecialArgCase(100, "");

            var instance = new CommonEventSpecialArgCaseList(MakeSpecialArgCaseArray(initLength));

            var errorOccured = false;
            try
            {
                instance.Update(index, item);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(1, -1, true)]
        [TestCase(1, 0, false)]
        [TestCase(1, 1, true)]
        [TestCase(4, -1, true)]
        [TestCase(4, 0, false)]
        [TestCase(4, 3, false)]
        [TestCase(4, 4, true)]
        public static void RemoveAtTest(int initLength, int index, bool isError)
        {
            var instance = new CommonEventSpecialArgCaseList(MakeSpecialArgCaseArray(initLength));

            var errorOccured = false;
            try
            {
                instance.RemoveAt(index);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(1, -1, -1, true)]
        [TestCase(1, -1, 0, true)]
        [TestCase(1, -1, 1, true)]
        [TestCase(1, 0, -1, true)]
        [TestCase(1, 0, 0, false)]
        [TestCase(1, 0, 1, false)]
        [TestCase(1, 1, -1, true)]
        [TestCase(1, 1, 0, true)]
        [TestCase(1, 1, 1, true)]
        [TestCase(4, -1, -1, true)]
        [TestCase(4, -1, 0, true)]
        [TestCase(4, -1, 1, true)]
        [TestCase(4, 0, -1, true)]
        [TestCase(4, 0, 0, false)]
        [TestCase(4, 0, 4, false)]
        [TestCase(4, 0, 5, true)]
        [TestCase(4, 1, -1, true)]
        [TestCase(4, 1, 0, false)]
        [TestCase(4, 1, 3, false)]
        [TestCase(4, 1, 4, true)]
        [TestCase(4, 3, -1, true)]
        [TestCase(4, 3, 0, false)]
        [TestCase(4, 3, 1, false)]
        [TestCase(4, 3, 2, true)]
        [TestCase(4, 4, -1, true)]
        [TestCase(4, 4, 0, true)]
        [TestCase(4, 4, 1, true)]
        public static void RemoveRangeTest(int initLength, int index, int count, bool isError)
        {
            var instance = new CommonEventSpecialArgCaseList(MakeSpecialArgCaseArray(initLength));

            var errorOccured = false;
            try
            {
                instance.RemoveRange(index, count);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        public static void ClearTest(bool isError)
        {
            var instance = new CommonEventSpecialArgCaseList(MakeSpecialArgCaseArray(10));

            var errorOccured = false;
            try
            {
                instance.Clear();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            // 取得した件数が0件であること
            var caseList = instance.GetAllCase();
            Assert.AreEqual(caseList.Count, 0);
        }

        private static CommonEventSpecialArgCase[] MakeSpecialArgCaseArray(int length)
        {
            if (length == -1) return null;

            var argCaseList = new List<CommonEventSpecialArgCase>();

            for (var i = 0; i < length; i++)
            {
                argCaseList.Add(new CommonEventSpecialArgCase(i, ""));
            }

            return argCaseList.ToArray();
        }
    }
}