Imports Fiddler

Public Class C_Proxy
    Public Sub StartServer()
        ServerStarting()

        If Not FiddlerApplication.IsSystemProxy Then
            AddHandler FiddlerApplication.BeforeResponse, AddressOf FiddlerBeforeResponseHandler
            AddHandler FiddlerApplication.BeforeRequest, AddressOf FiddlerBeforeRequestHandler
        End If

        FiddlerApplication.Startup(80, True, False, False)
    End Sub

    Public Sub StopServer()
        ServerClosing()

        Try
            RemoveHandler FiddlerApplication.BeforeResponse, AddressOf FiddlerBeforeResponseHandler
            RemoveHandler FiddlerApplication.BeforeRequest, AddressOf FiddlerBeforeRequestHandler
            FiddlerApplication.Shutdown()
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

    ' Event handlers:
    Public Overridable Sub ServerStarting()
    End Sub
    Public Overridable Sub ServerClosing()
    End Sub
End Class