using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using WodiLib.Test.IO;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Project
{
    /// <summary>
    /// WoditorProjectテスト時に読み込むためのデータをリソースから復元する。
    /// </summary>
    public static class ProjectFileTestItemGenerator
    {
        /// <summary>テストディレクトリルート</summary>
        public static string TestWorkRootDir => MapFileTestItemGenerator.TestWorkRootDir;

        public static readonly IEnumerable<(string, byte[])> TestProjectZips = new[]
        {
            ("TestProject_Ver224.zip", TestResources.TestProject_Ver224Zip),
        };

        public static readonly IEnumerable<(string, byte[])> EventCommandSentences = new[]
        {
            ("EventCommandSentence/Ver224_CommonEvent.txt", TestResources.EventCommandSentence_Ver224_CommonEvent),
            ("EventCommandSentence/Ver224_MapEvent.txt", TestResources.EventCommandSentence_Ver224_MapEvent),
        };

        public static readonly List<EventCommandSentenceTestInfo> TestInfoList = new List<EventCommandSentenceTestInfo>
        {
            new EventCommandSentenceTestInfo
            {
                ProjectDir = "TestProject_Ver224",
                CommonEventInfo = new EventCommandSentenceTestInfo.CommonEvent
                {
                    CommonEventId = 217,
                    MpsFilePath = "Data/MapData/TitleMap.mps",
                    EventCommandSentenceAnswerFilePath = "EventCommandSentence/Ver224_CommonEvent.txt",
                },
                MapEventInfo = new EventCommandSentenceTestInfo.MapEvent
                {
                    FilePath = "Data/MapData/TitleMap.mps",
                    MapEventId = 1,
                    PageIndex = 0,
                    EventCommandSentenceAnswerFilePath = "EventCommandSentence/Ver224_MapEvent.txt"
                }
            }
        };

        /// <summary>
        /// プロジェクトディレクトリ、イベントコマンド比較用テキストファイルを出力する。
        /// </summary>

        public static void OutputProjectItem()
        {
            TestWorkRootDir.CreateDirectoryIfNeed();

            foreach (var (fileName, bytes) in TestProjectZips)
            {
                var zipFileFullPath = MakeZipFileFullPath(fileName);

                using (var fs = new FileStream(MakeZipFileFullPath(fileName), FileMode.Create))
                {
                    fs.Write(bytes, 0, bytes.Length);
                }

                var outputRootDir = MakeOutputDirFullPath("");

                try
                {
                    using (var zip = ZipFile.Open(zipFileFullPath, ZipArchiveMode.Read))
                    {
                        zip.ExtractToDirectory(outputRootDir);
                    }
                }
                catch
                {
                    // すでに存在する場合は何もしない
                }
            }

            $"{TestWorkRootDir}/EventCommandSentence".CreateDirectoryIfNeed();
            foreach (var (fileName, bytes) in EventCommandSentences)
            {
                using (var fs = new FileStream(MakeFileFullPath(fileName), FileMode.Create))
                {
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
        }

        /// <summary>
        /// プロジェクトディレクトリ、および復元元のZipファイルを削除する
        /// </summary>
        public static void DeleteProjectItem()
        {
            foreach (var (fileName, _) in TestProjectZips)
            {
                var fileFullPath = MakeZipFileFullPath(fileName);
                if (!File.Exists(fileFullPath)) continue;
                try
                {
                    File.Delete(fileFullPath);
                }
                catch
                {
                    // 削除に失敗しても何もしない
                }

                var outputRootDir = MakeOutputDirFullPath(Regex.Replace(fileName, "(.*).zip", "$1"));
                try
                {
                    DeleteRecursive(outputRootDir);
                }
                catch
                {
                    // 削除に失敗しても何もしない
                }
            }

            foreach (var (fileName, _) in  EventCommandSentences)
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

        private static string MakeZipFileFullPath(string fileName)
        {
            return $@"{TestWorkRootDir}\{fileName}";
        }

        private static string MakeOutputDirFullPath(string dir)
        {
            return $@"{TestWorkRootDir}\{dir}";
        }

        private static void DeleteRecursive(string targetDirectoryPath){
            if (!Directory.Exists (targetDirectoryPath)) {
                return;
            }

            // ファイル
            var filePaths = Directory.GetFiles(targetDirectoryPath);
            foreach (var filePath in filePaths){
                File.SetAttributes(filePath, FileAttributes.Normal);
                File.Delete(filePath);
            }

            // ディレクトリ
            var directoryPaths = Directory.GetDirectories(targetDirectoryPath);
            foreach (var directoryPath in directoryPaths){
                DeleteRecursive(directoryPath);
            }

            // 自分自身
            Directory.Delete(targetDirectoryPath, false);
        }
    }
}