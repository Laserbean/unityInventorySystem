


namespace unityInventorySystem.Attribute {

    public interface IAttributeUsage {

        public int GetAttributeValue(AttributeType type);
        public int GetAttributeBaseValue(AttributeType type);
        public AttributeChange OnAttributeChange {get; set;}

    }

    public delegate void AttributeChange(Attribute attribute);

}
