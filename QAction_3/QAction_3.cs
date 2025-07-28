using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Skyline.DataMiner.Net.Helper;
using Skyline.DataMiner.Scripting;
using SLNetMessages = Skyline.DataMiner.Net.Messages;

public static class QAction
{
    public static void Run(SLProtocolExt protocol)
    {
        try
        {
            object[] @params = (object[])protocol.GetParameters(new uint[] { 300, 301 });
            string response = Convert.ToString(@params[0]);
            string responseCode = Convert.ToString(@params[1]);

            if (!responseCode.Contains("200") || String.IsNullOrWhiteSpace(response))
            {
                protocol.Log("QA3|Invalid response", LogType.Error, LogLevel.NoLogging);
                FillFallback(protocol);
                return;
            }

            Categories categories = JsonConvert.DeserializeObject<Categories>(response);
            if (categories?.Data == null)
            {
                protocol.Log("QA3|No data in response", LogType.Error, LogLevel.NoLogging);
                FillFallback(protocol);
                return;
            }

            List<object[]> tableData = new List<object[]>();
            foreach (var item in categories.Data)
            {
                tableData.Add(new object[]
                {
                    item.Id,
                    item.Name,
                    item.NumTokens,
                    item.AvgPriceChange,
                    item.MarketCap,
                    item.Volume,
                    item.LastUpdated.ToOADate(),
                    0
                });
            }

            protocol.FillArray(200, tableData, NotifyProtocol.SaveOption.Full);
        }
        catch (Exception ex)
        {
            protocol.Log($"QA3|Error: {ex}", LogType.Error, LogLevel.NoLogging);
            FillFallback(protocol);
        }
    }

    private static void FillFallback(SLProtocolExt protocol)
    {
        try
        {
            uint[] columnIds = new uint[] { 201, 202 };
            object[] response = (object[])protocol.NotifyProtocol(
                (int)SLNetMessages.NotifyType.NT_GET_TABLE_COLUMNS,
                200,
                columnIds);

            if (response == null || response.Length < 2)
            {
                protocol.Log("QA3|Fallback failed - no table data", LogType.Error, LogLevel.NoLogging);
                return;
            }

            object[] keys = response[0] as object[];
            object[] names = response[1] as object[];

            if (keys == null || names == null || keys.Length == 0)
            {
                protocol.Log("QA3|Fallback failed - invalid table data", LogType.Error, LogLevel.NoLogging);
                return;
            }

            List<object[]> fallbackData = new List<object[]>();
            double exceptionValue = -101;

            for (int i = 0; i < keys.Length; i++)
            {
                fallbackData.Add(new object[]
                {
                    keys[i],
                    names[i],
                    exceptionValue,
                    exceptionValue,
                    exceptionValue,
                    exceptionValue,
                    exceptionValue,
                    0
                });
            }

            protocol.FillArray(200, fallbackData, NotifyProtocol.SaveOption.Full);
        }
        catch (Exception ex)
        {
            protocol.Log($"QA3|Fallback error: {ex}", LogType.Error, LogLevel.NoLogging);
        }
    }
}