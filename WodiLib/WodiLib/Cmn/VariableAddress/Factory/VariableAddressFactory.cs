// ========================================
// Project Name : WodiLib
// File Name    : VariableAddressFactory.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Cmn
{
    /// <summary>
    /// 変数アドレス値Factory
    /// </summary>
    public static class VariableAddressFactory
    {
        /// <summary>
        /// int値からVariableAddressインスタンスを生成する
        /// </summary>
        /// <param name="value">対象値</param>
        /// <returns>VariableAddressのインスタンス</returns>
        /// <exception cref="ArgumentOutOfRangeException">valueが変数アドレス値として適切でない場合</exception>
        public static VariableAddress Create(int value)
        {
            if (ChangeableDatabaseAddress.MinValue <= value &&
                value <= ChangeableDatabaseAddress.MaxValue)
                return new ChangeableDatabaseAddress(value);

            if (CommonEventVariableAddress.MinValue <= value &&
                value <= CommonEventVariableAddress.MaxValue)
                return new CommonEventVariableAddress(value);

            if (EventInfoAddress.MinValue <= value &&
                value <= EventInfoAddress.MaxValue)
                return new EventInfoAddress(value);

            if (HeroInfoAddress.MinValue <= value &&
                value <= HeroInfoAddress.MaxValue)
                return new HeroInfoAddress(value);

            if (ThisMapEventInfoAddress.MinValue <= value &&
                value <= ThisMapEventInfoAddress.MaxValue)
                return new ThisMapEventInfoAddress(value);

            if (MapEventVariableAddress.MinValue <= value &&
                value <= MapEventVariableAddress.MaxValue)
                return new MapEventVariableAddress(value);

            if (MemberInfoAddress.MinValue <= value &&
                value <= MemberInfoAddress.MaxValue)
                return new MemberInfoAddress(value);

            if (NormalNumberVariableAddress.MinValue <= value &&
                value <= NormalNumberVariableAddress.MaxValue)
                return new NormalNumberVariableAddress(value);

            if (RandomVariableAddress.MinValue <= value &&
                value <= RandomVariableAddress.MaxValue)
                return new RandomVariableAddress(value);

            if (SpareNumberVariableAddress.MinValue <= value &&
                value <= SpareNumberVariableAddress.MaxValue)
                return new SpareNumberVariableAddress(value);

            if (StringVariableAddress.MinValue <= value &&
                value <= StringVariableAddress.MaxValue)
                return new StringVariableAddress(value);

            if (SystemDatabaseAddress.MinValue <= value &&
                value <= SystemDatabaseAddress.MaxValue)
                return new SystemDatabaseAddress(value);

            if (SystemVariableAddress.MinValue <= value &&
                value <= SystemVariableAddress.MaxValue)
                return new SystemVariableAddress(value);

            if (SystemStringVariableAddress.MinValue <= value &&
                value <= SystemStringVariableAddress.MaxValue)
                return new SystemStringVariableAddress(value);

            if (ThisCommonEventVariableAddress.MinValue <= value &&
                value <= ThisCommonEventVariableAddress.MaxValue)
                return new ThisCommonEventVariableAddress(value);

            if (ThisMapEventVariableAddress.MinValue <= value &&
                value <= ThisMapEventVariableAddress.MaxValue)
                return new ThisMapEventVariableAddress(value);

            if (UserDatabaseAddress.MinValue <= value &&
                value <= UserDatabaseAddress.MaxValue)
                return new UserDatabaseAddress(value);

            throw new ArgumentOutOfRangeException(
                $"指定された値は変数アドレス値ではありません。（value：{value}）");
        }
    }
}