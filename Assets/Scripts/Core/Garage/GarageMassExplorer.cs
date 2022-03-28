using Core.Game;

namespace Core.Garage
{
    public class GarageMassExplorer : GarageSlotDataExplorer
    {
        public override void AddInFor(Item item, ref AddInForUsed val)
        {
            base.AddInFor(item, ref val);
            val.all = GarageDataCollect.Instance.ship.data.maxCargoWeight;
            val.used += (float)item.GetKeyPair(KeyPairValue.Mass);
        }
    }
}
