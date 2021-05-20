Imports Microsoft.Win32

Class MainWindow
    'Private backColorNavigated = Color.FromRgb(70, 70, 77)
    'Private backColor = Color.FromRgb(46, 46, 50)
    Dim dba As New dbAccess
    Dim selected As TextBlock
    Dim selected_bool As Boolean
    Dim image_selected As String

    Private Sub Close_Click(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub
    Private Sub MouseDrag(ByVal obj As Object, e As MouseButtonEventArgs)
        If e.LeftButton Then DragMove()
    End Sub

    Private Sub Btn_Click_1(sender As Object, e As RoutedEventArgs)
        '    ChangeColor(Button1, backColorNavigated)
        '    ChangeColor(Button2, backColor)
        '    ChangeColor(Button3, backColor)
        '    Page.Navigate(New Page1)
    End Sub
    Private Sub Btn_Click_2(sender As Object, e As RoutedEventArgs)
        '    ChangeColor(Button1, backColor)
        '    ChangeColor(Button2, backColorNavigated)
        '    ChangeColor(Button3, backColor)
        '    Page.Navigate(New Page2)
    End Sub
    Private Sub Btn_Click_3(sender As Object, e As RoutedEventArgs)
        '    ChangeColor(Button1, backColor)
        '    ChangeColor(Button2, backColor)
        '    ChangeColor(Button3, backColorNavigated)
        '    Page.Navigate(New Page3)
    End Sub

    Private Sub Btn_Click_About(sender As Object, e As RoutedEventArgs)
        Dim abt As New about
        abt.ShowDialog()
    End Sub

    Sub ChangeColor(btn As Button, color As Color)
        btn.Background = New SolidColorBrush(color)
    End Sub

    Sub Click_HyperLink(sender As Object, e As RequestNavigateEventArgs)
        Process.Start(New ProcessStartInfo(e.Uri.AbsoluteUri))
        e.Handled = True
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
        Load_Table("SELECT * FROM `nat_id`.`person`;")
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
            Dim query = "DELETE FROM `nat_id`.`educational background` WHERE ID_Number='" & idNum & "';" &
                        "DELETE FROM `nat_id`.`biometric` WHERE ID_Number='" & idNum & "';" &
                        "DELETE FROM `nat_id`.`crime record` WHERE ID_Number='" & idNum & "';" &
                        "DELETE FROM `nat_id`.`family` WHERE ID_Number='" & idNum & "';" &
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

            Clear()

            Load_Table("SELECT * FROM `nat_id`.`person`;")
        End If


    End Sub

    Private Sub Search(sender As Object, e As TextChangedEventArgs)
        Dim search = SearchBox.Text
        Dim qry = "SELECT * 
                   FROM `nat_id`.`person`
                   WHERE ID_Number LIKE '%" & search & "%' 
                   OR Last_Name LIKE '%" & search & "%' 
                   OR First_Name LIKE '%" & search & "%' 
                   OR Middle_Name LIKE '%" & search & "%' 
                   OR Municipality LIKE '%" & search & "%' 
                   OR Province_City LIKE '%" & search & "%';"
        Load_Table(qry)
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
            MsgBox(ex.Message, MsgBoxStyle.Critical)
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
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
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
                dt.Text = ""
            End If
        Next

        ID_Number.Text = ""

        Zip_Code.Text = ""
        Street_Name.Text = "Street Name"
        Street_Number.Text = "Street Number"
        Municipality.Text = "Municipality"
        Province.Text = "Province/City"
        Photo.Source = Nothing

        Add_Btn.IsEnabled = True
        'Clear_Btn.IsEnabled = False
        Delete.IsEnabled = False
        Update.IsEnabled = False
        Work_btn.IsEnabled = False
        Educ_BG_btn.IsEnabled = False
        Family_btn.IsEnabled = False
        Criminal_Rec_btn.IsEnabled = False
        selected_bool = False

    End Sub

    Private Sub Unfocused(sender As Object, e As RoutedEventArgs) Handles Data_Grid.LostFocus
        Data_Grid.SelectedItem = Nothing
    End Sub

    Private Sub Add_Click(sender As Object, e As RoutedEventArgs) Handles Add_Btn.Click
        Dim str = "Please Fill the following:"

        For Each ctrl As Grid In TextBoxGrid.Children
            If TypeOf ctrl.Children.Item(1) Is TextBox Then
                Dim txtbox As TextBox = ctrl.Children.Item(1)
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

        MsgBox(str, MsgBoxStyle.Information, "Error")
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

            Dim bitmap As New BitmapImage(New Uri(image_selected))
            With bitmap
                If Not .PixelHeight = .PixelWidth Then
                    MsgBox("Please choose an image with the same dimension.")
                    Return
                End If
            End With

            Photo.Source = bitmap
        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Work_btn_Click(sender As Object, e As RoutedEventArgs) Handles Work_btn.Click

    End Sub

    Private Sub Educ_BG_btn_Click(sender As Object, e As RoutedEventArgs) Handles Educ_BG_btn.Click

    End Sub

    Private Sub Family_btn_Click(sender As Object, e As RoutedEventArgs) Handles Family_btn.Click

    End Sub

    Private Sub Criminal_Rec_btn_Click(sender As Object, e As RoutedEventArgs) Handles Criminal_Rec_btn.Click

    End Sub
End Class
