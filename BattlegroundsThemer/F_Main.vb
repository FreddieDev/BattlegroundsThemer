Public Class F_Main
    ' Global variables
    Dim proxyServer As New C_BattlegroundsProxy


    Private Sub F_Main_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        proxyServer.StopServer()
    End Sub

    Private Sub CB_Enable_CheckedChanged(sender As Object, e As EventArgs) Handles CB_Enable.CheckedChanged
        If CB_Enable.Checked Then
            proxyServer.StartServer()
        Else
            proxyServer.StopServer()
        End If
    End Sub
End Class