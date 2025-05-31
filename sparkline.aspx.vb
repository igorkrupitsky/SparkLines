Public Class sparkline
    Inherits System.Web.UI.Page

    Dim sBgColor As String = "ffffff"
    Dim sAvgLineColor As String = "gray"
    Dim sLineColor As String = "#000000"
    Dim sStdDevColor As String = "dcdcdc"

    Dim bStdDev As Boolean = True
    Dim sData As String = "1,2,3"

    Dim iImageWidth As Integer = 200
    Dim iImageHeight As Integer = 60

    Dim iTopMargin As Integer = 5
    Dim iBottomMargin As Integer = 5
    Dim iLeftMargin As Integer = 5
    Dim iRightMargin As Integer = 30

    Dim iMax As Double = 0
    Dim iMin As Double = 0
    Dim iAvg As Double = 0
    Dim iSum As Double = 0
    Dim iStdDev As Double = 0

    Private Sub Page_Load(ByVal sender As System.Object, _
                          ByVal e As System.EventArgs) _
                          Handles MyBase.Load

        SetVars()

        Dim oData() As String = sData.Split(",")
        If oData.Length <= 1 Then
            Exit Sub
        End If

        SetAvg()

        Dim oPoints(oData.Length - 1) As System.Drawing.Point
        Dim iScale As Double = (iImageHeight - (iTopMargin + iBottomMargin)) / _
                                Math.Abs(iMax - iMin)
        Dim iStepWidth As Double = (iImageWidth - (iLeftMargin + _
                                    iRightMargin)) / (oData.Length - 1)

        If Not Double.IsInfinity(iScale) Then
            For i As Integer = 0 To oData.Length - 1
                Dim sValue As String = oData(i)
                Dim iValue As Double = 0
                If sValue <> "" And IsNumeric(sValue) Then
                    iValue = CDbl(sValue)
                End If

                Dim x As Integer = (i * iStepWidth) + iLeftMargin
                Dim y As Integer = iImageHeight - (Math.Abs(iValue - iMin) * _
                                                   iScale) - iBottomMargin
                oPoints(i) = New System.Drawing.Point(x, y)
            Next
        End If

        Dim oBitmap As System.Drawing.Bitmap = New System.Drawing.Bitmap(iImageWidth, iImageHeight)
        Dim oPen As System.Drawing.Pen = New System.Drawing.Pen(GetColor(sLineColor))
        Dim oAvgPen As System.Drawing.Pen = New System.Drawing.Pen(GetColor(sAvgLineColor))
        Dim oGraphics As System.Drawing.Graphics = System.Drawing.Graphics.FromImage(oBitmap)
        oGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias
        oGraphics.FillRectangle(New System.Drawing.SolidBrush(GetColor(sBgColor)),
                                0, 0, iImageWidth, iImageHeight)

        Dim iMiddleY As Integer
        If Not Double.IsInfinity(iScale) Then

            iMiddleY = iImageHeight - (Math.Abs(iAvg - iMin) * iScale) - iBottomMargin

            'StdDev
            If bStdDev Then
                Dim oRect As New System.Drawing.Rectangle
                oRect.Width = iImageWidth - (iRightMargin + iLeftMargin)
                oRect.Height = iStdDev * iScale
                oRect.X = iLeftMargin
                oRect.Y = iMiddleY - (oRect.Height / 2)
                oGraphics.FillRectangle(New System.Drawing.SolidBrush(GetColor(sStdDevColor)), oRect)
            End If

            'Agv Line
            oGraphics.DrawLine(oAvgPen, iLeftMargin, iMiddleY, _
                               iImageWidth - iRightMargin, iMiddleY)

            'Lines
            oGraphics.DrawLines(oPen, oPoints)

            'Final Point
            Dim oLastPoint As System.Drawing.Point = oPoints(oPoints.Length - 1)
            Dim oBrush As New System.Drawing.SolidBrush(System.Drawing.Color.Red)
            oGraphics.FillPie(oBrush, oLastPoint.X - 2, oLastPoint.Y - 2, 4, 4, 0, 360)

            'Final Value
            Dim drawString As String = oData(oData.Length - 1)
            Dim drawFont As New System.Drawing.Font("Arial", 8)
            Dim drawBrush As New System.Drawing.SolidBrush(System.Drawing.Color.Black)
            oGraphics.DrawString(drawString, drawFont, drawBrush, _
                                 oLastPoint.X + 2, oLastPoint.Y - 6)
        Else
            iMiddleY = iImageHeight / 2
            oGraphics.DrawLine(oAvgPen, iLeftMargin, iMiddleY, _
                               iImageWidth - iRightMargin, iMiddleY)
        End If

        Response.ContentType = "image/jpeg"
        oBitmap.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg)
        oGraphics.Dispose()
        oBitmap.Dispose()

    End Sub

    Private Sub SetAvg()

        Dim oData() As String = sData.Split(",")

        For i As Integer = 0 To oData.Length - 1
            Dim sValue As String = oData(i)
            Dim iValue As Double = 0
            If sValue <> "" And IsNumeric(sValue) Then
                iValue = CDbl(sValue)
            End If

            iSum += iValue
            If i = 0 Then
                iMax = iValue
                iMin = iValue
            Else
                If iMax < iValue Then iMax = iValue
                If iMin > iValue Then iMin = iValue
            End If
        Next

        iAvg = iSum / oData.Length

        Dim iVar As Double

        If bStdDev Then

            For i As Integer = 0 To oData.Length - 1
                Dim sValue As String = oData(i)
                Dim iValue As Double = 0
                If sValue <> "" And IsNumeric(sValue) Then
                    iValue = CDbl(sValue)
                End If

                iVar += Math.Pow(iValue - iAvg, 2)
            Next

            iStdDev = Math.Sqrt(iVar / oData.Length)

        End If

    End Sub

    Private Sub SetVars()
        If Request.QueryString("data") <> "" Then
            sData = Request.QueryString("data")
        End If

        If Request.QueryString("StdDev") = "0" Then
            bStdDev = False
        End If

        If Request.QueryString("bgcolor") <> "" Then
            sBgColor = Request.QueryString("bgcolor")
        End If

        If Request.QueryString("avgcolor") <> "" Then
            sAvgLineColor = Request.QueryString("avgcolor")
        End If

        If Request.QueryString("linecolor") <> "" Then
            sLineColor = Request.QueryString("linecolor")
        End If

        If Request.QueryString("top") <> "" Then
            iTopMargin = Request.QueryString("top")
        End If

        If Request.QueryString("bottom") <> "" Then
            iBottomMargin = Request.QueryString("bottom")
        End If

        If Request.QueryString("left") <> "" Then
            iLeftMargin = Request.QueryString("left")
        End If

        If Request.QueryString("right") <> "" Then
            iRightMargin = Request.QueryString("right")
        End If

        If Request.QueryString("width") <> "" Then
            iImageWidth = Request.QueryString("width")
        End If

        If Request.QueryString("height") <> "" Then
            iImageHeight = Request.QueryString("height")
        End If

    End Sub

    Private Function GetColor(ByVal sColor As String) As System.Drawing.Color
        sColor = sColor.Replace("#", "")

        Dim oColor As System.Drawing.Color = System.Drawing.Color.FromName(sColor)
        Dim bColorEmpty As Boolean = oColor.R = 0 And oColor.G = 0 And oColor.B = 0
        If (bColorEmpty = False) Then Return oColor

        If sColor.Length <> 6 Then
            'On Error Return White
            Return System.Drawing.Color.White
        End If

        Dim sRed As String = sColor.Substring(0, 2)
        Dim sGreen As String = sColor.Substring(2, 2)
        Dim sBlue As String = sColor.Substring(4, 2)

        oColor = System.Drawing.Color.FromArgb(HexToInt(sRed), _
                 HexToInt(sGreen), HexToInt(sBlue))
        Return oColor
    End Function

    Function HexToInt(ByVal hexString As String) As Integer
        Return Integer.Parse(hexString, _
               System.Globalization.NumberStyles.HexNumber, Nothing)
    End Function

End Class