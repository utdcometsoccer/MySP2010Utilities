using System;
using Microsoft.SharePoint;

namespace MySP2010Utilities
{
    public class EventReceiverContext : IDisposable
    {
        public SPSite Site { get; protected set; }
        public SPWeb Web { get; protected set; }
        public SPList List { get; protected set; }
        public SPListItem Item { get; protected set; }

        public EventReceiverContext(SPItemEventProperties properties)
        {
            Site = new SPSite(properties.SiteId);
            Web = Site.OpenWeb(properties.RelativeWebUrl);
            List = Web.Lists[properties.ListId];
            Item = List.GetItemByIdAllFields(properties.ListItemId);
        }
        public void Dispose()
        {
            Dispose(true);
        }

        virtual protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                Site.Dispose();
                Web.Dispose();
            }
        }
    }
}
