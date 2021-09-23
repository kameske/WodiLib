// ReSharper disable once CheckNamespace

namespace WodiLib.Sys.Cmn
{
    public class WodiLibLogger
    {
        public static WodiLibLogger GetInstance()
            => new();

        public void Warning(params object[] args)
        {
        }
    }
}
