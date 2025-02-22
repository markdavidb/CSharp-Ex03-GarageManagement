using System.Collections.Generic;

namespace Ex03.GarageLogic
{
    public class Truck : Vehicle
    {
        private bool m_IsCooledCargo = false;
        private float m_CargoVolume = 0f;

        public bool IsCooledCargo
        {
            get
            {
                return m_IsCooledCargo;
            }
            set
            {
                m_IsCooledCargo = value;
            }
        }

        public float CargoVolume
        {
            get
            {
                return m_CargoVolume;
            }
            set
            {
                m_CargoVolume = value;
            }
        }

        public Truck() : base(14, 29f)
        {
            SetEnergyData(0f, 125f, eEngineType.Fuel, eFuelType.Soler);
        }

        public override Dictionary<string, string> GetVehicleData()
        {
            Dictionary<string, string> data = base.GetVehicleData();

            data["Is Cooled Cargo"] = m_IsCooledCargo ? "Yes" : "No";
            data["Cargo Volume"] = $"{m_CargoVolume} liters";

            return data;
        }

        public override List<VehicleFieldInfo> GetRequiredInfo()
        {
            List<VehicleFieldInfo> requiredInfo = base.GetRequiredInfo();

            requiredInfo.Add(new VehicleFieldInfo
            {
                FieldName = "IsCooledCargo",
                InputPrompt = $"Is there cooled cargo? (Y/N) or (YES/NO): ",
                FieldType = typeof(bool),
                IsEnum = false
            });
            requiredInfo.Add(new VehicleFieldInfo
            {
                FieldName = "CargoVolume",
                InputPrompt = $"Enter cargo volume (liters): ",
                FieldType = typeof(float),
            });

            return requiredInfo;
        }
    }
}