// ========================================
// Project Name : WodiLib
// File Name    : OverwriteMapChips.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.Project;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・マップチップ上書き
    /// </summary>
    [Serializable]
    public class OverwriteMapChips : EventCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat
            = "■ﾏｯﾌﾟﾁｯﾌﾟ上書き： [ ﾚｲﾔｰ {0} / X {1} / Y {2} ] から[ 横{3} / 縦{4} ] をﾁｯﾌﾟ[ {5} ]で上書き";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override EventCommandCode EventCommandCode => EventCommandCode.OverwriteMapChips;

        /// <inheritdoc />
        public override byte NumberVariableCount => 0x07;

        /// <inheritdoc />
        public override byte StringVariableCount => 0x00;

        /// <inheritdoc />
        protected override EventCommandColorSet EventCommandColorSet
            => EventCommandColorSet.YellowGreen;

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して数値変数を取得する。
        /// ウディタ標準仕様でサポートしているインデックスのみ取得可能。
        /// </summary>
        /// <param name="index">[Range(0, 6)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override int GetSafetyNumberVariable(int index)
        {
            switch (index)
            {
                case 0:
                    return EventCommandCode.Code;

                case 1:
                    return Layer;

                case 2:
                    return PositionX;

                case 3:
                    return PositionY;

                case 4:
                    return RangeWidth;

                case 5:
                    return RangeHeight;

                case 6:
                    return ChipId;

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 0, 6, index));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// 数値変数を設定する。
        /// </summary>
        /// <param name="index">[Range(1, 6)] インデックス</param>
        /// <param name="value">設定値</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void SetSafetyNumberVariable(int index, int value)
        {
            switch (index)
            {
                case 1:
                    Layer = value;
                    return;

                case 2:
                    PositionX = value;
                    return;

                case 3:
                    PositionY = value;
                    return;

                case 4:
                    RangeWidth = value;
                    return;

                case 5:
                    RangeHeight = value;
                    return;

                case 6:
                    ChipId = value;
                    return;

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 1, 6, index));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して文字列変数を取得する。
        /// ウディタ標準仕様でサポートしているインデックスのみ取得可能。
        /// </summary>
        /// <param name="index">[Range(0, -)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">常に</exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string GetSafetyStringVariable(int index)
        {
            throw new ArgumentOutOfRangeException();
        }

        /// <inheritdoc />
        /// <summary>
        /// 文字列変数を設定する。
        /// </summary>
        /// <param name="index">[Range(0, -)] インデックス</param>
        /// <param name="value">[NotNull] 設定値</param>
        /// <exception cref="ArgumentOutOfRangeException">常に</exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void SetSafetyStringVariable(int index, string value)
        {
            throw new ArgumentOutOfRangeException();
        }

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override string MakeEventCommandMainSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            var layerName = resolver.GetNumericVariableAddressStringIfVariableAddress(Layer, type, desc);
            var xName = resolver.GetNumericVariableAddressStringIfVariableAddress(PositionX, type, desc);
            var yName = resolver.GetNumericVariableAddressStringIfVariableAddress(PositionY, type, desc);
            var widthName = resolver.GetNumericVariableAddressStringIfVariableAddress(RangeWidth, type, desc);
            var heightName = resolver.GetNumericVariableAddressStringIfVariableAddress(RangeHeight, type, desc);
            var chipName = resolver.GetNumericVariableAddressStringIfVariableAddress(ChipId, type, desc);

            return string.Format(EventCommandSentenceFormat,
                layerName, xName, yName, widthName, heightName, chipName);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private int layer;

        /// <summary>レイヤー</summary>
        public int Layer
        {
            get => layer;
            set
            {
                layer = value;
                NotifyPropertyChanged();
            }
        }

        private int positionX;

        /// <summary>X座標</summary>
        public int PositionX
        {
            get => positionX;
            set
            {
                positionX = value;
                NotifyPropertyChanged();
            }
        }

        private int positionY;

        /// <summary>Y座標</summary>
        public int PositionY
        {
            get => positionY;
            set
            {
                positionY = value;
                NotifyPropertyChanged();
            }
        }

        private int chipId;

        /// <summary>チップID</summary>
        public int ChipId
        {
            get => chipId;
            set
            {
                chipId = value;
                NotifyPropertyChanged();
            }
        }

        private int rangeWidth;

        /// <summary>横幅</summary>
        public int RangeWidth
        {
            get => rangeWidth;
            set
            {
                rangeWidth = value;
                NotifyPropertyChanged();
            }
        }

        private int rangeHeight;

        /// <summary>縦幅</summary>
        public int RangeHeight
        {
            get => rangeHeight;
            set
            {
                rangeHeight = value;
                NotifyPropertyChanged();
            }
        }
    }
}