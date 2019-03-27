using System;
using System.Collections.Generic;
using WodiLib.Cmn;
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
                    Id = (CommonEventId) 0,
                    BootCondition = new CommonEventBootCondition
                    {
                        CommonEventBootType = CommonEventBootType.OnlyCall,
                        LeftSide = (VariableAddress) 2000000,
                        Operation = CriteriaOperator.Equal,
                        RightSide = (ConditionRight) 0
                    },
                    NumberArgsLength = 0,
                    StrArgsLength = 0,
                    Name = (CommonEventName) "コモンイベント000",
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
                    Description = (CommonEventDescription) "",
                    Memo = (CommonEventMemo) "",
                    LabelColor = CommonEventLabelColor.Black,
                    FooterString = (CommonEventFooterString) "",
                };

                result.UpdateVariableName((CommonEventSelfVariableIndex) 0,
                    (CommonEventSelfVariableName) "cself0");
                result.UpdateVariableName((CommonEventSelfVariableIndex) 99,
                    (CommonEventSelfVariableName) "cself99");

                result.SetReturnVariableIndex((CommonEventReturnVariableIndex) 99);
                result.ReturnValueDescription = (CommonEventResultDescription) "てすと";

                return result;
            }

            public static CommonEvent GenerateCommonEvent001()
            {
                var result = new CommonEvent
                {
                    Id = (CommonEventId) 1,
                    BootCondition = new CommonEventBootCondition
                    {
                        CommonEventBootType = CommonEventBootType.OnlyCall,
                        LeftSide = (VariableAddress) 2000000,
                        Operation = CriteriaOperator.Equal,
                        RightSide = (ConditionRight) 0
                    },
                    NumberArgsLength = 3,
                    StrArgsLength = 4,
                    Name = (CommonEventName) "コモンイベント001",
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
                    Description = (CommonEventDescription) "",
                    Memo = (CommonEventMemo) "Memo",
                    LabelColor = CommonEventLabelColor.Blue,
                    FooterString = (CommonEventFooterString) "",
                };

                /* ---------- セルフ変数名 ---------- */
                result.UpdateVariableName((CommonEventSelfVariableIndex) 0, (CommonEventSelfVariableName) "cself0");
                result.UpdateVariableName((CommonEventSelfVariableIndex) 1, (CommonEventSelfVariableName) "数値変数2");
                result.UpdateVariableName((CommonEventSelfVariableIndex) 2, (CommonEventSelfVariableName) "数値変数3");
                result.UpdateVariableName((CommonEventSelfVariableIndex) 3, (CommonEventSelfVariableName) "数値変数4");
                result.UpdateVariableName((CommonEventSelfVariableIndex) 5, (CommonEventSelfVariableName) "文字列変数1");
                result.UpdateVariableName((CommonEventSelfVariableIndex) 6, (CommonEventSelfVariableName) "文字列変数2");
                result.UpdateVariableName((CommonEventSelfVariableIndex) 7, (CommonEventSelfVariableName) "文字列変数3");
                result.UpdateVariableName((CommonEventSelfVariableIndex) 8, (CommonEventSelfVariableName) "文字列変数4");
                result.UpdateVariableName((CommonEventSelfVariableIndex) 89, (CommonEventSelfVariableName) "返戻");
                result.UpdateVariableName((CommonEventSelfVariableIndex) 90, (CommonEventSelfVariableName) "CSelf90");
                result.UpdateVariableName((CommonEventSelfVariableIndex) 99, (CommonEventSelfVariableName) "cself99");

                /* ---------- 数値引数 ---------- */
                result.UpdateSpecialNumberArgDesc((CommonEventNumberArgIndex) 0,
                    ((Func<CommonEventSpecialNumberArgDesc>) (() =>
                    {
                        var desc = new CommonEventSpecialNumberArgDesc
                        {
                            ArgName = (CommonEventArgName) "数値変数1",
                            InitValue = (CommonEventNumberArgInitValue) 128
                        };
                        desc.ChangeArgType(CommonEventArgType.Manual, new List<CommonEventSpecialArgCase>
                        {
                            new CommonEventSpecialArgCase(128, "DDD"),
                            new CommonEventSpecialArgCase(-1, "AAA"),
                            new CommonEventSpecialArgCase(127, "BBB")
                        });
                        return desc;
                    }))());
                result.UpdateSpecialNumberArgDesc((CommonEventNumberArgIndex) 1,
                    ((Func<CommonEventSpecialNumberArgDesc>) (() =>
                    {
                        var desc = new CommonEventSpecialNumberArgDesc
                        {
                            ArgName = (CommonEventArgName) "数値変数2",
                            InitValue = (CommonEventNumberArgInitValue) 2
                        };
                        desc.ChangeArgType(CommonEventArgType.Manual, new List<CommonEventSpecialArgCase>
                        {
                            new CommonEventSpecialArgCase(0, ""),
                            new CommonEventSpecialArgCase(128, ""),
                            new CommonEventSpecialArgCase(65535, "")
                        });
                        return desc;
                    }))());
                result.UpdateSpecialNumberArgDesc((CommonEventNumberArgIndex) 2,
                    ((Func<CommonEventSpecialNumberArgDesc>) (() =>
                    {
                        var desc = new CommonEventSpecialNumberArgDesc
                        {
                            ArgName = (CommonEventArgName) "数値変数3",
                            InitValue = (CommonEventNumberArgInitValue) 127
                        };
                        desc.ChangeArgType(CommonEventArgType.ReferDatabase, null);
                        desc.SetDatabaseRefer(DBKind.Changeable, (TypeId) 5);
                        desc.SetDatabaseUseAdditionalItemsFlag(false);
                        return desc;
                    }))());
                result.UpdateSpecialNumberArgDesc((CommonEventNumberArgIndex) 3,
                    ((Func<CommonEventSpecialNumberArgDesc>) (() =>
                    {
                        /* 数値変数4は実際には設定されていないが、一度でも設定されたことがある場合そのデータが残っている。 */
                        var desc = new CommonEventSpecialNumberArgDesc
                        {
                            ArgName = (CommonEventArgName) "数値変数4",
                            InitValue = (CommonEventNumberArgInitValue) 255
                        };
                        desc.ChangeArgType(CommonEventArgType.ReferDatabase, new[]
                        {
                            new CommonEventSpecialArgCase(-1, ""),
                            new CommonEventSpecialArgCase(-2, ""),
                            new CommonEventSpecialArgCase(-3, "End"),
                        });
                        desc.SetDatabaseRefer(DBKind.System, (TypeId) 0);
                        desc.SetDatabaseUseAdditionalItemsFlag(true);
                        return desc;
                    }))());

                /* ---------- 文字列引数1 ---------- */
                result.UpdateSpecialStringArgDesc((CommonEventStringArgIndex) 0,
                    new CommonEventSpecialStringArgDesc
                    {
                        ArgName = (CommonEventArgName) "文字列引数1"
                    });
                /* ---------- 文字列引数2 ---------- */
                result.UpdateSpecialStringArgDesc((CommonEventStringArgIndex) 1,
                    new CommonEventSpecialStringArgDesc
                    {
                        ArgName = (CommonEventArgName) "文字列引数2"
                    });
                /* ---------- 文字列引数3 ---------- */
                result.UpdateSpecialStringArgDesc((CommonEventStringArgIndex) 2,
                    new CommonEventSpecialStringArgDesc
                    {
                        ArgName = (CommonEventArgName) "文字列引数3"
                    });
                /* ---------- 文字列引数4 ---------- */
                result.UpdateSpecialStringArgDesc((CommonEventStringArgIndex) 3,
                    new CommonEventSpecialStringArgDesc
                    {
                        ArgName = (CommonEventArgName) "文字列引数4"
                    });

                result.SetReturnVariableIndex((CommonEventReturnVariableIndex) 90);
                result.ReturnValueDescription = (CommonEventResultDescription) "返戻";

                return result;
            }

            public static CommonEvent GenerateCommonEvent002()
            {
                var result = new CommonEvent
                {
                    Id = (CommonEventId) 2,
                    BootCondition = new CommonEventBootCondition
                    {
                        CommonEventBootType = CommonEventBootType.OnlyCall,
                        LeftSide = (VariableAddress) 2000000,
                        Operation = CriteriaOperator.Equal,
                        RightSide = (ConditionRight) 0
                    },
                    NumberArgsLength = 0,
                    StrArgsLength = 0,
                    Name = (CommonEventName) "コモンイベント002",
                    EventCommands = new EventCommandList(
                        new List<IEventCommand>
                        {
                            EventCommandFactory.CreateCommandString(
                                "[300][10,1]<0>(0,68,128,2,127,255,1600005,1600005,1600005,1600005)(\"コモンイベント001\")"),
                            EventCommandFactory.CreateCommandString("[0][0,0]<0>()()")
                        }),
                    Description = (CommonEventDescription) "",
                    Memo = (CommonEventMemo) "",
                    LabelColor = CommonEventLabelColor.Black,
                    FooterString = (CommonEventFooterString) "",
                };

                /* ---------- 数値引数 ---------- */
                result.UpdateSpecialNumberArgDesc((CommonEventNumberArgIndex) 0,
                    ((Func<CommonEventSpecialNumberArgDesc>) (() =>
                    {
                        /* 画面では見えないが、一度設定したことがあるためデータ内に存在する。*/
                        var desc = new CommonEventSpecialNumberArgDesc
                        {
                            ArgName = (CommonEventArgName) "テスト",
                            InitValue = (CommonEventNumberArgInitValue) 0
                        };
                        desc.ChangeArgType(CommonEventArgType.Normal, null);
                        return desc;
                    }))());

                result.UpdateVariableName((CommonEventSelfVariableIndex) 0, (CommonEventSelfVariableName) "テスト");

                result.SetReturnVariableIndex((CommonEventReturnVariableIndex) 0);
                result.ReturnValueDescription = (CommonEventResultDescription) "";

                return result;
            }

            public static CommonEvent GenerateCommonEvent003()
            {
                var result = new CommonEvent
                {
                    Id = (CommonEventId) 3,
                    BootCondition = new CommonEventBootCondition
                    {
                        CommonEventBootType = CommonEventBootType.Parallel,
                        LeftSide = (VariableAddress) 2300002,
                        Operation = CriteriaOperator.Below,
                        RightSide = (ConditionRight) (-13)
                    },
                    NumberArgsLength = 4,
                    StrArgsLength = 2,
                    Name = (CommonEventName) "コモンイベント003",
                    EventCommands = new EventCommandList(
                        new List<IEventCommand>
                        {
                            EventCommandFactory.CreateCommandString("[210][6,0]<0>(600100,34,0,0,1600005,1600005)()"),
                            EventCommandFactory.CreateCommandString("[210][3,0]<0>(600099,16777216,1600000)()"),
                            EventCommandFactory.CreateCommandString("[0][0,0]<0>()()"),
                        }),
                    Description = (CommonEventDescription) "",
                    Memo = (CommonEventMemo) "Memo",
                    LabelColor = CommonEventLabelColor.Purple,
                    FooterString = (CommonEventFooterString) "",
                };

                /* ---------- セルフ変数名 ---------- */
                result.UpdateVariableName((CommonEventSelfVariableIndex) 0, (CommonEventSelfVariableName) "Summary1");
                result.UpdateVariableName((CommonEventSelfVariableIndex) 3, (CommonEventSelfVariableName) "Summary4");
                result.UpdateVariableName((CommonEventSelfVariableIndex) 50, (CommonEventSelfVariableName) "Test");
                result.UpdateVariableName((CommonEventSelfVariableIndex) 51, (CommonEventSelfVariableName) "てすと");

                /* ---------- 数値引数 ---------- */
                result.UpdateSpecialNumberArgDesc((CommonEventNumberArgIndex) 0,
                    ((Func<CommonEventSpecialNumberArgDesc>) (() =>
                    {
                        var desc = new CommonEventSpecialNumberArgDesc
                        {
                            ArgName = (CommonEventArgName) "Summary1",
                            InitValue = (CommonEventNumberArgInitValue) 0
                        };
                        desc.ChangeArgType(CommonEventArgType.Normal, null);
                        return desc;
                    }))());
                result.UpdateSpecialNumberArgDesc((CommonEventNumberArgIndex) 1,
                    ((Func<CommonEventSpecialNumberArgDesc>) (() =>
                    {
                        var desc = new CommonEventSpecialNumberArgDesc
                        {
                            ArgName = (CommonEventArgName) "引数2",
                            InitValue = (CommonEventNumberArgInitValue) 0
                        };
                        desc.ChangeArgType(CommonEventArgType.ReferDatabase, null);
                        desc.SetDatabaseRefer(DBKind.System, (TypeId) 10);
                        desc.SetDatabaseUseAdditionalItemsFlag(false);
                        return desc;
                    }))());
                result.UpdateSpecialNumberArgDesc((CommonEventNumberArgIndex) 2,
                    ((Func<CommonEventSpecialNumberArgDesc>) (() =>
                    {
                        var desc = new CommonEventSpecialNumberArgDesc
                        {
                            ArgName = (CommonEventArgName) "引数3",
                            InitValue = (CommonEventNumberArgInitValue) 0
                        };
                        desc.ChangeArgType(CommonEventArgType.ReferDatabase, new[]
                        {
                            new CommonEventSpecialArgCase(-1, "Value-1"),
                            new CommonEventSpecialArgCase(-2, "Value-2"),
                            new CommonEventSpecialArgCase(-3, "Value-3"),
                        });
                        desc.SetDatabaseRefer(DBKind.Changeable, (TypeId) 10);
                        desc.SetDatabaseUseAdditionalItemsFlag(true);
                        return desc;
                    }))());
                result.UpdateSpecialNumberArgDesc((CommonEventNumberArgIndex) 3,
                    ((Func<CommonEventSpecialNumberArgDesc>) (() =>
                    {
                        var desc = new CommonEventSpecialNumberArgDesc
                        {
                            ArgName = (CommonEventArgName) "Summary4",
                            InitValue = (CommonEventNumberArgInitValue) 0
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
                result.UpdateSpecialStringArgDesc((CommonEventStringArgIndex) 0,
                    new CommonEventSpecialStringArgDesc
                    {
                        ArgName = (CommonEventArgName) ""
                    });
                result.UpdateSpecialStringArgDesc((CommonEventStringArgIndex) 1,
                    new CommonEventSpecialStringArgDesc
                    {
                        ArgName = (CommonEventArgName) ""
                    });

                /* ---------- セルフ変数名 ---------- */
                result.UpdateVariableName((CommonEventSelfVariableIndex) 0, (CommonEventSelfVariableName) "Summary1");
                result.UpdateVariableName((CommonEventSelfVariableIndex) 1, (CommonEventSelfVariableName) "引数2");
                result.UpdateVariableName((CommonEventSelfVariableIndex) 2, (CommonEventSelfVariableName) "引数3");
                result.UpdateVariableName((CommonEventSelfVariableIndex) 3, (CommonEventSelfVariableName) "Summary4");
                result.UpdateVariableName((CommonEventSelfVariableIndex) 50, (CommonEventSelfVariableName) "Test");
                result.UpdateVariableName((CommonEventSelfVariableIndex) 51, (CommonEventSelfVariableName) "てすと");

                result.SetReturnValueNone();

                return result;
            }

            public static CommonEvent GenerateCommonEvent004()
            {
                var result = new CommonEvent
                {
                    Id = (CommonEventId) 4,
                    BootCondition = new CommonEventBootCondition
                    {
                        CommonEventBootType = CommonEventBootType.Auto,
                        LeftSide = (VariableAddress) 2000000,
                        Operation = CriteriaOperator.Equal,
                        RightSide = (ConditionRight) 10
                    },
                    NumberArgsLength = 0,
                    StrArgsLength = 0,
                    Name = (CommonEventName) "コモンイベント004",
                    EventCommands = new EventCommandList(
                        new List<IEventCommand>
                        {
                            EventCommandFactory.CreateCommandString("[0][0,0]<0>()()"),
                        }),
                    Description = (CommonEventDescription) "",
                    Memo = (CommonEventMemo) "",
                    LabelColor = CommonEventLabelColor.Black,
                    FooterString = (CommonEventFooterString) "",
                };

                return result;
            }

            public static CommonEvent GenerateCommonEvent005()
            {
                var result = new CommonEvent
                {
                    Id = (CommonEventId) 5,
                    BootCondition = new CommonEventBootCondition
                    {
                        CommonEventBootType = CommonEventBootType.Auto,
                        LeftSide = (VariableAddress) 2000004,
                        Operation = CriteriaOperator.Above,
                        RightSide = (ConditionRight) 10
                    },
                    NumberArgsLength = 0,
                    StrArgsLength = 0,
                    Name = (CommonEventName) "コモンイベント005",
                    EventCommands = new EventCommandList(
                        new List<IEventCommand>
                        {
                            EventCommandFactory.CreateCommandString("[0][0,0]<0>()()"),
                        }),
                    Description = (CommonEventDescription) "",
                    Memo = (CommonEventMemo) "",
                    LabelColor = CommonEventLabelColor.Green,
                    FooterString = (CommonEventFooterString) "",
                };

                return result;
            }

            public static CommonEvent GenerateCommonEvent006()
            {
                var result = new CommonEvent
                {
                    Id = (CommonEventId) 6,
                    BootCondition = new CommonEventBootCondition
                    {
                        CommonEventBootType = CommonEventBootType.ParallelAlways,
                        LeftSide = (VariableAddress) 2000000,
                        Operation = CriteriaOperator.Above,
                        RightSide = (ConditionRight) 10
                    },
                    NumberArgsLength = 4,
                    StrArgsLength = 4,
                    Name = (CommonEventName) "コモンイベント006",
                    EventCommands = new EventCommandList(
                        new List<IEventCommand>
                        {
                            EventCommandFactory.CreateCommandString("[0][0,0]<0>()()"),
                        }),
                    Description = (CommonEventDescription) "",
                    Memo = (CommonEventMemo) "",
                    LabelColor = CommonEventLabelColor.Black,
                    FooterString = (CommonEventFooterString) "",
                };

                /* ---------- セルフ変数名 ---------- */
                result.UpdateVariableName((CommonEventSelfVariableIndex) 0,
                    (CommonEventSelfVariableName) "NumArg1Name");
                result.UpdateVariableName((CommonEventSelfVariableIndex) 1,
                    (CommonEventSelfVariableName) "NumArg2Name");
                result.UpdateVariableName((CommonEventSelfVariableIndex) 2,
                    (CommonEventSelfVariableName) "NumArg3Name");
                result.UpdateVariableName((CommonEventSelfVariableIndex) 3,
                    (CommonEventSelfVariableName) "NumArg4Name");
                result.UpdateVariableName((CommonEventSelfVariableIndex) 5,
                    (CommonEventSelfVariableName) "StrArg1Name");
                result.UpdateVariableName((CommonEventSelfVariableIndex) 6,
                    (CommonEventSelfVariableName) "StrArg2Name");
                result.UpdateVariableName((CommonEventSelfVariableIndex) 7,
                    (CommonEventSelfVariableName) "StrArg3Name");
                result.UpdateVariableName((CommonEventSelfVariableIndex) 8,
                    (CommonEventSelfVariableName) "StrArg4Name");

                /* ---------- 数値引数 ---------- */
                result.UpdateSpecialNumberArgDesc((CommonEventNumberArgIndex) 0,
                    ((Func<CommonEventSpecialNumberArgDesc>) (() =>
                    {
                        var desc = new CommonEventSpecialNumberArgDesc
                        {
                            ArgName = (CommonEventArgName) "NumArg1Name",
                            InitValue = (CommonEventNumberArgInitValue) 0
                        };
                        desc.ChangeArgType(CommonEventArgType.Normal, null);
                        return desc;
                    }))());
                result.UpdateSpecialNumberArgDesc((CommonEventNumberArgIndex) 1,
                    ((Func<CommonEventSpecialNumberArgDesc>) (() =>
                    {
                        var desc = new CommonEventSpecialNumberArgDesc
                        {
                            ArgName = (CommonEventArgName) "NumArg2Name",
                            InitValue = (CommonEventNumberArgInitValue) 0
                        };
                        desc.ChangeArgType(CommonEventArgType.Normal, null);
                        return desc;
                    }))());
                result.UpdateSpecialNumberArgDesc((CommonEventNumberArgIndex) 2,
                    ((Func<CommonEventSpecialNumberArgDesc>) (() =>
                    {
                        var desc = new CommonEventSpecialNumberArgDesc
                        {
                            ArgName = (CommonEventArgName) "NumArg3Name",
                            InitValue = (CommonEventNumberArgInitValue) 0
                        };
                        desc.ChangeArgType(CommonEventArgType.Normal, null);
                        return desc;
                    }))());
                result.UpdateSpecialNumberArgDesc((CommonEventNumberArgIndex) 3,
                    ((Func<CommonEventSpecialNumberArgDesc>) (() =>
                    {
                        var desc = new CommonEventSpecialNumberArgDesc
                        {
                            ArgName = (CommonEventArgName) "NumArg4Name",
                            InitValue = (CommonEventNumberArgInitValue) 0
                        };
                        desc.ChangeArgType(CommonEventArgType.Normal, null);
                        return desc;
                    }))());
                result.UpdateSpecialNumberArgDesc((CommonEventNumberArgIndex) 4,
                    ((Func<CommonEventSpecialNumberArgDesc>) (() =>
                    {
                        var desc = new CommonEventSpecialNumberArgDesc
                        {
                            ArgName = (CommonEventArgName) "N",
                            InitValue = (CommonEventNumberArgInitValue) 0
                        };
                        desc.ChangeArgType(CommonEventArgType.Normal, null);
                        return desc;
                    }))());

                /* ---------- 文字列引数 ---------- */
                result.UpdateSpecialStringArgDesc((CommonEventStringArgIndex) 0,
                    new CommonEventSpecialStringArgDesc
                    {
                        ArgName = (CommonEventArgName) "StrArg1Name"
                    });
                result.UpdateSpecialStringArgDesc((CommonEventStringArgIndex) 1,
                    new CommonEventSpecialStringArgDesc
                    {
                        ArgName = (CommonEventArgName) "StrArg2Name"
                    });
                result.UpdateSpecialStringArgDesc((CommonEventStringArgIndex) 2,
                    new CommonEventSpecialStringArgDesc
                    {
                        ArgName = (CommonEventArgName) "StrArg3Name"
                    });
                result.UpdateSpecialStringArgDesc((CommonEventStringArgIndex) 3,
                    new CommonEventSpecialStringArgDesc
                    {
                        ArgName = (CommonEventArgName) "StrArg4Name"
                    });
                result.UpdateSpecialStringArgDesc((CommonEventStringArgIndex) 4,
                    new CommonEventSpecialStringArgDesc
                    {
                        ArgName = (CommonEventArgName) "U"
                    });

                result.SetReturnValueNone();

                return result;
            }

            public static CommonEvent GenerateCommonEvent007()
            {
                var result = new CommonEvent
                {
                    Id = (CommonEventId) 7,
                    BootCondition = new CommonEventBootCondition
                    {
                        CommonEventBootType = CommonEventBootType.ParallelAlways,
                        LeftSide = (VariableAddress) 2000000,
                        Operation = CriteriaOperator.Equal,
                        RightSide = (ConditionRight) 0
                    },
                    NumberArgsLength = 1,
                    StrArgsLength = 0,
                    Name = (CommonEventName) "コモンイベント007",
                    EventCommands = new EventCommandList(
                        new List<IEventCommand>
                        {
                            EventCommandFactory.CreateCommandString("[0][0,0]<0>()()"),
                        }),
                    Description = (CommonEventDescription) "",
                    Memo = (CommonEventMemo) "",
                    LabelColor = CommonEventLabelColor.Black,
                    FooterString = (CommonEventFooterString) "",
                };

                /* ---------- 数値引数 ---------- */
                result.UpdateSpecialNumberArgDesc((CommonEventNumberArgIndex) 0,
                    ((Func<CommonEventSpecialNumberArgDesc>) (() =>
                    {
                        var desc = new CommonEventSpecialNumberArgDesc
                        {
                            ArgName = (CommonEventArgName) "",
                            InitValue = (CommonEventNumberArgInitValue) 0
                        };
                        desc.ChangeArgType(CommonEventArgType.ReferDatabase, null);
                        desc.SetDatabaseRefer(DBKind.System, (TypeId) 0);
                        desc.SetDatabaseUseAdditionalItemsFlag(false);
                        return desc;
                    }))());

                result.SetReturnVariableIndex((CommonEventReturnVariableIndex) 41);
                result.ReturnValueDescription = (CommonEventResultDescription) "Return";

                return result;
            }
        }
    }
}