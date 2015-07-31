using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySP2010Utilities
{
    class ContentOrganizerRuleCreationData : MySP2010Utilities.IContentOrganizerRuleCreationData
    {
        private List<IContentOrganizerConditionalData> _Conditions;
        public ContentOrganizerRuleCreationData()
        {
            _Conditions = new List<IContentOrganizerConditionalData>();
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LibraryName { get; set; }
        public IList<IContentOrganizerConditionalData> Conditions { get { return _Conditions; } }
        public string ContentTypeName { get; set; }
        public string AutoFolderPropertyName { get; set; }
        public string AutoFolderNameFormat { get; set; }
        public string SiteAbsoluteUrl { get; set; }
        public string Priority { get; set; }
        public string CustomRouter { get; set; }
    }
}
