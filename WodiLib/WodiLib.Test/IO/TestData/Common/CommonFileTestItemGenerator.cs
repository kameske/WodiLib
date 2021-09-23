using System.Collections.Generic;
using System.IO;
using WodiLib.Common;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    /// <summary>
    ///     CommonFileReader/Writerテスト時に比較するためのアイテムを生成する。
    /// </summary>
    public static class CommonFileTestItemGenerator
    {
        /**
         * ========================================
         * マップデータオブジェクト作成
         * ========================================
         */
        public static CommonFileData GenerateCommon000Data()
        {
            var commonEventList = new List<CommonEvent>
            {
                CommonEventDataFileTestItemGenerator.CommonEvent00Data.GenerateCommonEvent000()
            };

            var result = new CommonFileData();

            result.SetCommonEventList(commonEventList);

            return result;
        }

        public static CommonFileData GenerateCommon003To005Data()
        {
            var commonEventList = new List<CommonEvent>
            {
                CommonEventDataFileTestItemGenerator.CommonEvent00Data.GenerateCommonEvent003(),
                CommonEventDataFileTestItemGenerator.CommonEvent00Data.GenerateCommonEvent004(),
                CommonEventDataFileTestItemGenerator.CommonEvent00Data.GenerateCommonEvent005()
            };

            var result = new CommonFileData();

            result.SetCommonEventList(commonEventList);

            return result;
        }

        public static CommonFileData GenerateCommon005To006Data()
        {
            var commonEventList = new List<CommonEvent>
            {
                CommonEventDataFileTestItemGenerator.CommonEvent00Data.GenerateCommonEvent005(),
                CommonEventDataFileTestItemGenerator.CommonEvent00Data.GenerateCommonEvent006()
            };

            var result = new CommonFileData();

            result.SetCommonEventList(commonEventList);

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
            ("000_コモンイベント000.common", TestResources.c000_コモンイベント000),
            ("Common003to005_コモンイベント003.common", TestResources.Common003to005_コモンイベント003),
            ("Common005to006_コモンイベント005.common", TestResources.Common005to006_コモンイベント005),
            ("各種メニュー呼び出し.common", TestResources.各種メニュー呼出)
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
