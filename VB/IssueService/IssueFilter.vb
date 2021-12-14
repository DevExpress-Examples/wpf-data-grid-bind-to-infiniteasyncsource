Imports System

Namespace InfiniteAsyncSourceSample

    Public Class IssueFilter

        Private _Priority As InfiniteAsyncSourceSample.Priority?, _CreatedFrom As System.DateTime?, _CreatedTo As System.DateTime?, _MinVotes As Integer?

        Public Sub New(ByVal Optional priority As InfiniteAsyncSourceSample.Priority? = Nothing, ByVal Optional createdFrom As System.DateTime? = Nothing, ByVal Optional createdTo As System.DateTime? = Nothing, ByVal Optional minVotes As Integer? = Nothing)
            Me.Priority = priority
            Me.CreatedFrom = createdFrom
            Me.CreatedTo = createdTo
            Me.MinVotes = minVotes
        End Sub

        Public Property Priority As InfiniteAsyncSourceSample.Priority?
            Get
                Return _Priority
            End Get

            Private Set(ByVal value As InfiniteAsyncSourceSample.Priority?)
                _Priority = value
            End Set
        End Property

        Public Property CreatedFrom As System.DateTime?
            Get
                Return _CreatedFrom
            End Get

            Private Set(ByVal value As System.DateTime?)
                _CreatedFrom = value
            End Set
        End Property

        Public Property CreatedTo As System.DateTime?
            Get
                Return _CreatedTo
            End Get

            Private Set(ByVal value As System.DateTime?)
                _CreatedTo = value
            End Set
        End Property

        Public Property MinVotes As Integer?
            Get
                Return _MinVotes
            End Get

            Private Set(ByVal value As Integer?)
                _MinVotes = value
            End Set
        End Property
    End Class
End Namespace
