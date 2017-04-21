Imports System.ServiceProcess
Imports System.Runtime.InteropServices
Imports System.Configuration.Install

Public Module ServiceComponents
    Private _serviceStatus As SERVICE_STATUS

    Private Declare Auto Function SetServiceStatus Lib "ADVAPI32.DLL" ( _
        ByVal hServiceStatus As IntPtr, _
        ByRef lpServiceStatus As SERVICE_STATUS _
    ) As Boolean
    Public locker1 As Object = New Object()
    Public Function IsServiceInstalled() As Boolean
        Using ServiceController As ServiceController = New ServiceController(_serviceName)
            Try
                Dim status As ServiceControllerStatus = ServiceController.Status
            Catch ex As Exception
                Return False
            End Try
            Return True
        End Using
    End Function
    Public Function IsServiceRunning() As Boolean
        Using serviceController As New ServiceController(_serviceName)
            If Not IsServiceInstalled() Then
                Return False
            End If
            Return (serviceController.Status = ServiceControllerStatus.Running)
        End Using
    End Function

    Public Sub InstallService()
        If IsServiceInstalled() Then
            Return
        End If
        Try
            Dim commandLine As String() = New String(0) {}
            commandLine(0) = "Test install"
            Dim mySavedState As IDictionary = New Hashtable()
            Dim installer As AssemblyInstaller = GetAssemblyInstaller(commandLine)
            Try
                installer.Install(mySavedState)
                installer.Commit(mySavedState)
            Catch ex As Exception
                installer.Rollback(mySavedState)
            End Try
        Catch
        End Try
    End Sub

    Public Function GetAssemblyInstaller(commandLine As String()) As AssemblyInstaller
        Dim installer As New AssemblyInstaller()
        installer.Path = Environment.GetCommandLineArgs()(0)
        installer.CommandLine = commandLine
        installer.UseNewContext = True
        Return installer
    End Function

    Public Sub UninstallService()
        If Not IsServiceInstalled() Then
            Return
        End If
        Dim commandLine As String() = New String(0) {}
        commandLine(0) = "Test Uninstall"
        Dim mySavedState As IDictionary = New Hashtable()
        mySavedState.Clear()
        Dim installer As AssemblyInstaller = GetAssemblyInstaller(commandLine)
        Try
            installer.Uninstall(mySavedState)
        Catch
        End Try
    End Sub

    Public Sub StartService()
        If Not IsServiceInstalled() Then
            Return
        End If
        Using serviceController As New ServiceController(_serviceName)
            If serviceController.Status = ServiceControllerStatus.Stopped Then
                Try
                    serviceController.Start()
                    WaitForStatusChange(serviceController, ServiceControllerStatus.Running)
                Catch
                End Try
            End If
        End Using
    End Sub

    Public Sub StopService()
        If Not IsServiceInstalled() Then
            Return
        End If
        Using serviceController As New ServiceController(_serviceName)
            If serviceController.Status <> ServiceControllerStatus.Running Then
                Return
            End If
            serviceController.[Stop]()
            WaitForStatusChange(serviceController, ServiceControllerStatus.Stopped)
        End Using
    End Sub

    Public Sub WaitForStatusChange(serviceController As ServiceController, newStatus As ServiceControllerStatus)
        Dim count As Integer = 0
        While serviceController.Status <> newStatus AndAlso count < 30
            System.Threading.Thread.Sleep(1000)
            serviceController.Refresh()
            count += 1
        End While
        If serviceController.Status <> newStatus Then
            Throw New Exception("Failed to change status of service. New status: " & newStatus)
        End If
    End Sub
End Module
