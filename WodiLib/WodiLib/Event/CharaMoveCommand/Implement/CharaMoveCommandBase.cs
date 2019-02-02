// ========================================
// Project Name : WodiLib
// File Name    : CharaMoveCommandBase.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using WodiLib.Sys;

namespace WodiLib.Event.CharaMoveCommand
{
    /// <inheritdoc />
    /// <summary>
    /// <para>動作指定コマンド_共通動作クラス</para>
    /// <para>（隠蔽クラス）</para>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class CharaMoveCommandBase : ICharaMoveCommand
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>動作指定コマンドコード</summary>
        public abstract byte CommandCode { get; }

        /// <summary>変数の数</summary>
        public abstract byte ValueLengthByte { get; }

        private int valueLength = -1;

        /// <summary>変数の数</summary>
        public int ValueLength
        {
            get
            {
                if (valueLength != -1) return valueLength;
                var bytes = new byte[] {ValueLengthByte, 0x00, 0x00, 0x00};
                valueLength = bytes.ToInt32(Endian.Little);
                return valueLength;
            }
        }

        /// <summary>変数リスト</summary>
        protected int[] NumberValues { private get; set; }

        /// <summary>
        /// 数値変数の値を取得する。
        /// </summary>
        /// <param name="index">[Range(0, 変数の数-1)] 変数インデックス</param>
        /// <returns>取得した数値</returns>
        /// <exception cref="ArgumentOutOfRangeException">インデックスが指定範囲以外の場合</exception>
        public int GetNumberValue(int index)
        {
            if (index < 0 || ValueLength <= index)
                throw new ArgumentOutOfRangeException($"存在しない変数を取得しようとしました。index: {index} maxLength: {ValueLength} ");
            return NumberValues[index];
        }

        /// <summary>
        /// 数値変数の値を設定する。
        /// </summary>
        /// <param name="index">[Range(0, 変数の数-1)] 変数インデックス</param>
        /// <param name="value">設定値</param>
        /// <exception cref="ArgumentOutOfRangeException">インデックスが指定範囲以外の場合</exception>
        public void SetNumberValue(int index, int value)
        {
            if (index < 0 || ValueLength <= index)
                throw new ArgumentOutOfRangeException($"存在しない変数を取得しようとしました。index: {index} maxLength: {ValueLength} ");
            NumberValues[index] = value;
        }

        /// <summary>
        /// 終了バイトコード
        /// </summary>
        public static byte[] EndBlockCode => new byte[] {0x01, 0x00};

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        protected CharaMoveCommandBase()
        {
            NumberValues = new int[ValueLength];
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Common
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public byte[] ToBinary()
        {
            var result = new List<byte> {CommandCode, ValueLengthByte};
            // 動作コマンドコード
            // 変数の数
            // 変数
            foreach (var value in NumberValues)
            {
                result.AddRange(value.ToBytes(Endian.Woditor));
            }

            // 終端コード
            result.AddRange(EndBlockCode);
            return result.ToArray();
        }
    }
}