using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Common;
using DotNetNuke.Modules.HTMLEditorProvider;

namespace Dnn.JoditEditorProvider
{
    /// <summary>
    /// Implementes DNN HtmlEditorProvider for Jodit Editor.
    /// </summary>
    public class JoditHtmlEditorProvider : HtmlEditorProvider
    {
        private EditorControl htmlEditorControl;
        private string rootImageDirectory;

        public override Control HtmlEditorControl => this.htmlEditorControl;

        public override ArrayList AdditionalToolbars { get; set; }

        public override string ControlID { get; set; }

        public override string RootImageDirectory
        {
            get
            {
                if (this.rootImageDirectory == string.Empty)
                {
                    // Remove the Application Path from the Home Directory
                    return Globals.ApplicationPath != string.Empty
                               ? this.PortalSettings.HomeDirectory.Replace(Globals.ApplicationPath, string.Empty)
                               : this.PortalSettings.HomeDirectory;
                }

                return this.rootImageDirectory;
            }

            set
            {
                this.rootImageDirectory = value;
            }
        }

        public override string Text
        {
            get => this.htmlEditorControl.Value;
            set => this.htmlEditorControl.Value = value; 
        }

        public override Unit Width
        {
            get => this.htmlEditorControl.Width;
            set => this.htmlEditorControl.Width = value; 
        }

        public override Unit Height
        {
            get => this.htmlEditorControl.Height;
            set => this.htmlEditorControl.Height = value; 
        }

        public override void AddToolbar()
        {
        }

        public override void Initialize()
        {
            this.htmlEditorControl = new EditorControl { ID = this.ControlID };
        }
    }
}
