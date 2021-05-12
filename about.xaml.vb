Public Class about
    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub

    Private Sub Grid_MouseDown(sender As Object, e As MouseButtonEventArgs)
        If e.LeftButton Then
            DragMove()
        End If
    End Sub
End Class
