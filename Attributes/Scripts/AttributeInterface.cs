


namespace unityInventorySystem.Attribute {

    public interface IAttributeUsage {

        public int GetAttributeValue(AttributeType type);
        public int GetAttributeBaseValue(AttributeType type);
        public AttributeChange OnAttributeChange {get; set;}

    }

    public interface IAttributeController {

        public void AddAttributeModifier(AttributeType type, IModifier modifier);
        public void RemoveAttributeModifier(AttributeType type, IModifier modifier);

    }




    public delegate void AttributeChange(Attribute attribute);

}
