Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Threading

''' <summary>
''' I've made this class up of a few projects online, and tweaked it to my liking to fix bugs and performance.
''' 
''' Credit:
'''  - java2s.com/Tutorial/VB/0400__Socket-Network/TcpListenerbasedWebserver.htm
'''  - vb.net-informations.com/communications/vb.net_multithreaded_server_socket_programming.htm
''' </summary>
Public Class C_WebServer
    Private serverSocket As TcpListener
    Private clientSocket As Socket
    Private serverThread As Thread
    Private serverIP As IPAddress
    Private serverPort As Integer
    Protected rootPath As String

    Sub New(ByVal serverIP As IPAddress, ByVal serverPort As Integer, ByVal rootPath As String)
        ' Suggested IP&port: 127.0.0.1:80
        ' Suggested root: Directory.GetCurrentDirectory() & "\WWWRoot\"
        Me.serverIP = serverIP
        Me.serverPort = serverPort
        Me.rootPath = rootPath
    End Sub


    ''' <summary>
    ''' Start the localhost webserver
    ''' </summary>
    Public Sub Startup()
        If Not IsNothing(serverSocket) Then
            Console.WriteLine("Error: Can't start a new webserver until shutdown is called on the last instance.")
            Exit Sub
        End If

        Try
            ' Start listener
            serverSocket = New TcpListener(serverIP, serverPort)
            serverSocket.Start()

            ' Start request handler
            serverThread = New Thread(New ThreadStart(AddressOf ProcessThread))
            serverThread.Start()

            Console.WriteLine("Webserver started.")

        Catch ex As Exception
            MsgBox("An error occured when trying to start the webserver. Please check your firewall or run as admin. Error: " + vbNewLine +
                   ex.StackTrace.ToString(), MsgBoxStyle.Critical, "ERROR!")
            Application.Exit()
        End Try
    End Sub

    ''' <summary>
    ''' Stop the localhost webserver
    ''' </summary>
    Public Sub Shutdown()
        If IsNothing(serverSocket) Then
            Console.WriteLine("Shutdown aborted - the webserver was never started.")
        Else
            ShutdownClient(clientSocket)
            serverSocket.Stop()
            serverThread.Abort()
            serverSocket = Nothing
            Console.WriteLine("Webserver closed.")
        End If
    End Sub


    Private Sub ProcessThread()
        While (True)
            Try
                ' Exceptions may naturally occur here when requests are made after the server is closed
                clientSocket = serverSocket.AcceptSocket()

                ' Socket Information
                'Dim clientInfo As IPEndPoint = CType(clientSocket.RemoteEndPoint, IPEndPoint)

                ' Create new instance of HandleClient to handle the request
                Dim client As New HandleClient
                client.StartClient(clientSocket, rootPath)
            Catch
                ' Exceptions are caught here when waiting on AcceptSocket and the server/app is closed
                Console.WriteLine("Error processing request. If the server just closed, this error is not an issue.")
                ShutdownClient(clientSocket)
            End Try
        End While
    End Sub

    Public Class HandleClient
        Dim clientSocketB As Socket
        Dim rootPath As String

        Public Sub StartClient(ByVal inClientSocket As Socket, ByVal rootPath As String)
            Me.clientSocketB = inClientSocket
            Me.rootPath = rootPath
            Dim ctThread As Thread = New Thread(AddressOf ProcessRequestB)
            ctThread.Start()
        End Sub

        Private Sub ProcessRequestB()
            ' Buffer HTTP request
            Dim recvBytes(1024) As Byte
            Dim bytes = clientSocketB.Receive(recvBytes, 0, clientSocketB.Available, SocketFlags.None)
            Dim requestString = Encoding.ASCII.GetString(recvBytes, 0, bytes)

            If IsNothing(requestString) Then
                ShutdownClient(clientSocketB)
            ElseIf requestString.Length = 0 Then
                ShutdownClient(clientSocketB)
            End If

            ' Set default page
            Dim defaultPage As String = "index.html"

            Dim strArray() As String
            Dim strRequest As String

            strArray = requestString.Trim.Split(" ")

            ' Determine the HTTP method (GET only)
            If strArray(0).Trim().ToUpper.Equals("GET") Then
                strRequest = strArray(1).Trim

                If (strRequest.StartsWith("/")) Then
                    strRequest = strRequest.Substring(1)
                End If

                If (strRequest.EndsWith("/") Or strRequest.Equals("")) Then
                    strRequest = strRequest & defaultPage
                End If

                strRequest = rootPath & strRequest

                SendHTMLResponse(strRequest)

            Else ' Not HTTP GET method
                strRequest = rootPath & "Error\" & "400.html"

                '''''''''''sendHTMLResponse(strRequest)
            End If
        End Sub

        ' Send HTTP Response
        Private Sub SendHTMLResponse(ByVal httpRequest As String)
            ' Get the file content of the HTTP Request
            Dim respByte() As Byte = GetHTTPRequestFileBytes(httpRequest)

            ' Ignore the request if the file was not found
            If IsNothing(respByte) Then
                ShutdownClient(clientSocketB)
                Exit Sub
            End If

            ' Set HTML Header
            Dim htmlHeader As String =
                "HTTP/1.0 200 OK" & ControlChars.CrLf &
                "Server: WebServer 1.0" & ControlChars.CrLf &
                "Content-Length: " & respByte.Length & ControlChars.CrLf &
                "Content-Type: " & GetContentType(httpRequest) &
                ControlChars.CrLf & ControlChars.CrLf

            ' The content Length of HTML Header
            Dim headerByte() As Byte = Encoding.ASCII.GetBytes(htmlHeader)

            ' Send HTML Header back to Web Browser
            clientSocketB.Send(headerByte, 0, headerByte.Length, SocketFlags.None)

            ' Send HTML Content back to Web Browser
            clientSocketB.Send(respByte, 0, respByte.Length, SocketFlags.None)

            ShutdownClient(clientSocketB)
        End Sub

        ''' <summary>
        ''' Takes a file directory and returns the bytes in the file
        ''' </summary>
        ''' <param name="httpRequest">The HTTP file request</param>
        Private Function GetHTTPRequestFileBytes(ByVal httpRequest As String) As Byte()
            If Not File.Exists(httpRequest) Then
                Console.WriteLine("FILE NOT FOUND: " + httpRequest)
                Return Nothing
            End If

            Return File.ReadAllBytes(httpRequest)
        End Function

        ' Get Content Type
        ''' <summary>
        ''' Returns the type of content for a given HTTP request
        ''' </summary>
        ''' <param name="httpRequest">The HTTP file request</param>
        Private Function GetContentType(ByVal httpRequest As String) As String
            Select Case True
                Case httpRequest.EndsWith("html") Or httpRequest.EndsWith("htm")
                    Return "text/html"
                Case httpRequest.EndsWith("css")
                    Return "text/css"
                Case httpRequest.EndsWith("txt")
                    Return "text/plain"
                Case httpRequest.EndsWith("gif")
                    Return "image/gif"
                Case httpRequest.EndsWith("jpeg")
                    Return "image/jpeg"
                Case httpRequest.EndsWith("png")
                    Return "image/png"
                Case httpRequest.EndsWith("pdf")
                    Return "application/gif"
                Case httpRequest.EndsWith("doc")
                    Return "application/msword"
                Case httpRequest.EndsWith("xls")
                    Return "application/vnd.ms-excel"
                Case httpRequest.EndsWith("ppt")
                    Return "application/vnd.ms-powerpoint"
                Case Else
                    Return "text/plain"
            End Select
        End Function
    End Class

    ''' <summary>
    ''' Close the current connection with a client
    ''' </summary>
    Private Shared Sub ShutdownClient(ByVal socketToShutdown As Socket)
        If IsNothing(socketToShutdown) Then
            Exit Sub
        End If

        If socketToShutdown.Connected Then
            ' Close HTTP Socket connection
            socketToShutdown.Shutdown(SocketShutdown.Both)
            socketToShutdown.Close()
        End If
    End Sub
End Class