using System;

namespace SSUtils
{
    public struct Order : IComparable, IEquatable<Int32>, IEquatable<UInt32>, IEquatable<Order>
    {
        public const Int32 NotSet = -1;
        private Int32 _order;

        public Int32 Value
        {
            get { return _order; }
            set { _order = value >= 0 ? value : NotSet; }
        }

        public Order(Int32 order = NotSet)
        {
            _order = order;
        }

        public bool Valid() => Value != NotSet;

        public int CompareTo(object obj)
        {
            Order rhs = (Order)obj;
            return Valid() && rhs.Valid() ? Value.CompareTo(rhs.Value)
                : Valid() ? Int32.MaxValue 
                : rhs.Valid() ? Int32.MinValue : 0;
        }

        public bool Equals(Int32 rhs) => Valid() && Value == rhs;
        public bool Equals(UInt32 rhs) => Equals((Int32)rhs);
        public bool Equals(Order rhs) => Valid() && rhs.Valid() ? Value == rhs.Value : (!Valid() && !rhs.Valid());
        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case Int32 value:
                    return Equals(value);
                case UInt32 value:
                    return Equals(value);
                case Order value:
                    return Equals(value);
                default:
                    return base.Equals(obj);
            }
        }

        public override int GetHashCode() => Value;

        public static bool operator ==(Order lhs, Int32 rhs) => lhs.Equals(rhs);
        public static bool operator !=(Order lhs, Int32 rhs) => !lhs.Equals(rhs);
    };
};
