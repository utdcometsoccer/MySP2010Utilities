using System;
using System.Linq;
using Microsoft.SharePoint.Taxonomy;

namespace MySP2010Utilities
{
    class ManagedMetaDataOperations : IManagedMetaDataOperations
    {
        public Term CreateTerm(Term term, Guid termGuid, string nameString, int LCID)
        {
            return SharePointUtilities.CreateTerm(term, termGuid, nameString, LCID);
        }

        public Term CreateTerm(TermSet termSet, Guid termGuid, string nameString, int LCID)
        {
            return SharePointUtilities.CreateTerm(termSet, termGuid, nameString, LCID);
        }

        public Term FindTerm(TermSet termSet, Guid termGuid, string nameString)
        {
            termSet.RequireNotNull("termSet");
            nameString.RequireNotNullOrEmpty("nameString");
            Term term = FindTerm(termSet, termGuid) ?? FindTerm(termSet, nameString);
            return term;
        }

        public Term FindTerm(TermSet termSet, Guid termGuid)
        {
            termSet.RequireNotNull("termSet");
            return termSet.GetTerm(termGuid);
        }

        public Term FindTerm(TermSet termSet, string label)
        {
            termSet.RequireNotNull("termSet");
            return termSet.GetTerms(label, false).FirstOrDefault();
        }

        protected bool hasLabel(Term term, string label)
        {
            term.RequireNotNull("term");
            var labelObj = term.Labels.FirstOrDefault(l => l.Value.ToLower() == label.ToLower());
            return null != labelObj;
        }

        public Term FindTerm(Term term, Guid termGuid, string nameString)
        {
            return SharePointUtilities.FindTerm(term, termGuid, nameString);
        }

        public void SetDefaultTermValue(TermSet termSet, TaxonomyField field, string defaultTermValue)
        {
            SharePointUtilities.SetDefaultTermValue(termSet, field, defaultTermValue);
        }
    }
}
