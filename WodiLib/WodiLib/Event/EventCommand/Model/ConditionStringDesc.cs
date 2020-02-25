// ========================================
// Project Name : WodiLib
// File Name    : ConditionStringDesc.cs
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
    /// 条件（文字列）条件オブジェクトクラス
    /// </summary>
    [Serializable]
    public class ConditionStringDesc : IEquatable<ConditionStringDesc>, ISerializable
    {
        /// <summary>左辺</summary>
        public int LeftSide { get; set; } = 3000000;

        private IntOrStr rightSide = "";

        /// <summary>[NotNull] 右辺</summary>
        /// <exception cref="PropertyNullException">nullをセットしようとした場合</exception>
        /// <exception cref="PropertyException">IntOrStrType.None をセットしようとした場合</exception>
        public IntOrStr RightSide
        {
            get => rightSide;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(RightSide)));
                if (value.InstanceIntOrStrType == IntOrStrType.None)
                    throw new PropertyException(
                        ErrorMessage.NotEmpty(nameof(RightSide)));
                rightSide = value;
            }
        }

        private StringConditionalOperator condition = StringConditionalOperator.Equal;

        /// <summary>[NotNull] 条件演算子</summary>
        /// <exception cref="PropertyNullException">nullを設定しようとした場合</exception>
        public StringConditionalOperator Condition
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

        /// <summary>数値変数使用フラグ</summary>
        public bool IsUseNumberVariable { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ConditionStringDesc()
        {
        }

        /// <summary>
        /// 右辺の文字列を返す。
        /// </summary>
        /// <returns>右辺文字列。右辺が数値指定の場合、空文字。</returns>
        public string GetRightSideString()
        {
            if (IsUseNumberVariable) return "";
            return RightSide.ToStr();
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(ConditionStringDesc other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return rightSide.Equals(other.rightSide)
                   && condition == other.condition
                   && LeftSide == other.LeftSide
                   && IsUseNumberVariable == other.IsUseNumberVariable;
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
            info.AddValue(nameof(rightSide), rightSide);
            info.AddValue(nameof(condition), condition.Code);
            info.AddValue(nameof(IsUseNumberVariable), IsUseNumberVariable);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected ConditionStringDesc(SerializationInfo info, StreamingContext context)
        {
            LeftSide = info.GetInt32(nameof(LeftSide));
            rightSide = info.GetValue<IntOrStr>(nameof(rightSide));
            condition = StringConditionalOperator.ForByte(info.GetByte(nameof(condition)));
            IsUseNumberVariable = info.GetBoolean(nameof(IsUseNumberVariable));
        }
    }
}