// ========================================
// Project Name : WodiLib
// File Name    : EventCommandFactory.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// イベントコマンドFactory
    /// </summary>
    public static class EventCommandFactory
    {
        /// <summary>
        /// データから直接インスタンスを生成する。
        /// </summary>
        /// <param name="intValues">[NotNull] 数値引数</param>
        /// <param name="indent">インデント</param>
        /// <param name="strValues">[NotNull] 文字列引数</param>
        /// <param name="actionEntry">[CanBeNull] キャラ動作指定コマンド</param>
        /// <returns>イベントコマンドのインスタンス</returns>
        /// <exception cref="ArgumentOutOfRangeException">Range項目が規定の範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">NotNull項目がnullの場合</exception>
        /// <exception cref="ArgumentException">numberVariableCountとintValues.Lengthが一致しない場合</exception>
        /// <exception cref="ArgumentException">stringVariableCountとstrValuesが一致しない場合</exception>
        /// <exception cref="ArgumentException">該当イベントコマンドが存在しない場合</exception>
        public static IEventCommand CreateRaw(
            IEnumerable<int> intValues,
            Indent indent,
            IEnumerable<string> strValues,
            ActionEntry actionEntry)
        {
            return CreateRaw(intValues, indent, strValues,
                "", actionEntry);
        }

        /// <summary>
        /// データから直接インスタンスを生成する。
        /// </summary>
        /// <param name="intValues">[NotNull] 数値引数</param>
        /// <param name="indent">インデント</param>
        /// <param name="strValues">[NotNull] 文字列引数</param>
        /// <param name="expansionString">[NotNull] 拡張文字列</param>
        /// <param name="actionEntry">[CanBeNull] キャラ動作指定コマンド</param>
        /// <returns>イベントコマンドのインスタンス</returns>
        /// <exception cref="ArgumentOutOfRangeException">Range項目が規定の範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">NotNull項目がnullの場合</exception>
        /// <exception cref="ArgumentException">numberVariableCountとintValues.Lengthが一致しない場合</exception>
        /// <exception cref="ArgumentException">stringVariableCountとstrValuesが一致しない場合</exception>
        /// <exception cref="ArgumentException">該当イベントコマンドが存在しない場合</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static IEventCommand CreateRaw(
            IEnumerable<int> intValues,
            Indent indent,
            IEnumerable<string> strValues,
            string expansionString,
            ActionEntry actionEntry)
        {
            // 引数チェック
            if (intValues is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(intValues)));

            if (strValues is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(strValues)));

            var intValueList = intValues.ToList();
            var strValueList = strValues.ToList();

            EventCommandBase instance = null;

            const string failSearchMessage = "条件に合致するイベントコードが見つかりませんでした。";

            // インスタンスを決める

            #region GetInstance

            if (intValueList[0] == EventCommandCode.Blank.Code)
            {
                instance = new Blank();
            }
            else if (intValueList[0] == EventCommandCode.Message.Code)
            {
                instance = new Message();
            }
            else if (intValueList[0] == EventCommandCode.Comment.Code)
            {
                instance = new Comment();
            }
            else if (intValueList[0] == EventCommandCode.DebugText.Code)
            {
                instance = new DebugText();
            }
            else if (intValueList[0] == EventCommandCode.ClearDebugText.Code)
            {
                instance = new ClearDebugText();
            }
            else if (intValueList[0] == EventCommandCode.StopForceMessage.Code)
            {
                instance = new StopForceMessage();
            }
            else if (intValueList[0] == EventCommandCode.ChoiceStart.Code)
            {
                instance = new ChoiceStart();
            }
            else if (intValueList[0] == EventCommandCode.ChoiceStartForkingNumber.Code)
            {
                instance = new ForkStart();
            }
            else if (intValueList[0] == EventCommandCode.ChoiceStartForkingEtc.Code)
            {
                var choiceCode = intValueList[1].ToBytes(Endian.Little)[0];
                if (choiceCode == EventCommandConstant.ChoiceStartForkingEtc.ChoiceCode.LeftKey)
                    instance = new ChoiceStartForkingLeftKey();
                else if (choiceCode == EventCommandConstant.ChoiceStartForkingEtc.ChoiceCode.RightKey)
                    instance = new ChoiceStartForkingRightKey();
                else if (choiceCode == EventCommandConstant.ChoiceStartForkingEtc.ChoiceCode.ForceExit)
                    instance = new ChoiceStartForkingForceExit();

                if (instance is null) throw new ArgumentException(failSearchMessage);
            }
            else if (intValueList[0] == EventCommandCode.ChoiceStartForkingCancel.Code)
            {
                instance = new ChoiceStartForkingCancel();
            }
            else if (intValueList[0] == EventCommandCode.ForkEnd.Code)
            {
                instance = new ForkEnd();
            }
            else if (intValueList[0] == EventCommandCode.BreakChoiceForce.Code)
            {
                instance = new BreakChoiceForce();
            }
            else if (intValueList[0] == EventCommandCode.SetVariable.Code)
            {
                if (intValueList[1] == SetVariableChangeableDB.LeftSide)
                {
                    instance = new SetVariableChangeableDB();
                }
                else
                {
                    instance = new SetVariable();
                }
            }
            else if (intValueList[0] == EventCommandCode.DBManagement.Code)
            {
                var dataId = intValueList[2];
                var itemId = intValueList[3];
                if (dataId == EventCommandConstant.DBManagement.IdSet.GetTypeX.DataId
                    && itemId == EventCommandConstant.DBManagement.IdSet.GetTypeX.ItemId)
                {
                    var isUseTypeName = (intValueList[4].ToBytes(Endian.Little)[2]
                                         & EventCommandConstant.DBManagement.UseStringFlg.TypeIdFlagBit) != 0;
                    instance = isUseTypeName
                        ? (EventCommandBase) new DBManagementGetTypeId()
                        : new DBManagementGetTypeName();
                }
                else if (itemId == EventCommandConstant.DBManagement.IdSet.GetDataX.ItemId)
                {
                    var isUseDataName = (intValueList[4].ToBytes(Endian.Little)[2]
                                         & EventCommandConstant.DBManagement.UseStringFlg.DataIdFlagBit) != 0;
                    instance = isUseDataName
                        ? (EventCommandBase) new DBManagementGetDataId()
                        : new DBManagementGetDataName();
                }
                else if (dataId == EventCommandConstant.DBManagement.IdSet.GetItemX.DataId)
                {
                    var isUseItemName = (intValueList[4].ToBytes(Endian.Little)[2]
                                         & EventCommandConstant.DBManagement.UseStringFlg.ItemIdFlagBit) != 0;
                    instance = isUseItemName
                        ? (EventCommandBase) new DBManagementGetItemId()
                        : new DBManagementGetItemName();
                }
                else if (dataId == EventCommandConstant.DBManagement.IdSet.GetDataLength.DataId)
                {
                    instance = new DBManagementGetDataLength();
                }
                else if (itemId == EventCommandConstant.DBManagement.IdSet.GetItemLength.ItemId)
                {
                    instance = new DBManagementGetItemLength();
                }
                else if (dataId == EventCommandConstant.DBManagement.IdSet.ClearData.DataId)
                {
                    instance = new DBManagementClearData();
                }
                else if (itemId == EventCommandConstant.DBManagement.IdSet.ClearField.ItemId)
                {
                    instance = new DBManagementClearField();
                }
                else
                {
                    var bytes = intValueList[4].ToBytes(Endian.Environment);
                    var isInput = (bytes[1] & 0xF0) == 0;
                    if (isInput)
                    {
                        var inputTypeCode = bytes[0];
                        if (inputTypeCode == DBStringAssignmentOperator.Assign.Code
                            || inputTypeCode == DBStringAssignmentOperator.Addition.Code)
                        {
                            instance = new DBManagementInputString();
                        }
                        else
                        {
                            instance = new DBManagementInputNumber();
                        }
                    }
                    else
                    {
                        instance = new DBManagementOutput();
                    }
                }
            }
            else if (intValueList[0] == EventCommandCode.CsvIO.Code)
            {
                instance = new CsvIO();
            }
            else if (intValueList[0] == EventCommandCode.SetString.Code)
            {
                var mode = (byte) (intValueList[2].ToBytes(Endian.Little)[0] & 0x0F);
                if (mode == EventCommandConstant.SetString.RightSidePropertyCode.Manual)
                {
                    instance = new SetStringManual();
                }
                else if (mode == EventCommandConstant.SetString.RightSidePropertyCode.StringVar)
                {
                    instance = new SetStringStringVar();
                }
                else if (mode == EventCommandConstant.SetString.RightSidePropertyCode.ReferVar)
                {
                    instance = new SetStringReferVar();
                }
                else if (mode == EventCommandConstant.SetString.RightSidePropertyCode.KeyboardInput)
                {
                    instance = new SetStringKeyboardInput();
                }
            }
            else if (intValueList[0] == EventCommandCode.SetVariablePlus.Code)
            {
                var execCode = (byte) (intValueList[2].ToBytes(Endian.Little)[1] & 0xF0);
                if (execCode == EventCommandConstant.SetVariablePlus.ExecCode.Chara)
                {
                    instance = new SetVariablePlusChara();
                }
                else if (execCode == EventCommandConstant.SetVariablePlus.ExecCode.Position)
                {
                    instance = new SetVariablePlusPosition();
                }
                else if (execCode == EventCommandConstant.SetVariablePlus.ExecCode.Etc)
                {
                    instance = new SetVariablePlusEtc();
                }
                else if (execCode == EventCommandConstant.SetVariablePlus.ExecCode.Picture)
                {
                    instance = new SetVariablePlusPicture();
                }

                if (instance is null) throw new ArgumentException(failSearchMessage);
            }
            else if (intValueList[0] == EventCommandCode.ConditionNumberStart.Code)
            {
                instance = new ConditionNumberStart();
            }
            else if (intValueList[0] == EventCommandCode.ConditionElse.Code)
            {
                instance = new ConditionElse();
            }
            else if (intValueList[0] == EventCommandCode.ConditionStringStart.Code)
            {
                instance = new ConditionStringStart();
            }
            else if (intValueList[0] == EventCommandCode.KeyInput.Code)
            {
                var typeCode = (byte) (intValueList[2].ToBytes(Endian.Little)[1] & 0x0F);
                if (typeCode == EventCommandConstant.KeyInput.Type.Basic)
                {
                    instance = new KeyInputBasic();
                }
                else if (typeCode == EventCommandConstant.KeyInput.Type.Keyboard)
                {
                    instance = new KeyInputKeyboard();
                }
                else if (typeCode == EventCommandConstant.KeyInput.Type.Mouse)
                {
                    instance = new KeyInputMouse();
                }
                else if (typeCode == EventCommandConstant.KeyInput.Type.Pad)
                {
                    instance = new KeyInputPad();
                }

                if (instance is null) throw new ArgumentException(failSearchMessage);
            }
            else if (intValueList[0] == EventCommandCode.KeyInputAuto.Code)
            {
                var typeCode = (byte) (intValueList[1].ToBytes(Endian.Little)[3] & 0xF0);
                if (typeCode == EventCommandConstant.KeyInputAuto.Type.Basic)
                {
                    instance = new KeyInputAutoBasic();
                }
                else if (typeCode == EventCommandConstant.KeyInputAuto.Type.Keyboard)
                {
                    instance = new KeyInputAutoKeyboard();
                }
                else if (typeCode == EventCommandConstant.KeyInputAuto.Type.Mouse)
                {
                    instance = new KeyInputAutoMouse();
                }

                if (instance is null) throw new ArgumentException(failSearchMessage);
            }
            else if (intValueList[0] == EventCommandCode.StandardKeyInputControl.Code)
            {
                var typeCode = (byte) (intValueList[1].ToBytes(Endian.Little)[3] & 0xF0);
                if (typeCode == EventCommandConstant.KeyInputControl.TargetCode.Basic)
                {
                    instance = new StandardKeyInputControl();
                }
                else if (typeCode == EventCommandConstant.KeyInputControl.TargetCode.Device)
                {
                    instance = new DeviceInputControl();
                }

                if (instance is null) throw new ArgumentException(failSearchMessage);
            }
            else if (intValueList[0] == EventCommandCode.Picture.Code)
            {
                var execCode = (byte) (intValueList[1].ToBytes(Endian.Little)[0] & 0x0F);
                if (execCode == EventCommandConstant.PictureShow.ExecCode.Show)
                {
                    var typeCode = (byte) (intValueList[1].ToBytes(Endian.Little)[0] & 0xF0);
                    if (typeCode == EventCommandConstant.PictureShow.ShowTypeCode.LoadFileDirect)
                    {
                        instance = new PictureShowLoadFileDirect();
                    }
                    else if (typeCode == EventCommandConstant.PictureShow.ShowTypeCode.LoadFileVariable)
                    {
                        instance = new PictureShowLoadFileVariable();
                    }
                    else if (typeCode == EventCommandConstant.PictureShow.ShowTypeCode.String)
                    {
                        instance = new PictureShowString();
                    }
                    else if (typeCode == EventCommandConstant.PictureShow.ShowTypeCode.SimpleWindowDirect)
                    {
                        instance = new PictureShowSimpleWindowDirect();
                    }
                    else if (typeCode == EventCommandConstant.PictureShow.ShowTypeCode.SimpleWindowVariable)
                    {
                        instance = new PictureShowSimpleWindowVariable();
                    }

                    if (instance is null) throw new ArgumentException(failSearchMessage);
                }
                else if (execCode == EventCommandConstant.PictureShow.ExecCode.Move)
                {
                    instance = new PictureMove();
                }
                else if (execCode == EventCommandConstant.PictureShow.ExecCode.Erase)
                {
                    instance = new PictureErase();
                }
                else if (execCode == EventCommandConstant.PictureShow.ExecCode.DelayReset)
                {
                    instance = new PictureDelayReset();
                }

                if (instance is null) throw new ArgumentException(failSearchMessage);
            }
            else if (intValueList[0] == EventCommandCode.Effect.Code)
            {
                var targetCode = (byte) (intValueList[1].ToBytes(Endian.Environment)[0] & 0x0F);
                if (targetCode == EventCommandConstant.Effect.TargetCode.Picture)
                {
                    instance = new PictureEffect();
                }
                else if (targetCode == EventCommandConstant.Effect.TargetCode.Chara)
                {
                    instance = new CharacterEffect();
                }
                else if (targetCode == EventCommandConstant.Effect.TargetCode.Map)
                {
                    instance = new MapEffectZoom();
                }

                if (instance is null) throw new ArgumentException(failSearchMessage);
            }
            else if (intValueList[0] == EventCommandCode.MapEffectShake.Code)
            {
                instance = new MapEffectShake();
            }
            else if (intValueList[0] == EventCommandCode.ScrollScreen.Code)
            {
                instance = new ScrollScreen();
            }
            else if (intValueList[0] == EventCommandCode.ChangeScreenColor.Code)
            {
                instance = new ChangeScreenColor();
            }
            else if (intValueList[0] == EventCommandCode.Sound.Code)
            {
                var execCode = (byte) (intValueList[1].ToBytes(Endian.Little)[0] & 0x0F);
                if (execCode == EventCommandConstant.Sound.ExecCode.Playback)
                {
                    instance = new SoundPlayback();
                }
                else if (execCode == EventCommandConstant.Sound.ExecCode.Preload)
                {
                    instance = new SoundPreload();
                }
                else if (execCode == EventCommandConstant.Sound.ExecCode.ReleaseManual)
                {
                    instance = new SoundReleaseManual();
                }
                else if (execCode == EventCommandConstant.Sound.ExecCode.ReleaseAll)
                {
                    instance = new SoundReleaseAll();
                }

                if (instance is null) throw new ArgumentException(failSearchMessage);
            }
            else if (intValueList[0] == EventCommandCode.Save.Code)
            {
                var execCode = (byte) (intValueList[1].ToBytes(Endian.Little)[0] & 0x0F);
                if (execCode == EventCommandConstant.SaveLoad.ExecCode.Save)
                {
                    instance = new Save();
                }
                else if (execCode == EventCommandConstant.SaveLoad.ExecCode.Load)
                {
                    instance = new Load();
                }

                if (instance is null) throw new ArgumentException(failSearchMessage);
            }
            else if (intValueList[0] == EventCommandCode.LoadVariable.Code)
            {
                instance = new LoadSpecificSaveData();
            }
            else if (intValueList[0] == EventCommandCode.SaveVariable.Code)
            {
                instance = new SaveSpecificSaveData();
            }
            else if (intValueList[0] == EventCommandCode.PartyGraphic.Code)
            {
                var execCode = (byte) (intValueList[1].ToBytes(Endian.Little)[0] & 0x0F);
                if (execCode == EventCommandConstant.PartyGraphic.ExecCode.Remove)
                {
                    instance = new PartyGraphicRemove();
                }
                else if (execCode == EventCommandConstant.PartyGraphic.ExecCode.Insert)
                {
                    instance = new PartyGraphicInsert();
                }
                else if (execCode == EventCommandConstant.PartyGraphic.ExecCode.Replace)
                {
                    instance = new PartyGraphicReplace();
                }
                else if (execCode == EventCommandConstant.PartyGraphic.ExecCode.RemoveGraphic)
                {
                    instance = new PartyGraphicRemoveGraphic();
                }
                else if (execCode == EventCommandConstant.PartyGraphic.ExecCode.Special)
                {
                    instance = new PartyGraphicSpecial();
                }

                if (instance is null) throw new ArgumentException(failSearchMessage);
            }
            else if (intValueList[0] == EventCommandCode.ChangeMapChipSetting.Code)
            {
                instance = new ChangeMapChipSetting();
            }
            else if (intValueList[0] == EventCommandCode.SwitchChipSet.Code)
            {
                instance = new SwitchChipSet();
            }
            else if (intValueList[0] == EventCommandCode.OverwriteMapChips.Code)
            {
                instance = new OverwriteMapChips();
            }
            else if (intValueList[0] == EventCommandCode.Transfer.Code)
            {
                if (intValueList[1] != EventCommandConstant.Transfer.UseSavedPosition)
                {
                    instance = new TransferDestination();
                }
                else
                {
                    instance = new TransferSavedPosition();
                }
            }
            else if (intValueList[0] == EventCommandCode.SyntheticVoice.Code)
            {
                instance = new SyntheticVoice();
            }
            else if (intValueList[0] == EventCommandCode.LoopInfiniteStart.Code)
            {
                instance = new LoopInfiniteStart();
            }
            else if (intValueList[0] == EventCommandCode.LoopFiniteStart.Code)
            {
                instance = new LoopFiniteStart();
            }
            else if (intValueList[0] == EventCommandCode.LoopEnd.Code)
            {
                instance = new LoopEnd();
            }
            else if (intValueList[0] == EventCommandCode.LoopBreak.Code)
            {
                instance = new LoopBreak();
            }
            else if (intValueList[0] == EventCommandCode.LoopContinue.Code)
            {
                instance = new LoopContinue();
            }
            else if (intValueList[0] == EventCommandCode.PreparationTransition.Code)
            {
                instance = new PreparationTransition();
            }
            else if (intValueList[0] == EventCommandCode.ExecutionTransition.Code)
            {
                instance = new ExecutionTransition();
            }
            else if (intValueList[0] == EventCommandCode.TransitionWithOption.Code)
            {
                instance = new TransitionWithOption();
            }
            else if (intValueList[0] == EventCommandCode.MoveDuringEventsOn.Code)
            {
                instance = new MoveDuringEventsOn();
            }
            else if (intValueList[0] == EventCommandCode.MoveDuringEventsOff.Code)
            {
                instance = new MoveDuringEventsOff();
            }
            else if (intValueList[0] == EventCommandCode.GoToTitleScreen.Code)
            {
                instance = new GoToTitleScreen();
            }
            else if (intValueList[0] == EventCommandCode.GameEnd.Code)
            {
                instance = new GameEnd();
            }
            else if (intValueList[0] == EventCommandCode.HaltNonPictureUpdate.Code)
            {
                instance = new HaltNonPictureUpdate();
            }
            else if (intValueList[0] == EventCommandCode.ResumeNonPictureUpdate.Code)
            {
                instance = new ResumeNonPictureUpdate();
            }
            else if (intValueList[0] == EventCommandCode.ForceExitEvent.Code)
            {
                instance = new ForceExitEvent();
            }
            else if (intValueList[0] == EventCommandCode.MoveRoute.Code)
            {
                instance = new MoveRoute();
            }
            else if (intValueList[0] == EventCommandCode.WaitForMovement.Code)
            {
                instance = new WaitForMovement();
            }
            else if (intValueList[0] == EventCommandCode.EraseEvent.Code)
            {
                instance = new EraseEvent();
            }
            else if (intValueList[0] == EventCommandCode.Wait.Code)
            {
                instance = new WaitEventCommand();
            }
            else if (intValueList[0] == EventCommandCode.SetLabel.Code)
            {
                instance = new SetLabel();
            }
            else if (intValueList[0] == EventCommandCode.JumpToLabel.Code)
            {
                instance = new JumpToLabel();
            }
            else if (intValueList[0] == EventCommandCode.CallCommonEventById.Code)
            {
                instance = new CallCommonEventById();
            }
            else if (intValueList[0] == EventCommandCode.CallCommonEventByName.Code)
            {
                instance = new CallCommonEventByName();
            }
            else if (intValueList[0] == EventCommandCode.CommonEventReserve.Code)
            {
                instance = new CommonEventReserve();
            }
            else if (intValueList[0] == EventCommandCode.Download.Code)
            {
                instance = new Download();
            }
            else if (intValueList[0] == EventCommandCode.CheckPoint.Code)
            {
                if (intValueList.Count == 1)
                {
                    // ver2.00より前のチェックポイント
                    instance = new CheckPoint(false);
                }
                else if (intValueList[1] == EventCommandConstant.CheckPoint.Type.Standard)
                {
                    instance = new CheckPoint();
                }
                else if (intValueList[1] == EventCommandConstant.CheckPoint.Type.Special)
                {
                    instance = new SpecialCheckPoint();
                }

                if (instance is null) throw new ArgumentException(failSearchMessage);
            }
            else
            {
                instance = new Unknown(intValueList[0]);
            }

            #endregion

            // instance is null の場合、処理が誤っている
            if (instance is null)
                throw new InvalidOperationException();

            // 数値引数・文字列引数の数が最低限の数揃っていない場合エラー
            if (intValueList.Count < instance.NumberVariableCountMin)
                throw new ArgumentException(
                    ErrorMessage.UnderListLength(nameof(intValues), instance.NumberVariableCountMin));
            if (strValueList.Count < instance.StringVariableCountMin)
                throw new ArgumentException(
                    ErrorMessage.UnderListLength(nameof(strValues), instance.StringVariableCountMin));

            // 数値引数と文字列引数、インデント、動作指定コマンドをセットする
            for (var i = 1; i < intValueList.Count; i++)
            {
                // index0 はイベントコマンドコードなので設定しない
                if (instance.IsNormalNumberArgIndex(i))
                {
                    instance.SetSafetyNumberVariable(i, intValueList[i]);
                }
                else
                {
                    instance.ExpansionNumberArgList.Add(intValueList[i]);
                }
            }

            for (var i = 0; i < strValueList.Count; i++)
            {
                if (instance.IsNormalStringArgIndex(i))
                {
                    instance.SetSafetyStringVariable(i, strValueList[i]);
                }
                else
                {
                    instance.ExpansionStringArgList.Add(strValueList[i]);
                }
            }

            instance.Indent = (sbyte) indent;
            instance.ExpansionString = expansionString;
            instance.ActionEntry = actionEntry;

            return instance;
        }

        /// <summary>
        /// イベントコマンド文字列からインスタンスを生成する。
        /// </summary>
        /// <param name="src">[NotNull] イベントコマンド文字列</param>
        /// <returns>イベントコマンドのインスタンス</returns>
        /// <exception cref="ArgumentNullException">srcがnullの場合</exception>
        /// <exception cref="InvalidOperationException">srcがイベントコマンド文字列として不適切だった場合</exception>
        /// <exception cref="InvalidCastException">srcに含まれる文字列が不適切だった場合</exception>
        /// <exception cref="ArgumentException"></exception>
        public static IEventCommand CreateCommandString(string src)
        {
            if (src is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(src)));

            var parser = new EventCommandStringParser(src);
            parser.Parse();

            // パースした情報でインスタンスを生成
            return CreateRaw(parser.NumberArgList, parser.Indent,
                parser.StrArgList, parser.ExpansionString, null);
        }
    }
}