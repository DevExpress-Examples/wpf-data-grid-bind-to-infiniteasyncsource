Imports System

Namespace InfiniteAsyncSourceSample
	Public Class IssuesSummaries
'INSTANT VB NOTE: The variable count was renamed since Visual Basic does not handle local variables named the same as class members well:
'INSTANT VB NOTE: The variable lastCreated was renamed since Visual Basic does not handle local variables named the same as class members well:
		Public Sub New(ByVal count_Conflict As Integer, ByVal lastCreated_Conflict? As DateTime)
			Me.Count = count_Conflict
			Me.LastCreated = lastCreated_Conflict
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
		Private privateLastCreated? As DateTime
		Public Property LastCreated() As DateTime?
			Get
				Return privateLastCreated
			End Get
			Private Set(ByVal value? As DateTime)
				privateLastCreated = value
			End Set
		End Property
	End Class
End Namespace
