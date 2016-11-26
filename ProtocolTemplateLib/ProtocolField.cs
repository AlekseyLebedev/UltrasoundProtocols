using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace ProtocolTemplateLib
{
    public abstract class ProtocolField
    {
        public virtual void GetFromRequest(string[] values)
        {
            if (values.Length != GetFieldCount())
            {
                throw new ArgumentException("Wrong number of tokens in GetFromRequest");
            }
            ParseFromTokens(values);
        }
        public abstract string[] AddToSaveRequest();
        public abstract void PrintToProtocol(StringBuilder builder);
        public abstract int GetFieldCount();
        protected abstract void ParseFromTokens(string[] values);
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
        internal TextBox Control
        {
            get
            {
                return Contol_;
            }
            set
            {
                value.Text = Value;
                Contol_ = value;
            }
        }
        public override string[] AddToSaveRequest()
        {
            return new string[] { Value };
        }

        protected override void ParseFromTokens(string[] values)
        {
            Value = values[0];
        }

        public override void PrintToProtocol(StringBuilder builder)
        {
            builder.Append(Value);
        }

        public override int GetFieldCount()
        {
            return 1;
        }

        private string Value_ = "";
        private TextBox Contol_ = null;
    }

    public class ComboBoxField : ProtocolField
    {
        public string Value
        {
            get
            {
                return (Editable_.EnableOtherField ? ValueString : Editable_.Variants[ValueInt]);
            }
        }
        public ComboBoxField(ComboboxEditable editable)
        {
            Editable_ = editable;
            ValueInt_ = 0;
            ValueString_ = "";
        }

        public override void PrintToProtocol(StringBuilder builder)
        {
            builder.Append(Value);
        }

        public override string[] AddToSaveRequest()
        {
            if (Editable_.EnableOtherField)
            {
                string realValue = Value;
                int index = Editable_.Variants.IndexOf(realValue);
                if (index < 0)
                {
                    return new string[] { null, realValue };
                }
                else
                {
                    return new string[] { index.ToString(), null };
                }
            }
            else
            {
                return new string[] { ValueString.ToString() };
            }
        }

        internal ComboBox Control
        {
            get
            {
                return Control_;
            }
            set
            {
                if (Editable_.EnableOtherField)
                {
                    value.Text = ValueString;
                }
                else
                {
                    value.SelectedIndex = ValueInt;
                }
                Control_ = value;
            }
        }
        protected override void ParseFromTokens(string[] values)
        {
            if (Editable_.EnableOtherField)
            {
                if (values.Length > 2)
                {
                    throw new ArgumentException("Wrong number of tokens");
                }
                if (ChechNotNull(values[0]))
                {
                    ParseValueInt(values[0]);
                    ValueString = Editable_.Variants[ValueInt_];
                }
                else
                {
                    ValueInt_ = -1;
                    ValueString = "";
                }
                if ((values.Length == 2) && ChechNotNull(values[1]))
                {
                    ValueString = values[1];
                }
            }
            else
            {
                ParseValueInt(values[0]);
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

        public override int GetFieldCount()
        {
            return (Editable_.EnableOtherField ? 2 : 1);
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

        private string ValueString_ = "";
        private int ValueInt_ = -1;
        private ComboboxEditable Editable_;
        private ComboBox Control_;
    }
}
