// ========================================
// Project Name : WodiLib
// File Name    : ChangeScreenColor.cs
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
    /// イベントコマンド・色調変更
    /// </summary>
    [Serializable]
    public class ChangeScreenColor : EventCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat
            = "■色調変更：{4} R{0} G{1} B{2} / {3}ﾌﾚｰﾑ";

        private const string EventCommandSentenceFlash = "(フラッシュ)";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override EventCommandCode EventCommandCode => EventCommandCode.ChangeScreenColor;

        /// <inheritdoc />
        public override byte NumberVariableCount
        {
            get
            {
                if (IsNeedExpandNumberArg()) return NumberVariableCountMax;
                return 0x03;
            }
        }

        /// <inheritdoc />
        public override byte StringVariableCount => 0x00;

        /// <inheritdoc />
        /// <summary>数値変数最小個数</summary>
        public override byte NumberVariableCountMin => 0x03;

        /// <summary>数値引数の数最大値</summary>
        private byte NumberVariableCountMax => 0x06;

        /// <inheritdoc />
        protected override EventCommandColorSet EventCommandColorSet
            => EventCommandColorSet.BrightGreen;

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して数値変数を取得する。
        /// ウディタ標準仕様でサポートしているインデックスのみ取得可能。
        /// </summary>
        /// <param name="index">[Range(0, 2～5)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override int GetSafetyNumberVariable(int index)
        {
            if (index < 0 || NumberVariableCount <= index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, NumberVariableCount - 1, index));

            switch (index)
            {
                case 0:
                    return EventCommandCode.Code;

                case 1:
                    if (IsNeedExpandNumberArg())
                    {
                        byte[] flushBytes =
                        {
                            0,
                            0,
                            0,
                            (byte) (IsFlush ? 1 : 0)
                        };
                        return flushBytes.ToInt32(Endian.Little);
                    }

                    byte[] bytes =
                    {
                        Color.ByteR,
                        Color.ByteG,
                        Color.ByteB,
                        (byte) (IsFlush ? 1 : 0)
                    };
                    return bytes.ToInt32(Endian.Little);

                case 2:
                    return ProcessTime;

                case 3:
                    return Color.R;

                case 4:
                    return Color.G;

                case 5:
                    return Color.B;

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 0, NumberVariableCount - 1, index));
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
            // 引数3～5の使用フラグを引数0～2で管理していないため、他とは異なる処理
            //   引数3～5は取得できない場合があるが、設定は無条件で可能
            switch (index)
            {
                case 1:
                    var bytes = value.ToBytes(Endian.Environment);
                    Color = new Color
                    {
                        ByteR = bytes[0],
                        ByteG = bytes[1],
                        ByteB = bytes[2]
                    };
                    IsFlush = bytes[3] == 0x01;
                    return;

                case 2:
                    ProcessTime = value;
                    return;

                case 3:
                    Color.R = value;
                    return;

                case 4:
                    Color.G = value;
                    return;

                case 5:
                    Color.B = value;
                    return;

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 1, NumberVariableCountMax - 1, index));
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
        internal override bool IsNormalNumberArgIndex(int index) => index < NumberVariableCountMax;

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override string MakeEventCommandMainSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            var flash = IsFlush ? EventCommandSentenceFlash : string.Empty;

            return string.Format(EventCommandSentenceFormat,
                resolver.GetNumericVariableAddressStringIfVariableAddress(Color.R, type, desc),
                resolver.GetNumericVariableAddressStringIfVariableAddress(Color.G, type, desc),
                resolver.GetNumericVariableAddressStringIfVariableAddress(Color.B, type, desc),
                resolver.GetNumericVariableAddressStringIfVariableAddress(ProcessTime, type, desc),
                flash);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private Color color = new Color
        {
            R = 100,
            G = 100,
            B = 100
        };

        /// <summary>[NotNull] カラー</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public Color Color
        {
            get => color;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Color)));
                color = value;
            }
        }

        /// <summary>フラッシュにする</summary>
        public bool IsFlush { get; set; }

        /// <summary>処理時間</summary>
        public int ProcessTime { get; set; }

        private bool IsNeedExpandNumberArg()
        {
            return Color.R > 200 || Color.G > 200 || Color.B > 200;
        }
    }
}