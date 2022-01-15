using System;
using System.Collections.Generic;
using System.Text;

namespace game
{
    public class Shield:Entity
    {
        public bool isOn = false;
        public Shield(string path, Player player):base(path)
        {
            coordinates = player.coordinates;
        }
        public void Enable()
        {
            isOn = true;
        }
        public void Disable()
        {
            isOn = false;
        }
        public void MoveWithPlayer(Player player)
        {
            coordinates = player.coordinates;
        }
    }
}
