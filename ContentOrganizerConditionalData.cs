using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySP2010Utilities
{
    class ContentOrganizerConditionalData : MySP2010Utilities.IContentOrganizerConditionalData
    {
        public string ConditionFieldID { get; set; }
        public string ConditionFieldInternalName { get; set; }
        public string ConditionFieldTitle { get; set; }
        public string ConditionOperator { get; set; }
        public string ConditionValue { get; set; }        
    }
}
