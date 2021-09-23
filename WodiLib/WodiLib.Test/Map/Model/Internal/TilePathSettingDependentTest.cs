using System;
using System.Collections.Generic;
using Commons;
using NUnit.Framework;
using WodiLib.Map;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class TilePathSettingDependentTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [Test]
        public static void ConstructorTestA()
        {
            var errorOccured = false;
            try
            {
                var _ = new TilePathSettingDependent();
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
            new object[] { new TileCannotPassingFlags(), false },
            new object[] { null, true }
        };

        [TestCaseSource(nameof(ConstructorTestCaseSource))]
        public static void ConstructorTestB(TileCannotPassingFlags cannotPassingFlags, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new TilePathSettingDependent(cannotPassingFlags);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(0x00, false)] // 通行不可なし
        [TestCase(0x02, false)] // 左通行不可
        [TestCase(0x96, false)] // 左・右通行不可
        [TestCase(0x9F, true)] // 全方向不可
        public static void ConstructorTestC(int code, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new TilePathSettingDependent(code);
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
            var instance = new TilePathSettingDependent();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

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
            Assert.AreEqual(result, TilePathPermission.Dependent);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void ImpassableFlagsTest()
        {
            var instance = new TilePathSettingDependent();
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

            // エラーが発生すること
            Assert.IsTrue(errorOccured);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        private static readonly object[] PathOptionTestCaseSource =
        {
            new object[] { TilePathOption.Nothing, false },
            new object[] { null, true }
        };

        [TestCaseSource(nameof(PathOptionTestCaseSource))]
        public static void PathOptionTest(TilePathOption option, bool isError)
        {
            var instance = new TilePathSettingDependent();
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

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(TilePathSettingDependent.PathOption)));
            }
        }

        [Test]
        public static void CannotPassingFlagsTest()
        {
            var setValue = new TileCannotPassingFlags(0x02);
            var instance = new TilePathSettingDependent(setValue);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            TileCannotPassingFlags result = null;
            var errorOccured = false;
            try
            {
                result = instance.CannotPassingFlags;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 結果が意図した値であること
            Assert.AreEqual(result, setValue);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void IsCounterTest()
        {
            const bool setValue = true;
            var instance = new TilePathSettingDependent();
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

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(TilePathSettingDependent.IsCounter)));
        }
    }
}
