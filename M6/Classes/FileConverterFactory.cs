using System;

namespace M6.Classes
{
    public class FileConverterFactory : IFileConverterFactory
    {
        public IFileConverter GetBuilderFor(string path)
        {
            return path.EndsWith(".raw", StringComparison.InvariantCultureIgnoreCase) ? new RawPCMFileConverter(path, new FileSystemHelper()) : null;
        }
    }
}
