Public Class F_Main
    ' Global variables
    Dim proxyServer As New C_BattlegroundsProxy

    Private Sub CB_Enable_CheckedChanged() Handles CB_Enable.CheckedChanged
        If CB_Enable.Checked Then
            proxyServer.StartServer()
        Else
            proxyServer.StopServer()
        End If
    End Sub


    ''' <summary>
    ''' In case any Fiddler assets are in use, or a proxy is running, force quit Fiddler
    ''' on app startup and close.
    ''' </summary>
    Private Sub F_Main_FormClosing() Handles MyBase.FormClosing
        proxyServer.StopServer()
    End Sub
End Class