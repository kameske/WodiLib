// ========================================
// Project Name : WodiLib
// File Name    : CacheBase.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;

namespace WodiLib.Sys
{
    /// <summary>
    /// キャッシュ実装用基底クラス
    /// </summary>
    /// <typeparam name="T">キャッシュ対象型</typeparam>
    internal abstract class CacheBase<T>
        where T : class
    {
        /// <summary>キャッシュ保管庫</summary>
        private Dictionary<string, T> Cache { get; }
            = new();

        /// <summary>
        /// インスタンスを取得する。
        /// </summary>
        /// <remarks>
        /// キャッシュが存在する場合はキャッシュから取得する。<br/>
        /// キャッシュが存在しない場合は新規作成したインスタンスを返し、同時にキャッシュとして保持する。
        /// </remarks>
        /// <param name="key">キャッシュキー名</param>
        /// <returns>キャッシュインスタンス</returns>
        public T this[string key]
        {
            get
            {
                if (!Cache.ContainsKey(key))
                {
                    Cache.Add(key, MakeNewInstance(key));
                }

                return Cache[key];
            }
        }

        /// <summary>
        /// 新規インスタンスを作成する。
        /// </summary>
        /// <remarks>
        /// 指定した <paramref name="key"/> でキャッシュが作成されていない場合に実行される。
        /// </remarks>
        /// <param name="key">キャッシュキー名</param>
        /// <returns>新規作成したインスタンス</returns>
        protected abstract T MakeNewInstance(string key);
    }
}
