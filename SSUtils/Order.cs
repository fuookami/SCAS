using System;

namespace SSUtils
{
    public class Order : IComparable
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
    };
};
