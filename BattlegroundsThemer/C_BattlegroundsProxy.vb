Imports System.IO
Imports System.Text
Imports Fiddler

Public Class C_BattlegroundsProxy
    Inherits C_Proxy
    Private battlegroundsURL As String = "front.battlegroundsgame.com"
    Private fileHoster As C_BattlegroundsWebServer

    Public Sub ChangeTheme(ByVal themeName As String)
        fileHoster.ChangeRootDirectory(Environment.CurrentDirectory + "\Themes\" + themeName + "\")
    End Sub

    Public Overrides Sub ServerStarting()
        C_HostsFile.redirectSite(battlegroundsURL, F_Main.localhostAddress.ToString)

        ' Start webserver to host theme files
        fileHoster = New C_BattlegroundsWebServer
        fileHoster.Startup()
    End Sub

    Public Overrides Sub ServerClosing()
        C_HostsFile.unblockSiteRedirect("front.battlegroundsgame.com")
        fileHoster.Shutdown()
    End Sub

    ''' <summary>
    ''' Check HTTP requests to look for requests to the battlegrounds page
    ''' </summary>
    Overrides Sub FiddlerBeforeRequestHandler(ByVal tSession As Session)
        If tSession.url.Contains(battlegroundsURL + "/index.html") Then
            tSession.bBufferResponse = True

            ' Unblock the battlegrounds page so the response can be retrieved
            C_HostsFile.unblockSiteRedirect(battlegroundsURL)
        End If
    End Sub

    ''' <summary>
    ''' Returns a new edited battlegrounds page to the client
    ''' </summary>
    Overrides Sub FiddlerBeforeResponseHandler(ByVal tSession As Session)
        If Not tSession.bBufferResponse Then Exit Sub

        Console.WriteLine("Whitelisted URL response from " + tSession.fullUrl + "!")

        ' Decode the response
        tSession.utilDecodeResponse()

        ' Calculate the redirect URL to locate and download the main menu page (unblocking the page first)
        C_HostsFile.unblockSiteRedirect(battlegroundsURL)
        Dim menuHTML As String = C_WebMethods.fetchSiteHTML(GetBattlegroundsRedirectURL(Encoding.ASCII.GetString(tSession.ResponseBody)))

        If IsNothing(menuHTML) Then
            MsgBox("The official menu could not be streamed. Please check your internet connection, run as admin, or retry.")
            Application.Exit()
        End If

        ' Inject theme into webpage and convert back to transmittable bytes
        tSession.utilSetResponseBody(InstallThemeIntoPage(menuHTML))

        ' Re-block the battlegrounds page to catch future requests & responses
        C_HostsFile.redirectSite(battlegroundsURL, F_Main.localhostAddress.ToString)
    End Sub

    ''' <summary>
    ''' Locates the redirect URL and token inside the script at front.battlegroundsgame.com
    ''' </summary>
    Private Function GetBattlegroundsRedirectURL(ByVal redirectScript As String) As String
        ' This is what a redirect script from front.battlegroundsgame.com normally looks like:
        '<script>
        '    var url = window.location.search;
        '    url = url.replace("?", '');
        '    Location.href ='https://prod-live-front.playbattlegrounds.com/2017.09.14-2017.09.13-351/index-steam.2017.09.14-2017.09.13-351.html?'+url;
        '</script>

        ' Loop through the script to find the line that contains the next URL and extract the address
        For Each line As String In redirectScript.ToLower.Split(Environment.NewLine)
            If line.Contains("location.href='") Then
                Dim battlegroundsRedirectURL As String = line.Trim()

                ' Remove spaces and semi colons from code
                battlegroundsRedirectURL = battlegroundsRedirectURL.Replace(" ", "").Replace(";", "")

                ' Remove string beginning and end surrounding URL
                battlegroundsRedirectURL = battlegroundsRedirectURL.Replace("location.href='", "")
                battlegroundsRedirectURL = battlegroundsRedirectURL.Replace("'+url", "")

                Return battlegroundsRedirectURL
            End If
        Next

        Return Nothing
    End Function


    Private Function InstallThemeIntoPage(ByVal pageHTML As String) As String
        Static resourcesDirectory As String = Environment.CurrentDirectory + "\Resources\"
        Dim themeDirectory As String = Environment.CurrentDirectory + "\Themes\" + F_Main.CurrentTheme + "\"

        ' Tweak page for debugging
        pageHTML = pageHTML.Replace("oncontextmenu=""return false;""", "") 'Enable context menu in webbrowsers (blocked in-game)
        pageHTML = pageHTML.Replace("engine.hideOverlay();", "") 'Enable developer overlay in webbrowsers (blocked in-game)

        ' Check if any required files are missing
        Dim themeHTMLDir As String = themeDirectory + "index.html"
        Dim themeStylesheetDir As String = themeDirectory + "stylesheet.css"
        Select Case True
            Case Not File.Exists(themeHTMLDir)
                MsgBox("The theme you have selected does not contain the index.html file!", MsgBoxStyle.Critical, "ERROR!")
                Application.Exit()
            Case Not File.Exists(themeStylesheetDir)
                MsgBox("The theme you have selected does not contain a stylesheet.css file!", MsgBoxStyle.Critical, "ERROR!")
                Application.Exit()
        End Select

        ' Load theme code into vars
        Dim themeHTMLCode As String = File.ReadAllText(themeHTMLDir)
        Dim stylesheet As String = File.ReadAllText(themeStylesheetDir)

        ' Prepare theme HTML&CSS for injection via javascript string
        themeHTMLCode = themeHTMLCode.Replace("""", "\""") ' Escape all double quotes in the HTML for JS compatibility
        themeHTMLCode = themeHTMLCode.Replace(vbCr, "").Replace(vbLf, "") ' Remove all newLine characters
        stylesheet = stylesheet.Replace("""", "\""") ' Escape all double quotes in the HTML for JS compatibility
        stylesheet = stylesheet.Replace(vbCr, "").Replace(vbLf, "") ' Remove all newLine characters

        ' Append processed theme HTML&CSS into themeInjector variable
        Dim themeInjectorScript As String = File.ReadAllText(resourcesDirectory + "themeInjector.js")
        themeInjectorScript = themeInjectorScript.Replace("BT_HTMLHERE", themeHTMLCode)
        themeInjectorScript = themeInjectorScript.Replace("BT_CSSHERE", stylesheet)

        ' Append themeInjector into page end
        pageHTML = C_WebMethods.addScriptToHTML(themeInjectorScript, pageHTML)

        ' Inject theme scripts.js into document head
        Dim themeScriptsDir As String = themeDirectory + "scripts.js"
        If File.Exists(themeScriptsDir) Then
            pageHTML = C_WebMethods.addScriptToHTML(File.ReadAllText(themeScriptsDir), pageHTML)
        End If



        ' Apply user settings
        If F_Main.ImADevAndIWantAllTheMenuHTML Then pageHTML = C_WebMethods.addScriptToHTML(File.ReadAllText(resourcesDirectory + "fetchCode.js"), pageHTML)
        If F_Main.ShowScriptErrors Then pageHTML = C_WebMethods.addScriptToHTML(File.ReadAllText(resourcesDirectory + "highlightScriptErrors.js"), pageHTML)
        If F_Main.NoSplash Then pageHTML = C_WebMethods.addStylesheetToHTML(".intro{visibility:hidden !important}", pageHTML)
        If F_Main.HideRulesAndStore Then pageHTML = C_WebMethods.addStylesheetToHTML(".sideBanner {display: none !important}", pageHTML)
        If F_Main.IAmSoRich Then pageHTML = pageHTML.Replace("BT_IAMSORICH", "lots of money")
        If Not F_Main.RB_UsernameShow.Checked Then
            If F_Main.RB_UsernameHide.Checked Then
                pageHTML = pageHTML.Replace("BT_USERNAMEHERE", "")
            Else
                pageHTML = pageHTML.Replace("BT_USERNAMEHERE", F_Main.TB_SpoofedUsername.Text)
            End If
        End If


        ' Fix all references to local files in code
        pageHTML = pageHTML.Replace("battlegroundsThemer_ROOT", "http://" + F_Main.localhostAddress.ToString + ":" + F_Main.WebserverPortNumber.ToString)

        'Write completed HTML to desktop for debugging
        File.WriteAllText("C:\Users\fredd\Desktop\compiledDocument.html", pageHTML)

        Return pageHTML
    End Function
End Class
