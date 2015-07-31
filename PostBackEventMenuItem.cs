using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.WebControls;
using System.Web.UI;

namespace MySP2010Utilities
{
    public class PostBackEventMenuItem : MenuItemTemplate, IPostBackEventHandler
    {
        public PostBackEventMenuItem()
            : base() { }
        public PostBackEventMenuItem(string text)
            : base(text) { }
        public PostBackEventMenuItem(string text, string imageUrl)
            : base(text, imageUrl) { }
        public PostBackEventMenuItem(string text, string imageUrl, string clientOnClickScript)
            : base(text, imageUrl, clientOnClickScript) { }
        protected override void EnsureChildControls()
        {
            if (!this.ChildControlsCreated)
            {
                base.EnsureChildControls();
                if (string.IsNullOrEmpty(this.ClientOnClickUsingPostBackEvent))
                {
                    this.ClientOnClickUsingPostBackEventFromControl(this);
                }
            }
        }
        #region IPostBackEventHandler Members
        public void RaisePostBackEvent(string eventArgument)
        {
            EventHandler<EventArgs> handler = this.OnPostBackEvent;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }
        #endregion
        public event EventHandler<EventArgs> OnPostBackEvent;
    }
}
