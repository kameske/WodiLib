using System.Collections.Generic;
using System.IO;
using WodiLib.Database;

namespace WodiLib.Test.IO
{
    public static class DatabaseDatFileTestItemGenerator
    {
        #region CreateDatabaseDat

        public static DatabaseDat GenerateDataBaseDat0Data()
        {
            return new DatabaseDat
            {
                DBKind = DBKind.User,
                SettingList = new DBDataSettingList(new[]
                {
                    GenerateData0Type00Setting(),
                    GenerateData0Type01Setting(),
                    GenerateData0Type02Setting(),
                    GenerateData0Type03Setting(),
                })
            };
        }

        private static DBDataSetting GenerateData0Type00Setting()
        {
            var result = new DBDataSetting(DBDataSettingType.FirstStringData)
            {
                SettingValuesList = new DBItemValuesList(new List<List<DBItemValue>>
                {
                    new List<DBItemValue>
                    {
                        (DBValueInt) (-255),
                        (DBValueString) "文字列",
                        (DBValueString) "MapChip/[A]World_Grass-Grass_pipo.png",
                        (DBValueString) "Map002.mps",
                        (DBValueInt) 0,
                        (DBValueInt) (-2),
                        (DBValueInt) 3,
                    },
                    new List<DBItemValue>
                    {
                        (DBValueInt) 6,
                        (DBValueString) "7",
                        (DBValueString) "MapData/Map002.mps",
                        (DBValueString) "",
                        (DBValueInt) 0,
                        (DBValueInt) (-3),
                        (DBValueInt) 9,
                    },
                    new List<DBItemValue>
                    {
                        (DBValueInt) 0,
                        (DBValueString) "うでぃた",
                        (DBValueString) "",
                        (DBValueString) "",
                        (DBValueInt) 0,
                        (DBValueInt) 0,
                        (DBValueInt) 9,
                    },
                    new List<DBItemValue>
                    {
                        (DBValueInt) 0,
                        (DBValueString) "",
                        (DBValueString) "まっぷでーた",
                        (DBValueString) "Map007.mps",
                        (DBValueInt) 0,
                        (DBValueInt) (-1),
                        (DBValueInt) 3,
                    }
                })
            };
            return result;
        }

        private static DBDataSetting GenerateData0Type01Setting()
        {
            var result = new DBDataSetting(DBDataSettingType.Manual);
            return result;
        }

        private static DBDataSetting GenerateData0Type02Setting()
        {
            var result = new DBDataSetting(DBDataSettingType.DesignatedType, DBKind.User, 4)
            {
                SettingValuesList = new DBItemValuesList(new List<IReadOnlyList<DBItemValue>>
                {
                    new List<DBItemValue>
                    {
                        (DBValueString) "",
                        (DBValueString) "",
                        (DBValueString) "",
                        (DBValueString) "",
                        (DBValueString) "",
                    },
                    new List<DBItemValue>
                    {
                        (DBValueString) "",
                        (DBValueString) "",
                        (DBValueString) "",
                        (DBValueString) "",
                        (DBValueString) "",
                    },
                })
            };
            return result;
        }

        private static DBDataSetting GenerateData0Type03Setting()
        {
            var result = new DBDataSetting(DBDataSettingType.EqualBefore)
            {
                SettingValuesList = new DBItemValuesList(new List<IReadOnlyList<DBItemValue>>
                {
                    new List<DBItemValue>
                    {
                        (DBValueString) "ウルファール\r\nエディ\r\n夕一",
                        (DBValueInt) 33,
                        (DBValueInt) 20,
                    }
                })
            };
            return result;
        }

        #endregion

        #region CreateCDatabaseDat

        public static DatabaseDat GenerateCDatabaseData0Data()
        {
            return new DatabaseDat
            {
                DBKind = DBKind.Changeable,
                SettingList = new DBDataSettingList(new List<DBDataSetting>
                {
                    // CDB0
                    new DBDataSetting(DBDataSettingType.Manual)
                    {
                        SettingValuesList = new DBItemValuesList(new List<IReadOnlyList<DBItemValue>>
                        {
                            new List<DBItemValue>
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
                            },
                            new List<DBItemValue>
                            {
                                (DBValueString) "aaa",
                                (DBValueInt) 255,
                                (DBValueString) "aaa",
                                (DBValueString) "",
                                (DBValueInt) 0,
                                (DBValueInt) 0,
                                (DBValueInt) 0,
                                (DBValueInt) 0,
                                (DBValueString) "",
                            },
                            new List<DBItemValue>
                            {
                                (DBValueString) "",
                                (DBValueInt) 127,
                                (DBValueString) "",
                                (DBValueString) "",
                                (DBValueInt) 4,
                                (DBValueInt) 0,
                                (DBValueInt) 127,
                                (DBValueInt) 1,
                                (DBValueString) "",
                            },
                        })
                    },
                    // CDB1
                    new DBDataSetting(DBDataSettingType.DesignatedType, DBKind.Changeable, 4)
                    {
                        SettingValuesList = new DBItemValuesList(new List<IReadOnlyList<DBItemValue>>
                        {
                            new List<DBItemValue>
                            {
                                (DBValueString) "Wolf RPG Editor!",
                            },
                        })
                    },
                })
            };
        }

        #endregion

        /** ========================================
         *  テスト用ファイル出力
         *  ======================================== */

        /// <summary>テストディレクトリルート</summary>
        public static string TestWorkRootDir => MapFileTestItemGenerator.TestWorkRootDir;

        /// <summary>テストファイルデータ</summary>
        public static readonly IEnumerable<(string, byte[])> TestFiles = new List<(string, byte[])>
        {
            ("Database0.dat", TestResources.Database0Dat),
            ("CDatabase0.dat", TestResources.CDatabase0Dat),
            ("Database1.dat", TestResources.Database1Dat),
            ("CDatabase1.dat", TestResources.CDatabase1Dat),
            ("SysDatabase1.dat", TestResources.SysDatabase1Dat),
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