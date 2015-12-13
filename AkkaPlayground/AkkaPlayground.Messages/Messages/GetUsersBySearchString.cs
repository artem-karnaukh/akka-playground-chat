using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Messages.Messages
{
    public class GetUsersBySearchString
    {
        public string SearchString { get; private set; }

        public GetUsersBySearchString(string searchString)
        {
            SearchString = searchString;
        }
    }
}
