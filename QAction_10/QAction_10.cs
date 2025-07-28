using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Skyline.DataMiner.Scripting;

public static class QAction
{
    public static void Run(SLProtocol protocol)
    {
        try
        {
            // 1. Get Bearer token (id 5)
            string bearerToken = protocol.GetParameter(5) as string;
            if (String.IsNullOrWhiteSpace(bearerToken)) return;

            // 2. Decide: Refresh row, or full table
            string rowKey = protocol.RowKey;  // RowKey is non-empty when button is clicked in a row

            if (!string.IsNullOrEmpty(rowKey))
            {
                // Row Refresh (button clicked)
                var row = protocol.GetRow(200, rowKey);
                if (row == null) return;

                var categoryId = row[0]?.ToString(); // Assuming 201 is the first column
                if (string.IsNullOrEmpty(categoryId)) return;

                string url = $"/api/custom/coinmarketcap?content=category&id={categoryId}";
                var response = protocol.CallHttpRest("GET", url, bearerToken);

                // Parse single category and update just this row
                if (!string.IsNullOrEmpty(response))
                {
                    var category = JObject.Parse(response);
                    object[] updatedRow = new object[]
                    {
                        category["id"]?.ToString(),                  // 201
                        category["name"]?.ToString(),                // 202
                        category["num_tokens"] ?? 0,                 // 203
                        category["price_change"] ?? 0,               // 204
                        category["market_cap"] ?? 0,                 // 205
                        category["volume"] ?? 0,                     // 206
                        category["last_updated"] ?? 0,               // 207
                        0                                            // 208 (button)
                    };
                    protocol.UpdateRow(200, rowKey, updatedRow);
                }
            }
            else
            {
                // Full table update (startup, timer)
                string url = "/api/custom/coinmarketcap?content=categories";
                var response = protocol.CallHttpRest("GET", url, bearerToken);

                if (String.IsNullOrWhiteSpace(response)) return;

                var rows = new List<object[]>();

                var categories = JArray.Parse(response);
                foreach (var category in categories)
                {
                    rows.Add(new object[]
                    {
                        category["id"]?.ToString(),                  // 201
                        category["name"]?.ToString(),                // 202
                        category["num_tokens"] ?? 0,                 // 203
                        category["price_change"] ?? 0,               // 204
                        category["market_cap"] ?? 0,                 // 205
                        category["volume"] ?? 0,                     // 206
                        category["last_updated"] ?? 0,               // 207
                        0                                            // 208 (button)
                    });
                }
                protocol.ClearTable(200);
                protocol.FillArray(200, rows.ToArray());
            }
        }
        catch (Exception ex)
        {
            protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
        }
    }
}
