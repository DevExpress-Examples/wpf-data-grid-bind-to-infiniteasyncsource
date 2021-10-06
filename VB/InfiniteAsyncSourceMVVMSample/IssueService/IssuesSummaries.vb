Imports System

Namespace InfiniteAsyncSourceMVVMSample

    Public Class IssuesSummaries

        Private _Count As Integer, _LastCreated As System.DateTime?

        Public Sub New(ByVal count As Integer, ByVal lastCreated As System.DateTime?)
            Me.Count = count
            Me.LastCreated = lastCreated
        End Sub

        Public Property Count As Integer
            Get
                Return _Count
            End Get

            Private Set(ByVal value As Integer)
                _Count = value
            End Set
        End Property

        Public Property LastCreated As System.DateTime?
            Get
                Return _LastCreated
            End Get

            Private Set(ByVal value As System.DateTime?)
                _LastCreated = value
            End Set
        End Property
    End Class
End Namespace
