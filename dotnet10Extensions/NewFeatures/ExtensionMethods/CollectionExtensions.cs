using System;
using System.Collections.Generic;
using System.Text;

namespace NewFeatures.ExtensionMethods;

public static class CollectionExtensions
{
    extension<T>(IEnumerable<T> source)
    {
        public bool IsEmpty() => !source.Any();

        public bool HasItems() => source.Any();

        public int ItemCount => source.Count();

        //private List<T>? _materializedList;

        //public List<T> MaterializedList => _materializedList ??= source.ToList();

        //public bool IsEmpty => MaterializedList.Count == 0;
    }
}
