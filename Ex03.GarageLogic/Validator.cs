using System;

namespace Ex03.GarageLogic
{
    public class Validator
    {
        public static object ConvertUserInput(string i_UserInput, VehicleFieldInfo i_FieldMetadata)
        {
            if (string.IsNullOrWhiteSpace(i_UserInput))
            {
                throw new FormatException("Input cannot be empty.");
            }

            Type fieldType = i_FieldMetadata.FieldType;
            object convertedValue = null;

            if (fieldType == typeof(float))
            {
                if (!float.TryParse(i_UserInput, out float floatValue))
                {
                    throw new FormatException("Input must be a valid decimal number.");
                }

                validateRange(floatValue, i_FieldMetadata.MinValue, i_FieldMetadata.MaxValue);
                convertedValue = floatValue;
            }
            else if (fieldType == typeof(int))
            {
                if (!int.TryParse(i_UserInput, out int intValue))
                {
                    throw new FormatException("Input must be a valid integer.");
                }

                convertedValue = intValue;
            }
            else if (fieldType == typeof(bool))
            {
                convertedValue = ParseYesOrNo(i_UserInput);
            }
            else if (fieldType == typeof(string))
            {
                convertedValue = i_UserInput;
            }
            else
            {
                throw new NotSupportedException($"The field type {fieldType.Name} is not supported.");
            }

            return convertedValue;
        }

        private static void validateRange(float i_Value, float? i_MinValue, float? i_MaxValue)
        {
            if (i_MinValue.HasValue && i_Value < i_MinValue.Value)
            {
                float actualMax = i_MaxValue.HasValue ? i_MaxValue.Value : float.MaxValue;

                throw new ValueOutOfRangeException($"Input is below the allowed range. (Minimum: {i_MinValue.Value}, Maximum: {actualMax})", i_MinValue.Value, actualMax);
            }

            if (i_MaxValue.HasValue && i_Value > i_MaxValue.Value)
            {
                float actualMin = i_MinValue.HasValue ? i_MinValue.Value : 0f;

                throw new ValueOutOfRangeException($"Input is above the allowed range. (Minimum: {actualMin}, Maximum: {i_MaxValue.Value})", actualMin, i_MaxValue.Value);
            }
        }

        public static bool ParseYesOrNo(string i_UserInput)
        {
            if (string.IsNullOrWhiteSpace(i_UserInput))
            {
                throw new FormatException("Input cannot be empty.");
            }

            string userInput = i_UserInput.Trim().ToUpper();
            bool result;

            if (userInput == "Y" || userInput == "YES")
            {
                result = true;
            }
            else if (userInput == "N" || userInput == "NO")
            {
                result = false;
            }
            else
            {
                throw new FormatException("Invalid input.");
            }

            return result;
        }

        public static object ValidateEnumSelection(Type i_Type, int i_UserChoice)
        {
            string[] enumNames = Enum.GetNames(i_Type);

            if (i_UserChoice < 1 || i_UserChoice > enumNames.Length)
            {
                throw new ArgumentOutOfRangeException(null, $"Invalid choice. Please select a number between 1 and {enumNames.Length}.");
            }

            return Enum.Parse(i_Type, enumNames[i_UserChoice - 1]);
        }
    }
}

