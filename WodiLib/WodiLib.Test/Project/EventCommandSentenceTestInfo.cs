using WodiLib.Common;
using WodiLib.IO;
using WodiLib.Map;

namespace WodiLib.Test.Project
{
    /// <summary>
    /// イベントコマンド文字列テスト用情報クラス
    /// </summary>
    public class EventCommandSentenceTestInfo
    {
        /// <summary>プロジェクトルートディレクトリ</summary>
        public string ProjectDir { get; set; }

        /// <summary>マップイベント情報</summary>
        public MapEvent MapEventInfo { get; set; }

        /// <summary>コモンイベント情報</summary>
        public CommonEvent CommonEventInfo { get; set; }

        public class MapEvent
        {
            public MpsFilePath FilePath { get; set; }
            public MapEventId MapEventId { get; set; }
            public int PageIndex { get; set; }
            /// <summary>出力結果を比較するコマンド文が記録されたファイルのパス</summary>
            public string EventCommandSentenceAnswerFilePath { get; set; }

        }

        public class CommonEvent
        {
            public CommonEventId CommonEventId { get; set; }
            public MpsFilePath MpsFilePath { get; set; }
            /// <summary>出力結果を比較するコマンド文が記録されたファイルのパス</summary>
            public string EventCommandSentenceAnswerFilePath { get; set; }

        }
    }
}