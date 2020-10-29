using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    /// 【読み取り専用】データベース文字列項目値リスト
    /// </summary>
    public interface
        IReadOnlyDBItemStringValueList : IReadOnlyRestrictedCapacityList<DBValueString>
    {
    }
}
