Public Class MainService
    <MTAThread()>
    <System.Diagnostics.DebuggerNonUserCode()>
    Public Shared Sub Main()
        Dim args As String() = Environment.GetCommandLineArgs()
        Try
            If args.Length > 1 Then
                Select Case args(1).ToLower
                    Case "/u", "/uninstall"
                        If IsServiceInstalled() = True Then
                            Console.WriteLine("Uninstalling " & _serviceName)
                            UninstallService()
                        End If
                    Case "/i", "/install"
                        Console.WriteLine("Installing " & _serviceName)
                        If IsServiceInstalled() = False Then
                            InstallService()
                            SetRecoveryOptions()
                        End If
                        If IsServiceInstalled() = True And IsServiceRunning() = False Then
                            Console.WriteLine("Starting " & _serviceName)
                            StartService()
                        End If
                    Case "/io", "/installonly"
                        Console.WriteLine("Installing " & _serviceName)
                        If IsServiceInstalled() = False Then
                            InstallService()
                            SetRecoveryOptions()
                        End If
                    Case "/c", "/console"
                        IsConsole = True
                        ConsoleHelper.Create()
                        Dim M As New MainService
                        M.OnStart(args)
                        While True
                            System.Threading.Thread.Sleep(250)
                        End While
                End Select
            ElseIf args.Length = 1 AndAlso args(0).ToUpper = System.Reflection.Assembly.GetAssembly(GetType(MainService)).Location.ToUpper Then

                Try
                    Using SMB As New ServiceMessageBox
                        SMB.ServiceStarted = IsServiceInstalled()
                        SMB.ShowDialog()
                        Select Case SMB.returnVar
                            Case 1
                                Shell(String.Format("{0} /install", System.Reflection.Assembly.GetAssembly(GetType(MainService)).Location))
                            Case 2
                                Shell(String.Format("{0} /uninstall", System.Reflection.Assembly.GetAssembly(GetType(MainService)).Location))
                            Case 3
                                IsConsole = True
                                ConsoleHelper.Create()
                                Dim M As New MainService
                                M.OnStart(args)
                                While True
                                    System.Threading.Thread.Sleep(250)
                                End While
                        End Select
                    End Using
                Catch
                    Dim ServicesToRun() As System.ServiceProcess.ServiceBase
                    ServicesToRun = New System.ServiceProcess.ServiceBase() {New MainService}
                    System.ServiceProcess.ServiceBase.Run(ServicesToRun)
                End Try
            Else
                Dim ServicesToRun() As System.ServiceProcess.ServiceBase
                ServicesToRun = New System.ServiceProcess.ServiceBase() {New MainService}
                System.ServiceProcess.ServiceBase.Run(ServicesToRun)
            End If
        Catch ex As Exception
            System.IO.File.WriteAllText(Replace(Mid(Environment.CommandLine, 1, InStrRev(Environment.CommandLine, "\") - 1) & "\CRASHDUMP.TXT", Chr(34), ""), ex.ToString)
        End Try
    End Sub
    Public Shared IsConsole As Boolean = False
    Protected Overrides Sub OnStart(ByVal args() As String)
        Console.ForegroundColor = ConsoleColor.Green
        STATUS = "STARTING"
        '------------------------------------------------------------------------
        AddHandler AppDomain.CurrentDomain.AssemblyResolve, AddressOf AssemblyResolve
        Try
            BeginUpdateChecker()
            SetRecoveryOptions()
            TCPMirror.Program.Main({"S"})
            'IPeople.Connect.Audit.AuditPrime.Initialize()
            'Dim t As New Threading.Thread(AddressOf CheckForUpdates)
            't.IsBackground = True
            't.Start()
            '------------------------------------------------------------------------
            Log(_serviceName & " Started Successfully.")
            STATUS = "RUNNING"
        Catch ex As Exception
            Log(_serviceName & " Failed to start.")
            Log(ex.ToString)
            STATUS = "HALTED"
            Process.GetCurrentProcess.Kill()
        End Try
    End Sub
    Public Shared Sub SetRecoveryOptions()
        Dim exitCode As Integer
        Using process = New Process()
            Dim startInfo = process.StartInfo
            startInfo.FileName = "sc"
            startInfo.WindowStyle = ProcessWindowStyle.Hidden
            startInfo.Arguments = String.Format("failure ""{0}"" reset= 0 actions= restart/5000", _serviceName)
            process.Start()
            process.WaitForExit()
            exitCode = process.ExitCode
            process.Close()
        End Using
    End Sub

    Public Shared Sub BeginUpdateChecker()
        If Not IsConsole Then
            Dim t As New Threading.Thread(AddressOf ProcessCheckForUpdates)
            t.IsBackground = True
            t.Start()
        End If
    End Sub
    Public Shared Sub ProcessCheckForUpdates()
        Dim LastUpdateCheck As DateTime = DateTime.Now
        While True
            Try
                If LastUpdateCheck.Add(TimeSpan.FromMinutes(1)) < DateTime.Now Then
                    Try
                        If IO.File.Exists(IO.Path.Combine(_AssemblyDir, "wyUpdate.exe")) Then
                            Dim ShouldCheckForUpdates As String = READINI(IO.Path.Combine(_AssemblyDir, "WUpdate.ini"), "WUpdate", "CheckForUpdates")
                            If String.IsNullOrEmpty(ShouldCheckForUpdates) Then
                                ShouldCheckForUpdates = "1"
                                WRITEINI(IO.Path.Combine(_AssemblyDir, "WUpdate.ini"), "WUpdate", "CheckForUpdates", ShouldCheckForUpdates)
                            End If
                            If ShouldCheckForUpdates = "1" Then
                                Log("Checking for updates...")
                                Using P As Process = Process.Start(IO.Path.Combine(_AssemblyDir, "wyUpdate.exe"), "/fromservice")
                                    P.WaitForExit()
                                End Using
                            End If
                        End If
                    Finally
                        LastUpdateCheck = DateTime.Now
                    End Try
                End If
            Catch ex As Exception
                Log(ex.Message)
            Finally
                System.Threading.Thread.Sleep(500)
            End Try
        End While
    End Sub

    'Private Shared Sub CheckForUpdates()
    '    While True
    '        Try
    '            IPeople.Tools.AutomaticUpdate.CheckForUpdates("TcpService")
    '        Catch ex As Exception
    '            Log(ex.ToString)
    '        Finally
    '            System.Threading.Thread.Sleep(60000)
    '        End Try
    '    End While
    'End Sub

    Protected Overrides Sub OnStop()
        Log("Halting " & _serviceName)
        STATUS = "HALTING"
        '------------------------------------------------------------------------


        '------------------------------------------------------------------------
        STATUS = "HALTED"
    End Sub

End Class
