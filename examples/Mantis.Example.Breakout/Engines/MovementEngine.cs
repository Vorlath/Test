﻿using Mantis.Example.Breakout.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Svelto.ECS;

namespace Mantis.Example.Breakout.Engines
{
    public class MovementEngine : IFrameEngine, IQueryingEntitiesEngine
    {

        public EntitiesDB entitiesDB { get; set; }
        GraphicsDevice graphics;
        public MovementEngine(GraphicsDevice graphics)
        {
            this.graphics = graphics;
        }

        public void Ready()
        {
            //    throw new NotImplementedException();
        }

        public void Draw(GameTime gameTime)
        {

        }

        public void Update(GameTime gameTime)
        {
            var groups = this.entitiesDB.FindGroups<Velocity, Position, Size>();
            foreach (var ((velocities, positions, sizes, count), _) in entitiesDB.QueryEntities<Velocity, Position, Size>(groups))
            {
                for (int i = 0; i < count; i++)
                {
                    ref Velocity velocity = ref velocities[i];
                    ref Position position = ref positions[i];
                    ref Size size = ref sizes[i];


                    this.Update(ref velocity, ref position, ref size, gameTime);
                }
            }
        }

        private void Update(ref Velocity velocity, ref Position position, ref Size size, GameTime gameTime)
        {

            position.Value += (velocity.Value * (float)gameTime.ElapsedGameTime.TotalSeconds);

            // Check for walls
            if (position.Value.X + size.Value.X > this.graphics.Viewport.Width)
            {
                position.Value.X = this.graphics.Viewport.Width - size.Value.X;
                velocity.Value.X *= -1;
            }
            if (position.Value.X < 0)
            {
                position.Value.X = 0;
                velocity.Value.X *= -1;
            }
            if (position.Value.Y + size.Value.Y > this.graphics.Viewport.Height)
            {
                position.Value.Y = this.graphics.Viewport.Height - size.Value.Y;
                velocity.Value.Y *= -1;
            }
            if (position.Value.Y < 0)
            {
                position.Value.Y = 0;
                velocity.Value.Y *= -1;
            }
        }
    }
}