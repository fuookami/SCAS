using System;

namespace CompetitionConfigurationModule
{
    public class TeamworkInfo
    {
        public const Int32 NoTeamwork = -2;
        public const Int32 NotNeedEveryPerson = -1;
        public const Int32 NoLimit = 0;

        private bool isTeamwork;
        private bool needEveryPerson;
        private Int32 minMumberOfPeople;
        private Int32 maxNumberOfPeople;

        public bool BeTeamwork
        {
            get { return isTeamwork; }
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
            get { return MinNumberOfPeople; }
            set { SetNeedEveryPerson(value, maxNumberOfPeople); }
        }

        public Int32 MaxNumberOfPeople
        {
            get { return maxNumberOfPeople; }
            set { SetNeedEveryPerson(minMumberOfPeople, value); }
        }

        public TeamworkInfo()
        {
            SetIsNotTeamwork();
        }

        public void SetIsTeamwork()
        {
            if (!isTeamwork)
            {
                isTeamwork = true;
                needEveryPerson = false;
                minMumberOfPeople = NotNeedEveryPerson;
                maxNumberOfPeople = NotNeedEveryPerson;
            }
        }

        public void SetIsNotTeamwork()
        {
            isTeamwork = false;
            needEveryPerson = false;
            minMumberOfPeople = NoTeamwork;
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

            if (!isTeamwork)
            {
                SetIsTeamwork();
            }
            needEveryPerson = true;
            minMumberOfPeople = minNumber;
            maxNumberOfPeople = maxNumber;
        }

        public void SetNotNeedEveryPerson()
        {
            needEveryPerson = false;
            minMumberOfPeople = NotNeedEveryPerson;
            maxNumberOfPeople = NotNeedEveryPerson;
        }
    }
}
