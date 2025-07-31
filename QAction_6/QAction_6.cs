//using System;
//using Skyline.DataMiner.Scripting;
//using Skyline.DataMiner.Utils.Protocol.Extension;

///// <summary>
///// Refresh category row QAction.
///// </summary>
//public static class RefreshCategoryRowQAction
//{
//    public static void Run(SLProtocolExt protocol)
//    {
//        try
//        {
//            string rowKey = protocol.RowKey();

//            // Read the category ID from the categoriesTable instance column
//            string categoryId = protocol.GetCell(Parameter.categoriesTable, rowKey, Parameter.categoriesTableInstance);

//            // Set that category ID into the UpdateRowId parameter so the trigger listening on it fires
//            protocol.SetParameter(Parameter.updaterowid, categoryId);

//            // Fire the trigger that reacts to UpdateRowId change (Trigger id="3" in your XML)
//            protocol.CheckTrigger(3);
//        }
//        catch (Exception ex)
//        {
//            protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}",
//                LogType.Error, LogLevel.NoLogging);
//        }
//    }
//}
