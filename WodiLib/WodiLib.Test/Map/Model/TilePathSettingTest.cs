using System;
using NUnit.Framework;
using WodiLib.Map;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class TilePathSettingTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [TestCase(0x00, false)]
        [TestCase(0x05, false)]
        [TestCase(0x20, true)]
        [TestCase(0x22, false)]
        [TestCase(0x02_00, false)]
        [TestCase(0x40, false)]
        [TestCase(0xA3, false)]
        [TestCase(0x02_07, false)]
        [TestCase(0x08_00, false)]
        public static void ConstructorTest(int code, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new TilePathSetting(code);
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
            var instance = new TilePathSetting();

            var errorOccured = false;
            try
            {
                var _ = instance.PathPermission;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);
        }

        [TestCase(0x00_00, true)]
        [TestCase(0x00_21, false)]
        [TestCase(0x02_00, true)]
        public static void ImpassableFlagsTest(int code, bool isError)
        {
            var instance = new TilePathSetting(code);

            var errorOccured = false;
            try
            {
                var _ = instance.ImpassableFlags;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        private static readonly object[] PathOptionTestCaseSource =
        {
            new object[] {TilePathOption.Hide, false},
            new object[] {null, true},
        };

        [TestCaseSource(nameof(PathOptionTestCaseSource))]
        public static void PathOptionTest(TilePathOption option, bool isError)
        {
            var instance = new TilePathSetting();

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

            var value = instance.PathOption;

            // セットした値と取得した値が一致すること
            Assert.AreEqual(value, option);
        }

        [TestCase(0x00_00, false)]
        [TestCase(0x00_21, true)]
        [TestCase(0x02_00, false)]
        public static void CannotPassingFlagsTest(int code, bool isError)
        {
            var instance = new TilePathSetting(code);

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

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [Test]
        public static void IsCounterTest()
        {
            const bool setValue = true;
            var instance = new TilePathSetting();

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

            var value = instance.IsCounter;

            // セットした値と取得した値が一致すること
            Assert.AreEqual(value, setValue);
        }

        private static readonly object[] ChangePathPermissionAllowTestCaseSource =
        {
            new object[] {new TileCannotPassingFlags {Left = true}, false},
            new object[] {null, false},
        };

        [TestCaseSource(nameof(ChangePathPermissionAllowTestCaseSource))]
        public static void ChangePathPermissionAllowTest(TileCannotPassingFlags flags, bool isError)
        {
            var instance = new TilePathSetting();

            var errorOccured = false;
            try
            {
                instance.ChangePathPermissionAllow(flags);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // 通行許可設定が"許可"になっていること
            Assert.AreEqual(instance.PathPermission, TilePathPermission.Allow);

            // 通行方向設定がセットした値と一致すること（設定を指定しなかった場合デフォルト値が設定されていること）
            Assert.IsTrue(instance.CannotPassingFlags.Equals(flags ?? new TileCannotPassingFlags()));
        }

        private static readonly object[] ChangePathPermissionDependentTestCaseSource =
        {
            new object[] {new TileCannotPassingFlags {Left = true}, false},
            new object[] {null, false},
        };

        [TestCaseSource(nameof(ChangePathPermissionDependentTestCaseSource))]
        public static void ChangePathPermissionDependentTest(TileCannotPassingFlags flags, bool isError)
        {
            var instance = new TilePathSetting();

            var errorOccured = false;
            try
            {
                instance.ChangePathPermissionDependent(flags);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // 通行許可設定が"下レイヤーに依存"になっていること
            Assert.AreEqual(instance.PathPermission, TilePathPermission.Dependent);

            // 通行方向設定がセットした値と一致すること（設定を指定しなかった場合デフォルト値が設定されていること）
            Assert.IsTrue(instance.CannotPassingFlags.Equals(flags ?? new TileCannotPassingFlags()));
        }

        [Test]
        public static void ChangePathPermissionDenyTest()
        {
            var instance = new TilePathSetting();

            var errorOccured = false;
            try
            {
                instance.ChangePathPermissionDeny();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 通行許可設定が"通行不可"になっていること
            Assert.AreEqual(instance.PathPermission, TilePathPermission.Deny);
        }

        private static readonly object[] ChangePathPermissionPartialDenyTestCaseSource =
        {
            new object[] {new TileImpassableFlags {LeftUp = true}, false},
            new object[] {null, false},
        };

        [TestCaseSource(nameof(ChangePathPermissionPartialDenyTestCaseSource))]
        public static void ChangePathPermissionPartialDenyTest(TileImpassableFlags flags, bool isError)
        {
            var instance = new TilePathSetting();

            var errorOccured = false;
            try
            {
                instance.ChangePathPermissionPartialDeny(flags);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // 通行許可設定が"部分的に通行不可"になっていること
            Assert.AreEqual(instance.PathPermission, TilePathPermission.PartialDeny);

            // 通行許可設定がセットした値と一致すること（設定を指定しなかった場合デフォルト値が設定されていること）
            Assert.IsTrue(instance.ImpassableFlags.Equals(flags ?? new TileImpassableFlags()));
        }
    }
}