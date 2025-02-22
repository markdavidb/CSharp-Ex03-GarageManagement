using Ex03.GarageLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ex03.ConsoleUI
{
    public class GarageUI
    {
        private readonly GarageManager r_GarageManager = new GarageManager();

        public void Run()
        {
            bool isExitRequested = false;

            while (!isExitRequested)
            {
                bool isValidChoice = false;

                while (!isValidChoice)
                {
                    printMenu();
                    string userInput = Console.ReadLine();

                    Console.Clear();

                    switch (userInput)
                    {
                        case "1":
                            insertNewVehicle();
                            isValidChoice = true;
                            break;
                        case "2":
                            displayLicenseNumbers();
                            isValidChoice = true;
                            break;
                        case "3":
                            changeVehicleStatus();
                            isValidChoice = true;
                            break;
                        case "4":
                            inflateWheels();
                            isValidChoice = true;
                            break;
                        case "5":
                            refuelVehicle();
                            isValidChoice = true;
                            break;
                        case "6":
                            chargeVehicle();
                            isValidChoice = true;
                            break;
                        case "7":
                            displayVehicleDetails();
                            isValidChoice = true;
                            break;
                        case "Q":
                        case "q":
                            isExitRequested = true;
                            isValidChoice = true;
                            Console.WriteLine("Exiting the garage system...");
                            break;
                        default:
                            Console.WriteLine("Invalid choice, please choose again from the menu");
                            Console.WriteLine("Press any key to continue...");
                            Console.ReadKey();
                            Console.Clear();
                            break;
                    }
                }

                if (!isExitRequested)
                {
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        private void printMenu()
        {
            StringBuilder mainMenu = new StringBuilder();

            mainMenu.AppendLine("=== Garage Management System ===");
            mainMenu.AppendLine("1. Insert New Vehicle");
            mainMenu.AppendLine("2. Display License Numbers of Vehicles in Garage");
            mainMenu.AppendLine("3. Change Vehicle Status in Garage");
            mainMenu.AppendLine("4. Inflate All Wheels to Maximum");
            mainMenu.AppendLine("5. Refuel a Fuel-Based Vehicle");
            mainMenu.AppendLine("6. Charge an Electric Vehicle");
            mainMenu.AppendLine("7. Display Vehicle Details by License Number");
            mainMenu.AppendLine("Q. Quit");
            mainMenu.AppendLine("=================================");
            mainMenu.Append("Please choose an option: ");

            Console.Write(mainMenu.ToString());
        }

        private void collectFieldsFromUser(Vehicle i_Vehicle)
        {
            List<VehicleFieldInfo> fields = i_Vehicle.GetRequiredInfo();
            Dictionary<string, object> collectedValues = new Dictionary<string, object>();

            foreach (VehicleFieldInfo fieldMeta in fields)
            {
                bool isValidInput = false;
                object convertedValue = null;

                while (!isValidInput)
                {
                    try
                    {
                        if (fieldMeta.IsEnum)
                        {
                            Console.WriteLine(fieldMeta.InputPrompt);
                            convertedValue = getEnumInput(fieldMeta.FieldType);
                        }
                        else
                        {
                            Console.Write(fieldMeta.InputPrompt);
                            string userInput = Console.ReadLine();

                            convertedValue = Validator.ConvertUserInput(userInput, fieldMeta);
                        }

                        isValidInput = true;
                    }
                    catch (FormatException fe)
                    {
                        Console.WriteLine($"Invalid format: {fe.Message}. Please try again.");
                    }
                    catch (ValueOutOfRangeException voore)
                    {
                        Console.WriteLine($"Value out of range: {voore.Message} {Environment.NewLine}Please try again.");
                    }
                    catch (NotSupportedException nse)
                    {
                        Console.WriteLine($"Not supported: {nse.Message}.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                    }
                }

                collectedValues[fieldMeta.FieldName] = convertedValue;
            }

            Type vehicleType = i_Vehicle.GetType();

            foreach (KeyValuePair<string, object> kvp in collectedValues)
            {
                PropertyInfo prop = vehicleType.GetProperty(kvp.Key);

                if (prop != null)
                {
                    prop.SetValue(i_Vehicle, kvp.Value);
                }
            }
        }

        private void insertNewVehicle()
        {
            try
            {
                Console.WriteLine("=== Inserting New Vehicle ===");
                string licenseNumber = getInputFromUser("Enter Vehicle License Number: ", true);
                bool isVehicleExists = r_GarageManager.TryUpdateStatusIfVehicleExists(licenseNumber);

                if (isVehicleExists)
                {
                    Console.WriteLine("Vehicle already in garage. Changed status to In-Repair.");
                }
                else
                {
                    Vehicle newVehicle = createNewVehicle(licenseNumber);

                    collectFieldsFromUser(newVehicle);
                    string ownerName = getInputFromUser("Enter Owner Name: ", false);
                    string ownerPhone = getOwnerPhone();

                    r_GarageManager.InsertNewVehicle(newVehicle, ownerName, ownerPhone);
                    Console.WriteLine("Vehicle inserted successfully into the garage.");
                }
            }
            catch (KeyNotFoundException knfe)
            {
                Console.WriteLine(knfe.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        private T getSelectionFromList<T>(List<T> i_Items, string i_Prompt)
        {
            Console.WriteLine(i_Prompt);

            for (int i = 0; i < i_Items.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {i_Items[i]}");
            }

            T selectedItem = default;
            bool isValidChoice = false;

            while (!isValidChoice)
            {
                Console.Write("Enter your choice: ");
                string userInput = Console.ReadLine();

                if (int.TryParse(userInput, out int result) && result >= 1 && result <= i_Items.Count)
                {
                    selectedItem = i_Items[result - 1];
                    isValidChoice = true;
                }
                else
                {
                    Console.WriteLine($"Invalid selection. Please enter a number between 1 and {i_Items.Count}");
                }
            }

            return selectedItem;
        }

        private eEngineType askUserForEngineType(eEngineType[] i_PossibleEngineTypes)
        {
            List<eEngineType> engineTypes = i_PossibleEngineTypes.ToList();

            return getSelectionFromList(engineTypes, "Choose engine type:");
        }

        private string askUserForVehicleType()
        {
            List<string> vehicleTypes = VehicleFactory.sr_TypeToEnginesMap.Keys.ToList();

            return getSelectionFromList(vehicleTypes, "Choose vehicle type:");
        }

        private Vehicle createNewVehicle(string i_LicenseNumber)
        {
            bool isVehicleCreated = false;
            Vehicle newVehicle = null;

            while (!isVehicleCreated)
            {
                string chosenVehicleType = askUserForVehicleType();
                eEngineType[] possibleEngineTypes = GarageManager.GetPossibleEngineTypes(chosenVehicleType);
                eEngineType chosenEngineType = eEngineType.Fuel;

                if (possibleEngineTypes.Length == 1)
                {
                    chosenEngineType = possibleEngineTypes[0];
                    Console.WriteLine($"Selected {chosenVehicleType} - automatically set to {chosenEngineType} engine.");
                }
                else
                {
                    chosenEngineType = askUserForEngineType(possibleEngineTypes);
                }

                newVehicle = VehicleFactory.CreateVehicle(chosenVehicleType, chosenEngineType);
                newVehicle.LicenseNumber = i_LicenseNumber;
                isVehicleCreated = true;
            }

            return newVehicle;
        }

        private void displayLicenseNumbers()
        {
            bool isUseFilter = false;
            bool isValidInput = true;

            Console.WriteLine("=== Displaying License Numbers of Vehicles in Garage ===");

            try
            {
                isUseFilter = askYesOrNo("Would you like to filter by status?");
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);
                isValidInput = false;
            }

            if (isValidInput)
            {
                eVehicleGarageStatus? statusFilter = null;

                if (isUseFilter)
                {
                    Console.WriteLine("Choose Status to Filter By:");
                    statusFilter = getEnumSelection<eVehicleGarageStatus>();
                }

                List<string> licenseNumbers = r_GarageManager.GetLicenseNumbers(statusFilter);
                bool isLicenseListNotEmpty = licenseNumbers.Count > 0;

                if (!isLicenseListNotEmpty)
                {
                    Console.WriteLine(isUseFilter ? $"No vehicles exist in the garage with status: {statusFilter}" : "The garage is empty.");
                }
                else
                {
                    Console.WriteLine("License Numbers in Garage:");

                    foreach (string license in licenseNumbers)
                    {
                        Console.WriteLine(license);
                    }
                }
            }
        }

        private void changeVehicleStatus()
        {
            bool isChangingStatus = true;

            while (isChangingStatus)
            {
                try
                {
                    Console.WriteLine("=== Change Vehicle Status ===");

                    if (!tryValidateVehicle(null, out string licenseNumber))
                    {
                        Console.WriteLine("Operation cancelled.");
                        break;
                    }
                    Console.WriteLine("Please choose a status:");
                    eVehicleGarageStatus newStatus = getEnumSelection<eVehicleGarageStatus>();

                    r_GarageManager.ChangeVehicleStatus(licenseNumber, newStatus);
                    Console.WriteLine("The vehicle status was changed successfully.");
                    isChangingStatus = false;
                }
                catch (ArgumentException ae)
                {
                    Console.WriteLine($"Argument error: {ae.Message}");
                }
                catch (KeyNotFoundException knfe)
                {
                    Console.WriteLine(knfe.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                }

                if (isChangingStatus)
                {
                    isChangingStatus = askYesOrNo("Would you like to try again?");
                }
            }
        }

        private void inflateWheels()
        {
            bool isInflate = true;

            while (isInflate)
            {
                try
                {
                    Console.WriteLine("=== Inflating All Wheels to Maximum ===");

                    if (!tryValidateVehicle(null, out string licenseNumber))
                    {
                        Console.WriteLine("Operation cancelled.");
                        break;
                    }

                    r_GarageManager.InflateWheelsToMax(licenseNumber);
                    Console.WriteLine("All wheels inflated to maximum successfully.");
                    isInflate = false;
                }
                catch (ArgumentException ae)
                {
                    Console.WriteLine($"Argument error: {ae.Message}");
                }
                catch (KeyNotFoundException knfe)
                {
                    Console.WriteLine(knfe.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                }

                if (isInflate)
                {
                    isInflate = askYesOrNo("Would you like to try again?");
                }
            }
        }

        private void chargeVehicle()
        {
            bool isCharging = true;

            while (isCharging)
            {
                try
                {
                    Console.WriteLine("=== Charging an Electric Vehicle ===");

                    if (!tryValidateVehicle(eEngineType.Electric, out string licenseNumber))
                    {
                        Console.WriteLine("Operation cancelled.");
                        isCharging = false;
                        continue;
                    }

                    float chargeAmount = getChargeAmountInHoursFromUser();

                    r_GarageManager.ChargeVehicle(licenseNumber, chargeAmount);
                    Console.WriteLine("The electric vehicle was charged successfully.");
                    isCharging = false;
                }
                catch (ValueOutOfRangeException voore)
                {
                    Console.WriteLine($"Value Out of Range Error: {voore.Message} (Min: {voore.MinValue}, Max: {voore.MaxValue})");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                }

                if (isCharging)
                {
                    isCharging = askYesOrNo("Would you like to try again?");
                }
            }
        }

        private void refuelVehicle()
        {
            bool isRefueling = true;

            while (isRefueling)
            {
                try
                {
                    Console.WriteLine("=== Refueling a Fuel-Based Vehicle ===");

                    if (!tryValidateVehicle(eEngineType.Fuel, out string licenseNumber))
                    {
                        Console.WriteLine("Operation cancelled.");
                        break;
                    }

                    eFuelType fuelType = getEnumSelection<eFuelType>();
                    float fuelAmount = getFuelAmountFromUser();

                    r_GarageManager.RefuelVehicle(licenseNumber, fuelType, fuelAmount);
                    Console.WriteLine("The vehicle was refueled successfully.");
                    isRefueling = false;
                }
                catch (ArgumentException ae)
                {
                    Console.WriteLine($"Argument error: {ae.Message}");
                }
                catch (KeyNotFoundException knfe)
                {
                    Console.WriteLine(knfe.Message);
                }
                catch (ValueOutOfRangeException voore)
                {
                    Console.WriteLine($"Value out of range error: {voore.Message} (Min: {voore.MinValue}, Max: {voore.MaxValue})");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                }

                if (isRefueling)
                {
                    if (!askYesOrNo("Would you like to try again?"))
                    {
                        isRefueling = false;
                    }
                }
            }
        }

        private float getFloatInputFromUser(string i_PromptMessage)
        {
            float result = 0f;
            bool isValid = false;

            while (!isValid)
            {
                Console.Write(i_PromptMessage);
                string userInput = Console.ReadLine();

                if (float.TryParse(userInput, out result))
                {
                    if (result <= 0)
                    {
                        Console.WriteLine("Value must be greater than zero. Please try again.");
                    }
                    else
                    {
                        isValid = true;
                    }
                }
                else
                {
                    Console.WriteLine("The entered amount is not in a valid format. Please enter a numeric value.");
                }
            }

            return result;
        }

        private float getChargeAmountInHoursFromUser()
        {
            float minutesToCharge = getFloatInputFromUser("Enter Amount of Time to Charge (in minutes): ");

            return minutesToCharge / 60;
        }

        private float getFuelAmountFromUser()
        {
            return getFloatInputFromUser("Enter Amount of Fuel to Add (liters): ");
        }

        private bool askYesOrNo(string i_Prompt)
        {
            bool isValidAnswer = false;
            bool isUserChoiceYes = false;

            while (!isValidAnswer)
            {
                Console.Write($"{i_Prompt} (Y/N) or (YES/NO): ");
                string userInput = Console.ReadLine();

                try
                {
                    isUserChoiceYes = Validator.ParseYesOrNo(userInput);
                    isValidAnswer = true;
                }
                catch (FormatException fe)
                {
                    Console.WriteLine(fe.Message);
                }
            }

            return isUserChoiceYes;
        }

        private string formatVehicleData(Dictionary<string, string> i_Data)
        {
            StringBuilder vehicleDetails = new StringBuilder();

            foreach (KeyValuePair<string, string> kvp in i_Data)
            {
                vehicleDetails.AppendLine($"{kvp.Key}: {kvp.Value}");
            }

            return vehicleDetails.ToString();
        }

        private void displayVehicleDetails()
        {
            bool isDisplaying = true;
            StringBuilder details = new StringBuilder();

            while (isDisplaying)
            {
                try
                {
                    details.Clear();
                    details.AppendLine("=== Displaying Vehicle Details by License Number ===");
                    details.AppendLine();
                    details.AppendLine("=======================================");
                    details.AppendLine("Vehicle Details:");

                    string licenseNumber = getInputFromUser("Enter Vehicle License Number: ", true);
                    Dictionary<string, string> vehicleFullDetails = r_GarageManager.GetVehicleFullDetails(licenseNumber);

                    details.Append(formatVehicleData(vehicleFullDetails));
                    details.AppendLine("=======================================");
                    details.AppendLine();
                    Console.Write(details.ToString());
                    isDisplaying = false;
                }
                catch (ArgumentException ae)
                {
                    Console.WriteLine($"Argument error: {ae.Message}");
                }
                catch (KeyNotFoundException knfe)
                {
                    Console.WriteLine(knfe.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }

                if (isDisplaying)
                {
                    isDisplaying = askYesOrNo("Would you like to try again?");
                }
            }
        }

        private string getInputFromUser(string i_PromptMessage, bool i_IsNumbersOnly)
        {
            string userInput = string.Empty;
            bool isValid = false;

            while (!isValid)
            {
                Console.Write(i_PromptMessage);
                userInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(userInput))
                {
                    Console.WriteLine("Input cannot be empty or contain only spaces. Please try again.");
                }
                else if (i_IsNumbersOnly && !userInput.All(char.IsDigit))
                {
                    Console.WriteLine("Input must contain only numbers. Please try again.");
                }
                else if (!i_IsNumbersOnly && !userInput.All(char.IsLetter))
                {
                    Console.WriteLine("Input must contain only letters. Please try again.");
                }
                else
                {
                    isValid = true;
                }
            }

            return userInput;
        }

        private T getEnumSelection<T>() where T : Enum
        {
            return (T)getEnumInput(typeof(T));
        }

        private object getEnumInput(Type i_EnumType)
        {
            string[] enumNames = Enum.GetNames(i_EnumType);

            for (int i = 0; i < enumNames.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {enumNames[i]}");
            }

            bool isValidChoice = false;
            object selectedEnum = null;

            while (!isValidChoice)
            {
                Console.Write("Enter your choice: ");
                string userInput = Console.ReadLine();

                if (int.TryParse(userInput, out int result))
                {
                    try
                    {
                        selectedEnum = Validator.ValidateEnumSelection(i_EnumType, result);
                        isValidChoice = true;
                    }
                    catch (ArgumentOutOfRangeException aoore)
                    {
                        Console.WriteLine(aoore.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Please enter a valid number.");
                }
            }

            return selectedEnum;
        }

        private string getOwnerPhone()
        {
            string ownerPhone = string.Empty;
            bool isValid = false;

            while (!isValid)
            {
                Console.Write("Enter Owner Phone (must be 10 digits and start with '05'): ");
                ownerPhone = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(ownerPhone) && ownerPhone.Length == 10 && ownerPhone.StartsWith("05") && ownerPhone.All(char.IsDigit))
                {
                    isValid = true;
                }
                else
                {
                    Console.WriteLine("Invalid phone number. It must be exactly 10 digits, start with '05', and contain only digits. Please try again.");
                }
            }

            return ownerPhone;
        }

        private bool tryValidateVehicle(eEngineType? i_RequiredEngineType, out string io_LicenseNumber)
        {
            io_LicenseNumber = string.Empty;
            bool isValid = false;

            while (!isValid)
            {
                string licenseNumber = getInputFromUser("Enter Vehicle License Number: ", true);

                io_LicenseNumber = licenseNumber;

                try
                {
                    r_GarageManager.ValidateVehicle(io_LicenseNumber, i_RequiredEngineType);
                    isValid = true;
                }
                catch (KeyNotFoundException knfe)
                {
                    Console.WriteLine(knfe.Message);
                }
                catch (ArgumentException ae)
                {
                    Console.WriteLine($"Argument error: {ae.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }

                if (!isValid)
                {
                    bool isTryingAgain = askYesOrNo("Would you like to try again?");

                    if (!isTryingAgain)
                    {
                        io_LicenseNumber = string.Empty;
                        break;
                    }
                }
            }

            return isValid;
        }
    }
}