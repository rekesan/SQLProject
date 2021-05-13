Class MainWindow
    Private backColorNavigated = Color.FromRgb(70, 70, 77)
    Private backColor = Color.FromRgb(46, 46, 50)
    Private Sub Close_Click(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub
    Private Sub MouseDrag(ByVal obj As Object, e As MouseButtonEventArgs)
        If e.LeftButton Then DragMove()
    End Sub

    Private Sub Btn_Click_1(sender As Object, e As RoutedEventArgs)
        ChangeColor(Button1, backColorNavigated)
        ChangeColor(Button2, backColor)
        ChangeColor(Button3, backColor)
        Page.Navigate(New Page1)
    End Sub
    Private Sub Btn_Click_2(sender As Object, e As RoutedEventArgs)
        ChangeColor(Button1, backColor)
        ChangeColor(Button2, backColorNavigated)
        ChangeColor(Button3, backColor)
        Page.Navigate(New Page2)
    End Sub
    Private Sub Btn_Click_3(sender As Object, e As RoutedEventArgs)
        ChangeColor(Button1, backColor)
        ChangeColor(Button2, backColor)
        ChangeColor(Button3, backColorNavigated)
        Page.Navigate(New Page3)
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
            Me.WindowState = WindowState.Maximized
        Else
            Me.WindowState = WindowState.Normal
        End If
    End Sub

    Private Sub Minimize(sender As Object, e As RoutedEventArgs)
        Me.WindowState = WindowState.Minimized
    End Sub
End Class
