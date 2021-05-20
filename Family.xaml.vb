Public Class Family
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
            dba.adapter.Fill(dba.DataSet, "FamilyTable")
            Family_Table.DataContext = dba.DataSet
            conn.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub
    Private Sub Clear()
        Name.Text = ""
        Relationship.Text = ""

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
    Private Sub Family_Table_Select(sender As Object, e As SelectionChangedEventArgs) Handles Family_Table.SelectionChanged
        Try
            Dim item = Family_Table.SelectedItem
            If IsNothing(item) Then
                Return
            End If
            selected = Family_Table.SelectedCells(0).Column.GetCellContent(item)

            Dim query = "SELECT * FROM `nat_id`.`family` WHERE ID_Number='" & ID & "' AND Family_ID='" & selected.Text & "';"

            With dba
                .cmdReader = QueryReader(ConnectDB(), query)
                .cmdReader.Read()

                Name.Text = .cmdReader("Name")
                Relationship.Text = .cmdReader("Relationship")

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
        Me.Title = "Family " & Me.TitleName
        Load_Table("SELECT * FROM `nat_id`.`family` WHERE ID_Number='" & ID & "';")
    End Sub

    Private Sub Clear_btn_Click(sender As Object, e As RoutedEventArgs) Handles Clear_btn.Click
        Clear()
    End Sub

    Private Sub Add_btn_Click(sender As Object, e As RoutedEventArgs) Handles Add_btn.Click

        If Check_Empty() Then Return

        query = "INSERT INTO `nat_id`.`family`
                    (`ID_Number`,
                    `Name`,
                    `Relationship`)
                    VALUES
                    ('" & ID & "',
                    '" & Name.Text & "',
                    '" & Relationship.Text & "');"

        If QueryReader(ConnectDB(), query).RecordsAffected = 1 Then
            MsgBox("Insert Success!", MsgBoxStyle.Information, "Success")
        Else
            MsgBox("Failed", MsgBoxStyle.Critical, "Failed")
        End If

        Load_Table("SELECT * FROM `nat_id`.`family` WHERE ID_Number='" & ID & "';")
        Clear()
    End Sub

    Private Sub Update_btn_Click(sender As Object, e As RoutedEventArgs) Handles Update_btn.Click

        If Check_Empty() Then Return

        query = "UPDATE `nat_id`.`family`
                SET
                `Name` = '" & Name.Text & "',
                `Relationship` = '" & Relationship.Text & "'
                WHERE `Family_ID` = '" & selected.Text & "' AND `ID_Number` = '" & ID & "';"

        If QueryReader(ConnectDB(), query).RecordsAffected = 1 Then
            MsgBox("Successfully Updated!", MsgBoxStyle.Information, "Success")
        Else
            MsgBox("Failed", MsgBoxStyle.Critical, "Failed")
        End If

        Clear()
        Load_Table("SELECT * FROM `nat_id`.`family` WHERE ID_Number='" & ID & "';")

    End Sub
    Private Function Check_Empty() As Boolean
        If IfEmpty(Name.Text) Or IfEmpty(Relationship.Text) Then
            MsgBox("Please fill the Name and Relationship appropriately.", MsgBoxStyle.Exclamation, "Prompt")
            Return True
        End If
        Return False
    End Function
    Private Sub Delete_btn_Click(sender As Object, e As RoutedEventArgs) Handles Delete_btn.Click
        query = "DELETE FROM `nat_id`.`family`
                 WHERE `Family_ID` = '" & selected.Text & "' AND `ID_Number` = '" & ID & "';"

        If QueryReader(ConnectDB(), query).RecordsAffected = 1 Then
            MsgBox("Successfully Deleted!", MsgBoxStyle.Information, "Success")
        Else
            MsgBox("Failed", MsgBoxStyle.Critical, "Failed")
        End If

        Clear()
        Load_Table("SELECT * FROM `nat_id`.`family` WHERE ID_Number='" & ID & "';")

    End Sub

    Private Sub Family_Table_LostFocus(sender As Object, e As RoutedEventArgs) Handles Family_Table.LostFocus
        Family_Table.SelectedItem = Nothing
    End Sub
End Class
