using System.Collections.Generic;
using System.Linq;
using WodiLib.Common;
using WodiLib.Project;

namespace WodiLibSample.Model
{
    /// <summary>
    /// 選択したコモンイベント情報モデル
    /// </summary>
    internal class SelectedCommonEventInfo
    {
        internal List<LineAndEventCommandSentenceInfo> EventCommandSentenceList { get; }

        internal List<LineAndText> EventCommandCodeList { get; }

        internal SelectedCommonEventInfo(CommonEvent selected, WoditorProject project)
        {
            EventCommandSentenceList = project.GetCommonEventEventCommandSentenceInfoListSync(selected.Id)
                .Select((x, idx) => new LineAndEventCommandSentenceInfo(idx, x))
                .ToList();

            EventCommandCodeList = selected.EventCommands
                .Select((x, idx) => new LineAndText(idx, x.ToEventCodeString()))
                .ToList();
        }
    }
}