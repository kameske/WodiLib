// ========================================
// Project Name : WodiLib
// File Name    : Direction.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Sys
{
    /// <summary>
    ///     方向
    /// </summary>
    public record Direction : TypeSafeEnum<Direction>
    {
        /// <summary>行</summary>
        public static Direction Row { get; }

        /// <summary>列</summary>
        public static Direction Column { get; }

        /// <summary>未指定</summary>
        public static Direction None { get; }

        static Direction()
        {
            Row = new Direction(nameof(Row));
            Column = new Direction(nameof(Column));
            None = new Direction(nameof(None));
        }

        private Direction(string id) : base(id)
        {
        }

        /// <inheritdoc/>
        public override string ToString()
            => base.ToString();
    }
}
