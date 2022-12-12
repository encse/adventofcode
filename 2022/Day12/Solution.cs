namespace AdventOfCode.Y2022.Day12;

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;


[ProblemName("Hill Climbing Algorithm")]
class Solution : Solver {

    // I feel like a cartographer today
    record struct Coord(int lat, int lon);

    // we have two 'char' like things, let's introduce wrappers to keep them well separated in code
    record struct Symbol(char value);
    record struct Elevation(char value);

    // locations on the map will be represented by the following structure of points-of-interests.
    record struct Poi(Symbol symbol, Elevation elevation, int distanceFromGoal);
    
    Symbol startSymbol = new Symbol('S');
    Symbol goalSymbol = new Symbol('E');
    Elevation lowestElevation = new Elevation('a');
    Elevation highestElevation = new Elevation('z');

    public object PartOne(string input) =>
        GetPois(input)
            .Single(poi => poi.symbol == startSymbol)
            .distanceFromGoal;

    public object PartTwo(string input) =>
        GetPois(input)
            .Where(poi => poi.elevation == lowestElevation)
            .Select(poi => poi.distanceFromGoal)
            .Min();

    IEnumerable<Poi> GetPois(string input) {
        var map = ParseMap(input);
        var goal = map.Keys.Single(point => map[point] == goalSymbol);

        // starting from the goal symbol compute shortest paths for each point of 
        // the map using a breadth-first search.
        var poiByCoord = new Dictionary<Coord, Poi>() {
            {goal, new Poi(goalSymbol, GetElevation(goalSymbol), 0)}
        };

        var q = new Queue<Coord>();
        q.Enqueue(goal);
        while (q.Any()) {
            var pt = q.Dequeue();
            var distance = poiByCoord[pt].distanceFromGoal;

            foreach (var ptNext in Neighbours(pt).Where(map.ContainsKey)) {
                var symbolNext = map[ptNext];
                var elevationNext = GetElevation(symbolNext);

                if (!poiByCoord.ContainsKey(ptNext) && poiByCoord[pt].elevation.value - elevationNext.value <= 1) {
                    poiByCoord[ptNext] = new Poi(
                        symbol: symbolNext, 
                        elevation: elevationNext, 
                        distanceFromGoal: distance + 1
                    );
                    q.Enqueue(ptNext);
                }
            }
        }
        return poiByCoord.Values;
    }

    Elevation GetElevation(Symbol symbol) => 
        symbol.value switch {
            'S' => lowestElevation,
            'E' => highestElevation,
            _ => new Elevation(symbol.value )
        };
   
    // locations are parsed into a dictionary so that valid coordinates and
    // neighbours are easy to deal with
    ImmutableDictionary<Coord, Symbol> ParseMap(string input){
        var lines = input.Split("\n");
        return (
            from y in Enumerable.Range(0, lines.Length)
            from x in Enumerable.Range(0, lines[0].Length)
            select new KeyValuePair<Coord, Symbol>(
                new Coord(x, y), new Symbol(lines[y][x])
            )
        ).ToImmutableDictionary();
    } 

    IEnumerable<Coord> Neighbours(Coord coord) =>
        new[] {
           coord with {lat = coord.lat + 1},
           coord with {lat = coord.lat - 1},
           coord with {lon = coord.lon + 1},
           coord with {lon = coord.lon - 1},
        };
}
