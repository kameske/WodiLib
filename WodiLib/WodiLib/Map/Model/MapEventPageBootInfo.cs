// ========================================
// Project Name : WodiLib
// File Name    : MapEventPageBootInfo.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    /// マップイベントページ起動情報クラス
    /// </summary>
    public class MapEventPageBootInfo : IWodiLibObject
    {
        private EventBootType eventBootType = EventBootType.PushOKKey;

        /// <summary>イベント起動タイプ</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public EventBootType EventBootType
        {
            get => eventBootType;
            set
            {
                if (value == null) throw new PropertyNullException(ErrorMessage.NotNull(nameof(EventBootType)));
                eventBootType = value;
            }
        }

        private EventBootCondition eventBootCondition1 = new EventBootCondition();
        private EventBootCondition eventBootCondition2 = new EventBootCondition();
        private EventBootCondition eventBootCondition3 = new EventBootCondition();
        private EventBootCondition eventBootCondition4 = new EventBootCondition();

        /// <summary>起動条件1設定フラグ</summary>
        public bool HasEventBootCondition1
        {
            get => eventBootCondition1.UseCondition;
            set => eventBootCondition1.UseCondition = value;
        }

        /// <summary>起動条件2設定フラグ</summary>
        public bool HasEventBootCondition2
        {
            get => eventBootCondition2.UseCondition;
            set => eventBootCondition2.UseCondition = value;
        }

        /// <summary>起動条件3設定フラグ</summary>
        public bool HasEventBootCondition3
        {
            get => eventBootCondition3.UseCondition;
            set => eventBootCondition3.UseCondition = value;
        }

        /// <summary>起動条件4設定フラグ</summary>
        public bool HasEventBootCondition4
        {
            get => eventBootCondition4.UseCondition;
            set => eventBootCondition4.UseCondition = value;
        }

        /// <summary>
        /// 起動条件設定フラグを設定する。
        /// </summary>
        /// <param name="index">[Range(0, 3)] 条件インデックス</param>
        /// <param name="flag">条件設定フラグ</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが 0～3 以外</exception>
        public void SetHasEventBootCondition(int index, bool flag)
        {
            switch (index)
            {
                case 0:
                    HasEventBootCondition1 = flag;
                    return;
                case 1:
                    HasEventBootCondition2 = flag;
                    return;
                case 2:
                    HasEventBootCondition3 = flag;
                    return;
                case 3:
                    HasEventBootCondition4 = flag;
                    return;
                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 0, 3, index));
            }
        }

        /// <summary>[NotNull] イベント起動条件1</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public EventBootCondition EventBootCondition1
        {
            get => eventBootCondition1;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(EventBootCondition1)));
                eventBootCondition1 = value;
            }
        }

        /// <summary>[NotNull] イベント起動条件2</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public EventBootCondition EventBootCondition2
        {
            get => eventBootCondition2;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(EventBootCondition2)));
                eventBootCondition2 = value;
            }
        }

        /// <summary>[NotNull] イベント起動条件3</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public EventBootCondition EventBootCondition3
        {
            get => eventBootCondition3;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(EventBootCondition3)));
                eventBootCondition3 = value;
            }
        }

        /// <summary>[NotNull] イベント起動条件4</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public EventBootCondition EventBootCondition4
        {
            get => eventBootCondition4;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(EventBootCondition4)));
                eventBootCondition4 = value;
            }
        }

        /// <summary>
        /// 起動条件を設定する。
        /// </summary>
        /// <param name="index">[Range(0, 3)] 条件インデックス</param>
        /// <param name="condition">[NotNull] 条件</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが 0～3 以外</exception>
        /// <exception cref="ArgumentNullException">conditionがnull</exception>
        public void SetEventBootCondition(int index, EventBootCondition condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(condition)));
            }

            switch (index)
            {
                case 0:
                    EventBootCondition1 = condition;
                    return;
                case 1:
                    EventBootCondition2 = condition;
                    return;
                case 2:
                    EventBootCondition3 = condition;
                    return;
                case 3:
                    EventBootCondition4 = condition;
                    return;
                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 0, 3, index));
            }
        }

        /// <inheritdoc />
        public byte[] ToBinary()
        {
            var result = new List<byte>();

            // 起動条件
            result.Add(EventBootType.Code);
            // 条件1演算子 & 使用フラグ
            result.Add(EventBootCondition1.MakeEventBootConditionByte());
            // 条件2演算子 & 使用フラグ
            result.Add(EventBootCondition2.MakeEventBootConditionByte());
            // 条件3演算子 & 使用フラグ
            result.Add(EventBootCondition3.MakeEventBootConditionByte());
            // 条件4演算子 & 使用フラグ
            result.Add(EventBootCondition4.MakeEventBootConditionByte());
            // 条件1左辺
            result.AddRange(EventBootCondition1.LeftSide.ToWoditorIntBytes());
            // 条件2左辺
            result.AddRange(EventBootCondition2.LeftSide.ToWoditorIntBytes());
            // 条件3左辺
            result.AddRange(EventBootCondition3.LeftSide.ToWoditorIntBytes());
            // 条件4左辺
            result.AddRange(EventBootCondition4.LeftSide.ToWoditorIntBytes());
            // 条件1右辺
            result.AddRange(EventBootCondition1.RightSide.ToWoditorIntBytes());
            // 条件2右辺
            result.AddRange(EventBootCondition2.RightSide.ToWoditorIntBytes());
            // 条件3右辺
            result.AddRange(EventBootCondition3.RightSide.ToWoditorIntBytes());
            // 条件4右辺
            result.AddRange(EventBootCondition4.RightSide.ToWoditorIntBytes());

            return result.ToArray();
        }
    }
}