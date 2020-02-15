namespace SwaggerFun
{
    internal class EnumMember
    {
        public EnumMember(string name, object value, bool isObsolete, string obsoleteMessage)
        {
            this.Name = name;
            this.Value = value;
            this.IsObsolete = isObsolete;
            this.ObsoleteMessage = obsoleteMessage;
        }

        public string Name { get; }

        public object Value { get; }

        public bool IsObsolete { get; }

        public string ObsoleteMessage { get; }
    }
}
