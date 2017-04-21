Module Win32
    Private Declare Function GetPrivateProfileString Lib "kernel32" _
 Alias "GetPrivateProfileStringA" _
                     (ByVal lpApplicationName As String, _
                     ByVal lpKeyName As String, _
                     ByVal lpDefault As String, _
                     ByVal lpReturnedString As String, _
                     ByVal nSize As Integer, _
                     ByVal lpFileName As String) As Long

    Private Declare Unicode Function WritePrivateProfileString Lib "kernel32" _
 Alias "WritePrivateProfileStringW" (ByVal lpApplicationName As String, _
 ByVal lpKeyName As String, ByVal lpString As String, _
 ByVal lpFileName As String) As Int32
    Public Sub WRITEINI(ByVal PATH As String, ByVal Section As String, ByVal ParamName As String, ByVal ParamVal As String)
        Dim Result As Integer
        Result = WritePrivateProfileString(Section, ParamName, ParamVal, PATH)
    End Sub
    Public Function READINI(ByVal PATH As String, ByVal SECTION As String, ByVal PARAMNAME As String) As String
        Dim iniResults
        Dim TEMPVAL As String = Space$(1024)
        iniResults = GetPrivateProfileString(SECTION, PARAMNAME, "", TEMPVAL, 1024, PATH)
        Dim RETURNVAL As String = Trim(TEMPVAL.Replace(vbNullChar, ""))
        Return RETURNVAL
    End Function
End Module
