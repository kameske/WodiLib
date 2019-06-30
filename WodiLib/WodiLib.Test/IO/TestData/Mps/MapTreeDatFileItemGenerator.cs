using System.Collections.Generic;
using System.IO;
using WodiLib.Map;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    public static class MapTreeDataFileItemGenerator
    {
        #region CreateMapTreeData

        /** ========================================
         *  マップツリーデータオブジェクト作成
         *  ======================================== */

        public static MapTreeData GenerateData0()
        {
            return new MapTreeData
            {
                TreeNodeList = new MapTreeNodeList(new[]
                {
                    new MapTreeNode(0, -1),
                    new MapTreeNode(1, 4),
                    new MapTreeNode(2, 0),
                    new MapTreeNode(3, 1),
                    new MapTreeNode(4, -1),
                    new MapTreeNode(5, 4),
                    new MapTreeNode(6, 1),
                    new MapTreeNode(7, 6),
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
            (@"Dir0\MapTree.dat", TestResources.MapTree0),
            (@"Dir1\MapTree.dat", TestResources.MapTree1),
        };

        /// <summary>
        /// テストデータファイルを tmp フォルダに出力する。
        /// </summary>
        public static void OutputFile()
        {
            TestWorkRootDir.CreateDirectoryIfNeed();

            foreach (var (fileName, bytes) in TestFiles)
            {
                var fileFullPath = MakeFileFullPath(fileName);
                Path.GetDirectoryName(fileFullPath).CreateDirectoryIfNeed();

                using (var fs = new FileStream(fileFullPath, FileMode.Create))
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