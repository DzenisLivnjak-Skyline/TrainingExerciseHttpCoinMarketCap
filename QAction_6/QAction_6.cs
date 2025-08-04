using System;
using Skyline.DataMiner.Scripting;
using Skyline.DataMiner.Utils.Protocol.Extension;

/// <summary>
/// QAction 6 – Refresh a single “category” row in Categories table.
/// Trigger: write column pid=208 (categoriesUserAction), row-aware QAction.
/// Sets UpdateRowId (pid=302) and fires the refresh trigger (id=3).
/// </summary>
public static class QAction
{
    public static void Run(SLProtocolExt protocol)
    {
        try
        {
            string rowKey = protocol.RowKey();
            if (string.IsNullOrEmpty(rowKey))
                return;

            string categoryId = Convert.ToString(protocol.GetCell(
                Parameter.Categoriestable.tablePid,
                rowKey,
                Parameter.Categoriestable.Idx.categoriestableinstance));
            if (string.IsNullOrEmpty(categoryId))
                return;

            protocol.SetParameter(Parameter.updaterowid, categoryId);

            protocol.CheckTrigger(3);
        }
        catch (Exception ex)
        {
            protocol.Log(
                $"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception: {ex}",
                LogType.Error,
                LogLevel.NoLogging
            );
        }
    }
}
