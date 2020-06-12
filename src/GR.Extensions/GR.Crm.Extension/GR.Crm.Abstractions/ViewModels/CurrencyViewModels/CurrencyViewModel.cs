using GR.Crm.Abstractions.Models;
using Newtonsoft.Json;

namespace GR.Crm.Abstractions.ViewModels.CurrencyViewModels
{
    public sealed class CurrencyViewModel : Currency
    {
        /// <summary>
        /// Native symbol
        /// </summary>
        [JsonProperty("symbol_native")] public override string NativeSymbol { get; set; }

        /// <summary>
        /// Plural name
        /// </summary>
        [JsonProperty("name_plural")] public override string PluralName { get; set; }
    }
}