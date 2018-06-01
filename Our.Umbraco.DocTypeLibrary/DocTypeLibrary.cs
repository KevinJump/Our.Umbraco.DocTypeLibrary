using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Web.Trees;
using Umbraco.Core.Logging;
using Umbraco.Web.Models.Trees;
using Umbraco.Core.Models;

namespace Our.Umbraco.DocTypeLibrary
{
    public class DocTypeLibrary : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            ContentTypeTreeController.MenuRendering += ContentTypeTreeController_MenuRendering;
        }

        private void ContentTypeTreeController_MenuRendering(TreeControllerBase sender, MenuRenderingEventArgs e)
        {
            sender.Logger.Debug<DocTypeLibrary>("Sender: {0}", () => sender.TreeAlias);

            // add to the menu.
            if (sender.TreeAlias != "documentTypes")
                return;

            var importItem = e.Menu.Items.FirstOrDefault(x => x.Alias.InvariantEquals("importDocumentType"));
            if (importItem != null)
                e.Menu.Items.Remove(importItem);

            var exportItem = e.Menu.Items.FirstOrDefault(x => x.Alias.InvariantEquals("exportDocumentType"));
            if (exportItem != null)
                e.Menu.Items.Remove(exportItem);


            if (e.NodeId == Constants.System.Root.ToInvariantString())
            {
                e.Menu.Items.Insert(e.Menu.Items.Count - 1, GetLibraryImportMenuItem());
            }

            var container = sender.Services.EntityService.Get(int.Parse(e.NodeId), UmbracoObjectTypes.DocumentTypeContainer);
            if (container != null)
            {
                e.Menu.Items.Insert(e.Menu.Items.Count - 1, GetLibraryImportMenuItem());
            }
        }

        private MenuItem GetLibraryImportMenuItem()
        {
            var libraryItem = new MenuItem("doctypelibrary", "Import from Library")
            {
                Icon = "books",
                SeperatorBefore = true
            };

            libraryItem.AdditionalData.Add("actionView", "/App_plugins/DocTypeLibrary/backoffice/dialogs/import.html");

            return libraryItem;

        }
    }
}
