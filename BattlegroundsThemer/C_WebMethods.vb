''' <summary>
''' Functions to modify and inject code into web documents
''' </summary>
Public Class C_WebMethods
    ''' <summary>
    ''' Appends a given script to the end of a standard HTML document
    ''' </summary>
    Public Shared Function addScriptToHTML(ByVal scriptString As String, ByVal htmlDocument As String) As String
        scriptString = "<script>" + vbNewLine + scriptString + vbNewLine + "</script>"
        Return htmlDocument.Replace("</head>", vbNewLine + scriptString + vbNewLine + "</head>")
    End Function


    ''' <summary>
    ''' Appends a collection of CSS rules to the HTML document head section
    ''' </summary>
    Public Shared Function addStylesheetToHTML(ByVal stylesheet As String, ByVal htmlDocument As String) As String
        stylesheet = "<style>" + vbNewLine + stylesheet + vbNewLine + "</style>"
        Return htmlDocument.Replace("</head>", vbNewLine + stylesheet + vbNewLine + "</head>")
    End Function


    ''' <summary>
    ''' Appends given HTML code to the end of a standard HTML document
    ''' </summary>
    Public Shared Function addHTMLToHTML(ByVal htmlString As String, ByVal htmlDocument As String) As String
        htmlString = vbNewLine + htmlString + vbNewLine
        Return htmlDocument.Replace("</body>", vbNewLine + htmlString + vbNewLine + "</body>")
    End Function
End Class
