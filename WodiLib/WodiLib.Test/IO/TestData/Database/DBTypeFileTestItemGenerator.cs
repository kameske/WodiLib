using System.Collections.Generic;
using System.IO;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    public static class DBTypeFileTestItemGenerator
    {
        public static DBType GenerateUDB0DBType()
        {
            var result = new DBType
            {
                TypeName = "UDB0",
                Memo = "",
                ItemDescList =
                {
                    // 項目0
                    new DatabaseItemDesc
                    {
                        ItemName = "設定項目0",
                        ItemType = DBItemType.Int,
                        SpecialSettingDesc = new DBItemSpecialSettingDesc(DBItemSpecialSettingType.Normal, null)
                        {
                            ItemMemo = "",
                            InitValue = 0
                        }
                    },
                    // 項目1
                    new DatabaseItemDesc
                    {
                        ItemName = "文字列項目",
                        ItemType = DBItemType.String,
                        SpecialSettingDesc = new DBItemSpecialSettingDesc(DBItemSpecialSettingType.Normal, null)
                        {
                            ItemMemo = ""
                        }
                    },
                    // 項目2
                    new DatabaseItemDesc
                    {
                        ItemName = "ファイル名設定1",
                        ItemType = DBItemType.String,
                        SpecialSettingDesc =
                            new DBItemSpecialSettingDesc(DBItemSpecialSettingType.LoadFile, null)
                            {
                                ItemMemo = "",
                                FolderName = "MapChip",
                                OmissionFolderNameFlag = false
                            }
                    },
                    // 項目3
                    new DatabaseItemDesc
                    {
                        ItemName = "ファイル名設定2",
                        ItemType = DBItemType.String,
                        SpecialSettingDesc =
                            new DBItemSpecialSettingDesc(DBItemSpecialSettingType.LoadFile, null)
                            {
                                ItemMemo = "",
                                FolderName = "MapData",
                                OmissionFolderNameFlag = true
                            }
                    },
                    // 項目4
                    new DatabaseItemDesc
                    {
                        ItemName = "DBから",
                        ItemType = DBItemType.Int,
                        SpecialSettingDesc =
                            new DBItemSpecialSettingDesc(DBItemSpecialSettingType.ReferDatabase, null)
                            {
                                ItemMemo = "",
                                InitValue = 23,
                                DatabaseReferKind = DBReferType.User,
                                DatabaseDbTypeId = 4,
                                DatabaseUseAdditionalItemsFlag = false
                            }
                    },
                    // 項目5
                    new DatabaseItemDesc
                    {
                        ItemName = "DBから　その2",
                        ItemType = DBItemType.Int,
                        SpecialSettingDesc = new DBItemSpecialSettingDesc(
                            DBItemSpecialSettingType.ReferDatabase,
                            new List<DatabaseValueCase>
                            {
                                new(-1, "Minus1"),
                                new(-2, "Minus2"),
                                new(-3, "Minus3")
                            })
                        {
                            ItemMemo = "",
                            InitValue = 322,
                            DatabaseReferKind = DBReferType.Changeable,
                            DatabaseDbTypeId = 1,
                            DatabaseUseAdditionalItemsFlag = true
                        }
                    },
                    // 項目6
                    new DatabaseItemDesc
                    {
                        ItemName = "手動生成",
                        ItemType = DBItemType.Int,
                        SpecialSettingDesc = new DBItemSpecialSettingDesc(DBItemSpecialSettingType.Manual,
                            new List<DatabaseValueCase>
                            {
                                new(0, "Zero"),
                                new(3, "さん"),
                                new(10, "１０"),
                                new(9, "nine")
                            })
                        {
                            ItemMemo = "",
                            InitValue = 0
                        }
                    }
                }
            };
            result.DataDescList.Overwrite(0, new List<DatabaseDataDesc>
            {
                // データ0
                new()
                {
                    DataName = "文字列",
                    ItemValueList =
                    {
                        (DBValueInt)(-255),
                        (DBValueString)"文字列",
                        (DBValueString)"MapChip/[A]World_Grass-Grass_pipo.png",
                        (DBValueString)"Map002.mps",
                        (DBValueInt)0,
                        (DBValueInt)(-2),
                        (DBValueInt)3
                    }
                },
                // データ1
                new()
                {
                    DataName = "7",
                    ItemValueList =
                    {
                        (DBValueInt)6,
                        (DBValueString)"7",
                        (DBValueString)"MapData/Map002.mps",
                        (DBValueString)"",
                        (DBValueInt)0,
                        (DBValueInt)(-3),
                        (DBValueInt)9
                    }
                },
                // データ2
                new()
                {
                    DataName = "うでぃた",
                    ItemValueList =
                    {
                        (DBValueInt)0,
                        (DBValueString)"うでぃた",
                        (DBValueString)"",
                        (DBValueString)"",
                        (DBValueInt)0,
                        (DBValueInt)0,
                        (DBValueInt)9
                    }
                },
                // データ3
                new()
                {
                    DataName = "",
                    ItemValueList =
                    {
                        (DBValueInt)0,
                        (DBValueString)"",
                        (DBValueString)"まっぷでーた",
                        (DBValueString)"Map007.mps",
                        (DBValueInt)0,
                        (DBValueInt)(-1),
                        (DBValueInt)3
                    }
                }
            });
            result.SetDataSettingType(DBDataSettingType.FirstStringData);
            return result;
        }

        public static DBType GenerateCDB0DBType()
        {
            var result = new DBType
            {
                TypeName = "あいうえお",
                Memo = "メモ欄",
                ItemDescList =
                {
                    // 項目0
                    new DatabaseItemDesc
                    {
                        ItemName = "ItemName",
                        ItemType = DBItemType.String,
                        SpecialSettingDesc = new DBItemSpecialSettingDesc(DBItemSpecialSettingType.Normal, null)
                        {
                            InitValue = 255,
                            ItemMemo = ""
                        }
                    },
                    // 項目1
                    new DatabaseItemDesc
                    {
                        ItemName = "Field2",
                        ItemType = DBItemType.Int,
                        SpecialSettingDesc = new DBItemSpecialSettingDesc(DBItemSpecialSettingType.Normal, null)
                        {
                            InitValue = 0,
                            ItemMemo = ""
                        }
                    },
                    // 項目2
                    new DatabaseItemDesc
                    {
                        ItemName = "",
                        ItemType = DBItemType.String,
                        SpecialSettingDesc = new DBItemSpecialSettingDesc(DBItemSpecialSettingType.Normal, null)
                        {
                            ItemMemo = ""
                        }
                    },
                    // 項目3
                    new DatabaseItemDesc
                    {
                        ItemName = "FilePath",
                        ItemType = DBItemType.String,
                        SpecialSettingDesc =
                            new DBItemSpecialSettingDesc(DBItemSpecialSettingType.LoadFile, null)
                            {
                                FolderName = "CharaChip",
                                OmissionFolderNameFlag = false,
                                ItemMemo = ""
                            }
                    },
                    // 項目4
                    new DatabaseItemDesc
                    {
                        ItemName = "",
                        ItemType = DBItemType.Int,
                        SpecialSettingDesc = new DBItemSpecialSettingDesc(DBItemSpecialSettingType.Normal, null)
                        {
                            InitValue = 321,
                            ItemMemo = ""
                        }
                    },
                    // 項目5
                    new DatabaseItemDesc
                    {
                        ItemName = "",
                        ItemType = DBItemType.Int,
                        SpecialSettingDesc =
                            new DBItemSpecialSettingDesc(DBItemSpecialSettingType.ReferDatabase, null)
                            {
                                DatabaseReferKind = DBReferType.System,
                                DatabaseDbTypeId = 0,
                                DatabaseUseAdditionalItemsFlag = false,
                                InitValue = 65535,
                                ItemMemo = ""
                            }
                    },
                    // 項目6
                    new DatabaseItemDesc
                    {
                        ItemName = "",
                        ItemType = DBItemType.Int,
                        SpecialSettingDesc = new DBItemSpecialSettingDesc(DBItemSpecialSettingType.Normal, null)
                        {
                            InitValue = 255,
                            ItemMemo = ""
                        }
                    },
                    // 項目7
                    new DatabaseItemDesc
                    {
                        ItemName = "Case",
                        ItemType = DBItemType.Int,
                        SpecialSettingDesc = new DBItemSpecialSettingDesc(DBItemSpecialSettingType.Manual,
                            new List<DatabaseValueCase>
                            {
                                new(0, "選択肢1"),
                                new(1, "選択肢2"),
                                new(2, "選択肢3")
                            })
                        {
                            InitValue = 0,
                            ItemMemo = ""
                        }
                    },
                    // 項目8
                    new DatabaseItemDesc
                    {
                        ItemName = "NormalString",
                        ItemType = DBItemType.String,
                        SpecialSettingDesc = new DBItemSpecialSettingDesc(DBItemSpecialSettingType.Normal, null)
                        {
                            ItemMemo = ""
                        }
                    }
                }
            };
            result.DataDescList.Overwrite(0, new List<DatabaseDataDesc>
            {
                // データ0
                new()
                {
                    DataName = "a",
                    ItemValueList =
                    {
                        (DBValueString)"",
                        (DBValueInt)255,
                        (DBValueString)"",
                        (DBValueString)"CharaChip/[Animal]ChickenTX.png",
                        (DBValueInt)122,
                        (DBValueInt)8,
                        (DBValueInt)6,
                        (DBValueInt)1,
                        (DBValueString)"234"
                    }
                },
                // データ1
                new()
                {
                    DataName = "b",
                    ItemValueList =
                    {
                        (DBValueString)"aaa",
                        (DBValueInt)255,
                        (DBValueString)"aaa",
                        (DBValueString)"",
                        (DBValueInt)0,
                        (DBValueInt)0,
                        (DBValueInt)0,
                        (DBValueInt)0,
                        (DBValueString)""
                    }
                },
                // データ2
                new()
                {
                    DataName = "c",
                    ItemValueList =
                    {
                        (DBValueString)"",
                        (DBValueInt)127,
                        (DBValueString)"",
                        (DBValueString)"",
                        (DBValueInt)4,
                        (DBValueInt)0,
                        (DBValueInt)127,
                        (DBValueInt)1,
                        (DBValueString)""
                    }
                }
            });
            result.SetDataSettingType(DBDataSettingType.Manual);
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
            ("タイプ(データ含む)_000_UDB0.dbtype", TestResources.UDB0DBType),
            ("タイプ(データ含む)_000_あいうえお.dbtype", TestResources.CDB0DBType),
            ("タイプ(データ含む)_002_┣ 主人公行動AI.dbtype", TestResources.CDB2DBType),
            ("タイプ(データ含む)_008_状態設定.dbtype", TestResources.UDB8DBType)
        };

        /// <summary>
        ///     ファイルを tmp フォルダに出力する。
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
        ///     ファイルを削除する。
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
