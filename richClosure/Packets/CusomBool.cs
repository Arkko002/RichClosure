using System.Runtime.CompilerServices;

namespace richClosure.Packets
{
    public class CustomBool
    {
        public string Name { get; }
        public bool IsSet { get; set; }

        public CustomBool([CallerMemberName] string name = null)
        {
            Name = name;
        }

        public override string ToString()
        {
            if (IsSet)
            {
                return Name;
            }
            else
            {
                return "False";
            }
        }
    }
}
