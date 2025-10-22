using System;
using System.Collections.Generic;
using System.Numerics;
using Infrastructure;
using Logs;

namespace Game.Maze
{
    public static class MazeGenerator
    {
        // Cell codes: 0 = wall, 1 = path, 2 = exit (border opening)
        public static int[,] Generate(MazeData mazeData)
        {
            // Validate and normalize dimensions
            if (mazeData.mazeWidth < 3) mazeData.mazeWidth = 3;
            if (mazeData.mazeHeight < 3) mazeData.mazeHeight = 3;

            // Ensure odd dimensions to keep proper walls between cells
            if (mazeData.mazeWidth % 2 == 0) mazeData.mazeWidth++;
            if (mazeData.mazeHeight % 2 == 0) mazeData.mazeHeight++;

            var grid = new int[mazeData.mazeWidth, mazeData.mazeHeight]; // initialized to 0 (walls)
            var rand = new Random(unchecked((int)DateTime.UtcNow.Ticks));

            // Directions in 2-step for carving (dx, dy) and also keep inter-wall offsets
            var directions = new (int dx, int dy)[] { (2, 0), (-2, 0), (0, 2), (0, -2) };

            // Pick a random odd starting cell
            int startX = 1 + 2 * rand.Next((mazeData.mazeWidth - 1) / 2);
            int startY = 1 + 2 * rand.Next((mazeData.mazeHeight - 1) / 2);

            // Iterative DFS using stack
            var stack = new Stack<(int x, int y)>();
            grid[startX, startY] = 1; // mark path
            stack.Push((startX, startY));

            while (stack.Count > 0)
            {
                var (cx, cy) = stack.Peek();

                // Collect unvisited neighbors 2 cells away
                var neighbors = new List<(int nx, int ny, int wx, int wy)>();
                foreach (var (dx, dy) in directions)
                {
                    int nx = cx + dx;
                    int ny = cy + dy;
                    int wx = cx + dx / 2; // wall cell between current and neighbor
                    int wy = cy + dy / 2;

                    if (nx > 0 && nx < mazeData.mazeWidth && ny > 0 && ny < mazeData.mazeHeight)
                    {
                        if (IsInside(nx, ny, mazeData.mazeWidth, mazeData.mazeHeight) && grid[nx, ny] == 0)
                            neighbors.Add((nx, ny, wx, wy));
                    }
                }

                if (neighbors.Count == 0)
                {
                    stack.Pop();
                    continue;
                }

                // Choose a random neighbor, carve passage
                var choice = neighbors[rand.Next(neighbors.Count)];
                grid[choice.wx, choice.wy] = 1; // carve the wall between
                grid[choice.nx, choice.ny] = 1; // carve the next cell
                stack.Push((choice.nx, choice.ny));
            }

            // Create exits on the border
            CreateExits(grid, mazeData.mazeWidth, mazeData.mazeHeight, Math.Max(0, mazeData.exitCount), rand);

            return grid;
        }

        private static bool IsInside(int x, int y, int width, int height)
        {
            // Inside means not on the outer border
            return x > 0 && x < width - 1 && y > 0 && y < height - 1;
        }

        private static void CreateExits(int[,] grid, int width, int height, int requested, Random rand)
        {
            // Gather all border positions that can be opened (adjacent to an internal path)
            var candidates = new List<(int x, int y)>();

            // Top and bottom rows
            for (int x = 1; x < width - 1; x++)
            {
                if (grid[x, 1] == 1) candidates.Add((x, 0)); // top border opening
                if (grid[x, height - 2] == 1) candidates.Add((x, height - 1)); // bottom border opening
            }

            // Left and right columns
            for (int y = 1; y < height - 1; y++)
            {
                if (grid[1, y] == 1) candidates.Add((0, y)); // left border opening
                if (grid[width - 2, y] == 1) candidates.Add((width - 1, y)); // right border opening
            }

            // Shuffle candidates
            for (var i = candidates.Count - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);
                (candidates[i], candidates[j]) = (candidates[j], candidates[i]);
            }

            var toOpen = Math.Min(requested, candidates.Count);
            for (var i = 0; i < toOpen; i++)
            {
                var (x, y) = candidates[i];
                grid[x, y] = 2; // mark as exit
            }

            // Ensure at least one exit if requested but none possible: try force-open near a path
            if (requested <= 0 || toOpen != 0) 
                return;
            
            // Search first path on inner ring and open the nearest border cell
            for (int x = 1; x < width - 1; x++)
            {
                if (grid[x, 1] == 1) { grid[x, 0] = 2; return; }
                if (grid[x, height - 2] == 1) { grid[x, height - 1] = 2; return; }
            }
            for (int y = 1; y < height - 1; y++)
            {
                if (grid[1, y] == 1) { grid[0, y] = 2; return; }
                if (grid[width - 2, y] == 1) { grid[width - 1, y] = 2; return; }
            }
            
        }
        
        public static Vector2 FindNearestToCenter(int[,] grid, int targetValue = 1)
        {
            if (grid == null)
            {
                Context.GetSystem<ILog>().Error(() => "Grid is null");
                return Vector2.Zero;
            }
            
            int width = grid.GetLength(0);
            int height = grid.GetLength(1);
            if (width <= 0 || height <= 0) return Vector2.Zero;

            // Geometric center (can be .5 if even dimensions)
            double cx = (width - 1) / 2.0;
            double cy = (height - 1) / 2.0;

            double bestDist = double.MaxValue;
            int bestX = -1, bestY = -1;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (grid[x, y] != targetValue) continue;
                    double dx = x - cx;
                    double dy = y - cy;
                    double dist = dx * dx + dy * dy; // squared distance is enough for comparison
                    if (dist < bestDist)
                    {
                        bestDist = dist;
                        bestX = x;
                        bestY = y;
                    }
                }
            }

            return new Vector2(bestX, bestY);
        }
    }
}