Imports System.Data
Imports System.Security.Cryptography
Imports System.Text
Imports MySql.Data.MySqlClient
Imports Org.BouncyCastle.Crypto

Public Class LogInScreen
    Dim conn As New MySqlConnection
    Dim readFile As String
    ReadOnly cmd As New MySqlCommand
    Dim cmdReader As MySqlDataReader

    Dim userFocused = False
    Dim passFocused = False

    Private Sub Cancel_Click(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub

    Private Sub Login_Click(sender As Object, e As RoutedEventArgs)
        ConnectDB()

        Dim _username = login_username.Text
        Dim _password = HashString(login_password.Password)

        Dim query = "select count(1) as `return` from log_in where username='" & _username & "' and password='" & _password & "';"

        Try
            With cmd
                .Connection = conn
                .CommandText = query
                cmdReader = .ExecuteReader
            End With
            cmdReader.Read()

            If cmdReader.GetInt32("return") = 0 Then
                MsgBox("wrong username or password, try again.")
            Else
                Dim mainwindow As New MainWindow
                mainwindow.Show()
                Me.Close()
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        conn.Close()
    End Sub

    ' Connect DB function
    Function ConnectDB() As Boolean
        Try
            readFile = My.Computer.FileSystem.ReadAllText(My.Computer.FileSystem.CurrentDirectory & "\connection.txt")
            With conn
                If .State = ConnectionState.Open Then .Close()
                .ConnectionString = readFile
                .Open()
            End With
            Return True
        Catch ex As Exception
            MsgBox(ex.Message)
            Return False
        End Try
    End Function


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

    Private Sub MouseDrag(ByVal obj As Object, e As MouseButtonEventArgs)
        If e.LeftButton.Pressed Then DragMove()
    End Sub

    Private Sub login_username_GotFocus(sender As Object, e As RoutedEventArgs)
        If Not userFocused Then login_username.Text = ""
        userFocused = True
    End Sub

    Private Sub login_password_GotFocus(sender As Object, e As RoutedEventArgs)
        If Not passFocused Then login_password.Password = ""
        passFocused = True
    End Sub
End Class