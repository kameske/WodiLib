using NUnit.Framework;
using WodiLib.Map;
using WodiLib.Sys;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class MapChipTest
    {
        [TestCase(-1, true)]
        [TestCase(0, false)]
        [TestCase(99999, false)]
        [TestCase(100000, false)]
        [TestCase(1604444, false)]
        [TestCase(1604445, true)]
        public static void NewInstanceTest(int id, bool isError)
        {
            var errorOccured = false;
            MapChip instance = null;
            try
            {
                instance = new MapChip();
                instance.Value = id;
            }
            catch (PropertyOutOfRangeException)
            {
                errorOccured = true;
            }
            
            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
            
            if(errorOccured) return;
            
            // インスタンスの値と設定値が一致すること
            Assert.AreEqual(instance.Value, id);
        }

        [TestCase(-1, true)]
        [TestCase(0, false)]
        [TestCase(99999, false)]
        [TestCase(100000, false)]
        [TestCase(1604444, false)]
        [TestCase(1604445, true)]
        public static void ExplicitFromIntTest(int id, bool isError)
        {
            var errorOccured = false;
            MapChip instance = null;
            try
            {
                instance = (MapChip) id;
            }
            catch (PropertyOutOfRangeException)
            {
                errorOccured = true;
            }
            
            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
            
            if(errorOccured) return;
            
            // インスタンスの値と設定値が一致すること
            Assert.AreEqual(instance.Value, id);
        }

        [TestCase(-1, true)]
        [TestCase(0, false)]
        [TestCase(99999, false)]
        [TestCase(100000, false)]
        [TestCase(1604444, false)]
        [TestCase(1604445, true)]
        public static void SetValueTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var instance = new MapChip();
                instance.Value = value;
            }
            catch (PropertyOutOfRangeException)
            {
                errorOccured = true;
            }
            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
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
            catch (PropertyAccessException)
            {
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
            catch (PropertyAccessException)
            {
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
            catch (PropertyAccessException)
            {
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
            catch (PropertyAccessException)
            {
                errorOccured = true;
            }
            
            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;
            
            // オートタイル種別が一致すること
            Assert.AreEqual(result, type);
        }

        private static readonly object[] LeftUpAutoTileSetTestCaseSource =
        {
            new object[] { 103333, AutoTilePartType.ConnectionCentral, false, 100333},
            new object[] { 102344, AutoTilePartType.ConnectionVertical, false, 101344},
            new object[] { 1500000, AutoTilePartType.ConnectionHorizontal, false, 1502000},
            new object[] { 701234, AutoTilePartType.ConnectionOutSide, false, 703234},
            new object[] { 701234, AutoTilePartType.SurroundingFilled, false, 704234},
            new object[] { 20, AutoTilePartType.ConnectionOutSide, true, 0},
            new object[] { 20, null, true, 0},
        };
        [TestCaseSource(nameof(LeftUpAutoTileSetTestCaseSource))]
        public static void LeftUpAutoTileSetTest(int defaultId, 
            AutoTilePartType type, bool isError, int resultId)
        {
            var instance = (MapChip) defaultId;

            var errorOccured = false;
            try
            {
                instance.LeftUpAutoTile = type;
            }
            catch (PropertyAccessException)
            {
                errorOccured = true;
            }
            
            // エラー発生フラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;
            
            // 結果値が一致すること
            Assert.AreEqual(instance.Value, resultId);
        }

        private static readonly object[] RightUpAutoTileSetTestCaseSource =
        {
            new object[] { 103333, AutoTilePartType.ConnectionCentral, false, 103033},
            new object[] { 102344, AutoTilePartType.ConnectionVertical, false, 102144},
            new object[] { 1500000, AutoTilePartType.ConnectionHorizontal, false, 1500200},
            new object[] { 702222, AutoTilePartType.ConnectionOutSide, false, 702322},
            new object[] { 702222, AutoTilePartType.SurroundingFilled, false, 702422},
            new object[] { 20, AutoTilePartType.ConnectionOutSide, true, 0},
            new object[] { 20, null, true, 0},
        };
        [TestCaseSource(nameof(RightUpAutoTileSetTestCaseSource))]
        public static void RightUpAutoTileSetTest(int defaultId, 
            AutoTilePartType type, bool isError, int resultId)
        {
            var instance = (MapChip) defaultId;

            var errorOccured = false;
            try
            {
                instance.RightUpAutoTile = type;
            }
            catch (PropertyAccessException)
            {
                errorOccured = true;
            }
            
            // エラー発生フラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;
            
            // 結果値が一致すること
            Assert.AreEqual(instance.Value, resultId);
        }

        private static readonly object[] LeftDownAutoTileSetTestCaseSource =
        {
            new object[] { 103333, AutoTilePartType.ConnectionCentral, false, 103303},
            new object[] { 102344, AutoTilePartType.ConnectionVertical, false, 102314},
            new object[] { 1500000, AutoTilePartType.ConnectionHorizontal, false, 1500020},
            new object[] { 701111, AutoTilePartType.ConnectionOutSide, false, 701131},
            new object[] { 701234, AutoTilePartType.SurroundingFilled, false, 701244},
            new object[] { 20, AutoTilePartType.ConnectionOutSide, true, 0},
            new object[] { 20, null, true, 0},
        };
        [TestCaseSource(nameof(LeftDownAutoTileSetTestCaseSource))]
        public static void LeftDownAutoTileSetTest(int defaultId, 
            AutoTilePartType type, bool isError, int resultId)
        {
            var instance = (MapChip) defaultId;

            var errorOccured = false;
            try
            {
                instance.LeftDownAutoTile = type;
            }
            catch (PropertyAccessException)
            {
                errorOccured = true;
            }
            
            // エラー発生フラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;
            
            // 結果値が一致すること
            Assert.AreEqual(instance.Value, resultId);
        }

        private static readonly object[] RightDownAutoTileSetTestCaseSource =
        {
            new object[] { 103333, AutoTilePartType.ConnectionCentral, false, 103330},
            new object[] { 102344, AutoTilePartType.ConnectionVertical, false, 102341},
            new object[] { 1500000, AutoTilePartType.ConnectionHorizontal, false, 1500002},
            new object[] { 701234, AutoTilePartType.ConnectionOutSide, false, 701233},
            new object[] { 701111, AutoTilePartType.SurroundingFilled, false, 701114},
            new object[] { 20, AutoTilePartType.ConnectionOutSide, true, 0},
            new object[] { 20, null, true, 0},
        };
        [TestCaseSource(nameof(RightDownAutoTileSetTestCaseSource))]
        public static void RightDownAutoTileSetTest(int defaultId, 
            AutoTilePartType type, bool isError, int resultId)
        {
            var instance = (MapChip) defaultId;

            var errorOccured = false;
            try
            {
                instance.RightDownAutoTile = type;
            }
            catch (PropertyAccessException)
            {
                errorOccured = true;
            }
            
            // エラー発生フラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;
            
            // 結果値が一致すること
            Assert.AreEqual(instance.Value, resultId);
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
    }
}