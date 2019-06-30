using System.Collections.Generic;
using System.IO;
using WodiLib.Map;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    public static class TileSetDataTestItemGenerator
    {
        #region TileSetData

        /** ========================================
         *  タイルセットデータオブジェクト作成
         *  ======================================== */

        public static TileSetData GenerateData0()
        {
            return new TileSetData
            {
                TileSetSettingList = new TileSetSettingList(new List<TileSetSetting>
                {
                    TileSetFileDataTestItemGenerator.GenerateSettingData0(),
                    TileSetFileDataTestItemGenerator.GenerateSettingData1(),
                })
            };
        }

        #endregion

        #region OutputTestFile

        /// <summary>テストディレクトリルート</summary>
        public static string TestWorkRootDir => $@"{Path.GetTempPath()}WodiLibTest";

        /// <summary>テストファイルデータ</summary>
        public static readonly IEnumerable<(string, byte[])> TestFiles = new List<(string, byte[])>
        {
            (@"Dir0\TileSetData.dat", TestResources.TileSetData0),
            (@"Dir1\TileSetData.dat", TestResources.TileSetData1),
        };

        /// <summary>
        /// テストデータファイルを tmp フォルダに出力する。
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
        /// テストデータファイルを削除する。
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