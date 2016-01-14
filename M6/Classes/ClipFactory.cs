namespace M6.Classes
{
    public class ClipFactory : IClipFactory
    {
        private readonly IBuilderFactory _builderFactory;

        public ClipFactory(IBuilderFactory builderFactory)
        {
            _builderFactory = builderFactory;
        }

        public IClip GetClip(string path)
        {
            var builder = _builderFactory.GetBuilderFor(path);

            if (builder == null) return null;

            var frames = builder.Build();

            return frames == null ? null : new Clip(frames);
        }
    }
}