namespace Svnvav.UberSpace
{
    public class Order
    {
        private Race _race;
        private Planet _departure, _destination;

        public Race Race => _race;
        public Planet Departure => _departure;
        public Planet Destination => _destination;

        public Order(Race race, Planet departure, Planet destination)
        {
            _race = race;
            _departure = departure;
            _destination = destination;
        }
    }
}