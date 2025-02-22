using System;
using System.Collections.Generic;

namespace Ex03.GarageLogic
{
    public static class VehicleFactory
    {
        public static readonly Dictionary<string, eEngineType[]> sr_TypeToEnginesMap = initializeEngineMap();

        private static Dictionary<string, eEngineType[]> initializeEngineMap()
        {
            Dictionary<string, eEngineType[]> engineMap = new Dictionary<string, eEngineType[]>();

            engineMap.Add("Car", new[] { eEngineType.Fuel, eEngineType.Electric });
            engineMap.Add("Motorcycle", new[] { eEngineType.Fuel, eEngineType.Electric });
            engineMap.Add("Truck", new[] { eEngineType.Fuel });

            return engineMap;
        }

        public static Vehicle CreateVehicle(string i_VehicleType, eEngineType i_EngineType)
        {
            Vehicle newVehicle;

            switch (i_VehicleType)
            {
                case "Car":
                    newVehicle = new Car(i_EngineType);
                    break;
                case "Motorcycle":
                    newVehicle = new Motorcycle(i_EngineType);
                    break;
                case "Truck":
                    newVehicle = new Truck();
                    break;
                default:
                    throw new ArgumentException($"Unsupported vehicle type: {i_VehicleType}");
            }

            return newVehicle;
        }
    }
}