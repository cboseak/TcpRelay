Imports System.Runtime.InteropServices

Public Module Structures
    Public Structure DataStruct
        Public DATA As Byte()
        Public LEN As Integer
    End Structure

    <StructLayout(LayoutKind.Sequential)> _
    Structure SECURITY_ATTRIBUTES
        Public nLength As Integer
        Public lpSecurityDescriptor As IntPtr
        Public bInheritHandle As Integer
    End Structure

    Structure PROCESS_INFORMATION
        Public hProcess As IntPtr
        Public hThread As IntPtr
        Public dwProcessId As Integer
        Public dwThreadId As Integer
    End Structure

    Public Const SW_SHOW As Short = 5
    Public Const TOKEN_QUERY As UInteger = &H8
    Public Const TOKEN_DUPLICATE As UInteger = &H2
    Public Const TOKEN_ASSIGN_PRIMARY As UInteger = &H1
    Public Const GENERIC_ALL_ACCESS As Integer = &H10000000
    Public Const STARTF_USESHOWWINDOW As Integer = &H1
    Public Const STARTF_FORCEONFEEDBACK As Integer = &H40
    Public Const CREATE_UNICODE_ENVIRONMENT As UInteger = &H400

    Friend Enum TOKEN_TYPE
        TokenPrimary = 1
        TokenImpersonation
    End Enum

    Enum SECURITY_IMPERSONATION_LEVEL
        SecurityAnonymous
        SecurityIdentification
        SecurityImpersonation
        SecurityDelegation
    End Enum

    <Flags()> Public Enum ACCESS_MASK : Uint32
        DELETE = &H10000
        READ_CONTROL = &H20000
        WRITE_DAC = &H40000
        WRITE_OWNER = &H80000
        SYNCHRONIZE = &H100000

        STANDARD_RIGHTS_REQUIRED = &HF0000

        STANDARD_RIGHTS_READ = &H20000
        STANDARD_RIGHTS_WRITE = &H20000
        STANDARD_RIGHTS_EXECUTE = &H20000

        STANDARD_RIGHTS_ALL = &H1F0000

        SPECIFIC_RIGHTS_ALL = &HFFFF

        ACCESS_SYSTEM_SECURITY = &H1000000

        MAXIMUM_ALLOWED = &H2000000

        GENERIC_READ = &H80000000
        GENERIC_WRITE = &H40000000
        GENERIC_EXECUTE = &H20000000
        GENERIC_ALL = &H10000000

        DESKTOP_READOBJECTS = &H1
        DESKTOP_CREATEWINDOW = &H2
        DESKTOP_CREATEMENU = &H4
        DESKTOP_HOOKCONTROL = &H8
        DESKTOP_JOURNALRECORD = &H10
        DESKTOP_JOURNALPLAYBACK = &H20
        DESKTOP_ENUMERATE = &H40
        DESKTOP_WRITEOBJECTS = &H80
        DESKTOP_SWITCHDESKTOP = &H100

        WINSTA_ENUMDESKTOPS = &H1
        WINSTA_READATTRIBUTES = &H2
        WINSTA_ACCESSCLIPBOARD = &H4
        WINSTA_CREATEDESKTOP = &H8
        WINSTA_WRITEATTRIBUTES = &H10
        WINSTA_ACCESSGLOBALATOMS = &H20
        WINSTA_EXITWINDOWS = &H40
        WINSTA_ENUMERATE = &H100
        WINSTA_READSCREEN = &H200

        WINSTA_ALL_ACCESS = &H37F
    End Enum

    Enum TOKEN_INFORMATION_CLASS
        TokenUser = 1
        TokenGroups
        TokenPrivileges
        TokenOwner
        TokenPrimaryGroup
        TokenDefaultDacl
        TokenSource
        TokenType
        TokenImpersonationLevel
        TokenStatistics
        TokenRestrictedSids
        TokenSessionId
        TokenGroupsAndPrivileges
        TokenSessionReference
        TokenSandBoxInert
        TokenAuditPolicy
        TokenOrigin
        TokenElevationType
        TokenLinkedToken
        TokenElevation
        TokenHasRestrictions
        TokenAccessInformation
        TokenVirtualizationAllowed
        TokenVirtualizationEnabled
        TokenIntegrityLevel
        TokenUIAccess
        TokenMandatoryPolicy
        TokenLogonSid
        MaxTokenInfoClass
    End Enum

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)> _
    Structure STARTUPINFO
        Public cb As Integer
        Public lpReserved As String
        Public lpDesktop As String
        Public lpTitle As String
        Public dwX As Integer
        Public dwY As Integer
        Public dwXSize As Integer
        Public dwYSize As Integer
        Public dwXCountChars As Integer
        Public dwYCountChars As Integer
        Public dwFillAttribute As Integer
        Public dwFlags As Integer
        Public wShowWindow As Short
        Public cbReserved2 As Short
        Public lpReserved2 As Integer
        Public hStdInput As Integer
        Public hStdOutput As Integer
        Public hStdError As Integer
    End Structure

    Public Enum State
        SERVICE_STOPPED = &H1
        SERVICE_START_PENDING = &H2
        SERVICE_STOP_PENDING = &H3
        SERVICE_RUNNING = &H4
        SERVICE_CONTINUE_PENDING = &H5
        SERVICE_PAUSE_PENDING = &H6
        SERVICE_PAUSED = &H7
    End Enum
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure SERVICE_STATUS
        Public serviceType As Integer
        Public currentState As Integer
        Public controlsAccepted As Integer
        Public win32ExitCode As Integer
        Public serviceSpecificExitCode As Integer
        Public checkPoint As Integer
        Public waitHint As Integer
    End Structure
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure MIB_TCPROW_OWNER_PID
        Public state As UInteger
        Public localAddr As UInteger
        Public localPort1 As Byte
        Public localPort2 As Byte
        Public localPort3 As Byte
        Public localPort4 As Byte
        Public remoteAddr As UInteger
        Public remotePort1 As Byte
        Public remotePort2 As Byte
        Public remotePort3 As Byte
        Public remotePort4 As Byte
        Public owningPid As Integer
    End Structure
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure MIB_TCPTABLE_OWNER_PID
        Public dwNumEntries As UInteger
        Private table As MIB_TCPROW_OWNER_PID
    End Structure

    Enum TCP_TABLE_CLASS
        TCP_TABLE_BASIC_LISTENER
        TCP_TABLE_BASIC_CONNECTIONS
        TCP_TABLE_BASIC_ALL
        TCP_TABLE_OWNER_PID_LISTENER
        TCP_TABLE_OWNER_PID_CONNECTIONS
        TCP_TABLE_OWNER_PID_ALL
        TCP_TABLE_OWNER_MODULE_LISTENER
        TCP_TABLE_OWNER_MODULE_CONNECTIONS
        TCP_TABLE_OWNER_MODULE_ALL
    End Enum
    Public Enum SOCKETSTATUS
        CLOSED = 0
        CLOSING = 1
        OPEN = 2
        SENDING = 3
        LISTENING = 4
        CONNECTING = 5
        SOCKETERROR = 6
        READY = 7
    End Enum
End Module
