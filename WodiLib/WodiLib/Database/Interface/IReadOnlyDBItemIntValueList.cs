using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    /// 【読み取り専用】データベース数値項目値リスト
    /// </summary>
    public interface IReadOnlyDBItemIntValueList : IReadOnlyRestrictedCapacityList<DBValueInt>
    {
    }
}
