using System.Linq;

namespace WodiLib.Test.Tools
{
    public static class TwoDimensionalArrayExtension
    {
        /// <summary>
        /// 二次元配列を比較する。
        /// </summary>
        /// <param name="src">判定対象</param>
        /// <param name="other">比較インスタンス</param>
        /// <typeparam name="T">配列内包型</typeparam>
        /// <returns>すべての要素が一致する場合true</returns>
        public static bool Equals<T>(this T[][] src, T[][] other)
        {
            if (src.Length != other.Length) return false;
            if (src.Length == 0) return true;

            var hasNotEqualItem = src.Select((srcLine, i) =>
                    srcLine.SequenceEqual(other[i]))
                .All(b => b);
            return hasNotEqualItem;
        }
    }
}
