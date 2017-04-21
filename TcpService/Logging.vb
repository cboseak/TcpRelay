Imports System.IO
Module Logging
    Public Sub Log(ByVal txt As String)
        IPeople.Tools.ReactiveExtensions.Logwriter.Log(System.IO.Path.Combine(_AssemblyDir, "ServiceLog.log"), txt)
    End Sub
End Module
