namespace System.Collections.Generic
{
    public class Map<T1, T2> 
    {
        private Dictionary<T1, T2> _left;
        private Dictionary<T2, T1> _right;
        
        public Indexer<T1, T2> Left
        {
            get;
            private set;
        }

        public Indexer<T2, T1> Right
        {
            get;
            private set;
        }

        public Map()
        {
            _left = new Dictionary<T1, T2>();
            _right = new Dictionary<T2, T1>();
            Left = new Indexer<T1, T2>(_left);
            Right = new Indexer<T2, T1>(_right);
        }

        public class Indexer<T3, T4>
        {
            private Dictionary<T3, T4> _dictionary;

            public Indexer(Dictionary<T3, T4> dictionary)
            {
                _dictionary = dictionary;
            }

            public T4 this[T3 index]
            {
                get { return _dictionary[index]; }
                set { _dictionary[index] = value; }
            }

            public bool ContainsKey(T3 key)
            {
                return _dictionary.ContainsKey(key);
            }

            public bool ContainsValue(T4 value)
            {
                return _dictionary.ContainsValue(value);
            }
        }

        public void Add(T1 t1, T2 t2)
        {
            _left.Add(t1, t2);
            _right.Add(t2, t1);
        }

        public void Remove(T1 t1Key)
        {
            T2 value = _left[t1Key];
            if (value != null)
            {
                _left.Remove(t1Key);
                _right.Remove(value);
            }
        }

        public void Remove(T2 t2Key)
        {
            T1 value = _right[t2Key];
            if (value != null)
            {
                _left.Remove(value);
                _right.Remove(t2Key);
            }
        }

        public void Clear()
        {
            _left.Clear();
            _right.Clear();
        }
    };
};
