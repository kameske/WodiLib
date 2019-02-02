// ========================================
// Project Name : WodiLib
// File Name    : EmptyDisposable.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys.Observer
{
    /// <inheritdoc />
    /// <summary>
    /// WodiLib用EmptyDisposable
    /// </summary>
    internal class EmptyDisposable : IDisposable
    {
        public static IDisposable Instance => new EmptyDisposable();

        /// <inheritdoc />
        public void Dispose()
        {
            // nothing to do
        }
    }
}