using System;
using System.Collections.Generic;
using WodiLib.Common;
using WodiLib.Database;
using WodiLib.Event;
using WodiLib.Event.EventCommand;

namespace WodiLib.Test.IO
{
    public static partial class CommonEventDataFileTestItemGenerator
    {
        public static class CommonEvent00Data
        {
            public static CommonEvent GenerateCommonEvent000()
            {
                var result = new CommonEvent
                {
                    Id = 0,
                    BootCondition = new CommonEventBootCondition
                    {
                        CommonEventBootType = CommonEventBootType.OnlyCall,
                        LeftSide = 2000000,
                        Operation = CriteriaOperator.Equal,
                        RightSide = 0
                    },
                    NumberArgsLength = 0,
                    StrArgsLength = 0,
                    Name = "コモンイベント000",
                    EventCommands = new EventCommandList(
                        new List<IEventCommand>
                        {
                            new Message
                            {
                                Indent = 0,
                                Text = "イベントコマンド"
                            },
                            new Blank
                            {
                                Indent = 0
                            }
                        }),
                    Description = "",
                    Memo = "",
                    LabelColor = CommonEventLabelColor.Black,
                    FooterString = "",
                };

                result.SelfVariableNameList[0] = "cself0";
                result.SelfVariableNameList[99] = "cself99";

                result.SetReturnVariableIndex(99);
                result.ReturnValueDescription = "てすと";

                return result;
            }

            public static CommonEvent GenerateCommonEvent001()
            {
                var result = new CommonEvent
                {
                    Id = 1,
                    BootCondition = new CommonEventBootCondition
                    {
                        CommonEventBootType = CommonEventBootType.OnlyCall,
                        LeftSide = 2000000,
                        Operation = CriteriaOperator.Equal,
                        RightSide = 0
                    },
                    NumberArgsLength = 3,
                    StrArgsLength = 4,
                    Name = "コモンイベント001",
                    EventCommands = new EventCommandList(
                        new List<IEventCommand>
                        {
                            new Message
                            {
                                Indent = 0,
                                Text = "イベントコマンド"
                            },
                            new Blank
                            {
                                Indent = 0
                            }
                        }),
                    Description = "",
                    Memo = "Memo",
                    LabelColor = CommonEventLabelColor.Blue,
                    FooterString = "",
                };

                /* ---------- セルフ変数名 ---------- */
                result.SelfVariableNameList[0] = "cself0";
                result.SelfVariableNameList[1] = "数値変数2";
                result.SelfVariableNameList[2] = "数値変数3";
                result.SelfVariableNameList[3] = "数値変数4";
                result.SelfVariableNameList[5] = "文字列変数1";
                result.SelfVariableNameList[6] = "文字列変数2";
                result.SelfVariableNameList[7] = "文字列変数3";
                result.SelfVariableNameList[8] = "文字列変数4";
                result.SelfVariableNameList[89] = "返戻";
                result.SelfVariableNameList[90] = "CSelf90";
                result.SelfVariableNameList[99] = "cself99";

                /* ---------- 数値引数 ---------- */
                result.UpdateSpecialNumberArgDesc(0,
                    ((Func<CommonEventSpecialNumberArgDesc>) (() =>
                    {
                        var desc = new CommonEventSpecialNumberArgDesc
                        {
                            ArgName = "数値変数1",
                            InitValue = 128
                        };
                        desc.ChangeArgType(CommonEventArgType.Manual, new List<CommonEventSpecialArgCase>
                        {
                            new CommonEventSpecialArgCase(128, "DDD"),
                            new CommonEventSpecialArgCase(-1, "AAA"),
                            new CommonEventSpecialArgCase(127, "BBB")
                        });
                        return desc;
                    }))());
                result.UpdateSpecialNumberArgDesc(1,
                    ((Func<CommonEventSpecialNumberArgDesc>) (() =>
                    {
                        var desc = new CommonEventSpecialNumberArgDesc
                        {
                            ArgName = "数値変数2",
                            InitValue = 2
                        };
                        desc.ChangeArgType(CommonEventArgType.Manual, new List<CommonEventSpecialArgCase>
                        {
                            new CommonEventSpecialArgCase(0, ""),
                            new CommonEventSpecialArgCase(128, ""),
                            new CommonEventSpecialArgCase(65535, "")
                        });
                        return desc;
                    }))());
                result.UpdateSpecialNumberArgDesc(2,
                    ((Func<CommonEventSpecialNumberArgDesc>) (() =>
                    {
                        var desc = new CommonEventSpecialNumberArgDesc
                        {
                            ArgName = "数値変数3",
                            InitValue = 127
                        };
                        desc.ChangeArgType(CommonEventArgType.ReferDatabase, null);
                        desc.SetDatabaseRefer(DBKind.Changeable, 5);
                        desc.SetDatabaseUseAdditionalItemsFlag(false);
                        return desc;
                    }))());
                result.UpdateSpecialNumberArgDesc(3,
                    ((Func<CommonEventSpecialNumberArgDesc>) (() =>
                    {
                        /* 数値変数4は実際には設定されていないが、一度でも設定されたことがある場合そのデータが残っている。 */
                        var desc = new CommonEventSpecialNumberArgDesc
                        {
                            ArgName = "数値変数4",
                            InitValue = 255
                        };
                        desc.ChangeArgType(CommonEventArgType.ReferDatabase, new[]
                        {
                            new CommonEventSpecialArgCase(-1, ""),
                            new CommonEventSpecialArgCase(-2, ""),
                            new CommonEventSpecialArgCase(-3, "End"),
                        });
                        desc.SetDatabaseRefer(DBKind.System, 0);
                        desc.SetDatabaseUseAdditionalItemsFlag(true);
                        return desc;
                    }))());

                /* ---------- 文字列引数1 ---------- */
                result.UpdateSpecialStringArgDesc(0,
                    new CommonEventSpecialStringArgDesc
                    {
                        ArgName = "文字列引数1"
                    });
                /* ---------- 文字列引数2 ---------- */
                result.UpdateSpecialStringArgDesc(1,
                    new CommonEventSpecialStringArgDesc
                    {
                        ArgName = "文字列引数2"
                    });
                /* ---------- 文字列引数3 ---------- */
                result.UpdateSpecialStringArgDesc(2,
                    new CommonEventSpecialStringArgDesc
                    {
                        ArgName = "文字列引数3"
                    });
                /* ---------- 文字列引数4 ---------- */
                result.UpdateSpecialStringArgDesc(3,
                    new CommonEventSpecialStringArgDesc
                    {
                        ArgName = "文字列引数4"
                    });

                result.SetReturnVariableIndex(90);
                result.ReturnValueDescription = "返戻";

                return result;
            }

