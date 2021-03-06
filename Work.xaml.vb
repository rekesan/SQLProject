Public Class Work

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
            dba.adapter.Fill(dba.DataSet, "WorkTable")
            Work_Table.DataContext = dba.DataSet
            conn.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub
    Private Sub Clear()
        Name.Text = ""
        Type.Text = ""
        Address.Text = ""

        Add_btn.IsEnabled = True
        Update_btn.IsEnabled = False
        Delete_btn.IsEnabled = False
    End Sub
    Private Sub Work_Table_LostFocus(sender As Object, e As RoutedEventArgs) Handles Work_Table.LostFocus
        Work_Table.SelectedItem = Nothing
    End Sub

    Property ID As String
    Property TitleName As String
    Private dba As New dbAccess
    Dim selected As TextBlock
    Dim selected_bool As Boolean
    Dim query As String

    Private Sub Work_Table_Select(sender As Object, e As SelectionChangedEventArgs) Handles Work_Table.SelectionChanged
        Try
            Dim item = Work_Table.SelectedItem
            If IsNothing(item) Then
                Return
            End If
            selected = Work_Table.SelectedCells(0).Column.GetCellContent(item)

            Dim query = "SELECT * FROM `nat_id`.`work` WHERE ID_Number='" & ID & "' AND Work_ID='" & selected.Text & "';"

            With dba
                .cmdReader = QueryReader(ConnectDB(), query)
                .cmdReader.Read()

                Name.Text = .cmdReader("Name")
                Type.Text = .cmdReader("Type")
                Address.Text = .cmdReader("Address")

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
        Me.Title = "Work " & TitleName
        Load_Table("SELECT * FROM `nat_id`.`work` WHERE ID_Number='" & ID & "';")
    End Sub

    Private Sub Clear_btn_Click(sender As Object, e As RoutedEventArgs) Handles Clear_btn.Click
        Clear()
    End Sub

    Private Sub Add_btn_Click(sender As Object, e As RoutedEventArgs) Handles Add_btn.Click

        If Check_Empty() Then Return

        query = "INSERT INTO `nat_id`.`work`
                    (`ID_Number`,
                    `Name`,
                    `Type`,
                    `Address`)
                    VALUES
                    ('" & ID & "',
                    '" & Name.Text & "',
                    '" & Type.Text & "',
                    '" & Address.Text & "');"

        If QueryReader(ConnectDB(), query).RecordsAffected = 1 Then
            MsgBox("Insert Success!", MsgBoxStyle.Information, "Success")
        Else
            MsgBox("Failed", MsgBoxStyle.Critical, "Failed")
        End If

        Load_Table("SELECT * FROM `nat_id`.`work` WHERE ID_Number='" & ID & "';")
        Clear()
    End Sub

    Private Sub Update_btn_Click(sender As Object, e As RoutedEventArgs) Handles Update_btn.Click
        If Check_Empty() Then Return

        query = "UPDATE `nat_id`.`work`
                SET
                `Name` = '" & Name.Text & "',
                `Type` = '" & Type.Text & "',
                `Address` = '" & Address.Text & "'
                WHERE `Work_ID` = '" & selected.Text & "' AND `ID_Number` = '" & ID & "';"

        If QueryReader(ConnectDB(), query).RecordsAffected = 1 Then
            MsgBox("Successfully Updated!", MsgBoxStyle.Information, "Success")
        Else
            MsgBox("Failed", MsgBoxStyle.Critical, "Failed")
        End If

        Clear()
        Load_Table("SELECT * FROM `nat_id`.`work` WHERE ID_Number='" & ID & "';")

    End Sub

    Private Function Check_Empty() As Boolean
        If IfEmpty(Name.Text) Or IfEmpty(Type.Text) Or IfEmpty(Address.Text) Then
            MsgBox("Please fill the Name, Type, and Address appropriately.", MsgBoxStyle.Exclamation, "Prompt")
            Return True
        End If
        Return False
    End Function

    Private Sub Delete_btn_Click(sender As Object, e As RoutedEventArgs) Handles Delete_btn.Click
        query = "DELETE FROM `nat_id`.`work`
                 WHERE `Work_ID` = '" & selected.Text & "' AND `ID_Number` = '" & ID & "';"

        If QueryReader(ConnectDB(), query).RecordsAffected = 1 Then
            MsgBox("Successfully Deleted!", MsgBoxStyle.Information, "Success")
        Else
            MsgBox("Failed", MsgBoxStyle.Critical, "Failed")
        End If

        Clear()
        Load_Table("SELECT * FROM `nat_id`.`work` WHERE ID_Number='" & ID & "';")

    End Sub


End Class
