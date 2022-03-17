// ========================================
// Project Name : WodiLib
// File Name    : WodiLibContainer.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;

namespace WodiLib.Sys
{
    /// <summary>
    ///     オブジェクト注入用クラス
    /// </summary>
    internal static partial class WodiLibContainer
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Constants
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     デフォルト設定キー名
        /// </summary>
        public static WodiLibContainerKeyName DefaultKeyName => "default";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Static Properties
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     現在の設定キー名
        /// </summary>
        /// <remarks>
        ///     キー名の変更は <see cref="ChangeTargetKey"/> メソッドで行う。
        /// </remarks>
        public static WodiLibContainerKeyName TargetKeyName { get; private set; } = DefaultKeyName;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Protected Static Properties
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private static DictForKey ContainerDic { get; } = new();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        static WodiLibContainer()
        {
            RegisterModels();
        }

        /// <summary>
        /// WodiLib 内モデルクラスの生成処理を登録する。
        /// </summary>
        /// <remarks>
        /// このメソッドの実装は WodiLib.SourceGenerator から出力される。
        /// </remarks>
        static partial void RegisterModels();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Static Methods
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     指定したキー名のコンテナが作成済みかどうかを返す。
        /// </summary>
        /// <param name="key">キー名</param>
        /// <returns>生成メソッドが登録されている場合 <see langword="true"/></returns>
        public static bool HasContainer(WodiLibContainerKeyName key)
        {
            ThrowHelper.ValidateArgumentNotNull(key is null, nameof(key));

            return ContainerDic.HasKey(key);
        }

        /// <summary>
        ///     指定したキー名のコンテナ内に指定したクラスの生成メソッドが登録されているかどうかを返す。
        /// </summary>
        /// <param name="key">キー名(<see langword="null"/>の場合<see cref="TargetKeyName"/>が設定される)</param>
        /// <typeparam name="TOut">登録型</typeparam>
        /// <returns>生成メソッドが登録されている場合 <see langword="true"/></returns>
        public static bool HasCreateMethod<TOut>(WodiLibContainerKeyName? key = null)
            => ContainerDic.HasInfo(key ?? TargetKeyName, typeof(TOut));

        /// <summary>
        ///     メインで使用する設定キーを変更する。
        /// </summary>
        /// <remarks>
        ///     新規キー名を指定した場合、 "default" に設定されているすべてのインスタンス生成情報を引き継いだ状態で設定される。
        /// </remarks>
        /// <param name="keyName">設定キー名</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="keyName"/> が <see langword="null"/> の場合。
        /// </exception>
        public static void ChangeTargetKey(WodiLibContainerKeyName keyName)
        {
            ThrowHelper.ValidateArgumentNotNull(keyName is null, nameof(keyName));

            ContainerDic.CreateNewContainerIfNotHas(keyName);

            TargetKeyName = keyName;
        }

        /// <summary>
        ///     指定されたキー名のコンテナを新規作成する。作成済みの場合は何も行わない。
        /// </summary>
        /// <param name="key">作成コンテナキー名</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="key"/> が <see langword="null"/> の場合。
        /// </exception>
        public static void AddContainerIfNotHas(WodiLibContainerKeyName key)
        {
            ThrowHelper.ValidateArgumentNotNull(key is null, nameof(key));

            ContainerDic.CreateNewContainerIfNotHas(key);
        }

        /// <summary>
        ///     実装クラスを登録する。登録済みの型でも上書き可能。
        /// </summary>
        /// <param name="createMethod">インスタンス生成メソッド</param>
        /// <param name="lifetime">ライフタイム</param>
        /// <param name="key">キー名(<see langword="null"/>の場合<see cref="TargetKeyName"/>が設定される)</param>
        /// <typeparam name="TOut">登録型</typeparam>
        public static void Register<TOut>(
            Func<TOut> createMethod,
            Lifetime lifetime,
            WodiLibContainerKeyName? key = null
        )
            where TOut : IContainerCreatable
            => RegisterImpl(createMethod, lifetime, key ?? TargetKeyName, false);

        /// <summary>
        ///     実装クラスを登録する。登録済みの型の場合上書きは行わない。
        /// </summary>
        /// <param name="createMethod">インスタンス生成メソッド</param>
        /// <param name="lifetime">ライフタイム</param>
        /// <param name="key">キー名(<see langword="null"/>の場合<see cref="TargetKeyName"/>が設定される)</param>
        /// <typeparam name="TOut">登録型</typeparam>
        public static void RegisterIfNotHas<TOut>(
            Func<TOut> createMethod,
            Lifetime lifetime,
            WodiLibContainerKeyName? key = null
        )
            where TOut : IContainerCreatable
            => RegisterImpl(createMethod, lifetime, key ?? TargetKeyName, true);

