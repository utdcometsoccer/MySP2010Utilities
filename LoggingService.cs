using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Administration;

namespace MySP2010Utilities
{
    class LoggingService : SPDiagnosticsServiceBase
    {
        public static string MySP2010iUtilitiesDiagnosticLoggingName { get { return "MySP2010 Utilities Logging Service"; } }
        public static SPDiagnosticsCategory DefaultCategory { get { return Current.Areas["MySP2010 Utilities"].Categories["MySP2010 Utilities Information"]; } }
        public static SPDiagnosticsCategory DefaultErrorCategory { get { return Current.Areas["MySP2010 Utilities"].Categories["MySP2010 Utilities Error"]; } }
        private static LoggingService current;
        public static LoggingService Current
        {
            get
            {
                if (null == current)
                {
                    current = new LoggingService();
                }
                return current;
            }
        }

        public LoggingService()
            :base("MySP2010 Utilities Logging Service", SPFarm.Local)
        {

        }

        protected override IEnumerable<SPDiagnosticsArea> ProvideAreas()
        {
            SPDiagnosticsCategory defaultCategory = new SPDiagnosticsCategory("MySP2010 Utilities Information", TraceSeverity.Medium, EventSeverity.Information);
            SPDiagnosticsCategory defaultErrorCategory = new SPDiagnosticsCategory("MySP2010 Utilities Error", TraceSeverity.High, EventSeverity.Error);
            List<SPDiagnosticsArea> areas = new List<SPDiagnosticsArea>();
            List<SPDiagnosticsCategory> categories = new List<SPDiagnosticsCategory>();
            categories.Add(defaultErrorCategory);
            categories.Add(defaultCategory);
            
            areas.Add(new SPDiagnosticsArea("MySP2010 Utilities", categories));
            return areas;
        }

    }
}
