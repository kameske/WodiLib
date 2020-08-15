using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database
{
    [TestFixture]
    public class DataIdSpecificationDescTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        private static readonly object[] ConstructorTestCaseSource =
        {
            new object[] {null, (TypeId) TypeId.MinValue, true},
            new object[] {DBKind.User, (TypeId) TypeId.MaxValue, false},
            new object[] {DBKind.Changeable, (TypeId) TypeId.MinValue, false},
            new object[] {DBKind.System, (TypeId) TypeId.MaxValue, false},
        };

        [TestCaseSource(nameof(ConstructorTestCaseSource))]
        public static void ConstructorTest(DBKind dbKind, TypeId typeId, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new DataIdSpecificationDesc(dbKind, typeId);
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
            new object[]
            {
                new DataIdSpecificationDesc(DBKind.Changeable, 3), new DataIdSpecificationDesc(DBKind.Changeable, 3),
                true
            },
            new object[]
                {new DataIdSpecificationDesc(DBKind.User, 3), new DataIdSpecificationDesc(DBKind.Changeable, 3), false},
            new object[]
                {new DataIdSpecificationDesc(DBKind.System, 3), new DataIdSpecificationDesc(DBKind.System, 12), false},
        };

        [TestCaseSource(nameof(EqualsTestCaseSource))]
        public static void EqualsTest(DataIdSpecificationDesc left, DataIdSpecificationDesc right, bool isEquals)
        {
            Assert.AreEqual(left.Equals(right), isEquals);
        }
    }
}
