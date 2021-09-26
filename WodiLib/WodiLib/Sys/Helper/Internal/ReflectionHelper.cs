// ========================================
// Project Name : WodiLib
// File Name    : ReflectionHelper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace WodiLib.Sys
{
    /// <summary>
    ///     リフレクション処理Helperクラス
    /// </summary>
    internal static class ReflectionHelper
    {
        /// <summary>
        ///     呼び出し元メソッドに付与された <see cref="ObsoleteAttribute"/> の <see cref="ObsoleteAttribute.Message"/> を取得する。
        /// </summary>
        /// <param name="caller">呼び出し元クラス</param>
        /// <param name="targetName">対象メソッド名</param>
        /// <returns>メッセージ</returns>
        public static string GetObsoleteMsg(Type caller, [CallerMemberName] string targetName = "")
        {
            var isFailure = true;
            try
            {
                var attr = caller
                    .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                    .Where(m => m.Name.Equals(targetName))
                    .Select(m => m.GetCustomAttributes(false))
                    .SelectMany(objs => objs)
                    .Cast<Attribute>()
                    .FirstOrDefault(a => a is ObsoleteAttribute);

                if (attr is null) return "null";

                var result = ((ObsoleteAttribute)attr).Message;
                isFailure = false;
                return result;
            }
            finally
            {
                if (isFailure) throw new InvalidOperationException();
            }
        }
    }
}