            public static CommonEvent GenerateCommonEvent002()
            {
                var result = new CommonEvent
                {
                    Id = 2,
                    BootCondition = new CommonEventBootCondition
                    {
                        CommonEventBootType = CommonEventBootType.OnlyCall,
                        LeftSide = 2000000,
                        Operation = CriteriaOperator.Equal,
                        RightSide = 0
                    },
                    NumberArgsLength = 0,
                    StrArgsLength = 0,
                    Name = "コモンイベント002",
                    EventCommands = new EventCommandList(
                        new List<IEventCommand>
                        {
                            EventCommandFactory.CreateCommandString(
                                "[300][10,1]<0>(0,68,128,2,127,255,1600005,1600005,1600005,1600005)(\"コモンイベント001\")"),
                            EventCommandFactory.CreateCommandString("[0][0,0]<0>()()")
                        }),
                    Description = "",
                    Memo = "",
                    LabelColor = CommonEventLabelColor.Black,
                    FooterString = "",
                };

                /* ---------- 数値引数 ---------- */
                result.UpdateSpecialNumberArgDesc(0,
                    ((Func<CommonEventSpecialNumberArgDesc>) (() =>
                    {
                        /* 画面では見えないが、一度設定したことがあるためデータ内に存在する。*/
                        var desc = new CommonEventSpecialNumberArgDesc
                        {
                            ArgName = "テスト",
                            InitValue = 0
                        };
                        desc.ChangeArgType(CommonEventArgType.Normal, null);
                        return desc;
                    }))());

                result.SelfVariableNameList[0] = "テスト";

                result.SetReturnVariableIndex(0);
                result.ReturnValueDescription = "";

                return result;
            }

