using System;
using System.Collections.Generic;
using NUnit.Framework;
using WodiLib.Map;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class TileCannotPassingFlagsTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [TestCase(0, false, false, false, false, false)]
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
        [TestCase(15, true, true, true, true, true)]
        [TestCase(16, false, false, false, false, false)]
        public static void ConstructorTest(int code, bool isError,
            bool down, bool left, bool right, bool up)
        {
            TileCannotPassingFlags instance = null;

            var errorOccured = false;
            try
            {
                instance = new TileCannotPassingFlags(code);
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
            Assert.AreEqual(instance.Down, down);
            Assert.AreEqual(instance.Left, left);
            Assert.AreEqual(instance.Right, right);
            Assert.AreEqual(instance.Up, up);
        }

        [TestCase(false, 0, false)]
        [TestCase(true, 0, false)]
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
        [TestCase(true, 14, true)]
        public static void DownTest(bool down, int initCode, bool isError)
        {
            var instance = new TileCannotPassingFlags(initCode);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.Down = down;
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
                var setValue = instance.Down;

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
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(TileCannotPassingFlags.Down)));
            }
        }

        [TestCase(false, 0, false)]
        [TestCase(true, 0, false)]
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
        [TestCase(true, 13, true)]
        [TestCase(false, 14, false)]
        [TestCase(true, 14, false)]
        public static void LeftTest(bool left, int initCode, bool isError)
        {
            var instance = new TileCannotPassingFlags(initCode);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.Left = left;
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
                var setValue = instance.Left;

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
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(TileCannotPassingFlags.Left)));
            }
        }

        [TestCase(false, 0, false)]
        [TestCase(true, 0, false)]
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
        [TestCase(false, 8, false)]
        [TestCase(true, 8, false)]
        [TestCase(false, 9, false)]
        [TestCase(true, 9, false)]
        [TestCase(false, 10, false)]
        [TestCase(true, 10, false)]
        [TestCase(false, 11, false)]
        [TestCase(true, 11, true)]
        [TestCase(false, 12, false)]
        [TestCase(true, 12, false)]
        [TestCase(false, 13, false)]
        [TestCase(true, 13, false)]
        [TestCase(false, 14, false)]
        [TestCase(true, 14, false)]
        public static void RightTest(bool right, int initCode, bool isError)
        {
            var instance = new TileCannotPassingFlags(initCode);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.Right = right;
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
                var setValue = instance.Right;

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
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(TileCannotPassingFlags.Right)));
            }
        }

        [TestCase(false, 0, false)]
        [TestCase(true, 0, false)]
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
        [TestCase(true, 7, true)]
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
        public static void UpTest(bool up, int initCode, bool isError)
        {
            var instance = new TileCannotPassingFlags(initCode);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.Up = up;
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
                var setValue = instance.Up;

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
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(TileCannotPassingFlags.Up)));
            }
        }

        [TestCase(1, 1, true)]
        [TestCase(1, 3, false)]
        [TestCase(1, 4, false)]
        [TestCase(7, 1, false)]
        [TestCase(7, 7, true)]
        [TestCase(7, 0, false)]
        public static void EqualsTest(int targetCode, int otherCode, bool answer)
        {
            var target = new TileCannotPassingFlags(targetCode);
            var other = new TileCannotPassingFlags(otherCode);

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
            var target = new TileCannotPassingFlags
            {
                Down = true,
            };
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }
    }
}