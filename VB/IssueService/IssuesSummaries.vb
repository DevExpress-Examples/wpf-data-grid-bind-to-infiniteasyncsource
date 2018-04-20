Imports System

Namespace InfiniteAsyncSourceSample
    Public Class IssuesSummaries
        Public Sub New(ByVal count As Integer, ByVal lastCreated? As Date)
            Me.Count = count
            Me.LastCreated = lastCreated
        End Sub

        Private privateCount As Integer
        Public Property Count() As Integer
            Get
                Return privateCount
            End Get
            Private Set(ByVal value As Integer)
                privateCount = value
            End Set
        End Property
        Private privateLastCreated? As Date
        Public Property LastCreated() As Date?
            Get
                Return privateLastCreated
            End Get
            Private Set(ByVal value? As Date)
                privateLastCreated = value
            End Set
        End Property
    End Class
End Namespace
