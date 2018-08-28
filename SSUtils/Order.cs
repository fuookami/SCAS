using System;

namespace SSUtils
{
    public struct Order : IComparable, IEquatable<Int32>, IEquatable<UInt32>, IEquatable<Order>
    {
        public const Int32 NotSet = -1;

        private Int32 _order;

        public Int32 Value
        {
            get
            {
                return _order;
            }
            set
            {
                _order = value >= 0 ? value : NotSet;
            }
        }

        public Order(Int32 order = NotSet)
        {
            _order = order;
        }

        public bool Valid()
        {
            return Value != NotSet;
        }

        public int CompareTo(object obj)
        {
            Order rhs = (Order)obj;
            return Valid() && rhs.Valid() ? Value.CompareTo(rhs.Value)
                : Valid() ? Int32.MaxValue 
                : rhs.Valid() ? Int32.MinValue : 0;
        }

        public bool Equals(Int32 rhs)
        {
            return Valid() && Value == rhs;
        }

        public bool Equals(UInt32 rhs)
        {
            return Equals((Int32)rhs);
        }

        public bool Equals(Order rhs)
        {
            if (Valid() && rhs.Valid())
            {
                return Value == rhs.Value;
            }
            return !Valid() && !rhs.Valid();
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(Int32))
            {
                return Equals((Int32)obj);
            }
            else if (obj.GetType() == typeof(UInt32))
            {
                return Equals((UInt32)obj);
            }
            else if (obj.GetType() == typeof(Order))
            {
                return Equals((Order)obj);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Value;
        }

        public static bool operator ==(Order lhs, Int32 rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Order lhs, Int32 rhs)
        {
            return !lhs.Equals(rhs);
        }
    };
};
