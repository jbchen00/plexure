line 19: should set up Dependency Injection in startup.

line 33: use await and make the method to async. otherwise use .GetAwaiter().GetResult() to get actual exception instead of aggregated exception, if need to block the asnyc code.

line 37: use var depends on programming norm at Plexure. Personally, I think it allows better readability and easier to refactor later.

line 38: depends on IAirlinePriceProvider.GetQuotes. 
Using Parallel.ForEach may not be the best option.
Does GetQuotes require long time to compute?
Is GetQuotes thread safe?
Does GetQuotes use a shared data store?
This require test and measure.

line 41: use AddRange.

line 58: could use List<Task<long>> instead of calling GetDistanceAsync and doing sum in a  for loop, which is not async.

`var tasks = new List<Task<double>>();
 for (int i = 0; i < itinerary.Waypoints.Count - 1; i++)
 {
     tasks.Add(_distanceCalculator.GetDistanceAsync(itinerary.Waypoints[i],
         itinerary.Waypoints[i + 1]));
 }
 await Task.WhenAll(tasks);
 result = tasks.Sum(t => t.Result);`
 
Another possible option is to use Parallel if GoogleMapDistanceCalculator supports it.
             
line 74: the function name is FindAgent, then this function should just do one thing, return an agent.
We should have another function for update.
Another point is that ItineraryManager should not have Agent related methods. FindAgent should be moved to another class, e.g. AgentManager.
             
line 85: AutoMapper 9.0 does not support static API anymore. could use DI to inject a mapper instance if we decide to upgrade to 9.0.


