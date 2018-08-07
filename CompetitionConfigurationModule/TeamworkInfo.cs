using System;

namespace CompetitionConfigurationModule
{
    public class TeamworkInfo
    {
        public const Int32 NoTeamwork = -2;
        public const Int32 NotNeedEveryPerson = -1;
        public const Int32 NoLimit = 0;

        private bool beTeamwork;
        private bool beMultiRank;
        private bool needEveryPerson;
        private Int32 minNumberOfPeople;
        private Int32 maxNumberOfPeople;

        public bool BeTeamwork
        {
            get { return beTeamwork; }
            set
            {
                if (value)
                {
                    SetIsTeamwork();
                }
                else
                {
                    SetIsNotTeamwork();
                }
            }
        }

        public bool BeMultiRank
        {
            get { return beMultiRank; }
            set
            {
                if (BeTeamwork)
                {
                    beMultiRank = value;
                }
            }
        }

        public bool NeedEveryPerson
        {
            get { return needEveryPerson; }
            set
            {
                if (value)
                {
                    SetNeedEveryPerson();
                }
                else
                {
                    SetNotNeedEveryPerson();
                }
            }
        }

        public Int32 MinNumberOfPeople
        {
            get { return minNumberOfPeople; }
            set { SetNeedEveryPerson(value, maxNumberOfPeople); }
        }

        public Int32 MaxNumberOfPeople
        {
            get { return maxNumberOfPeople; }
            set { SetNeedEveryPerson(minNumberOfPeople, value); }
        }

        public TeamworkInfo()
        {
            SetIsNotTeamwork();
        }

        public void SetIsTeamwork()
        {
            if (!beTeamwork)
            {
                beTeamwork = true;
                beMultiRank = false;
                needEveryPerson = false;
                minNumberOfPeople = NotNeedEveryPerson;
                maxNumberOfPeople = NotNeedEveryPerson;
            }
        }

        public void SetIsNotTeamwork()
        {
            beTeamwork = false;
            beMultiRank = false;
            needEveryPerson = false;
            minNumberOfPeople = NoTeamwork;
            maxNumberOfPeople = NoTeamwork;
        }

        public void SetNeedEveryPerson(Int32 minNumber = NoLimit, Int32 maxNumber = NoLimit)
        {
            if (maxNumber != NoLimit && maxNumber < minNumber)
            {
                throw new Exception("最少需要的人数不能大于最大需要的人数");
            }
            if (maxNumber < NotNeedEveryPerson || minNumber < NotNeedEveryPerson)
            {
                throw new Exception("最少需要的人数或最大需要的人数是个无效值");
            }

            if (!beTeamwork)
            {
                SetIsTeamwork();
            }
            needEveryPerson = true;
            minNumberOfPeople = minNumber;
            maxNumberOfPeople = maxNumber;
        }

        public void SetNotNeedEveryPerson()
        {
            needEveryPerson = false;
            minNumberOfPeople = NotNeedEveryPerson;
            maxNumberOfPeople = NotNeedEveryPerson;
        }
    }
}
