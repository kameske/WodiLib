using System;
using System.Linq;
using NUnit.Framework;
using WodiLib.Map;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class MapTreeNodeTest
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
            new object[] {(MapId) (-1), (MapId) (-1), true},
            new object[] {(MapId) (-1), (MapId) 0, true},
            new object[] {(MapId) 5, (MapId) (-1), false},
            new object[] {(MapId) 5, (MapId) 4, false},
            new object[] {(MapId) 5, (MapId) 5, true},
            new object[] {(MapId) 5, (MapId) 6, false},
        };

        [TestCaseSource(nameof(ConstructorTestCaseSource))]
        public static void ConstructorTest(MapId me, MapId parent, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new MapTreeNode(me, parent);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        private static readonly object[] ToBytesTestCaseSource =
        {
            new object[]
            {
                (MapId) 10, (MapId) 22, Endian.Little, new byte[]
                {
                    0x16, 0x00, 0x00, 0x00,
                    0x0A, 0x00, 0x00, 0x00,
                }
            },
            new object[]
            {
                (MapId) 2544, (MapId) (-1), Endian.Big, new byte[]
                {
                    0xFF, 0xFF, 0xFF, 0xFF,
                    0x00, 0x00, 0x09, 0xF0,
                }
            },
        };

        [TestCaseSource(nameof(ToBytesTestCaseSource))]
        public static void ToBytesTest(MapId me, MapId parent, Endian endian, byte[] answer)
        {
            var instance = new MapTreeNode(me, parent);

            byte[] result = { };

            var errorOccured = false;
            try
            {
                result = instance.ToBytes(endian).ToArray();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 結果が意図した値と一致すること
            Assert.IsTrue(result.SequenceEqual(answer));
        }

        private static readonly object[] EqualTestCaseSource =
        {
            new object[] {new MapTreeNode(20, 21), new MapTreeNode(20, 21), true},
            new object[] {new MapTreeNode(20, 21), new MapTreeNode(30, 31), false},
            new object[] {new MapTreeNode(20, 21), new MapTreeNode(20, 31), false},
            new object[] {new MapTreeNode(20, 21), new MapTreeNode(30, 21), false},
            new object[] {new MapTreeNode(20, 21), new MapTreeNode(21, 20), false},
        };

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualTest(MapTreeNode left, MapTreeNode right, bool isEqual)
        {
            Assert.AreEqual(left == right, isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorNotEqualTest(MapTreeNode left, MapTreeNode right, bool isEqual)
        {
            Assert.AreEqual(left != right, !isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualsTest(MapTreeNode left, MapTreeNode right, bool isEqual)
        {
            Assert.AreEqual(left.Equals(right), isEqual);
        }

        [Test]
        public static void SerializeTest()
        {
            var target = new MapTreeNode(20, 21);
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }
    }
}