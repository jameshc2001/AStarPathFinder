using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinder
{
    class AStar
    {
        public Grid grid;

        public AStar(Grid _grid)
        {
            grid = _grid;
        }

        public void FindPath(Node start, Node destination)
        {
            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(start);

            while (openSet.Count > 0)
            {
                Node node = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost)
                    {
                        if (openSet[i].hCost < node.hCost) node = openSet[i];
                    }
                }

                openSet.Remove(node);
                closedSet.Add(node);

                if (node == destination)
                {
                    RetracePath(start, destination);
                }

                foreach (Node n in grid.GetNeighbours(node))
                {
                    if (!n.walkable || closedSet.Contains(n)) continue;
                    if (Diagonal(node, n)) continue;

                    int newCostToNeighbour = node.gCost + GetDistance(node, n);
                    if (newCostToNeighbour < n.gCost || !openSet.Contains(n))
                    {
                        n.gCost = newCostToNeighbour;
                        n.hCost = GetDistance(n, destination);
                        n.parent = node;

                        if (!openSet.Contains(n))
                        {
                            openSet.Add(n);
                        }
                    }
                }
            }
        }

        bool Diagonal(Node start, Node destination)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 || y == 0) continue;
                    int checkX = start.gridX + x;
                    int checkY = start.gridY + y;
                    if (checkX >= 0 && checkX < 25 && checkY >= 0 && checkY < 15)
                    {
                        if (!grid.grid[checkX, start.gridY].walkable && !grid.grid[start.gridX, checkY].walkable && destination.gridX == checkX && destination.gridY == checkY) return true;
                    }
                }
            }
            return false;
        }

        void RetracePath(Node start, Node destination)
        {
            List<Node> path = new List<Node>();
            Node currentNode = destination;
            while(currentNode != start)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }
            path.Reverse();
            grid.path = path;
        }

        int GetDistance(Node nodeA, Node nodeB)
        {
            int dstX = Math.Abs(nodeA.gridX - nodeB.gridX);
            int dstY = Math.Abs(nodeA.gridY - nodeB.gridY);
            if (dstX > dstY) return 14 * dstY + 10 * (dstX - dstY);
            return 14 * dstX + 10 * (dstY - dstX);
        }
    }
}
