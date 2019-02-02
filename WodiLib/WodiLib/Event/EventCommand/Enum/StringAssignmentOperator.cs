// ========================================
// Project Name : WodiLib
// File Name    : StringAssignmentOperator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// 文字列操作演算子
    /// </summary>
    public class StringAssignmentOperator : TypeSafeEnum<StringAssignmentOperator>
    {
        /// <summary>=</summary>
        public static readonly StringAssignmentOperator Assign;

        /// <summary>+=</summary>
        public static readonly StringAssignmentOperator Addition;

        /// <summary>1行コピー</summary>
        public static readonly StringAssignmentOperator CopyFirstLine;

        /// <summary>1行切り出し</summary>
        public static readonly StringAssignmentOperator CutFirstLine;

        /// <summary>1文字切り出し</summary>
        public static readonly StringAssignmentOperator CutFirstCharacter;

        /// <summary>ファイル内容読み込み</summary>
        public static readonly StringAssignmentOperator LoadFileContent;

        /// <summary>ファイル出力</summary>
        public static readonly StringAssignmentOperator ExportToFile;

        /// <summary>フォルダのファイルリスト取得</summary>
        public static readonly StringAssignmentOperator GetFileList;

        /// <summary>文字列消去</summary>
        public static readonly StringAssignmentOperator Remove;

        /// <summary>文字列置換</summary>
        public static readonly StringAssignmentOperator Replace;

        /// <summary>指定文字まで切り出し</summary>
        public static readonly StringAssignmentOperator CutUp;

        /// <summary>指定文字以降切り出し</summary>
        public static readonly StringAssignmentOperator CutAfter;

        /// <summary>値</summary>
        public byte Code { get; }

        static StringAssignmentOperator()
        {
            Assign = new StringAssignmentOperator(nameof(Assign), 0x00);
            Addition = new StringAssignmentOperator(nameof(Addition), 0x01);
            CopyFirstLine = new StringAssignmentOperator(nameof(CopyFirstLine), 0x02);
            CutFirstLine = new StringAssignmentOperator(nameof(CutFirstLine), 0x03);
            CutFirstCharacter = new StringAssignmentOperator(nameof(CutFirstCharacter), 0x04);
            LoadFileContent = new StringAssignmentOperator(nameof(LoadFileContent), 0x05);
            ExportToFile = new StringAssignmentOperator(nameof(ExportToFile), 0x06);
            GetFileList = new StringAssignmentOperator(nameof(GetFileList), 0x07);
            Remove = new StringAssignmentOperator(nameof(Remove), 0x08);
            Replace = new StringAssignmentOperator(nameof(Replace), 0x09);
            CutUp = new StringAssignmentOperator(nameof(CutUp), 0x0A);
            CutAfter = new StringAssignmentOperator(nameof(CutAfter), 0x0B);
        }

        private StringAssignmentOperator(string id, byte code) : base(id)
        {
            Code = code;
        }

        /// <summary>
        /// バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">バイト値</param>
        /// <returns>インスタンス</returns>
        public static StringAssignmentOperator FromByte(byte code)
        {
            return _FindFirst(x => x.Code == code);
        }
    }
}