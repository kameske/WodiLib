// ========================================
// Project Name : WodiLib
// File Name    : TransferSavedPosition.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.Cmn;
using WodiLib.Database;
using WodiLib.Project;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・場所移動（登録位置）
    /// </summary>
    [Serializable]
    public class TransferSavedPosition : TransferBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat_NotVariable = "SysDB 7-{0}：[{1}]の位置";
        private const string EventCommandSentenceFormat_Variable = "SysDB 7-[{0}]";

        private static TypeId MapPositionSettingTypeId => 7;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override EventCommandCode EventCommandCode => EventCommandCode.Transfer;

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override string MakeEventCommandMoveParamSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            if (SavedPositionId.IsVariableAddressSimpleCheck())
            {
                return MakeEventCommandMoveParamSentence_TargetVariableAddress(
                    resolver, type, desc);
            }

            return MakeEventCommandMoveParamSentence_TargetNotVariableAddress(resolver);
        }

        private string MakeEventCommandMoveParamSentence_TargetNotVariableAddress(
            EventCommandSentenceResolver resolver)
        {
            var idStr = SavedPositionId >= 100
                ? SavedPositionId.ToString()
                : SavedPositionId.ToString().PadLeft(2, '0');

            var nameStr = resolver.GetDatabaseDataName(
                DBKind.System, MapPositionSettingTypeId, SavedPositionId).Item2;

            return string.Format(EventCommandSentenceFormat_NotVariable,
                idStr, nameStr);
        }

        private string MakeEventCommandMoveParamSentence_TargetVariableAddress(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            var idStr = resolver.GetNumericVariableAddressString(SavedPositionId, type, desc);
            return string.Format(EventCommandSentenceFormat_Variable, idStr);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>位置登録ID</summary>
        public int SavedPositionId
        {
            get => _SavedPositionId;
            set => _SavedPositionId = value;
        }

        /// <summary>[NotNull] 場所移動時トランジションオプション</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public TransferOption TransferOption
        {
            get => _TransferOption;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(TransferOption)));
                _TransferOption = value;
            }
        }
    }
}