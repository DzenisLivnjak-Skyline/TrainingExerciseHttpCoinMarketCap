using System;
using Skyline.DataMiner.Scripting;
using Newtonsoft.Json;

public static class Parameter
{
    public const int ActiveCryptocurrencies = 209;
    public const int TotalCryptocurrencies = 210;
    public const int ActiveExchanges = 211;
    public const int TotalExchanges = 212;
    public const int EthDominance = 213;
    public const int BtcDominance = 214;
    public const int LatestQuoteLastUpdate = 215;
    public const int LatestQuotesContent = 216;
}

public static class QAction
{
    public static void Run(SLProtocol protocol)

    {
        var @params = (object[])protocol.GetParameters(new uint[] { 216, 217 });
        var rawJson = Convert.ToString(@params[0]);
        var statusCode = Convert.ToString(@params[1]);

        if (string.IsNullOrWhiteSpace(rawJson) || !statusCode.Contains("200"))
        {
            protocol.Log($"QA{protocol.QActionID}|run| Empty or invalid status code.", LogType.Error, LogLevel.NoLogging);
            return;
        }
        protocol.Log($"QA{protocol.QActionID}|latestquotes|{rawJson}", LogType.DebugInfo, LogLevel.NoLogging);
        try
        {
            var latestquotes = JsonConvert.DeserializeObject<LatestQuotes>(rawJson);
            protocol.SetParameters(
                new[] { Parameter.ActiveCryptocurrencies, Parameter.TotalCryptocurrencies, Parameter.ActiveExchanges, Parameter.TotalExchanges, Parameter.EthDominance, Parameter.BtcDominance, Parameter.LatestQuoteLastUpdate },
                new object[] {latestquotes.Data.ActiveCryptocurrencies, latestquotes.Data.TotalCryptocurrencies, latestquotes.Data.ActiveExchanges, latestquotes.Data.TotalExchanges, latestquotes.Data.EthDominance, latestquotes.Data.BtcDominance, latestquotes.Data.LastUpdated });
        }
        catch (Exception ex)
        {
            protocol.Log("QA4|Exception: " + ex);
        }
    }
    private static double ConvertToEpoch(string isoTime)
    {
        if (DateTime.TryParse(isoTime, out DateTime dt))
        {
            return (dt.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds;
        }
        return 0;
    }
}
