using System;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Event;
using WodiLib.Map;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class MapEventPageGraphicInfoTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [TestCase(false, 0, true)]
        [TestCase(true, 0, false)]
        [TestCase(false, 9999, true)]
        [TestCase(true, 9999, false)]
        public static void GraphicTileIdSetTest(bool isGraphicTileChip, int tileId, bool isError)
        {
            var instance = new MapEventPageGraphicInfo();
            var graphicTileId = (MapEventTileId) tileId;
            var errorOccured = false;
            try
            {
                instance.IsGraphicTileChip = isGraphicTileChip;
                instance.GraphicTileId = graphicTileId;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(false, "abc", false)]
        [TestCase(true, "abc", true)]
        public static void CharaChipFileNameSetTest(bool isGraphicTileChip, string fileName, bool isError)
        {
            var instance = new MapEventPageGraphicInfo();
            var errorOccured = false;
            var charaChipName = (CharaChipFileName) fileName;
            try
            {
                instance.IsGraphicTileChip = isGraphicTileChip;
                instance.CharaChipFileName = charaChipName;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
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
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(-1, false, false)]
        [TestCase(-1, true, false)]
        [TestCase(0, false, true)]
        [TestCase(0, true, true)]
        public static void SetGraphicTileIdTest(int tileId, bool beforeIsGraphicTileChip, bool afterIsGraphicTileChip)
        {
            var instance = new MapEventPageGraphicInfo();
            instance.IsGraphicTileChip = beforeIsGraphicTileChip;
            var mapEventTileId = (MapEventTileId) tileId;
            var errorOccured = false;
            try
            {
                instance.SetGraphicTileId(mapEventTileId);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // タイルチップ使用フラグが一致すること
            Assert.AreEqual(instance.IsGraphicTileChip, afterIsGraphicTileChip);
        }

        [TestCase("CharaChip/Hero.png", false, false)]
        [TestCase("CharaChip/Hero.png", true, false)]
        public static void SetGraphicFileNameTest(string fileName, bool beforeIsGraphicTileChip, bool isError)
        {
            var instance = new MapEventPageGraphicInfo();
            instance.IsGraphicTileChip = beforeIsGraphicTileChip;
            var charaChipFileName = (CharaChipFileName) fileName;
            var errorOccured = false;
            try
            {
                instance.SetGraphicFileName(charaChipFileName);
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
                // タイルチップ使用フラグがfalseであること
                Assert.IsFalse(instance.IsGraphicTileChip);
            }
        }
    }
}