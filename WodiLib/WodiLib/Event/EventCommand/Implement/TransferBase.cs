// ========================================
// Project Name : WodiLib
// File Name    : TransferBase.cs
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
    /// <summary>
    /// イベントコマンド・場所移動共通クラス
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class TransferBase : EventCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat = "■場所移動：{0} {1} [{2}]";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override byte NumberVariableCount => 0x06;

        /// <inheritdoc />
        public override byte StringVariableCount => 0x00;

        /// <inheritdoc />
        protected override EventCommandColorSet EventCommandColorSet
            => EventCommandColorSet.Black;

        /// <summary>
        /// インデックスを指定して数値変数を取得する。
        /// ウディタ標準仕様でサポートしているインデックスのみ取得可能。
        /// </summary>
        /// <param name="index">[Range(0, 5)] インデックス</param>
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
                    return _Target;

                case 2:
                    return _PositionX;

                case 3:
                    return _PositionY;

                case 4:
                    if (_IsSameMap) return FlgSameMap;
                    return _DestinationMapId;

                case 5:
                {
                    byte byte0 = 0x00;
                    if (_IsPreciseCoordinates) byte0 += FlgPreciseCoordinates;
                    byte0 += _TransferOption.Code;
                    return new byte[] {byte0, 0x00, 0x00, 0x00}.ToInt32(Endian.Little);
                }

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 0, 5, index));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// 数値変数を設定する。
        /// </summary>
        /// <param name="index">[Range(1, 5)] インデックス</param>
        /// <param name="value">設定値</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void SetSafetyNumberVariable(int index, int value)
        {
            switch (index)
            {
                case 1:
                    _Target = value;
                    return;

                case 2:
                    _PositionX = value;
                    return;

                case 3:
                    _PositionY = value;
                    return;

                case 4:
                    _DestinationMapId = value;
                    _IsSameMap = value == FlgSameMap;
                    return;

                case 5:
                {
                    var bytes = value.ToBytes(Endian.Little);
                    _TransferOption = TransferOption.FromByte((byte) (bytes[0] & 0xF0));
                    _IsPreciseCoordinates = (bytes[0] & FlgPreciseCoordinates) != 0;
                    return;
                }

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 1, 5, index));
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
            var targetStr = resolver.GetTransferEventName(_Target, type, desc);
            var moveParamStr = MakeEventCommandMoveParamSentence(resolver, type, desc);

            return string.Format(EventCommandSentenceFormat,
                targetStr, moveParamStr, _TransferOption.EventCommandSentence);
        }

        /// <summary>
        /// イベントコマンド文字列のメイン部分（インデント以降の部分）を生成する。
        /// </summary>
        /// <param name="resolver">[NotNull] 名前解決クラスインスタンス</param>
        /// <param name="type">[NotNull] イベント種別</param>
        /// <param name="desc">[Nullable] 付加情報</param>
        /// <returns>イベントコマンド文字列のメイン部分</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected abstract string MakeEventCommandMoveParamSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc);

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>対象</summary>
        protected int _Target { get; set; }

        /// <summary>移動先マップ</summary>
        protected int _DestinationMapId { get; set; }

        /// <summary>同じマップ</summary>
        protected bool _IsSameMap { get; set; }

        /// <summary>移動先座標X</summary>
        protected int _PositionX { get; set; }

        /// <summary>移動先座標Y</summary>
        protected int _PositionY { get; set; }

        /// <summary>精密座標</summary>
        protected bool _IsPreciseCoordinates { get; set; }

        /// <summary>位置登録ID</summary>
        protected int _SavedPositionId
        {
            get => _PositionX;
            set => _PositionX = value;
        }

        private TransferOption transferOption = TransferOption.NoTransition;

        /// <summary>[NotNull] 場所移動時トランジションオプション</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        protected TransferOption _TransferOption
        {
            get => transferOption;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(_TransferOption)));
                transferOption = value;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Const
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private readonly int FlgSameMap = -1;
        private readonly byte FlgPreciseCoordinates = 0x01;

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
            if (_TransferOption.Code != 0)
            {
                Logger.Warning(VersionWarningMessage.NotUnderInCommand(
                    $"{nameof(ScrollScreen)}.{nameof(_TransferOption)}",
                    VersionConfig.GetConfigWoditorVersion(),
                    WoditorVersion.Ver2_00));
            }
        }
    }
}