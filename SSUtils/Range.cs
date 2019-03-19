using System;

namespace SSUtils
{
    public class Range<T>
        where T : IComparable
    {
        public T Minimum 
        { 
            get; 
            private set; 
        }

        public T Maximum 
        { 
            get; 
            private set; 
        }

        public Range(T min, T max)
        {
            Set(min, max);
        }

        public virtual bool Set(T min, T max)
        {
            if (!Valid(min, max))
            {
                return false;
            }

            Minimum = min;
            Maximum = max;
            return true;
        }
        
        public virtual bool Valid() => Valid(Minimum, Maximum);
        protected virtual bool Valid(T min, T max) => min.CompareTo(max) < 0;
    }

    public class UInt16Range : Range<UInt16>
    {
        public UInt16Range(UInt16 min = UInt16.MinValue, UInt16 max = UInt16.MaxValue)
            : base(min, max) {}
    }

    public class UInt32Range : Range<UInt32>
    {
        public UInt32Range(UInt32 min = UInt32.MinValue, UInt32 max = UInt32.MaxValue)
            : base(min, max) {}
    }

    public class UInt64Range : Range<UInt64>
    {
        public UInt64Range(UInt64 min = UInt64.MinValue, UInt64 max = UInt64.MaxValue)
            : base(min, max) {}
    }

    public class Int16Range : Range<Int16>
    {
        public Int16Range(Int16 min = Int16.MinValue, Int16 max = Int16.MaxValue)
            : base(min, max) {}
    }

    public class Int32Range : Range<Int32>
    {
        public Int32Range(Int32 min = Int32.MinValue, Int32 max = Int32.MaxValue)
            : base(min, max) {}
    }

    public class Int64Range : Range<Int64>
    {
        public Int64Range(Int64 min = Int64.MinValue, Int64 max = Int64.MaxValue)
            : base(min, max) {}
    }
}
