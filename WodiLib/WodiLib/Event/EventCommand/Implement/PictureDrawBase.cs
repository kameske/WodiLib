// ========================================
// Project Name : WodiLib
// File Name    : PictureDrawBase.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・ピクチャ（表示・移動）ベース
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class PictureDrawBase : EventCommandBase
    {
        /// <summary>自由変形フラグ表示位置コード値</summary>
        private static readonly byte FreePositionPositionCode = PictureAnchorPosition.Center.Code;

        /// <summary>自由変形フラグコード値</summary>
        private static readonly byte FreePositionFlagCode = 0x04;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>数値変数最大数</summary>
        public static readonly int NumberVariableMax = 26;

        /// <inheritdoc />
        public override byte NumberVariableCount
        {
            get
            {
                // 自由変形あり
                if (_IsFreePosition) return 0x1A;
                // 拡大率別々
                if (zoomRate.ZoomRateType == ZoomRateType.Different && !zoomRate.IsDefaultRate) return 0x14;
                // カラー指定ありかついずれかが100以外
                if (!IsSameColor)
                {
                    if (ColorR != 100) return 0x13;
                    if (ColorG != 100) return 0x13;
                    if (ColorB != 100) return 0x13;
                }

                // 連続ピクチャ指定あり
                if (IsMultiTarget) return 0x10;
                // 発動ディレイ指定
                if (Delay != 0) return 0x0F;
                // カラー同値ON
                if (IsSameColor) return 0x0E;
                // 読み込みファイル変数指定
                if (IsLoadForVariableAddress) return 0x0D;
                // 拡大率「縦のみ」「横のみ」
                if ((zoomRate.ZoomRateType == ZoomRateType.OnlyDepth || zoomRate.ZoomRateType == ZoomRateType.OnlyWidth)
                    && !zoomRate.IsDefaultRate) return 0x0D;

                return 0x0C;
            }
        }

        /// <inheritdoc />
        public override byte StringVariableCount => (byte) (IsUseString ? 0x01 : 0x00);

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して数値変数を取得する。
        /// </summary>
        /// <param name="index">[Range(0, 25)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override int GetNumberVariable(int index)
        {
            // 画像表示は数値引数の数を決定する。めに数値引数自身の数値を参照する。め、
            // 他のイベントコマンドのようにプロパティから許容値を算出できない
            // （＝状態によらず固定範囲のIOを可能にする。
            if (index < 0 || NumberVariableMax < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, NumberVariableMax - 1, index));
            switch (index)
            {
                case 0:
                    return EventCommandCode.Code;

                case 1:
                {
                    var byte0 = (byte) (DisplayTypeCode + ExecCode);
                    var byte1 = (byte) (AnchorPositionCode + DrawTypeCode);
                    var byte2 = zoomRate.ZoomRateType.Code;
                    byte2 += (byte) (IsRelativeCoordinate ? FlgRelativeCoordinate : 0x00);
                    byte byte3 = 0x00;
                    {
                        // byte3
                        byte3 += (byte) (IsMultiTarget ? FlgMultiTarget : 0x00);
                        byte3 += (byte) (_IsLinkScroll ? FlgLinkScroll : 0x00);
                        byte3 += (byte) (_IsFreePosition ? FlgFreePosition : 0x00);
                    }
                    return new[] {byte0, byte1, byte2, byte3}.ToInt32(Endian.Environment);
                }

                case 2:
                    return PictureNumber;

                case 3:
                    return ProcessTime;

                case 4:
                    return _DivisionWidth;

                case 5:
                    return _DivisionHeight;

                case 6:
                    return Pattern;

                case 7:
                    return Opacity;

                case 8:
                    return _IsFreePosition
                        ? Position.FreePositionLeftUpX
                        : Position.NormalPositionX;

                case 9:
                    return _IsFreePosition
                        ? Position.FreePositionLeftUpY
                        : Position.NormalPositionY;

                case 10:
                {
                    return zoomRate.IsDifference
                        ? ZoomRateWidth
                        : ZoomRate;
                }

                case 11:
                    return Angle;

                case 12:
                    return _LoadFireStringVar;

                case 13:
                {
                    var byte3 = IsSameColor ? FlgSameColor : FlgDifferColor;
                    return new byte[] {0x00, 0x00, 0x00, byte3}.ToInt32(Endian.Little);
                }
                case 14:
                    return Delay;

                case 15:
                    return SequenceValue;

                case 16:
                    return ColorR;

                case 17:
                    return ColorG;

                case 18:
                    return ColorB;

                case 19:
                    return ZoomRateHeight;

                case 20:
                    return Position.FreePositionRightUpX;

                case 21:
                    return Position.FreePositionRightUpY;

                case 22:
                    return Position.FreePositionLeftDownX;

                case 23:
                    return Position.FreePositionLeftDownY;

                case 24:
                    return Position.FreePositionRightDownX;

                case 25:
                    return Position.FreePositionRightDownY;

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 0, NumberVariableCount, index));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// 数値変数を設定する。
        /// </summary>
        /// <param name="index">[Range(1, 25)] インデックス</param>
        /// <param name="value">設定値</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void SetNumberVariable(int index, int value)
        {
            if (index < 0 || NumberVariableMax < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 1, NumberVariableMax - 1, index));
            switch (index)
            {
                case 1:
                {
                    var bytes = value.ToBytes(Endian.Environment);
                    DisplayTypeCode = (byte) (bytes[0] & 0xF0);
                    ExecCode = (byte) (bytes[0] & 0x0F);

                    AnchorPositionCode = (byte) (bytes[1] & 0xF0);
                    DrawTypeCode = (byte) (bytes[1] & 0x0F);

                    zoomRate.ZoomRateType = ZoomRateType.FromByte((byte) (bytes[2] & 0xF0));
                    IsRelativeCoordinate = (bytes[2] & 0x0F) == FlgRelativeCoordinate;

                    IsMultiTarget = (bytes[3] & FlgMultiTarget) != 0;
                    _IsLinkScroll = (bytes[3] & FlgLinkScroll) != 0;
                    _IsFreePosition = (bytes[3] & FlgFreePosition) != 0;
                    return;
                }

                case 2:
                    PictureNumber = value;
                    return;

                case 3:
                    ProcessTime = value;
                    return;

                case 4:
                    _DivisionWidth = value;
                    return;

                case 5:
                    _DivisionHeight = value;
                    return;

                case 6:
                    Pattern = value;
                    return;

                case 7:
                    Opacity = value;
                    return;

                case 8:
                    if (_IsFreePosition) Position.FreePositionLeftUpX = value;
                    else Position.NormalPositionX = value;
                    return;

                case 9:
                    if (_IsFreePosition) Position.FreePositionLeftUpY = value;
                    else Position.NormalPositionY = value;
                    return;

                case 10:
                    if (zoomRate.IsDifference) ZoomRateWidth = value;
                    else ZoomRate = value;
                    return;

                case 11:
                    Angle = value;
                    return;

                case 12:
                    _LoadFireStringVar = value;
                    return;

                case 13:
                {
                    var bytes = value.ToBytes(Endian.Environment);
                    IsSameColor = bytes[3] == FlgSameColor;
                    return;
                }
                case 14:
                    Delay = value;
                    return;

                case 15:
                    SequenceValue = value;
                    return;

                case 16:
                    ColorR = value;
                    return;

                case 17:
                    ColorG = value;
                    return;

                case 18:
                    ColorB = value;
                    return;

                case 19:
                    ZoomRateHeight = value;
                    return;

                case 20:
                    Position.FreePositionRightUpX = value;
                    return;

                case 21:
                    Position.FreePositionRightUpY = value;
                    return;

                case 22:
                    Position.FreePositionLeftDownX = value;
                    return;

                case 23:
                    Position.FreePositionLeftDownY = value;
                    return;

                case 24:
                    Position.FreePositionRightDownX = value;
                    return;

                case 25:
                    Position.FreePositionRightDownY = value;
                    return;

                default:
                    throw new ArgumentOutOfRangeException(ErrorMessage.OutOfRange(
                        nameof(index), 1, NumberVariableCount, index));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して文字列変数を取得する。
        /// </summary>
        /// <param name="index">[Range(0, -1～0)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override string GetStringVariable(int index)
        {
            if (index < 0 || StringVariableCount <= index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, StringVariableCount - 1, index));
            switch (index)
            {
                case 0:
                    return LoadFileNameOrDrawString;

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 0, StringVariableCount - 1, index));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// 文字列変数を設定する。
        /// </summary>
        /// <param name="index">[Range(0, -1～0)] インデックス</param>
        /// <param name="value">[NotNull] 設定値</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        /// <exception cref="ArgumentNullException">valueがnull</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void SetStringVariable(int index, string value)
        {
            if (value == null) throw new ArgumentNullException(ErrorMessage.NotNull(nameof(value)));
            if (index < 0 || StringVariableCount <= index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, StringVariableCount - 1, index));
            switch (index)
            {
                case 0:
                    LoadFileNameOrDrawString = value;
                    return;

                default:
                    throw new ArgumentOutOfRangeException(ErrorMessage.OutOfRange(
                        nameof(index), 0, StringVariableCount - 1, index));
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Abstract Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>読み込みファイル指定文字列変数</summary>
        protected abstract int _LoadFireStringVar { get; set; }

        /// <summary>分割数横</summary>
        protected abstract int _DivisionWidth { get; set; }

        /// <summary>分割数縦</summary>
        protected abstract int _DivisionHeight { get; set; }

        /// <summary>スクロールとリンク</summary>
        protected abstract bool _IsLinkScroll { get; set; }

        /// <summary>読み込みファイル名または表示文字列</summary>
        protected abstract string LoadFileNameOrDrawString { get; set; }

        /// <summary>表示タイプコード</summary>
        protected abstract byte DisplayTypeCode { get; set; }

        /// <summary>処理内容コード</summary>
        protected abstract byte ExecCode { get; set; }

        /// <summary>表示位置コード</summary>
        protected abstract byte AnchorPositionCode { get; set; }

        /// <summary>表示形式コード</summary>
        protected abstract byte DrawTypeCode { get; set; }

        /// <summary>文字列使用フラグ</summary>
        protected abstract bool IsUseString { get; }

        /// <summary>文字列変数指定フラグフラグ</summary>
        protected abstract bool IsLoadForVariableAddress { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>ピクチャ番号</summary>
        public int PictureNumber { get; set; }

        /// <summary>連続ピクチャ操作フラグ</summary>
        public bool IsMultiTarget { get; set; }

        /// <summary>連続ピクチャ数</summary>
        public int SequenceValue { get; set; }

        private NormalOrFreePosition position = new NormalOrFreePosition();

        /// <summary>[NotNull] 座標</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public NormalOrFreePosition Position
        {
            get => position;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Position)));
                position = value;
            }
        }

        /// <summary>座標相対モード</summary>
        public bool IsRelativeCoordinate { get; set; }

        /// <summary>自由変形フラグコード</summary>
        private byte IsFreePositionCode { get; set; }

        /// <summary>自由変形フラグ</summary>
        protected bool _IsFreePosition
        {
            get => AnchorPositionCode == FreePositionPositionCode && IsFreePositionCode == FreePositionFlagCode;
            set
            {
                if (value)
                {
                    AnchorPositionCode = FreePositionPositionCode;
                }

                IsFreePositionCode = (byte) (value ? FreePositionFlagCode : 0x00);
            }
        }

        /// <summary>パターン</summary>
        public int Pattern
        {
            get => pattern.Value;
            set => pattern.Value = value;
        }

        /// <summary>パターン同値</summary>
        public bool IsSamePattern
        {
            get => pattern.IsSame;
            set => pattern.IsSame = value;
        }

        /// <summary>不透明度</summary>
        public int Opacity
        {
            get => opacity.Value;
            set => opacity.Value = value;
        }

        /// <summary>不透明度同値</summary>
        public bool IsSameOpacity
        {
            get => opacity.IsSame;
            set => opacity.IsSame = value;
        }

        private PictureDrawType printType = PictureDrawType.Normal;

        /// <summary>[NotNull] 表示形式</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public PictureDrawType PrintType
        {
            get => printType;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(PrintType)));
                printType = value;
            }
        }

        /// <summary>表示形式同値</summary>
        public bool IsSamePrintType { get; set; }

        /// <summary>処理時間</summary>
        public int ProcessTime { get; set; }

        /// <summary>発動ディレイ</summary>
        public int Delay { get; set; }

        /// <summary>角度</summary>
        public int Angle
        {
            get => angle.Value;
            set => angle.Value = value;
        }

        /// <summary>角度同値</summary>
        public bool IsSameAngle
        {
            get => angle.IsSame;
            set => angle.IsSame = value;
        }

        /// <summary>拡大率（縦横同じ）</summary>
        public int ZoomRate
        {
            get => zoomRate.Rate;
            set => zoomRate.Rate = value;
        }

        /// <summary>拡大率（横）</summary>
        public int ZoomRateWidth
        {
            get => zoomRate.IsDifference
                ? zoomRate.RateWidth
                : 0;
            set
            {
                if (zoomRate.IsDifference) zoomRate.RateWidth = value;
            }
        }

        /// <summary>拡大率（縦）</summary>
        public int ZoomRateHeight
        {
            get => zoomRate.IsDifference
                ? zoomRate.RateHeight
                : 0;
            set
            {
                if (zoomRate.IsDifference) zoomRate.RateHeight = value;
            }
        }

        /// <summary>
        /// [NotNull] 拡大率種別
        /// </summary>
        /// <exception cref="PropertyNullException">nullを設定した場合</exception>
        public ZoomRateType ZoomRateType
        {
            get => zoomRate.ZoomRateType;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(ZoomRateType)));
                zoomRate.ZoomRateType = value;
            }
        }

        /// <summary>カラーR</summary>
        public int ColorR
        {
            get => color.R;
            set => color.R = value;
        }

        /// <summary>カラーG</summary>
        public int ColorG
        {
            get => color.G;
            set => color.G = value;
        }

        /// <summary>カラーB</summary>
        public int ColorB
        {
            get => color.B;
            set => color.B = value;
        }

        /// <summary>カラー同値</summary>
        public bool IsSameColor { get; set; }

        private readonly CanSameInt pattern = new CanSameInt();
        private readonly CanSameInt opacity = new CanSameInt();
        private readonly CanSameInt angle = new CanSameInt();
        private readonly ZoomRate zoomRate = new ZoomRate();
        private readonly Color color = new Color();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>座標相対指定フラグ値</summary>
        private const byte FlgRelativeCoordinate = 0x01;

        /// <summary>ピクチャ連続フラグ値</summary>
        private const byte FlgMultiTarget = 0x01;

        /// <summary>スクロールとリンクフラグ値</summary>
        private const byte FlgLinkScroll = 0x02;

        /// <summary>自由変形フラグ値</summary>
        private const byte FlgFreePosition = 0x04;

        /// <summary>カラー同値フラグ値</summary>
        private const byte FlgSameColor = 1;

        /// <summary>カラー指定フラグ値</summary>
        private const byte FlgDifferColor = 2;

        private class CanSameInt
        {
            /// <summary>パターン同値フラグ値</summary>
            private static readonly int SameValue = new byte[] {0xFF, 0xF0, 0xDB, 0xC0}.ToInt32(Endian.Environment);

            private int value;

            public int Value
            {
                get => value;
                set
                {
                    this.value = value;
                    isSame = value == SameValue;
                }
            }

            private bool isSame;

            public bool IsSame
            {
                get => isSame;
                set
                {
                    isSame = value;
                    if (value) this.value = SameValue;
                }
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     VersionCheck
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>
        /// VersionConfigにセットされたバージョンとイベントコマンドの内容を確認し、
        /// イベントコマンドの内容が設定バージョンに対応していないものであれば警告ログを出力する。
        /// </summary>
        public override void OutputVersionWarningLogIfNeed()
        {
            if (VersionConfig.IsUnderVersion(WoditorVersion.Ver2_00))
            {
                OutputVersionWarningLogIfNeed_UnderVer2_00();
            }
        }

        /// <summary>
        /// 設定バージョン = 2.00未満 の場合の警告
        /// </summary>
        private void OutputVersionWarningLogIfNeed_UnderVer2_00()
        {
            if (_IsFreePosition)
            {
                Logger.Warning(VersionWarningMessage.NotUnderInCommandSetting($"{nameof(PictureDrawBase)}.{nameof(_IsFreePosition)}",
                    $"{true}",
                    VersionConfig.GetConfigWoditorVersion(),
                    WoditorVersion.Ver2_00));
            }
        }
    }
}