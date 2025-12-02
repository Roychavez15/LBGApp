using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBGScore.Models
{
    public class Grouping<K, T> : ObservableCollection<T>
    {
        public K Key { get; }

        public Grouping(K key, IEnumerable<T> items) : base(items)
        {
            Key = key;
        }
    }
}
