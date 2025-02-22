namespace Ex03.GarageLogic
{
    public class EnergyData
    {
        private float m_CurrentAmount;
        private float m_MaxAmount;
        private eEngineType m_EngineType;
        private eFuelType? m_FuelType;

        public float CurrentAmount
        {
            get
            {
                return m_CurrentAmount;
            }
            set
            {
                m_CurrentAmount = value;
            }
        }

        public float MaxAmount
        {
            get
            {
                return m_MaxAmount;
            }
            set
            {
                m_MaxAmount = value;
            }
        }

        public eEngineType EngineType
        {
            get
            {
                return m_EngineType;
            }
            set
            {
                m_EngineType = value;
            }
        }

        public eFuelType? FuelType
        {
            get
            {
                return m_FuelType;
            }
            set
            {
                m_FuelType = value;
            }
        }

        public EnergyData(float i_CurrentAmount, float i_MaxAmount, eEngineType i_EngineType, eFuelType? i_FuelType = null)
        {
            m_CurrentAmount = i_CurrentAmount;
            m_MaxAmount = i_MaxAmount;
            m_EngineType = i_EngineType;
            m_FuelType = i_FuelType;
        }

        public void AddAmount(float i_AmountToAdd)
        {
            if (m_CurrentAmount + i_AmountToAdd > m_MaxAmount)
            {
                if (EngineType == eEngineType.Electric)
                {
                    throw new ValueOutOfRangeException($"Battery amount exceeds the max battery capacity. You can charge up to {m_MaxAmount - m_CurrentAmount} hours.", 0, m_MaxAmount - m_CurrentAmount);
                }

                if (EngineType == eEngineType.Fuel)
                {
                    throw new ValueOutOfRangeException($"Fuel amount exceeds the tank capacity. You can add up to {m_MaxAmount - m_CurrentAmount} liters.", 0, m_MaxAmount - m_CurrentAmount);
                }
            }

            m_CurrentAmount += i_AmountToAdd;
        }
    }
}