using System.Collections.Generic;
using System.IO;
using WodiLib.Ini;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    public static class GameIniDataTestItemGenerator
    {
        #region CreateGameIniData

        /**
         * ========================================
         * GameIniオブジェクト作成
         * ========================================
         */
        public static GameIniData GenerateData0()
        {
            return new GameIniData
            {
                StartCode = 0,
                IsSoftGraphicMode = false,
                IsWindowMode = true,
                IsPlayBgm = true,
                IsPlaySe = true,
                FrameSkipType = FrameSkipType.FromCode("0"),
                ProxyAddress = "2001:a453:69:1111:54ff:15:1111:2222",
                ProxyPort = -1,
                CanTakeScreenShot = true,
                CanReset = true,
                DisplayNumber = 0,
                IsUseOldDirectX = false
            };
        }

        #endregion

        #region OutputTestFile

        /// <summary>テストディレクトリルート</summary>
        public static string TestWorkRootDir => $@"{Path.GetTempPath()}WodiLibTest";

        /// <summary>テストファイルデータ</summary>
        public static readonly IEnumerable<(string, byte[])> TestFiles = new List<(string, byte[])>
        {
            (@"Dir0\Game.ini", TestResources.GameIni0),
            (@"Dir1\Game.ini", TestResources.GameIni1)
        };

        /// <summary>
        ///     テストデータファイルを tmp フォルダに出力する。
        /// </summary>
        public static void OutputFile()
        {
            TestWorkRootDir.CreateDirectoryIfNeed();

            foreach (var (fileName, bytes) in TestFiles)
            {
                var filePath = MakeFileFullPath(fileName);
                Path.GetDirectoryName(filePath).CreateDirectoryIfNeed();

                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
        }

        /// <summary>
        ///     テストデータファイルを削除する。
        /// </summary>
        public static void DeleteFile()
        {
            foreach (var (fileName, _) in TestFiles)
            {
                var fileFullPath = MakeFileFullPath(fileName);
                if (!File.Exists(fileFullPath)) continue;
                try
                {
                    File.Delete(fileFullPath);
                }
                catch
                {
                    // 削除に失敗しても何もしない
                }
            }
        }

        private static string MakeFileFullPath(string fileName)
        {
            return $@"{TestWorkRootDir}\{fileName}";
        }

        #endregion
    }
}