        /// <summary>
        ///     実装クラスからインスタンスを生成して返却する。
        /// </summary>
        /// <typeparam name="T">インスタンス型</typeparam>
        /// <param name="key">キー名(<see langword="null"/>の場合<see cref="TargetKeyName"/>が設定される)</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">登録されていない型を指定した場合</exception>
        public static T Resolve<T>(WodiLibContainerKeyName? key = null)
            where T : IContainerCreatable
            => ResolveImpl<T>(key ?? TargetKeyName);

        /// <summary>
        ///     指定されたキー名のコンテナを除去する。未登録の場合は何も行わない。
        /// </summary>
        /// <param name="key">除去コンテナキー名</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="key"/> が <see langword="null"/> の場合。
        /// </exception>
        public static void RemoveContainerIfHas(WodiLibContainerKeyName key)
        {
            ThrowHelper.ValidateArgumentNotNull(key is null, nameof(key));

            ContainerDic.Remove(key);
        }

        /// <summary>
        ///     クラス生成情報を解除する。未登録、解除済みの場合は何もしない。
        /// </summary>
        /// <param name="key">キー名(<see langword="null"/>の場合<see cref="TargetKeyName"/>が設定される)</param>
        /// <typeparam name="TOut">登録型</typeparam>
        public static void Unregister<TOut>(WodiLibContainerKeyName? key = null)
            where TOut : IContainerCreatable
            => UnregisterImpl<TOut>(key ?? TargetKeyName);

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Private Static Methods
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private static void RegisterImpl<TOut>(
            Func<TOut> createMethod,
            Lifetime lifetime,
            string key,
            bool isSkipIfAlreadyRegistered
        )
            where TOut : notnull
        {
            // 上書きしない場合、登録済みなら処理しない
            if (isSkipIfAlreadyRegistered && ContainerDic.HasInfo(key, typeof(TOut)))
            {
                return;
            }

            // 登録情報作成
            var createObjMethod = new Func<object>(() => createMethod());
            var createInfo = new CreateInfo(createObjMethod, lifetime);

            ContainerDic[key, typeof(TOut)] = createInfo;
        }

        private static TOut ResolveImpl<TOut>(string key)
            where TOut : IContainerCreatable
        {
            var createInfo = ContainerDic[key, typeof(TOut)];
            var instance = (TOut)createInfo.GetInstance();
            instance.ContainerKeyName = key;
            return instance;
        }

        private static void UnregisterImpl<TOut>(WodiLibContainerKeyName targetKeyName)
            => ContainerDic.Unregister(targetKeyName, typeof(TOut));

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Classes
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     インスタンスのライフタイム
        /// </summary>
        public class Lifetime : TypeSafeEnum<Lifetime>
        {
            /// <summary>コンテナで一意</summary>
            public static readonly Lifetime Container;

            /// <summary>呼び出し毎に生成</summary>
            public static readonly Lifetime Transient;

            static Lifetime()
            {
                Container = new Lifetime(nameof(Container));
                Transient = new Lifetime(nameof(Transient));
            }

            private Lifetime(string id) : base(id)
            {
            }
        }

        /// <summary>
        ///     インスタンス生成情報
        /// </summary>
        private class CreateInfo
        {
            private Lifetime Lifetime { get; }
            private Func<dynamic> CreateMethod { get; }
            private object? instance;

            public CreateInfo(Func<dynamic> createMethod, Lifetime lifetime)
            {
                Lifetime = lifetime;
                CreateMethod = createMethod;
            }

            /// <summary>
            ///     ライフタイムに応じたインスタンス取得
            /// </summary>
            /// <returns></returns>
            /// <exception cref="InvalidOperationException"></exception>
            public object GetInstance()
            {
                if (Lifetime == Lifetime.Transient)
                {
                    // 都度生成する
                    return CreateMethod();
                }

                if (Lifetime == Lifetime.Container)
                {
                    // コンテナ内で一意
                    return instance ??= CreateMethod();
                }

                throw new InvalidOperationException();
            }

            /// <summary>
            ///     新規コンテナ用に情報をコピーしたインスタンスを取得する。
            /// </summary>
            /// <remarks>
            ///     インスタンス生成用の情報のみコピーし、自身が作成したインスタンス情報は引き継がない。
            /// </remarks>
            /// <returns>新規インスタンス</returns>
            public CreateInfo CloneInfoForNewContainer()
                => new(CreateMethod, Lifetime);
        }

