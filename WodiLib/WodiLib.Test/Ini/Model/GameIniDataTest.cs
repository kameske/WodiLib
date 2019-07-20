using System;
using NUnit.Framework;
using WodiLib.Ini;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Ini.Model
{
    [TestFixture]
    public class GameIniDataTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [TestCase(1)]
        public static void StartCodeTest(int code)
        {
            var instance = new GameIniData();

            var errorOccured = false;
            try
            {
                instance.StartCode = code;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            var setValue = instance.StartCode;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.Equals(code));
        }

        [TestCase(false)]
        public static void IsSoftGraphicModeTest(bool isSoftGraphicMode)
        {
            var instance = new GameIniData();

            var errorOccured = false;
            try
            {
                instance.IsSoftGraphicMode = isSoftGraphicMode;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            var setValue = instance.IsSoftGraphicMode;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.Equals(isSoftGraphicMode));
        }

        [TestCase(true)]
        public static void IsWindowModeTest(bool isWindowMode)
        {
            var instance = new GameIniData();

            var errorOccured = false;
            try
            {
                instance.IsWindowMode = isWindowMode;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            var setValue = instance.IsWindowMode;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.Equals(isWindowMode));
        }

        [TestCase(true)]
        public static void IsPlayBgmTest(bool isPlayBgm)
        {
            var instance = new GameIniData();

            var errorOccured = false;
            try
            {
                instance.IsPlayBgm = isPlayBgm;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            var setValue = instance.IsPlayBgm;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.Equals(isPlayBgm));
        }

        [TestCase(true)]
        public static void IsPlaySeTest(bool isPlaySe)
        {
            var instance = new GameIniData();

            var errorOccured = false;
            try
            {
                instance.IsPlaySe = isPlaySe;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            var setValue = instance.IsPlaySe;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.Equals(isPlaySe));
        }

        private static readonly object[] FrameSkipTypeTestCaseSource =
        {
            new object[] {null, true},
            new object[] {FrameSkipType.HighSpec, false},
        };

        [TestCaseSource(nameof(FrameSkipTypeTestCaseSource))]
        public static void FrameSkipTypeTest(FrameSkipType type, bool isError)
        {
            var instance = new GameIniData();

            var errorOccured = false;
            try
            {
                instance.FrameSkipType = type;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            var setValue = instance.FrameSkipType;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.Equals(type));
        }

        private static readonly object[] ProxyAddressTestCaseSource =
        {
            new object[] {null, true},
            new object[] {(ProxyAddress)"address", false},
        };

        [TestCaseSource(nameof(ProxyAddressTestCaseSource))]
        public static void ProxyAddressTest(ProxyAddress address, bool isError)
        {
            var instance = new GameIniData();

            var errorOccured = false;
            try
            {
                instance.ProxyAddress = address;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            var setValue = instance.ProxyAddress;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.Equals(address));
        }

        private static readonly object[] ProxyPortTestCaseSource =
        {
            new object[] {(ProxyPort)222},
        };

        [TestCaseSource(nameof(ProxyPortTestCaseSource))]
        public static void ProxyPortTest(ProxyPort port)
        {
            var instance = new GameIniData();

            var errorOccured = false;
            try
            {
                instance.ProxyPort = port;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            var setValue = instance.ProxyPort;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.Equals(port));
        }

        [TestCase(false)]
        public static void CanTakeScreenShotTest(bool canTakeScreenShot)
        {
            var instance = new GameIniData();

            var errorOccured = false;
            try
            {
                instance.CanTakeScreenShot = canTakeScreenShot;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            var setValue = instance.CanTakeScreenShot;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.Equals(canTakeScreenShot));
        }

        [TestCase(false)]
        public static void CanResetTest(bool canReset)
        {
            var instance = new GameIniData();

            var errorOccured = false;
            try
            {
                instance.CanReset = canReset;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            var setValue = instance.CanReset;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.Equals(canReset));
        }

        private static readonly object[] DisplayNumberTestCaseSource =
        {
            new object[] {(DisplayNumber)0},
        };

        [TestCaseSource(nameof(DisplayNumberTestCaseSource))]
        public static void DisplayNumberTest(DisplayNumber displayNumber)
        {
            var instance = new GameIniData();

            var errorOccured = false;
            try
            {
                instance.DisplayNumber = displayNumber;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            var setValue = instance.DisplayNumber;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.Equals(displayNumber));
        }

        [TestCase(false)]
        public static void IsUseOldDirectXTest(bool isUseOldDirectX)
        {
            var instance = new GameIniData();

            var errorOccured = false;
            try
            {
                instance.IsUseOldDirectX = isUseOldDirectX;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            var setValue = instance.IsUseOldDirectX;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.Equals(isUseOldDirectX));
        }

    }
}