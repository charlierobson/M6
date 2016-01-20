using System;

namespace M6.Classes
{
    public class RawPCMFileConverter : IFileConverter
    {
        private readonly string _path;
        private readonly IFileSystemHelper _fileSystemHelper;

        public RawPCMFileConverter(string path, IFileSystemHelper fileSystemHelper)
        {
            _path = path;
            _fileSystemHelper = fileSystemHelper;
        }

        public IFrameData ProcessFile()
        {
            var bytes = _fileSystemHelper.ReadAllBytes(_path);
            var frames = bytes.Length / 4;

            var left = new float[frames];
            var right = new float[frames];

            var j = 0;
            for (var i = 0; i < bytes.Length; i += 4)
            {
                var l = BitConverter.ToInt16(bytes, i);
                left[j] = l / 32768f;

                var r = BitConverter.ToInt16(bytes, i + 2);
                right[j] = r / 32768f;

                ++j;
            }

            return new FrameData(left, right);
        }
    }
}
