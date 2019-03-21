// ========================================
// Project Name : WodiLib
// File Name    : CommonEventReturnValue.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using WodiLib.Sys;

namespace WodiLib.Common
{
    /// <summary>
    /// コモンイベント返戻値（Ver2.00～）
    /// </summary>
    internal class CommonEventReturnValue
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>返戻セルフ変数インデックス最大値</summary>
        private static readonly int ReturnVariableIndexMax = 99;

        /// <summary>返戻セルフ変数インデックス最大値</summary>
        private static readonly int ReturnVariableIndexMin = 0;

        /// <summary>値を返さない場合の返戻アドレス値</summary>
        private static readonly int ReturnValueForNotReturn = -1;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private string description = "";

        /// <summary>
        /// [NotNull] 返戻値の意味
        /// </summary>
        /// <exception cref="PropertyNullException">nullを設定した場合</exception>
        public string Description
        {
            get => description;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(description)));
                description = value;
            }
        }

        /// <summary>
        /// 値を返すフラグ
        /// </summary>
        public bool IsReturnValue { get; private set; }

        /// <summary>
        /// 返戻セルフ変数インデックス（値を返さない場合-1）
        /// </summary>
        public int ReturnVariableIndex { get; private set; } = -1;


        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// 返戻コモンセルフ変数をセットする。
        /// </summary>
        /// <param name="commonVariableIndex">[Range(-1, 99)] 返戻アドレス</param>
        /// <exception cref="ArgumentOutOfRangeException">commonVarAddressが指定範囲外の場合</exception>
        public void SetReturnVariableIndex(int commonVariableIndex)
        {
            if (commonVariableIndex == ReturnValueForNotReturn)
            {
                SetReturnValueNone();
                return;
            }

            if (commonVariableIndex < ReturnVariableIndexMin || ReturnVariableIndexMax < commonVariableIndex)
            {
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(commonVariableIndex), ReturnValueForNotReturn,
                        ReturnVariableIndexMax, commonVariableIndex));
            }

            ReturnVariableIndex = commonVariableIndex;
            IsReturnValue = true;
        }

        /// <summary>
        /// 返戻フラグをOffにする。
        /// </summary>
        public void SetReturnValueNone()
        {
            ReturnVariableIndex = ReturnValueForNotReturn;
            IsReturnValue = false;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Common
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// バイナリ変換する。
        /// </summary>
        /// <returns>バイナリデータ</returns>
        public byte[] ToBinary()
        {
            var result = new List<byte>();

            // 返戻値の意味
            var descWoditorString = new WoditorString(description);
            result.AddRange(descWoditorString.StringByte);

            // 返戻アドレス
            result.AddRange(ReturnVariableIndex.ToBytes(Endian.Woditor));

            return result.ToArray();
        }
    }
}