Imports System.Net

Public Class F_Main
    ' Global variables
    'Developer:
    Public localhostAddress As IPAddress = IPAddress.Parse("127.0.0.1")
    Public WebserverPortNumber As Integer = 1025
    Public ImADevAndIWantAllTheMenuHTML As Boolean = True

    'Servers:
    Dim proxyServer As New C_BattlegroundsProxy

    'Settings:
    Public CurrentTheme As String = "Minimal"
    Public NoSplash As Boolean = True
    Public ShowScriptErrors As Boolean = True

    Private Sub CB_Enable_CheckedChanged() Handles CB_Enable.CheckedChanged
        If CB_Enable.Checked Then
            proxyServer.StartServer()
        Else
            proxyServer.StopServer()
        End If
        Process.Start("ipconfig", "/flushdns")
    End Sub


    ''' <summary>
    ''' In case any Fiddler assets are in use, or a proxy is running, force quit Fiddler
    ''' on app startup and close.
    ''' </summary>
    Private Sub F_Main_FormClosing() Handles MyBase.FormClosing
        proxyServer.StopServer()
    End Sub

    Private Sub CB_Theme_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CB_Theme.SelectedIndexChanged
        proxyServer.ChangeTheme("Minimal")
    End Sub
End Class