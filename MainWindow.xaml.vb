Imports Microsoft.Win32

Class MainWindow
    Dim dba As New dbAccess
    Dim selected As TextBlock
    Dim selected_bool As Boolean
    Dim image_selected As String
    Dim image_selected_FileName As String
    Dim selected_name As String
    Dim query As String
    Dim Image_Directory As String
    Dim selected_image_fromDB As BitmapImage


    Private Sub MouseDrag(ByVal obj As Object, e As MouseButtonEventArgs)
        If e.LeftButton Then DragMove()
    End Sub

    Private Sub Btn_Click_About(sender As Object, e As RoutedEventArgs)
        Dim abt As New about
        abt.ShowDialog()
    End Sub

    Sub Click_HyperLink(sender As Object, e As RequestNavigateEventArgs)
        Process.Start(New ProcessStartInfo(e.Uri.AbsoluteUri))
        e.Handled = True
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

    Sub Main() Handles Me.Loaded
        Try

            Image_Directory = My.Computer.FileSystem.SpecialDirectories.MyDocuments + "/Nat_ID_Images"
            My.Computer.FileSystem.CreateDirectory(Image_Directory)

        Catch ex As Exception

        End Try
        Load_Table("SELECT * FROM `nat_id`.`person`;")
    End Sub

    Private Sub Data_Grid_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles Data_Grid.SelectionChanged
        Try
            Dim item = Data_Grid.SelectedItem
            If IsNothing(item) Then
                Return
            End If
            selected = Data_Grid.SelectedCells(0).Column.GetCellContent(item)


            query = "SELECT * 
                        FROM `nat_id`.`person`
                        LEFT JOIN `nat_id`.`biometric` USING (ID_Number)
                        WHERE ID_Number='" & selected.Text & "';"
            Dim conn = ConnectDB()

            dba.cmdReader = QueryReader(conn, query)
            dba.cmdReader.Read()

            Last_Name.Text = dba.cmdReader("Last_Name").ToString
            First_Name.Text = dba.cmdReader("First_Name").ToString
            Middle_Name.Text = dba.cmdReader("Middle_Name").ToString

            selected_name = "[" & Last_Name.Text & ", " & First_Name.Text & " " & Middle_Name.Text & "]"

            Dim ext = dba.cmdReader("Extension").ToString

            If ext.Length = 0 Then
                Extension_Name.Text = "N/A"
            Else
                Extension_Name.Text = ext
            End If

            Dim gndr = dba.cmdReader("Gender").ToString
            Dim photo_loc = dba.cmdReader("photo_loc")

            Try
                selected_image_fromDB = New BitmapImage(New Uri(photo_loc))
                Photo.Source = If(IsDBNull(photo_loc), Nothing, selected_image_fromDB)
            Catch ex As Exception

            End Try


            With dba.cmdReader("Birth_Date").ToString
                Birth_Date.Text = .Remove(.IndexOf(" "))
            End With

            Gender.Text = dba.cmdReader("Gender").ToString
            Marital_Status.Text = dba.cmdReader("Marital_Status").ToString
            Blood_Type.Text = dba.cmdReader("Blood_Type").ToString

            ID_Number.Text = dba.cmdReader("ID_Number").ToString
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

            Street_Number.Text = dba.cmdReader("Street_Number").ToString
            Street_Name.Text = dba.cmdReader("Street_Name").ToString
            Municipality.Text = dba.cmdReader("Municipality").ToString
            Province.Text = dba.cmdReader("Province_City").ToString

            dba.cmdReader.Close()
            conn.Close()
            dba.conn.Close()

            Add_Btn.IsEnabled = False
            'Clear_Btn.IsEnabled = True
            Delete.IsEnabled = True
            Update.IsEnabled = True
            Work_btn.IsEnabled = True
            Educ_BG_btn.IsEnabled = True
            Family_btn.IsEnabled = True
            Criminal_Rec_btn.IsEnabled = True

            selected_bool = True
        Catch ex As ArgumentNullException

        End Try

    End Sub

    Private Sub Delete_Click(sender As Object, e As RoutedEventArgs) Handles Delete.Click
        If MsgBox("Are You sure you want to delete?", MsgBoxStyle.YesNo, "Confirm Delete") = 6 Then
            Dim idNum = selected.Text
            query = "DELETE FROM `nat_id`.`educational background` WHERE ID_Number='" & idNum & "';" &
                        "DELETE FROM `nat_id`.`biometric` WHERE ID_Number='" & idNum & "';" &
                        "DELETE FROM `nat_id`.`crime record` WHERE ID_Number='" & idNum & "';" &
                        "DELETE FROM `nat_id`.`family` WHERE ID_Number='" & idNum & "';" &
                        "DELETE FROM `nat_id`.`work` WHERE ID_Number='" & idNum & "';" &
                        "DELETE FROM `nat_id`.`person` WHERE ID_Number='" & idNum & "';"


            Dim conn = ConnectDB()
            dba.cmdReader = QueryReader(conn, query)

            With dba.cmdReader
                .Read()
                If .RecordsAffected > 0 Then
                    MsgBox("Deleted", MsgBoxStyle.Information, "Prompt")
                Else
                    MsgBox("No Records deleted", MsgBoxStyle.Exclamation, "Prompt")
                End If
            End With

            dba.cmdReader.Close()
            dba.conn.Close()
            conn.Close()

            Clear()

            Load_Table("SELECT * FROM `nat_id`.`person`;")
        End If


    End Sub

    Private Sub Search(sender As Object, e As TextChangedEventArgs)
        Dim search = SearchBox.Text
        query = "SELECT * 
                   FROM `nat_id`.`person`
                   WHERE ID_Number LIKE '%" & search & "%' 
                   OR Last_Name LIKE '%" & search & "%' 
                   OR First_Name LIKE '%" & search & "%' 
                   OR Middle_Name LIKE '%" & search & "%' 
                   OR Municipality LIKE '%" & search & "%' 
                   OR Province_City LIKE '%" & search & "%';"
        Load_Table(query)
    End Sub

    Sub Load_Table(qry As String)
        Try
            Dim conn = ConnectDB()
            dba.adapter = QueryReader(conn, qry, True)
            dba.DataSet = New Data.DataSet()
            dba.adapter.Fill(dba.DataSet, "PersonTable")
            Data_Grid.DataContext = dba.DataSet
            conn.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Prompt")
        End Try
    End Sub

    Sub GotFocus_Txt(sender As Object, e As RoutedEventArgs)
        Try
            If Not selected_bool Then
                Dim a As TextBox
                a = e.Source
                a.Text = ""
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Prompt")
        End Try
    End Sub

    Private Sub Clear_Btn_Click(sender As Object, e As RoutedEventArgs) Handles Clear_Btn.Click
        Clear()
    End Sub

    Private Sub Clear()

        For Each ctrl As Grid In TextBoxGrid.Children
            If TypeOf ctrl.Children.Item(1) Is TextBox Then
                Dim txtbox As TextBox = ctrl.Children.Item(1)
                txtbox.Text = ""
            End If
            If TypeOf ctrl.Children.Item(1) Is DatePicker Then
                Dim dt As DatePicker = ctrl.Children.Item(1)
                dt.Text = "1970-01-01"
            End If
        Next


        ID_Number.Text = ""
        Gender.Text = ""
        Blood_Type.Text = ""
        Marital_Status.Text = ""
        Birth_Date.Text = "1970-01-01"

        Zip_Code.Text = ""
        Street_Name.Text = "Barangay"
        Street_Number.Text = "Street"
        Municipality.Text = "Municipality"
        Province.Text = "Province/City"
        Photo.Source = Nothing

        image_selected = ""
        image_selected_FileName = ""
        selected_name = ""

        Add_Btn.IsEnabled = True
        Delete.IsEnabled = False
        Update.IsEnabled = False
        Work_btn.IsEnabled = False
        Educ_BG_btn.IsEnabled = False
        Family_btn.IsEnabled = False
        Criminal_Rec_btn.IsEnabled = False
        selected_bool = False

    End Sub

    Private Sub Data_Grid_LostFocus(sender As Object, e As RoutedEventArgs) Handles Data_Grid.LostFocus
        Data_Grid.SelectedItem = Nothing
    End Sub

    Private Sub Add_Click(sender As Object, e As RoutedEventArgs) Handles Add_Btn.Click
        Dim check = Check_Emty_Textboxes()
        If Not check.Equals("Please Fill the following:") Then
            MsgBox(check, MsgBoxStyle.Information, "Prompt")
            Return
        End If

        Dim Generated_ID As String

        Do
            Generated_ID = GenerateID()
            query = "SELECT Count(1) AS `RETURN` FROM nat_id.`person` WHERE ID_Number='" & Generated_ID & "';"
            dba.cmdReader = QueryReader(ConnectDB(), query)
            dba.cmdReader.Read()
        Loop Until dba.cmdReader("RETURN") = 0

        Change_Directory_Image()

        query = "INSERT INTO `nat_id`.`person`
                (`ID_Number`,
                `First_Name`,
                `Middle_Name`,
                `Last_Name`,
                `Extension`,
                `Marital_Status`,
                `Gender`,
                `Blood_Type`,
                `Birth_Date`,
                `Street_Number`,
                `Street_Name`,
                `Municipality`,
                `Province_City`,
                `Zip_Code`)
                VALUES
                ('" & Generated_ID & "',
                '" & First_Name.Text & "',
                '" & Middle_Name.Text & "',
                '" & Last_Name.Text & "',
                '" & Extension_Name.Text & "',
                '" & Marital_Status.Text & "',
                '" & Gender.Text & "',
                '" & Blood_Type.Text & "',
                '" & Format(Birth_Date.SelectedDate.Value, "yyyy/MM/dd") & "',
                '" & Street_Number.Text & "',
                '" & Street_Name.Text & "',
                '" & Municipality.Text & "',
                '" & Province.Text & "',
                '" & Zip_Code.Text & "');

                INSERT INTO `nat_id`.`biometric`
                (`photo_loc`,
                `ID_Number`)
                VALUES
                (
                '" & Image_Directory.Replace("\", "/") & "/" & image_selected_FileName & "',
                '" & Generated_ID & "');"

        dba.cmdReader = QueryReader(ConnectDB(), query)

        With dba.cmdReader
            .Read()
            If .RecordsAffected > 0 Then
                MsgBox("Added Successfully!", MsgBoxStyle.Information, "Prompt")
            Else
                MsgBox("Failed", MsgBoxStyle.Exclamation, "Prompt")
            End If
        End With

        dba.cmdReader.Close()
        dba.conn.Close()


        Load_Table("SELECT * FROM `nat_id`.`person`;")
        Clear()
    End Sub

    Private Function Check_Emty_Textboxes() As String
        Dim str = "Please Fill the following:"

        For Each ctrl As Grid In TextBoxGrid.Children
            If TypeOf ctrl.Children.Item(1) Is TextBox Then
                Dim txtbox As TextBox = ctrl.Children.Item(1)
                If txtbox.Name.Equals("Extension_Name") Then Continue For
                If (IfEmpty(txtbox.Text)) Then str &= vbNewLine & txtbox.Name.Replace("_", " ")
            End If
            If TypeOf ctrl.Children.Item(1) Is DatePicker Then
                Dim txtbox As DatePicker = ctrl.Children.Item(1)
                If (IfEmpty(txtbox.Text)) Then str &= vbNewLine & txtbox.Name.Replace("_", " ")
            End If
        Next

        If Photo.Source Is Nothing Then str &= vbNewLine & "Image"

        If IfEmpty(Street_Number.Text) Then str &= vbNewLine & "Street Number"
        If IfEmpty(Street_Name.Text) Then str &= vbNewLine & "Street Name"
        If IfEmpty(Municipality.Text) Then str &= vbNewLine & "Municipality"
        If IfEmpty(Province.Text) Then str &= vbNewLine & "Province/City"
        If IfEmpty(Zip_Code.Text) Then str &= vbNewLine & "Zip Code"

        Return str
    End Function

    Sub Change_Directory_Image()
        Try
            My.Computer.FileSystem.CopyFile(image_selected, Image_Directory & "/" & image_selected_FileName)
            My.Computer.FileSystem.DeleteFile(image_selected)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Add_Image_Btn_Click(sender As Object, e As RoutedEventArgs) Handles Add_Image_Btn.Click
        Try
            Dim fileDiag As New OpenFileDialog
            fileDiag.DefaultExt = ".png"
            fileDiag.Filter = "PNG|*.png|BMP|*.bmp|JPG|*.jpg;*.jpeg|TIFF|*.tif;*.tiff|All Graphics Types|*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff"
            fileDiag.Title = "Select Image"
            fileDiag.Multiselect = False
            fileDiag.ShowDialog()

            image_selected = fileDiag.FileName
            image_selected_FileName = fileDiag.SafeFileName

            Dim bitmap As New BitmapImage(New Uri(image_selected))
            'With bitmap
            '    If Not .PixelHeight = .PixelWidth Then
            '        MsgBox("Please choose an image with the same dimension.")
            '        Return
            '    End If
            'End With

            Photo.Source = bitmap
        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Work_btn_Click(sender As Object, e As RoutedEventArgs) Handles Work_btn.Click
        Dim work As New Work
        With work
            .ID = selected.Text
            .TitleName = selected_name
            .ShowDialog()
        End With
    End Sub

    Private Sub Educ_BG_btn_Click(sender As Object, e As RoutedEventArgs) Handles Educ_BG_btn.Click
        Dim educ As New Education
        With educ
            .ID = selected.Text
            .TitleName = selected_name
            .ShowDialog()
        End With
    End Sub

    Private Sub Family_btn_Click(sender As Object, e As RoutedEventArgs) Handles Family_btn.Click
        Dim family As New Family
        With family
            .ID = selected.Text
            .TitleName = selected_name
            .ShowDialog()
        End With
    End Sub

    Private Sub Criminal_Rec_btn_Click(sender As Object, e As RoutedEventArgs) Handles Criminal_Rec_btn.Click
        Dim crime As New CrimeRecords
        With crime
            .ID = selected.Text
            .TitleName = selected_name
            .ShowDialog()
        End With
    End Sub

    Private Sub Update_Click(sender As Object, e As RoutedEventArgs) Handles Update.Click
        Dim check = Check_Emty_Textboxes()
        If Not check.Equals("Please Fill the following:") Then
            MsgBox(check, MsgBoxStyle.Information, "Prompt")
            Return
        End If

        Dim img As String

        If Not IfEmpty(image_selected_FileName) Then
            img = Image_Directory.Replace("\", "/") & "/" & image_selected_FileName
        Else
            img = selected_image_fromDB.UriSource.OriginalString.Replace("\", "/")
        End If

        query = "UPDATE `nat_id`.`person`
                SET
                `First_Name` = '" & First_Name.Text & "',
                `Middle_Name` = '" & Middle_Name.Text & "',
                `Last_Name` = '" & Last_Name.Text & "',
                `Extension` = '" & If(Extension_Name.Text.Equals("N/A"), "", Extension_Name.Text) & "',
                `Marital_Status` = '" & Marital_Status.Text & "',
                `Gender` = '" & Gender.Text & "',
                `Blood_Type` = '" & Blood_Type.Text & "',
                `Birth_Date` = '" & Format(Birth_Date.SelectedDate.Value, "yyyy/MM/dd") & "',
                `Street_Number` = '" & Street_Number.Text & "',
                `Street_Name` = '" & Street_Name.Text & "',
                `Municipality` = '" & Municipality.Text & "',
                `Province_City` = '" & Province.Text & "',
                `Zip_Code` = '" & Zip_Code.Text & "'
                WHERE `ID_Number` = '" & selected.Text & "';

                UPDATE `nat_id`.`biometric`
                SET
                `photo_loc` = '" & img & "'
                WHERE `ID_Number` = '" & selected.Text & "';"

        Change_Directory_Image()

        dba.cmdReader = QueryReader(ConnectDB(), query)

        With dba.cmdReader
            .Read()
            If .RecordsAffected > 0 Then
                MsgBox("Successfully Updated!", MsgBoxStyle.Information, "Prompt")
            Else
                MsgBox("Failed", MsgBoxStyle.Exclamation, "Prompt")
            End If
        End With

        dba.cmdReader.Close()
        dba.conn.Close()

        Load_Table("SELECT * FROM `nat_id`.`person`;")
        Clear()

    End Sub
End Class
