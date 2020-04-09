// ========================================
// Project Name : WodiLib
// File Name    : ChangeGraphic.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.Database;
using WodiLib.Project;

namespace WodiLib.Event.CharaMoveCommand
{
    /// <inheritdoc />
    /// <summary>
    /// 動作指定：グラフィック変更
    /// </summary>
    [Serializable]
    public class ChangeGraphic : CharaMoveCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat = "ｸﾞﾗﾌｨｯｸ変更 {0}:{1}";

        private static readonly string EventCommandSentenceGraphicNotFound = string.Empty;

        private static readonly TypeId CharacterGraphicTypeId = 8;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override CharaMoveCommandCode CommandCode => CharaMoveCommandCode.ChangeGraphic;

        /// <inheritdoc />
        public override byte ValueLengthByte => 0x01;

        /// <summary>画像ID</summary>
        public CharaMoveCommandValue GraphicId
        {
            get => GetNumberValue(0);
            set
            {
                SetNumberValue(0, value);
                NotifyPropertyChanged();
            }
        }

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string GetEventCommandSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            var (hasData, dataName) = resolver.GetDatabaseDataName(DBKind.System,
                CharacterGraphicTypeId, GraphicId);
            var graphicIdStr = resolver.GetNumericVariableAddressStringIfVariableAddress(
                GraphicId, type, desc);
            var graphicStr = hasData
                ? dataName
                : EventCommandSentenceGraphicNotFound;

            return string.Format(EventCommandSentenceFormat,
                graphicIdStr, graphicStr);
        }
    }
}