            public static CommonEvent GenerateCommonEvent003()
            {
                var result = new CommonEvent
                {
                    Id = 3,
                    BootCondition = new CommonEventBootCondition
                    {
                        CommonEventBootType = CommonEventBootType.Parallel,
                        LeftSide = 2300002,
                        Operation = CriteriaOperator.Below,
                        RightSide = (-13)
                    },
                    NumberArgsLength = 4,
                    StrArgsLength = 2,
                    Name = "コモンイベント003",
                    EventCommands = new EventCommandList(
                        new List<IEventCommand>
                        {
                            EventCommandFactory.CreateCommandString("[210][6,0]<0>(600100,34,0,0,1600005,1600005)()"),
                            EventCommandFactory.CreateCommandString("[210][3,0]<0>(600099,16777216,1600000)()"),
                            EventCommandFactory.CreateCommandString("[0][0,0]<0>()()"),
                        }),
                    Description = "",
                    Memo = "Memo",
                    LabelColor = CommonEventLabelColor.Purple,
                    FooterString = "",
                };

                /* ---------- セルフ変数名 ---------- */
                result.SelfVariableNameList[0] = "Summary1";
                result.SelfVariableNameList[3] = "Summary4";
                result.SelfVariableNameList[50] = "Test";
                result.SelfVariableNameList[51] = "てすと";

                /* ---------- 数値引数 ---------- */
                result.UpdateSpecialNumberArgDesc(0,
                    ((Func<CommonEventSpecialNumberArgDesc>) (() =>
                    {
                        var desc = new CommonEventSpecialNumberArgDesc
                        {
                            ArgName = "Summary1",
                            InitValue = 0
                        };
                        desc.ChangeArgType(CommonEventArgType.Normal, null);
                        return desc;
                    }))());
                result.UpdateSpecialNumberArgDesc(1,
                    ((Func<CommonEventSpecialNumberArgDesc>) (() =>
                    {
                        var desc = new CommonEventSpecialNumberArgDesc
                        {
                            ArgName = "引数2",
                            InitValue = 0
                        };
                        desc.ChangeArgType(CommonEventArgType.ReferDatabase, null);
                        desc.SetDatabaseRefer(DBKind.System, 10);
                        desc.SetDatabaseUseAdditionalItemsFlag(false);
                        return desc;
                    }))());
                result.UpdateSpecialNumberArgDesc(2,
                    ((Func<CommonEventSpecialNumberArgDesc>) (() =>
                    {
                        var desc = new CommonEventSpecialNumberArgDesc
                        {
                            ArgName = "引数3",
                            InitValue = 0
                        };
                        desc.ChangeArgType(CommonEventArgType.ReferDatabase, new[]
                        {
                            new CommonEventSpecialArgCase(-1, "Value-1"),
                            new CommonEventSpecialArgCase(-2, "Value-2"),
                            new CommonEventSpecialArgCase(-3, "Value-3"),
                        });
                        desc.SetDatabaseRefer(DBKind.Changeable, 10);
                        desc.SetDatabaseUseAdditionalItemsFlag(true);
                        return desc;
                    }))());
                result.UpdateSpecialNumberArgDesc(3,
                    ((Func<CommonEventSpecialNumberArgDesc>) (() =>
                    {
                        var desc = new CommonEventSpecialNumberArgDesc
                        {
                            ArgName = "Summary4",
                            InitValue = 0
                        };
                        desc.ChangeArgType(CommonEventArgType.Manual, new[]
                        {
                            new CommonEventSpecialArgCase(0, "Select0"),
                            new CommonEventSpecialArgCase(1, "Select1"),
                            new CommonEventSpecialArgCase(2, "Select2"),
                            new CommonEventSpecialArgCase(3, "Select3"),
                        });
                        return desc;
                    }))());

                /* ---------- 文字列引数 ---------- */
                result.UpdateSpecialStringArgDesc(0,
                    new CommonEventSpecialStringArgDesc
                    {
                        ArgName = ""
                    });
                result.UpdateSpecialStringArgDesc(1,
                    new CommonEventSpecialStringArgDesc
                    {
                        ArgName = ""
                    });

                /* ---------- セルフ変数名 ---------- */
                result.SelfVariableNameList[0] = "Summary1";
                result.SelfVariableNameList[1] = "引数2";
                result.SelfVariableNameList[2] = "引数3";
                result.SelfVariableNameList[3] = "Summary4";
                result.SelfVariableNameList[50] = "Test";
                result.SelfVariableNameList[51] = "てすと";

                result.SetReturnValueNone();

                return result;
            }

