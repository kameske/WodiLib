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
    [Serializable]
    internal class CommonEventReturnValue : ModelBase<CommonEventReturnValue>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private CommonEventResultDescription description = "";

        /// <summary>
        /// [NotNull] 返戻値の意味
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public CommonEventResultDescription Description
        {
            get => description;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Description)));

                description = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// 値を返すフラグ
        /// </summary>
        public bool IsReturnValue { get; private set; }

        /// <summary>
        /// 返戻セルフ変数インデックス（値を返さない場合-1）
        /// </summary>
        public CommonEventReturnVariableIndex ReturnVariableIndex { get; private set; } =
            CommonEventReturnVariableIndex.NotReturn;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 返戻コモンセルフ変数をセットする。
        /// </summary>
        /// <param name="commonVariableIndex">返戻アドレス</param>
        public void SetReturnVariableIndex(CommonEventReturnVariableIndex commonVariableIndex)
        {
            if (commonVariableIndex == CommonEventReturnVariableIndex.NotReturn)
            {
                SetReturnValueNone();
                return;
            }

            ReturnVariableIndex = commonVariableIndex;
            IsReturnValue = true;
            NotifyPropertyChanged(nameof(ReturnVariableIndex));
            NotifyPropertyChanged(nameof(IsReturnValue));
        }

        /// <summary>
        /// 返戻フラグをOffにする。
        /// </summary>
        public void SetReturnValueNone()
        {
            ReturnVariableIndex = CommonEventReturnVariableIndex.NotReturn;
            IsReturnValue = false;
            NotifyPropertyChanged(nameof(ReturnVariableIndex));
            NotifyPropertyChanged(nameof(IsReturnValue));
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public override bool Equals(CommonEventReturnValue other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return description.Equals(other.description)
                   && IsReturnValue == other.IsReturnValue
                   && ReturnVariableIndex.Equals(other.ReturnVariableIndex);
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
            result.AddRange(Description.ToWoditorStringBytes());

            // 返戻アドレス
            result.AddRange(ReturnVariableIndex.ToBytes(Endian.Woditor));

            return result.ToArray();
        }
    }
}