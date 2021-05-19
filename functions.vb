Imports System.Data
Imports System.Security.Cryptography
Imports System.Text
Imports MySql.Data.MySqlClient

Class dbAccess
    Public conn As New MySqlConnection
    Public readFile As String
    Public cmd As New MySqlCommand
    Public cmdReader As MySqlDataReader
    Public adapter As MySqlDataAdapter
    Public DataSet As New DataSet()
End Class

Module functions
    Dim dba As New dbAccess

    ' Hash the password using sha256
    Function HashString(ByVal StringToHash As String) As String

        'initialize sha256 algo
        Dim algo As SHA256 = SHA256.Create()
        'convert the string to hash to utf8
        Dim utf8Bytes = Encoding.UTF8.GetBytes(StringToHash)
        'convert the encoded string to sha256
        Dim hashedString = algo.ComputeHash(utf8Bytes)
        'return the hashed string on base64 string
        Return Convert.ToBase64String(hashedString)


    End Function

    ' Connect DB function
    Function ConnectDB() As MySqlConnection
        Try
            dba.readFile = My.Computer.FileSystem.ReadAllText(My.Computer.FileSystem.CurrentDirectory & "\connection.txt")
            With dba.conn
                If .State Then .Close()
                .ConnectionString = dba.readFile
                .Open()
            End With
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        Return dba.conn
    End Function

    Function QueryReader(conn As MySqlConnection, query As String) As MySqlDataReader
        Try
            With dba.cmd
                .Connection = conn
                .CommandText = query
                dba.cmdReader = .ExecuteReader
            End With
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
        Return dba.cmdReader
    End Function

    Function QueryReader(conn As MySqlConnection, query As String, a As Boolean) As MySqlDataAdapter
        Try
            With dba.cmd
                .Connection = conn
                .CommandText = query
                dba.cmdReader = .ExecuteReader
            End With
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try

        dba.adapter = New MySqlDataAdapter(dba.cmd)
        dba.cmdReader.Close()
        Return dba.adapter
    End Function

    Function GenerateID() As String

    End Function
    Function IfEmpty(toCheck As String) As Boolean
        Return String.IsNullOrWhiteSpace(toCheck)
    End Function
End Module
