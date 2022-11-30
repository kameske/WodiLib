using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using WodiLib.Sys;

namespace WodiLib.Test.Sys
{
    [TestFixture]
    public class ModelTest
    {
        [Test]
        public static void PropertyChangeTest()
        {
            var propertyChanged = new List<string>();

            var assertNotifiedProperty = new Action<List<string>, string[]>(
                (list, strings) =>
                {
                    Assert.AreEqual(list.Count, strings.Length);

                    var listSorted = list.ToList();
                    listSorted.Sort();
                    var stringsSorted = strings.ToList();
                    stringsSorted.Sort();

                    for (var i = 0; i < list.Count; i++)
                    {
                        Assert.IsTrue(listSorted[i].Equals(stringsSorted[i]));
                    }
                }
            );

            {
                var model = new BasicModel();
                model.PropertyChanged += (_, args) => propertyChanged.Add(args.PropertyName);

                {
                    // Text
                    // プロパティ名が通知されること
                    model.Text = "update";

                    assertNotifiedProperty(propertyChanged, new[] { nameof(model.Text) });

                    propertyChanged.Clear();
                }

                {
                    // IntValue
                    // 指定したプロパティ名が通知されること
                    model.IntValue(35);

                    assertNotifiedProperty(propertyChanged, new[] { nameof(model.Id) });

                    propertyChanged.Clear();
                }
            }

            {
                var model = new ExtendModel();
                model.PropertyChanged += (_, args) => propertyChanged.Add(args.PropertyName);

                {
                    // Text
                    // 親クラスのプロパティ変更通知されること
                    model.Text = "update";

                    assertNotifiedProperty(propertyChanged, new[] { nameof(model.Text) });

                    propertyChanged.Clear();
                }

                {
                    // Age
                    // 子クラスのプロパティ変更通知されること
                    model.Age = 20;

                    assertNotifiedProperty(propertyChanged, new[] { nameof(model.Age) });

                    propertyChanged.Clear();
                }
            }

            {
                var model = new WrapperModel0();
                model.PropertyChanged += (_, args) => propertyChanged.Add(args.PropertyName);

                {
                    // Id
                    // 親クラスで定義されたメソッド内のプロパティ変更通知が呼ばれること
                    model.Id = 30;

                    assertNotifiedProperty(propertyChanged, new[] { nameof(BasicModel.Id) });

                    propertyChanged.Clear();
                }

                {
                    // Name
                    // 親クラスで定義されたメソッド内のプロパティ変更通知、および自分自身の変更通知が呼ばれること
                    model.Name = "Alex";

                    assertNotifiedProperty(propertyChanged, new[] { nameof(BasicModel.Text), nameof(model.Name) });

                    propertyChanged.Clear();
                }
            }

            {
                var model = new WrapperModel1();
                model.PropertyChanged += (_, args) => propertyChanged.Add(args.PropertyName);

                {
                    // Id
                    // 許可していないプロパティ名が通知されないこと
                    model.Id = 30;

                    Assert.IsTrue(propertyChanged.Count == 0);

                    propertyChanged.Clear();
                }

                {
                    // Name
                    // 許可したプロパティ名が通知されること
                    model.Name = "Alex";

                    assertNotifiedProperty(propertyChanged, new[] { $"{nameof(BasicModel.Text)}" });

                    propertyChanged.Clear();
                }
            }

            {
                var model = new WrapperModel2();
                model.PropertyChanged += (_, args) => propertyChanged.Add(args.PropertyName);

                {
                    // Id
                    // 親クラスで定義されたメソッド内のプロパティ変更通知が呼ばれること
                    model.Id = 30;

                    assertNotifiedProperty(propertyChanged, new[] { nameof(BasicModel.Id) });

                    propertyChanged.Clear();
                }

                {
                    // Name
                    // 通知ブロックしたプロパティ名が通知されないこと
                    model.Name = "Alex";

                    assertNotifiedProperty(propertyChanged, new[] { nameof(model.Name) });

                    propertyChanged.Clear();
                }
            }

            {
                var model = new WrapperModel3();
                model.PropertyChanged += (_, args) => propertyChanged.Add(args.PropertyName);

                {
                    // Name
                    // "Text" に加え "Id" が通知されること
                    model.Name = "Alex";

                    assertNotifiedProperty(
                        propertyChanged,
                        new[] { nameof(BasicModel.Text), nameof(BasicModel.Id), nameof(model.Name) }
                    );

                    propertyChanged.Clear();
                }
            }
        }


