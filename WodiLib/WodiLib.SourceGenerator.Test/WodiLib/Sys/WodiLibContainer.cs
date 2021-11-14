// ReSharper disable once CheckNamespace

namespace WodiLib.Sys
{
    internal static partial class WodiLibContainer
    {
        static WodiLibContainer()
        {
            RegisterModels();
        }

        static partial void RegisterModels();
    }
}
