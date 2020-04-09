using System;
using System.Collections.Generic;
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
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

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

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [TestCase(0x00_00, true)]
        [TestCase(0x00_21, false)]
        [TestCase(0x02_00, true)]
        public static void ImpassableFlagsTest(int code, bool isError)
        {
            var instance = new TilePathSetting(code);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

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

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
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
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

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

            if (!errorOccured)
            {
                var value = instance.PathOption;

                // セットした値と取得した値が一致すること
                Assert.AreEqual(value, option);
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(TilePathSetting.PathOption)));
            }
        }

        [TestCase(0x00_00, false)]
        [TestCase(0x00_21, true)]
        [TestCase(0x02_00, false)]
        public static void CannotPassingFlagsTest(int code, bool isError)
        {
            var instance = new TilePathSetting(code);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

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

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void IsCounterTest()
        {
            const bool setValue = true;
            var instance = new TilePathSetting();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

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

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(TilePathSetting.IsCounter)));
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
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

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

            if (!errorOccured)
            {
                // 通行許可設定が"許可"になっていること
                Assert.AreEqual(instance.PathPermission, TilePathPermission.Allow);

                // 通行方向設定がセットした値と一致すること（設定を指定しなかった場合デフォルト値が設定されていること）
                Assert.IsTrue(instance.CannotPassingFlags.Equals(flags ?? new TileCannotPassingFlags()));
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 5);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(TilePathSetting.PathPermission)));
                Assert.IsTrue(changedPropertyList[1].Equals(nameof(TilePathSetting.ImpassableFlags)));
                Assert.IsTrue(changedPropertyList[2].Equals(nameof(TilePathSetting.PathOption)));
                Assert.IsTrue(changedPropertyList[3].Equals(nameof(TilePathSetting.CannotPassingFlags)));
                Assert.IsTrue(changedPropertyList[4].Equals(nameof(TilePathSetting.IsCounter)));
            }
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
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

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

            if (!errorOccured)
            {
                // 通行許可設定が"下レイヤーに依存"になっていること
                Assert.AreEqual(instance.PathPermission, TilePathPermission.Dependent);

                // 通行方向設定がセットした値と一致すること（設定を指定しなかった場合デフォルト値が設定されていること）
                Assert.IsTrue(instance.CannotPassingFlags.Equals(flags ?? new TileCannotPassingFlags()));
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 5);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(TilePathSetting.PathPermission)));
                Assert.IsTrue(changedPropertyList[1].Equals(nameof(TilePathSetting.ImpassableFlags)));
                Assert.IsTrue(changedPropertyList[2].Equals(nameof(TilePathSetting.PathOption)));
                Assert.IsTrue(changedPropertyList[3].Equals(nameof(TilePathSetting.CannotPassingFlags)));
                Assert.IsTrue(changedPropertyList[4].Equals(nameof(TilePathSetting.IsCounter)));
            }
        }

        [Test]
        public static void ChangePathPermissionDenyTest()
        {
            var instance = new TilePathSetting();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

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

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 5);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(TilePathSetting.PathPermission)));
            Assert.IsTrue(changedPropertyList[1].Equals(nameof(TilePathSetting.ImpassableFlags)));
            Assert.IsTrue(changedPropertyList[2].Equals(nameof(TilePathSetting.PathOption)));
            Assert.IsTrue(changedPropertyList[3].Equals(nameof(TilePathSetting.CannotPassingFlags)));
            Assert.IsTrue(changedPropertyList[4].Equals(nameof(TilePathSetting.IsCounter)));
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
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

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

            if (!errorOccured)
            {
                // 通行許可設定が"部分的に通行不可"になっていること
                Assert.AreEqual(instance.PathPermission, TilePathPermission.PartialDeny);

                // 通行許可設定がセットした値と一致すること（設定を指定しなかった場合デフォルト値が設定されていること）
                Assert.IsTrue(instance.ImpassableFlags.Equals(flags ?? new TileImpassableFlags()));
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 5);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(TilePathSetting.PathPermission)));
                Assert.IsTrue(changedPropertyList[1].Equals(nameof(TilePathSetting.ImpassableFlags)));
                Assert.IsTrue(changedPropertyList[2].Equals(nameof(TilePathSetting.PathOption)));
                Assert.IsTrue(changedPropertyList[3].Equals(nameof(TilePathSetting.CannotPassingFlags)));
                Assert.IsTrue(changedPropertyList[4].Equals(nameof(TilePathSetting.IsCounter)));
            }
        }

        [Test]
        public static void SerializeTest()
        {
            var target = new TilePathSetting
            {
                IsCounter = true
            };
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }
    }
}