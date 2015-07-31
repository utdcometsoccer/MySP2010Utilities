using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.Office.RecordsManagement.RecordsRepository;
using Microsoft.SharePoint.Taxonomy;

namespace MySP2010Utilities
{
    class ManagedMetadataAutofolder : MySP2010Utilities.IAutofolderCreator
    {
        public void CreateAutofolder(IContentOrganizerRuleCreationData data, SPContentType ruleContentType, EcmDocumentRouterRule organizeDocument)
        {
            // Ensure the SPField for the autofolder property
            TaxonomyField autoFolderField = ruleContentType.Fields[data.AutoFolderPropertyName] as TaxonomyField;
            if (autoFolderField == null)
                throw new ArgumentException(String.Format("The field {0} is not a valid Taxonomy Field", data.AutoFolderPropertyName));

            // Get a handle to the rule auto folder settings.
            DocumentRouterAutoFolderSettings autoFolderSettings = organizeDocument.AutoFolderSettings;
            // Configure AutoFolderSettings for this rule based on the Taxonomy field.                 
            autoFolderSettings.AutoFolderPropertyName = autoFolderField.Title;
            autoFolderSettings.AutoFolderPropertyInternalName = autoFolderField.InternalName;
            autoFolderSettings.AutoFolderPropertyId = autoFolderField.Id;
            // Term store Id required to get the value of the field from the document. Required for TaxonomyField types.
            autoFolderSettings.TaxTermStoreId = autoFolderField.SspId;
            // Set a format for the name of the folder. 
            autoFolderSettings.AutoFolderFolderNameFormat = data.AutoFolderNameFormat;
            // Enabled automatic folder creation for values of the field.
            autoFolderSettings.Enabled = true;
        }
    }
}
