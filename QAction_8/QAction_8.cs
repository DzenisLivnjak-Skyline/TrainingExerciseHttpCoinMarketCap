using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using Skyline.DataMiner.Scripting;
using Skyline.DataMiner.Utils.Protocol.Extension;

/// <summary>
/// DataMiner QAction Class.
/// </summary>
public static class QAction
{
    /// <summary>
    /// The QAction entry point.
    /// </summary>
    /// <param name="protocol">Link with SLProtocol process.</param>
    public static void Run(SLProtocolExt protocol)
    {
        try
        {
            string jsonResponse = Convert.ToString(protocol.GetParameter(219));
            var singleCategory = Newtonsoft.Json.JsonConvert.DeserializeObject<SingleCategoryResponse>(jsonResponse);
            var data = singleCategory.Data;

#pragma warning disable SA1118 // Parameter should not span multiple lines
            _ = protocol.categoriestable.SetRow(data.Id, new object[]
            {
                 data.Id,
                 data.Name,
                 data.NumTokens,
                 data.AvgPriceChange,
                 data.MarketCap,
                 data.Volume,
                 data.LastUpdated.ToOADate(),
                 0,
            },
            true);
        }
        catch (Exception ex)
        {
            protocol.Log("Error parsing and updating category row: " + ex, LogType.Error, LogLevel.NoLogging);
        }
    }

}
