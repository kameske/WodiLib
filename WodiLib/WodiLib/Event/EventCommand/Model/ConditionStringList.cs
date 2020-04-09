// ========================================
// Project Name : WodiLib
// File Name    : ConditionStringList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// 条件（文字列）条件リスト
    /// </summary>
    [Serializable]
    public class ConditionStringList : FixedLengthList<ConditionStringDesc>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 容量
        /// </summary>
        public static readonly int Capacity = 15;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private int conditionValue;

        /// <summary>[Range(1, 15)] 条件数</summary>
        /// <exception cref="PropertyOutOfRangeException">1～4以外の値を設定しようとした場合</exception>
        public int ConditionValue
        {
            get => conditionValue;
            set
            {
                if (value < 1 || Capacity < value)
                    throw new PropertyOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(ConditionValue), 1, 4, value));
                conditionValue = value;
                NotifyPropertyChanged();
            }
        }


        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal ConditionStringList()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="items">[NotNull] 初期リスト</param>
        /// <exception cref="TypeInitializationException">派生クラスの設定値が不正な場合</exception>
        /// <exception cref="ArgumentNullException">
        ///     itemsがnullの場合、
        ///     またはitems中にnullが含まれる場合
        /// </exception>
        /// <exception cref="InvalidOperationException">itemsの要素数が不適切な場合</exception>
        public ConditionStringList(IEnumerable<ConditionStringDesc> items) : base(items)
        {
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>
        /// 容量値を返す。
        /// </summary>
        /// <returns>容量最大値</returns>
        public override int GetCapacity() => Capacity;

        /// <inheritdoc />
        public override IEnumerator<ConditionStringDesc> GetEnumerator()
        {
            for (var i = 0; i < ConditionValue; i++)
            {
                yield return Items[i];
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 右辺に数値変数を設定している条件を探し、その最大のインデックスを返す。
        /// </summary>
        /// <returns>右辺に数値変数を設定している条件のインデックス最大値（0～4）</returns>
        public int SearchUseNumberVariableForRightSideMax()
        {
            for (var i = ConditionValue - 1; i >= 0; i--)
            {
                if (this[i].IsUseNumberVariable) return i + 1;
            }

            return 0;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>
        /// 格納対象のデフォルトインスタンスを生成する。
        /// </summary>
        /// <param name="index">挿入インデックス</param>
        /// <returns>デフォルトインスタンス</returns>
        protected override ConditionStringDesc MakeDefaultItem(int index)
            => new ConditionStringDesc();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Serializable
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected ConditionStringList(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}