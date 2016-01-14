using System;

namespace M6.Classes
{
    public class BuilderFactory : IBuilderFactory
    {
        public IFrameDataBuilder GetBuilderFor(string path)
        {
            return path.EndsWith(".raw", StringComparison.InvariantCultureIgnoreCase) ? new FrameDataBuilder(path, new FileSystemHelper()) : null;
        }
    }
}
