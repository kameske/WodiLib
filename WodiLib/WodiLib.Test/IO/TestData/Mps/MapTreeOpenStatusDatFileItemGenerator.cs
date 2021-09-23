using System.Collections.Generic;
using System.IO;
using WodiLib.Map;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    public static class MapTreeOpenStatusDataFileItemGenerator
    {
        #region CreateMapTreeData

        /**
         * ========================================
         * マップツリーデータオブジェクト作成
         * ========================================
         */
        public static MapTreeOpenStatusData GenerateData0()
        {
            return new MapTreeOpenStatusData
            {
                StatusList = new MapTreeOpenStatusList(new[]
                {
                    MapTreeOpenState.Close,
                    MapTreeOpenState.Open,
                    MapTreeOpenState.Close,
                    MapTreeOpenState.Close,
                    MapTreeOpenState.Open,
                    MapTreeOpenState.Close,
                    MapTreeOpenState.Close,
                    MapTreeOpenState.Close
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
            (@"Dir0\MapTreeOpenStatus.dat", TestResources.MapTreeOpenStatus0),
            (@"Dir1\MapTreeOpenStatus.dat", TestResources.MapTreeOpenStatus1)
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
