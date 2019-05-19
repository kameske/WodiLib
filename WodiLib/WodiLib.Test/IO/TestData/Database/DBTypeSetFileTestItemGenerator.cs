using System.Collections.Generic;
using System.IO;
using WodiLib.Database;

namespace WodiLib.Test.IO
{
    public static class DBTypeSetFileTestItemGenerator
    {
        public static DBTypeSet GenerateUDB0Data()
        {
            return new DBTypeSet
            {
                TypeName = "UDB0",
                Memo = "",
                ItemSettingList =
                {
                    new DBItemSetting
                    {
                        ItemName = "設定項目0",
                        SpecialSettingDesc = new DBItemSpecialSettingDesc(DBItemSpecialSettingType.Normal, null)
                        {
                            ItemMemo = "",
                            InitValue = 0
                        },
                        ItemType = DBItemType.Int
                    },
                    new DBItemSetting
                    {
                        ItemName = "文字列項目",
                        SpecialSettingDesc = new DBItemSpecialSettingDesc(DBItemSpecialSettingType.Normal, null)
                        {
                            ItemMemo = "",
                        },
                        ItemType = DBItemType.String
                    },
                    new DBItemSetting
                    {
                        ItemName = "ファイル名設定1",
                        SpecialSettingDesc =
                            new DBItemSpecialSettingDesc(DBItemSpecialSettingType.LoadFile, null)
                            {
                                ItemMemo = "",
                                FolderName = "MapChip",
                                OmissionFolderNameFlag = false,
                            },
                        ItemType = DBItemType.String
                    },
                    new DBItemSetting
                    {
                        ItemName = "ファイル名設定2",
                        SpecialSettingDesc =
                            new DBItemSpecialSettingDesc(DBItemSpecialSettingType.LoadFile, null)
                            {
                                ItemMemo = "",
                                FolderName = "MapData",
                                OmissionFolderNameFlag = true,
                            },
                        ItemType = DBItemType.String
                    },
                    new DBItemSetting
                    {
                        ItemName = "DBから",
                        SpecialSettingDesc =
                            new DBItemSpecialSettingDesc(DBItemSpecialSettingType.ReferDatabase, null)
                            {
                                ItemMemo = "",
                                InitValue = 23,
                                DatabaseReferKind = DBReferType.User,
                                DatabaseDbTypeId = 4,
                                DatabaseUseAdditionalItemsFlag = false
                            },
                        ItemType = DBItemType.Int
                    },
                    new DBItemSetting
                    {
                        ItemName = "DBから　その2",
                        SpecialSettingDesc = new DBItemSpecialSettingDesc(
                            DBItemSpecialSettingType.ReferDatabase,
                            new List<DatabaseValueCase>
                            {
                                new DatabaseValueCase(-1, "Minus1"),
                                new DatabaseValueCase(-2, "Minus2"),
                                new DatabaseValueCase(-3, "Minus3"),
                            })
                        {
                            ItemMemo = "",
                            InitValue = 322,
                            DatabaseReferKind = DBReferType.Changeable,
                            DatabaseDbTypeId = 1,
                            DatabaseUseAdditionalItemsFlag = true
                        },
                        ItemType = DBItemType.Int
                    },
                    new DBItemSetting
                    {
                        ItemName = "手動生成",
                        SpecialSettingDesc = new DBItemSpecialSettingDesc(DBItemSpecialSettingType.Manual,
                            new List<DatabaseValueCase>
                            {
                                new DatabaseValueCase(0, "Zero"),
                                new DatabaseValueCase(3, "さん"),
                                new DatabaseValueCase(10, "１０"),
                                new DatabaseValueCase(9, "nine"),
                            })
                        {
                            ItemMemo = "",
                            InitValue = 0,
                        },
                        ItemType = DBItemType.Int
                    }
                }
            };
        }

