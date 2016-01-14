namespace M6.Classes
{
    public class ClipFactory : IClipFactory
    {
        private readonly IFileConverterFactory _fileConverterFactory;

        public ClipFactory(IFileConverterFactory fileConverterFactory)
        {
            _fileConverterFactory = fileConverterFactory;
        }

        public IClip GetClip(string path)
        {
            var builder = _fileConverterFactory.GetBuilderFor(path);

            if (builder == null) return null;

            var frames = builder.ProcessFile();

            return frames == null ? null : new Clip(frames);
        }
    }
}