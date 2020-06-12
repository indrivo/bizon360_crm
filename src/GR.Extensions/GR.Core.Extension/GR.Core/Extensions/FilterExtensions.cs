using GR.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GR.Core.Helpers.Filters.Enums;
using GR.Core.Helpers.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace GR.Core.Extensions
{
    public static class FilterExtensions
    {
        /// <summary>
        /// Filter source by regular expression
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="regexExp"></param>
        /// <returns></returns>
        public static IQueryable<TSource> FilterSourceByRegEx<TSource>(this IQueryable<TSource> source, string regexExp)
        {
            var props = typeof(TSource).GetTypeProprietiesByType(typeof(string)).ToList();
            return !props.Any() ? source : source.Where(t => MatchRegEx(t, regexExp, props.Select(x => x.Name)));
        }

        /// <summary>
        /// Filter objects by text expression
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="toSearch"></param>
        /// <returns></returns>
        public static IQueryable<TSource> FilterSourceByTextExpression<TSource>(this IQueryable<TSource> source, string toSearch)
        {
            var props = typeof(TSource).GetTypeProprietiesByType(typeof(string)).ToList();
            return !props.Any() ? source : source.Where(t => FindExpression(t, toSearch, props.Select(x => x.Name)));
            //return !props.Any() ? source : source.Where(t => FindExpression(typeof(TSource), t, toSearch));
        }

        /// <summary>
        /// Filter source by multiple filters
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="filtersList"></param>
        /// <returns></returns>
        public static IQueryable<TSource> FilterSourceByFilters<TSource>(this IQueryable<TSource> source, IEnumerable<PageRequestFilter> filtersList)
        {
            var pageRequestFilters = filtersList.ToList();
            var distinctPageRequestFilters = filtersList.DistinctBy(x => x.Propriety).ToList();
            if (!pageRequestFilters.Any() || !distinctPageRequestFilters.Any()) return source;

            foreach (var filterDistinct in distinctPageRequestFilters)
            {
                var temp = new List<TSource>();

                switch (filterDistinct.Operator)
                { 
                    case Criteria.Equals:

                        foreach (var filter in pageRequestFilters.Where(x=> x.Propriety == filterDistinct.Propriety))
                        {
                            var result = source.EqualsByProprietyValue(filter.Propriety, filter.Value);
                            if (result != null)
                                temp.AddRange(result);
                        }

                        var listIdTemp = temp.Select(s => s.GetPropertyValue("Id").ToStringIgnoreNull())
                            .Where(x=> !string.IsNullOrEmpty(x)).ToList();
                        source = source.Where(x => listIdTemp.Contains(x.GetPropertyValue("Id").ToStringIgnoreNull()));

                        break;
                    case Criteria.Contains:
                        temp.AddRange(source.ContainsByProprietyValue(filterDistinct.Propriety, filterDistinct.Value));
                        break;
                    case Criteria.Greater:
                        temp.AddRange(source.CompareByProprietyValue(filterDistinct.Propriety, filterDistinct.Value, CompareType.Greater));
                        break;
                    case Criteria.Less:
                        temp.AddRange(source.CompareByProprietyValue(filterDistinct.Propriety, filterDistinct.Value, CompareType.Less));
                        break;
                    case Criteria.LessOrEqual:
                        temp.AddRange(source.CompareByProprietyValue(filterDistinct.Propriety, filterDistinct.Value, CompareType.LessOrEqual));
                        break;
                    case Criteria.GreaterOrEqual:
                        temp.AddRange(source.CompareByProprietyValue(filterDistinct.Propriety, filterDistinct.Value, CompareType.GreaterOrEqual));
                        break;
                    case Criteria.StartWith:
                        temp.AddRange(source.StartWithByProprietyValue(filterDistinct.Propriety, filterDistinct.Value.ToString()));
                        break;
                    case Criteria.EndWith:
                        temp.AddRange(source.EndWithByProprietyValue(filterDistinct.Propriety, filterDistinct.Value.ToString()));
                        break;
                }
            }

            //source =  source.Where(x => temp.Contains(x));

            return source;
        }

        /// <summary>
        /// Contains with reflection
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IQueryable<TSource> ContainsByProprietyValue<TSource>(this IQueryable<TSource> source, string property, object value)
        {
            if (property.IsNullOrEmpty() || value == null)
                return source;

            return source.Where(t => t.GetPropertyValue(property).ToString().Contains(value.ToString()));
        }

        /// <summary>
        /// Start with by propriety name
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IQueryable<TSource> StartWithByProprietyValue<TSource>(this IQueryable<TSource> source, string property, string value)
        {
            if (property.IsNullOrEmpty() || value == null)
                return source;

            return source.Where(t => t.GetPropertyValue(property).ToString().StartsWith(value));
        }

        /// <summary>
        /// Not start with
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IQueryable<TSource> NotStartWithByProprietyValue<TSource>(this IQueryable<TSource> source, string property, string value)
        {
            if (property.IsNullOrEmpty() || value == null)
                return source;

            return source.Where(t => !t.GetPropertyValue(property).ToString().StartsWith(value));
        }

        /// <summary>
        /// End with by propriety name
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IQueryable<TSource> EndWithByProprietyValue<TSource>(this IQueryable<TSource> source, string property, string value)
        {
            if (property.IsNullOrEmpty() || value == null)
                return source;

            return source.Where(t => t.GetPropertyValue(property).ToString().EndsWith(value));
        }

        /// <summary>
        /// Not end
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IQueryable<TSource> NotEndWithByProprietyValue<TSource>(this IQueryable<TSource> source, string property, string value)
        {
            if (property.IsNullOrEmpty() || value == null)
                return source;

            return source.Where(t => !t.GetPropertyValue(property).ToString().EndsWith(value));
        }

        /// <summary>
        /// Equals with reflection
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IQueryable<TSource> EqualsByProprietyValue<TSource>(this IQueryable<TSource> source, string property, object value)
        {
            if (property.IsNullOrEmpty() || value == null)
                return source;

            return source.Where(t => string.Equals(t.GetPropertyValue(property).ToStringIgnoreNull(), value.ToStringIgnoreNull(), StringComparison.InvariantCultureIgnoreCase) );
        }

        /// <summary>
        /// Not equals by propriety name
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IQueryable<TSource> NotEqualsByProprietyValue<TSource>(this IQueryable<TSource> source, string property, object value)
        {
            if (property.IsNullOrEmpty() || value == null)
                return source;

            return source.Where(t => t.GetPropertyValue(property).ToString() != value.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public static IQueryable<TSource> CompareByProprietyValue<TSource>(this IQueryable<TSource> source, string property, object value, CompareType operation)
        {
            if (property.IsNullOrEmpty() || value == null)
                return source;
            return source.Where(t => CompareObjects(t.GetPropertyValue(property), value, operation));
        }

        #region Helpers

        /// <summary>
        /// Compare 
        /// </summary>
        private static readonly Func<object, object, CompareType, bool> CompareObjects = delegate (object left, object right, CompareType operation)
        {
            if (left == null) return false;
            var output = true;
            switch (left)
            {
                case string str:
                    {
                        var compareResult = string.CompareOrdinal(str, right.ToString());
                        output = CompareObjectsByCompareResult(compareResult, operation);
                    }
                    break;
                case DateTime leftDate:
                    {
                        var valid = DateTime.TryParse(right.ToString(), out var rightDate);
                        output = valid && CompareObjectsByCompareResult(leftDate.CompareTo(rightDate), operation);
                    }
                    break;
                case double doubleNumber:
                    {
                        var valid = double.TryParse(right.ToString(), out var doubleRNumber);
                        output = valid && CompareObjectsByCompareResult(doubleNumber.CompareTo(doubleRNumber), operation);
                    }
                    break;
                case int intNumber:
                    {
                        var valid = int.TryParse(right.ToString(), out var intRNumber);
                        output = valid && CompareObjectsByCompareResult(intNumber.CompareTo(intRNumber), operation);
                    }
                    break;
                case decimal decNumber:
                    {
                        var valid = decimal.TryParse(right.ToString(), out var decRNumber);
                        output = valid && CompareObjectsByCompareResult(decNumber.CompareTo(decRNumber), operation);
                    }
                    break;
                case bool leftBool:
                    {
                        var valid = bool.TryParse(right.ToString(), out var rightBool);
                        output = valid && CompareObjectsByCompareResult(leftBool.CompareTo(rightBool), operation);
                    }
                    break;
            }

            return output;
        };

        /// <summary>
        /// Compare objects by compare result
        /// </summary>
        /// <param name="compareResult"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        private static bool CompareObjectsByCompareResult(int compareResult, CompareType operation)
            => operation == CompareType.Less && compareResult < 0
               || operation == CompareType.Greater && compareResult > 0
               || (operation == CompareType.GreaterOrEqual ||
                   operation == CompareType.LessOrEqual) && compareResult == 0;

        /// <summary>
        /// Match any object prop value the regular exp 
        /// </summary>
        private static readonly Func<object, string, IEnumerable<string>, bool> MatchRegEx =
            delegate (object o, string regExp, IEnumerable<string> props)
            {
                var values = props.Select(x => o.GetPropertyValue(x).ToString()).ToList();
                return values.Any(c => Regex.Match(c, regExp).Success);
            };

        /// <summary>
        /// Find string expression in object values
        /// </summary>
        private static readonly Func<object, string, IEnumerable<string>, bool> FindExpression =
            delegate (object o, string exp, IEnumerable<string> props)
            {
                var values = props.Select(x => o.GetPropertyValue(x)?.ToString()).Where(x => x != null).ToList();
                return values.Any(c => c.IndexOf(exp, 0, StringComparison.CurrentCultureIgnoreCase) != -1);
            };


        /// <summary>
        /// Set period data time by property name 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="filtersList"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static IQueryable<TSource> SetPeriodByProperty<TSource>(this IQueryable<TSource> source,
            IEnumerable<PageRequestFilter> filtersList, string propertyName)
        {
            var pageRequestFilters = filtersList.ToList();
            var selectStartDate = pageRequestFilters.FirstOrDefault(x => string.Equals(x.Propriety.Trim(), "StartDate", StringComparison.CurrentCultureIgnoreCase))?.Value;
            var selectEndDate = pageRequestFilters.FirstOrDefault(x => string.Equals(x.Propriety.Trim(), "EndDate", StringComparison.CurrentCultureIgnoreCase))?.Value;

            var startDate = selectStartDate.StringToDateTime() ?? DateTime.Now.AddDays(-30);
            var endDate = selectEndDate.StringToDateTime() ?? DateTime.Now;

            return source.Where(x => (DateTime?)x.GetPropertyValue(propertyName) != null
                                     && (DateTime?)x.GetPropertyValue(propertyName) >= startDate
                                     && (DateTime?)x.GetPropertyValue(propertyName) <= endDate);
        }

        #endregion
    }
}
