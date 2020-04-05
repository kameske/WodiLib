// ========================================
// Project Name : WodiLib
// File Name    : MapEventPageMoveRouteInfo.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using WodiLib.Event;
using WodiLib.Event.CharaMoveCommand;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.Map
{
    /// <summary>
    /// マップイベント移動ルート情報クラス
    /// </summary>
    [Serializable]
    public class MapEventPageMoveRouteInfo : ModelBase<MapEventPageMoveRouteInfo>, ISerializable
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        [field: NonSerialized] private WodiLibLogger Logger { get; } = WodiLibLogger.GetInstance();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private AnimateSpeed animateSpeed = AnimateSpeed.Middle;

        /// <summary>[NotNull] アニメ速度</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        /// <exception cref="ArgumentException">マップイベントでは設定できない値を指定した場合</exception>
        public AnimateSpeed AnimateSpeed
        {
            get => animateSpeed;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(AnimateSpeed)));

                if (!value.CanSetForMapEventMoveRoute)
                    Logger.Warning(WarningMessage.CannotSetValue(nameof(AnimateSpeed), value));

                animateSpeed = value;
                NotifyPropertyChanged();
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
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(MoveSpeed)));

                if (!value.CanSetForMapEventMoveRoute)
                    Logger.Warning(WarningMessage.CannotSetValue(nameof(AnimateSpeed), value));

                moveSpeed = value;
                NotifyPropertyChanged();
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
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(MoveFrequency)));
                moveFrequency = value;
                NotifyPropertyChanged();
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
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(MoveType)));
                moveType = value;
                NotifyPropertyChanged();

                if (moveType == MoveType.Custom && customMoveRoute is null)
                {
                    // 移動ルート＝カスタムのとき、移動ルート必須のためインスタンスをセット
                    customMoveRoute = new ActionEntry();
                    NotifyPropertyChanged(nameof(CustomMoveRoute));
                }
            }
        }

        private ActionEntry customMoveRoute = new ActionEntry
        {
            Owner = TargetAddressOwner.MapEvent
        };

        /// <summary><para>[CanBeNull] カスタム移動ルート</para>
        /// <para>移動ルート＝カスタムの場合、必須</para></summary>
        /// <exception cref="PropertyNullException">移動ルート＝カスタムのときにnullをセットした場合</exception>
        public ActionEntry CustomMoveRoute
        {
            get => customMoveRoute;
            set
            {
                if (value is null && MoveType == MoveType.Custom)
                    throw new ArgumentNullException(
                        $"移動ルートがカスタムの場合、{ErrorMessage.NotNull(nameof(CustomMoveRoute))}");

                // nullをセットした場合、内部的には ActionEntryの初期値を扱う
                customMoveRoute = value ?? new ActionEntry();

                customMoveRoute.Owner = TargetAddressOwner.MapEvent;
                NotifyPropertyChanged();
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MapEventPageMoveRouteInfo()
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
        public override bool Equals(MapEventPageMoveRouteInfo other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return animateSpeed.Equals(other.animateSpeed)
                   && moveSpeed.Equals(other.moveSpeed)
                   && moveFrequency.Equals(other.moveFrequency)
                   && moveType.Equals(other.moveType)
                   && customMoveRoute.Equals(other.customMoveRoute);
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
            if (!(CustomMoveRoute is null))
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
            var hasCustomRoute = !(CustomMoveRoute is null);
            if (isTypeCustom && !hasCustomRoute)
                throw new InvalidOperationException($"移動ルートが「カスタム」の場合、{nameof(CustomMoveRoute)}は必須です。");
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
            info.AddValue(nameof(animateSpeed), animateSpeed.Code);
            info.AddValue(nameof(moveSpeed), moveSpeed.Code);
            info.AddValue(nameof(moveFrequency), moveFrequency.Code);
            info.AddValue(nameof(moveType), moveType.Code);
            info.AddValue(nameof(customMoveRoute), customMoveRoute);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected MapEventPageMoveRouteInfo(SerializationInfo info, StreamingContext context)
        {
            animateSpeed = AnimateSpeed.FromByte(info.GetByte(nameof(animateSpeed)));
            moveSpeed = MoveSpeed.FromByte(info.GetByte(nameof(moveSpeed)));
            moveFrequency = MoveFrequency.FromByte(info.GetByte(nameof(moveFrequency)));
            moveType = MoveType.FromByte(info.GetByte(nameof(moveType)));
            customMoveRoute = info.GetValue<ActionEntry>(nameof(customMoveRoute));
        }
    }
}