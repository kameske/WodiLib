// ========================================
// Project Name : WodiLib
// File Name    : IntExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Diagnostics.CodeAnalysis;

namespace WodiLib.Cmn
{
    /// <summary>
    /// int拡張クラス
    /// </summary>
    public static class IntExtension
    {
        /// <summary>
        /// この数値が変数アドレス値として適切か厳密に判定する。
        /// </summary>
        /// <remarks>
        ///     簡易チェックを行いたい場合は、int.IsVariableAddressSimpleCheck() を使用する。
        ///     例えば、 9180048 は int.IsVariableAddress() では false となるが、
        ///     int.IsVariableAddressSimpleCheck() では true となる。
        /// </remarks>
        /// <param name="src">対象</param>
        /// <returns>変数アドレス値として適切な場合true</returns>
        public static bool IsVariableAddress(this int src)
        {
            if (ChangeableDatabaseAddress.MinValue <= src &&
                src <= ChangeableDatabaseAddress.MaxValue) return true;

            if (CommonEventVariableAddress.MinValue <= src &&
                src <= CommonEventVariableAddress.MaxValue) return true;

            if (EventInfoAddress.MinValue <= src &&
                src <= EventInfoAddress.MaxValue) return true;

            if (HeroInfoAddress.MinValue <= src &&
                src <= HeroInfoAddress.MaxValue) return true;

            if (ThisMapEventInfoAddress.MinValue <= src &&
                src <= ThisMapEventInfoAddress.MaxValue) return true;

            if (MapEventVariableAddress.MinValue <= src &&
                src <= MapEventVariableAddress.MaxValue) return true;

            if (MemberInfoAddress.MinValue <= src &&
                src <= MemberInfoAddress.MaxValue) return true;

            if (NormalNumberVariableAddress.MinValue <= src &&
                src <= NormalNumberVariableAddress.MaxValue) return true;

            if (RandomVariableAddress.MinValue <= src &&
                src <= RandomVariableAddress.MaxValue) return true;

            if (SpareNumberVariableAddress.MinValue <= src &&
                src <= SpareNumberVariableAddress.MaxValue) return true;

            if (StringVariableAddress.MinValue <= src &&
                src <= StringVariableAddress.MaxValue) return true;

            if (SystemDatabaseAddress.MinValue <= src &&
                src <= SystemDatabaseAddress.MaxValue) return true;

            if (SystemVariableAddress.MinValue <= src &&
                src <= SystemVariableAddress.MaxValue) return true;

            if (SystemStringVariableAddress.MinValue <= src &&
                src <= SystemStringVariableAddress.MaxValue) return true;

            if (ThisCommonEventVariableAddress.MinValue <= src &&
                src <= ThisCommonEventVariableAddress.MaxValue) return true;

            if (ThisMapEventVariableAddress.MinValue <= src &&
                src <= ThisMapEventVariableAddress.MaxValue) return true;

            if (UserDatabaseAddress.MinValue <= src &&
                src <= UserDatabaseAddress.MaxValue) return true;

            return false;
        }

        /// <summary>
        /// この数値が変数アドレス値として適切か簡易判定する。
        /// </summary>
        /// <remarks>
        ///     厳密なチェックを行いたい場合は、int.IsVariableAddress() を使用する。
        ///     例えば、 9180048 は int.IsVariableAddress() では false となるが、
        ///     int.IsVariableAddressSimpleCheck() では true となる。
        /// </remarks>
        /// <param name="src">対象</param>
        /// <returns>変数アドレス値として適切な場合true</returns>
        public static bool IsVariableAddressSimpleCheck(this int src)
        {
            return VariableAddress.MinValue <= src && src <= VariableAddress.MaxValue;
        }

        /// <summary>
        /// この数値が数値変数アドレス値として適切か簡易判定する。
        /// </summary>
        /// <param name="src">対象</param>
        /// <returns>数値変数アドレス値として適切な場合true</returns>
        public static bool IsNumericVariableAddressSimpleCheck(this int src)
        {
            if (!src.IsVariableAddressSimpleCheck()) return false;
            if (!src.TryToVariableAddress(out var variableAddress)) return false;
            return variableAddress.ValueType.CheckTypeInclude(VariableAddressValueType.Numeric);
        }

        /// <summary>
        /// int値をVariableAddressに変換する。
        /// </summary>
        /// <param name="src">対象</param>
        /// <param name="result">
        ///     変換に成功した場合、変換値。
        ///     変換に失敗した場合、null。
        /// </param>
        /// <returns>変換に成功した場合、true</returns>
        public static bool TryToVariableAddress(this int src, [NotNullWhen(true)] out VariableAddress? result)
        {
            if (!src.IsVariableAddress())
            {
                result = null;
                return false;
            }

            result = VariableAddressFactory.Create(src);
            return true;
        }
    }
}