using System;
using NUnit.Framework;
using WodiLib.Event;
using WodiLib.Map;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class MapEventPageGraphicInfoTest
    {
        [TestCase(false, -1, true)]
        [TestCase(true, -1, true)]
        [TestCase(false, 0, true)]
        [TestCase(true, 0, false)]
        [TestCase(false, 99999, true)]
        [TestCase(true, 99999, false)]
        [TestCase(false, 100000, true)]
        [TestCase(true, 100000, true)]
        public static void GraphicTileIdSetTest(bool isGraphicTileChip, int tileId, bool isError)
        {
            var instance = new MapEventPageGraphicInfo();
            var errorOccured = false;
            try
            {
                instance.IsGraphicTileChip = isGraphicTileChip;
                instance.GraphicTileId = tileId;
            }
            catch (Exception)
            {
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(false, "", false)]
        [TestCase(true, "", true)]
        [TestCase(false, "abc", false)]
        [TestCase(true, "abc", true)]
        [TestCase(false, null, true)]
        [TestCase(true, null, true)]
        public static void CharaChipFileNameSetTest(bool isGraphicTileChip, string fileName, bool isError)
        {
            var instance = new MapEventPageGraphicInfo();
            var errorOccured = false;
            try
            {
                instance.IsGraphicTileChip = isGraphicTileChip;
                instance.CharaChipFileName = fileName;
            }
            catch (Exception)
            {
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void CharaChipDrawTypeTest(bool isNull, bool isError)
        {
            var instance = new MapEventPageGraphicInfo();
            var errorOccured = false;
            try
            {
                instance.CharaChipDrawType = isNull ? null : PictureDrawType.Add;
            }
            catch (Exception)
            {
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(-1, false, true)]
        [TestCase(-1, true, true)]
        [TestCase(0, false, false)]
        [TestCase(0, true, false)]
        [TestCase(99999, false, false)]
        [TestCase(99999, true, false)]
        [TestCase(100000, false, true)]
        [TestCase(100000, true, true)]
        public static void SetGraphicTileIdTest(int tileId, bool beforeIsGraphicTileChip, bool isError)
        {
            var instance = new MapEventPageGraphicInfo();
            instance.IsGraphicTileChip = beforeIsGraphicTileChip;
            var errorOccured = false;
            try
            {
                instance.SetGraphicTileId(tileId);
            }
            catch (Exception)
            {
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (!errorOccured)
            {
                // タイルチップ使用フラグがtrueであること
                Assert.IsTrue(instance.IsGraphicTileChip);
            }
        }

        [TestCase(null, false, true)]
        [TestCase(null, true, true)]
        [TestCase("", false, false)]
        [TestCase("", true, false)]
        [TestCase("CharaChip/Hero.png", false, false)]
        [TestCase("CharaChip/Hero.png", true, false)]
        public static void SetGraphicFileNameTest(string fileName, bool beforeIsGraphicTileChip, bool isError)
        {
            var instance = new MapEventPageGraphicInfo();
            instance.IsGraphicTileChip = beforeIsGraphicTileChip;
            var errorOccured = false;
            try
            {
                instance.SetGraphicFileName(fileName);
            }
            catch (Exception)
            {
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
            
            if (!errorOccured)
            {
                // タイルチップ使用フラグがfalseであること
                Assert.IsFalse(instance.IsGraphicTileChip);
            }

        }
    }
}