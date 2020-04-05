using System;
using System.Collections.Generic;
using NUnit.Framework;
using WodiLib.Map;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class TileImpassableFlagsTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [TestCase(0, true, false, false, false, false)]
        [TestCase(1, false, true, false, false, false)]
        [TestCase(2, false, false, true, false, false)]
        [TestCase(3, false, true, true, false, false)]
        [TestCase(4, false, false, false, true, false)]
        [TestCase(5, false, true, false, true, false)]
        [TestCase(6, false, false, true, true, false)]
        [TestCase(7, false, true, true, true, false)]
        [TestCase(8, false, false, false, false, true)]
        [TestCase(9, false, true, false, false, true)]
        [TestCase(10, false, false, true, false, true)]
        [TestCase(11, false, true, true, false, true)]
        [TestCase(12, false, false, false, true, true)]
        [TestCase(13, false, true, false, true, true)]
        [TestCase(14, false, false, true, true, true)]
        [TestCase(15, false, true, true, true, true)]
        [TestCase(16, true, false, false, false, false)]
        public static void ConstructorTest(int code, bool isError,
            bool rightDown, bool leftDown, bool rightUp, bool leftUp)
        {
            TileImpassableFlags instance = null;

            var errorOccured = false;
            try
            {
                instance = new TileImpassableFlags(code);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // 各プロパティの値が意図した値であること
            Assert.AreEqual(instance.RightDown, rightDown);
            Assert.AreEqual(instance.LeftDown, leftDown);
            Assert.AreEqual(instance.RightUp, rightUp);
            Assert.AreEqual(instance.LeftUp, leftUp);
        }

        [TestCase(false, 1, true)]
        [TestCase(true, 1, false)]
        [TestCase(false, 2, false)]
        [TestCase(true, 2, false)]
        [TestCase(false, 3, false)]
        [TestCase(true, 3, false)]
        [TestCase(false, 4, false)]
        [TestCase(true, 4, false)]
        [TestCase(false, 5, false)]
        [TestCase(true, 5, false)]
        [TestCase(false, 6, false)]
        [TestCase(true, 6, false)]
        [TestCase(false, 7, false)]
        [TestCase(true, 7, false)]
        [TestCase(false, 8, false)]
        [TestCase(true, 8, false)]
        [TestCase(false, 9, false)]
        [TestCase(true, 9, false)]
        [TestCase(false, 10, false)]
        [TestCase(true, 10, false)]
        [TestCase(false, 11, false)]
        [TestCase(true, 11, false)]
        [TestCase(false, 12, false)]
        [TestCase(true, 12, false)]
        [TestCase(false, 13, false)]
        [TestCase(true, 13, false)]
        [TestCase(false, 14, false)]
        [TestCase(true, 14, false)]
        [TestCase(false, 15, false)]
        [TestCase(true, 15, false)]
        public static void RightDownTest(bool rightDown, int initCode, bool isError)
        {
            var instance = new TileImpassableFlags(initCode);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.RightDown = rightDown;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured)
            {
                var setValue = instance.RightDown;

                // セットした値と取得した値が一致すること
                Assert.IsTrue(setValue.Equals(setValue));
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(TileImpassableFlags.RightDown)));
            }
        }

        [TestCase(false, 1, false)]
        [TestCase(true, 1, false)]
        [TestCase(false, 2, true)]
        [TestCase(true, 2, false)]
        [TestCase(false, 3, false)]
        [TestCase(true, 3, false)]
        [TestCase(false, 4, false)]
        [TestCase(true, 4, false)]
        [TestCase(false, 5, false)]
        [TestCase(true, 5, false)]
        [TestCase(false, 6, false)]
        [TestCase(true, 6, false)]
        [TestCase(false, 7, false)]
        [TestCase(true, 7, false)]
        [TestCase(false, 8, false)]
        [TestCase(true, 8, false)]
        [TestCase(false, 9, false)]
        [TestCase(true, 9, false)]
        [TestCase(false, 10, false)]
        [TestCase(true, 10, false)]
        [TestCase(false, 11, false)]
        [TestCase(true, 11, false)]
        [TestCase(false, 12, false)]
        [TestCase(true, 12, false)]
        [TestCase(false, 13, false)]
        [TestCase(true, 13, false)]
        [TestCase(false, 14, false)]
        [TestCase(true, 14, false)]
        [TestCase(false, 15, false)]
        [TestCase(true, 15, false)]
        public static void LeftDownTest(bool leftDown, int initCode, bool isError)
        {
            var instance = new TileImpassableFlags(initCode);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.LeftDown = leftDown;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured)
            {
                var setValue = instance.LeftDown;

                // セットした値と取得した値が一致すること
                Assert.IsTrue(setValue.Equals(setValue));
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(TileImpassableFlags.LeftDown)));
            }
        }

        [TestCase(false, 1, false)]
        [TestCase(true, 1, false)]
        [TestCase(false, 2, false)]
        [TestCase(true, 2, false)]
        [TestCase(false, 3, false)]
        [TestCase(true, 3, false)]
        [TestCase(false, 4, true)]
        [TestCase(true, 4, false)]
        [TestCase(false, 5, false)]
        [TestCase(true, 5, false)]
        [TestCase(false, 6, false)]
        [TestCase(true, 6, false)]
        [TestCase(false, 7, false)]
        [TestCase(true, 7, false)]
        [TestCase(false, 8, false)]
        [TestCase(true, 8, false)]
        [TestCase(false, 9, false)]
        [TestCase(true, 9, false)]
        [TestCase(false, 10, false)]
        [TestCase(true, 10, false)]
        [TestCase(false, 11, false)]
        [TestCase(true, 11, false)]
        [TestCase(false, 12, false)]
        [TestCase(true, 12, false)]
        [TestCase(false, 13, false)]
        [TestCase(true, 13, false)]
        [TestCase(false, 14, false)]
        [TestCase(true, 14, false)]
        [TestCase(false, 15, false)]
        [TestCase(true, 15, false)]
        public static void RightUpTest(bool rightUp, int initCode, bool isError)
        {
            var instance = new TileImpassableFlags(initCode);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.RightUp = rightUp;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured)
            {
                var setValue = instance.RightUp;

                // セットした値と取得した値が一致すること
                Assert.IsTrue(setValue.Equals(setValue));
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(TileImpassableFlags.RightUp)));
            }
        }

        [TestCase(false, 1, false)]
        [TestCase(true, 1, false)]
        [TestCase(false, 2, false)]
        [TestCase(true, 2, false)]
        [TestCase(false, 3, false)]
        [TestCase(true, 3, false)]
        [TestCase(false, 4, false)]
        [TestCase(true, 4, false)]
        [TestCase(false, 5, false)]
        [TestCase(true, 5, false)]
        [TestCase(false, 6, false)]
        [TestCase(true, 6, false)]
        [TestCase(false, 7, false)]
        [TestCase(true, 7, false)]
        [TestCase(false, 8, true)]
        [TestCase(true, 8, false)]
        [TestCase(false, 9, false)]
        [TestCase(true, 9, false)]
        [TestCase(false, 10, false)]
        [TestCase(true, 10, false)]
        [TestCase(false, 11, false)]
        [TestCase(true, 11, false)]
        [TestCase(false, 12, false)]
        [TestCase(true, 12, false)]
        [TestCase(false, 13, false)]
        [TestCase(true, 13, false)]
        [TestCase(false, 14, false)]
        [TestCase(true, 14, false)]
        [TestCase(false, 15, false)]
        [TestCase(true, 15, false)]
        public static void LeftUpTest(bool LeftUp, int initCode, bool isError)
        {
            var instance = new TileImpassableFlags(initCode);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.LeftUp = LeftUp;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured)
            {
                var setValue = instance.LeftUp;

                // セットした値と取得した値が一致すること
                Assert.IsTrue(setValue.Equals(setValue));
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(TileImpassableFlags.LeftUp)));
            }
        }

        [TestCase(1, 1, true)]
        [TestCase(1, 3, false)]
        [TestCase(1, 4, false)]
        [TestCase(7, 1, false)]
        [TestCase(7, 7, true)]
        [TestCase(7, 15, false)]
        public static void EqualsTest(int targetCode, int otherCode, bool answer)
        {
            var target = new TileImpassableFlags(targetCode);
            var other = new TileImpassableFlags(otherCode);

            var result = false;

            var errorOccured = false;
            try
            {
                result = target.Equals(other);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 結果が取得した値と一致すること
            Assert.AreEqual(result, answer);
        }

        [Test]
        public static void SerializeTest()
        {
            var target = new TileImpassableFlags
            {
                LeftDown = true
            };
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }
    }
}