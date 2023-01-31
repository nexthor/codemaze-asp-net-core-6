using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.RequestParameters
{
    public class CompanyParameters : RequestParameters
    {
        public CompanyParameters() => OrderBy = "name";
    }
}
