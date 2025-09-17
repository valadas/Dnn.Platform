using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Dnn.JoditEditorProvider
{
    public class EditorControl : WebControl
    {
        public EditorControl()
        {
            // TODO: IMPLEMENT THIS PROPERLY
            this.Height = Unit.Pixel(300);
            this.Width = Unit.Percentage(100);
        }

        [DefaultValue("")]
        public string Value
        {
            get
            {
                var value = this.ViewState["Value"];
                return value == null ? string.Empty : (string)value;
            }

            set
            {
                this.ViewState["Value"] = value;
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.WriteLine("<h1>Custom Editor is loading!!!</h1>");
        }
    }
}
