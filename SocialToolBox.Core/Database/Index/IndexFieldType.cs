using System;
using System.Diagnostics;
using System.Text;

namespace SocialToolBox.Core.Database.Index
{
    /// <summary>
    /// The type of an index field.
    /// </summary>
    public class IndexFieldType
    {
        /// <summary>
        /// The underlying data type.
        /// </summary>
        public enum DataType
        {
            Int,
            Float,
            Char,
            Varchar,
            Bool,
            DateTime
        }

        /// <summary>
        /// The underlying data type.
        /// </summary>
        public readonly DataType Type;

        /// <summary>
        /// The number of characters in a Char or Varchar.
        /// </summary>
        public readonly int Length;

        /// <summary>
        /// Does allow NULL values ?
        /// </summary>
        public readonly bool NotNull;

        /// <summary>
        /// Does this value contain only ASCII characters ? Otherwise,
        /// always assume UTF8.
        /// </summary>
        public readonly bool IsAscii;

        /// <summary>
        /// Are comparisons on this field case-sensitive ? Default
        /// is true.
        /// </summary>
        public readonly bool IsCaseSensitive;

        /// <summary>
        /// Returns the type declaration in MySQL format.
        /// </summary>
        public string MySqlDeclaration { get { return GetDeclaration(true); } }

        /// <summary>
        /// Returns the type declaration in MSSQL format.
        /// </summary>
        public string SqlServerDeclaration { get { return GetDeclaration(false); } }

        /// <summary>
        /// Constructs a standard SQL declaration.
        /// </summary>
        private string GetDeclaration(bool isMySql)
        {
            var sb = new StringBuilder();
            switch (Type)
            {
                case DataType.Varchar:
                    sb.AppendFormat("{1}VARCHAR({0})", Length, isMySql || IsAscii ? "" : "N");
                    break;
                case DataType.Char:
                    sb.AppendFormat("{1}CHAR({0})", Length, isMySql || IsAscii ? "" : "N");
                    break;
                case DataType.Int:
                    sb.Append("INT");
                    break;
                case DataType.Float:
                    sb.Append("FLOAT");
                    break;
                case DataType.DateTime:
                    sb.Append("DATETIME");
                    break;
                default:
                    // All possible data types should be handled above.
                    Debug.Assert(false);
                    break;
            }

            sb.Append(NotNull ? " NOT NULL" : " NULL");

            if (Type == DataType.Char || Type == DataType.Varchar)
            {
                if (isMySql)
                    sb.AppendFormat(" CHARACTER SET {0}", IsAscii ? "ascii" : "utf8");
                else if (IsAscii)
                    sb.Append(" CHARACTER SET latin1");

                if (isMySql)
                    sb.AppendFormat(" COLLATE {0}",
                        IsAscii
                            ? (IsCaseSensitive ? "ascii_bin" : "ascii")
                            : (IsCaseSensitive ? "utf8_bin" : "utf8_general_ci"));
                else
                    sb.AppendFormat(" COLLATE {0}",
                        // Code page does not apply to NCHAR and NVARCHAR
                        IsCaseSensitive ? "latin1_general_cs_as" : "latin1_general_as");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Construct an index field type from the actual type and the
        /// index field attribute.
        /// </summary>
        public IndexFieldType(Type t, IndexFieldAttribute attr)
        {
            if (t == typeof (string))
            {
                Type = DataType.Varchar;
                Length = attr.Length;
                NotNull = attr.NotNull;
                IsCaseSensitive = attr.IsCaseSensitive;
                IsAscii = false;
                return;
            }

            if (t == typeof (int) || t == typeof(Int32))
            {
                Type = DataType.Int;
                Length = 1;
                NotNull = true;
                IsCaseSensitive = false;
                IsAscii = false;
                return;
            }

            if (t == typeof (Id))
            {
                Type = DataType.Char;
                Length = Id.Length;
                IsCaseSensitive = true;
                IsAscii = true;
                NotNull = attr.NotNull;
                return;
            }

            if (t == typeof (DateTime))
            {
                Type = DataType.DateTime;
                Length = 1;
                IsCaseSensitive = false;
                IsAscii = false;
                NotNull = true;
                return;
            }

            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof (Nullable<>))
            {
                var inner = t.GetGenericArguments()[0];
                if (inner == typeof (int) || t == typeof(Int32))
                {
                    Type = DataType.Int;
                    Length = 1;
                    NotNull = false;
                    IsAscii = false;
                    IsCaseSensitive = false;
                    return;
                }

                if (inner == typeof (DateTime))
                {
                    Type = DataType.DateTime;
                    Length = 1;
                    IsCaseSensitive = false;
                    IsAscii = false;
                    NotNull = false;
                    return;
                }
            }

            throw new ArgumentException(
                string.Format("Unknown type {0}", t), "t");
        }
    }   
}