        /// <summary>
        ///     キー名毎のディクショナリ
        /// </summary>
        private class DictForKey
        {
            public CreateInfo this[WodiLibContainerKeyName key, Type inputType]
            {
                get => Impl[key][inputType];
                set
                {
                    CreateNewContainerIfNotHas(key);
                    var inner = Impl[key];
                    inner[inputType] = value;
                }
            }

            public DictForKey()
            {
                Impl = new Dictionary<WodiLibContainerKeyName, DictForOutputType>
                {
                    [DefaultKeyName] = new()
                };
            }

            /// <summary>
            ///     生成情報を保持しているかどうかを返す。
            /// </summary>
            /// <param name="key">キー名</param>
            /// <param name="outputType">出力型</param>
            /// <returns>生成情報を保持している場合<see langword="true"/></returns>
            public bool HasInfo(WodiLibContainerKeyName key, Type outputType)
                => Impl.ContainsKey(key) && Impl[key].HasInfo(outputType);

            /// <summary>
            ///     キー名の情報が格納されているかどうかを返す。
            /// </summary>
            /// <param name="key">キー名</param>
            /// <returns>情報が格納されている場合<see langword="true"/></returns>
            public bool HasKey(WodiLibContainerKeyName key)
                => Impl.ContainsKey(key);

            /// <summary>
            ///     指定したキー名のコンテナを除去する。
            /// </summary>
            /// <remarks>
            ///     コンテナが存在しない場合は何も行わない。
            /// </remarks>
            /// <param name="key"></param>
            public void Remove(WodiLibContainerKeyName key)
            {
                if (!HasKey(key))
                {
                    return;
                }

                Impl.Remove(key);
            }

            /// <summary>
            ///     生成情報が登録されていれば解除する。
            /// </summary>
            /// <param name="key">コンテナキー名</param>
            /// <param name="outputType">出力型</param>
            public void Unregister(WodiLibContainerKeyName key, Type outputType)
            {
                if (!HasKey(key))
                {
                    return;
                }

                if (!HasInfo(key, outputType))
                {
                    return;
                }

                Impl[key].Unregister(outputType);
            }

            /// <summary>
            ///     キー名の情報が格納されていなければ作成する。
            /// </summary>
            /// <remarks>
            ///     新規作成する場合、実装生成情報はデフォルトコンテナに登録された内容を引き継ぐ。
            /// </remarks>
            /// <param name="keyName">キー名</param>
            public void CreateNewContainerIfNotHas(string keyName)
            {
                if (HasKey(keyName))
                {
                    return;
                }

                // デフォルトコンテナに登録されている情報を新規キー名のコンテナにコピーする
                var defaultInfo = Impl[DefaultKeyName];
                Impl[keyName] = defaultInfo.CloneInfoForNewContainer();
            }

            private Dictionary<WodiLibContainerKeyName, DictForOutputType> Impl { get; }

            /// <summary>
            ///     出力型毎のディクショナリ
            /// </summary>
            private class DictForOutputType
            {
                public CreateInfo this[Type inputType]
                {
                    get => Impl[inputType];
                    set => Impl[inputType] = value;
                }

                /// <summary>
                ///     生成情報を保持しているかどうかを返す。
                /// </summary>
                /// <param name="outputType">出力型</param>
                /// <returns>生成情報を保持している場合<see langword="true"/></returns>
                public bool HasInfo(Type outputType)
                    => Impl.ContainsKey(outputType);


                /// <summary>
                ///     生成情報を解除する。
                /// </summary>
                /// <param name="inputType">入力型</param>
                public void Unregister(Type inputType)
                    => Impl.Remove(inputType);

                /// <summary>
                ///     新規コンテナ用に情報をコピーしたインスタンスを取得する。
                /// </summary>
                /// <remarks>
                ///     インスタンス生成用の情報のみコピーし、自身が作成したインスタンス情報は引き継がない。
                /// </remarks>
                /// <returns>新規インスタンス</returns>
                public DictForOutputType CloneInfoForNewContainer()
                {
                    var newInstance = new DictForOutputType();

                    foreach (var key in Impl.Keys)
                    {
                        newInstance.Impl[key] = Impl[key].CloneInfoForNewContainer();
                    }

                    return newInstance;
                }

                private Dictionary<Type, CreateInfo> Impl { get; } = new();
            }
        }
    }
}
