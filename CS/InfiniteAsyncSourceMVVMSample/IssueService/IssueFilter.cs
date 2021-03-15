using System;

namespace InfiniteAsyncSourceMVVMSample {
    public class IssueFilter {
        public IssueFilter(Priority? priority = null, DateTime? createdFrom = null, DateTime? createdTo = null, int? minVotes = null) {
            Priority = priority;
            CreatedFrom = createdFrom;
            CreatedTo = createdTo;
            MinVotes = minVotes;
        }
        public Priority? Priority { get; private set; }
        public DateTime? CreatedFrom { get; private set; }
        public DateTime? CreatedTo { get; private set; }
        public int? MinVotes { get; private set; }
    }
}
