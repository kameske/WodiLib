using System.Collections.Generic;
using System.IO;
using WodiLib.Common;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    /// <summary>
    ///     CommonEventDataFileReader/Writerテスト時に比較するためのアイテムを生成する。
    /// </summary>
    public static partial class CommonEventDataFileTestItemGenerator
    {
        /**
         * ========================================
         * マップデータオブジェクト作成
         * ========================================
         */
        public static CommonEventData GenerateCommonEvent00Data()
        {
            var commonEventList = new List<CommonEvent>
            {
                CommonEvent00Data.GenerateCommonEvent000(),
                CommonEvent00Data.GenerateCommonEvent001(),
                CommonEvent00Data.GenerateCommonEvent002(),
                CommonEvent00Data.GenerateCommonEvent003(),
                CommonEvent00Data.GenerateCommonEvent004(),
                CommonEvent00Data.GenerateCommonEvent005(),
                CommonEvent00Data.GenerateCommonEvent006(),
                CommonEvent00Data.GenerateCommonEvent007()
            };

            var result = new CommonEventData();

            result.CommonEventList = new CommonEventList(commonEventList);

            return result;
        }

        /// ========================================
        /// テスト用ファイル出力
        /// ========================================
        /// <summary>テストディレクトリルート</summary>
        public static string TestWorkRootDir => MapFileTestItemGenerator.TestWorkRootDir;

        /// <summary>テストファイルデータ</summary>
        public static readonly IEnumerable<(string, byte[])> TestFiles = new List<(string, byte[])>
        {
            ("CommonEvent_00.dat", TestResources.CommonEvent_00)
        };

        /// <summary>
        ///     マップファイルを tmp フォルダに出力する。
        /// </summary>
        public static void OutputFile()
        {
            TestWorkRootDir.CreateDirectoryIfNeed();

            foreach (var (fileName, bytes) in TestFiles)
            {
                using (var fs = new FileStream(MakeFileFullPath(fileName), FileMode.Create))
                {
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
        }

        /// <summary>
        ///     マップファイルを削除する。
        /// </summary>
        public static void DeleteMapFile()
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
    }
}
