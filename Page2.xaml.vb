Class Page2
    Dim dba As New dbAccess
    Dim selected As TextBlock

    Sub Main() Handles Me.Loaded
        Try
            Dim conn = ConnectDB()

            Dim query = "SELECT ID_Number, First_Name, Last_Name FROM `nat_id`.`person`;"

            'dba.cmdReader = QueryReader(conn, query)
            'dba.cmdReader.Read()
            'text.Text = dba.cmdReader("Last_Name").ToString
            dba.adapter = QueryReader(conn, query, True)
            dba.DataSet = New System.Data.DataSet()
            dba.adapter.Fill(dba.DataSet, "PersonTable")

            Data_Grid.DataContext = dba.DataSet

            conn.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub select_cell(sender As Object, e As SelectionChangedEventArgs) Handles Data_Grid.SelectionChanged
        Try
            Dim item = Data_Grid.SelectedItem
            If IsNothing(item) Then
                Return
            End If
            selected = Data_Grid.SelectedCells(0).Column.GetCellContent(item)


            Dim query = "SELECT * FROM `nat_id`.`person` WHERE ID_Number='" & selected.Text & "';"
            Dim conn = ConnectDB()

            dba.cmdReader = QueryReader(conn, query)
            dba.cmdReader.Read()

            Last_Name.Text = dba.cmdReader("Last_Name").ToString
            First_Name.Text = dba.cmdReader("First_Name").ToString
            Middle_Name.Text = dba.cmdReader("Middle_Name").ToString
            Dim ext = dba.cmdReader("Extension").ToString

            If ext.Length = 0 Then
                Extension_Name.Text = "N/A"
            Else
                Extension_Name.Text = ext
            End If

            Dim gndr = dba.cmdReader("Gender").ToString
            Try
                If gndr = "Male" Then
                    Photo.Source = Me.FindResource("Male")
                    Gender.Text = gndr
                Else
                    Photo.Source = Me.FindResource("Female")
                    Gender.Text = gndr
                End If
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try

            With dba.cmdReader("Birth_Date").ToString
                Birth_Date.Text = .Remove(.IndexOf(" "))
            End With

            Marital_Status.Text = dba.cmdReader("Marital_Status").ToString
            Blood_Type.Text = dba.cmdReader("Blood_Type").ToString

            ID_Number.Text = dba.cmdReader("ID_Number").ToString
            'Address.Text = (dba.cmdReader("Street_Number").ToString & " " & dba.cmdReader("Street_Name").ToString & " " & dba.cmdReader("Municipality").ToString & " " & dba.cmdReader("Province_City").ToString).Trim
            Zip_Code.Text = dba.cmdReader("Zip_Code")

            Dim st As String
            With dba
                If .cmdReader("Street_Number").ToString = Nothing Then
                    st = ""
                    If Not .cmdReader("Street_Name") = Nothing Then
                        st = st & .cmdReader("Street_Name")
                    End If
                Else
                    st = .cmdReader("Street_Number").ToString
                    If Not .cmdReader("Street_Name") = Nothing Then
                        st = st & ", " & .cmdReader("Street_Name")
                    End If
                End If

            End With

            Street.Text = st
            Municipality.Text = dba.cmdReader("Municipality").ToString
            Province.Text = dba.cmdReader("Province_City").ToString

            dba.cmdReader.Close()
            conn.Close()
            dba.conn.Close()

            More_Btn.IsEnabled = True
            Delete.IsEnabled = True
            Update.IsEnabled = True
        Catch ex As ArgumentNullException

        End Try

    End Sub

    Private Sub Delete_Click(sender As Object, e As RoutedEventArgs) Handles Delete.Click
        Dim idNum = selected.Text
        Dim query = "DELETE FROM `nat_id`.`educational background` WHERE ID_Number='" & idNum & "';" &
                    "DELETE FROM `nat_id`.`biometric` WHERE ID_Number='" & idNum & "';" &
                    "DELETE FROM `nat_id`.`crime record` WHERE ID_Number='" & idNum & "';" &
                    "DELETE FROM `nat_id`.`family` WHERE ID_Num='" & idNum & "';" &
                    "DELETE FROM `nat_id`.`person` WHERE ID_Number='" & idNum & "';"

        Dim conn = ConnectDB()
        dba.cmdReader = QueryReader(conn, query)

        With dba.cmdReader
            .Read()
            If .RecordsAffected = 1 Then
                MsgBox("Deleted", MsgBoxStyle.Information)
            Else
                MsgBox("No Records deleted", MsgBoxStyle.Exclamation)
            End If
        End With

        dba.cmdReader.Close()
        dba.conn.Close()
        conn.Close()

        Last_Name.Text = ""
        First_Name.Text = ""
        Middle_Name.Text = ""

        Extension_Name.Text = ""

        Gender.Text = ""
        Birth_Date.Text = ""

        Marital_Status.Text = ""
        Blood_Type.Text = ""

        ID_Number.Text = ""

        Zip_Code.Text = ""
        Street.Text = ""
        Street.Text = ""
        Municipality.Text = ""
        Province.Text = ""
        Photo.Source = Nothing
        More_Btn.IsEnabled = False
        Delete.IsEnabled = False
        Update.IsEnabled = False

        Main()

    End Sub
End Class
