using Godot;

namespace Game.Resources
{
    [GlobalClass]
    public partial class AmmoResource : Resource
    {
        [Export]
        public Texture2D ammoSprite { get; private set; }
    }

}