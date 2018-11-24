Public Class Form1
    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Yes.Click

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        MsgBox("Files unsuccessfully restored!")
        Close()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        MsgBox("Files successfully restored!")
        Close()
    End Sub

    Private Sub Button2_MouseClick(sender As Object, e As MouseEventArgs) Handles Button2.MouseClick

    End Sub
End Class
