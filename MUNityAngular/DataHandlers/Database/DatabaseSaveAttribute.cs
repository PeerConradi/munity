using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.DataHandlers.Database
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    sealed class DatabaseSaveAttribute : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        readonly string cname;

        // This is a positional argument
        public DatabaseSaveAttribute(string columnname)
        {
            this.cname = columnname;
        }

        public string ColumnName
        {
            get { return cname; }
        }

    }
}
