// ========================================
// Project Name : WodiLib
// File Name    : MapEffectZoom.cs
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
    /// イベントコマンド・マップエフェクト
    /// </summary>
    [Serializable]
    public class MapEffectZoom : EventCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat
            = "■マップエフェクト：[ズーム] 中心X {0} / 中心Y {1} / {2}％  ({3})ﾌﾚｰﾑ";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override EventCommandCode EventCommandCode => EventCommandCode.Effect;

        /// <inheritdoc />
        public override byte NumberVariableCount => 0x08;

        /// <inheritdoc />
        public override byte StringVariableCount => 0x00;

        /// <inheritdoc />
        protected override EventCommandColorSet EventCommandColorSet
            => EventCommandColorSet.Gold;

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して数値変数を取得する。
        /// ウディタ標準仕様でサポートしているインデックスのみ取得可能。
        /// </summary>
        /// <param name="index">[Range(0, 7)] インデックス</param>
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
                    var byte0 = EventCommandConstant.Effect.TargetCode.Map;
                    return new byte[] {byte0, 0x00, 0x00, 0x00}.ToInt32(Endian.Little);

                case 2:
                    return Duration;

                case 3:
                    return 0;

                case 4:
                    return 0;

                case 5:
                    return CenterX;

                case 6:
                    return CenterY;

                case 7:
                    return ZoomRate;

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 0, 7, index));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// 数値変数を設定する。
        /// </summary>
        /// <param name="index">[Range(1, 7)] インデックス</param>
        /// <param name="value">設定値</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void SetSafetyNumberVariable(int index, int value)
        {
            switch (index)
            {
                case 1:
                    return;

                case 2:
                    Duration = value;
                    return;

                case 3:
                    return;

                case 4:
                    return;

                case 5:
                    CenterX = value;
                    return;

                case 6:
                    CenterY = value;
                    return;

                case 7:
                    ZoomRate = value;
                    return;

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 1, 7, index));
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
            var xVarName = resolver.GetNumericVariableAddressStringIfVariableAddress(CenterX, type, desc);
            var yVarName = resolver.GetNumericVariableAddressStringIfVariableAddress(CenterY, type, desc);
            var zoomVarName = resolver.GetNumericVariableAddressStringIfVariableAddress(ZoomRate, type, desc);
            var durationVarName = resolver.GetNumericVariableAddressStringIfVariableAddress(Duration, type, desc);

            return string.Format(EventCommandSentenceFormat,
                xVarName, yVarName, zoomVarName, durationVarName);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private int centerX;

        /// <summary>中心X座標</summary>
        public int CenterX
        {
            get => centerX;
            set
            {
                centerX = value;
                NotifyPropertyChanged();
            }
        }

        private int centerY;

        /// <summary>中心Y座標</summary>
        public int CenterY
        {
            get => centerY;
            set
            {
                centerY = value;
                NotifyPropertyChanged();
            }
        }

        private int zoomRate;

        /// <summary>拡大率</summary>
        public int ZoomRate
        {
            get => zoomRate;
            set
            {
                zoomRate = value;
                NotifyPropertyChanged();
            }
        }

        private int duration;

        /// <summary>処理時間</summary>
        public int Duration
        {
            get => duration;
            set
            {
                duration = value;
                NotifyPropertyChanged();
            }
        }
    }
}