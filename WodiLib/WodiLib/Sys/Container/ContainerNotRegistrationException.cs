// ========================================
// Project Name : WodiLib
// File Name    : ContainerNotRegistrationException.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys
{
    /// <inheritdoc />
    /// <summary>
    ///     Containerに登録されていないインスタンスを呼び出そうとしたときの例外
    /// </summary>
    public class ContainerNotRegistrationException : Exception
    {
    }
}