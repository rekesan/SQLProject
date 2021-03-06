
Public Class LogInScreen

    Private Sub Cancel_Click(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub

    Private Sub Login_Click(sender As Object, e As RoutedEventArgs)
        Dim dba As New dbAccess
        Dim conn = ConnectDB()

        Dim _username = login_username.Text
        Dim _password = HashString(login_password.Password)

        Try
            With _username
                _username = .Remove(.IndexOf("'")).Trim
            End With
        Catch ex As Exception

        End Try


        Dim query = "select count(1) as `return` from log_in where username='" & _username & "' and password='" & _password & "';"

        Try
            dba.cmdReader = QueryReader(conn, query)
            dba.cmdReader.Read()

            If dba.cmdReader.GetInt32("return") = 0 Then
                MsgBox("Wrong username or password, try again.", MsgBoxStyle.Exclamation, "Error")
            Else
                Dim mainWin As New MainWindow
                mainWin.Show()
                Me.Close()
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        dba.cmdReader.Close()
        conn.Close()
    End Sub


    Private Sub MouseDrag(ByVal obj As Object, e As MouseButtonEventArgs)
        If e.LeftButton Then DragMove()
    End Sub

    Sub GotFocus_txt(sender As Object, e As RoutedEventArgs)
        Try
            Dim a As TextBox
            a = e.Source
            a.Text = ""
        Catch ex As Exception
            Dim a As PasswordBox
            a = e.Source
            a.Password = ""
        End Try

    End Sub
End Class