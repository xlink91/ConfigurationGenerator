using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigFileGeneratorTests.CustomTypes
{
    public class Information
    {
        public Guid Guid { get; set; } = Guid.Parse("087D3AD4-7F5D-457B-A16C-9D402D22B2E6");
        public string MongoConnectionString { get; set; } = "mongo://azure.weer-xww-334s-xx3c5.dev.microsoft.com";
        public Tuple<double, double> Location { get; set; } = new Tuple<double, double>(120.0, 123.0);
    }
}
