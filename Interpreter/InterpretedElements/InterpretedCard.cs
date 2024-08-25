namespace Interpreter
{
     public partial class InterpretedCard : InterpretedElement
    {
        public string Name { get; }
        public string Type { get; }
        public string Faction { get; }
        public string Range { get; }
        public int Power { get; }
        
        public List<OnActivationObject> OnActivation {get;} 
        
        public InterpretedCard(string name, string type, string faction, string range, int power,List<OnActivationObject> onActivations)
        {
            this.Name = name;
            this.Type = type;
            this.Faction = faction;
            this.Range = range;
            this.Power = power;
            this.OnActivation=onActivations;
        }
    }
}