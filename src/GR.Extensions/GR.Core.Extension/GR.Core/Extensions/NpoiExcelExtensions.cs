using System;
using System.Globalization;
using NPOI.SS.UserModel;

namespace GR.Core.Extensions
{
    public static class NpoiExcelExtensions
    {


        public static string CellStringValue(this ICell cell)
        {
            var cellData = string.Empty;

            if (cell == null) return cellData;

            switch (cell.CellType)
            {
                case CellType.Numeric:
                    cellData = string.Copy(cell.NumericCellValue.ToString());
                    break;
                case CellType.String:
                    cellData = string.Copy(cell.StringCellValue);
                    break;
                case CellType.Boolean:
                    cellData = string.Copy(cell.BooleanCellValue.ToString());
                    break;
                case CellType.Unknown:
                case CellType.Formula:
                case CellType.Blank:
                case CellType.Error:
                    break;
                default:
                    return string.Empty;
            }

            return cellData?.Trim();
        }


        public static DateTime? CellDateTimeValue(this ICell cell)
        {
            if (cell == null) return null;


            switch (cell.CellType)
            {
                case CellType.String:
                    {
                        var date = cell.StringCellValue?.Trim();
                        DateTime dDate;

                        if (string.IsNullOrEmpty(cell.StringCellValue))
                            return null;

                        if (DateTime.TryParse(date, out dDate))
                            return DateTime.Parse(date) != DateTime.MinValue ? DateTime.Parse(date) : (DateTime?)null;

                        if (DateTime.TryParseExact(cell.StringCellValue, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dDate))
                            return dDate;

                        if (DateTime.TryParseExact(date, "M/d/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out dDate))
                            return dDate;

                        if (DateTime.TryParseExact(date, "M/d/yy", new CultureInfo("en-US"), DateTimeStyles.None, out dDate))
                            return dDate;

                        return null;
                    }
                case CellType.Numeric:
                    return cell.DateCellValue != DateTime.MinValue ? cell.DateCellValue :
                        (DateTime?)null;
                case CellType.Blank:
                    return null;
                case CellType.Unknown:
                    break;
                case CellType.Formula:
                    break;
                case CellType.Boolean:
                    break;
                case CellType.Error:
                    break;
                default:
                    {
                        return null;
                    }
            }

            return null;
        }


        public static decimal? CellDecimalValue(this ICell cell)
        {
            if (cell == null) return null;


            if (cell.CellType == CellType.Numeric)
            {
                return Convert.ToDecimal(cell.NumericCellValue);
            }

            if (string.IsNullOrEmpty(cell.StringCellValue))
                return null;

            decimal output;

            
            if (decimal.TryParse(cell.StringCellValue, NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign | NumberStyles.AllowExponent, CultureInfo.CurrentCulture, out output))
                return Math.Round(output, 2, MidpointRounding.AwayFromZero);


            return null;
        }

    }
}
