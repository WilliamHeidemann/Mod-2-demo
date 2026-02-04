using Model;
using UtilityToolkit.Runtime;

namespace Services
{
    public class SelectionService
    {
        public Option<SegmentConfig> Segment { get; private set; } = Option<SegmentConfig>.None;
        public void Set(SegmentConfig segmentConfig) => Segment = Option<SegmentConfig>.Some(segmentConfig);
        public void Unset() => Segment = Option<SegmentConfig>.None;
    }
}