using System;

namespace Ex03.GarageLogic
{
    public class ValueOutOfRangeException : Exception
    {
        private float m_MinValue;
        private float m_MaxValue;

        public float MinValue
        {
            get
            {
                return m_MinValue;
            }
            private set
            {
                m_MinValue = value;
            }
        }

        public float MaxValue
        {
            get
            {
                return m_MaxValue;
            }
            private set
            {
                m_MaxValue = value;
            }
        }

        public ValueOutOfRangeException(string i_Message, float i_MinValue, float i_MaxValue) : base(i_Message)
        {
            MinValue = i_MinValue;
            MaxValue = i_MaxValue;
        }
    }
}