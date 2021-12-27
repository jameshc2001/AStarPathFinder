using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace Pathfinder
{
    class Node
    {
        public bool walkable;
        public bool destination = false;
        public bool start = false;
        public Vector2 worldPosition;
        public int gridX;
        public int gridY;
        public Rectangle rectangle;
        public Texture2D rect;

        public int gCost;
        public int hCost;
        public Node parent;

        public Node(bool _walkable, Vector2 _worldPosition, int _gridX, int _gridY)
        {
            walkable = _walkable;
            worldPosition = _worldPosition;
            gridX = _gridX;
            gridY = _gridY;
            rectangle = new Rectangle((int)worldPosition.X, (int)worldPosition.Y, 32, 32);
        }

        public int fCost
        {
            get
            {
                return hCost + gCost;
            }
        }

        public void LoadContent(GraphicsDeviceManager graphics)
        {
            rect = new Texture2D(graphics.GraphicsDevice, 30, 30);
        }

        public void Update()
        {
            if (destination || start)
            {
                walkable = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            Color[] data = new Color[30 * 30];
            for (int i = 0; i < data.Length; i++) data[i] = color;
            rect.SetData(data);
            Vector2 pos = new Vector2(worldPosition.X + 1, worldPosition.Y + 1);
            spriteBatch.Begin();
            spriteBatch.Draw(rect, pos, Color.White);
            spriteBatch.End();
        }
    }
}