        public static DBTypeSet GenerateCDB0Data()
        {
            return new DBTypeSet
            {
                TypeName = "あいうえお",
                Memo = "メモ欄",
                ItemSettingList =
                {
                    new DBItemSetting
                    {
                        ItemName = "ItemName",
                        SpecialSettingDesc = new DBItemSpecialSettingDesc(DBItemSpecialSettingType.Normal, null)
                        {
                            InitValue = 255,
                            ItemMemo = ""
                        },
                        ItemType = DBItemType.String
                    },
                    new DBItemSetting
                    {
                        ItemName = "Field2",
                        SpecialSettingDesc = new DBItemSpecialSettingDesc(DBItemSpecialSettingType.Normal, null)
                        {
                            InitValue = 0,
                            ItemMemo = ""
                        },
                        ItemType = DBItemType.Int
                    },
                    new DBItemSetting
                    {
                        ItemName = "",
                        SpecialSettingDesc = new DBItemSpecialSettingDesc(DBItemSpecialSettingType.Normal, null)
                        {
                            ItemMemo = ""
                        },
                        ItemType = DBItemType.String
                    },
                    new DBItemSetting
                    {
                        ItemName = "FilePath",
                        SpecialSettingDesc =
                            new DBItemSpecialSettingDesc(DBItemSpecialSettingType.LoadFile, null)
                            {
                                FolderName = "CharaChip",
                                OmissionFolderNameFlag = false,
                                ItemMemo = ""
                            },
                        ItemType = DBItemType.String
                    },
                    new DBItemSetting
                    {
                        ItemName = "",
                        SpecialSettingDesc = new DBItemSpecialSettingDesc(DBItemSpecialSettingType.Normal, null)
                        {
                            InitValue = 321,
                            ItemMemo = ""
                        },
                        ItemType = DBItemType.Int
                    },
                    new DBItemSetting
                    {
                        ItemName = "",
                        SpecialSettingDesc =
                            new DBItemSpecialSettingDesc(DBItemSpecialSettingType.ReferDatabase, null)
                            {
                                DatabaseReferKind = DBReferType.System,
                                DatabaseDbTypeId = 0,
                                DatabaseUseAdditionalItemsFlag = false,
                                InitValue = 65535,
                                ItemMemo = ""
                            },
                        ItemType = DBItemType.Int
                    },
                    new DBItemSetting
                    {
                        ItemName = "",
                        SpecialSettingDesc = new DBItemSpecialSettingDesc(DBItemSpecialSettingType.Normal, null)
                        {
                            InitValue = 255,
                            ItemMemo = ""
                        },
                        ItemType = DBItemType.Int
                    },
                    new DBItemSetting
                    {
                        ItemName = "Case",
                        SpecialSettingDesc = new DBItemSpecialSettingDesc(DBItemSpecialSettingType.Manual,
                            new List<DatabaseValueCase>
                            {
                                new DatabaseValueCase(0, "選択肢1"),
                                new DatabaseValueCase(1, "選択肢2"),
                                new DatabaseValueCase(2, "選択肢3"),
                            })
                        {
                            InitValue = 0,
                            ItemMemo = ""
                        },
                        ItemType = DBItemType.Int
                    },
                    new DBItemSetting
                    {
                        ItemName = "NormalString",
                        SpecialSettingDesc = new DBItemSpecialSettingDesc(DBItemSpecialSettingType.Normal, null)
                        {
                            ItemMemo = ""
                        },
                        ItemType = DBItemType.String
                    },
                }
            };
        }

        /** ========================================
         *  テスト用ファイル出力
         *  ======================================== */

        /// <summary>テストディレクトリルート</summary>
        public static string TestWorkRootDir => MapFileTestItemGenerator.TestWorkRootDir;

        /// <summary>テストファイルデータ</summary>
        public static readonly IEnumerable<(string, byte[])> TestFiles = new List<(string, byte[])>
        {
            ("タイプ設定_000_UDB0.dbtypeset", TestResources.UDB0DBTypeSet),
            ("タイプ設定_000_あいうえお.dbtypeset", TestResources.CDB0DBTypeSet),
            ("タイプ設定_002_┣ 主人公行動AI.dbtypeset", TestResources.CDB2DBTypeSet),
            ("タイプ設定_008_状態設定.dbtypeset", TestResources.UDB8DBTypeSet),
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