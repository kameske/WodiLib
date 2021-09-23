// ========================================
// Project Name : WodiLib
// File Name    : PropertyValue.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using WodiLib.SourceGenerator.Core.Extensions;

namespace WodiLib.SourceGenerator.Core.Dtos
{
    internal class PropertyValue
    {
        private PropertyValueProtocol Protocol { get; }

        public PropertyValue(TypedConstant? impl)
        {
            Protocol = new ForTypeConstant(impl);
        }

        public PropertyValue(string value)
        {
            Protocol = new ForString(value);
        }

        /// <summary>
        ///     値を文字列として返却する。
        /// </summary>
        /// <returns>文字列</returns>
        public string? ToValueString()
            => Protocol.ToValueString();

        /// <summary>
        ///     値を文字列の配列として返却する。
        /// </summary>
        /// <returns>文字列配列</returns>
        public string[]? ToValueStrings()
            => Protocol.ToValueStrings();

        public override string ToString()
            => Protocol.ToString();

        private interface PropertyValueProtocol
        {
            string? ToValueString();
            string[]? ToValueStrings();
        }

        private class ForTypeConstant : PropertyValueProtocol
        {
            private TypedConstant? Impl { get; }

            public ForTypeConstant(TypedConstant? impl)
            {
                Impl = impl;
            }

            /// <summary>
            ///     値を文字列として返却する。
            /// </summary>
            /// <returns>文字列</returns>
            public string? ToValueString()
                => Impl?.ToValueString();

            /// <summary>
            ///     値を文字列の配列として返却する。
            /// </summary>
            /// <returns>文字列配列</returns>
            public string[]? ToValueStrings()
            {
                if (Impl is null) return null;

                var constant = Impl.Value;
                if (constant.Kind != TypedConstantKind.Array)
                {
                    return constant.Value is null
                        ? null
                        : new[] { constant.Value.ToString() };
                }

                return constant.Values.Select(x => x.ToValueString() ?? "null")
                    .ToArray();
            }

            public override string ToString()
            {
                var strings = ToValueStrings();
                if (strings is null) return "null";
                return string.Join(", ", ToValueStrings() ?? Array.Empty<string>());
            }
        }

        private class ForString : PropertyValueProtocol
        {
            private string? Impl { get; }

            public ForString(string? impl)
            {
                Impl = impl;
            }

            /// <summary>
            ///     値を文字列として返却する。
            /// </summary>
            /// <returns>文字列</returns>
            public string? ToValueString()
                => Impl;

            /// <summary>
            ///     値を文字列の配列として返却する。
            /// </summary>
            /// <returns>文字列配列</returns>
            public string[]? ToValueStrings()
            {
                if (Impl is null) return null;

                return new[] { Impl };
            }

            public override string ToString()
            {
                var strings = ToValueStrings();
                if (strings is null) return "null";
                return string.Join(", ", ToValueStrings() ?? Array.Empty<string>());
            }
        }
    }
}
