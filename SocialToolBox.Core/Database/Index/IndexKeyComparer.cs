using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SocialToolBox.Core.Database.Index
{
    /// <summary>
    /// Compares index keys based on the field attributes.
    /// </summary>
    public class IndexKeyComparer<T> : IComparer<T>
    {
        /// <summary>
        /// Applies all field comparers in order, returns 0 if all of
        /// them return 0, otherwise returns the first non-zero result.
        /// </summary>
        public int Compare(T x, T y)
        {
            foreach (var comparer in _fieldComparers)
            {
                var result = comparer(x, y);
                if (result != 0) return result;
            }

            return 0;
        }

        /// <summary>
        /// All available field comparers. Uses reflection to actually 
        /// compare fields.
        /// </summary>
        private readonly Func<object, object, int>[] _fieldComparers;

        public IndexKeyComparer()
        {
            var t = typeof (T);

            if (t.GetCustomAttribute<IndexKeyAttribute>() == null)
                throw new Exception(
                    string.Format("Type {0} does not have attribute IndexKey.", t));

            _fieldComparers = t.GetMembers()
                .Select(m => new KeyValuePair<MemberInfo,IndexFieldAttribute>(m,m.GetCustomAttribute<IndexFieldAttribute>()))
                .Where(kv => kv.Value != null)
                .Select(kv =>
                {
                    var member = kv.Key;
                    var attr = kv.Value;
                    var order = attr.Order;
                    Func<object, object, int> f = null;
                    Func<object, object> getValue = null;
                    Type fType = null;
                    
                    var asField = member as FieldInfo;
                    if (null != asField)
                    {
                        fType = asField.FieldType;
                        getValue = asField.GetValue;
                    }

                    var asProperty = member as PropertyInfo;
                    if (null != asProperty)
                    {
                        fType = asProperty.PropertyType;
                        getValue = asProperty.GetValue;
                    }

                    if (null != getValue)
                    {
                        f = (obja, objb) =>
                        {
                            try
                            {
                                var a = getValue(obja);
                                var b = getValue(objb);

                                if (fType == typeof (string)) return StringCompare(a, b, attr.IsCaseSensitive);
                                if (fType == typeof (Id)) return IdCompare(a, b);
                                if (fType == typeof (int)) return Compare((int)a, (int)b);
                                if (fType == typeof (bool)) return Compare((bool)a, (bool)b);
                                if (fType == typeof (float)) return Compare((float)a, (float)b);
                                if (fType == typeof (double)) return Compare((double)a, (double)b);
                                if (fType == typeof (DateTime)) return Compare((DateTime)a, (DateTime)b);

                                if (fType == typeof(int?)) return CompareNullable((int?)a, (int?)b); 
                                if (fType == typeof(bool?)) return CompareNullable((bool?)a, (bool?)b);
                                if (fType == typeof(float?)) return CompareNullable((float?)a, (float?)b);
                                if (fType == typeof(double?)) return CompareNullable((double?)a, (double?)b);
                                if (fType == typeof(DateTime?)) return CompareNullable((DateTime?)a, (DateTime?)b);
                            }
                            catch (Exception inner)
                            {
                                throw new InvalidDataException(
                                    string.Format("Comparison failed on field {0}", member.Name),
                                    inner);
                            }

                            throw new InvalidDataException(
                                string.Format("Cannot compare type {0} for field {1}", fType, member.Name));
                        };
                    }

                    return new KeyValuePair<int, Func<object, object, int>>(order, f);
                })
                .OrderBy(kv => kv.Key)
                .Select(kv => kv.Value)
                .ToArray();
        }

        /// <summary>
        /// Compares two strings.
        /// </summary>
        private static int StringCompare(object a, object b, bool caseSensitive)
        {
            if (a == null && b == null) return 0;
            if (a == null) return -1;
            if (b == null) return 1;

            return string.Compare((string) a, (string) b, !caseSensitive, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Compare two identifiers.
        /// </summary>
        private static int IdCompare(object a, object b)
        {
            if (a == null && b == null) return 0;
            if (a == null) return -1;
            if (b == null) return 1;

            return string.Compare(a.ToString(), b.ToString(), false, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Compare two non-null objects of the specified type.
        /// </summary>
        private static int Compare<TU>(TU a, TU b) where TU : IComparable
        {
            return a.CompareTo(b);
        }

        /// <summary>
        /// Compare two non-null objects of the specified type.
        /// </summary>
        private static int CompareNullable<TU>(TU? a, TU? b) where TU : struct, IComparable
        {
            if (a == null && b == null) return 0;
            if (a == null) return -1;
            if (b == null) return 1;

            return ((TU)a).CompareTo((TU)b);
        }
    }
}
