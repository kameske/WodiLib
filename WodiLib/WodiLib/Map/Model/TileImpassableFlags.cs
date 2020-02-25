// ========================================
// Project Name : WodiLib
// File Name    : TileImpassableFlags.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    /// タイル通行不可フラグ
    /// </summary>
    /// <remarks>
    /// 各プロパティが "true" のとき、そのエリアへの進入は不可能。
    /// 「trueのときに進入可能」ではないことに注意。
    ///
    /// すべてのプロパティを "false" にすることはできない。
    /// </remarks>
    [Serializable]
    public class TileImpassableFlags : IEquatable<TileImpassableFlags>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>右下コード</summary>
        private const int RightDownCode = 0x01;

        /// <summary>左下コード</summary>
        private const int LeftDownCode = 0x02;

        /// <summary>右上コード</summary>
        private const int RightUpCode = 0x04;

        /// <summary>左上コード</summary>
        private const int LeftUpCode = 0x08;

        private readonly string PropertyAllFalseErrorMessage
            = $"{nameof(RightDown)}, {nameof(LeftDown)}, {nameof(RightUp)}, " +
              $"{nameof(LeftUp)}のうちいずれかは{true}である必要があります。";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private bool rightDown = true;

        /// <summary>
        /// 右下通行不可フラグ
        /// </summary>
        public bool RightDown
        {
            get => rightDown;
            set
            {
                if (value)
                {
                    rightDown = true;
                    return;
                }

                // Value == false
                if (IsAllFalse(LeftDown, RightUp, LeftUp))
                {
                    throw new PropertyException(PropertyAllFalseErrorMessage);
                }

                rightDown = false;
            }
        }

        private bool leftDown = true;

        /// <summary>
        /// 左下通行不可フラグ
        /// </summary>
        public bool LeftDown
        {
            get => leftDown;
            set
            {
                if (value)
                {
                    leftDown = true;
                    return;
                }

                // Value == false
                if (IsAllFalse(RightDown, RightUp, LeftUp))
                {
                    throw new PropertyException(PropertyAllFalseErrorMessage);
                }

                leftDown = false;
            }
        }

        private bool rightUp = true;

        /// <summary>
        /// 右上通行不可フラグ
        /// </summary>
        public bool RightUp
        {
            get => rightUp;
            set
            {
                if (value)
                {
                    rightUp = true;
                    return;
                }

                // Value == false
                if (IsAllFalse(RightDown, LeftDown, LeftUp))
                {
                    throw new PropertyException(PropertyAllFalseErrorMessage);
                }

                rightUp = false;
            }
        }

        private bool leftUp = true;

        /// <summary>
        /// 左上通行不可フラグ
        /// </summary>
        public bool LeftUp
        {
            get => leftUp;
            set
            {
                if (value)
                {
                    leftUp = true;
                    return;
                }

                // Value == false
                if (IsAllFalse(RightDown, LeftDown, RightUp))
                {
                    throw new PropertyException(PropertyAllFalseErrorMessage);
                }

                leftUp = false;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TileImpassableFlags()
        {
            RightDown = true;
            LeftDown = true;
            RightUp = true;
            LeftUp = true;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="impassableFlagCode">通行不可設定フラグコード値</param>
        public TileImpassableFlags(int impassableFlagCode)
        {
            RightDown = (impassableFlagCode & RightDownCode) != 0;
            LeftDown = (impassableFlagCode & LeftDownCode) != 0;
            RightUp = (impassableFlagCode & RightUpCode) != 0;
            LeftUp = (impassableFlagCode & LeftUpCode) != 0;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(TileImpassableFlags other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            if (RightDown != other.RightDown) return false;
            if (LeftDown != other.LeftDown) return false;
            if (RightUp != other.RightUp) return false;
            if (LeftUp != other.LeftUp) return false;

            return true;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Static Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private static bool IsAllFalse(params bool[] targets)
        {
            return !targets.Any(x => x);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Common
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コード値変換する。
        /// </summary>
        /// <returns>コード値</returns>
        public int ToCode()
        {
            var code = 0;
            code += RightDown ? RightDownCode : 0;
            code += LeftDown ? LeftDownCode : 0;
            code += RightUp ? RightUpCode : 0;
            code += LeftUp ? LeftUpCode : 0;

            return code;
        }
    }
}