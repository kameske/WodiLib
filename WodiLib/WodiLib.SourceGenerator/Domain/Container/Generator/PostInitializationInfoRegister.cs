// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : PostInitializationInfoRegister.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using WodiLib.SourceGenerator.Core;
using WodiLib.SourceGenerator.Domain.Container.Generation.PostInitAction.Attributes;

namespace WodiLib.SourceGenerator.Domain.Container
{
    public partial class Generator
    {
        /// <summary>
        ///     自動生成に必要なクラスソースコード生成情報登録処理
        /// </summary>
        private static class PostInitializationInfoRegister
        {
            public static IEnumerable<IInitializeSourceAddable> MakePostInitializationRegisterInfoList()
            {
                var result = new List<IInitializeSourceAddable>
                {
                    // attributes
                    InjectableAttribute.Instance,
                };

                return result;
            }
        }
    }
}
