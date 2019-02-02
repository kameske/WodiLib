// ========================================
// Project Name : WodiLib
// File Name    : DuplicateEnumException.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;

namespace WodiLib.Sys
{
    /// <summary>
    /// TypeSafeEnumの要素が重複して登録されようとした場合に発生する例外
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class DuplicateEnumException : Exception
    {
    }
}