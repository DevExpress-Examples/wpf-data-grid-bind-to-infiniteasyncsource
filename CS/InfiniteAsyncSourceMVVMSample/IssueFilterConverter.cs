using DevExpress.Data.Filtering;
using DevExpress.Xpf.Data;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;

namespace InfiniteAsyncSourceMVVMSample {
    public class IssueFilterConverter : MarkupExtension, IValueConverter {
        object IValueConverter.Convert(object filter, Type targetType, object parameter, CultureInfo culture) {
            return ((CriteriaOperator)filter).Match(
                binary: (propertyName, value, type) => {
                    if(propertyName == "Votes" && type == BinaryOperatorType.GreaterOrEqual)
                        return new IssueFilter(minVotes: (int)value);

                    if(propertyName == "Priority" && type == BinaryOperatorType.Equal)
                        return new IssueFilter(priority: (Priority)value);

                    if(propertyName == "Created") {
                        if(type == BinaryOperatorType.GreaterOrEqual)
                            return new IssueFilter(createdFrom: (DateTime)value);
                        if(type == BinaryOperatorType.Less)
                            return new IssueFilter(createdTo: (DateTime)value);
                    }

                    throw new InvalidOperationException();
                },
                and: filters => {
                    return new IssueFilter(
                        createdFrom: filters.Select(x => x.CreatedFrom).SingleOrDefault(x => x != null),
                        createdTo: filters.Select(x => x.CreatedTo).SingleOrDefault(x => x != null),
                        minVotes: filters.Select(x => x.MinVotes).SingleOrDefault(x => x != null),
                        priority: filters.Select(x => x.Priority).SingleOrDefault(x => x != null)
                    );
                },
                @null: default(IssueFilter)
            );
        }
        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
