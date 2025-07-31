//using System;
//using Skyline.DataMiner.Scripting;

///// <summary>
///// DataMiner QAction Class: Adds Bearer prefix to token.
///// </summary>
//public static class QAction
//{
//    /// <summary>
//    /// The QAction entry point.
//    /// </summary>
//    /// <param name="protocol">Link with SLProtocol process.</param>
//    public static void Run(SLProtocol protocol)
//    {
//        try
//        {
//            string token = Convert.ToString(protocol.GetParameter(Parameter.Write.bearertoken_55));

//            if (!string.IsNullOrWhiteSpace(token))
//            {
//                string bearer = $"Bearer {token.Trim()}";
//                protocol.SetParameter(Parameter.bearertoken, bearer);
//            }
//        }
//        catch (Exception ex)
//        {
//            protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
//        }
//    }
//}