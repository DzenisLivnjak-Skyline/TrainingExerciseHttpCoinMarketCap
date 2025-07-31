using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Skyline.DataMiner.Scripting;
using SLNetMessages = Skyline.DataMiner.Net.Messages;

public static class QAction
{
    public static void Run(SLProtocolExt protocol)
    {
        try
        {
            object[] @params = (object[])protocol.GetParameters(new uint[] { 3, 4 });
            string response = Convert.ToString(@params[0]);
            string responseCode = Convert.ToString(@params[1]);

            if (!responseCode.Contains("200") || String.IsNullOrWhiteSpace(response))
            {
                protocol.Log("QA5|Invalid response", LogType.Error, LogLevel.NoLogging);
                FillFallback(protocol);
                return;
            }

            LatestListings listings = JsonConvert.DeserializeObject<LatestListings>(response);
            if (listings?.Data == null || listings.Data.Count == 0)
            {
                protocol.Log("QA5|No data in response", LogType.Error, LogLevel.NoLogging);
                FillFallback(protocol);
                return;
            }

            List<object[]> tableData = new List<object[]>();
            foreach (var item in listings.Data)
            {
                tableData.Add(new object[]
                {
                    item.Id.ToString(),
                    item.Name ?? string.Empty,
                    item.Quote?.USD?.Price ?? -101d,
                    item.DateAdded.ToOADate(),
                    item.LastUpdated.ToOADate(),
                    item.Symbol ?? string.Empty,
                    item.Quote?.USD?.PercentChange24h ?? -101d,
                });
            }

            protocol.FillArray(100, tableData, NotifyProtocol.SaveOption.Full);
        }
        catch (Exception ex)
        {
            protocol.Log($"QA5|Error: {ex}", LogType.Error, LogLevel.NoLogging);
            FillFallback(protocol);
        }
    }

    private static void FillFallback(SLProtocolExt protocol)
    {
        try
        {
            uint[] columnIds = new uint[] { 101, 102 };
            object[] response = (object[])protocol.NotifyProtocol(
                (int)SLNetMessages.NotifyType.NT_GET_TABLE_COLUMNS,
                100,
                columnIds);

            if (response == null || response.Length < 2)
            {
                protocol.Log("QA5|Fallback failed - no table data", LogType.Error, LogLevel.NoLogging);
                return;
            }

            object[] keys = response[0] as object[];
            object[] names = response[1] as object[];

            if (keys == null || names == null || keys.Length == 0)
            {
                protocol.Log("QA5|Fallback failed - invalid table data", LogType.Error, LogLevel.NoLogging);
                return;
            }

            double exceptionValue = -101;
            int rowCount = keys.Length;

            // Prepare columns
            object[] colKeys = keys;
            object[] colNames = names;
            object[] colPrice = new object[rowCount];
            object[] colDateAdded = new object[rowCount];
            object[] colLastUpdated = new object[rowCount];
            object[] colSymbol = new object[rowCount];
            object[] colPercentChange24h = new object[rowCount];

            for (int i = 0; i < rowCount; i++)
            {
                colPrice[i] = exceptionValue;
                colDateAdded[i] = exceptionValue;
                colLastUpdated[i] = exceptionValue;
                colSymbol[i] = "-101";
                colPercentChange24h[i] = exceptionValue;
            }

            protocol.FillArray(100, new object[] {
                colKeys, colNames, colPrice, colDateAdded, colLastUpdated, colSymbol, colPercentChange24h
            });
        }
        catch (Exception ex)
        {
            protocol.Log($"QA5|Fallback error: {ex}", LogType.Error, LogLevel.NoLogging);
        }
    }
}