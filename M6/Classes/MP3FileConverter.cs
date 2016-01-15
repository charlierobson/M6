using System.Runtime.InteropServices;

namespace M6.Classes
{
    public class MP3FileConverter : IFileConverter
    {
        [DllImport("libmad.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern int ToRaw(string input, string output);

        private readonly string _path;
        private readonly IFileSystemHelper _fileSystemHelper;

        public MP3FileConverter(string path, IFileSystemHelper fileSystemHelper)
        {
            _path = path;
            _fileSystemHelper = fileSystemHelper;
        }

        public IFrameData ProcessFile()
        {
            var error = ToRaw(_path, @"c:\temp.raw");

            return error != 0 ? null : new RawPCMFileConverter(@"c:\temp.raw", _fileSystemHelper).ProcessFile();
        }
    }
}
