// ========================================
// Project Name : WodiLib
// File Name    : PropertyChangedEventArgsCache.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.ComponentModel;

namespace WodiLib.Sys
{
    /// <summary>
    ///     <see cref="PropertyChangedEventArgs"/> のキャッシュクラス
    /// </summary>
    internal class PropertyChangedEventArgsCache : CacheBase<PropertyChangedEventArgs>
    {
        /// <inheritdoc/>
        protected override PropertyChangedEventArgs MakeNewInstance(string key)
            => new(key);

        /// <summary>シングルトンインスタンス</summary>
        private static PropertyChangedEventArgsCache Instance { get; } = new();

        /// <summary>
        ///     インスタンスを取得する。
        /// </summary>
        /// <remarks>
        ///     キャッシュが存在する場合はキャッシュから取得する。<br/>
        ///     キャッシュが存在しない場合は新規作成したインスタンスを返し、同時にキャッシュとして保持する。
        /// </remarks>
        /// <param name="propertyName">プロパティ名</param>
        /// <returns>キャッシュインスタンス</returns>
        public static PropertyChangedEventArgs GetInstance(string propertyName)
            => Instance[propertyName];
    }
}
