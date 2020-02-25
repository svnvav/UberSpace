namespace Svnvav.UberSpace
{
    public class Order
    {
        private Race _client;
        private Planet _departure, _destination;
        private OrderStatus _status;

        public Race Client => _client;
        public Planet Departure => _departure;
        public Planet Destination => _destination;
        public OrderStatus Status => _status;
        
        public Order Prev { get; set; }
        public Order Next { get; set; }

        public Order()
        {
            _status = OrderStatus.Completed;
        }

        public void Init(Race race, Planet departure, Planet destination)
        {
            _client = race;
            _departure = departure;
            _destination = destination;
            
            departure.AddRaceToDeparture(race);
            destination.AddRaceToArrive(race);
            
            _status = OrderStatus.Queued;
        }

        public void Complete()
        {
            _destination.AddRace(_client);
            _client = null;
            _departure = null;
            _destination = null;

            _status = OrderStatus.Completed;
        }

        public void Cancel()
        {
            switch (_status)
            {
                //TODO:
            }
            _client = null;
            _departure = null;
            _destination = null;

            _status = OrderStatus.Completed;
        }
    }
}