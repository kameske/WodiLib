// ========================================
// Project Name : WodiLib
// File Name    : PlaySE.cs
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
    /// 動作指定：効果音再生
    /// </summary>
    [Serializable]
    public class PlaySE : CharaMoveCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat = "効果音再生 {0}:{1}";

        private static readonly string EventCommandSentenceSeNotFound = string.Empty;

        private static readonly TypeId SeTypeId = 3;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override CharaMoveCommandCode CommandCode => CharaMoveCommandCode.PlaySE;

        /// <inheritdoc />
        public override byte ValueLengthByte => 0x01;

        /// <summary>効果音ID</summary>
        public CharaMoveCommandValue SoundId
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
                SeTypeId, SoundId);
            var graphicStr = hasData
                ? dataName
                : EventCommandSentenceSeNotFound;

            return string.Format(EventCommandSentenceFormat,
                SoundId, graphicStr);
        }
    }
}