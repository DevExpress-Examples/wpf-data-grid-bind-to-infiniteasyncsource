Imports DevExpress.Data.Filtering
Imports DevExpress.Xpf.Data
Imports System
Imports System.Globalization
Imports System.Linq
Imports System.Windows.Data
Imports System.Windows.Markup

Namespace InfiniteAsyncSourceMVVMSample

    Public Class IssueFilterConverter
        Inherits MarkupExtension
        Implements IValueConverter

        Private Function Convert(ByVal filter As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As CultureInfo) As Object Implements IValueConverter.Convert
            Return CType(filter, CriteriaOperator).Match(binary:=Function(propertyName, value, type)
                If Equals(propertyName, "Votes") AndAlso type = BinaryOperatorType.GreaterOrEqual Then Return New IssueFilter(minVotes:=CInt(value))
                If Equals(propertyName, "Priority") AndAlso type = BinaryOperatorType.Equal Then Return New IssueFilter(priority:=CType(value, Priority))
                If Equals(propertyName, "Created") Then
                    If type = BinaryOperatorType.GreaterOrEqual Then Return New IssueFilter(createdFrom:=CDate(value))
                    If type = BinaryOperatorType.Less Then Return New IssueFilter(createdTo:=CDate(value))
                End If

                Throw New InvalidOperationException()
            End Function, [and]:=Function(filters) New IssueFilter(createdFrom:=filters.[Select](Function(x) x.CreatedFrom).SingleOrDefault(Function(x) x IsNot Nothing), createdTo:=filters.[Select](Function(x) x.CreatedTo).SingleOrDefault(Function(x) x IsNot Nothing), minVotes:=filters.[Select](Function(x) x.MinVotes).SingleOrDefault(Function(x) x IsNot Nothing), priority:=filters.[Select](Function(x) x.Priority).SingleOrDefault(Function(x) x IsNot Nothing)), null:=Nothing)
        End Function

        Private Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
            Throw New NotImplementedException()
        End Function

        Public Overrides Function ProvideValue(ByVal serviceProvider As IServiceProvider) As Object
            Return Me
        End Function
    End Class
End Namespace
