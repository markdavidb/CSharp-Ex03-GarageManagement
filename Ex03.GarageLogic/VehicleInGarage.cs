namespace Ex03.GarageLogic
{
    public class VehicleInGarage
    {
        private readonly Vehicle r_Vehicle;
        private readonly string r_OwnerName;
        private readonly string r_OwnerPhone;
        private eVehicleGarageStatus m_Status;

        public Vehicle Vehicle
        {
            get
            {
                return r_Vehicle;
            }
        }

        public string OwnerName
        {
            get
            {
                return r_OwnerName;
            }
        }

        public string OwnerPhone
        {
            get
            {
                return r_OwnerPhone;
            }
        }

        public eVehicleGarageStatus Status
        {
            get
            {
                return m_Status;
            }
            set
            {
                m_Status = value;
            }
        }

        public VehicleInGarage(Vehicle i_Vehicle, string i_OwnerName, string i_OwnerPhone)
        {
            r_Vehicle = i_Vehicle;
            r_OwnerName = i_OwnerName;
            r_OwnerPhone = i_OwnerPhone;
            m_Status = eVehicleGarageStatus.InRepair;
        }
    }
}