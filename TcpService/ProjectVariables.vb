Imports System.Reflection

Module ProjectVariables
    Friend Const _defaultNotifyDestination As String = "zz-interfaces@IPEOPLE.COM"
    Friend Const _companyName As String = "IPeople"
    Friend ReadOnly Property _AssemblyDir As String
        Get
            Return IPeople.Tools.ReactiveExtensions._AssemblyDir
        End Get
    End Property
    Friend _serviceName As String = "TcpService"
    Friend _assemblyName As String = Assembly.GetExecutingAssembly().FullName.Substring(0, Assembly.GetExecutingAssembly().FullName.IndexOf(", Version"))
    Friend _myPID As Integer = Process.GetCurrentProcess.Id

    Private _STATUS As String = "HALTED"
    Friend Property STATUS As String
        Get
            Return _STATUS
        End Get
        Set(value As String)
            If Not _STATUS = value Then
                Try
                    If Not System.IO.Directory.Exists(IO.Path.Combine(_AssemblyDir, "STATUS")) Then System.IO.Directory.CreateDirectory(IO.Path.Combine(_AssemblyDir, "STATUS"))
                Catch ex As Exception
                End Try
                Try
                    For Each FILE As System.IO.FileInfo In New System.IO.DirectoryInfo(IO.Path.Combine(_AssemblyDir, "STATUS")).GetFiles()
                        Try
                            FILE.Delete()
                        Catch ex As Exception
                        End Try
                    Next
                Catch ex As Exception
                End Try
                Try
                    Using wFile As System.IO.FileStream = New System.IO.FileStream(IO.Path.Combine(_AssemblyDir, "STATUS", value.ToUpper),
                                                                       System.IO.FileMode.Create)
                    End Using
                Catch ex As Exception
                End Try
            End If
            _STATUS = value
        End Set
    End Property
End Module
