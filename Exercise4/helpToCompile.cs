using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Exercise4
{
    
    internal class SqlAgentStore : IDataStore
    {
        public SqlAgentStore(string para)
        {
        }

        public Task<Itinerary> GetItinaryAsync(int para)
        {
            throw new NotImplementedException();
        }

        public void UpdateAgent(int id, Agent agentDao)
        {
            throw new NotImplementedException();
        }

        public Agent GetAgent(int id)
        {
            throw new NotImplementedException();
        }
    }

    internal class GoogleMapsDistanceCalculator : IDistanceCalculator
    {
        public GoogleMapsDistanceCalculator(string para)
        {
        }

        public Task<double> GetDistanceAsync(double p, double p2)
        {
            throw new NotImplementedException();
        }
    }

    internal interface IDistanceCalculator
    {
        Task<double> GetDistanceAsync(double p, double p2);
    }

    internal interface IDataStore
    {
        Task<Itinerary> GetItinaryAsync(int para);
        void UpdateAgent(int id, Agent agentDao);
        Agent GetAgent(int id);
    }

    internal class Agent
    {
        public string PhoneNumber { get; set; }
    }

    internal class Itinerary
    {
        public List<long> Waypoints { get; set; }
        public string TicketClass { get; set; }
    }

    public class Quote
    {
    }

    public interface IAirlinePriceProvider
    {
        IEnumerable<Quote> GetQuotes(string TicketClass, List<long> Waypoints);
    }

    public class TravelAgent
    {
    }
}