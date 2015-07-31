using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.Office.RecordsManagement.RecordsRepository;
using Microsoft.SharePoint.Taxonomy;

namespace MySP2010Utilities
{
    class ContentOrganizerCreator : MySP2010Utilities.IContentOrganizerCreator
    {
        IAutofolderCreator managedMetadataAutoCreator = new ManagedMetadataAutofolder();
        IContentOrganizerCreatorImpl creator = new ContentOrganizerCreatorImpl();
        public void CreateRuleManagedMetadataField(IContentOrganizerRuleCreationData data)
        {
            data.RequireNotNull("data");
            ILogUtility logger = new LogUtility();
            string conditionsXml = setConditions(data);
            logger.TraceDebugInformation(string.Format("Conditions:{0}", conditionsXml), GetType());
            using (SPSite site = new SPSite(data.SiteAbsoluteUrl))
            using (SPWeb web = site.OpenWeb())
            {
                creator.CreateRuleManagedMetadataField(data, conditionsXml, site, web, managedMetadataAutoCreator);
            }
        }        

        private static string setConditions(IContentOrganizerRuleCreationData data)
        {
            StringBuilder conditionsSB = new StringBuilder("<Conditions>");

            foreach (var item in data.Conditions)
            {
                conditionsSB.Append(setConditional(item));
            }

            // The condition Xml can repeat 0-5 times depending on the number of conditions required for a document to match this rule.
            conditionsSB.Append("</Conditions>");
            return conditionsSB.ToString();
        }

        private static string setConditional(IContentOrganizerConditionalData data)
        {
            string conditionXml = string.IsNullOrEmpty(data.ConditionFieldTitle) ?
                string.Empty :
                String.Format(@"<Condition Column=""{0}|{1}|{2}"" Operator=""{3}"" Value=""{4}"" />",
                data.ConditionFieldID, data.ConditionFieldInternalName, data.ConditionFieldTitle,
                data.ConditionOperator,
                data.ConditionValue);
            return conditionXml;
        }
    }
}
