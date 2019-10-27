// ========================================
// Project Name : WodiLib
// File Name    : KeyboardCode.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;

namespace WodiLib.Cmn
{
    /// <summary>
    /// キーボードコード
    /// </summary>
    public static class KeyboardCode
    {
        private static readonly Dictionary<int, string> KeyCodeMap = new Dictionary<int, string>
        {
            {101, "ESC"}, {102, "1"}, {103, "2"}, {104, "3"}, {105, "4"},
            {106, "5"}, {107, "6"}, {108, "7"}, {109, "8"}, {110, "9"},
            {111, "0"}, {112, "-"}, {114, "BS"}, {115, "TAB"}, {116, "Q"},
            {117, "W"}, {118, "E"}, {119, "R"}, {120, "T"}, {121, "Y"},
            {122, "U"}, {123, "I"}, {124, "O"}, {125, "P"}, {126, "["},
            {127, "]"}, {128, "Enter"}, {129, "左Ctrl"}, {130, "A"}, {131, "S"},
            {132, "D"}, {133, "F"}, {134, "G"}, {135, "H"}, {136, "J"},
            {137, "K"}, {138, "L"}, {139, ";"}, {142, "左Shift"}, {143, "＼"},
            {144, "Z"}, {145, "X"}, {146, "C"}, {147, "V"}, {148, "B"},
            {149, "N"}, {150, "M"}, {151, ","}, {152, "."}, {153, "/"},
            {154, "右Shift"}, {155, "テンキー *"}, {156, "左Alt"}, {157, "Space"}, {158, "Caps"},
            {159, "F1"}, {160, "F2"}, {161, "F3"}, {162, "F4"}, {163, "F5"},
            {164, "F6"}, {165, "F7"}, {166, "F8"}, {167, "F9"}, {168, "F10"},
            {170, "Scroll"}, {171, "テンキー7"}, {172, "テンキー8"}, {173, "テンキー9"}, {174, "テンキー -"},
            {175, "テンキー4"}, {176, "テンキー5"}, {177, "テンキー6"}, {178, "テンキー +"}, {179, "テンキー1"},
            {180, "テンキー2"}, {181, "テンキー3"}, {182, "テンキー0"}, {183, "テンキー ."}, {187, "F11"},
            {188, "F12"}, {212, "ｶﾀｶﾀ・ひらがな"}, {221, "変換[切替]"}, {223, "無変換[切替]"}, {225, "\\"},
            {244, "^"}, {245, "@"}, {246, ":"}, {248, "半角/全角[切替]"}, {257, "右Ctrl"},
            {281, "テンキー /"}, {283, "PrintScreen"}, {284, "右Alt"}, {297, "Pause"}, {299, "HOME"},
            {300, "↑"}, {301, "PageUp"}, {303, "←"}, {305, "→"}, {307, "END"},
            {308, "↓"}, {309, "PageDown"}, {311, "DEL"},
        };

        private const string NotFound = "非対応";

        private const int KeyCodeMin = 100;
        private const int KeyCodeMax = 355;

        /// <summary>
        /// 指定したint値がキーコード値であるかどうかを判定して返す。
        /// </summary>
        /// <param name="value">判定対象値</param>
        /// <returns>キーコード値の場合true</returns>
        public static bool IsKeyCode(int value)
        {
            return KeyCodeMin <= value && value <= KeyCodeMax;
        }

        /// <summary>
        /// キーコード値からキー名を取得する。
        /// </summary>
        /// <param name="keyCode">キーコード</param>
        /// <returns>インスタンス</returns>
        public static string GetKeyName(int keyCode)
        {
            if (!KeyCodeMap.ContainsKey(keyCode)) return NotFound;
            return KeyCodeMap[keyCode];
        }
    }
}