            public static CommonEvent GenerateCommonEvent004()
            {
                var result = new CommonEvent
                {
                    Id = 4,
                    BootCondition = new CommonEventBootCondition
                    {
                        CommonEventBootType = CommonEventBootType.Auto,
                        LeftSide = 2000000,
                        Operation = CriteriaOperator.Equal,
                        RightSide = 10
                    },
                    NumberArgsLength = 0,
                    StrArgsLength = 0,
                    Name = "コモンイベント004",
                    EventCommands = new EventCommandList(
                        new List<IEventCommand>
                        {
                            EventCommandFactory.CreateCommandString("[0][0,0]<0>()()"),
                        }),
                    Description = "",
                    Memo = "",
                    LabelColor = CommonEventLabelColor.Black,
                    FooterString = "",
                };

                return result;
            }

            public static CommonEvent GenerateCommonEvent005()
            {
                var result = new CommonEvent
                {
                    Id = 5,
                    BootCondition = new CommonEventBootCondition
                    {
                        CommonEventBootType = CommonEventBootType.Auto,
                        LeftSide = 2000004,
                        Operation = CriteriaOperator.Above,
                        RightSide = 10
                    },
                    NumberArgsLength = 0,
                    StrArgsLength = 0,
                    Name = "コモンイベント005",
                    EventCommands = new EventCommandList(
                        new List<IEventCommand>
                        {
                            EventCommandFactory.CreateCommandString("[0][0,0]<0>()()"),
                        }),
                    Description = "",
                    Memo = "",
                    LabelColor = CommonEventLabelColor.Green,
                    FooterString = "",
                };

                return result;
            }

