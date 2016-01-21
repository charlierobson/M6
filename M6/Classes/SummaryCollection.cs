using ProtoBuf;

namespace M6.Classes
{
    [ProtoContract]
    public class SummaryCollection
    {
        [ProtoMember(1, OverwriteList = true)]
        public readonly FrameData[] Summary;

        public SummaryCollection()
        {
            Summary = new FrameData[3];
        }
    }
}