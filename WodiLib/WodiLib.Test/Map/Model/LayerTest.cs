using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using WodiLib.Map;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class LayerTest
    {
        [Test]
        public static void PropertyTest()
        {
            var layer = new Layer();
            layer.SetChips(GenerateTestChipsData(20, 15));

            var _ = layer.Chips;
            // ここまでの処理でエラーにならないこと
            Assert.True(true);

            // 幅、高さが正しく取得できること
            Assert.AreEqual(layer.Width, 20);
            Assert.AreEqual(layer.Height, 15);
        }

        [TestCase(-1, true)]
        [TestCase(19, true)]
        [TestCase(20, false)]
        [TestCase(30, false)]
        [TestCase(35, false)]
        public static void SetWidthTest(int width, bool isError)
        {
            var layer = new Layer();
            layer.SetChips(GenerateTestChipsData(30, 15));

            var errorOccured = false;
            try
            {
                layer.UpdateWidth(width);
            }
            catch (ArgumentOutOfRangeException)
            {
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (!errorOccured)
            {
                // プロパティのマップサイズ横が一致すること
                Assert.AreEqual(layer.Width, width);
            }
        }

        [TestCase(-1, true)]
        [TestCase(14, true)]
        [TestCase(15, false)]
        [TestCase(20, false)]
        [TestCase(30, false)]
        public static void SetHeightTest(int height, bool isError)
        {
            var layer = new Layer();
            layer.SetChips(GenerateTestChipsData(20, 20));

            var errorOccured = false;
            try
            {
                layer.UpdateHeight(height);
            }
            catch (ArgumentOutOfRangeException)
            {
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (!errorOccured)
            {
                // プロパティのマップサイズ横が一致すること
                Assert.AreEqual(layer.Height, height);
            }
        }

        private static readonly object[] SetChipTestCaseSource =
        {
            new object[] {2, 4, (MapChip)322, false},
            new object[] {19, 4, (MapChip)322, false},
            new object[] {20, 4, (MapChip)322, true},
            new object[] {2, 14, (MapChip)322, false},
            new object[] {2, 15, (MapChip)322, true},
            new object[] {2, 4, null, true},
        };
        [TestCaseSource(nameof(SetChipTestCaseSource))]
        public static void SetChipTest(int x, int y, MapChip chip, bool isError)
        {
            var layer = new Layer();
            layer.SetChips(GenerateTestChipsData(20, 15));

            var errorOccured = false;
            try
            {
                layer.SetChip(x, y, chip);
            }
            catch (ArgumentNullException)
            {
                errorOccured = true;
            }
            catch (ArgumentOutOfRangeException)
            {
                errorOccured = true;
            }
            // エラー発生フラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (!errorOccured)
            {
                // チップ番号値が指定した値になっていること
                var result = layer.GetChip(x, y);
                Assert.AreEqual((int)result, (int)chip);
            }
        }

        [TestCase(4, 2, false)]
        [TestCase(19, 2, false)]
        [TestCase(20, 2, true)]
        [TestCase(4, 14, false)]
        [TestCase(4, 15, true)]
        public static void GetChipTest(int x, int y, bool isError)
        {
            var layer = new Layer();
            layer.SetChips(GenerateTestChipsData(20, 15));

            var errorOccured = false;
            MapChip getChip = MapChip.Default;
            try
            {
                getChip = layer.GetChip(x, y);
            }
            catch (ArgumentOutOfRangeException)
            {
                errorOccured = true;
            }
            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (!errorOccured)
            {
                // 座標の値が初期化した値になっていること
                Assert.AreEqual((int)getChip, x * 10 + y);
            }
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