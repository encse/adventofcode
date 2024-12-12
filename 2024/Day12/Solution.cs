namespace AdventOfCode.Y2024.Day12;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;

using Map = System.Collections.Immutable.ImmutableDictionary<System.Numerics.Complex, char>;
using Component = System.Collections.Generic.HashSet<System.Numerics.Complex>;

[ProblemName("Garden Groups")]
class Solution : Solver {

    Complex Up = Complex.ImaginaryOne;
    Complex Down = -Complex.ImaginaryOne;
    Complex Left = -1;
    Complex Right = 1;

    public object PartOne(string input) {
         var map = GetMap(input);
        var points = map.Keys.ToHashSet();

        var componentMap = new Dictionary<Complex, Component>();
        var components = new List<Component>();

        while (points.Any()) {
            var component = Floodfill(map, points.First()).ToHashSet();
            points.ExceptWith(component);

            components.Add(component);
            foreach (var pt in component) {
                componentMap[pt] = component;
            }
        }

        var borders = new Dictionary<Component, int>();
        CollectBorders(componentMap, borders, Right, Down, Up, false);
        CollectBorders(componentMap, borders, Right, Down, Down, false);
        CollectBorders(componentMap, borders, Down, Right, Left, false);
        CollectBorders(componentMap, borders, Down, Right, Right, false);

        var res = 0;
        foreach (var component in components) {
            var area = component.Count;
            var perimeter = borders[component];
            res += area * perimeter;
        }
        return res;
    }

    public object PartTwo(string input) {
        var map = GetMap(input);
        var points = map.Keys.ToHashSet();

        var componentMap = new Dictionary<Complex, Component>();
        var components = new List<Component>();

        while (points.Any()) {
            var component = Floodfill(map, points.First()).ToHashSet();
            points.ExceptWith(component);

            components.Add(component);
            foreach (var pt in component) {
                componentMap[pt] = component;
            }
        }

        var borders = new Dictionary<Component, int>();
        CollectBorders(componentMap, borders, Right, Down, Up, true);
        CollectBorders(componentMap, borders, Right, Down, Down, true);
        CollectBorders(componentMap, borders, Down, Right, Left, true);
        CollectBorders(componentMap, borders, Down, Right, Right, true);

        var res = 0;
        foreach (var component in components) {
            var area = component.Count;
            var perimeter = borders[component];
            res += area * perimeter;
        }
        return res;
    }

    void CollectBorders(Dictionary<Complex, Component> componentMap, Dictionary<Component, int> borders, Complex du, Complex dv, Complex look, bool b) {
        var start = Complex.Zero;
        var pt = start;
        while (componentMap.ContainsKey(pt)) {
            var found = false;
            while (componentMap.ContainsKey(pt)) {
                if (componentMap[pt] != componentMap.GetValueOrDefault(pt + look)) {
                    if (!found) {
                        borders[componentMap[pt]] = borders.GetValueOrDefault(componentMap[pt]) + 1;
                        found = b & true;
                    }
                } else {
                    found = false;
                }

                if (componentMap[pt] != componentMap.GetValueOrDefault(pt + du)) {
                    found = false;
                }

                pt += du;
            }
            start += dv;
            pt = start;
        }
    }

    Component Floodfill(Map map, Complex start) {
        // standard floodfill algorithm using a queue
        var positions = new Queue<Complex>();
        positions.Enqueue(start);
        var component = new Component();
        var plant = map[start];
        while (positions.Any()) {
            var point = positions.Dequeue();
            component.Add(point);
            foreach (var dir in new[] { Up, Down, Left, Right }) {
                if (!component.Contains(point + dir) && map.GetValueOrDefault(point + dir) == plant) {
                    positions.Enqueue(point + dir);
                }
            }
        }
        return component;
    }

    // store the points in a dictionary so that we can iterate over them and 
    // to easily deal with points outside the area using GetValueOrDefault
    Map GetMap(string input) {
        var map = input.Split("\n");
        return (
            from y in Enumerable.Range(0, map.Length)
            from x in Enumerable.Range(0, map[0].Length)
            select new KeyValuePair<Complex, char>(x + y * Down, map[y][x])
        ).ToImmutableDictionary();
    }
}