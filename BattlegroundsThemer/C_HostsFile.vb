Imports System.IO

''' <summary>
''' Tools to read and write to the Windows hosts file
''' </summary>
Public Class C_HostsFile
    Private Shared hostsFileDir As String = Environment.SystemDirectory + "\drivers\etc\hosts"

    ''' <summary>
    ''' Checks if a site's address is currently being redirected by the Windows host file
    ''' </summary>
    Private Shared Function willSiteRedirect(ByVal siteAddress As String) As Boolean
        Dim hostsContent As String() = getHostsLines()

        For Each line As String In hostsContent
            If line.Contains(" " + siteAddress) Then Return True
        Next

        Return False
    End Function

    ''' <summary>
    ''' Adds the provided site address to the Windows hosts file to redirect to the localhost address instead
    ''' </summary>
    Public Shared Sub redirectSite(ByVal siteAddress As String, ByVal newAddress As String)
        ' Check the site isn't already redirecting
        If willSiteRedirect(siteAddress) Then Exit Sub

        'Add to hosts file to redirect requests to address to redirectIP
        Dim blockSiteCode As String = newAddress + " " + siteAddress
        setHostsText(String.Join(vbNewLine, getHostsLines()) + vbNewLine & blockSiteCode)
    End Sub

    ''' <summary>
    ''' Removes the provided site address from the Windows hosts file
    ''' </summary>
    Public Shared Sub unblockSiteRedirect(ByVal siteAddress As String)
        ' Check the site isn't already unblocked
        If Not willSiteRedirect(siteAddress) Then Exit Sub

        Dim hostsContent As String() = getHostsLines()
        Dim newHostsFileText As String = ""

        'Rebuild the hosts file without the redirect rule
        For i As Integer = 0 To hostsContent.Length - 1
            If Not hostsContent(i).Contains(" " + siteAddress) Then
                newHostsFileText += hostsContent(i)

                ' Add linebreak to the end of each line, except the last line
                If i < hostsContent.Length - 2 Then
                    newHostsFileText += vbNewLine
                End If
            End If
        Next

        setHostsText(newHostsFileText)
    End Sub


    ''' <summary>
    ''' Get all lines of text from the Windows hosts file
    ''' </summary>
    ''' <returns></returns>
    Private Shared Function getHostsLines() As String()
        Return File.ReadAllLines(hostsFileDir)
    End Function

    ''' <summary>
    ''' Set the hosts file text to the given parameter value
    ''' </summary>
    ''' <param name="newText">The new text for the hosts file to be set to.</param>
    Private Shared Sub setHostsText(ByVal newText As String)
        File.WriteAllText(hostsFileDir, newText)
    End Sub
End Class
