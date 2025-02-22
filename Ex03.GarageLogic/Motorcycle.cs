using System.Collections.Generic;

namespace Ex03.GarageLogic
{
    public class Motorcycle : Vehicle
    {
        private eLicenseType m_LicenseType = eLicenseType.A1;
        private int m_EngineVolume = 150;

        public eLicenseType LicenseType
        {
            get
            {
                return m_LicenseType;
            }
            set
            {
                m_LicenseType = value;
            }
        }

        public int EngineVolume
        {
            get
            {
                return m_EngineVolume;
            }
            set
            {
                m_EngineVolume = value;
            }
        }

        public Motorcycle(eEngineType i_EngineType) : base(2, 32f)
        {
            if (i_EngineType == eEngineType.Fuel)
            {
                SetEnergyData(0f, 6.2f, eEngineType.Fuel, eFuelType.Octan98);
            }
            else
            {
                SetEnergyData(0f, 2.9f, eEngineType.Electric);
            }
        }

        public override Dictionary<string, string> GetVehicleData()
        {
            Dictionary<string, string> data = base.GetVehicleData();

            data["License Type"] = m_LicenseType.ToString();
            data["Engine Volume"] = $"{m_EngineVolume}cc";

            return data;
        }

        public override List<VehicleFieldInfo> GetRequiredInfo()
        {
            List<VehicleFieldInfo> requiredInfo = base.GetRequiredInfo();

            requiredInfo.Add(new VehicleFieldInfo
            {
                FieldName = "LicenseType",
                InputPrompt = $"Select license type: ",
                FieldType = typeof(eLicenseType),
                IsEnum = true
            });
            requiredInfo.Add(new VehicleFieldInfo
            {
                FieldName = "EngineVolume",
                InputPrompt = $"Enter engine volume (cc): ",
                FieldType = typeof(int),
            });

            return requiredInfo;
        }
    }
}