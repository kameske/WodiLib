using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WodiLibSample.Extension
{
    /// <summary>
    /// Enumerable 拡張クラス
    /// </summary>
    internal static class Enumerable
    {
        /// <summary>
        /// 副作用のない処理を実行する。
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="src">対象</param>
        /// <param name="action">実行する処理</param>
        /// <returns>受け取ったEnumerable</returns>
        public static IEnumerable<T> Do<T>(this IEnumerable<T> src, Action<T> action)
        {
            foreach (var item in src)
            {
                action(item);
                yield return item;
            }
        }
    }
}
