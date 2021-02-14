// ========================================
// Project Name : WodiLib
// File Name    : TargetAddressOwner.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Linq;
using WodiLib.Cmn;
using WodiLib.Sys;

namespace WodiLib.Event.CharaMoveCommand
{
    /// <summary>
    /// キャラ動作指定コマンド保有クラス種別
    /// </summary>
    internal record TargetAddressOwner : TypeSafeEnum<TargetAddressOwner>
    {
        /// <summary>マップイベント</summary>
        internal static readonly TargetAddressOwner MapEvent;

        /// <summary>コモンイベント</summary>
        internal static readonly TargetAddressOwner CommonEvent;

        static TargetAddressOwner()
        {
            MapEvent = new TargetAddressOwner(nameof(MapEvent));
            CommonEvent = new TargetAddressOwner(nameof(CommonEvent));
        }

        private TargetAddressOwner(string id) : base(id)
        {
        }

        /// <summary>
        /// キャラ動作指定コマンド内のセルフ変数を適切な値に変換する。
        /// </summary>
        /// <param name="target">対象</param>
        /// <returns>変換後のアドレス値</returns>
        /// <exception cref="ArgumentNullException">target が null の場合</exception>
        /// <exception cref="InvalidOperationException">サポート外の場合</exception>
        internal VariableAddress ConvertVariableAddress(CalledEventVariableAddress target)
        {
            if (target is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(target)));

            var index = ((int) target).SubInt(0, 1);
            if (this == MapEvent)
            {
                return new ThisMapEventVariableAddress(ThisMapEventVariableAddress.MinValue + index);
            }

            if (this == CommonEvent)
            {
                return new ThisCommonEventVariableAddress(ThisCommonEventVariableAddress.MinValue + index);
            }

            // 通常ここには来ない
            throw new InvalidOperationException();
        }

        internal static TargetAddressOwner FromId(string id)
        {
            return AllItems.First(x => x.Id.Equals(id));
        }

        /// <inheritdoc />
        public override string ToString()
            => base.ToString();
    }
}
