// ========================================
// Project Name : WodiLib
// File Name    : PictureDrawBase.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.Project;
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
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>自由変形フラグ表示位置コード値</summary>
        private static readonly byte FreePositionPositionCode = PictureAnchorPosition.Center.Code;

        /// <summary>自由変形フラグコード値</summary>
        private const byte FreePositionFlagCode = 0x04;

        /// <summary>同値（パターン/透過度/角度）</summary>
        private const int SameValue = -1000000;

        private const string EventCommandSentenceFormat
            = "■ﾋﾟｸﾁｬ{0}：{1} {2}{3}{4}{5} / {6}({7})ﾌﾚｰﾑ  / ﾊﾟﾀｰﾝ {8} / 透 {9} / {10} {11}ｶﾗｰ {12}";

        private const string EventCommandSentenceNormalPositionOption = " / 角 {0} / 拡 {1} / ";

        private const string EventCommandSentenceSingleTarget = "";
        private const string EventCommandSentenceMultiTarget = "～ {0} ";

        private const string EventCommandSentenceRelativeCoordinate = "相対";
        private const string EventCommandSentenceNotRelativeCoordinate = " ";

        private const string EventCommandSentenceValueSame = "同値";

        private const string EventCommandSentenceSamePrintType = "表示形式:同値";

        /// <summary>表示種別文字列（表示/移動）</summary>
        protected abstract string DrawTypeStr { get; }

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
                if (ZoomRateType == ZoomRateType.Different && !zoomRate.IsDefaultRate) return 0x14;
                // カラー指定ありかついずれかが100以外
                if (!IsSameColor)
                {
                    if (ColorR != 100) return 0x13;
                    if (ColorG != 100) return 0x13;
                    if (ColorB != 100) return 0x13;
                }

                // 連続ピクチャ指定あり
                if (IsMultiTarget) return IsSameColor ? (byte) 0x10 : (byte) 0x13;
                // 発動ディレイ指定
                if (Delay != 0) return IsSameColor ? (byte) 0x0F : (byte) 0x13;
                // カラー同値ON
                if (IsSameColor) return 0x0E;
                // 読み込みファイル変数指定
                if (IsLoadForVariableAddress) return 0x0D;
                // 拡大率「縦のみ」「横のみ」
                if ((ZoomRateType == ZoomRateType.OnlyDepth || ZoomRateType == ZoomRateType.OnlyWidth)
                    && !zoomRate.IsDefaultRate) return 0x0D;

                return 0x0C;
            }
        }

        /// <inheritdoc />
        public override byte StringVariableCount => (byte) (IsUseString ? 0x01 : 0x00);

        /// <inheritdoc />
        /// <summary>数値変数最小個数</summary>
        public override byte NumberVariableCountMin => 0x0C;

        /// <inheritdoc />
        /// <summary>文字列変数最小個数</summary>
        public override byte StringVariableCountMin => 0x00;

        /// <inheritdoc />
        protected override EventCommandColorSet EventCommandColorSet
            => EventCommandColorSet.BrightGreen;

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して数値変数を取得する。
        /// ウディタ標準仕様でサポートしているインデックスのみ取得可能。
        /// </summary>
        /// <param name="index">[Range(0, 25)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override int GetSafetyNumberVariable(int index)
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
        public override void SetSafetyNumberVariable(int index, int value)
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
        /// ウディタ標準仕様でサポートしているインデックスのみ取得可能。
        /// </summary>
        /// <param name="index">[Range(0, -1～0)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override string GetSafetyStringVariable(int index)
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
        public override void SetSafetyStringVariable(int index, string value)
        {
            if (value is null) throw new ArgumentNullException(ErrorMessage.NotNull(nameof(value)));
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

        /// <summary>
        /// 指定した数値引数インデックスが通常使用の範囲であるか（拡張引数でないか）を返す。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns>通常使用範囲の引数インデックスの場合true</returns>
        internal override bool IsNormalNumberArgIndex(int index) => index <= 0x1A;

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override string MakeEventCommandMainSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            var pictureNumberStr = resolver.GetNumericVariableAddressStringIfVariableAddress(PictureNumber, type, desc);
            string pictureEndStr;
            if (IsMultiTarget)
            {
                var pictureEndVarName =
                    resolver.GetNumericVariableAddressStringIfVariableAddress(SequenceValue, type, desc);
                pictureEndStr = string.Format(EventCommandSentenceMultiTarget, pictureEndVarName);
            }
            else
            {
                pictureEndStr = EventCommandSentenceSingleTarget;
            }

            var anchorStr = MakeEventCommandAnchorSentence();
            var itemStr = MakeEventCommandDrawItemSentence(resolver, type, desc);
            var positionStr = _IsFreePosition
                ? Position.GetEventCommandSentenceFree(resolver, type, desc)
                : Position.GetEventCommandSentenceNormal(resolver, type, desc);
            positionStr = IsRelativeCoordinate
                ? $"{EventCommandSentenceRelativeCoordinate}{positionStr}"
                : $"{EventCommandSentenceNotRelativeCoordinate}{positionStr}";
            var processTimeStr = resolver.GetNumericVariableAddressStringIfVariableAddress(
                ProcessTime, type, desc);
            var delayStr = resolver.GetNumericVariableAddressStringIfVariableAddress(
                Delay, type, desc);
            var patternStr = IsSamePattern
                ? EventCommandSentenceValueSame
                : resolver.GetNumericVariableAddressStringIfVariableAddress(
                    Pattern, type, desc);
            var opacityStr = IsSameOpacity
                ? EventCommandSentenceValueSame
                : resolver.GetNumericVariableAddressStringIfVariableAddress(
                    Opacity, type, desc);
            var angleStr = IsSameAngle
                ? EventCommandSentenceValueSame
                : resolver.GetNumericVariableAddressStringIfVariableAddress(
                    Angle, type, desc);
            var printTypeStr = IsSamePrintType
                ? EventCommandSentenceSamePrintType
                : PrintType.EventCommandSentence;
            var zoomRateStr = zoomRate.GetEventCommandSentence(resolver, type, desc);
            var colorStr = IsSameColor
                ? EventCommandSentenceValueSame
                : color.GetEventCommandSentence(resolver, type, desc);

            var normalPositionStr = _IsFreePosition
                ? ""
                : string.Format(EventCommandSentenceNormalPositionOption, angleStr, zoomRateStr);

            return string.Format(EventCommandSentenceFormat,
                DrawTypeStr, pictureNumberStr, anchorStr, pictureEndStr, itemStr, positionStr,
                processTimeStr, delayStr, patternStr, opacityStr, printTypeStr,
                normalPositionStr, colorStr);
        }

        /// <summary>
        /// イベントコマンド文字列の表示基準部分を生成する。
        /// </summary>
        /// <returns>イベントコマンド文字列の表示基準部分</returns>
        protected abstract string MakeEventCommandAnchorSentence();

        /// <summary>
        /// イベントコマンド文字列の表示内容部分を生成する。
        /// </summary>
        /// <param name="resolver">[NotNull] 名前解決クラスインスタンス</param>
        /// <param name="type">[NotNull] イベント種別</param>
        /// <param name="desc">[Nullable] 付加情報</param>
        /// <returns>イベントコマンド文字列の表示内容部分</returns>
        protected abstract string MakeEventCommandDrawItemSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc);

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

        private int pictureNumber;

        /// <summary>ピクチャ番号</summary>
        public int PictureNumber
        {
            get => pictureNumber;
            set
            {
                pictureNumber = value;
                NotifyPropertyChanged();
            }
        }

        private bool isMultiTarget;

        /// <summary>連続ピクチャ操作フラグ</summary>
        public bool IsMultiTarget
        {
            get => isMultiTarget;
            set
            {
                isMultiTarget = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(NumberVariableCount));
            }
        }

        private int sequenceValue;

        /// <summary>連続ピクチャ数</summary>
        public int SequenceValue
        {
            get => sequenceValue;
            set
            {
                sequenceValue = value;
                NotifyPropertyChanged();
            }
        }

        private NormalOrFreePosition position = new NormalOrFreePosition();

        /// <summary>[NotNull] 座標</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public NormalOrFreePosition Position
        {
            get => position;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Position)));
                position = value;
                NotifyPropertyChanged();
            }
        }

        private bool isRelativeCoordinate;

        /// <summary>座標相対モード</summary>
        public bool IsRelativeCoordinate
        {
            get => isRelativeCoordinate;
            set
            {
                isRelativeCoordinate = value;
                NotifyPropertyChanged();
            }
        }

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
                NotifyPropertyChanged(nameof(NumberVariableCount));
            }
        }

        /// <summary>パターン</summary>
        public int Pattern
        {
            get => pattern.Value;
            set
            {
                pattern.Value = value;
                NotifyPropertyChanged();
                IsSamePattern = value == SameValue;
                NotifyPropertyChanged(nameof(NumberVariableCount));
            }
        }

        /// <summary>パターン同値</summary>
        public bool IsSamePattern
        {
            get => pattern.IsSame;
            set
            {
                pattern.IsSame = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>不透明度</summary>
        public int Opacity
        {
            get => opacity.Value;
            set
            {
                opacity.Value = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>不透明度同値</summary>
        public bool IsSameOpacity
        {
            get => opacity.IsSame;
            set
            {
                opacity.IsSame = value;
                NotifyPropertyChanged();
            }
        }

        private PictureDrawType printType = PictureDrawType.Normal;

        /// <summary>[NotNull] 表示形式</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public PictureDrawType PrintType
        {
            get => printType;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(PrintType)));
                printType = value;
                NotifyPropertyChanged();
            }
        }

        private bool isSamePrintType;

        /// <summary>表示形式同値</summary>
        public bool IsSamePrintType
        {
            get => isSamePrintType;
            set
            {
                isSamePrintType = value;
                NotifyPropertyChanged();
            }
        }

        private int processTime;

        /// <summary>処理時間</summary>
        public int ProcessTime
        {
            get => processTime;
            set
            {
                processTime = value;
                NotifyPropertyChanged();
            }
        }

        private int delay;

        /// <summary>発動ディレイ</summary>
        public int Delay
        {
            get => delay;
            set
            {
                delay = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(NumberVariableCount));
            }
        }

        /// <summary>角度</summary>
        public int Angle
        {
            get => angle.Value;
            set
            {
                angle.Value = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>角度同値</summary>
        public bool IsSameAngle
        {
            get => angle.IsSame;
            set
            {
                angle.IsSame = value;
                NotifyPropertyChanged();
            }
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
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(ZoomRateType)));
                zoomRate.ZoomRateType = value;
            }
        }

        /// <summary>カラーR</summary>
        public int ColorR
        {
            get => color.R;
            set
            {
                color.R = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(NumberVariableCount));
            }
        }

        /// <summary>カラーG</summary>
        public int ColorG
        {
            get => color.G;
            set
            {
                color.G = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(NumberVariableCount));
            }
        }

        /// <summary>カラーB</summary>
        public int ColorB
        {
            get => color.B;
            set
            {
                color.B = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(NumberVariableCount));
            }
        }

        private bool isSameColor;

        /// <summary>カラー同値</summary>
        public bool IsSameColor
        {
            get => isSameColor;
            set
            {
                isSameColor = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(NumberVariableCount));
            }
        }

        private readonly CanSameInt pattern = new CanSameInt(1);
        private readonly CanSameInt opacity = new CanSameInt(255);
        private readonly CanSameInt angle = new CanSameInt(0);
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
            private static readonly int SameValue = -1000000;

            private int DefaultValue { get; }

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
                    var before = isSame;
                    isSame = value;
                    if (value) this.value = SameValue;

                    if (before && !isSame) this.value = DefaultValue;
                }
            }

            public CanSameInt(int defaultValueValue)
            {
                DefaultValue = defaultValueValue;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     InnerNotifyChanged
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 拡大率プロパティ変更通知
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="args">情報</param>
        private void OnZoomRatePropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            switch (args.PropertyName)
            {
                case nameof(EventCommand.ZoomRate.ZoomRateType):
                    NotifyPropertyChanged(nameof(ZoomRateType));
                    NotifyPropertyChanged(nameof(NumberVariableCount));
                    break;

                case nameof(EventCommand.ZoomRate.IsDifference):
                    NotifyPropertyChanged(nameof(ZoomRateWidth));
                    NotifyPropertyChanged(nameof(ZoomRateHeight));
                    NotifyPropertyChanged(nameof(NumberVariableCount));
                    break;

                case nameof(EventCommand.ZoomRate.IsSame):
                    break;

                case nameof(EventCommand.ZoomRate.Rate):
                    NotifyPropertyChanged(nameof(ZoomRate));
                    break;

                case nameof(EventCommand.ZoomRate.RateWidth):
                    NotifyPropertyChanged(nameof(ZoomRateWidth));
                    break;

                case nameof(EventCommand.ZoomRate.RateHeight):
                    NotifyPropertyChanged(nameof(ZoomRateHeight));
                    break;

                case nameof(EventCommand.ZoomRate.IsDefaultRate):
                    NotifyPropertyChanged(nameof(NumberVariableCount));
                    break;
            }
        }

        /// <summary>
        /// 色プロパティ変更通知
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="args">情報</param>
        private void OnColorPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            switch (args.PropertyName)
            {
                case nameof(Color.R):
                    NotifyPropertyChanged(nameof(ColorR));
                    NotifyPropertyChanged(nameof(NumberVariableCount));
                    break;

                case nameof(Color.G):
                    NotifyPropertyChanged(nameof(ColorG));
                    NotifyPropertyChanged(nameof(NumberVariableCount));
                    break;

                case nameof(Color.B):
                    NotifyPropertyChanged(nameof(ColorB));
                    NotifyPropertyChanged(nameof(NumberVariableCount));
                    break;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PictureDrawBase()
        {
            zoomRate.PropertyChanged += OnZoomRatePropertyChanged;
            color.PropertyChanged += OnColorPropertyChanged;
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
                Logger.Warning(VersionWarningMessage.NotUnderInCommandSetting(
                    $"{nameof(PictureDrawBase)}.{nameof(_IsFreePosition)}",
                    $"{true}",
                    VersionConfig.GetConfigWoditorVersion(),
                    WoditorVersion.Ver2_00));
            }
        }
    }
}