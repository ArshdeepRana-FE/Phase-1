using System;
using System.Data;
using System.Web.Script.Serialization;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DataAccessLayer da = new DataAccessLayer();
        string query = "SELECT * FROM users";
        var dataTable = da.ExecuteQuery(query);

        var rows = new System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, object>>();
        foreach (DataRow row in dataTable.Rows)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            foreach (DataColumn col in dataTable.Columns)
            {
                dict[col.ColumnName] = row[col];
            }
            rows.Add(dict);
        }
        var serializer = new JavaScriptSerializer();
        string json = serializer.Serialize(rows);

        string script = $"<script>console.log({json});</script>";
        ClientScript.RegisterStartupScript(this.GetType(), "consoleLogRows", script);
    }
}   