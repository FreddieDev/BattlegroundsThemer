﻿Imports System.Text
Imports Fiddler

Public Class C_BattlegroundsProxy
    Inherits C_Proxy

    Overrides Sub FiddlerBeforeRequestHandler(ByVal tSession As Session)
        If tSession.url.Contains("bigassmessage.com/BAM-get") Then
            tSession.bBufferResponse = True
        End If
    End Sub

    Overrides Sub FiddlerBeforeResponseHandler(ByVal tSession As Session)
        If Not tSession.bBufferResponse Then Exit Sub

        Console.WriteLine("Blacklisted URL requested: " + tSession.url)
        tSession.utilDecodeResponse()
        tSession.utilReplaceInResponse("HELLO WORLD", "nopeee")
        Console.WriteLine("body code: " + Encoding.ASCII.GetString(tSession.ResponseBody))


        '' Tweak page for debugging
        'menuHTML = menuHTML.Replace("oncontextmenu=""return false;""", "") 'Enable context menu in webbrowsers (blocked in-game)
        'menuHTML = menuHTML.Replace("engine.hideOverlay();", "") 'Enable developer overlay in webbrowsers (blocked in-game)

        '' Apply user settings
        'If F_Main.skipIntroLogos Then menuHTML = C_WebMethods.addStylesheetToHTML(".intro{visibility:hidden !important}", menuHTML)

        '' Hide original page UI
        ''menuHTML = C_WebMethods.addStylesheetToHTML(".con-connected{visibility:hidden !important}", menuHTML)

    End Sub
End Class