        #region TestClass

        public class BasicModel : ModelBase<BasicModel>
        {
            private string text = "";

            public string Text
            {
                get => text;
                set => SetField(ref text, value);
            }

            private int number = 0;

            public int Id => number;

            public void IntValue(int value)
            {
                SetField(ref number, value, nameof(Id));
            }

            public override bool ItemEquals(BasicModel? other)
                => ReferenceEquals(this, other);

            public override BasicModel DeepClone()
            {
                // ModelBaseに対してDeepCloneをテストする意味はない
                throw new Exception();
            }
        }

        public class ExtendModel : BasicModel
        {
            private int age = 0;

            public int Age
            {
                get => age;
                set => SetField(ref age, value);
            }
        }

        public class WrapperModel0 : ModelBase<WrapperModel0>
        {
            public string Name
            {
                get => impl.Text;
                set
                {
                    impl.Text = value;
                    NotifyPropertyChanged();
                }
            }

            public int Id
            {
                get => impl.Id;
                set => impl.IntValue(value);
            }

            private readonly BasicModel impl = new BasicModel();

            public WrapperModel0()
            {
                // 通知を無条件に伝播する
                PropagatePropertyChangeEvent(impl);
            }

            public override bool ItemEquals(WrapperModel0? other)
                => ReferenceEquals(this, other);

            public override WrapperModel0 DeepClone()
            {
                throw new Exception();
            }
        }

        public class WrapperModel1 : ModelBase<WrapperModel1>
        {
            public string Name
            {
                get => impl.Text;
                set => impl.Text = value;
            }

            public int Id
            {
                get => impl.Id;
                set => impl.IntValue(value);
            }

            private readonly BasicModel impl = new BasicModel();

            public WrapperModel1()
            {
                // "Text" のみ通知する
                PropagatePropertyChangeEvent(impl, new[] { $"{nameof(impl.Text)}" });
            }

            public override bool ItemEquals(WrapperModel1? other)
                => ReferenceEquals(this, other);

            public override WrapperModel1 DeepClone()
            {
                throw new Exception();
            }
        }

        public class WrapperModel2 : ModelBase<WrapperModel2>
        {
            public string Name
            {
                get => impl.Text;
                set
                {
                    impl.Text = value;
                    NotifyPropertyChanged();
                }
            }

            public int Id
            {
                get => impl.Id;
                set => impl.IntValue(value);
            }

            private readonly BasicModel impl = new BasicModel();

            public WrapperModel2()
            {
                // "Text" の通知をブロック
                PropagatePropertyChangeEvent(
                    impl,
                    (_, propName) => !propName.Equals(nameof(impl.Text))
                );
            }

            public override bool ItemEquals(WrapperModel2? other)
                => ReferenceEquals(this, other);

            public override WrapperModel2 DeepClone()
            {
                throw new Exception();
            }
        }

        public class WrapperModel3 : ModelBase<WrapperModel3>
        {
            public string Name
            {
                get => impl.Text;
                set
                {
                    impl.Text = value;
                    NotifyPropertyChanged();
                }
            }

            public int Id
            {
                get => impl.Id;
                set => impl.IntValue(value);
            }

            private readonly BasicModel impl = new();

            public WrapperModel3()
            {
                // "Text" が通知されたら "Id" も通知する
                PropagatePropertyChangeEvent(
                    impl,
                    (_, propName) =>
                    {
                        return propName switch
                        {
                            nameof(impl.Text) => new[] { nameof(impl.Text), nameof(impl.Id) },
                            _ => new[] { propName }
                        };
                    }
                );
            }

            public override bool ItemEquals(WrapperModel3? other)
                => ReferenceEquals(this, other);

            public override WrapperModel3 DeepClone()
            {
                throw new Exception();
            }
        }

        #endregion
    }
}
