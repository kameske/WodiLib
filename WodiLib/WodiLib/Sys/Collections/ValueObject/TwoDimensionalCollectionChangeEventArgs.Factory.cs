
namespace WodiLib.Sys
{
    public partial class TwoDimensionalCollectionChangeEventArgs<T>
    {
        internal static class Factory
        {
            public static TwoDimensionalCollectionChangeEventArgs<T> Set(int row, int column,
                T[][] oldItems, T[][] newItems)
                => new TwoDimensionalCollectionChangeEventArgs<T>(row, column,
                    oldItems, newItems);

            public static TwoDimensionalCollectionChangeEventArgs<T> AddRow(int row, T[][] items)
                => new TwoDimensionalCollectionChangeEventArgs<T>(row,
                    items, TwoDimensionalCollectionChangeAction.Add, Direction.Row);

            public static TwoDimensionalCollectionChangeEventArgs<T> AddColumn(int column, T[][] items)
                => new TwoDimensionalCollectionChangeEventArgs<T>(column,
                    items, TwoDimensionalCollectionChangeAction.Add, Direction.Column);

            public static TwoDimensionalCollectionChangeEventArgs<T> MoveRow(int oldRow, int newRow, T[][] items)
                => new TwoDimensionalCollectionChangeEventArgs<T>(oldRow, newRow,
                    items, Direction.Row);

            public static TwoDimensionalCollectionChangeEventArgs<T> MoveColumn(int oldColumn, int newColumn,
                T[][] items)
                => new TwoDimensionalCollectionChangeEventArgs<T>(oldColumn, newColumn,
                    items, Direction.Column);

            public static TwoDimensionalCollectionChangeEventArgs<T> RemoveRow(int row, T[][] items)
                => new TwoDimensionalCollectionChangeEventArgs<T>(row,
                    items, TwoDimensionalCollectionChangeAction.Remove, Direction.Row);

            public static TwoDimensionalCollectionChangeEventArgs<T> RemoveColumn(int column, T[][] items)
                => new TwoDimensionalCollectionChangeEventArgs<T>(column,
                    items, TwoDimensionalCollectionChangeAction.Remove, Direction.Column);

            public static TwoDimensionalCollectionChangeEventArgs<T> Reset()
                => new TwoDimensionalCollectionChangeEventArgs<T>();
        }
    }
}
