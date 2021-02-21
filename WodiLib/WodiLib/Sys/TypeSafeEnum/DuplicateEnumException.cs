// ========================================
// Project Name : WodiLib
// File Name    : DuplicationEnumException.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys
{
    /// <summary>
    ///     <see cref="TypeSafeEnum{T}"/>の要素が重複して登録されようとした場合に発生する例外
    /// </summary>
    public class DuplicateEnumException : Exception
    {
    }
}
