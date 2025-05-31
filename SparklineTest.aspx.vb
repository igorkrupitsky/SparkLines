Imports System.Data.OleDb

Public Class SparklineTest
    Inherits System.Web.UI.Page

    Dim sConnectionString As String = "Provider=SQLOLEDB.1;Password=test;" &
    "User ID=test;Initial Catalog=nwind;Data Source=(local)"

    Private Sub Page_Load(ByVal sender As System.Object,
                          ByVal e As System.EventArgs) Handles MyBase.Load
        Response.Expires = 0
    End Sub

    Public Sub ShowSparklines()
        Response.Write("<table border=1 cellspacing=0>")

        Response.Write("<tr>")
        Response.Write("<th>Category</th>")
        Response.Write("<th>1998 Sales</th>")
        Response.Write("</tr>")

        Dim cn As New System.Data.OleDb.OleDbConnection(sConnectionString)
        cn.Open()

        Dim sSql As String = "SELECT CategoryID, CategoryName FROM Categories"
        Dim cm As New System.Data.OleDb.OleDbCommand(sSql, cn)
        Dim dr As OleDbDataReader = cm.ExecuteReader()
        While dr.Read
            Response.Write("<tr>")
            Response.Write("<td>" & dr.GetValue(dr.GetOrdinal("CategoryName")) & "</td>")

            Dim sCategoryID As String = dr.GetValue(dr.GetOrdinal("CategoryID")) & ""

            Response.Write("<td>" & GetSparkLine(cn, sCategoryID, 1998) & "</td>")
            Response.Write("</tr>")
        End While
        dr.Close()
        cn.Close()

        Response.Write("</table>")
    End Sub

    Private Function GetSparkLine(cn As System.Data.OleDb.OleDbConnection, ByVal sCategoryID As String, ByVal sYear As String) As String
        Dim sSql As String = "SELECT o.OrderDate, SUM(od.UnitPrice" &
          " * od.Quantity) AS Sales" &
          " FROM Orders o INNER JOIN " &
          " [order-details] od ON o.OrderID = od.OrderID INNER JOIN" &
          " Products p ON od.ProductID = p.ProductID" &
          " WHERE p.CategoryID = " & sCategoryID &
          " AND YEAR(o.OrderDate) = 1998" &
          " GROUP BY o.OrderDate" &
          " ORDER BY o.OrderDate"

        Dim cm As New System.Data.OleDb.OleDbCommand(sSql, cn)
        Dim dr As OleDbDataReader = cm.ExecuteReader()
        Dim sData As String
        While dr.Read
            If sData <> "" Then
                sData += ","
            End If
            sData += dr.GetValue(dr.GetOrdinal("Sales")) & ""
        End While
        dr.Close()

        Dim iWidth As Integer = 150
        Dim iHeight As Integer = 50

        Return "<img width=" & iWidth & " height=" & iHeight &
            " src='sparkline.aspx?width=" & iWidth &
                                "&height=" & iHeight &
                                "&data=" & sData & "'>"
    End Function



End Class