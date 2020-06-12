using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_SeedCurrencies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Crm",
                table: "JobPositions",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.InsertData(
                schema: "Crm",
                table: "Currencies",
                columns: new[] { "Code", "DecimalDigits", "Name", "NativeSymbol", "PluralName", "Rounding", "Symbol" },
                values: new object[,]
                {
                    { "USD", 0, "US Dollar", "$", null, 0m, "$" },
                    { "PHP", 0, "Philippine Peso", "₱", "Philippine pesos", 0m, "₱" },
                    { "PEN", 0, "Peruvian Nuevo Sol", "S/.", "Peruvian nuevos soles", 0m, "S/." },
                    { "PAB", 0, "Panamanian Balboa", "B/.", "Panamanian balboas", 0m, "B/." },
                    { "OMR", 0, "Omani Rial", "ر.ع.‏", "Omani rials", 0m, "OMR" },
                    { "NZD", 0, "New Zealand Dollar", "$", "New Zealand dollars", 0m, "NZ$" },
                    { "NPR", 0, "Nepalese Rupee", "नेरू", "Nepalese rupees", 0m, "NPRs" },
                    { "NOK", 0, "Norwegian Krone", "kr", "Norwegian kroner", 0m, "Nkr" },
                    { "NIO", 0, "Nicaraguan Córdoba", "C$", "Nicaraguan córdobas", 0m, "C$" },
                    { "NGN", 0, "Nigerian Naira", "₦", "Nigerian nairas", 0m, "₦" },
                    { "NAD", 0, "Namibian Dollar", "N$", "Namibian dollars", 0m, "N$" },
                    { "MZN", 0, "Mozambican Metical", "MTn", "Mozambican meticals", 0m, "MTn" },
                    { "MYR", 0, "Malaysian Ringgit", "RM", "Malaysian ringgits", 0m, "RM" },
                    { "MXN", 0, "Mexican Peso", "$", "Mexican pesos", 0m, "MX$" },
                    { "MUR", 0, "Mauritian Rupee", "MURs", "Mauritian rupees", 0m, "MURs" },
                    { "MOP", 0, "Macanese Pataca", "MOP$", "Macanese patacas", 0m, "MOP$" },
                    { "MMK", 0, "Myanma Kyat", "K", "Myanma kyats", 0m, "MMK" },
                    { "MKD", 0, "Macedonian Denar", "MKD", "Macedonian denari", 0m, "MKD" },
                    { "MGA", 0, "Malagasy Ariary", "MGA", "Malagasy Ariaries", 0m, "MGA" },
                    { "MDL", 0, "Moldovan Leu", "MDL", "Moldovan lei", 0m, "MDL" },
                    { "MAD", 0, "Moroccan Dirham", "د.م.‏", "Moroccan dirhams", 0m, "MAD" },
                    { "LYD", 0, "Libyan Dinar", "د.ل.‏", "Libyan dinars", 0m, "LD" },
                    { "LVL", 0, "Latvian Lats", "Ls", "Latvian lati", 0m, "Ls" },
                    { "LTL", 0, "Lithuanian Litas", "Lt", "Lithuanian litai", 0m, "Lt" },
                    { "LKR", 0, "Sri Lankan Rupee", "SL Re", "Sri Lankan rupees", 0m, "SLRs" },
                    { "LBP", 0, "Lebanese Pound", "ل.ل.‏", "Lebanese pounds", 0m, "LB£" },
                    { "PKR", 0, "Pakistani Rupee", "₨", "Pakistani rupees", 0m, "PKRs" },
                    { "PLN", 0, "Polish Zloty", "zł", "Polish zlotys", 0m, "zł" },
                    { "PYG", 0, "Paraguayan Guarani", "₲", "Paraguayan guaranis", 0m, "₲" },
                    { "QAR", 0, "Qatari Rial", "ر.ق.‏", "Qatari rials", 0m, "QR" },
                    { "YER", 0, "Yemeni Rial", "ر.ي.‏", "Yemeni rials", 0m, "YR" },
                    { "XOF", 0, "CFA Franc BCEAO", "CFA", "CFA francs BCEAO", 0m, "CFA" },
                    { "XAF", 0, "CFA Franc BEAC", "FCFA", "CFA francs BEAC", 0m, "FCFA" },
                    { "VND", 0, "Vietnamese Dong", "₫", "Vietnamese dong", 0m, "₫" },
                    { "VEF", 0, "Venezuelan Bolívar", "Bs.F.", "Venezuelan bolívars", 0m, "Bs.F." },
                    { "UZS", 0, "Uzbekistan Som", "UZS", "Uzbekistan som", 0m, "UZS" },
                    { "UYU", 0, "Uruguayan Peso", "$", "Uruguayan pesos", 0m, "$U" },
                    { "UGX", 0, "Ugandan Shilling", "USh", "Ugandan shillings", 0m, "USh" },
                    { "UAH", 0, "Ukrainian Hryvnia", "₴", "Ukrainian hryvnias", 0m, "₴" },
                    { "TZS", 0, "Tanzanian Shilling", "TSh", "Tanzanian shillings", 0m, "TSh" },
                    { "TWD", 0, "New Taiwan Dollar", "NT$", "New Taiwan dollars", 0m, "NT$" },
                    { "TTD", 0, "Trinidad and Tobago Dollar", "$", "Trinidad and Tobago dollars", 0m, "TT$" },
                    { "KZT", 0, "Kazakhstani Tenge", "тңг.", "Kazakhstani tenges", 0m, "KZT" },
                    { "TRY", 0, "Turkish Lira", "TL", "Turkish Lira", 0m, "TL" },
                    { "TND", 0, "Tunisian Dinar", "د.ت.‏", "Tunisian dinars", 0m, "DT" },
                    { "THB", 0, "Thai Baht", "฿", "Thai baht", 0m, "฿" },
                    { "SYP", 0, "Syrian Pound", "ل.س.‏", "Syrian pounds", 0m, "SY£" },
                    { "SOS", 0, "Somali Shilling", "Ssh", "Somali shillings", 0m, "Ssh" },
                    { "SGD", 0, "Singapore Dollar", "$", "Singapore dollars", 0m, "S$" },
                    { "SEK", 0, "Swedish Krona", "kr", "Swedish kronor", 0m, "Skr" },
                    { "SDG", 0, "Sudanese Pound", "SDG", "Sudanese pounds", 0m, "SDG" },
                    { "SAR", 0, "Saudi Riyal", "ر.س.‏", "Saudi riyals", 0m, "SR" },
                    { "RWF", 0, "Rwandan Franc", "FR", "Rwandan francs", 0m, "RWF" },
                    { "RUB", 0, "Russian Ruble", "руб.", "Russian rubles", 0m, "RUB" },
                    { "RSD", 0, "Serbian Dinar", "дин.", "Serbian dinars", 0m, "din." },
                    { "RON", 0, "Romanian Leu", "RON", "Romanian lei", 0m, "RON" },
                    { "TOP", 0, "Tongan Paʻanga", "T$", "Tongan paʻanga", 0m, "T$" },
                    { "KWD", 0, "Kuwaiti Dinar", "د.ك.‏", "Kuwaiti dinars", 0m, "KD" },
                    { "KRW", 0, "South Korean Won", "₩", "South Korean won", 0m, "₩" },
                    { "KMF", 0, "Comorian Franc", "FC", "Comorian francs", 0m, "CF" },
                    { "CRC", 0, "Costa Rican Colón", "₡", "Costa Rican colóns", 0m, "₡" },
                    { "COP", 0, "Colombian Peso", "$", "Colombian pesos", 0m, "CO$" },
                    { "CNY", 0, "Chinese Yuan", "CN¥", "Chinese yuan", 0m, "CN¥" },
                    { "CLP", 0, "Chilean Peso", "$", "Chilean pesos", 0m, "CL$" },
                    { "CHF", 0, "Swiss Franc", "CHF", "Swiss francs", 0.05m, "CHF" },
                    { "CDF", 0, "Congolese Franc", "FrCD", "Congolese francs", 0m, "CDF" },
                    { "BZD", 0, "Belize Dollar", "$", "Belize dollars", 0m, "BZ$" },
                    { "BYR", 0, "Belarusian Ruble", "BYR", "Belarusian rubles", 0m, "BYR" },
                    { "BWP", 0, "Botswanan Pula", "P", "Botswanan pulas", 0m, "BWP" },
                    { "BRL", 0, "Brazilian Real", "R$", "Brazilian reals", 0m, "R$" },
                    { "BOB", 0, "Bolivian Boliviano", "Bs", "Bolivian bolivianos", 0m, "Bs" },
                    { "BND", 0, "Brunei Dollar", "$", "Brunei dollars", 0m, "BN$" },
                    { "CVE", 0, "Cape Verdean Escudo", "CV$", "Cape Verdean escudos", 0m, "CV$" },
                    { "BIF", 0, "Burundian Franc", "FBu", "Burundian francs", 0m, "FBu" },
                    { "BGN", 0, "Bulgarian Lev", "лв.", "Bulgarian leva", 0m, "BGN" },
                    { "BDT", 0, "Bangladeshi Taka", "৳", "Bangladeshi takas", 0m, "Tk" },
                    { "BAM", 0, "Bosnia-Herzegovina Convertible Mark", "KM", "Bosnia-Herzegovina convertible marks", 0m, "KM" },
                    { "AZN", 0, "Azerbaijani Manat", "ман.", "Azerbaijani manats", 0m, "man." },
                    { "AUD", 0, "Australian Dollar", "$", "Australian dollars", 0m, "AU$" },
                    { "ARS", 0, "Argentine Peso", "$", "Argentine pesos", 0m, "AR$" },
                    { "AMD", 0, "Armenian Dram", "դր.", "Armenian drams", 0m, "AMD" },
                    { "ALL", 0, "Albanian Lek", "Lek", "Albanian lekë", 0m, "ALL" },
                    { "AFN", 0, "Afghan Afghani", "؋", "Afghan Afghanis", 0m, "Af" },
                    { "AED", 0, "United Arab Emirates Dirham", "د.إ.‏", "UAE dirhams", 0m, "AED" },
                    { "EUR", 0, "Euro", "€", "euros", 0m, "€" },
                    { "CAD", 0, "Canadian Dollar", "$", "Canadian dollars", 0m, "CA$" },
                    { "BHD", 0, "Bahraini Dinar", "د.ب.‏", "Bahraini dinars", 0m, "BD" },
                    { "ZAR", 0, "South African Rand", "R", "South African rand", 0m, "R" },
                    { "CZK", 0, "Czech Republic Koruna", "Kč", "Czech Republic korunas", 0m, "Kč" },
                    { "DKK", 0, "Danish Krone", "kr", "Danish kroner", 0m, "Dkr" },
                    { "KHR", 0, "Cambodian Riel", "៛", "Cambodian riels", 0m, "KHR" },
                    { "KES", 0, "Kenyan Shilling", "Ksh", "Kenyan shillings", 0m, "Ksh" },
                    { "JPY", 0, "Japanese Yen", "￥", "Japanese yen", 0m, "¥" },
                    { "JOD", 0, "Jordanian Dinar", "د.أ.‏", "Jordanian dinars", 0m, "JD" },
                    { "JMD", 0, "Jamaican Dollar", "$", "Jamaican dollars", 0m, "J$" },
                    { "ISK", 0, "Icelandic Króna", "kr", "Icelandic krónur", 0m, "Ikr" },
                    { "IRR", 0, "Iranian Rial", "﷼", "Iranian rials", 0m, "IRR" },
                    { "IQD", 0, "Iraqi Dinar", "د.ع.‏", "Iraqi dinars", 0m, "IQD" },
                    { "INR", 0, "Indian Rupee", "টকা", "Indian rupees", 0m, "Rs" },
                    { "ILS", 0, "Israeli New Sheqel", "₪", "Israeli new sheqels", 0m, "₪" },
                    { "IDR", 0, "Indonesian Rupiah", "Rp", "Indonesian rupiahs", 0m, "Rp" },
                    { "HUF", 0, "Hungarian Forint", "Ft", "Hungarian forints", 0m, "Ft" },
                    { "DJF", 0, "Djiboutian Franc", "Fdj", "Djiboutian francs", 0m, "Fdj" },
                    { "HRK", 0, "Croatian Kuna", "kn", "Croatian kunas", 0m, "kn" },
                    { "HKD", 0, "Hong Kong Dollar", "$", "Hong Kong dollars", 0m, "HK$" },
                    { "GTQ", 0, "Guatemalan Quetzal", "Q", "Guatemalan quetzals", 0m, "GTQ" },
                    { "GNF", 0, "Guinean Franc", "FG", "Guinean francs", 0m, "FG" },
                    { "GHS", 0, "Ghanaian Cedi", "GH₵", "Ghanaian cedis", 0m, "GH₵" },
                    { "GEL", 0, "Georgian Lari", "GEL", "Georgian laris", 0m, "GEL" },
                    { "GBP", 0, "British Pound Sterling", "£", "British pounds sterling", 0m, "£" },
                    { "ETB", 0, "Ethiopian Birr", "Br", "Ethiopian birrs", 0m, "Br" },
                    { "ERN", 0, "Eritrean Nakfa", "Nfk", "Eritrean nakfas", 0m, "Nfk" },
                    { "EGP", 0, "Egyptian Pound", "ج.م.‏", "Egyptian pounds", 0m, "EGP" },
                    { "EEK", 0, "Estonian Kroon", "kr", "Estonian kroons", 0m, "Ekr" },
                    { "DZD", 0, "Algerian Dinar", "د.ج.‏", "Algerian dinars", 0m, "DA" },
                    { "DOP", 0, "Dominican Peso", "RD$", "Dominican pesos", 0m, "RD$" },
                    { "HNL", 0, "Honduran Lempira", "L", "Honduran lempiras", 0m, "HNL" },
                    { "ZMK", 0, "Zambian Kwacha", "ZK", "Zambian kwachas", 0m, "ZK" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "AED");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "AFN");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "ALL");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "AMD");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "ARS");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "AUD");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "AZN");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "BAM");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "BDT");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "BGN");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "BHD");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "BIF");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "BND");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "BOB");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "BRL");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "BWP");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "BYR");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "BZD");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "CAD");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "CDF");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "CHF");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "CLP");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "CNY");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "COP");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "CRC");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "CVE");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "CZK");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "DJF");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "DKK");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "DOP");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "DZD");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "EEK");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "EGP");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "ERN");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "ETB");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "EUR");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "GBP");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "GEL");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "GHS");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "GNF");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "GTQ");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "HKD");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "HNL");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "HRK");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "HUF");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "IDR");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "ILS");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "INR");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "IQD");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "IRR");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "ISK");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "JMD");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "JOD");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "JPY");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "KES");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "KHR");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "KMF");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "KRW");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "KWD");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "KZT");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "LBP");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "LKR");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "LTL");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "LVL");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "LYD");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "MAD");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "MDL");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "MGA");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "MKD");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "MMK");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "MOP");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "MUR");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "MXN");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "MYR");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "MZN");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "NAD");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "NGN");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "NIO");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "NOK");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "NPR");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "NZD");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "OMR");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "PAB");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "PEN");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "PHP");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "PKR");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "PLN");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "PYG");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "QAR");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "RON");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "RSD");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "RUB");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "RWF");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "SAR");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "SDG");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "SEK");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "SGD");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "SOS");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "SYP");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "THB");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "TND");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "TOP");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "TRY");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "TTD");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "TWD");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "TZS");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "UAH");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "UGX");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "USD");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "UYU");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "UZS");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "VEF");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "VND");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "XAF");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "XOF");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "YER");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "ZAR");

            migrationBuilder.DeleteData(
                schema: "Crm",
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "ZMK");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Crm",
                table: "JobPositions",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
