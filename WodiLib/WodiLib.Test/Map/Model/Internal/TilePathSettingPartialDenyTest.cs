using System;
using NUnit.Framework;
using WodiLib.Map;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class TilePathSettingPartialDenyTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [Test]
        public static void ConstructorTestA()
        {
            var errorOccured = false;
            try
            {
                var _ = new TilePathSettingPartialDeny();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);
        }

        private static readonly object[] ConstructorTestCaseSource =
        {
            new object[] {new TileImpassableFlags(), false},
            new object[] {null, true},
        };

        [TestCaseSource(nameof(ConstructorTestCaseSource))]
        public static void ConstructorTestB(TileImpassableFlags impassable, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new TilePathSettingPartialDeny(impassable);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(0x00, true)] // 通行不可なし
        [TestCase(0x02, false)] // 左通行不可
        [TestCase(0x96, false)] // 左・右通行不可
        [TestCase(0x9F, false)] // 全方向不可
        public static void ConstructorTestC(int code, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new TilePathSettingPartialDeny(code);
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
        public static void PathPermissionTest()
        {
            var instance = new TilePathSettingPartialDeny();

            TilePathPermission result = null;

            var errorOccured = false;
            try
            {
                result = instance.PathPermission;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 結果が意図した値と一致すること
            Assert.AreEqual(result, TilePathPermission.PartialDeny);
        }

        [Test]
        public static void ImpassableFlagsTest()
        {
            var setValue = new TileImpassableFlags(0x02);
            var instance = new TilePathSettingPartialDeny(setValue);

            TileImpassableFlags result = null;

            var errorOccured = false;
            try
            {
                result = instance.ImpassableFlags;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 取得した結果が意図した値であること
            Assert.AreEqual(result, setValue);
        }

        private static readonly object[] PathOptionTestCaseSource =
        {
            new object[] {TilePathOption.Nothing, false},
            new object[] {null, true},
        };

        [TestCaseSource(nameof(PathOptionTestCaseSource))]
        public static void PathOptionTest(TilePathOption option, bool isError)
        {
            var instance = new TilePathSettingPartialDeny();

            var errorOccured = false;
            try
            {
                instance.PathOption = option;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            TilePathOption result = null;

            try
            {
                result = instance.PathOption;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 結果が意図した値と一致すること
            Assert.AreEqual(result, option);
        }

        [Test]
        public static void CannotPassingFlagsTest()
        {
            var instance = new TilePathSettingPartialDeny();

            var errorOccured = false;
            try
            {
                var _ = instance.CannotPassingFlags;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生すること
            Assert.IsTrue(errorOccured);
        }

        [Test]
        public static void IsCounterTest()
        {
            const bool setValue = true;
            var instance = new TilePathSettingPartialDeny();


            var errorOccured = false;
            try
            {
                instance.IsCounter = setValue;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            var result = false;

            try
            {
                result = instance.IsCounter;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 結果が意図した値と一致すること
            Assert.AreEqual(result, setValue);
        }
    }
}