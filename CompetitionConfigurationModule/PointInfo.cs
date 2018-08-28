using System;
using System.Collections.Generic;

namespace SCAS
{
    namespace CompetitionConfiguration
    {
        public class PointInfo : ICloneable
        {
            public const Double NoPointRate = 1.0;
            public const Double PointRateDisabled = .0;
            public static readonly List<UInt32> DefaultPoints = new List<UInt32> { 9, 7, 6, 5, 4, 3, 2, 1 };

            private List<UInt32> _points;
            private Double _pointRate;
            private bool _breakRecordPointRateEnabled;
            private Double _breakRecordPointRate;

            public List<UInt32> Points
            {
                get
                {
                    return _points;
                }
                set
                {
                    if (value.Count == 0)
                    {
                        throw new Exception("不能将积分列表设置为空列表");
                    }
                    _points = value;
                    _points.Sort();
                    _points.Reverse();
                }
            }

            public Double PointRate
            {
                get
                {
                    return _pointRate;
                }
                set
                {
                    if (value < .0)
                    {
                        throw new Exception("积分比例不能小于0");
                    }
                    _pointRate = value;
                }
            }

            public bool BreakRecordPointRateEnabled
            {
                get
                {
                    return _breakRecordPointRateEnabled;
                }
                set
                {
                    if (_breakRecordPointRateEnabled != value)
                    {
                        if (value)
                        {
                            SetBreakRecordPointRateEnabled();
                        }
                        else
                        {
                            SetBreakRecordPointRateDisabled();
                        }
                    }
                }
            }

            public Double BreakRecordPointRate
            {
                get
                {
                    return _breakRecordPointRate;
                }
                set
                {
                    if (value < .0)
                    {
                        throw new Exception("打破纪录时的积分比例不能小于0");
                    }
                    SetBreakRecordPointRateEnabled(value);
                }
            }

            public PointInfo()
            {
                _points = DefaultPoints;
                _pointRate = NoPointRate;
                SetBreakRecordPointRateDisabled();
            }

            public Object Clone()
            {
                PointInfo ret = new PointInfo
                {
                    _points = _points,
                    _pointRate = _pointRate,
                    _breakRecordPointRateEnabled = _breakRecordPointRateEnabled,
                    _breakRecordPointRate = _breakRecordPointRate
                };
                return ret;
            }

            public void SetBreakRecordPointRateEnabled(Double rate = NoPointRate)
            {
                _breakRecordPointRateEnabled = true;
                if (rate < .0)
                {
                    throw new Exception("打破记录的积分比例不能小于0");
                }
                _breakRecordPointRate = rate;
            }

            public void SetBreakRecordPointRateDisabled()
            {
                _breakRecordPointRateEnabled = false;
                _breakRecordPointRate = PointRateDisabled;
            }
        }
    };
};
