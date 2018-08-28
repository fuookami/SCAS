using System;

namespace SSUtils
{
    public class NumberRange : UInt32Range
    {
        public const UInt32 Nolimit = 0;

        public NumberRange(UInt32 min = 0, UInt32 max = Nolimit)
            : base(min, max)
        {
        }

        public bool Set(UInt32 number)
        {
            return Set(number, number);
        }

        public override bool Valid()
        {
            return Valid(Minimun, Maximun);
        }

        protected override bool Valid(UInt32 min, UInt32 max)
        {
            return min == 0
                || min <= max;
        }
    }

    public class EnabledNumberRange
    {
        private NumberRange _range;
        
        public NumberRange Range
        {
            get
            {
                return _range;
            }
            set
            {
                SetEnabled(value);
            }
        }

        public EnabledNumberRange()
        {
            SetDisabled();
        }

        public EnabledNumberRange(NumberRange range)
        {
            SetEnabled(range);
        }

        public EnabledNumberRange(UInt32 min, UInt32 max)
        {
            SetEnabled(min, max);
        }

        public bool IsEnabled()
        {
            return _range != null;
        }

        public bool SetEnabled()
        {
            return SetEnabled(new NumberRange());
        }

        public bool SetEnabled(UInt32 number)
        {
            return SetEnabled(new NumberRange(number, number));
        }

        public bool SetEnabled(UInt32 min, UInt32 max)
        {
            return SetEnabled(new NumberRange(min, max));
        }

        public bool SetEnabled(NumberRange range)
        {
            _range = range ?? throw new Exception("设置的范围值是个无效值");
            return true;
        }

        public bool SetDisabled()
        {
            _range = null;
            return true;
        }
    }
}
