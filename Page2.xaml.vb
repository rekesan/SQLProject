Class Page2
    'Dim dba As New dbAccess
    'Dim selected As TextBlock
    'Dim selected_bool As Boolean

    'Sub Main() Handles Me.Loaded
    '    Load_Table("SELECT * FROM `nat_id`.`person`;")
    'End Sub

    'Private Sub select_cell(sender As Object, e As SelectionChangedEventArgs) Handles Data_Grid.SelectionChanged
    '    Try
    '        Dim item = Data_Grid.SelectedItem
    '        If IsNothing(item) Then
    '            Return
    '        End If
    '        selected = Data_Grid.SelectedCells(0).Column.GetCellContent(item)

    '        Dim query = "SELECT * FROM `nat_id`.`person` WHERE ID_Number='" & selected.Text & "';"
    '        Dim conn = ConnectDB()

    '        dba.cmdReader = QueryReader(conn, query)
    '        dba.cmdReader.Read()

    '        Last_Name.Text = dba.cmdReader("Last_Name").ToString
    '        First_Name.Text = dba.cmdReader("First_Name").ToString
    '        Middle_Name.Text = dba.cmdReader("Middle_Name").ToString
    '        Dim ext = dba.cmdReader("Extension").ToString

    '        If ext.Length = 0 Then
    '            Extension_Name.Text = "N/A"
    '        Else
    '            Extension_Name.Text = ext
    '        End If

    '        Dim gndr = dba.cmdReader("Gender").ToString
    '        Try
    '            If gndr = "Male" Then
    '                Photo.Source = Me.FindResource("Male")
    '                Gender.Text = gndr
    '            Else
    '                Photo.Source = Me.FindResource("Female")
    '                Gender.Text = gndr
    '            End If
    '        Catch ex As Exception
    '            MsgBox(ex.Message)
    '        End Try

    '        With dba.cmdReader("Birth_Date").ToString
    '            Birth_Date.Text = .Remove(.IndexOf(" "))
    '        End With

    '        Marital_Status.Text = dba.cmdReader("Marital_Status").ToString
    '        Blood_Type.Text = dba.cmdReader("Blood_Type").ToString

    '        ID_Number.Text = dba.cmdReader("ID_Number").ToString
    '        Zip_Code.Text = dba.cmdReader("Zip_Code")

    '        Dim st As String
    '        With dba
    '            If .cmdReader("Street_Number").ToString = Nothing Then
    '                st = ""
    '                If Not .cmdReader("Street_Name") = Nothing Then
    '                    st = st & .cmdReader("Street_Name")
    '                End If
    '            Else
    '                st = .cmdReader("Street_Number").ToString
    '                If Not .cmdReader("Street_Name") = Nothing Then
    '                    st = st & ", " & .cmdReader("Street_Name")
    '                End If
    '            End If

    '        End With

    '        Street_Number.Text = dba.cmdReader("Street_Number").ToString
    '        Street_Name.Text = dba.cmdReader("Street_Name").ToString
    '        Municipality.Text = dba.cmdReader("Municipality").ToString
    '        Province.Text = dba.cmdReader("Province_City").ToString

    '        dba.cmdReader.Close()
    '        conn.Close()
    '        dba.conn.Close()

    '        Add_Btn.IsEnabled = False
    '        'Clear_Btn.IsEnabled = True
    '        Delete.IsEnabled = True
    '        Update.IsEnabled = True
    '        Work_btn.IsEnabled = True
    '        Educ_BG_btn.IsEnabled = True
    '        Family_btn.IsEnabled = True
    '        Criminal_Rec_btn.IsEnabled = True

    '        selected_bool = True
    '    Catch ex As ArgumentNullException

    '    End Try

    'End Sub

    'Private Sub Delete_Click(sender As Object, e As RoutedEventArgs) Handles Delete.Click
    '    If MsgBox("Are You sure you want to delete?", MsgBoxStyle.YesNo, "Confirm Delete") = 6 Then
    '        Dim idNum = selected.Text
    '        Dim query = "DELETE FROM `nat_id`.`educational background` WHERE ID_Number='" & idNum & "';" &
    '                    "DELETE FROM `nat_id`.`biometric` WHERE ID_Number='" & idNum & "';" &
    '                    "DELETE FROM `nat_id`.`crime record` WHERE ID_Number='" & idNum & "';" &
    '                    "DELETE FROM `nat_id`.`family` WHERE ID_Number='" & idNum & "';" &
    '                    "DELETE FROM `nat_id`.`person` WHERE ID_Number='" & idNum & "';"

    '        Dim conn = ConnectDB()
    '        dba.cmdReader = QueryReader(conn, query)

    '        With dba.cmdReader
    '            .Read()
    '            If .RecordsAffected = 1 Then
    '                MsgBox("Deleted", MsgBoxStyle.Information)
    '            Else
    '                MsgBox("No Records deleted", MsgBoxStyle.Exclamation)
    '            End If
    '        End With

    '        dba.cmdReader.Close()
    '        dba.conn.Close()
    '        conn.Close()

    '        Clear()

    '        Load_Table("SELECT * FROM `nat_id`.`person`;")
    '    End If


    'End Sub

    'Private Sub Search(sender As Object, e As TextChangedEventArgs)
    '    Dim search = SearchBox.Text
    '    Dim qry = "SELECT * 
    '               FROM `nat_id`.`person`
    '               WHERE ID_Number LIKE '%" & search & "%' 
    '               OR Last_Name LIKE '%" & search & "%' 
    '               OR First_Name LIKE '%" & search & "%' 
    '               OR Middle_Name LIKE '%" & search & "%' 
    '               OR Municipality LIKE '%" & search & "%' 
    '               OR Province_City LIKE '%" & search & "%';"
    '    Load_Table(qry)
    'End Sub

    'Sub Load_Table(qry As String)
    '    Try
    '        Dim conn = ConnectDB()
    '        dba.adapter = QueryReader(conn, qry, True)
    '        dba.DataSet = New Data.DataSet()
    '        dba.adapter.Fill(dba.DataSet, "PersonTable")
    '        Data_Grid.DataContext = dba.DataSet
    '        conn.Close()
    '    Catch ex As Exception
    '        MsgBox(ex.Message, MsgBoxStyle.Critical)
    '    End Try
    'End Sub

    'Sub GotFocus_Txt(sender As Object, e As RoutedEventArgs)
    '    Try
    '        If Not selected_bool Then
    '            Dim a As TextBox
    '            a = e.Source
    '            a.Text = ""
    '        End If
    '    Catch ex As Exception
    '        MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
    '    End Try
    'End Sub

    'Private Sub Clear_Btn_Click(sender As Object, e As RoutedEventArgs) Handles Clear_Btn.Click

    '    Clear()
    'End Sub

    'Private Sub Clear()

    '    Last_Name.Text = ""
    '    First_Name.Text = ""
    '    Middle_Name.Text = ""

    '    Extension_Name.Text = ""

    '    Gender.Text = ""
    '    Birth_Date.Text = "1/1/1970"

    '    Marital_Status.Text = ""
    '    Blood_Type.Text = ""

    '    ID_Number.Text = ""

    '    Zip_Code.Text = ""
    '    Street_Name.Text = "Street Name"
    '    Street_Number.Text = "Street Number"
    '    Municipality.Text = "Municipality"
    '    Province.Text = "Province/City"
    '    Photo.Source = Nothing

    '    Add_Btn.IsEnabled = True
    '    'Clear_Btn.IsEnabled = False
    '    Delete.IsEnabled = False
    '    Update.IsEnabled = False
    '    Work_btn.IsEnabled = False
    '    Educ_BG_btn.IsEnabled = False
    '    Family_btn.IsEnabled = False
    '    Criminal_Rec_btn.IsEnabled = False
    '    selected_bool = False

    'End Sub

    'Private Sub Unfocused(sender As Object, e As RoutedEventArgs) Handles Data_Grid.LostFocus
    '    Data_Grid.SelectedItem = Nothing
    'End Sub

    'Private Sub Add_Click(sender As Object, e As RoutedEventArgs) Handles Add_Btn.Click
    '    If Not IfEmpty(Last_Name.Text) Or Not IfEmpty(First_Name.Text) Then
    '    End If

    '    Dim empty =
    '    If empty.Any Then
    '        MsgBox("Please fill")
    '    End If

    'End Sub

End Class