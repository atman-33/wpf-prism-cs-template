namespace Template2.Domain.ValueObjects
{
    public sealed class ImageFolderLink : ValueObject<ImageFolderLink>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name=""value""></param>
        public ImageFolderLink(string? value)
        {
            Value = value;
        }

        public string? Value { get; }

        protected override bool EqualsCore(ImageFolderLink other)
        {
            return Value == other.Value;
        }

        protected override int GetHashCodeCore()
        {
            if (Value == null)
            {
                return 0;
            }

            return Value.GetHashCode();
        }

        public override string ToString()
        {
            if (Value == null)
            {
                return string.Empty;
            }

            return Value.ToString();
        }
    }
}
