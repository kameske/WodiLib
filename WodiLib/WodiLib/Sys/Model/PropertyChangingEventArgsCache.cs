// ========================================
// Project Name : WodiLib
// File Name    : PropertyChangingEventArgsCache.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.ComponentModel;

namespace WodiLib.Sys
{
    /// <summary>
    /// <see cref="PropertyChangingEventArgs"/> のキャッシュクラス
    /// </summary>
    internal class PropertyChangingEventArgsCache : CacheBase<PropertyChangingEventArgs>
    {
        /// <inheritdoc />
        protected override PropertyChangingEventArgs MakeNewInstance(string key)
            => new(key);

        /// <summary>シングルトンインスタンス</summary>
        private static PropertyChangingEventArgsCache Instance { get; } = new();

        /// <summary>
        /// インスタンスを取得する。
        /// </summary>
        /// <remarks>
        /// キャッシュが存在する場合はキャッシュから取得する。<br/>
        /// キャッシュが存在しない場合は新規作成したインスタンスを返し、同時にキャッシュとして保持する。
        /// </remarks>
        /// <param name="propertyName">プロパティ名</param>
        /// <returns>キャッシュインスタンス</returns>
        public static PropertyChangingEventArgs GetInstance(string propertyName)
            => Instance[propertyName];
    }
}
