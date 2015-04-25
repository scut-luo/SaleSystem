using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabaseControlLib
{
    public class AdapterFactory
    {
        public static CommonAdapter CreateAdapter(DBase db,string tableName)
        {
            CommonAdapter adapter = null;
            switch (tableName)
            {
                case "StudentUser":
                    //adapter = new UserAdapter(db,"StudentUser");
                    break;
            }
            return adapter;
        }
    }
}
