// ========================================
// Project Name : WodiLib
// File Name    : ConditionNumberDesc.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// 条件（変数）条件オブジェクトクラス
    /// </summary>
    [Serializable]
    public class ConditionNumberDesc : IEquatable<ConditionNumberDesc>, ISerializable
    {
        /// <summary>左辺</summary>
        public int LeftSide { get; set; }

        /// <summary>右辺</summary>
        public int RightSide { get; set; }

        private NumberConditionalOperator condition = NumberConditionalOperator.Equal;

        /// <summary>[NotNull] 条件演算子</summary>
        /// <exception cref="PropertyNullException">nullをセットしようとした場合</exception>
        public NumberConditionalOperator Condition
        {
            get => condition;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Condition)));
                condition = value;
            }
        }

        /// <summary>条件右辺）データを呼ばない</summary>
        public bool IsNotReferX { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ConditionNumberDesc() {}

        /// <summary>
        /// 右辺データ呼ばないフラグ＆比較演算子のバイトを返す。
        /// </summary>
        /// <returns>バイナリデータ</returns>
        public byte ToConditionFlag()
        {
            byte referXFlag = (byte) (IsNotReferX ? 0x10 : 0x00);
            return (byte) (referXFlag + Condition.Code);
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(ConditionNumberDesc other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return LeftSide == other.LeftSide
                   && RightSide == other.RightSide
                   && IsNotReferX == other.IsNotReferX
                   && condition == other.condition;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Serializable
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// オブジェクトをシリアル化するために必要なデータを設定する。
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(LeftSide), LeftSide);
            info.AddValue(nameof(RightSide), RightSide);
            info.AddValue(nameof(condition), condition.Code);
            info.AddValue(nameof(IsNotReferX), IsNotReferX);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected ConditionNumberDesc(SerializationInfo info, StreamingContext context)
        {
            LeftSide = info.GetInt32(nameof(LeftSide));
            RightSide = info.GetInt32(nameof(RightSide));
            condition = NumberConditionalOperator.FromByte(info.GetByte(nameof(condition)));
            IsNotReferX = info.GetBoolean(nameof(IsNotReferX));
        }
    }
}