using System.Collections.Generic;

namespace Ex03.GarageLogic
{
    public abstract class Vehicle
    {
        private string m_ModelName = string.Empty;
        private string m_LicenseNumber = string.Empty;
        private float m_RemainingEnergyPercentage = 0f;
        private List<Wheel> m_Wheels = new List<Wheel>();
        private EnergyData m_EnergyData = null;

        public string ModelName
        {
            get
            {
                return m_ModelName;
            }
            set
            {
                m_ModelName = value;
            }
        }

        public string LicenseNumber
        {
            get
            {
                return m_LicenseNumber;
            }
            set
            {
                m_LicenseNumber = value;
            }
        }

        public List<Wheel> Wheels
        {
            get
            {
                return m_Wheels;
            }
        }

        public float CurrentEnergy
        {
            get
            {
                return m_EnergyData.CurrentAmount;
            }
            set
            {
                m_EnergyData.CurrentAmount = value;
            }
        }

        public float MaxEnergy
        {
            get
            {
                return m_EnergyData.MaxAmount;
            }
        }

        public EnergyData EnergyData
        {
            get
            {
                return m_EnergyData;
            }
            set
            {
                m_EnergyData = value;
            }
        }

        public string WheelManufacturerName
        {
            get
            {
                return m_Wheels[0].ManufacturerName;
            }
            set
            {
                foreach (Wheel wheel in m_Wheels)
                {
                    wheel.ManufacturerName = value;
                }
            }
        }

        public float RemainingEnergyPercentage
        {
            get
            {
                if (MaxEnergy == 0)
                {
                    m_RemainingEnergyPercentage = 0f;
                }
                else
                {
                    m_RemainingEnergyPercentage = (CurrentEnergy / MaxEnergy) * 100f;
                }

                return m_RemainingEnergyPercentage;
            }
        }

        public float WheelMaxAirPressure
        {
            get
            {
                return m_Wheels[0].MaxAirPressure;
            }
        }

        public float WheelCurrentAirPressure
        {
            get
            {
                return m_Wheels[0].CurrentAirPressure;
            }
            set
            {
                foreach (Wheel wheel in m_Wheels)
                {
                    wheel.CurrentAirPressure = value;
                }
            }
        }

        protected Vehicle(int i_NumberOfWheels, float i_WheelMaxAirPressure)
        {
            InitializeWheels(i_NumberOfWheels, i_WheelMaxAirPressure);
        }

        protected void InitializeWheels(int i_NumberOfWheels, float i_WheelMaxAirPressure)
        {
            m_Wheels = new List<Wheel>(i_NumberOfWheels);

            for (int i = 0; i < i_NumberOfWheels; i++)
            {
                m_Wheels.Add(new Wheel(string.Empty, i_WheelMaxAirPressure, 0f));
            }
        }

        protected void SetEnergyData(float i_Current, float i_Max, eEngineType i_EngineType, eFuelType? i_FuelType = null)
        {
            m_EnergyData = new EnergyData(i_Current, i_Max, i_EngineType, i_FuelType);
        }

        public void AddEnergy(float i_AmountToAdd)
        {
            m_EnergyData.AddAmount(i_AmountToAdd);
        }

        public virtual Dictionary<string, string> GetVehicleData()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();

            data["Vehicle Type"] = this.GetType().Name;
            data["Model Name"] = m_ModelName;
            data["License Number"] = m_LicenseNumber;
            data["Energy"] = $"{m_EnergyData.EngineType}";
            data["Remaining Energy Percentage"] = $"{RemainingEnergyPercentage:F2}%";

            if (m_EnergyData.EngineType == eEngineType.Fuel)
            {
                data["Current Fuel Amount"] = $"{CurrentEnergy} liters ({CurrentEnergy}/{MaxEnergy})";
                data["Max Fuel Amount"] = $"{MaxEnergy} liters";
                data["Fuel Type"] = m_EnergyData.FuelType.ToString();
            }
            else if (m_EnergyData.EngineType == eEngineType.Electric)
            {
                data["Battery Time Left"] = $"{CurrentEnergy} hours";
                data["Max Battery Time"] = $"{MaxEnergy} hours";
            }

            for (int i = 0; i < m_Wheels.Count; i++)
            {
                data[$"Wheel {i + 1} Manufacturer"] = WheelManufacturerName;
                data[$"Wheel {i + 1} Current Air Pressure"] = WheelCurrentAirPressure.ToString();
                data[$"Wheel {i + 1} Max Air Pressure"] = WheelMaxAirPressure.ToString();
            }

            return data;
        }

        public virtual List<VehicleFieldInfo> GetRequiredInfo()
        {
            List<VehicleFieldInfo> requiredInfo = new List<VehicleFieldInfo>();

            requiredInfo.Add(new VehicleFieldInfo
            {
                FieldName = "ModelName",
                InputPrompt = "Enter the model name: ",
                FieldType = typeof(string),
            });
            requiredInfo.Add(new VehicleFieldInfo
            {
                FieldName = "WheelManufacturerName",
                InputPrompt = "Enter the wheel manufacturer name: ",
                FieldType = typeof(string),
            });
            requiredInfo.Add(new VehicleFieldInfo
            {
                FieldName = "WheelCurrentAirPressure",
                InputPrompt = $"Enter the current air pressure for the wheels (Max: {WheelMaxAirPressure}): ",
                FieldType = typeof(float),
                MinValue = 0f,
                MaxValue = this.WheelMaxAirPressure
            });
            requiredInfo.Add(new VehicleFieldInfo
            {
                FieldName = "CurrentEnergy",
                InputPrompt = this.EnergyData.EngineType == eEngineType.Fuel
                                                       ? $"Enter the current fuel amount (0 - {this.MaxEnergy} liters): "
                                                       : $"Enter the battery time left (0 - {this.MaxEnergy} hours): ",
                FieldType = typeof(float),
                MinValue = 0f,
                MaxValue = this.MaxEnergy
            });

            return requiredInfo;
        }
    }
}
