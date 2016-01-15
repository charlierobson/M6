using System;

namespace M6.Classes
{
    public class FileConverterFactory : IFileConverterFactory
    {
        private readonly IFileSystemHelper _fileSystemHelper;

        public FileConverterFactory(IFileSystemHelper fileSystemHelper)
        {
            _fileSystemHelper = fileSystemHelper;
        }

        public IFileConverter ParseFile(string path)
        {
            if (!_fileSystemHelper.FileExists(path)) return null;

            if (path.EndsWith(".raw", StringComparison.InvariantCultureIgnoreCase))
                return new RawPCMFileConverter(path, _fileSystemHelper);

            else if (path.EndsWith(".mp3", StringComparison.InvariantCultureIgnoreCase))
                return new MP3FileConverter(path, _fileSystemHelper);

            else
                return null;
        }
    }
}
