Namespace InfiniteAsyncSourceMVVMSample

    Public Class UserData

        Public Sub New(ByVal id As Integer, ByVal firstName As String, ByVal lastName As String)
            Me.Id = id
            Me.FirstName = firstName
            Me.LastName = lastName
        End Sub

        Public ReadOnly Property Id As Integer

        Public ReadOnly Property FirstName As String

        Public ReadOnly Property LastName As String

        Public ReadOnly Property FullName As String
            Get
                Return FirstName & " " & LastName
            End Get
        End Property
    End Class
End Namespace
