//using System;
//using Skyline.DataMiner.Scripting;

//public static class QAction6
//{
//    public static void Run(SLProtocol protocol)
//    {
//        try
//        {
//            object[] row = protocol.categoriestable.GetRow(protocol.RowKey());
//            string categoryId = (string)row[Parameter.Categoriestable.Idx.categoriestableinstance_201];

//            protocol.SetParameter(Parameter.onecategoryurl_8, GenerateUrl(categoryId));

//            protocol.CheckTrigger();

//        }
//        catch (Exception ex)
//        {
//            protocol.Log($"QA{protocol.QActionID}|Run exception:{ex}", LogType.Error, LogLevel.NoLogging);
//        }
//    }

//    private static string GenerateUrl(string id)
//    {
//        if (string.IsNullOrWhiteSpace(id))
//            throw new ArgumentException(nameof(id));
//        return "/api/custom/coinmarketcap?content=category&id=" + id;
//    }
//}
