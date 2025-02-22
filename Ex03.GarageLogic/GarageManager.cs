using System;
using System.Collections.Generic;

namespace Ex03.GarageLogic
{
    public class GarageManager
    {
        private readonly Dictionary<string, VehicleInGarage> r_Vehicles;

        public GarageManager()
        {
            r_Vehicles = new Dictionary<string, VehicleInGarage>();
        }

        public static eEngineType[] GetPossibleEngineTypes(string i_VehicleType)
        {
            eEngineType[] engineTypes = Array.Empty<eEngineType>();

            if (VehicleFactory.sr_TypeToEnginesMap.TryGetValue(i_VehicleType, out eEngineType[] types))
            {
                engineTypes = types;
            }

            return engineTypes;
        }

        public Dictionary<string, string> GetVehicleFullDetails(string i_LicenseNumber)
        {
            VehicleInGarage vehicleInGarage = getVehicleInGarage(i_LicenseNumber);
            Dictionary<string, string> vehicleData = vehicleInGarage.Vehicle.GetVehicleData();

            vehicleData["Owner Name"] = vehicleInGarage.OwnerName;
            vehicleData["Owner Phone"] = vehicleInGarage.OwnerPhone;
            vehicleData["Status"] = vehicleInGarage.Status.ToString();

            return vehicleData;
        }

        public void InsertNewVehicle(Vehicle i_Vehicle, string i_OwnerName, string i_OwnerPhone)
        {
            r_Vehicles.Add(i_Vehicle.LicenseNumber, new VehicleInGarage(i_Vehicle, i_OwnerName, i_OwnerPhone));
        }

        public List<string> GetLicenseNumbers(eVehicleGarageStatus? i_FilterByStatus = null)
        {
            List<string> licenseNumbers = new List<string>();

            foreach (KeyValuePair<string, VehicleInGarage> kvp in r_Vehicles)
            {
                if (!i_FilterByStatus.HasValue || kvp.Value.Status == i_FilterByStatus.Value)
                {
                    licenseNumbers.Add(kvp.Key);
                }
            }

            return licenseNumbers;
        }

        public void ChangeVehicleStatus(string i_LicenseNumber, eVehicleGarageStatus i_NewStatus)
        {
            VehicleInGarage vehicle = getVehicleInGarage(i_LicenseNumber);

            vehicle.Status = i_NewStatus;
        }

        public void InflateWheelsToMax(string i_LicenseNumber)
        {
            VehicleInGarage vehicleEntry = getVehicleInGarage(i_LicenseNumber);

            foreach (Wheel wheel in vehicleEntry.Vehicle.Wheels)
            {
                wheel.InflateToMax();
            }
        }

        private VehicleInGarage getVehicleInGarage(string i_LicenseNumber)
        {
            if (!r_Vehicles.TryGetValue(i_LicenseNumber, out VehicleInGarage vehicleInGarage))
            {
                throw new KeyNotFoundException("The vehicle is not found in the garage.");
            }

            return vehicleInGarage;
        }

        private bool isVehicleExists(string i_LicenseNumber)
        {
            return !string.IsNullOrEmpty(i_LicenseNumber) && r_Vehicles.ContainsKey(i_LicenseNumber);
        }


        public void RefuelVehicle(string i_LicenseNumber, eFuelType i_FuelType, float i_FuelAmount)
        {
            VehicleInGarage vehicleEntry = getVehicleInGarage(i_LicenseNumber);
            Vehicle vehicle = vehicleEntry.Vehicle;

            if (vehicle.EnergyData.FuelType != i_FuelType)
            {
                throw new ArgumentException($"Incorrect fuel type. Expected: {vehicle.EnergyData.FuelType}, provided: {i_FuelType}.");
            }

            vehicle.AddEnergy(i_FuelAmount);
        }

        public void ChargeVehicle(string i_LicenseNumber, float i_HoursToCharge)
        {
            VehicleInGarage vehicleEntry = getVehicleInGarage(i_LicenseNumber);
            Vehicle vehicle = vehicleEntry.Vehicle;

            vehicle.AddEnergy(i_HoursToCharge);
        }

        private eEngineType? getVehicleEngineType(string i_LicenseNumber)
        {
            eEngineType? engineType = null;

            if (r_Vehicles.TryGetValue(i_LicenseNumber, out VehicleInGarage vehicle))
            {
                engineType = vehicle.Vehicle.EnergyData.EngineType;
            }

            return engineType;
        }

        public void ValidateVehicle(string i_LicenseNumber, eEngineType? i_RequiredEngineType = null)
        {
            if (!isVehicleExists(i_LicenseNumber))
            {
                throw new KeyNotFoundException($"No vehicle with license number {i_LicenseNumber} found in the garage.");
            }

            if (i_RequiredEngineType.HasValue)
            {
                eEngineType? vehicleEngineType = getVehicleEngineType(i_LicenseNumber);

                if (!vehicleEngineType.HasValue || vehicleEngineType.Value != i_RequiredEngineType.Value)
                {
                    string requiredTypeName = i_RequiredEngineType.Value == eEngineType.Fuel ? "a fuel-based" : "an electric";

                    throw new ArgumentException($"The vehicle with license number {i_LicenseNumber} is not {requiredTypeName} vehicle.");
                }
            }
        }

        public bool TryUpdateStatusIfVehicleExists(string i_LicenseNumber)
        {
            bool isUpdated = false;

            if (isVehicleExists(i_LicenseNumber))
            {
                ChangeVehicleStatus(i_LicenseNumber, eVehicleGarageStatus.InRepair);
                isUpdated = true;
            }

            return isUpdated;
        }
    }
}
