using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Pathfinder
{
    class Grid
    {
        public Node[,] grid = new Node[25, 15];
        public AStar aStar;
        public List<Node> path;
        public MouseState mouseState, prevMouseState;
        public KeyboardState kState, prevKState;
        public bool showPath = true;

        public Grid()
        {
            for (int x = 0; x < 25; x++)
            {
                for (int y = 0; y < 15; y++)
                {
                    grid[x, y] = new Node(true, new Vector2(x * 32, y * 32), x, y);
                }
            }
            aStar = new AStar(this);
        }

        public List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue;
                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;
                    if (checkX >= 0 && checkX < 25 && checkY >= 0 && checkY < 15)
                    {
                        neighbours.Add(grid[checkX, checkY]);
                    }
                }
            }
            return neighbours;
        }

        public void LoadContent(GraphicsDeviceManager graphics)
        {
            foreach (Node n in grid) n.LoadContent(graphics);
        }

        public void Update()
        {
            prevMouseState = mouseState;
            mouseState = Mouse.GetState();
            prevKState = kState;
            kState = Keyboard.GetState();

            //left click toggles walkable
            if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
            {
                Point mousePos = new Point(mouseState.Position.X, mouseState.Position.Y);
                foreach (Node n in grid)
                {
                    if (n.rectangle.Contains(mousePos)) n.walkable = !n.walkable;
                }
            }

            //right click removes old destination and sets new one
            if (mouseState.RightButton == ButtonState.Pressed && prevMouseState.RightButton == ButtonState.Released)
            {
                Point mousePos = new Point(mouseState.Position.X, mouseState.Position.Y);
                foreach (Node n in grid)
                {
                    if (n.rectangle.Contains(mousePos) && n.destination) continue;
                    if (n.destination) n.destination = false;
                }
                foreach (Node n in grid)
                {
                    if (n.rectangle.Contains(mousePos))
                    {
                        n.start = false;
                        n.destination = !n.destination;
                    }
                }
            }

            //middle click removes old start and sets new one
            if (mouseState.MiddleButton == ButtonState.Pressed && prevMouseState.MiddleButton == ButtonState.Released)
            {
                Point mousePos = new Point(mouseState.Position.X, mouseState.Position.Y);
                foreach (Node n in grid)
                {
                    if (n.rectangle.Contains(mousePos) && n.start) continue;
                    if (n.start) n.start = false;
                }
                foreach (Node n in grid)
                {
                    if (n.rectangle.Contains(mousePos))
                    {
                        n.destination = false;
                        n.start = !n.start;
                    }
                }
            }

            //fix walkable
            foreach (Node n in grid) n.Update();

            //decide whether to pathfind or not
            foreach(Node n in grid)
            {
                if (n.destination)
                {
                    foreach (Node m in grid)
                    {
                        if (m.start)
                        {
                            aStar.FindPath(m, n);
                        }
                    }
                }
            }
            
            //toggle path view
            if (kState.IsKeyDown(Keys.R) && prevKState.IsKeyUp(Keys.R))
            {
                showPath = !showPath;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(Node n in grid)
            {
                if (n.walkable) n.Draw(spriteBatch, Color.White);
                if (!n.walkable) n.Draw(spriteBatch, Color.Red);
                if (n.destination) n.Draw(spriteBatch, Color.LawnGreen);
                if (n.start) n.Draw(spriteBatch, Color.Cyan);
                if (path != null && showPath)
                {
                    if (path.Contains(n) && !n.destination && !n.start)
                    {
                        n.Draw(spriteBatch, Color.Black);
                    }
                }
            }
        }
    }
}
