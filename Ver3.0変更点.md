Ver 3.0 変更点
========================================

<details open>

<summary>現コミット時点で対応完了しているもの</summary>

ターゲットフレームワークと言語バージョン
----------------------------------------

- TargetFramework
    - ```.netframework461```, ```.netstandard2.0```
    - ```Ver1.X``` と比較して ```.netcoreapp3.0``` が非対応
    - ```Ver2.X``` と比較して ```.netframwork461``` が追加、 ```.netstandard``` がバージョンダウン

共通部分（WodiLib.Sys）
----------------------------------------

- ```WodiLibConfig``` （```Ver 3.0``` にて新規追加）
    - ```WodiLib``` 全体の動作を決定するコンフィグコンテナ。
        - 使用方法は ```VersionConfig``` と同じ。
            - キー名ごとに設定を保持する。
            - ```WodiLib``` 全体に適用するコンフィグをキー名によって指定。
    - ```Ver 3.0``` 時点では後述の「プロパティ変更通知」「コレクション変更通知」の動作決定のために使用するのみ。
    - 既存クラス ```VersionConfig``` は続投。仕様変更等もなし。
    - 設定値と初期値は以下のとおり。

|設定名|概要|デフォルト値|
|:--|:--|:--|
|DefaultNotifyBeforePropertyChangeFlag|プロパティ変更前の変更通知タイプ|```Disabled```|
|DefaultNotifyAfterPropertyChangeFlag|プロパティ変更後の変更通知タイプ|```Enabled```|
|DefaultNotifyBeforeCollectionChangeFlag|コレクション変更前の変更通知タイプ|```None```|
|DefaultNotifyAfterCollectionChangeFlag|コレクション変更後の変更通知タイプ|```Single```|

- ```ModelBase``` （可変クラス）
    - ```IEquatable<T>``` を除去、 ```==``` および ```!=``` 演算子のオーバーロードを解除。
        - デフォルトの参照型の動作と異なるため。（```List<T>.IndexOf``` などでこの影響を受けるため）
    - ```IEquatable<T>``` の代替機能として ```IEqualityComparable<T>``` インタフェースおよび ```bool ItemEquals(T)``` メソッドを新規実装。これまでの ```IEquatable<T>.Equals(T)``` メソッド同様モデルクラスの同値比較を行い結果を返す。
    - プロパティ変更通知に関する仕様変更および追加。
        - 変更前の通知イベントとして ```INotifyPropertyChanging``` を実装。
        - 変更通知の有無を決定するプロパティ ``` NotifyPropertyChangingEventType``` および ```NotifyPropertyChangedEventType``` を追加。それぞれ ```Enabled``` の場合のみ ```PropertyChanging```、```PropertyChanged``` が通知される。
            - ``` NotifyPropertyChangingEventType``` および ```NotifyPropertyChangedEventType``` の初期値は前述の ```WodiLibConfig``` の設定に準ずる。
    - ディープコピーできることを示す ```IDeepCloneable<T>``` インタフェース、およびディープコピー用のメソッド ```DeepClone()``` を新規追加。
        - このメソッドで作成したコピーインスタンスはすべての参照がディープコピーされる。

- ```RestrictedCapacityList```（旧名```RestrictedCapacityCollection```）、```FixedLengthList```（リストクラス）
    - コレクション変更通知に関する仕様変更および追加。
        - 変更前の通知イベントとして ```NotifyCollectionChangedEventHandler CollectionChanging``` イベントを実装。
        - 変更通知の有無を決定するプロパティ ```NotifyCollectionChangingEventType``` および ```NotifyCollectionChangedEventType``` を追加。設定値に応じて ```CollectionChanging```、```CollectionChanged``` が通知される際のイベント引数の内容が変化する。
            - ``` NotifyCollectionChangingEventType``` および ```NotifyCollectionChangedEventType``` の初期値は前述の ```WodiLibConfig``` の設定に準ずる。
            - ```NotifyCollectionChangingEventType``` および ```NotifyCollectionChangedEventType``` に設定する値の意味は [NotifyCollectionChangeEventType.cs](https://github.com/kameske/WodiLib/blob/v2.x/feature/WodiLib/WodiLib/Sys/Collections/Enum/NotifyCollectionChangeEventType.cs) を参照。
        - 通知する引数の実装を独自クラスである ```NotifyCollectionChangedEventArgsEx<T>``` (標準クラスである ```NotifyCollectionChangedEventArgs``` を継承) に変更。```NotifyCollectionChangedEventArgs``` との違いは以下のとおり。
            - ```OldItems```, ```NewItems``` が ```IList``` ではなく ```WodiLib.Sys.Collections.IReadOnlyExtendedList<T>``` で定義される。
            - ```Reset``` イベントが通知された際に変更前の要素が ```OldItems``` に格納される。

</details>

----------------------------------------

<details open>

<summary>これから対応予定のもの・順次対応中のもの</summary>

ターゲットフレームワークと言語バージョン
----------------------------------------

- langversion
    - ```9.0```

全域
----------------------------------------

- 不変なオブジェクト（値オブジェクト）を ```record``` 型に統一。
    - ```Ver 2.X``` 以前で ```struct``` 定義されていた値オブジェクトが影響を受ける。
    - 演算子や ```Equals``` の実装には変化なし。

- すべてのモデルクラスに "インタフェース" および "ReadOnly インタフェース" を定義。
    - インタフェースに定義されたプロパティやメソッドが扱うモデルはすべてインタフェースとする。

- ```ISerializable``` インタフェースを除去、 ```SerializableAttribute``` 付与を取りやめ。
    - ```.NET 5``` にて ```BinaryFormatter``` が非推奨となったため。

- モデルクラスが持つモデル型プロパティの```Setter```を極力除去。
    - ライブラリ外での意図しない操作を防ぐため。
    - 代替として状態を更新するためのメソッドを追加。

- モデルクラスのバイナリデータ化メソッド（```ToBinary()```等）をライブラリ内部に隠蔽。

- ```JsonSerialize``` 対応。

- 内部状態の一部を更新しながらディープコピーを作成する ```DeepCloneWith()``` メソッドの実装。
    - 引数や制約などは各インタフェース/クラスに依存する。

データベース周りの機能改修
----------------------------------------

- モデルクラスの構造変更
    - ```Ver 2.X``` 以前ではウディタが出力するバイナリデータを基準にした構造だったものを、ウディタ上で見えるような状態を基準にした構造に変更

</details>
