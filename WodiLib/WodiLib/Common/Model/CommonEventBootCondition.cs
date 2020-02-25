// ========================================
// Project Name : WodiLib
// File Name    : CommonEventBootCondition.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using WodiLib.Cmn;
using WodiLib.Event;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.Common
{
    /// <summary>
    /// コモンイベント起動条件実装クラス
    /// </summary>
    [Serializable]
    public class CommonEventBootCondition : ISerializable, IEquatable<CommonEventBootCondition>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private CommonEventBootType commonEventBootType = CommonEventBootType.OnlyCall;

        /// <summary>[NotNull] イベント起動タイプ</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public CommonEventBootType CommonEventBootType
        {
            get => commonEventBootType;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(CommonEventBootType)));
                commonEventBootType = value;
            }
        }

        private int leftSide = (NormalNumberVariableAddress) 2000000;

        /// <summary>
        ///     [SafetyConvertible(NormalNumberVariableAddress)]
        ///     [SafetyConvertible(SpareNumberVariableAddress)]
        ///     左辺
        /// </summary>
        public int LeftSide
        {
            get => leftSide;
            set
            {
                if (!(NormalNumberVariableAddress.MinValue <= value && value <= NormalNumberVariableAddress.MaxValue)
                    && !(SpareNumberVariableAddress.MinValue <= value && value <= SpareNumberVariableAddress.MaxValue))
                {
                    WodiLibLogger.GetInstance()
                        .Warning($"[Warning]イベントコマンドの起動条件が意図しない値です。（設定値：{value}）");
                }

                leftSide = value;
            }
        }

        private CriteriaOperator operation = CriteriaOperator.Equal;

        /// <summary>[NotNull] 条件演算子</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public CriteriaOperator Operation
        {
            get => operation;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Operation)));
                operation = value;
            }
        }

        /// <summary>右辺</summary>
        public ConditionRight RightSide { get; set; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CommonEventBootCondition()
        {
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(CommonEventBootCondition other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return commonEventBootType.Equals(other.commonEventBootType)
                   && leftSide == other.leftSide
                   && operation.Equals(other.operation)
                   && RightSide.Equals(other.RightSide);
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

            // 比較演算子 & 起動条件
            result.Add((byte) (Operation.Code + CommonEventBootType.Code));

            // 左辺
            result.AddRange(LeftSide.ToBytes(Endian.Woditor));

            // 右辺
            result.AddRange(RightSide.ToBytes(Endian.Woditor));

            return result.ToArray();
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
            info.AddValue(nameof(commonEventBootType), commonEventBootType.Code);
            info.AddValue(nameof(leftSide), leftSide);
            info.AddValue(nameof(operation), operation.Code);
            info.AddValue(nameof(RightSide), RightSide);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected CommonEventBootCondition(SerializationInfo info, StreamingContext context)
        {
            commonEventBootType = CommonEventBootType.FromByte(info.GetByte(nameof(commonEventBootType)));
            leftSide = info.GetInt32(nameof(leftSide));
            operation = CriteriaOperator.FromByte(info.GetByte(nameof(operation)));
            RightSide = info.GetInt32(nameof(RightSide));
        }
    }
}