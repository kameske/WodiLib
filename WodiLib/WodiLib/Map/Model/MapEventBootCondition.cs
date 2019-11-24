// ========================================
// Project Name : WodiLib
// File Name    : MapEventBootCondition.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using WodiLib.Cmn;
using WodiLib.Event;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    ///     マップイベント起動条件実装クラス
    /// </summary>
    [Serializable]
    public class MapEventBootCondition : IEquatable<MapEventBootCondition>, ISerializable
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>条件設定ONフラグ</summary>
        private static byte FlgHasCondition => 0x01;

        private static int DefaultValue => 1000000;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     左辺
        /// </summary>
        public int LeftSide { get; set; } = DefaultValue;

        private CriteriaOperator operation = CriteriaOperator.Equal;

        /// <summary>[NotNull] 条件演算子</summary>
        public CriteriaOperator Operation
        {
            get => operation;
            set => operation = value ?? throw new PropertyNullException(ErrorMessage.NotNull(nameof(Operation)));
        }

        /// <summary>条件使用フラグ</summary>
        public bool UseCondition { get; set; }

        /// <summary>右辺</summary>
        public ConditionRight RightSide { get; set; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MapEventBootCondition()
        {
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     条件演算子＆使用フラグ用のbyte生成
        /// </summary>
        /// <returns>バイトデータ</returns>
        public byte MakeEventBootConditionByte()
        {
            byte result = 0x00;
            result += Operation.Code;
            result += UseCondition ? FlgHasCondition : (byte) 0x00;
            return result;
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(MapEventBootCondition other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return UseCondition == other.UseCondition
                   && LeftSide == other.LeftSide
                   && RightSide.Equals(other.RightSide)
                   && Equals(operation, other.operation);
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
            info.AddValue(nameof(operation), operation.Code);
            info.AddValue(nameof(UseCondition), UseCondition);
            info.AddValue(nameof(RightSide), RightSide);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected MapEventBootCondition(SerializationInfo info, StreamingContext context)
        {
            LeftSide = info.GetInt32(nameof(LeftSide));
            operation = CriteriaOperator.FromByte(info.GetByte(nameof(operation)));
            UseCondition = info.GetBoolean(nameof(UseCondition));
            RightSide = info.GetInt32(nameof(RightSide));
        }
    }
}