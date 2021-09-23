// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : PostInitializationInfoRegister.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using WodiLib.SourceGenerator.Core;
using WodiLib.SourceGenerator.ValueObject.Generation.PostInitAction.Attributes;
using WodiLib.SourceGenerator.ValueObject.Generation.PostInitAction.Enums;

namespace WodiLib.SourceGenerator.ValueObject
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
                    ByteValueObjectAttribute.Instance,
                    IntValueObjectAttribute.Instance,
                    MultiValueObjectAttribute.Instance,
                    StringValueObjectAttribute.Instance,
                    // enums
                    CastType.Instance,
                    IntegralNumericOperation.Instance
                };

                return result;
            }
        }
    }
}
