
Public Class LogInScreen

    Dim userFocused = False
    Dim passFocused = False

    Private Sub Cancel_Click(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub

    Private Sub Login_Click(sender As Object, e As RoutedEventArgs)
        Dim dba As New dbAccess
        Dim conn = ConnectDB()

        Dim _username = login_username.Text
        Dim _password = HashString(login_password.Password)

        Dim query = "select count(1) as `return` from log_in where username='" & _username & "' and password='" & _password & "';"

        Try
            dba.cmdReader = QueryReader(conn, query)
            dba.cmdReader.Read()

            If dba.cmdReader.GetInt32("return") = 0 Then
                MsgBox("Wrong username or password, try again.", MsgBoxStyle.Exclamation, "Error")
            Else
                Dim mainwindow As New MainWindow
                mainwindow.Show()
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

    Private Sub login_username_GotFocus(sender As Object, e As RoutedEventArgs)
        If Not userFocused Then login_username.Text = ""
        userFocused = True
    End Sub

    Private Sub login_password_GotFocus(sender As Object, e As RoutedEventArgs)
        If Not passFocused Then login_password.Password = ""
        passFocused = True
    End Sub
End Class