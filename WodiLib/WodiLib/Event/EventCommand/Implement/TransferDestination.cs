// ========================================
// Project Name : WodiLib
// File Name    : TransferDestination.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.Cmn;
using WodiLib.Project;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・場所移動（移動先指定）
    /// </summary>
    [Serializable]
    public class TransferDestination : TransferBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat = "{0}X:{1} Y:{2} {3}";

        private const string EventCommandSentenceThisMap = "▲現在のマップ";
        private const string EventCommandSentenceFormatNormal = "▲マップID{0}[{1}]";
        private const string EventCommandSentenceFormatVariable = "▲マップIDXX<{0}>";

        private const string EventCommandSentencePreciseCoordinates = "[精密]";
        private const string EventCommandSentenceNotPreciseCoordinates = "";

        private const int TargetHero = -1;

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
            var moveMapStr = Target == TargetHero
                ? MakeEventCommandMoveMapSentence(resolver, type, desc) + " "
                : "";
            var xStr = resolver.GetNumericVariableAddressStringIfVariableAddress(PositionX, type, desc);
            var yStr = resolver.GetNumericVariableAddressStringIfVariableAddress(PositionY, type, desc);
            var preciseCoordinatesStr = IsPreciseCoordinates
                ? EventCommandSentencePreciseCoordinates
                : EventCommandSentenceNotPreciseCoordinates;

            return string.Format(EventCommandSentenceFormat,
                moveMapStr, xStr, yStr, preciseCoordinatesStr);
        }

        private string MakeEventCommandMoveMapSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            if (IsSameMap) return EventCommandSentenceThisMap;

            if (DestinationMapId.IsVariableAddressSimpleCheck())
            {
                var varStr = resolver.GetNumericVariableAddressStringIfVariableAddress(
                    DestinationMapId, type, desc);
                return string.Format(EventCommandSentenceFormatVariable, varStr);
            }

            var eventName = resolver.GetMapName(DestinationMapId).Item2;
            return string.Format(EventCommandSentenceFormatNormal,
                DestinationMapId, eventName);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>対象</summary>
        public int Target
        {
            get => _Target;
            set => _Target = value;
        }

        /// <summary>移動先マップ</summary>
        public int DestinationMapId
        {
            get => _DestinationMapId;
            set => _DestinationMapId = value;
        }

        /// <summary>同じマップ</summary>
        public bool IsSameMap
        {
            get => _IsSameMap;
            set => _IsSameMap = value;
        }

        /// <summary>移動先座標X</summary>
        public int PositionX
        {
            get => _PositionX;
            set => _PositionX = value;
        }

        /// <summary>移動先座標Y</summary>
        public int PositionY
        {
            get => _PositionY;
            set => _PositionY = value;
        }

        /// <summary>精密座標</summary>
        public bool IsPreciseCoordinates
        {
            get => _IsPreciseCoordinates;
            set => _IsPreciseCoordinates = value;
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