Imports System.Reflection

Module Utilities
    Public Function AssemblyResolve(ByVal sender As Object, ByVal e As ResolveEventArgs) As Assembly
        If e.Name.StartsWith("IPeople.Tools.Notify.Client,", StringComparison.OrdinalIgnoreCase) Then
            Using stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(_assemblyName & ".IPeople.Tools.Notify.Client.dll")
                Dim assemblyData(CInt(stream.Length)) As Byte
                stream.Read(assemblyData, 0, assemblyData.Length)
                Return Assembly.Load(assemblyData)
            End Using
        Else
            Return Nothing
        End If
    End Function
End Module
