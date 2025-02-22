using System.Collections.Generic;

namespace Ex03.GarageLogic
{
    public class Car : Vehicle
    {
        private eCarColor m_Color = eCarColor.Blue;
        private eNumOfDoors m_NumOfDoors = eNumOfDoors.Two;

        public eCarColor Color
        {
            get
            {
                return m_Color;
            }
            set
            {
                m_Color = value;
            }
        }

        public eNumOfDoors NumOfDoors
        {
            get
            {
                return m_NumOfDoors;
            }
            set
            {
                m_NumOfDoors = value;
            }
        }

        public Car(eEngineType i_EngineType) : base(5, 34f)
        {
            if (i_EngineType == eEngineType.Fuel)
            {
                SetEnergyData(0f, 52f, eEngineType.Fuel, eFuelType.Octan95);
            }
            else
            {
                SetEnergyData(0f, 5.4f, eEngineType.Electric);
            }
        }

        public override Dictionary<string, string> GetVehicleData()
        {
            Dictionary<string, string> data = base.GetVehicleData();

            data["Color"] = m_Color.ToString();
            data["Number of Doors"] = m_NumOfDoors.ToString();

            return data;
        }

        public override List<VehicleFieldInfo> GetRequiredInfo()
        {
            List<VehicleFieldInfo> requiredInfo = base.GetRequiredInfo();

            requiredInfo.Add(new VehicleFieldInfo
            {
                FieldName = "Color",
                InputPrompt = "Choose the car color:",
                FieldType = typeof(eCarColor),
                IsEnum = true
            });
            requiredInfo.Add(new VehicleFieldInfo
            {
                FieldName = "NumOfDoors",
                InputPrompt = "Choose the number of doors: ",
                FieldType = typeof(eNumOfDoors),
                IsEnum = true
            });

            return requiredInfo;
        }
    }
}
