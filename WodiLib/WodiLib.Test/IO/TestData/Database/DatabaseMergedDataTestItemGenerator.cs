using System.Collections.Generic;
using System.IO;
using WodiLib.Database;

namespace WodiLib.Test.IO
{
    public static class DatabaseMergedDataTestItemGenerator
    {
        public static DatabaseMergedData GenerateUDB0MergedData()
        {
            return new DatabaseMergedData(
                new DBTypeSettingList(new List<DBTypeSetting>
                {
                    new DBTypeSetting
                    {
                        TypeName = "UDB0",
                        Memo = "",
                        DataNameList = new DataNameList(new List<DataName>
                        {
                            "文字列",
                            "7",
                            "うでぃた",
                            "",
                        }),
                        ItemSettingList = new DBItemSettingList(new List<DBItemSetting>
                        {
                            new DBItemSetting
                            {
                                ItemName = "設定項目0",
                                SpecialSettingDesc = new DBItemSpecialSettingDesc(DBItemSpecialSettingType.Normal, null)
                                {
                                    ItemMemo = "",
                                    InitValue = 0
                                },
                            },
                            new DBItemSetting
                            {
                                ItemName = "文字列項目",
                                SpecialSettingDesc = new DBItemSpecialSettingDesc(DBItemSpecialSettingType.Normal, null)
                                {
                                    ItemMemo = "",
                                },
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
                                }
                            }
                        })
                    },
                    new DBTypeSetting
                    {
                        TypeName = "",
                        Memo = "",
                        DataNameList = new DataNameList(new List<DataName>
                        {
                            ""
                        }),
                    },
                    new DBTypeSetting
                    {
                        TypeName = "ゆーでーびーつー",
                        Memo = "UDB2メモ欄\r\n改行",
                        DataNameList = new DataNameList(new List<DataName>
                        {
                            "", // ウディタ上で「×NoData」と表示される場合、空文字が格納されている
                            "",
                        }),
                        ItemSettingList = new DBItemSettingList(new List<DBItemSetting>
                        {
                            new DBItemSetting
                            {
                                SpecialSettingDesc = new DBItemSpecialSettingDesc(DBItemSpecialSettingType.Normal, null)
                                {
                                    ItemMemo = "",
                                },
                                ItemName = "文字列項目",
                            },
                            new DBItemSetting
                            {
                                SpecialSettingDesc = new DBItemSpecialSettingDesc(DBItemSpecialSettingType.Normal, null)
                                {
                                    ItemMemo = "",
                                },
                                ItemName = "文字列項目2"
                            },
                            new DBItemSetting
                            {
                                SpecialSettingDesc = new DBItemSpecialSettingDesc(DBItemSpecialSettingType.Normal, null)
                                {
                                    ItemMemo = "",
                                },
                                ItemName = "文字列項目3"
                            },
                            new DBItemSetting
                            {
                                SpecialSettingDesc = new DBItemSpecialSettingDesc(DBItemSpecialSettingType.Normal, null)
                                {
                                    ItemMemo = "",
                                },
                                ItemName = "文字列項目4"
                            },
                            new DBItemSetting
                            {
                                SpecialSettingDesc = new DBItemSpecialSettingDesc(DBItemSpecialSettingType.Normal, null)
                                {
                                    ItemMemo = "",
                                },
                                ItemName = "文字列項目5"
                            },
                        })
                    },
                    new DBTypeSetting
                    {
                        TypeName = "UDB3",
                        Memo = "",
                        DataNameList = new DataNameList(new List<DataName>
                        {
                            "",
                        }),
                        ItemSettingList = new DBItemSettingList(new List<DBItemSetting>
                        {
                            new DBItemSetting
                            {
                                ItemName = "",
                                SpecialSettingDesc = new DBItemSpecialSettingDesc(DBItemSpecialSettingType.Normal, null)
                                {
                                    ItemMemo = "",
                                },
                            },
                            new DBItemSetting
                            {
                                ItemName = "",
                                SpecialSettingDesc = new DBItemSpecialSettingDesc(DBItemSpecialSettingType.Normal, null)
                                {
                                    InitValue = 0,
                                    ItemMemo = "",
                                },
                            },
                            new DBItemSetting
                            {
                                ItemName = "項目",
                                SpecialSettingDesc = new DBItemSpecialSettingDesc(DBItemSpecialSettingType.Normal, null)
                                {
                                    InitValue = 0,
                                    ItemMemo = "",
                                },
                            },
                        })
                    }
                }),
                new DBDataSettingList(new List<DBDataSetting>
                {
                    new DBDataSetting(DBDataSettingType.FirstStringData)
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
                    },
                    new DBDataSetting(DBDataSettingType.Manual),
                    new DBDataSetting(DBDataSettingType.DesignatedType, DBKind.User, 4)
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
                    },
                    new DBDataSetting(DBDataSettingType.EqualBefore)
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
                    }
                }));
        }

        public static DatabaseMergedData GenerateCDB0MergedData()
        {
            return new DatabaseMergedData(new DBTypeSettingList(new List<DBTypeSetting>
            {
                // CDB0
                new DBTypeSetting
                {
                    TypeName = "あいうえお",
                    Memo = "メモ欄",
                    DataNameList = new DataNameList(new List<DataName>
                    {
                        "a",
                        "b",
                        "c",
                    }),
                    ItemSettingList = new DBItemSettingList(new List<DBItemSetting>
                    {
                        new DBItemSetting
                        {
                            ItemName = "ItemName",
                            SpecialSettingDesc = new DBItemSpecialSettingDesc(DBItemSpecialSettingType.Normal, null)
                            {
                                InitValue = 255,
                                ItemMemo = ""
                            }
                        },
                        new DBItemSetting
                        {
                            ItemName = "Field2",
                            SpecialSettingDesc = new DBItemSpecialSettingDesc(DBItemSpecialSettingType.Normal, null)
                            {
                                InitValue = 0,
                                ItemMemo = ""
                            }
                        },
                        new DBItemSetting
                        {
                            ItemName = "",
                            SpecialSettingDesc = new DBItemSpecialSettingDesc(DBItemSpecialSettingType.Normal, null)
                            {
                                ItemMemo = ""
                            }
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
                                }
                        },
                        new DBItemSetting
                        {
                            ItemName = "",
                            SpecialSettingDesc = new DBItemSpecialSettingDesc(DBItemSpecialSettingType.Normal, null)
                            {
                                InitValue = 321,
                                ItemMemo = ""
                            }
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
                                }
                        },
                        new DBItemSetting
                        {
                            ItemName = "",
                            SpecialSettingDesc = new DBItemSpecialSettingDesc(DBItemSpecialSettingType.Normal, null)
                            {
                                InitValue = 255,
                                ItemMemo = ""
                            }
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
                            }
                        },
                        new DBItemSetting
                        {
                            ItemName = "NormalString",
                            SpecialSettingDesc = new DBItemSpecialSettingDesc(DBItemSpecialSettingType.Normal, null)
                            {
                                ItemMemo = ""
                            }
                        },
                    })
                },
                // CDB1
                new DBTypeSetting
                {
                    TypeName = "",
                    Memo = "",
                    DataNameList = new DataNameList(new List<DataName>
                    {
                        "",
                    }),
                    ItemSettingList = new DBItemSettingList(new List<DBItemSetting>
                    {
                        new DBItemSetting
                        {
                            ItemName = "ItemField",
                            SpecialSettingDesc = new DBItemSpecialSettingDesc(DBItemSpecialSettingType.Normal, null)
                            {
                                ItemMemo = "",
                            }
                        },
                    })
                },
            }), new DBDataSettingList(new List<DBDataSetting>
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
            }));
        }

        /** ========================================
         *  テスト用ファイル出力
         *  ======================================== */

        /// <summary>テストディレクトリルート</summary>
        public static string TestWorkRootDir => MapFileTestItemGenerator.TestWorkRootDir;

        /// <summary>テストファイルデータ</summary>
        public static readonly IEnumerable<(string, byte[])> TestFiles = new List<(string, byte[])>
        {
            ("Database.dat", TestResources.Database0Dat),
            ("CDatabase.dat", TestResources.CDatabase0Dat),
            ("Database.project", TestResources.Database0Project),
            ("CDatabase.project", TestResources.CDatabase0Project),
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