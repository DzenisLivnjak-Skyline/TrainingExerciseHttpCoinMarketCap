using System;
using Newtonsoft.Json;
using Skyline.DataMiner.Scripting;

public static class QAction
{
    public static void Run(SLProtocol protocol)
    {
        var @params = (object[])protocol.GetParameters(new uint[] { Parameter.latestquotescontent, Parameter.lateststatusquotes });
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
#pragma warning disable SA1118 
            _ = protocol.SetParameters(
                new[]
                {
                    Parameter.activecryptocurrencies,
                    Parameter.totalcryptocurrencies,
                    Parameter.activeexchanges,
                    Parameter.totalexchanges,
                    Parameter.ethdominance,
                    Parameter.btcdominance,
                    Parameter.latestquotelastupdate,
                },
                new object[]
                {
                    latestquotes.Data.ActiveCryptocurrencies,
                    latestquotes.Data.TotalCryptocurrencies,
                    latestquotes.Data.ActiveExchanges,
                    latestquotes.Data.TotalExchanges,
                    latestquotes.Data.EthDominance,
                    latestquotes.Data.BtcDominance,
                    latestquotes.Data.LastUpdated,
                });
#pragma warning restore SA1118 // Parameter should not span multiple lines
        }
        catch (Exception ex)
        {
            protocol.Log("QA4|Exception: " + ex);
        }
    }
}