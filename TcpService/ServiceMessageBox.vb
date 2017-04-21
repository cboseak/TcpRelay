Imports System.Runtime.InteropServices
Imports System.Security

Public Class ServiceMessageBox
    Public ServiceStarted As Boolean = False
    Public returnVar As Integer = -1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        returnVar = 1
        Me.Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        returnVar = 2
        Me.Close()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        returnVar = 3
        Me.Close()
    End Sub

    Private Sub ServiceMessageBox_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If ServiceStarted Then
            Me.Button1.Enabled = False
            Me.Button2.Enabled = True
            Me.Button3.Enabled = False
        Else
            Me.Button1.Enabled = True
            Me.Button2.Enabled = False
            Me.Button3.Enabled = True
        End If
    End Sub

    Private Sub Button1_EnabledChanged(sender As Object, e As EventArgs) Handles Button3.EnabledChanged, Button2.EnabledChanged, Button1.EnabledChanged
        If sender.Enabled Then
            sender.BackColor = Drawing.Color.GhostWhite
        Else
            sender.BackColor = Drawing.Color.Gray
        End If
    End Sub
End Class

Public Class ConsoleHelper
    Public Shared Function Create() As Integer
        If AllocConsole() Then
            Return 0
        Else
            Return Marshal.GetLastWin32Error()
        End If
    End Function

    Public Shared Function Destroy() As Integer
        If FreeConsole() Then
            Return 0
        Else
            Return Marshal.GetLastWin32Error()
        End If
    End Function

    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage"), SuppressUnmanagedCodeSecurity>
    <DllImport("kernel32.dll", SetLastError:=True)>
    Private Shared Function AllocConsole() As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function


    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage"), SuppressUnmanagedCodeSecurity>
    <DllImport("kernel32.dll", SetLastError:=True)>
    Private Shared Function FreeConsole() As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function
End Class
