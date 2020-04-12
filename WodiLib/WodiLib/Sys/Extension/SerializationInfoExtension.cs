using System;
using System.Runtime.Serialization;

namespace WodiLib.Sys
{
    /// <summary>
    /// System.Runtime.Serialization.SerializationInfo 拡張クラス
    /// </summary>
    internal static class SerializationInfoExtension
    {
        /// <summary>
        /// Serializationストアから値を返す。
        /// </summary>
        /// <param name="info">自分自身</param>
        /// <param name="name">取得する値に関連付けられた名前</param>
        /// <typeparam name="T">取得するインスタンスの型</typeparam>
        /// <returns>Serializationストア内の値</returns>
        /// <exception cref="ArgumentNullException">name が null の場合</exception>
        /// <exception cref="InvalidCastException">関連付けられた値が T に変換できない場合</exception>
        /// <exception cref="SerializationException">ストア内に name に関連付けられた値が存在しない場合</exception>
        public static T GetValue<T>(this SerializationInfo info, string name)
        {
            var type = typeof(T);
            return (T) info.GetValue(name, type);
        }
    }
}