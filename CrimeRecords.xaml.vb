Public Class CrimeRecords
    Private Sub CloseCommandBinding_Executed(sender As Object, e As ExecutedRoutedEventArgs)
        Me.Close()
    End Sub
    Private Sub Close_Click(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub
    Private Sub Maximize(sender As Object, e As RoutedEventArgs)
        If Me.WindowState = 0 Then
            Me.WindowState = 2
        Else
            Me.WindowState = 0
        End If
    End Sub
    Private Sub Minimize(sender As Object, e As RoutedEventArgs)
        Me.WindowState = 1
    End Sub
    Private Sub MouseDrag(ByVal obj As Object, e As MouseButtonEventArgs)
        If e.LeftButton Then DragMove()
    End Sub
    Sub Load_Table(qry As String)
        Try
            Dim conn = ConnectDB()
            dba.adapter = QueryReader(conn, qry, True)
            dba.DataSet = New Data.DataSet()
            dba.adapter.Fill(dba.DataSet, "CrimeTable")
            Crime_Table.DataContext = dba.DataSet
            conn.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub
    Private Sub Clear()
        Violation.Text = ""
        Year.Text = ""

        Add_btn.IsEnabled = True
        Update_btn.IsEnabled = False
        Delete_btn.IsEnabled = False
    End Sub

    Property ID As String
    Property TitleName As String
    Private dba As New dbAccess
    Dim selected As TextBlock
    Dim selected_bool As Boolean
    Dim query As String
    Private Sub Crime_Table_Select(sender As Object, e As SelectionChangedEventArgs) Handles Crime_Table.SelectionChanged
        Try
            Dim item = Crime_Table.SelectedItem
            If IsNothing(item) Then
                Return
            End If
            selected = Crime_Table.SelectedCells(0).Column.GetCellContent(item)

            Dim query = "SELECT * FROM `nat_id`.`crime record` WHERE ID_Number='" & ID & "' AND Crime_ID='" & selected.Text & "';"

            With dba
                .cmdReader = QueryReader(ConnectDB(), query)
                .cmdReader.Read()

                Violation.Text = .cmdReader("Violation")
                Year.Text = .cmdReader("Year")

                .cmdReader.Close()
                .conn.Close()
            End With

            selected_bool = True

            Add_btn.IsEnabled = False
            Update_btn.IsEnabled = True
            Delete_btn.IsEnabled = True
        Catch ex As Exception

        End Try
    End Sub

    Sub Main() Handles Me.Loaded
        Me.Title = "Criminal Record " & Me.TitleName
        Load_Table("SELECT * FROM `nat_id`.`crime record` WHERE ID_Number='" & ID & "';")
    End Sub

    Private Sub Clear_btn_Click(sender As Object, e As RoutedEventArgs) Handles Clear_btn.Click
        Clear()
    End Sub

    Private Sub Add_btn_Click(sender As Object, e As RoutedEventArgs) Handles Add_btn.Click
        query = "INSERT INTO `nat_id`.`crime record`
                    (`ID_Number`,
                    `Violation`,
                    `Year`)
                    VALUES
                    ('" & ID & "',
                    '" & Violation.Text & "',
                    '" & Year.Text & "');"

        If QueryReader(ConnectDB(), query).RecordsAffected = 1 Then
            MsgBox("Insert Success!", MsgBoxStyle.Information, "Success")
        Else
            MsgBox("Failed", MsgBoxStyle.Critical, "Failed")
        End If

        Load_Table("SELECT * FROM `nat_id`.`crime record` WHERE ID_Number='" & ID & "';")
        Clear()
    End Sub

    Private Sub Update_btn_Click(sender As Object, e As RoutedEventArgs) Handles Update_btn.Click
        query = "UPDATE `nat_id`.`crime record`
                SET
                `Violation` = '" & Violation.Text & "',
                `Year` = '" & Year.Text & "'
                WHERE `Crime_ID` = '" & selected.Text & "' AND `ID_Number` = '" & ID & "';"

        If QueryReader(ConnectDB(), query).RecordsAffected = 1 Then
            MsgBox("Successfully Updated!", MsgBoxStyle.Information, "Success")
        Else
            MsgBox("Failed", MsgBoxStyle.Critical, "Failed")
        End If

        Clear()
        Load_Table("SELECT * FROM `nat_id`.`crime record` WHERE ID_Number='" & ID & "';")

    End Sub

    Private Sub Delete_btn_Click(sender As Object, e As RoutedEventArgs) Handles Delete_btn.Click
        query = "DELETE FROM `nat_id`.`crime record`
                 WHERE `Crime_ID` = '" & selected.Text & "' AND `ID_Number` = '" & ID & "';"

        If QueryReader(ConnectDB(), query).RecordsAffected = 1 Then
            MsgBox("Successfully Deleted!", MsgBoxStyle.Information, "Success")
        Else
            MsgBox("Failed", MsgBoxStyle.Critical, "Failed")
        End If

        Clear()
        Load_Table("SELECT * FROM `nat_id`.`crime record` WHERE ID_Number='" & ID & "';")

    End Sub

    Private Sub Crime_Table_LostFocus(sender As Object, e As RoutedEventArgs) Handles Crime_Table.LostFocus
        Crime_Table.SelectedItem = Nothing
    End Sub
End Class
