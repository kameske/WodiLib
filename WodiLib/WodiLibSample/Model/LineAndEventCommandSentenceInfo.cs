using WodiLib.Project;

namespace WodiLibSample.Model
{
    /// <summary>
    /// 行数とイベントコマンド文情報の組モデル
    /// </summary>
    internal class LineAndEventCommandSentenceInfo
    {
        public int Line { get; }
        public EventCommandSentenceInfo Info { get; }

        public LineAndText LineAndText { get; }

        public LineAndEventCommandSentenceInfo(int line, EventCommandSentenceInfo info)
        {
            Line = line;
            Info = info;
            LineAndText = new LineAndText(line, info.Sentence);
        }

    }
}