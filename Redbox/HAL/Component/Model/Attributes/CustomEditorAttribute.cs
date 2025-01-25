using System;

namespace Redbox.HAL.Component.Model.Attributes
{
    public class CustomEditorAttribute : Attribute
    {
        public CustomEditorAttribute(string text) => this.Text = text;

        public string Text { get; private set; }

        public string GetMethodName { get; set; }

        public string SetMethodName { get; set; }
    }
}
