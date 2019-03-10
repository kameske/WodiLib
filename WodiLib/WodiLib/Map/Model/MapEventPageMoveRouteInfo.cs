// ========================================
// Project Name : WodiLib
// File Name    : MapEventPageMoveRouteInfo.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using WodiLib.Event;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    /// マップイベント移動ルート情報クラス
    /// </summary>
    public class MapEventPageMoveRouteInfo
    {
        private AnimateSpeed animateSpeed = AnimateSpeed.Middle;

        /// <summary>[NotNull] アニメ速度</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public AnimateSpeed AnimateSpeed
        {
            get => animateSpeed;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(AnimateSpeed)));
                animateSpeed = value;
            }
        }

        private MoveSpeed moveSpeed = MoveSpeed.Normal;

        /// <summary>[NotNull] 移動速度</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public MoveSpeed MoveSpeed
        {
            get => moveSpeed;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(MoveSpeed)));
                moveSpeed = value;
            }
        }

        private MoveFrequency moveFrequency = MoveFrequency.Middle;

        /// <summary>[NotNull] 移動頻度</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public MoveFrequency MoveFrequency
        {
            get => moveFrequency;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(MoveFrequency)));
                moveFrequency = value;
            }
        }

        private MoveType moveType = MoveType.Not;

        /// <summary>[NotNull] 移動ルート種別</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public MoveType MoveType
        {
            get => moveType;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(MoveType)));
                moveType = value;
                if (moveType == MoveType.Custom && customMoveRoute == null)
                {
                    // 移動ルート＝カスタムのとき、移動ルート必須のためからインスタンスをセット
                    customMoveRoute = new ActionEntry();
                }
            }
        }

        private ActionEntry customMoveRoute = new ActionEntry();

        /// <summary><para>[CanBeNull] カスタム移動ルート</para>
        /// <para>移動ルート＝カスタムの場合、必須</para></summary>
        /// <exception cref="PropertyNullException">移動ルート＝カスタムのときにnullをセットした場合</exception>
        public ActionEntry CustomMoveRoute
        {
            get => customMoveRoute;
            set
            {
                if (value == null && MoveType == MoveType.Custom)
                    throw new ArgumentNullException(
                        $"移動ルートがカスタムの場合、{ErrorMessage.NotNull(nameof(CustomMoveRoute))}");

                // nullをセットした場合、内部的には ActionEntryの初期値を扱う
                customMoveRoute = value ?? new ActionEntry();
            }
        }

        /// <summary>
        /// バイナリ変換する。
        /// </summary>
        /// <returns>バイナリデータ</returns>
        public byte[] ToBinary()
        {
            var result = new List<byte>();

            result.Add(AnimateSpeed.Code);
            // 移動速度
            result.Add(MoveSpeed.Code);
            // 移動頻度
            result.Add(MoveFrequency.Code);
            // 移動ルート
            result.Add(MoveType.Code);

            return result.ToArray();
        }

        /// <summary>
        /// カスタム移動ルートをバイナリデータに変換する。
        /// </summary>
        /// <returns>バイナリデータ</returns>
        public IEnumerable<byte> ToCustomMoveRouteBinary()
        {
            var result = new List<byte>();

            // カスタム移動ルートフラグ
            result.Add(customMoveRoute.MakeOptionByte());
            // 動作指定コマンド数
            result.AddRange(customMoveRoute.CommandList.Count.ToWoditorIntBytes());
            // 動作指定コマンド
            if (CustomMoveRoute != null)
            {
                result.AddRange(CustomMoveRoute.MakeMoveRouteByte());
            }

            return result;
        }

        /// <summary>
        /// 移動ルートタイプのバリデーションを行う。
        /// </summary>
        /// <exception cref="InvalidOperationException">移動ルートがカスタムかつCustomMoveRoute=nullの場合</exception>
        public void ValidateMoveType()
        {
            var isTypeCustom = MoveType == MoveType.Custom;
            var hasCustomRoute = CustomMoveRoute != null;
            if (isTypeCustom && !hasCustomRoute)
                throw new InvalidOperationException($"移動ルートが「カスタム」の場合、{nameof(CustomMoveRoute)}は必須です。");
        }
    }
}