using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML.APIResult
{
    public class SearchParams
    {
        private string strSearchKey = "";

        private object strSearchValue = "";

        public string SearchKey
        {
            get
            {
                return strSearchKey;
            }
            set
            {
                strSearchKey = value;
            }
        }

        public object SearchValue
        {
            get
            {
                return strSearchValue;
            }
            set
            {
                strSearchValue = value;
            }
        }

        public SearchParams(string strSearchKey, object strSearchValue)
        {
            this.strSearchKey = strSearchKey;
            this.strSearchValue = strSearchValue;
        }

        public override string ToString()
        {
            return strSearchKey + ":" + Convert.ToString(strSearchValue);
        }
    }
}
