using System;
using System.Collections.Generic;
using NUnit.Framework;
using WodiLib.Map;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class LayerTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [Test]
        public static void PropertyTest()
        {
            var layer = new Layer
            {
                Chips = new MapChipList(GenerateTestChipsData(20, 15))
            };

            var _ = layer.Chips;
            // ここまでの処理でエラーにならないこと
            Assert.True(true);

            // 幅、高さが正しく取得できること
            Assert.AreEqual((int) layer.Width, 20);
            Assert.AreEqual((int) layer.Height, 15);
        }

        [TestCase(20)]
        [TestCase(30)]
        [TestCase(40)]
        public static void UpdateWidthTest(int sizeWidth)
        {
            var layer = new Layer
            {
                Chips = new MapChipList(GenerateTestChipsData(30, 15))
            };
            var width = (MapSizeWidth) sizeWidth;

            var errorOccured = false;
            try
            {
                layer.UpdateWidth(width);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // プロパティのマップサイズ横が一致すること
            Assert.AreEqual(layer.Width, width);
        }

        [TestCase(15)]
        [TestCase(20)]
        [TestCase(30)]
        public static void SetHeightTest(int heightSize)
        {
            var layer = new Layer
            {
                Chips = new MapChipList(GenerateTestChipsData(20, 20))
            };
            var height = (MapSizeHeight) heightSize;

            var errorOccured = false;
            try
            {
                layer.UpdateHeight(height);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // プロパティのマップサイズ横が一致すること
            Assert.AreEqual(layer.Height, height);
        }

        [Test]
        public static void UpdateSizeTest()
        {
            var instance = new MapChipList(GenerateTestChipsData(20, 20));

            var width = (MapSizeWidth) 30;
            var height = (MapSizeHeight) 24;

            var errorOccured = false;
            try
            {
                instance.UpdateSize(width, height);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // サイズが変化していること
            Assert.AreEqual(instance.Width, width);
            Assert.AreEqual(instance.Height, height);
        }

        private static readonly object[] SetChipTestCaseSource =
        {
            new object[] {2, 4, (MapChip) 322, false},
            new object[] {19, 4, (MapChip) 322, false},
            new object[] {20, 4, (MapChip) 322, true},
            new object[] {2, 14, (MapChip) 322, false},
            new object[] {2, 15, (MapChip) 322, true},
            // new object[] {2, 4, null, true},  /* MapChipを構造体にしたため、nullは設定不可能 */
        };

        [TestCaseSource(nameof(SetChipTestCaseSource))]
        public static void SetChipTest(int x, int y, MapChip chip, bool isError)
        {
            var layer = new Layer
            {
                Chips = new MapChipList(GenerateTestChipsData(20, 15))
            };

            var errorOccured = false;
            try
            {
                layer.Chips[x][y] = chip;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラー発生フラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // チップ番号値が指定した値になっていること
            var result = layer.Chips[x][y];
            Assert.AreEqual((int) result, (int) chip);
        }

        [TestCase(4, 2, false)]
        [TestCase(19, 2, false)]
        [TestCase(20, 2, true)]
        [TestCase(4, 14, false)]
        [TestCase(4, 15, true)]
        public static void GetChipTest(int x, int y, bool isError)
        {
            var layer = new Layer
            {
                Chips = new MapChipList(GenerateTestChipsData(20, 15))
            };

            var errorOccured = false;
            var getChip = MapChip.Default;
            try
            {
                getChip = layer.Chips[x][y];
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // 座標の値が初期化した値になっていること
            Assert.AreEqual((int) getChip, x * 10 + y);
        }

        [Test]
        public static void SerializeTest()
        {
            var target = new Layer
            {
                Chips = new MapChipList(GenerateTestChipsData(20, 15))
            };
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }


        private static List<List<MapChip>> GenerateTestChipsData(int width, int height)
        {
            var testChips = new List<List<MapChip>>();
            for (var i = 0; i < width; i++)
            {
                var testChipList = new List<MapChip>();
                for (var j = 0; j < height; j++)
                {
                    var chip = (MapChip) (i * 10 + j);
                    testChipList.Add(chip);
                }

                testChips.Add(testChipList);
            }

            return testChips;
        }
    }
}