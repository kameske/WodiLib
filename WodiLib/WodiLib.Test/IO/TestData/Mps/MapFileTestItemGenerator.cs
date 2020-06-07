using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WodiLib.Database;
using WodiLib.Event;
using WodiLib.Event.CharaMoveCommand;
using WodiLib.Event.EventCommand;
using WodiLib.Map;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    /// <summary>
    /// MpsFileReader/Writerテスト時に比較するためのアイテムを生成する。
    /// </summary>
    public static class MapFileTestItemGenerator
    {
        #region CreateMapDataObject

        /** ========================================
         *  マップデータオブジェクト作成
         *  ======================================== */
        public static MapData GenerateMap023Data()
        {
            // マップ情報
            var memo = "なし";
            var tileSetId = 1;
            var mapSizeWidth = 22;
            var mapSizeHeight = 20;

            return new MapData
            {
                Memo = memo,
                TileSetId = tileSetId,

                // レイヤー1
                Layer1 = ((Func<Layer>) (() =>
                {
                    var chips = new List<List<MapChip>>();
                    for (var i = 0; i < mapSizeWidth; i++)
                    {
                        chips.Add(((Func<List<MapChip>>) (() =>
                        {
                            var lineChips = new List<MapChip>();
                            lineChips.Add(0);
                            lineChips.Add(1);
                            lineChips.Add(2);
                            for (var j = 0; j < 3; j++)
                            {
                                lineChips.Add(3);
                            }

                            for (var j = 0; j < 14; j++)
                            {
                                lineChips.Add(0);
                            }

                            return lineChips;
                        }))());
                    }

                    var layer = new Layer
                    {
                        Chips = new MapChipList(chips)
                    };
                    return layer;
                }))(),

                // レイヤー2
                Layer2 = ((Func<Layer>) (() =>
                {
                    return new Layer
                    {
                        Chips = new MapChipList(new List<List<MapChip>>
                        {
                            Enumerable.Range(1, 20).Select(_ => (MapChip) 204444).ToList(),
                            new List<MapChip>
                            {
                                204444, 204443, 204141, 204343, 204141,
                                204141, 204343, 204141, 204141, 204141,
                                204343, 204141, 204141, 204141, 204141,
                                204344, 204444, 204444, 204444, 204444
                            },
                            new List<MapChip>
                            {
                                204444, 204422, 300000, 202222, 300011,
                                301100, 202222, 300213, 301111, 301100,
                                202222, 300214, 301414, 301414, 301402,
                                202244, 204444, 204444, 204444, 204444
                            },
                            new List<MapChip>
                            {
                                204444, 204433, 201111, 203333, 201111,
                                201111, 203322, 302222, 200214, 201414,
                                203422, 302244, 304444, 304444, 304422,
                                202244, 204444, 204444, 204444, 204444
                            },
                            new List<MapChip>
                            {
                                204444, 204422, 300202, 202222, 300214,
                                301402, 202222, 302020, 202244, 204444,
                                204422, 302244, 304444, 304443, 304120,
                                202244, 204444, 204444, 204444, 204444
                            },
                            new List<MapChip>
                            {
                                204444, 204422, 302020, 202222, 302041,
                                304120, 202233, 201111, 203141, 204141,
                                204322, 302041, 304141, 304120, 200214,
                                203444, 204444, 204444, 204444, 204444
                            },
                            new List<MapChip>
                            {
                                204444, 204434, 201413, 203131, 201314,
                                201414, 203422, 300011, 301313, 301100,
                                202234, 201414, 201413, 201111, 203344,
                                204444, 204444, 204444, 204444, 204444
                            },
                            new List<MapChip>
                            {
                                204444, 204443, 204120, 300202, 202041,
                                204344, 204433, 201100, 302222, 200011,
                                203343, 204141, 204120, 300202, 202244,
                                204444, 204444, 204444, 204444, 204444
                            },
                            new List<MapChip>
                            {
                                204444, 204422, 300011, 303333, 301100,
                                202244, 204422, 300011, 303131, 301100,
                                202222, 300214, 301414, 303422, 202244,
                                204444, 204444, 204444, 204444, 204444
                            },
                            new List<MapChip>
                            {
                                204444, 204434, 201402, 302020, 200214,
                                203443, 204131, 201313, 201111, 201314,
                                203422, 302243, 304141, 304120, 202244,
                                204444, 204444, 204444, 204444, 204444
                            },
                            new List<MapChip>
                            {
                                204444, 204443, 204131, 201111, 203141,
                                204322, 300202, 202020, 300202, 202244,
                                204422, 302020, 200214, 201414, 203444,
                                204444, 204444, 204444, 204444, 204444
                            },
                            new List<MapChip>
                            {
                                204444, 204422, 300213, 301111, 301302,
                                202222, 302233, 301111, 303322, 202243,
                                204131, 201111, 203141, 204344, 204444,
                                204444, 204444, 204444, 204444, 204444
                            },
                            new List<MapChip>
                            {
                                204444, 204422, 302222, 200000, 302222,
                                202222, 302020, 200202, 302020, 202222,
                                300011, 301314, 301402, 202041, 204344,
                                204444, 204444, 204444, 204444, 204444
                            },
                            new List<MapChip>
                            {
                                204444, 204422, 302031, 301111, 303120,
                                202233, 201111, 203131, 201314, 203434,
                                201402, 302041, 304131, 301100, 202244,
                                204444, 204444, 204444, 204444, 204444
                            },
                            new List<MapChip>
                            {
                                204444, 204434, 201413, 201111, 201314,
                                203422, 300214, 301402, 202041, 204343,
                                204131, 201111, 201313, 201111, 203344,
                                204444, 204444, 204444, 204444, 204444
                            },
                            new List<MapChip>
                            {
                                204444, 204443, 204120, 300202, 202041,
                                204322, 302041, 304334, 301402, 202222,
                                300214, 301402, 202020, 300202, 202041,
                                204344, 204444, 204444, 204444, 204444
                            },
                            new List<MapChip>
                            {
                                204444, 204422, 300214, 303434, 301402,
                                202234, 201402, 302041, 304120, 202222,
                                302041, 304333, 301111, 303334, 301402,
                                202244, 204444, 204444, 204444, 204444
                            },
                            new List<MapChip>
                            {
                                204444, 204422, 302041, 304343, 304120,
                                202244, 204434, 201414, 201414, 203434,
                                201402, 302020, 200202, 302041, 304120,
                                202244, 204444, 204444, 204444, 204444
                            },
                            new List<MapChip>
                            {
                                204444, 204434, 201402, 302020, 200214,
                                203444, 204444, 204444, 204444, 204444,
                                204434, 201414, 203434, 201414, 201414,
                                203444, 204444, 204444, 204444, 204444
                            },
                            new List<MapChip>
                            {
                                204444, 204444, 204434, 201414, 203444,
                                204444, 204444, 204444, 204444, 204444,
                                204444, 204444, 204444, 204444, 204444,
                                204444, 204444, 204444, 204444, 204444
                            },
                            new List<MapChip>
                            {
                                204444, 204444, 204444, 204444, 204444,
                                204444, 204444, 204444, 204444, 204444,
                                204444, 204444, 204444, 204444, 204444,
                                204444, 204444, 204444, 204444, 204444
                            },
                            new List<MapChip>
                            {
                                204444, 204444, 204444, 204444, 204444,
                                204444, 204444, 204444, 204444, 204444,
                                204444, 204444, 204444, 204444, 204444,
                                204444, 204444, 204444, 204444, 204444
                            },
                        })
                    };
                }))(),

                // レイヤー3
                Layer3 = GenerateEmptyLayer(mapSizeWidth, mapSizeHeight),

                // マップイベント
                MapEvents = new MapEventList(new List<MapEvent>
                {
                    // マップイベント00
                    new MapEvent
                    {
                        EventName = "マップイベント",
                        MapEventId = 0,
                        Position = (1, 1),
                        MapEventPageList = new MapEventPageList(new List<MapEventPage>
                        {
                            // マップイベント00 - ページ1
                            new MapEventPage
                            {
                                GraphicInfo = new MapEventPageGraphicInfo
                                {
                                    IsGraphicTileChip = false,
                                    CharaChipFilePath = "CharaChip/[Animal]Chicken.png",
                                    CharaChipOpacity = 255,
                                    CharaChipDrawType = PictureDrawType.Normal
                                },
                                BootInfo = new MapEventPageBootInfo
                                {
                                    MapEventBootType = MapEventBootType.Parallel,
                                    MapEventBootCondition1 = new MapEventBootCondition
                                    {
                                        UseCondition = true,
                                        LeftSide = 1100000,
                                        Operation = CriteriaOperator.Equal,
                                        RightSide = 22
                                    },
                                    MapEventBootCondition2 = new MapEventBootCondition
                                    {
                                        UseCondition = true,
                                        LeftSide = 1100003,
                                        Operation = CriteriaOperator.Not,
                                        RightSide = 13002
                                    },
                                    MapEventBootCondition3 = new MapEventBootCondition
                                    {
                                        UseCondition = true,
                                        LeftSide = 2200000,
                                        Operation = CriteriaOperator.Greater,
                                        RightSide = (-35)
                                    },
                                    MapEventBootCondition4 = new MapEventBootCondition()
                                },
                                MoveRouteInfo = new MapEventPageMoveRouteInfo
                                {
                                    AnimateSpeed = AnimateSpeed.Middle,
                                    MoveSpeed = MoveSpeed.Normal,
                                    MoveFrequency = MoveFrequency.Middle,
                                    MoveType = MoveType.Not,
                                    CustomMoveRoute = null,
                                },
                                Option = new MapEventPageOption
                                {
                                    IsWaitAnimationOn = true,
                                    IsMoveAnimationOn = true,
                                    IsFixedDirection = true,
                                    IsSkipThrough = true,
                                    IsAboveHero = true,
                                    IsHitBox = true,
                                    IsPlaceHalfStepUp = true
                                },
                                HitExtendRange = (4, 2),
                                ShadowGraphicId = 1,
                                // イベントコマンド
                                EventCommands = new EventCommandList(new List<IEventCommand>
                                {
                                    new Message
                                    {
                                        Text = "文章の表示",
                                        Indent = 0
                                    },
                                    new Comment
                                    {
                                        Text = "コメント",
                                        Indent = 0
                                    },
                                    new Comment
                                    {
                                        Text = "改行",
                                        Indent = 0
                                    },
                                    new DebugText
                                    {
                                        Text = "デバッグ文\r\n\\cdb[0:1:2]",
                                        Indent = 0
                                    },
                                    new ClearDebugText {Indent = 0},
                                    new StopForceMessage {Indent = 0},
                                    new ChoiceStart
                                    {
                                        CancelForkIndex = ChoiceCancelForkType.Case5,
                                        CaseValue = 5,
                                        Case1 = "選択肢A",
                                        Case2 = "選択肢B",
                                        Case3 = "選択肢C",
                                        Case4 = "選択肢D",
                                        Case5 = "選択肢E（キャンセル分岐）",
                                        IsForkLeftKey = true,
                                        IsForkRightKey = true,
                                        IsStopForce = false,
                                        Indent = 0
                                    },
                                    new ForkStart
                                    {
                                        CaseNumber = 0,
                                        Indent = 0
                                    },
                                    new MoveRoute
                                    {
                                        ActionEntry = new ActionEntry
                                        {
                                            CommandList = new CharaMoveCommandList
                                            {
                                                new MoveLeftUp(),
                                                new MoveUp(),
                                                new MoveRightUp(),
                                                new MoveRight(),
                                                new MoveRightDown(),
                                                new MoveDown(),
                                                new MoveLeftDown(),
                                                new MoveLeft()
                                            },
                                            IsRepeatAction = false,
                                            IsSkipIfCannotMove = true,
                                            IsWaitForComplete = false,
                                        },
                                        Indent = 1,
                                        Target = MapCharacterId.ThisMapEvent
                                    },
                                    new MoveRoute
                                    {
                                        ActionEntry = new ActionEntry
                                        {
                                            CommandList = new CharaMoveCommandList
                                            {
                                                new MoveRandom(),
                                                new StepForward(),
                                                new MoveTowardHero(),
                                                new StepBackward(),
                                                new MoveAwayFromHero(),
                                                new Jump
                                                {
                                                    DownPoint = 2,
                                                    RightPoint = 1
                                                },
                                                new Jump
                                                {
                                                    DownPoint = 1100005,
                                                    RightPoint = 1100001
                                                },
                                                new ApproachEvent
                                                {
                                                    EventId = 4
                                                },
                                                new ApproachEvent
                                                {
                                                    EventId = 2000003
                                                },
                                                new ApproachPosition
                                                {
                                                    PositionX = 1,
                                                    PositionY = 2
                                                },
                                                new ApproachPosition
                                                {
                                                    PositionX = 1000024,
                                                    PositionY = 1000025
                                                }
                                            },
                                        },
                                        Indent = 1,
                                        Target = MapCharacterId.Hero
                                    },
                                    new MoveRoute
                                    {
                                        ActionEntry = new ActionEntry
                                        {
                                            CommandList = new CharaMoveCommandList
                                            {
                                                new ChangeGraphic
                                                {
                                                    GraphicId = 3
                                                },
                                                new ChangePenetration
                                                {
                                                    Opacity = 4
                                                },
                                                new ChangeHeight
                                                {
                                                    Height = 5
                                                },
                                                new PlaySE
                                                {
                                                    SoundId = 6
                                                },
                                                new WaitMoveCommand
                                                {
                                                    Frame = 7
                                                }
                                            },
                                            IsRepeatAction = false,
                                            IsSkipIfCannotMove = true,
                                            IsWaitForComplete = false
                                        },
                                        Indent = 1,
                                        Target = MapCharacterId.Member2
                                    },
                                    new MoveRoute
                                    {
                                        ActionEntry = new ActionEntry
                                        {
                                            CommandList = new CharaMoveCommandList
                                            {
                                                new LookLeftUp(),
                                                new LookUp(),
                                                new LookRightUp(),
                                                new LookRight(),
                                                new LookRightDown(),
                                                new LookDown(),
                                                new LookLeftDown(),
                                                new LookLeft(),
                                                new TurnRight(),
                                                new TurnRandom(),
                                                new TurnLeft(),
                                                new TurnRound(),
                                                new TurnLorR(),
                                                new TurnTail()
                                            },
                                            IsRepeatAction = false,
                                            IsSkipIfCannotMove = false,
                                            IsWaitForComplete = true
                                        },
                                        Indent = 1,
                                        Target = 3
                                    },
                                    new MoveRoute
                                    {
                                        ActionEntry = new ActionEntry
                                        {
                                            CommandList = new CharaMoveCommandList
                                            {
                                                new AssignValue
                                                {
                                                    TargetAddress = 1100006,
                                                    Value = 7
                                                },
                                                new AssignValue
                                                {
                                                    TargetAddress = 1100006,
                                                    Value = 1100003
                                                },
                                                new AddValue
                                                {
                                                    TargetAddress = 1100007,
                                                    Value = 7
                                                },
                                                new AddValue
                                                {
                                                    TargetAddress = 1100008,
                                                    Value = 1100000
                                                },
                                                new SetMoveSpeed
                                                {
                                                    Value = MoveSpeed.Slower
                                                },
                                                new SetMoveFrequency
                                                {
                                                    Value = MoveFrequency.Short,
                                                },
                                                new SetAnimateSpeed
                                                {
                                                    Value = AnimateSpeed.Middle,
                                                },
                                                new ValidReadinessAnimation(),
                                                new ValidSnake(),
                                                new InvalidReadinessAnimation(),
                                                new InvalidSnake(),
                                                new ValidMoveAnimation(),
                                                new ValidDrawForefront(),
                                                new InvalidMoveAnimation(),
                                                new InvalidDrawForefront(),
                                                new ValidFixDirection(),
                                                new InvalidFixDirection()
                                            },
                                            IsRepeatAction = true,
                                            IsSkipIfCannotMove = false,
                                            IsWaitForComplete = false
                                        },
                                        Indent = 1,
                                        Target = 1100003
                                    },
                                    new MoveRoute
                                    {
                                        ActionEntry = new ActionEntry
                                        {
                                            CommandList = new CharaMoveCommandList
                                            {
                                                new SetStepHalf(),
                                                new SetStepFull(),
                                                new ChangePatternFirst(),
                                                new ChangePatternSecond(),
                                                new ChangePatternThird(),
                                                new ChangePatternFourth(),
                                                new ChangePatternFifth()
                                            },
                                            IsRepeatAction = false,
                                            IsSkipIfCannotMove = false,
                                            IsWaitForComplete = false
                                        },
                                        Indent = 1,
                                        Target = MapEventId.ThisMapEvent
                                    },
                                    new Blank {Indent = 1},
                                    new ForkStart
                                    {
                                        CaseNumber = 1,
                                        Indent = 0
                                    },
                                    new Blank {Indent = 1},
                                    new ForkStart
                                    {
                                        CaseNumber = 2,
                                        Indent = 0
                                    },
                                    new Blank {Indent = 1},
                                    new ForkStart
                                    {
                                        CaseNumber = 3,
                                        Indent = 0
                                    },
                                    new ChoiceStart
                                    {
                                        CaseValue = 2,
                                        Case1 = "選択肢A",
                                        Case2 = "選択肢B",
                                        CancelForkIndex = ChoiceCancelForkType.Else,
                                        IsForkLeftKey = true,
                                        IsForkRightKey = false,
                                        IsStopForce = true,
                                        Indent = 1
                                    },
                                    new ForkStart
                                    {
                                        CaseNumber = 0,
                                        Indent = 1,
                                    },
                                    new BreakChoiceForce {Indent = 2},
                                    new Blank {Indent = 2},
                                    new ForkStart
                                    {
                                        CaseNumber = 1,
                                        Indent = 1
                                    },
                                    new Blank {Indent = 2},
                                    new ChoiceStartForkingLeftKey {Indent = 1},
                                    new Blank {Indent = 2},
                                    new ChoiceStartForkingForceExit {Indent = 1},
                                    new Blank {Indent = 2},
                                    new ChoiceStartForkingCancel {Indent = 1},
                                    new CsvIO
                                    {
                                        Mode = CsvIOMode.Input,
                                        DBKind = DBKind.User,
                                        DBTypeId = 1,
                                        DBDataId = 2,
                                        DBItemIndex = 2,
                                        IsTypeIdUseStr = false,
                                        IsDataIdUseStr = false,
                                        ItemLength = 10,
                                        FileName = "Data/db.csv",
                                        Indent = 2
                                    },
                                    new CsvIO
                                    {
                                        Mode = CsvIOMode.Output,
                                        DBKind = DBKind.Changeable,
                                        DBTypeId = (3, "パーティー情報"),
                                        DBDataId = "メイン設定",
                                        DBItemIndex = 7,
                                        IsTypeIdUseStr = true,
                                        IsDataIdUseStr = true,
                                        ItemLength = 10,
                                        FileName = "Data/db.csv",
                                        Indent = 2
                                    },
                                    new Blank {Indent = 2},
                                    new ForkEnd {Indent = 1},
                                    new Blank {Indent = 1},
                                    new ForkStart
                                    {
                                        Indent = 0,
                                        CaseNumber = 4
                                    },
                                    new Blank {Indent = 1},
                                    new ChoiceStartForkingLeftKey {Indent = 0},
                                    new Blank {Indent = 1},
                                    new ChoiceStartForkingRightKey {Indent = 0},
                                    new Blank {Indent = 1},
                                    new ForkEnd {Indent = 0},
                                    new Blank {Indent = 0},
                                }),
                            },
                        })
                    },
                })
            };
        }

        public static MapData GenerateMap024Data()
        {
            return new MapData
            {
                Memo = "なし",
                TileSetId = 0,
                Layer1 = GenerateEmptyLayer(20, 15),
                Layer2 = GenerateEmptyLayer(20, 15),
                Layer3 = GenerateEmptyLayer(20, 15),
                MapEvents = new MapEventList(new List<MapEvent>
                {
                    new MapEvent
                    {
                        EventName = "テストイベント1",
                        MapEventId = 0,
                        Position = (3, 1),
                        MapEventPageList = new MapEventPageList(new[]
                        {
                            new MapEventPage
                            {
                                GraphicInfo = new MapEventPageGraphicInfo
                                {
                                    IsGraphicTileChip = false,
                                    CharaChipFilePath = "",
                                    CharaChipDrawType = PictureDrawType.Normal,
                                    CharaChipOpacity = 255,
                                },
                                BootInfo = new MapEventPageBootInfo
                                {
                                    MapEventBootType = MapEventBootType.HitMapEvent,
                                    MapEventBootCondition1 = new MapEventBootCondition
                                    {
                                        UseCondition = true,
                                        LeftSide = 1100001,
                                        RightSide = 10,
                                        Operation = CriteriaOperator.Above
                                    },
                                    MapEventBootCondition2 = new MapEventBootCondition
                                    {
                                        UseCondition = true,
                                        LeftSide = 1100002,
                                        RightSide = 111,
                                        Operation = CriteriaOperator.Greater
                                    },
                                    MapEventBootCondition3 = new MapEventBootCondition
                                    {
                                        UseCondition = true,
                                        LeftSide = 2000000,
                                        RightSide = 30,
                                        Operation = CriteriaOperator.Less
                                    },
                                    MapEventBootCondition4 = new MapEventBootCondition
                                    {
                                        UseCondition = true,
                                        LeftSide = 2000001,
                                        RightSide = 220,
                                        Operation = CriteriaOperator.Not
                                    },
                                },
                                MoveRouteInfo = new MapEventPageMoveRouteInfo
                                {
                                    MoveType = MoveType.Nearer,
                                    MoveSpeed = MoveSpeed.Slower,
                                    MoveFrequency = MoveFrequency.Short,
                                    AnimateSpeed = AnimateSpeed.Longer,
                                },
                                Option = new MapEventPageOption
                                {
                                    IsWaitAnimationOn = true,
                                    IsMoveAnimationOn = true,
                                    IsFixedDirection = false,
                                    IsSkipThrough = false,
                                    IsAboveHero = false,
                                    IsHitBox = true,
                                    IsPlaceHalfStepUp = false,
                                },
                                HitExtendRange = (1, 3),
                                ShadowGraphicId = 2,
                                EventCommands = new EventCommandList(new[]
                                {
                                    EventCommandFactory.CreateCommandString(
                                        @"[121][4,0]<0>(1000000,1025000,2000000,68)()"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[121][7,0]<0>(1100000000,1100006,2000000,4409,3,2,10)()"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[121][4,0]<0>(2000003,1100006,2000000,8778)()"),
                                    EventCommandFactory.CreateCommandString(@"[121][4,0]<0>(2000003,30,400,17154)()"),
                                    EventCommandFactory.CreateCommandString(@"[121][5,0]<0>(2000004,30,400,87057,3)()"),
                                    EventCommandFactory.CreateCommandString(@"[121][4,0]<0>(2000004,30,400,25944)()"),
                                    EventCommandFactory.CreateCommandString(@"[121][4,0]<0>(2000004,30,40,1536)()"),
                                    EventCommandFactory.CreateCommandString(@"[121][4,0]<0>(2000004,30,40,1792)()"),
                                    EventCommandFactory.CreateCommandString(@"[121][4,0]<0>(2000004,30,40,2048)()"),
                                    EventCommandFactory.CreateCommandString(@"[121][4,0]<0>(2000004,30,40,63744)()"),
                                    EventCommandFactory.CreateCommandString(@"[121][4,0]<0>(2000004,30,40,2560)()"),
                                    EventCommandFactory.CreateCommandString(@"[121][4,0]<0>(2000004,30,40,2816)()"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[250][5,4]<0>(3,4,5,0,220)("""","""","""","""")"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[250][5,4]<0>(3,4,1,262160,220)("""","""","""",""メンバー1"")"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[250][5,4]<0>(3,4,2000000,33,220)("""","""","""","""")"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[250][5,4]<0>(3,0,1,131120,4)("""","""",""メイン設定"","""")"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[250][5,4]<0>(3,1100006,2,64,1100001)("""","""","""","""")"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[250][5,4]<0>(3,5,2,65617,1100000)("""",""パーティー情報"","""","""")"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[250][5,4]<0>(1100002,5,2,96,1100000)("""","""","""","""")"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[250][5,4]<0>(3,5,2,112,2700000)("""","""","""","""")"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[250][5,4]<0>(3,5,-2,112,0)("""","""","""","""")"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[250][5,4]<0>(1,-2,0,65648,0)("""",""┣ 技能習得Lv"","""","""")"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[250][5,4]<0>(1,3,6,4096,1100000)("""","""","""","""")"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[250][5,4]<0>(3,2,0,266241,1100001)("""","""","""",""所持金"")"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[250][5,4]<0>(1,0,-1,4096,1100001)("""","""","""","""")"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[250][5,4]<0>(1,-3,2,4096,1100001)("""","""","""","""")"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[250][5,4]<0>(2,-3,0,266240,1100001)("""","""","""",""\udb[8:0]の残り歩数"")"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[250][5,4]<0>(3,0,-3,200704,1100001)("""",""パーティー情報"",""メイン設定"","""")"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[250][5,4]<0>(6,-1,0,69632,1100001)("""",""┗所持防具リスト"","""","""")"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[250][5,4]<0>(2,0,-3,4096,1100001)("""","""","""","""")"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[250][5,4]<0>(1,-3,-3,4096,1100001)("""","""","""","""")"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[250][5,4]<0>(3,-3,-3,69632,1100001)("""",""パーティー情報"","""","""")"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[250][5,4]<0>(2,1,0,332032,1100001)("""",""BGSリスト"","""",""ファイル名"")"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[250][5,4]<0>(1,1,-3,135424,1100001)("""","""","""","""")"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[250][5,4]<0>(2,-1,0,4608,1100001)("""","""","""","""")"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[250][5,4]<0>(3,2000002,2,332288,1100001)("""",""UDB3"","""",""項目"")"),
                                    EventCommandFactory.CreateCommandString(@"[122][2,1]<0>(9900000,0)(""文字列入力"")"),
                                    EventCommandFactory.CreateCommandString(@"[122][3,0]<0>(2000002,273,9900000)()"),
                                    EventCommandFactory.CreateCommandString(@"[122][3,0]<0>(9900002,4611,15)()"),
                                    EventCommandFactory.CreateCommandString(@"[122][3,0]<0>(3000000,8963,15)()"),
                                    EventCommandFactory.CreateCommandString(@"[122][3,0]<0>(3000002,1026,2000010)()"),
                                    EventCommandFactory.CreateCommandString(@"[122][3,0]<0>(3000002,1281,9900000)()"),
                                    EventCommandFactory.CreateCommandString(@"[122][3,0]<0>(3000002,1537,9900000)()"),
                                    EventCommandFactory.CreateCommandString(@"[122][3,0]<0>(3000002,1793,9900000)()"),
                                    EventCommandFactory.CreateCommandString(@"[122][3,0]<0>(3000002,2049,9900000)()"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[122][2,2]<0>(3000002,2304)(""文字列入力"",""置換先"")"),
                                    EventCommandFactory.CreateCommandString(@"[122][2,1]<0>(3000002,2560)(""文字列入力"")"),
                                    EventCommandFactory.CreateCommandString(@"[122][2,1]<0>(3000002,2816)(""文字列入力"")"),
                                    EventCommandFactory.CreateCommandString(@"[0][0,0]<0>()()")
                                })
                            }
                        })
                    },
                    new MapEvent
                    {
                        EventName = "テストイベント2",
                        MapEventId = 1,
                        Position = (4, 8),
                        MapEventPageList = new MapEventPageList(new[]
                        {
                            new MapEventPage
                            {
                                GraphicInfo = new MapEventPageGraphicInfo
                                {
                                    IsGraphicTileChip = false,
                                    CharaChipFilePath = "",
                                    CharaChipDrawType = PictureDrawType.Normal,
                                    CharaChipOpacity = 255,
                                },
                                BootInfo = new MapEventPageBootInfo
                                {
                                    MapEventBootType = MapEventBootType.Parallel,
                                },
                                MoveRouteInfo = new MapEventPageMoveRouteInfo
                                {
                                    MoveType = MoveType.Not,
                                    MoveSpeed = MoveSpeed.Slowest,
                                    MoveFrequency = MoveFrequency.Longer,
                                    AnimateSpeed = AnimateSpeed.Short,
                                },
                                Option = new MapEventPageOption
                                {
                                    IsWaitAnimationOn = false,
                                    IsMoveAnimationOn = false,
                                    IsFixedDirection = true,
                                    IsSkipThrough = true,
                                    IsAboveHero = false,
                                    IsHitBox = false,
                                    IsPlaceHalfStepUp = true,
                                },
                                HitExtendRange = (0, 0),
                                ShadowGraphicId = 0,
                                EventCommands = new EventCommandList(new[]
                                {
                                    EventCommandFactory.CreateCommandString(
                                        @"[111][10,0]<0>(3,1100000,20,2,1000000,1024000000,0,1000014,2900034,20)()"),
                                    EventCommandFactory.CreateCommandString(@"[401][1,0]<0>(1)()"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[111][7,0]<1>(18,1130001004,180,3,2000000,22,4)()"),
                                    EventCommandFactory.CreateCommandString(@"[401][1,0]<1>(1)()"),
                                    EventCommandFactory.CreateCommandString(@"[0][0,0]<2>()()"),
                                    EventCommandFactory.CreateCommandString(@"[401][1,0]<1>(2)()"),
                                    EventCommandFactory.CreateCommandString(@"[0][0,0]<2>()()"),
                                    EventCommandFactory.CreateCommandString(@"[420][1,0]<1>(0)()"),
                                    EventCommandFactory.CreateCommandString(@"[111][4,0]<2>(1,9000003,3,6)()"),
                                    EventCommandFactory.CreateCommandString(@"[401][1,0]<2>(1)()"),
                                    EventCommandFactory.CreateCommandString(@"[0][0,0]<3>()()"),
                                    EventCommandFactory.CreateCommandString(@"[499][0,0]<2>()()"),
                                    EventCommandFactory.CreateCommandString(@"[0][0,0]<2>()()"),
                                    EventCommandFactory.CreateCommandString(@"[499][0,0]<1>()()"),
                                    EventCommandFactory.CreateCommandString(@"[0][0,0]<1>()()"),
                                    EventCommandFactory.CreateCommandString(@"[401][1,0]<0>(2)()"),
                                    EventCommandFactory.CreateCommandString(@"[0][0,0]<1>()()"),
                                    EventCommandFactory.CreateCommandString(@"[401][1,0]<0>(3)()"),
                                    EventCommandFactory.CreateCommandString(@"[0][0,0]<1>()()"),
                                    EventCommandFactory.CreateCommandString(@"[499][0,0]<0>()()"),
                                    EventCommandFactory.CreateCommandString(@"[0][0,0]<0>()()"),
                                })
                            }
                        })
                    }
                })
            };
        }

        public static MapData GenerateMap025Data()
        {
            return new MapData
            {
                Memo = "なし",
                TileSetId = 1,
                Layer1 = GenerateEmptyLayer(20, 15),
                Layer2 = GenerateEmptyLayer(20, 15),
                Layer3 = GenerateEmptyLayer(20, 15),
                MapEvents = new MapEventList(new[]
                {
                    new MapEvent
                    {
                        EventName = "CharaChip",
                        MapEventId = 0,
                        Position = (4, 2),
                        MapEventPageList = new MapEventPageList(new[]
                        {
                            new MapEventPage
                            {
                                GraphicInfo = new MapEventPageGraphicInfo
                                {
                                    IsGraphicTileChip = false,
                                    CharaChipFilePath = "CharaChip/[Special]Edy.png",
                                    CharaChipDrawType = PictureDrawType.Normal,
                                    CharaChipOpacity = 255
                                },
                                BootInfo = new MapEventPageBootInfo
                                {
                                    MapEventBootType = MapEventBootType.PushOKKey,
                                },
                                MoveRouteInfo = new MapEventPageMoveRouteInfo
                                {
                                    MoveType = MoveType.Not,
                                    MoveSpeed = MoveSpeed.Normal,
                                    MoveFrequency = MoveFrequency.Middle,
                                    AnimateSpeed = AnimateSpeed.Middle,
                                },
                                HitExtendRange = (4, 4),
                                ShadowGraphicId = 0,
                                EventCommands = new EventCommandList(new[]
                                {
                                    EventCommandFactory.CreateCommandString(
                                        @"[112][7,4]<0>(4,3000000,288212672,539870912,808306368,0,3000004)(""条件1"","""",""条件3"",""条件4"")"),
                                    EventCommandFactory.CreateCommandString(@"[401][1,0]<0>(1)()"),
                                    EventCommandFactory.CreateCommandString(@"[0][0,0]<1>()()"),
                                    EventCommandFactory.CreateCommandString(@"[401][1,0]<0>(2)()"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[112][9,4]<1>(20,3000000,288212672,556648128,825083584,0,3000000,3000010,3000020)(""条件1"","""","""","""")"),
                                    EventCommandFactory.CreateCommandString(@"[401][1,0]<1>(1)()"),
                                    EventCommandFactory.CreateCommandString(@"[0][0,0]<2>()()"),
                                    EventCommandFactory.CreateCommandString(@"[401][1,0]<1>(2)()"),
                                    EventCommandFactory.CreateCommandString(@"[0][0,0]<2>()()"),
                                    EventCommandFactory.CreateCommandString(@"[401][1,0]<1>(3)()"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[112][5,4]<2>(18,539870912,19777216,0,3000000)(""条件1"","""","""","""")"),
                                    EventCommandFactory.CreateCommandString(@"[401][1,0]<2>(1)()"),
                                    EventCommandFactory.CreateCommandString(@"[0][0,0]<3>()()"),
                                    EventCommandFactory.CreateCommandString(@"[401][1,0]<2>(2)()"),
                                    EventCommandFactory.CreateCommandString(@"[0][0,0]<3>()()"),
                                    EventCommandFactory.CreateCommandString(@"[420][1,0]<2>(0)()"),
                                    EventCommandFactory.CreateCommandString(@"[0][0,0]<3>()()"),
                                    EventCommandFactory.CreateCommandString(@"[499][0,0]<2>()()"),
                                    EventCommandFactory.CreateCommandString(@"[0][0,0]<2>()()"),
                                    EventCommandFactory.CreateCommandString(@"[401][1,0]<1>(4)()"),
                                    EventCommandFactory.CreateCommandString(@"[0][0,0]<2>()()"),
                                    EventCommandFactory.CreateCommandString(@"[420][1,0]<1>(0)()"),
                                    EventCommandFactory.CreateCommandString(@"[0][0,0]<2>()()"),
                                    EventCommandFactory.CreateCommandString(@"[499][0,0]<1>()()"),
                                    EventCommandFactory.CreateCommandString(@"[0][0,0]<1>()()"),
                                    EventCommandFactory.CreateCommandString(@"[401][1,0]<0>(3)()"),
                                    EventCommandFactory.CreateCommandString(@"[0][0,0]<1>()()"),
                                    EventCommandFactory.CreateCommandString(@"[401][1,0]<0>(4)()"),
                                    EventCommandFactory.CreateCommandString(@"[0][0,0]<1>()()"),
                                    EventCommandFactory.CreateCommandString(@"[499][0,0]<0>()()"),
                                    EventCommandFactory.CreateCommandString(@"[0][0,0]<0>()()"),
                                })
                            },
                            new MapEventPage
                            {
                                GraphicInfo = new MapEventPageGraphicInfo
                                {
                                    IsGraphicTileChip = true,
                                    GraphicTileId = 11,
                                    CharaChipDrawType = PictureDrawType.Normal,
                                    CharaChipOpacity = 255
                                },
                                MoveRouteInfo = new MapEventPageMoveRouteInfo
                                {
                                    MoveType = MoveType.Not,
                                    MoveSpeed = MoveSpeed.Normal,
                                    MoveFrequency = MoveFrequency.Middle,
                                    AnimateSpeed = AnimateSpeed.Middle,
                                    CustomMoveRoute = null
                                },
                                Option = new MapEventPageOption
                                {
                                    IsWaitAnimationOn = true,
                                    IsMoveAnimationOn = true,
                                    IsFixedDirection = false,
                                    IsSkipThrough = false,
                                    IsAboveHero = false,
                                    IsHitBox = true,
                                    IsPlaceHalfStepUp = false
                                },
                                HitExtendRange = (0, 0),
                                EventCommands = new EventCommandList(new[]
                                {
                                    EventCommandFactory.CreateCommandString(@"[123][2,0]<0>(2000000,241)()"),
                                    EventCommandFactory.CreateCommandString(@"[123][2,0]<0>(2000000,4)()"),
                                    EventCommandFactory.CreateCommandString(@"[123][3,0]<0>(2000000,260,100)()"),
                                    EventCommandFactory.CreateCommandString(@"[123][3,0]<0>(2000000,4356,130)()"),
                                    EventCommandFactory.CreateCommandString(@"[123][2,0]<0>(2000000,864)()"),
                                    EventCommandFactory.CreateCommandString(@"[123][2,0]<0>(2000000,770)()"),
                                    EventCommandFactory.CreateCommandString(@"[123][3,0]<0>(2000000,514,301)()"),
                                    EventCommandFactory.CreateCommandString(@"[123][3,0]<0>(2000000,4610,301)()"),
                                    EventCommandFactory.CreateCommandString(@"[125][1,0]<0>(226)()"),
                                    EventCommandFactory.CreateCommandString(@"[125][1,0]<0>(21)()"),
                                    EventCommandFactory.CreateCommandString(@"[125][2,0]<0>(268435456,122)()"),
                                    EventCommandFactory.CreateCommandString(@"[125][1,0]<0>(536870915)()"),
                                    EventCommandFactory.CreateCommandString(@"[125][3,0]<0>(536870933,3,0)()"),
                                    EventCommandFactory.CreateCommandString(@"[125][3,0]<0>(536870925,20,400)()"),
                                    EventCommandFactory.CreateCommandString(@"[126][1,0]<0>(247)()"),
                                    EventCommandFactory.CreateCommandString(@"[126][1,0]<0>(53)()"),
                                    EventCommandFactory.CreateCommandString(@"[126][1,0]<0>(211)()"),
                                    EventCommandFactory.CreateCommandString(@"[126][1,0]<0>(256)()"),
                                    EventCommandFactory.CreateCommandString(@"[126][1,0]<0>(641)()"),
                                    EventCommandFactory.CreateCommandString(@"[126][2,0]<0>(268435456,180)()"),
                                    EventCommandFactory.CreateCommandString(@"[126][1,0]<0>(268435713)()"),
                                    EventCommandFactory.CreateCommandString(@"[126][1,0]<0>(268435458)()"),
                                    EventCommandFactory.CreateCommandString(@"[126][1,0]<0>(268435459)()"),
                                    EventCommandFactory.CreateCommandString(@"[0][0,0]<0>()()"),
                                })
                            },
                            new MapEventPage
                            {
                                GraphicInfo = new MapEventPageGraphicInfo
                                {
                                    IsGraphicTileChip = true,
                                    GraphicTileId = 8,
                                    CharaChipDrawType = PictureDrawType.Normal,
                                    CharaChipOpacity = 255
                                },
                                MoveRouteInfo = new MapEventPageMoveRouteInfo
                                {
                                    MoveType = MoveType.Not,
                                    MoveSpeed = MoveSpeed.Normal,
                                    MoveFrequency = MoveFrequency.Middle,
                                    AnimateSpeed = AnimateSpeed.Middle,
                                    CustomMoveRoute = null
                                },
                                Option = new MapEventPageOption
                                {
                                    IsWaitAnimationOn = true,
                                    IsMoveAnimationOn = true,
                                    IsFixedDirection = false,
                                    IsSkipThrough = false,
                                    IsAboveHero = false,
                                    IsHitBox = true,
                                    IsPlaceHalfStepUp = false
                                },
                                HitExtendRange = (0, 0),
                                EventCommands = new EventCommandList(new[]
                                {
                                    EventCommandFactory.CreateCommandString(
                                        @"[150][18,1]<0>(0,120,20,4,3,1,255,200,300,95,18,0,33554432,30,0,101,102,103)(""CharaChip/[Animal]Chicken.png"")"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[150][19,0]<0>(19996944,120,1000030,1,10,1000010,1000020,1000000,1000001,1000060,1000050,3000020,33554432,1000040,123,1000080,1000081,1000082,1000070)()"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[150][15,1]<0>(54533920,120,11,1,10,-1000000,-1000000,10,20,-1000000,-1000000,0,16777216,22,123)(""表示文字列"")"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[150][18,1]<0>(33567280,120,11,4,4,3,65,10,20,1000060,4000,0,33554432,22,0,70,80,90)(""SystemGraphic/Text_Pause.png"")"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[150][18,0]<0>(33571392,120,11,4,4,3,65,10,20,1000060,4000,3000015,33554432,22,0,70,80,90)()"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[150][25,1]<0>(100667904,120,11,4,4,3,65,1,2,1000060,4000,0,33554432,22,0,70,80,90,0,3,4,5,6,7,8)(""3000015"")"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[150][18,0]<0>(33559041,100,11,0,0,3,65,30,32,110,4000,0,33554432,22,0,70,80,90)()"),
                                    EventCommandFactory.CreateCommandString(@"[150][4,0]<0>(33559042,100,11,22)()"),
                                    EventCommandFactory.CreateCommandString(@"[150][2,0]<0>(33559043,100)()"),
                                    EventCommandFactory.CreateCommandString(@"[0][0,0]<0>()()"),
                                })
                            },
                            new MapEventPage
                            {
                                GraphicInfo = new MapEventPageGraphicInfo
                                {
                                    IsGraphicTileChip = false,
                                    CharaChipFilePath = "",
                                    CharaChipDrawType = PictureDrawType.Normal,
                                    CharaChipOpacity = 255
                                },
                                BootInfo = new MapEventPageBootInfo
                                {
                                    MapEventBootType = MapEventBootType.PushOKKey,
                                    MapEventBootCondition1 = new MapEventBootCondition
                                    {
                                        UseCondition = true,
                                        LeftSide = 1000000, // ウディタ上で無編集のため、初期値のままのはず
                                        RightSide = 0,
                                        Operation = CriteriaOperator.Equal
                                    },
                                    MapEventBootCondition2 = new MapEventBootCondition
                                    {
                                        UseCondition = true,
                                        LeftSide = 1100001,
                                        RightSide = 1,
                                        Operation = CriteriaOperator.Less
                                    },
                                    MapEventBootCondition3 = new MapEventBootCondition
                                    {
                                        UseCondition = true,
                                        LeftSide = 1100002,
                                        RightSide = 2,
                                        Operation = CriteriaOperator.Less
                                    },
                                },
                                MoveRouteInfo = new MapEventPageMoveRouteInfo
                                {
                                    MoveType = MoveType.Not,
                                    MoveSpeed = MoveSpeed.Slow,
                                    MoveFrequency = MoveFrequency.Short,
                                    AnimateSpeed = AnimateSpeed.Long,
                                    CustomMoveRoute = null
                                },
                                Option = new MapEventPageOption
                                {
                                    IsWaitAnimationOn = false,
                                    IsMoveAnimationOn = false,
                                    IsFixedDirection = true,
                                    IsSkipThrough = true,
                                    IsAboveHero = false,
                                    IsHitBox = true,
                                    IsPlaceHalfStepUp = false
                                },
                                HitExtendRange = (1, 4),
                                ShadowGraphicId = 1,
                                EventCommands = new EventCommandList(new[]
                                {
                                    EventCommandFactory.CreateCommandString(@"[290][7,0]<0>(0,10,10,10,40,50,60)()"),
                                    EventCommandFactory.CreateCommandString(@"[290][7,0]<0>(16,10,10,20,40,50,60)()"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[290][7,0]<0>(32,1100003,2000000,2000001,1100000,1100001,1100002)()"),
                                    EventCommandFactory.CreateCommandString(@"[290][7,0]<0>(48,40,3,3,10,20,30)()"),
                                    EventCommandFactory.CreateCommandString(@"[290][7,0]<0>(64,40,3,3,10,20,30)()"),
                                    EventCommandFactory.CreateCommandString(@"[290][7,0]<0>(80,40,3,3,10,20,30)()"),
                                    EventCommandFactory.CreateCommandString(@"[290][7,0]<0>(96,40,3,3,10,20,30)()"),
                                    EventCommandFactory.CreateCommandString(@"[290][7,0]<0>(112,40,3,3,10,20,30)()"),
                                    EventCommandFactory.CreateCommandString(@"[290][7,0]<0>(128,40,3,3,10,20,30)()"),
                                    EventCommandFactory.CreateCommandString(@"[290][7,0]<0>(144,40,3,3,10,20,30)()"),
                                    EventCommandFactory.CreateCommandString(@"[290][7,0]<0>(160,40,3,3,10,20,30)()"),
                                    EventCommandFactory.CreateCommandString(@"[281][3,0]<0>(0,2,3)()"),
                                    EventCommandFactory.CreateCommandString(@"[281][3,0]<0>(273,2,3)()"),
                                    EventCommandFactory.CreateCommandString(@"[281][3,0]<0>(274,2,3)()"),
                                    EventCommandFactory.CreateCommandString(@"[281][3,0]<0>(275,2,3)()"),
                                    EventCommandFactory.CreateCommandString(@"[281][3,0]<0>(864,5,1)()"),
                                    EventCommandFactory.CreateCommandString(@"[151][2,0]<0>(3942420,10)()"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[151][5,0]<0>(16777216,1100003,1100000,1100001,1100002)()"),
                                    EventCommandFactory.CreateCommandString(@"[0][0,0]<0>()()"),
                                })
                            }
                        })
                    },
                    new MapEvent
                    {
                        EventName = "",
                        MapEventId = 1,
                        Position = (6, 8),
                        MapEventPageList = new MapEventPageList(new[]
                        {
                            new MapEventPage
                            {
                                GraphicInfo = new MapEventPageGraphicInfo
                                {
                                    IsGraphicTileChip = false,
                                    CharaChipFilePath = "",
                                    CharaChipDrawType = PictureDrawType.Normal,
                                    CharaChipOpacity = 255
                                },
                                BootInfo = new MapEventPageBootInfo
                                {
                                    MapEventBootType = MapEventBootType.PushOKKey,
                                },
                                MoveRouteInfo = new MapEventPageMoveRouteInfo
                                {
                                    MoveType = MoveType.Custom,
                                    MoveSpeed = MoveSpeed.Normal,
                                    MoveFrequency = MoveFrequency.Middle,
                                    AnimateSpeed = AnimateSpeed.Middle,
                                    CustomMoveRoute = new ActionEntry
                                    {
                                        IsWaitForComplete = true,
                                        IsRepeatAction = false,
                                        IsSkipIfCannotMove = true,
                                        CommandList = new CharaMoveCommandList
                                        {
                                            new MoveLeftUp(),
                                            new MoveUp(),
                                            new MoveRightUp(),
                                            new MoveRight(),
                                            new MoveRightDown(),
                                            new MoveDown(),
                                            new MoveLeftDown(),
                                            new MoveLeft(),
                                            new MoveRandom(),
                                            new StepForward(),
                                            new MoveTowardHero(),
                                            new StepBackward(),
                                            new MoveAwayFromHero(),
                                            new ApproachEvent {EventId = 10},
                                            new ApproachPosition
                                            {
                                                PositionX = 5,
                                                PositionY = 20
                                            },
                                            new Jump
                                            {
                                                RightPoint = 1,
                                                DownPoint = 4
                                            },
                                            new ChangeGraphic {GraphicId = 5},
                                            new ChangePenetration {Opacity = 6},
                                            new ChangeHeight {Height = 7},
                                            new PlaySE {SoundId = 8},
                                            new WaitMoveCommand {Frame = 9},
                                            new ApproachEvent {EventId = 1000020},
                                            new ApproachPosition
                                            {
                                                PositionX = 1000030,
                                                PositionY = 1000040,
                                            },
                                            new LookLeftUp(),
                                            new LookUp(),
                                            new LookRightUp(),
                                            new LookRight(),
                                            new LookRightDown(),
                                            new LookDown(),
                                            new LookLeftDown(),
                                            new LookLeft(),
                                            new TurnRight(),
                                            new TurnRandom(),
                                            new TurnLeft(),
                                            new TurnRound(),
                                            new TurnLorR(),
                                            new TurnTail(),
                                            new AssignValue
                                            {
                                                TargetAddress = 1100003,
                                                Value = 7
                                            },
                                            new AddValue
                                            {
                                                TargetAddress = 1100003,
                                                Value = 12
                                            },
                                            new SetMoveSpeed {Value = MoveSpeed.Slower},
                                            new SetMoveFrequency {Value = MoveFrequency.Middle},
                                            new SetAnimateSpeed {Value = AnimateSpeed.Longest},
                                            new ValidReadinessAnimation(),
                                            new ValidSnake(),
                                            new InvalidReadinessAnimation(),
                                            new InvalidSnake(),
                                            new ValidMoveAnimation(),
                                            new ValidDrawForefront(),
                                            new InvalidMoveAnimation(),
                                            new InvalidDrawForefront(),
                                            new ValidFixDirection(),
                                            new InvalidFixDirection(),
                                            new SetStepHalf(),
                                            new SetStepFull(),
                                            new ChangePatternFirst(),
                                            new ChangePatternSecond(),
                                            new ChangePatternThird(),
                                            new ChangePatternFourth(),
                                            new ChangePatternFifth(),
                                        }
                                    }
                                },
                                Option = new MapEventPageOption
                                {
                                    IsWaitAnimationOn = true,
                                    IsMoveAnimationOn = true,
                                    IsFixedDirection = false,
                                    IsSkipThrough = false,
                                    IsAboveHero = false,
                                    IsHitBox = true,
                                    IsPlaceHalfStepUp = false
                                },
                                HitExtendRange = (0, 0),
                                ShadowGraphicId = 0,
                                EventCommands = new EventCommandList(new[]
                                {
                                    EventCommandFactory.CreateCommandString(
                                        @"[140][7,1]<0>(33554432,3,0,4,100,105,20)(""BGM/bgm.mp3"")"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[140][7,1]<0>(33554448,3,0,4,100,105,20)(""BGS/音楽.mp3"")"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[140][6,1]<0>(33554464,3,0,4,105,100)(""SE/サウンド.wav"")"),
                                    EventCommandFactory.CreateCommandString(@"[140][4,0]<0>(512,1,0,4)()"),
                                    EventCommandFactory.CreateCommandString(@"[140][4,0]<0>(784,5,0,2)()"),
                                    EventCommandFactory.CreateCommandString(@"[140][4,0]<0>(288,5,0,2)()"),
                                    EventCommandFactory.CreateCommandString(@"[140][4,0]<0>(16777216,5,2000040,2)()"),
                                    EventCommandFactory.CreateCommandString(@"[140][4,0]<0>(16777232,5,2000002,2)()"),
                                    EventCommandFactory.CreateCommandString(@"[140][4,0]<0>(16777248,5,1100003,2)()"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[140][7,1]<0>(33554433,0,0,0,105,100,20)(""BGM/bgm.mp3"")"),
                                    EventCommandFactory.CreateCommandString(@"[140][4,0]<0>(16776960,0,0,0)()"),
                                    EventCommandFactory.CreateCommandString(@"[140][1,0]<0>(16776961)()"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[140][7,1]<0>(33554435,0,0,0,105,100,20)(""BGM/bgm.mp3"")"),
                                    EventCommandFactory.CreateCommandString(@"[0][0,0]<0>()()"),
                                })
                            },
                            new MapEventPage
                            {
                                GraphicInfo = new MapEventPageGraphicInfo
                                {
                                    IsGraphicTileChip = false,
                                    CharaChipFilePath = "",
                                    CharaChipDrawType = PictureDrawType.Normal,
                                    CharaChipOpacity = 255
                                },
                                BootInfo = new MapEventPageBootInfo
                                {
                                    MapEventBootType = MapEventBootType.PushOKKey,
                                },
                                MoveRouteInfo = new MapEventPageMoveRouteInfo
                                {
                                    MoveSpeed = MoveSpeed.Slower,
                                    MoveFrequency = MoveFrequency.Long,
                                    AnimateSpeed = AnimateSpeed.Frame,
                                    MoveType = MoveType.Custom,
                                    CustomMoveRoute = new ActionEntry
                                    {
                                        IsWaitForComplete = true,
                                        IsRepeatAction = true,
                                        IsSkipIfCannotMove = false,
                                        CommandList = new CharaMoveCommandList
                                        {
                                            new MoveLeftUp(),
                                            new MoveUp(),
                                            new MoveRightUp(),
                                            new MoveRight(),
                                            new MoveRightDown(),
                                            new MoveDown(),
                                            new MoveLeftDown(),
                                            new MoveLeft(),
                                        }
                                    }
                                },
                                Option = new MapEventPageOption
                                {
                                    IsWaitAnimationOn = false,
                                    IsMoveAnimationOn = false,
                                    IsFixedDirection = true,
                                    IsSkipThrough = true,
                                    IsAboveHero = false,
                                    IsHitBox = true,
                                    IsPlaceHalfStepUp = false
                                },
                                HitExtendRange = (0, 0),
                                ShadowGraphicId = 0,
                                EventCommands = new EventCommandList(new[]
                                {
                                    EventCommandFactory.CreateCommandString(@"[220][2,0]<0>(0,5)()"),
                                    EventCommandFactory.CreateCommandString(@"[220][2,0]<0>(1,9)()"),
                                    EventCommandFactory.CreateCommandString(@"[221][4,0]<0>(1100005,7,1100000,0)()"),
                                    EventCommandFactory.CreateCommandString(@"[221][4,0]<0>(2500000,1,2000900,1)()"),
                                    EventCommandFactory.CreateCommandString(@"[222][4,0]<0>(1100000,1,2000000,0)()"),
                                    EventCommandFactory.CreateCommandString(@"[222][4,0]<0>(31,5,1024000000,1)()"),
                                    EventCommandFactory.CreateCommandString(@"[0][0,0]<0>()()"),
                                })
                            }
                        })
                    },
                    new MapEvent
                    {
                        EventName = "CharaChip/[Animal]Chicken.png",
                        MapEventId = 2,
                        Position = (8,
                            2),
                        MapEventPageList = new MapEventPageList(new[]
                        {
                            new MapEventPage
                            {
                                GraphicInfo = new MapEventPageGraphicInfo
                                {
                                    IsGraphicTileChip = false,
                                    CharaChipFilePath = "CharaChip/[Animal]Chicken.png",
                                    CharaChipDrawType = PictureDrawType.Normal,
                                    CharaChipOpacity = 255
                                },
                                BootInfo = new MapEventPageBootInfo
                                {
                                    MapEventBootType = MapEventBootType.PushOKKey,
                                },
                                MoveRouteInfo = new MapEventPageMoveRouteInfo
                                {
                                    MoveType = MoveType.Not,
                                    MoveSpeed = MoveSpeed.Normal,
                                    MoveFrequency = MoveFrequency.Middle,
                                    AnimateSpeed = AnimateSpeed.Middle
                                },
                                Option = new MapEventPageOption
                                {
                                    IsWaitAnimationOn = true,
                                    IsMoveAnimationOn = true,
                                    IsFixedDirection = false,
                                    IsSkipThrough = false,
                                    IsAboveHero = false,
                                    IsHitBox = true,
                                    IsPlaceHalfStepUp = false
                                },
                                HitExtendRange = (1, 0),
                                ShadowGraphicId = 2,
                                EventCommands = new EventCommandList(new[]
                                {
                                    EventCommandFactory.CreateCommandString(@"[270][2,0]<0>(0,4)()"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[270][2,1]<0>(1,2)(""CharaChip/[Special]Edy.png"")"),
                                    EventCommandFactory.CreateCommandString(@"[270][3,0]<0>(257,4,1000019)()"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[270][2,1]<0>(2,2)(""CharaChip/[Special]Edy.png"")"),
                                    EventCommandFactory.CreateCommandString(@"[270][3,0]<0>(258,2,1000016)()"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[270][1,1]<0>(3)(""CharaChip/[Special]Edy.png"")"),
                                    EventCommandFactory.CreateCommandString(@"[270][3,0]<0>(259,0,1000016)()"),
                                    EventCommandFactory.CreateCommandString(@"[270][1,0]<0>(4)()"),
                                    EventCommandFactory.CreateCommandString(@"[270][1,0]<0>(20)()"),
                                    EventCommandFactory.CreateCommandString(@"[270][1,0]<0>(36)()"),
                                    EventCommandFactory.CreateCommandString(@"[270][1,0]<0>(52)()"),
                                    EventCommandFactory.CreateCommandString(@"[270][1,0]<0>(68)()"),
                                    EventCommandFactory.CreateCommandString(@"[270][1,0]<0>(84)()"),
                                    EventCommandFactory.CreateCommandString(@"[270][1,0]<0>(100)()"),
                                    EventCommandFactory.CreateCommandString(@"[270][1,0]<0>(116)()"),
                                    EventCommandFactory.CreateCommandString(@"[270][1,0]<0>(132)()"),
                                    EventCommandFactory.CreateCommandString(@"[270][1,0]<0>(148)()"),
                                    EventCommandFactory.CreateCommandString(@"[270][1,0]<0>(164)()"),
                                    EventCommandFactory.CreateCommandString(@"[0][0,0]<0>()()"),
                                })
                            }
                        })
                    },
                    new MapEvent
                    {
                        EventName = "イベント",
                        MapEventId = 3,
                        Position = (10, 6),
                        MapEventPageList = new MapEventPageList(new[]
                        {
                            new MapEventPage
                            {
                                GraphicInfo = new MapEventPageGraphicInfo
                                {
                                    IsGraphicTileChip = false,
                                    CharaChipFilePath = "",
                                    CharaChipDrawType = PictureDrawType.Normal,
                                    CharaChipOpacity = 255
                                },
                                BootInfo = new MapEventPageBootInfo
                                {
                                    MapEventBootType = MapEventBootType.PushOKKey,
                                },
                                MoveRouteInfo = new MapEventPageMoveRouteInfo
                                {
                                    MoveType = MoveType.Not,
                                    MoveSpeed = MoveSpeed.Normal,
                                    MoveFrequency = MoveFrequency.Middle,
                                    AnimateSpeed = AnimateSpeed.Middle
                                },
                                Option = new MapEventPageOption
                                {
                                    IsWaitAnimationOn = true,
                                    IsMoveAnimationOn = true,
                                    IsFixedDirection = false,
                                    IsSkipThrough = false,
                                    IsAboveHero = false,
                                    IsHitBox = true,
                                    IsPlaceHalfStepUp = false
                                },
                                HitExtendRange = (0, 0),
                                ShadowGraphicId = 0,
                                EventCommands = new EventCommandList(new[]
                                {
                                    EventCommandFactory.CreateCommandString(@"[240][2,0]<0>(-1000000,0)()"),
                                    EventCommandFactory.CreateCommandString(@"[240][2,0]<0>(-12,538)()"),
                                    EventCommandFactory.CreateCommandString(@"[240][2,0]<0>(13,199)()"),
                                    EventCommandFactory.CreateCommandString(@"[240][2,0]<0>(8,644)()"),
                                    EventCommandFactory.CreateCommandString(@"[241][1,0]<0>(2)()"),
                                    EventCommandFactory.CreateCommandString(@"[241][1,0]<0>(1100004)()"),
                                    EventCommandFactory.CreateCommandString(@"[242][6,0]<0>(1,4,6,20,21,-13)()"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[242][6,0]<0>(1100000,1100001,1100002,1100004,1100005,1100003)()"),
                                    EventCommandFactory.CreateCommandString(@"[130][5,0]<0>(-1,3,5,1,0)()"),
                                    EventCommandFactory.CreateCommandString(@"[130][5,0]<0>(-1,6,8,-1,17)()"),
                                    EventCommandFactory.CreateCommandString(@"[130][5,0]<0>(-10001,5,0,0,33)()"),
                                    EventCommandFactory.CreateCommandString(@"[130][5,0]<0>(-2,6,8,0,16)()"),
                                    EventCommandFactory.CreateCommandString(@"[130][5,0]<0>(2,1100000,1100001,0,0)()"),
                                    EventCommandFactory.CreateCommandString(@"[130][5,0]<0>(1100002,3,5,0,16)()"),
                                    EventCommandFactory.CreateCommandString(@"[0][0,0]<0>()()"),
                                })
                            }
                        })
                    },
                    new MapEvent
                    {
                        EventName = "イベント制御",
                        MapEventId = 4,
                        Position = (11, 8),
                        MapEventPageList = new MapEventPageList(new[]
                        {
                            new MapEventPage
                            {
                                GraphicInfo = new MapEventPageGraphicInfo
                                {
                                    IsGraphicTileChip = false,
                                    CharaChipFilePath = "",
                                    CharaChipDrawType = PictureDrawType.Normal,
                                    CharaChipOpacity = 255
                                },
                                BootInfo = new MapEventPageBootInfo
                                {
                                    MapEventBootType = MapEventBootType.HitMapEvent,
                                },
                                MoveRouteInfo = new MapEventPageMoveRouteInfo
                                {
                                    MoveType = MoveType.Not,
                                    MoveSpeed = MoveSpeed.Normal,
                                    MoveFrequency = MoveFrequency.Middle,
                                    AnimateSpeed = AnimateSpeed.Middle
                                },
                                Option = new MapEventPageOption
                                {
                                    IsWaitAnimationOn = true,
                                    IsMoveAnimationOn = true,
                                    IsFixedDirection = false,
                                    IsSkipThrough = false,
                                    IsAboveHero = false,
                                    IsHitBox = true,
                                    IsPlaceHalfStepUp = false
                                },
                                HitExtendRange = (2, 2),
                                ShadowGraphicId = 0,
                                EventCommands = new EventCommandList(new[]
                                {
                                    EventCommandFactory.CreateCommandString(@"[170][0,0]<0>()()"),
                                    EventCommandFactory.CreateCommandString(@"[171][0,0]<1>()()"),
                                    EventCommandFactory.CreateCommandString(@"[176][0,0]<1>()()"),
                                    EventCommandFactory.CreateCommandString(@"[161][0,0]<1>()()"),
                                    EventCommandFactory.CreateCommandString(@"[162][0,0]<1>()()"),
                                    EventCommandFactory.CreateCommandString(@"[174][0,0]<1>()()"),
                                    EventCommandFactory.CreateCommandString(@"[175][0,0]<1>()()"),
                                    EventCommandFactory.CreateCommandString(@"[177][0,0]<1>()()"),
                                    EventCommandFactory.CreateCommandString(@"[178][0,0]<1>()()"),
                                    EventCommandFactory.CreateCommandString(@"[202][0,0]<1>()()"),
                                    EventCommandFactory.CreateCommandString(@"[230][0,0]<1>()()"),
                                    EventCommandFactory.CreateCommandString(@"[231][0,0]<1>()()"),
                                    EventCommandFactory.CreateCommandString(@"[172][0,0]<1>()()"),
                                    EventCommandFactory.CreateCommandString(@"[160][2,0]<1>(-1,3)()"),
                                    EventCommandFactory.CreateCommandString(@"[160][2,0]<1>(1,65566)()"),
                                    EventCommandFactory.CreateCommandString(@"[173][2,0]<1>(-2,1)()"),
                                    EventCommandFactory.CreateCommandString(@"[173][2,0]<1>(0,1100000)()"),
                                    EventCommandFactory.CreateCommandString(@"[173][2,0]<1>(1100002,30)()"),
                                    EventCommandFactory.CreateCommandString(@"[180][1,0]<1>(5)()"),
                                    EventCommandFactory.CreateCommandString(@"[180][1,0]<1>(1020000000)()"),
                                    EventCommandFactory.CreateCommandString(@"[179][1,0]<1>(5)()"),
                                    EventCommandFactory.CreateCommandString(@"[179][1,0]<2>(2900032)()"),
                                    EventCommandFactory.CreateCommandString(@"[212][0,1]<3>()(""ラベル名"")"),
                                    EventCommandFactory.CreateCommandString(@"[213][0,1]<3>()(""ラベル名\s[1]"")"),
                                    EventCommandFactory.CreateCommandString(@"[0][0,0]<3>()()"),
                                    EventCommandFactory.CreateCommandString(@"[498][0,0]<2>()()"),
                                    EventCommandFactory.CreateCommandString(@"[210][2,0]<2>(500000,0)()"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[210][10,0]<2>(500001,68,1,2,-2,4,3000000,3000001,3000002,3000003)()"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[210][10,0]<2>(500005,68,0,1,2,3,3000000,3000001,3000002,3000003)()"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[210][10,5]<2>(500005,61508,0,1,2,3,0,0,0,0)("""",""0"",""1"",""2"",""3"")"),
                                    EventCommandFactory.CreateCommandString(@"[210][2,0]<2>(500002,0)()"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[210][3,0]<2>(500002,16777216,1100001)()"),
                                    EventCommandFactory.CreateCommandString(@"[210][2,0]<2>(1,0)()"),
                                    EventCommandFactory.CreateCommandString(@"[210][2,0]<2>(3,9)()"),
                                    EventCommandFactory.CreateCommandString(@"[210][2,0]<2>(2100000,5)()"),
                                    EventCommandFactory.CreateCommandString(@"[211][2,0]<2>(500001,0)()"),
                                    EventCommandFactory.CreateCommandString(@"[211][2,0]<2>(1,0)()"),
                                    EventCommandFactory.CreateCommandString(@"[211][2,0]<2>(1100001,5)()"),
                                    EventCommandFactory.CreateCommandString(@"[211][2,0]<2>(2000003,5)()"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[300][10,1]<2>(0,68,0,1,0,3,3000000,3000001,3000002,3000003)(""コモンイベント001"")"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[300][9,3]<2>(0,16781348,0,2,4,3,0,3000001,1100002)(""コモンイベント004"",""aaaaa"","""")"),
                                    EventCommandFactory.CreateCommandString(@"[0][0,0]<2>()()"),
                                    EventCommandFactory.CreateCommandString(@"[498][0,0]<1>()()"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[260][2,2]<1>(0,3000003)(""http://DownloadURL.jp"",""Data/save.dat"")"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[260][2,2]<1>(3,0)(""http://DownloadURL.jp"","""")"),
                                    EventCommandFactory.CreateCommandString(@"[0][0,0]<1>()()"),
                                    EventCommandFactory.CreateCommandString(@"[498][0,0]<0>()()"),
                                    EventCommandFactory.CreateCommandString(@"[0][0,0]<0>()()"),
                                })
                            }
                        })
                    }
                })
            };
        }

        public static MapData GenerateFixMapData()
        {
            return new MapData
            {
                Memo = "なし",
                TileSetId = 0,
                Layer1 = GenerateEmptyLayer(20, 15),
                Layer2 = GenerateEmptyLayer(20, 15),
                Layer3 = GenerateEmptyLayer(20, 15),
                MapEvents = new MapEventList(new[]
                {
                    new MapEvent
                    {
                        EventName = "不具合修正確認（コマンド）",
                        MapEventId = 0,
                        Position = (9, 3),
                        MapEventPageList = new MapEventPageList(new[]
                        {
                            new MapEventPage
                            {
                                GraphicInfo = new MapEventPageGraphicInfo
                                {
                                    IsGraphicTileChip = false,
                                    CharaChipFilePath = "",
                                    CharaChipDrawType = PictureDrawType.Normal,
                                    CharaChipOpacity = 255
                                },
                                BootInfo = new MapEventPageBootInfo
                                {
                                    MapEventBootType = MapEventBootType.PushOKKey,
                                },
                                MoveRouteInfo = new MapEventPageMoveRouteInfo
                                {
                                    MoveType = MoveType.Not,
                                    MoveSpeed = MoveSpeed.Normal,
                                    MoveFrequency = MoveFrequency.Middle,
                                    AnimateSpeed = AnimateSpeed.Middle,
                                },
                                Option =
                                {
                                    IsWaitAnimationOn = true,
                                    IsMoveAnimationOn = true,
                                    IsHitBox = true,
                                },
                                HitExtendRange = (0, 0),
                                ShadowGraphicId = 0,
                                EventCommands = new EventCommandList(new[]
                                {
                                    EventCommandFactory.CreateCommandString(
                                        @"[103][0,1]<0>()(""イベントコマンドのピクチャ表示がディレイ有りの時出力がおかしくなる #8"")"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[103][0,1]<0>()(""操作が「表示」「移動」の場合にバグ発生。「消去」「ディレイリセット」は問題なし。"")"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[150][18,1]<0>(32,3221,0,1,1,1,255,0,0,100,0,0,33554432,51,0,100,100,100)(""ディレイあり、カラー100、同値false"")"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[150][18,0]<0>(33,65534,0,0,0,1,255,0,0,100,0,0,33554432,234,0,100,100,100)()"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[103][0,1]<0>()(""ピクチャ連続指定時にも同様の不具合あり"")"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[150][18,1]<0>(16777216,50000,0,1,1,1,255,0,0,100,0,0,33554432,0,50002,100,100,100)(""3000000"")"),
                                    EventCommandFactory.CreateCommandString(
                                        @"[150][18,0]<0>(16777217,3,0,0,0,1,255,0,0,100,0,0,33554432,0,4,100,100,100)()"),
                                    EventCommandFactory.CreateCommandString(@"[99][1,0]<0>(1)()"),
                                    EventCommandFactory.CreateCommandString(@"[0][0,0]<0>()()"),
                                })
                            },
                        })
                    },
                })
            };
        }

        private static Layer GenerateEmptyLayer(int width, int height)
        {
            var chips = new List<List<MapChip>>();
            for (var i = 0; i < width; i++)
            {
                var lineChips = new List<MapChip>();
                for (var j = 0; j < height; j++)
                {
                    lineChips.Add(100000);
                }

                chips.Add(lineChips);
            }

            var layer = new Layer
            {
                Chips = new MapChipList(chips)
            };
            return layer;
        }

        #endregion

        #region OutputTestFile

        /** ========================================
         *  テスト用ファイル出力
         *  ======================================== */
        /// <summary>テストディレクトリルート</summary>
        public static string TestWorkRootDir => $@"{Path.GetTempPath()}WodiLibTest";

        /// <summary>テストファイルデータ</summary>
        public static readonly IEnumerable<(string, byte[])> TestFiles = new List<(string, byte[])>
        {
            ("Map023.mps", TestResources.MapFile_Map023),
            ("Map024.mps", TestResources.MapFile_Map024),
            ("Map025.mps", TestResources.MapFile_Map025),
            ("Map100.mps", TestResources.MapFile_Map100),
            ("SampleMapA.mps", TestResources.MapFile_SampleMapA),
            ("SampleMapB.mps", TestResources.MapFile_SampleMapB),
            ("Dungeon.mps", TestResources.MapFile_Dungeon),
            ("fix.mps", TestResources.FixMap),
        };

        /// <summary>
        /// マップファイルを tmp フォルダに出力する。
        /// </summary>
        public static void OutputMapFile()
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
        /// マップファイルを削除する。
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

        #endregion
    }
}