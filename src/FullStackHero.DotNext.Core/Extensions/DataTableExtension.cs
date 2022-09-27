namespace FullStackHero.DotNext.Core.Extensions;

public static class DataTableExtension
{
    public static string? DataTableToJson(this DataTable dataTable)
    {
        if (dataTable == null)
            throw new ArgumentNullException(nameof(dataTable), "DataTable không được null");

        if (dataTable.Rows.Count <= 0)
            return null;

        var jsonString = new StringBuilder();
        jsonString.Append("[");

        for (var i = 0; i < dataTable.Rows.Count; i++)
        {
            jsonString.Append("{");

            for (var j = 0; j < dataTable.Columns.Count; j++)
            {
                if (j < dataTable.Columns.Count - 1)
                {
                    jsonString.Append($"\"{dataTable.Columns[j].ColumnName}\": \"{dataTable.Rows[i][j]}\",");

                    continue;
                }

                if (j == dataTable.Columns.Count - 1) jsonString.Append($"\"{dataTable.Columns[j].ColumnName}\": \"{dataTable.Rows[i][j]}\"");
            }

            jsonString.Append(i == dataTable.Rows.Count - 1 ? "}" : "},");
        }

        jsonString.Append("]");

        return jsonString.ToString();
    }
}