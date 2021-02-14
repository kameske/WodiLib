// ========================================
// Project Name : WodiLib
// File Name    : StringAssignmentOperator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.ComponentModel;
using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    ///     文字列操作演算子
    /// </summary>
    public record StringAssignmentOperator : TypeSafeEnum<StringAssignmentOperator>
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

        /// <summary>イベントコマンド文字列</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal string EventCommandSentence { get; }

        static StringAssignmentOperator()
        {
            Assign = new StringAssignmentOperator(nameof(Assign), 0x00,
                "=");
            Addition = new StringAssignmentOperator(nameof(Addition), 0x01,
                "+=");
            CopyFirstLine = new StringAssignmentOperator(nameof(CopyFirstLine), 0x02,
                "=<上1行ｺﾋﾟｰ>");
            CutFirstLine = new StringAssignmentOperator(nameof(CutFirstLine), 0x03,
                "=<上1行切出>");
            CutFirstCharacter = new StringAssignmentOperator(nameof(CutFirstCharacter), 0x04,
                "=<1文字切出>");
            LoadFileContent = new StringAssignmentOperator(nameof(LoadFileContent), 0x05,
                "=<ﾌｧｲﾙ内容読込>");
            ExportToFile = new StringAssignmentOperator(nameof(ExportToFile), 0x06,
                "=<を→のﾌｧｲﾙに出力>");
            GetFileList = new StringAssignmentOperator(nameof(GetFileList), 0x07,
                "=<→のﾌｫﾙﾀﾞのﾌｧｲﾙﾘｽﾄ取得>");
            Remove = new StringAssignmentOperator(nameof(Remove), 0x08,
                "=<から→の文字列を全消去>");
            Replace = new StringAssignmentOperator(nameof(Replace), 0x09,
                "=<から文字列を置換>");
            CutUp = new StringAssignmentOperator(nameof(CutUp), 0x0A,
                "=<から指定文字まで切り出し>");
            CutAfter = new StringAssignmentOperator(nameof(CutAfter), 0x0B,
                "=<の指定文字以降を切り出し>");
        }

        private StringAssignmentOperator(string id, byte code,
            string eventCommandSentence) : base(id)
        {
            Code = code;
            EventCommandSentence = eventCommandSentence;
        }

        /// <summary>
        ///     バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">バイト値</param>
        /// <returns>インスタンス</returns>
        public static StringAssignmentOperator FromByte(byte code)
        {
            return AllItems.First(x => x.Code == code);
        }

        /// <inheritdoc/>
        public override string ToString()
            => base.ToString();
    }
}
