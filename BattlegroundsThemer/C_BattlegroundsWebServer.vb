Imports System.IO

''' <summary>
''' This class allows for custom root directories and modifies file content before it is returned to the client
''' </summary>
Public Class C_BattlegroundsWebServer
    Inherits C_WebServer

    Sub New()
        MyBase.New(F_Main.localhostAddress, F_Main.WebserverPortNumber, Environment.CurrentDirectory + "\Themes\" + F_Main.CurrentTheme + "\")
    End Sub

    ''' <summary>
    ''' Update the root directory of the webserver, useful for when changing themes
    ''' </summary>
    Public Sub ChangeRootDirectory(ByVal newDirectory As String)
        MyBase.rootPath = newDirectory
    End Sub
End Class