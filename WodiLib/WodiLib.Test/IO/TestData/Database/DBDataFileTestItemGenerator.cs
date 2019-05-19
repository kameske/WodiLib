using System.Collections.Generic;
using System.IO;
using WodiLib.Database;

namespace WodiLib.Test.IO
{
    public static class DBDataFileTestItemGenerator
    {
        public static DBData GenerateUDB0DBData()
        {
            var result = new DBData();
            result.DataDescList.Overwrite(0, new List<DatabaseDataDesc>
            {
                // データ1
                new DatabaseDataDesc
                {
                    DataName = "7",
                    ItemValueList =
                    {
                        (DBValueInt) 6,
                        (DBValueString) "7",
                        (DBValueString) "MapData/Map002.mps",
                        (DBValueString) "",
                        (DBValueInt) 0,
                        (DBValueInt) (-3),
                        (DBValueInt) 9,
                    }
                },
                // データ2
                new DatabaseDataDesc
                {
                    DataName = "うでぃた",
                    ItemValueList =
                    {
                        (DBValueInt) 0,
                        (DBValueString) "うでぃた",
                        (DBValueString) "",
                        (DBValueString) "",
                        (DBValueInt) 0,
                        (DBValueInt) 0,
                        (DBValueInt) 9,
                    }
                },
                // データ3
                new DatabaseDataDesc
                {
                    DataName = "",
                    ItemValueList =
                    {
                        (DBValueInt) 0,
                        (DBValueString) "",
                        (DBValueString) "まっぷでーた",
                        (DBValueString) "Map007.mps",
                        (DBValueInt) 0,
                        (DBValueInt) (-1),
                        (DBValueInt) 3,
                    }
                },
            });
            return result;
        }

        public static DBData GenerateCDB0DBData()
        {
            var result = new DBData();
            result.DataDescList.Overwrite(0, new List<DatabaseDataDesc>
            {
                // データ0
                new DatabaseDataDesc
                {
                    DataName = "a",
                    ItemValueList =
                    {
                        (DBValueString) "",
                        (DBValueInt) 255,
                        (DBValueString) "",
                        (DBValueString) "CharaChip/[Animal]ChickenTX.png",
                        (DBValueInt) 122,
                        (DBValueInt) 8,
                        (DBValueInt) 6,
                        (DBValueInt) 1,
                        (DBValueString) "234",
                    }
                },
            });
            return result;
        }


        /** ========================================
         *  テスト用ファイル出力
         *  ======================================== */

        /// <summary>テストディレクトリルート</summary>
        public static string TestWorkRootDir => MapFileTestItemGenerator.TestWorkRootDir;

        /// <summary>テストファイルデータ</summary>
        public static readonly IEnumerable<(string, byte[])> TestFiles = new List<(string, byte[])>
        {
            ("UDB0_データ_001to003_7.dbdata", TestResources.UDB0_1to3DBData),
            ("あいうえお_データ_000to000_a.dbdata", TestResources.CDB0_0to0DBData),
            ("┣ 主人公行動AI_データ_003to018_.dbdata", TestResources.CDB2_3to18DBData),
            ("状態設定_データ_000to023_戦闘不能.dbdata", TestResources.UDB8_0to23DBData),
        };

        /// <summary>
        /// ファイルを tmp フォルダに出力する。
        /// </summary>
        public static void OutputFile()
        {
            if (!Directory.Exists(TestWorkRootDir))
            {
                Directory.CreateDirectory(TestWorkRootDir);
            }

            foreach (var (fileName, bytes) in TestFiles)
            {
                using (var fs = new FileStream(MakeFileFullPath(fileName), FileMode.Create))
                {
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
        }

        /// <summary>
        /// ファイルを削除する。
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
    }
}