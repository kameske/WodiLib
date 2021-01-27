using System;
using System.Collections.Generic;
using NUnit.Framework;
using WodiLib.Sys;

namespace WodiLib.Test.Sys
{
    [TestFixture]
    public class ModelTest
    {
        [TestCase(false, false)]
        [TestCase(true, false)]
        [TestCase(true, true)]
        public static void PropertyChangeTest(bool isNotifyBeforePropertyChange,
            bool isNotifyAfterPropertyChange)
        {
            var propertyChanging = new List<string>();
            var propertyChanged = new List<string>();

            var assertNotifiedProperty = new Action<List<string>, string[]>((list, strings) =>
            {
                Assert.AreEqual(list.Count, strings.Length);
                for (var i = 0; i < list.Count; i++)
                {
                    Assert.IsTrue(list[i].Equals(strings[i]));
                }
            });

            {
                var model = new BasicModel
                {
                    IsNotifyBeforePropertyChange = isNotifyBeforePropertyChange,
                    IsNotifyAfterPropertyChange = isNotifyAfterPropertyChange,
                };
                model.PropertyChanging += (_, args) => propertyChanging.Add(args.PropertyName);
                model.PropertyChanged += (_, args) => propertyChanged.Add(args.PropertyName);

                {
                    // Text
                    // プロパティ名が通知されること
                    model.Text = "update";

                    assertNotifiedProperty(propertyChanging,
                        isNotifyBeforePropertyChange ? new[] {nameof(model.Text)} : Array.Empty<string>());
                    assertNotifiedProperty(propertyChanged,
                        isNotifyAfterPropertyChange ? new[] {nameof(model.Text)} : Array.Empty<string>());

                    propertyChanging.Clear();
                    propertyChanged.Clear();
                }

                {
                    // IntValue
                    // 指定したプロパティ名が通知されること
                    model.IntValue(35);

                    assertNotifiedProperty(propertyChanging,
                        isNotifyBeforePropertyChange ? new[] {nameof(model.Id)} : Array.Empty<string>());
                    assertNotifiedProperty(propertyChanged,
                        isNotifyAfterPropertyChange ? new[] {nameof(model.Id)} : Array.Empty<string>());

                    propertyChanging.Clear();
                    propertyChanged.Clear();
                }
            }

            {
                var model = new ExtendModel
                {
                    IsNotifyBeforePropertyChange = isNotifyBeforePropertyChange,
                    IsNotifyAfterPropertyChange = isNotifyAfterPropertyChange,
                };
                model.PropertyChanging += (_, args) => propertyChanging.Add(args.PropertyName);
                model.PropertyChanged += (_, args) => propertyChanged.Add(args.PropertyName);

                {
                    // Text
                    // 親クラスのプロパティ変更通知されること
                    model.Text = "update";

                    assertNotifiedProperty(propertyChanging,
                        isNotifyBeforePropertyChange ? new[] {nameof(model.Text)} : Array.Empty<string>());
                    assertNotifiedProperty(propertyChanged,
                        isNotifyAfterPropertyChange ? new[] {nameof(model.Text)} : Array.Empty<string>());

                    propertyChanging.Clear();
                    propertyChanged.Clear();
                }

                {
                    // Age
                    // 子クラスのプロパティ変更通知されること
                    model.Age = 20;

                    assertNotifiedProperty(propertyChanging,
                        isNotifyBeforePropertyChange ? new[] {nameof(model.Age)} : Array.Empty<string>());
                    assertNotifiedProperty(propertyChanged,
                        isNotifyAfterPropertyChange ? new[] {nameof(model.Age)} : Array.Empty<string>());

                    propertyChanging.Clear();
                    propertyChanged.Clear();
                }
            }

            {
                var model = new WrapperModel
                {
                    IsNotifyBeforePropertyChange = isNotifyBeforePropertyChange,
                    IsNotifyAfterPropertyChange = isNotifyAfterPropertyChange,
                };
                model.PropertyChanging += (_, args) => propertyChanging.Add(args.PropertyName);
                model.PropertyChanged += (_, args) => propertyChanged.Add(args.PropertyName);

                {
                    // Count
                    // 親クラスで定義されたメソッド内のプロパティ変更通知が呼ばれること
                    model.Count = 30;

                    assertNotifiedProperty(propertyChanging,
                        isNotifyBeforePropertyChange ? new[] {nameof(BasicModel.Id)} : Array.Empty<string>());
                    assertNotifiedProperty(propertyChanged,
                        isNotifyAfterPropertyChange ? new[] {nameof(BasicModel.Id)} : Array.Empty<string>());

                    propertyChanging.Clear();
                    propertyChanged.Clear();
                }

                {
                    // Name
                    // 通知ブロックしたプロパティ名が通知されないこと
                    model.Name = "Alex";

                    assertNotifiedProperty(propertyChanging,
                        isNotifyBeforePropertyChange ? new[] {nameof(model.Name)} : Array.Empty<string>());
                    assertNotifiedProperty(propertyChanged,
                        isNotifyAfterPropertyChange ? new[] {nameof(model.Name)} : Array.Empty<string>());

                    propertyChanging.Clear();
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
                set
                {
                    NotifyPropertyChanging();
                    text = value;
                    NotifyPropertyChanged();
                }
            }

            private int number = 0;

            public int Id => number;

            public void IntValue(int value)
            {
                NotifyPropertyChanging(nameof(Id));
                number = value;
                NotifyPropertyChanged(nameof(Id));
            }

            public override bool ItemEquals(BasicModel other)
                => ReferenceEquals(this, other);
        }

        public class ExtendModel : BasicModel
        {
            private int age = 0;

            public int Age
            {
                get => age;
                set
                {
                    NotifyPropertyChanging();
                    age = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public class WrapperModel : ModelBase<WrapperModel>
        {
            public string Name
            {
                get => impl.Text;
                set
                {
                    NotifyPropertyChanging();
                    impl.Text = value;
                    NotifyPropertyChanged();
                }
            }

            public int Count
            {
                get => impl.Id;
                set => impl.IntValue(value);
            }

            private readonly BasicModel impl = new BasicModel();

            public WrapperModel()
            {
                // "Text" の通知をブロック
                PropagatePropertyChangeEvent(impl,
                    (_, propName) => !propName.Equals(nameof(impl.Text)));
            }

            public override bool ItemEquals(WrapperModel other)
                => ReferenceEquals(this, other);
        }

        #endregion
    }
}
