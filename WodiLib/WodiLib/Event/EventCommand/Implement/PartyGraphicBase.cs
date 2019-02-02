// ========================================
// Project Name : WodiLib
// File Name    : PartyGraphicBase.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// パーティ画像ベース
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class PartyGraphicBase : EventCommandBase
    {
        /// <summary>処理対象変数指定フラグ値</summary>
        private const byte FlgTargetValue = 0x01;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override int EventCommandCode => EventCommand.EventCommandCode.PartyGraphic;

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して数値変数を取得する。
        /// </summary>
        /// <param name="index">[Range(0, 1～3)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override int GetNumberVariable(int index)
        {
            if (index < 0 || NumberVariableCount <= index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, NumberVariableCount, index));
            switch (index)
            {
                case 0:
                    return EventCommandCode;

                case 1:
                {
                    var byte1 = (byte) (IsTargetingValue ? FlgTargetValue : 0x00);
                    return new byte[] {ExecCode, byte1, 0x00, 0x00}.ToInt32(Endian.Environment);
                }

                case 2:
                    return TargetIndex;

                case 3:
                    return Target.ToInt();

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// 数値変数を設定する。
        /// </summary>
        /// <param name="index">[Range(1, 1～3)] インデックス</param>
        /// <param name="value">設定値</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void SetNumberVariable(int index, int value)
        {
            if (index < 1 || NumberVariableCount <= index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 1, NumberVariableCount, index));
            switch (index)
            {
                case 1:
                {
                    var bytes = value.ToBytes(Endian.Environment);
                    ExecCode = bytes[0];
                    IsTargetingValue = (bytes[1] & FlgTargetValue) != 0;
                    return;
                }

                case 2:
                    TargetIndex = value;
                    return;

                case 3:
                    Target = value;
                    return;

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 1, NumberVariableCount, index));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して文字列変数を取得する。
        /// </summary>
        /// <param name="index">[Range(0, 0)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override string GetStringVariable(int index)
        {
            switch (index)
            {
                case 0:
                    return Target.ToStr();

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 0, 0, index));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// 文字列変数を設定する。
        /// </summary>
        /// <param name="index">[Range(0, 0)] インデックス</param>
        /// <param name="value">[NotNull] 設定値</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        /// <exception cref="ArgumentNullException">valueがnull</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void SetStringVariable(int index, string value)
        {
            if (value == null) throw new ArgumentNullException(ErrorMessage.NotNull(nameof(value)));
            switch (index)
            {
                case 0:
                    Target = value;
                    return;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Abstract Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>処理内容</summary>
        protected abstract byte ExecCode { get; set; }

        /// <summary>処理対象数値変数指定フラグ（対象文字列指定のときfalse）</summary>
        public abstract bool IsTargetingValue { get; set; }

        /// <summary>操作対象序列</summary>
        protected abstract int TargetIndex { get; set; }

        /// <summary>[NotNull] 対象指定変数または対象ファイル名</summary>
        protected abstract IntOrStr Target { get; set; }
    }
}