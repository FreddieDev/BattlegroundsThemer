Imports System.Net
Imports System.Runtime.InteropServices
Imports Microsoft.Win32
Imports Fiddler

Public Class C_Proxy
    <DllImport("wininet.dll")>
    Public Shared Function InternetSetOption(hInternet As IntPtr, dwOption As Integer, lpBuffer As IntPtr, dwBufferLength As Integer) As Boolean
    End Function
    Public Const INTERNET_OPTION_SETTINGS_CHANGED As Integer = 39
    Public Const INTERNET_OPTION_REFRESH As Integer = 37
    Private settingsReturn As Boolean, refreshReturn As Boolean


    ''' <summary>
    ''' Set the system proxy for websites
    ''' </summary>
    Private Sub setSystemProxy(ByVal proxyAddress As IPAddress, ByVal proxyPort As Integer)
        Dim registryProxy As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Internet Settings", True)
        registryProxy.SetValue("ProxyServer", proxyAddress.ToString + ":" + proxyPort.ToString)
        registryProxy.SetValue("ProxyEnable", 1)

        ' These lines implement the Interface in the beginning of program 
        ' They cause the OS to refresh the settings, causing IP to realy update
        settingsReturn = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0)
        refreshReturn = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0)
    End Sub

    Public Sub StartServer()
        If Not FiddlerApplication.IsSystemProxy Then
            AddHandler FiddlerApplication.BeforeResponse, AddressOf FiddlerBeforeResponseHandler
            AddHandler FiddlerApplication.BeforeRequest, AddressOf FiddlerBeforeRequestHandler
        End If

        FiddlerApplication.Startup(8090, True, False, False)

        ' setSystemProxy(IPAddress.Parse("127.0.0.1"), 8888)
    End Sub

    Public Sub StopServer()
        'Dim registryProxy As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Internet Settings", True)
        'registryProxy.SetValue("ProxyEnable", 0)
        Try
            FiddlerApplication.Shutdown()
            RemoveHandler FiddlerApplication.BeforeResponse, AddressOf FiddlerBeforeResponseHandler
            RemoveHandler FiddlerApplication.BeforeRequest, AddressOf FiddlerBeforeRequestHandler
            Threading.Thread.Sleep(500)
        Catch ex As Exception
        End Try
    End Sub

    ''' <summary>
    ''' Process the request before it is passed along to the next server in the request chain.
    ''' </summary>
    Public Overridable Sub FiddlerBeforeRequestHandler(ByVal tSession As Session)
    End Sub

    ''' <summary>
    ''' ' Process the response before passing along to the next client in the response chain (typically the browser)
    ''' </summary>
    Public Overridable Sub FiddlerBeforeResponseHandler(ByVal tSession As Session)
    End Sub
End Class