            public static CommonEvent GenerateCommonEvent006()
            {
                var result = new CommonEvent
                {
                    Id = 6,
                    BootCondition = new CommonEventBootCondition
                    {
                        CommonEventBootType = CommonEventBootType.ParallelAlways,
                        LeftSide = 2000000,
                        Operation = CriteriaOperator.Above,
                        RightSide = 10
                    },
                    NumberArgsLength = 4,
                    StrArgsLength = 4,
                    Name = "コモンイベント006",
                    EventCommands = new EventCommandList(
                        new List<IEventCommand>
                        {
                            EventCommandFactory.CreateCommandString("[0][0,0]<0>()()"),
                        }),
                    Description = "",
                    Memo = "",
                    LabelColor = CommonEventLabelColor.Black,
                    FooterString = "",
                };

                /* ---------- セルフ変数名 ---------- */
                result.SelfVariableNameList[0] = "NumArg1Name";
                result.SelfVariableNameList[1] = "NumArg2Name";
                result.SelfVariableNameList[2] = "NumArg3Name";
                result.SelfVariableNameList[3] = "NumArg4Name";
                result.SelfVariableNameList[5] = "StrArg1Name";
                result.SelfVariableNameList[6] = "StrArg2Name";
                result.SelfVariableNameList[7] = "StrArg3Name";
                result.SelfVariableNameList[8] = "StrArg4Name";

                /* ---------- 数値引数 ---------- */
                result.UpdateSpecialNumberArgDesc(0,
                    ((Func<CommonEventSpecialNumberArgDesc>) (() =>
                    {
                        var desc = new CommonEventSpecialNumberArgDesc
                        {
                            ArgName = "NumArg1Name",
                            InitValue = 0
                        };
                        desc.ChangeArgType(CommonEventArgType.Normal, null);
                        return desc;
                    }))());
                result.UpdateSpecialNumberArgDesc(1,
                    ((Func<CommonEventSpecialNumberArgDesc>) (() =>
                    {
                        var desc = new CommonEventSpecialNumberArgDesc
                        {
                            ArgName = "NumArg2Name",
                            InitValue = 0
                        };
                        desc.ChangeArgType(CommonEventArgType.Normal, null);
                        return desc;
                    }))());
                result.UpdateSpecialNumberArgDesc(2,
                    ((Func<CommonEventSpecialNumberArgDesc>) (() =>
                    {
                        var desc = new CommonEventSpecialNumberArgDesc
                        {
                            ArgName = "NumArg3Name",
                            InitValue = 0
                        };
                        desc.ChangeArgType(CommonEventArgType.Normal, null);
                        return desc;
                    }))());
                result.UpdateSpecialNumberArgDesc(3,
                    ((Func<CommonEventSpecialNumberArgDesc>) (() =>
                    {
                        var desc = new CommonEventSpecialNumberArgDesc
                        {
                            ArgName = "NumArg4Name",
                            InitValue = 0
                        };
                        desc.ChangeArgType(CommonEventArgType.Normal, null);
                        return desc;
                    }))());
                result.UpdateSpecialNumberArgDesc(4,
                    ((Func<CommonEventSpecialNumberArgDesc>) (() =>
                    {
                        var desc = new CommonEventSpecialNumberArgDesc
                        {
                            ArgName = "N",
                            InitValue = 0
                        };
                        desc.ChangeArgType(CommonEventArgType.Normal, null);
                        return desc;
                    }))());

                /* ---------- 文字列引数 ---------- */
                result.UpdateSpecialStringArgDesc(0,
                    new CommonEventSpecialStringArgDesc
                    {
                        ArgName = "StrArg1Name"
                    });
                result.UpdateSpecialStringArgDesc(1,
                    new CommonEventSpecialStringArgDesc
                    {
                        ArgName = "StrArg2Name"
                    });
                result.UpdateSpecialStringArgDesc(2,
                    new CommonEventSpecialStringArgDesc
                    {
                        ArgName = "StrArg3Name"
                    });
                result.UpdateSpecialStringArgDesc(3,
                    new CommonEventSpecialStringArgDesc
                    {
                        ArgName = "StrArg4Name"
                    });
                result.UpdateSpecialStringArgDesc(4,
                    new CommonEventSpecialStringArgDesc
                    {
                        ArgName = "U"
                    });

                result.SetReturnValueNone();

                return result;
            }

            public static CommonEvent GenerateCommonEvent007()
            {
                var result = new CommonEvent
                {
                    Id = 7,
                    BootCondition = new CommonEventBootCondition
                    {
                        CommonEventBootType = CommonEventBootType.ParallelAlways,
                        LeftSide = 2000000,
                        Operation = CriteriaOperator.Equal,
                        RightSide = 0
                    },
                    NumberArgsLength = 1,
                    StrArgsLength = 0,
                    Name = "コモンイベント007",
                    EventCommands = new EventCommandList(
                        new List<IEventCommand>
                        {
                            EventCommandFactory.CreateCommandString("[0][0,0]<0>()()"),
                        }),
                    Description = "",
                    Memo = "",
                    LabelColor = CommonEventLabelColor.Black,
                    FooterString = "",
                };

                /* ---------- 数値引数 ---------- */
                result.UpdateSpecialNumberArgDesc(0,
                    ((Func<CommonEventSpecialNumberArgDesc>) (() =>
                    {
                        var desc = new CommonEventSpecialNumberArgDesc
                        {
                            ArgName = "",
                            InitValue = 0
                        };
                        desc.ChangeArgType(CommonEventArgType.ReferDatabase, null);
                        desc.SetDatabaseRefer(DBKind.System, 0);
                        desc.SetDatabaseUseAdditionalItemsFlag(false);
                        return desc;
                    }))());

                result.SetReturnVariableIndex(41);
                result.ReturnValueDescription = "Return";

                return result;
            }
        }
    }
}