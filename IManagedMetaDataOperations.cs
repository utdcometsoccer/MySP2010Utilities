using System;
using Microsoft.SharePoint.Taxonomy;
namespace MySP2010Utilities
{
    public interface IManagedMetaDataOperations
    {
        Term CreateTerm(Term term, Guid termGuid, string nameString, int LCID);
        Term CreateTerm(TermSet termSet, Guid termGuid, string nameString, int LCID);
        Term FindTerm(TermSet termSet, Guid termGuid, string nameString);
        Term FindTerm(Term term, Guid termGuid, string nameString);
        Term FindTerm(TermSet termSet, Guid termGuid);
        Term FindTerm(TermSet termSet, string label);
        void SetDefaultTermValue(TermSet termSet, TaxonomyField field, string defaultTermValue);
    }
}
