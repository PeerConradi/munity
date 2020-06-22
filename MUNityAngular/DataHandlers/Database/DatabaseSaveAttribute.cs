using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.DataHandlers.Database
{

    [Obsolete("Legacy Code: Was removed with using the Entity Framework and should be deleted!")]
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed class DatabaseSaveAttribute : Attribute
    {

        public enum EFieldType
        {
            AUTO,
            VARCHAR,
            TEXT,
            INT,
            TINYINT,
            BIGINT
        }

        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        readonly string cname;

        readonly EFieldType fieldType;

        // This is a positional argument
        public DatabaseSaveAttribute(string columnname)
        {
            this.cname = columnname;
            this.fieldType = EFieldType.AUTO;
        }

        public DatabaseSaveAttribute(string columnname, EFieldType fieldtype)
        {
            this.cname = columnname;
            this.fieldType = fieldtype;
        }

        public string ColumnName
        {
            get { return cname; }
        }

        public EFieldType FieldType
        {
            get => fieldType;
        }

    }

    [System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed class PrimaryKeyAttribute : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        readonly string positionalString;

        // This is a positional argument
        public PrimaryKeyAttribute(string positionalString)
        {
            this.positionalString = positionalString;
        }


        public PrimaryKeyAttribute() { }

        public string PositionalString
        {
            get { return positionalString; }
        }
    }

    [System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    sealed class ConnectionTableAttribute : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        readonly string tablename;

        // This is a positional argument
        public ConnectionTableAttribute(string tablename)
        {
            this.tablename = tablename;
        }

        public string TableName
        {
            get { return tablename; }
        }
    }

    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    sealed class DatabaseTableAttribute : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        readonly string positionalString;

        // This is a positional argument
        public DatabaseTableAttribute(string positionalString)
        {
            this.positionalString = positionalString;

            // TODO: Implement code here

            throw new NotImplementedException();
        }

        public string PositionalString
        {
            get { return positionalString; }
        }
    }
}
