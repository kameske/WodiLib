using System;
using System.Collections.Generic;
using NUnit.Framework;
using WodiLib.Common;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Common.Internal
{
    [TestFixture]
    public class CommonEventSpecialNumberArgDesc_InnerDescFactory
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        private static readonly object[] CreateTestCaseSource =
        {
            new object[] {CommonEventArgType.Normal, true, false},
            new object[] {CommonEventArgType.Normal, false, false},
            new object[] {CommonEventArgType.ReferDatabase, true, false},
            new object[] {CommonEventArgType.ReferDatabase, false, false},
            new object[] {CommonEventArgType.Manual, true, false},
            new object[] {CommonEventArgType.Manual, false, false},
            new object[] {null, true, true},
            new object[] {null, false, true},
        };

        [TestCaseSource(nameof(CreateTestCaseSource))]
        public static void CreateTest(CommonEventArgType type, bool isNullArgCaseList, bool isError)
        {
            var errorOccured = false;
            try
            {
                var argCaseList = isNullArgCaseList ? null : new CommonEventSpecialArgCaseList();
                CommonEventSpecialNumberArgDesc.InnerDescFactory.Create(type, argCaseList);
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
        public static void CreateNormalTest()
        {
            var errorOccured = false;
            try
            {
                CommonEventSpecialNumberArgDesc.InnerDescFactory.CreateNormal();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);
        }

        private static readonly object[] CreateReferDatabaseTestCaseSource =
        {
            new object[]
            {
                new Dictionary<int, string>
                {
                    {-1, "-1"}, {-2, "-2"}, {-3, "-3"}
                },
                false
            },
            new object[]
            {
                new Dictionary<int, string>
                {
                    {-1, "-1"}, {-2, "-2"}
                },
                false
            },
            new object[]
            {
                new Dictionary<int, string>
                {
                    {1, "-1"}, {2, "-2"}, {3, "-3"}
                },
                false
            },
            new object[]
            {
                new Dictionary<int, string>(),
                false
            },
            new object[] {null, false},
        };

        [TestCaseSource(nameof(CreateReferDatabaseTestCaseSource))]
        public static void CreateReferDatabaseTest(Dictionary<int, string> argCaseDic, bool isError)
        {
            CommonEventSpecialArgCaseList argCaseList;
            if (argCaseDic == null)
            {
                argCaseList = null;
            }
            else
            {
                var argCases = new List<CommonEventSpecialArgCase>();
                foreach (var argCase in argCaseDic)
                {
                    argCases.Add(new CommonEventSpecialArgCase(argCase.Key, argCase.Value));
                }

                argCaseList = new CommonEventSpecialArgCaseList(argCases.ToArray());
            }

            var errorOccured = false;
            try
            {
                CommonEventSpecialNumberArgDesc.InnerDescFactory.CreateReferDatabase(argCaseList);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(false, false)]
        [TestCase(true, false)]
        public static void CreateManualTest(bool isNullArgCaseList, bool isError)
        {
            var errorOccured = false;
            try
            {
                var argCaseList = isNullArgCaseList ? null : new CommonEventSpecialArgCaseList();
                CommonEventSpecialNumberArgDesc.InnerDescFactory.CreateManual(argCaseList);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }
    }
}