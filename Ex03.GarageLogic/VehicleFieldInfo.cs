using System;

namespace Ex03.GarageLogic
{
    public class VehicleFieldInfo
    {
        private string m_FieldName = string.Empty;
        private string m_InputPrompt = string.Empty;
        private Type m_FieldType = null;
        private bool m_IsEnum = false;
        private float? m_MinAllowedValue = null;
        private float? m_MaxAllowedValue = null;

        public string FieldName
        {
            get
            {
                return m_FieldName;
            }
            set
            {
                m_FieldName = value;
            }
        }

        public string InputPrompt
        {
            get
            {
                return m_InputPrompt;
            }
            set
            {
                m_InputPrompt = value;
            }
        }

        public Type FieldType
        {
            get
            {
                return m_FieldType;
            }
            set
            {
                m_FieldType = value;
            }
        }

        public bool IsEnum
        {
            get
            {
                return m_IsEnum;
            }
            set
            {
                m_IsEnum = value;
            }
        }

        public float? MinValue
        {
            get
            {
                return m_MinAllowedValue;
            }
            set
            {
                m_MinAllowedValue = value;
            }
        }

        public float? MaxValue
        {
            get
            {
                return m_MaxAllowedValue;
            }
            set
            {
                m_MaxAllowedValue = value;
            }
        }
    }
}
