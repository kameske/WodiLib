using System;
using Commons;
using NUnit.Framework;
using WodiLib.Map;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class MapChipTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [TestCase(-1, true)]
        [TestCase(0, false)]
        [TestCase(99999, false)]
        [TestCase(100000, false)]
        [TestCase(1604444, false)]
        [TestCase(1604445, true)]
        public static void NewInstanceTest(int id, bool isError)
        {
            var errorOccured = false;
            MapChip instance = MapChip.Default;
            try
            {
                instance = new MapChip(id);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // インスタンスの値と設定値が一致すること
            Assert.AreEqual((int) instance, id);
        }

        [TestCase(-1, true)]
        [TestCase(0, false)]
        [TestCase(99999, false)]
        [TestCase(100000, false)]
        [TestCase(1604444, false)]
        [TestCase(1604445, true)]
        public static void ImplicitFromIntTest(int id, bool isError)
        {
            var errorOccured = false;
            MapChip instance = MapChip.Default;
            try
            {
                instance = id;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // インスタンスの値と設定値が一致すること
            Assert.AreEqual((int) instance, id);
        }

        [TestCase(0, false)]
        [TestCase(99999, false)]
        [TestCase(100000, true)]
        [TestCase(1604444, true)]
        public static void IsAutoTileNumberTest(int id, bool isAutoTileNumber)
        {
            var instance = (MapChip) id;

            // フラグが一致すること
            Assert.AreEqual(instance.IsAutoTile, isAutoTileNumber);
        }

        private static readonly object[] LeftUpAutoTileGetTestCaseSource =
        {
            new object[] {0, true, null},
            new object[] {100123, false, AutoTilePartType.ConnectionCentral},
            new object[] {101234, false, AutoTilePartType.ConnectionVertical},
            new object[] {102340, false, AutoTilePartType.ConnectionHorizontal},
            new object[] {103401, false, AutoTilePartType.ConnectionOutSide},
            new object[] {104012, false, AutoTilePartType.SurroundingFilled},
        };

        [TestCaseSource(nameof(LeftUpAutoTileGetTestCaseSource))]
        public static void LeftUpAutoTileGetTest(int id, bool isError, AutoTilePartType type)
        {
            var instance = (MapChip) id;

            var errorOccured = false;
            AutoTilePartType result = null;
            try
            {
                result = instance.LeftUpAutoTile;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // オートタイル種別が一致すること
            Assert.AreEqual(result, type);
        }

        private static readonly object[] RightUpAutoTileGetTestCaseSource =
        {
            new object[] {0, true, null},
            new object[] {104012, false, AutoTilePartType.ConnectionCentral},
            new object[] {100123, false, AutoTilePartType.ConnectionVertical},
            new object[] {101234, false, AutoTilePartType.ConnectionHorizontal},
            new object[] {102345, false, AutoTilePartType.ConnectionOutSide},
            new object[] {103401, false, AutoTilePartType.SurroundingFilled},
        };

        [TestCaseSource(nameof(RightUpAutoTileGetTestCaseSource))]
        public static void RightUpAutoTileGetTest(int id, bool isError, AutoTilePartType type)
        {
            var instance = (MapChip) id;

            var errorOccured = false;
            AutoTilePartType result = null;
            try
            {
                result = instance.RightUpAutoTile;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // オートタイル種別が一致すること
            Assert.AreEqual(result, type);
        }

        private static readonly object[] LeftDownAutoTileGetTestCaseSource =
        {
            new object[] {0, true, null},
            new object[] {103401, false, AutoTilePartType.ConnectionCentral},
            new object[] {104012, false, AutoTilePartType.ConnectionVertical},
            new object[] {100123, false, AutoTilePartType.ConnectionHorizontal},
            new object[] {101234, false, AutoTilePartType.ConnectionOutSide},
            new object[] {102340, false, AutoTilePartType.SurroundingFilled},
        };

        [TestCaseSource(nameof(LeftDownAutoTileGetTestCaseSource))]
        public static void LeftDownAutoTileGetTest(int id, bool isError, AutoTilePartType type)
        {
            var instance = (MapChip) id;

            var errorOccured = false;
            AutoTilePartType result = null;
            try
            {
                result = instance.LeftDownAutoTile;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // オートタイル種別が一致すること
            Assert.AreEqual(result, type);
        }

        private static readonly object[] RightDownAutoTileGetTestCaseSource =
        {
            new object[] {0, true, null},
            new object[] {102340, false, AutoTilePartType.ConnectionCentral},
            new object[] {103401, false, AutoTilePartType.ConnectionVertical},
            new object[] {104012, false, AutoTilePartType.ConnectionHorizontal},
            new object[] {100123, false, AutoTilePartType.ConnectionOutSide},
            new object[] {101234, false, AutoTilePartType.SurroundingFilled},
        };

        [TestCaseSource(nameof(RightDownAutoTileGetTestCaseSource))]
        public static void RightDownAutoTileGetTest(int id, bool isError, AutoTilePartType type)
        {
            var instance = (MapChip) id;

            var errorOccured = false;
            AutoTilePartType result = null;
            try
            {
                result = instance.RightDownAutoTile;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // オートタイル種別が一致すること
            Assert.AreEqual(result, type);
        }

        [TestCase(-1, false)]
        [TestCase(0, true)]
        [TestCase(99999, true)]
        [TestCase(100000, true)]
        [TestCase(1604444, true)]
        [TestCase(1604445, false)]
        public static void Static_IsMapChipNumberTest(int id, bool isMapChipNumber)
        {
            var result = MapChip.IsMapChipNumber(id);

            // フラグが一致すること
            Assert.AreEqual(result, isMapChipNumber);
        }

        [TestCase(-1, false)]
        [TestCase(0, true)]
        [TestCase(99999, true)]
        [TestCase(100000, false)]
        [TestCase(1604444, false)]
        [TestCase(1604445, false)]
        public static void Static_IsStandardTileNumber(int id, bool isStandardTileNumber)
        {
            var result = MapChip.IsStandardTileNumber(id);

            // フラグが一致すること
            Assert.AreEqual(result, isStandardTileNumber);
        }

        [TestCase(-1, false)]
        [TestCase(0, false)]
        [TestCase(99999, false)]
        [TestCase(100000, true)]
        [TestCase(1604444, true)]
        [TestCase(1604445, false)]
        public static void Static_IsAutoTileNumberTest(int id, bool isAutoTileNumber)
        {
            var result = MapChip.IsAutoTileNumber(id);

            // フラグが一致すること
            Assert.AreEqual(result, isAutoTileNumber);
        }

        private static readonly object[] EqualTestCaseSource =
        {
            new object[] {0, 0, true},
            new object[] {0, 31, false},
        };

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualTest(int left, int right, bool isEqual)
        {
            var leftIndex = (MapChip) left;
            var rightIndex = (MapChip) right;
            Assert.AreEqual(leftIndex == rightIndex, isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorNotEqualTest(int left, int right, bool isEqual)
        {
            var leftIndex = (MapChip) left;
            var rightIndex = (MapChip) right;
            Assert.AreEqual(leftIndex != rightIndex, !isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualsTest(int left, int right, bool isEqual)
        {
            var leftIndex = (MapChip) left;
            var rightIndex = (MapChip) right;
            Assert.AreEqual(leftIndex.Equals(rightIndex), isEqual);
        }
    }
}
