using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace ProtocolTemplateLib
{
    public abstract class ProtocolField
    {
        public abstract void GetFromRequest(string value);
        public abstract string AddToSaveRequest();
        public abstract void AddToHtmlProtocol(StringBuilder builder);
    }

    public class TextboxField : ProtocolField
    {
        private string Value
        {
            get
            {
                if (Control != null)
                {
                    Value_ = Control.Text;
                }
                return Value_;
            }
            set
            {
                Value_ = value;
                if (Control != null)
                {
                    Control.Text = value;
                }
            }
        }
        public override string AddToSaveRequest()
        {
            return Value;
        }

        public override void GetFromRequest(string value)
        {
            Value = value;
        }

        public override void AddToHtmlProtocol(StringBuilder builder)
        {
            throw new NotImplementedException();
        }

        private string Value_;
        internal TextBox Control;
    }

    public class ComboBoxField : ProtocolField
    {
        public string Value
        {
            get
            {
                return (Editable_.EnableOtherField ? ValueString : ValueInt.ToString());
            }
        }
        public ComboBoxField(ComboboxEditable editable)
        {
            Editable_ = editable;
            ValueInt_ = -1;
            ValueString_ = "";
        }

        public override void AddToHtmlProtocol(StringBuilder builder)
        {
            throw new NotImplementedException();
        }

        public override string AddToSaveRequest()
        {
            if (Editable_.EnableOtherField)
            {
                string realValue = Value;
                int index = Editable_.Variants.IndexOf(realValue);
                if (index < 0)
                {
                    return "NULL, " + realValue;
                }
                else
                {
                    return index + ", NULL";
                }
            }
            else
            {
                return ValueString.ToString();
            }
        }

        public override void GetFromRequest(string value)
        {
            if (Editable_.EnableOtherField)
            {
                string[] tokens = value.Split(" ,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length > 2)
                {
                    throw new ArgumentException("Wrong number of tokens");
                }
                if (ChechNotNull(tokens[0]))
                {
                    ParseValueInt(tokens[0]);
                    ValueString = Editable_.Variants[ValueInt_];
                }
                else
                {
                    ValueInt_ = -1;
                    ValueString = "";
                }
                if ((tokens.Length == 2) && ChechNotNull(tokens[1]))
                {
                    ValueString = value;
                }
            }
            else
            {
                ParseValueInt(value);
            }
        }

        private static bool ChechNotNull(string value)
        {
            return value.ToUpper() != "NULL";
        }

        private void ParseValueInt(string value)
        {
            try
            {
                ValueInt = int.Parse(value);
            }
            catch (FormatException ex)
            {
                throw new ArgumentException("Value have to be null", ex);
            }
        }

        protected string ValueString
        {
            get
            {
                if (Control != null)
                {
                    ValueString_ = Control.Text;
                }
                return ValueString_;
            }
            set
            {
                ValueString_ = value;
                if (Control != null)
                {
                    Control.Text = ValueString_;
                }
            }
        }
        protected int ValueInt
        {
            get
            {
                if (Control != null)
                {
                    ValueInt_ = Control.SelectedIndex;
                }
                return ValueInt_;
            }
            set
            {
                ValueInt_ = value;
                if (Control != null)
                {
                    Control.SelectedIndex = ValueInt_;
                }
            }
        }

        private string ValueString_;
        private int ValueInt_;
        private ComboboxEditable Editable_;
        internal ComboBox Control;
    }
}
