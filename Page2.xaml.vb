Class Page2
    Dim dba As New dbAccess

    Sub Main() Handles Me.Loaded
        Try
            Dim conn = ConnectDB()

            Dim query = "SELECT ID_Number, First_Name, Last_Name FROM `nat_id`.`person`;"

            'dba.cmdReader = QueryReader(conn, query)
            'dba.cmdReader.Read()
            'text.Text = dba.cmdReader("Last_Name").ToString
            dba.adapter = QueryReader(conn, query, True)

            dba.adapter.Fill(dba.DataSet, "PersonTable")

            Data_Grid.DataContext = dba.DataSet

            conn.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub select_cell(sender As Object, e As SelectionChangedEventArgs) Handles Data_Grid.SelectionChanged
        'Dim drv = Data_Grid.SelectedItem
        Dim selected As TextBlock = Data_Grid.SelectedCells(0).Column.GetCellContent(Data_Grid.SelectedItem)

        Dim query = "SELECT Last_Name, First_Name, Last_Name, Middle_Name, Extension, Gender FROM `nat_id`.`person` WHERE ID_Number='" & selected.Text & "';"

        dba.cmdReader = QueryReader(ConnectDB(), query)
        dba.cmdReader.Read()

        Last_Name.Text = dba.cmdReader("Last_Name").ToString
        First_Name.Text = dba.cmdReader("First_Name").ToString
        Middle_Name.Text = dba.cmdReader("Middle_Name").ToString
        Extension_Name.Text = dba.cmdReader("Extension").ToString

        Try
            If dba.cmdReader("Gender").ToString = "Male" Then
                Photo.Source = Me.FindResource("Male")
            Else
                Photo.Source = Me.FindResource("Female")
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try



        dba.cmdReader.Close()

    End Sub
End Class
