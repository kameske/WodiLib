// ========================================
// Project Name : WodiLib
// File Name    : WodiLibContainer.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace WodiLib.Sys
{
    /// <summary>
    /// オブジェクト注入用クラス
    /// </summary>
    internal static class WodiLibContainer
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Static Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 指定したキー名のコンテナ内に指定したクラスの生成メソッドが登録されているかどうかを返す。
        /// </summary>
        /// <param name="key">[NotNull] キー名</param>
        /// <typeparam name="T">チェック対象のクラス型</typeparam>
        /// <returns>生成メソッドが登録されている場合true</returns>
        /// <exception cref="ArgumentNullException">keyがnullの場合</exception>
        public static bool HasCreateMethod<T>(string key = "default")
        {
            if (key is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(key)));

            // キー名のコンテナ存在チェック
            var containerKv = ContainerDic.FirstOrDefault(kv => kv.Key.Equals(key));
            if (IsNull(containerKv)) return false;

            // コンテナ内の情報チェック
            var container = containerKv.Value;
            return container.ContainsKey(typeof(T));
        }

        /// <summary>
        /// 実装クラスを登録する。登録済みの型でも上書き可能。
        /// </summary>
        /// <param name="createMethod">インスタンス生成メソッド</param>
        /// <param name="lifetime">ライフタイム</param>
        /// <param name="key">コンテナ名</param>
        /// <typeparam name="T">登録型</typeparam>
        public static void Register<T>(Func<T> createMethod, Lifetime lifetime, string key = "default")
        {
            // 登録情報作成
            var createObjMethod = new Func<object>(() => (object) createMethod());
            var createInfo = new CreateInfo(createObjMethod, lifetime);

            // 登録先コンテナ取得
            var containerKv = ContainerDic.FirstOrDefault(kv => kv.Key.Equals(key));
            if (IsNull(containerKv))
            {
                // コンテナが存在しないので、先にコンテナを作ってから
                var newContainer = new Dictionary<Type, CreateInfo>
                {
                    {typeof(T), createInfo}
                };
                ContainerDic.Add(key, newContainer);
            }
            else
            {
                // コンテナに登録
                var container = containerKv.Value;
                if (container.ContainsKey(typeof(T))) container.Remove(typeof(T));
                container.Add(typeof(T), createInfo);
            }
        }

        /// <summary>
        /// 実装クラスからインスタンスを生成して返却する。
        /// </summary>
        /// <typeparam name="T">インスタンス型</typeparam>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">登録されていない型を指定した場合</exception>
        public static T Resolve<T>(string key = "default")
        {
            // コンテナ取得
            if (!ContainerDic.ContainsKey(key)) throw new ContainerNotRegistrationException();
            var container = ContainerDic.First(kv => kv.Key.Equals(key)).Value;

            // インスタンス生成情報取得
            if (!container.ContainsKey(typeof(T))) throw new ContainerNotRegistrationException();
            var createInfo = container.First(kv => kv.Key == typeof(T)).Value;

            return (T) createInfo.GetInstance();
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Static Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private static bool IsNull(KeyValuePair<string, Dictionary<Type, CreateInfo>> src)
        {
            return src.Equals(default(KeyValuePair<string, Dictionary<Type, CreateInfo>>));
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private static readonly Dictionary<string, Dictionary<Type, CreateInfo>> ContainerDic;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        static WodiLibContainer()
        {
            ContainerDic = new Dictionary<string, Dictionary<Type, CreateInfo>>();
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Inner Class
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// インスタンスのライフタイム
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
        /// インスタンス生成情報
        /// </summary>
        private class CreateInfo
        {
            private Lifetime Lifetime { get; }
            private Func<object> CreateMethod { get; }
            private object instance;

            public CreateInfo(Func<object> createMethod, Lifetime lifetime)
            {
                Lifetime = lifetime;
                CreateMethod = createMethod;
            }

            /// <summary>
            /// ライフタイムに応じたインスタンス取得
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
                    return instance ?? (instance = CreateMethod());
                }

                throw new InvalidOperationException();
            }
        }
    }
}