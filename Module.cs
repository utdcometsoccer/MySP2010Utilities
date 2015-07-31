using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySP2010Utilities
{
    public class Module
    {
        public string ProvisioningUrl { get; set; }
        public string PhysicalPath { get; set; }
        public Module.File[] Files { get; set; }

        public class File
        {
            public string Name { get; set; }
            public Dictionary<string, string> Properties { get; set; }
        }
    }
}
