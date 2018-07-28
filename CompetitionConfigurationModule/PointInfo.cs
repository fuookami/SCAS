using System;
using System.Collections.Generic;

namespace CompetitionConfigurationModule
{
    public class PointInfo
    {
        public const Double NoPointRate = 1.0;
        public const Double PointRateDisabled = .0;
        public static readonly List<UInt32> DefaultPoints = new List<UInt32> { 9, 7, 6, 5, 4, 3, 2, 1 };

        private List<UInt32> points;
        private Double pointRate;
        private bool breakRecordPointRateEnabled;
        private Double breakRecordPointRate;

        public List<UInt32> Points
        {
            get { return points; }
            set
            {
                if (value.Count == 0)
                {
                    throw new Exception("不能将积分列表设置为空列表");
                }
                points = value;
                points.Sort();
                points.Reverse();
            }
        }

        public Double PointRate
        {
            get { return pointRate; }
            set
            {
                if (value < .0)
                {
                    throw new Exception("积分比例不能小于0");
                }
                pointRate = value;
            }
        }

        public bool BreakRecordPointRateEnabled
        {
            get { return breakRecordPointRateEnabled; }
            set
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

        public Double BreakRecordPointRate
        {
            get { return breakRecordPointRate; }
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
            points = DefaultPoints;
            pointRate = NoPointRate;
            SetBreakRecordPointRateDisabled();
        }

        public void SetBreakRecordPointRateEnabled(Double rate = NoPointRate)
        {
            breakRecordPointRateEnabled = true;
            if (rate < .0)
            {
                throw new Exception("打破记录的积分比例不能小于0");
            }
            breakRecordPointRate = rate;
        }

        public void SetBreakRecordPointRateDisabled()
        {
            breakRecordPointRateEnabled = false;
            breakRecordPointRate = PointRateDisabled;
        }
    }
}
