Class MainWindow
    Private Sub Close_Click(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub
    Private Sub MouseDrag(ByVal obj As Object, e As MouseButtonEventArgs)
        If e.LeftButton.Pressed Then DragMove()
    End Sub

    Private Sub Btn_Click_1(sender As Object, e As RoutedEventArgs)
        Page.Navigate(New Page1)
    End Sub
    Private Sub Btn_Click_2(sender As Object, e As RoutedEventArgs)
        Page.Navigate(New Page2)
    End Sub
    Private Sub Btn_Click_3(sender As Object, e As RoutedEventArgs)
        Page.Navigate(New Page3)
    End Sub
End Class
