﻿using Mantis.Example.Breakout.Components;
using Microsoft.Xna.Framework;
using Svelto.ECS;

namespace Mantis.Example.Breakout.Descriptors
{
    public class BlockDescriptor : IEntityDescriptor
    {
        public IComponentBuilder[] componentsToBuild => _componentsToBuild;

        private static readonly IComponentBuilder[] _componentsToBuild =
        [
            new ComponentBuilder<Position>(new Position()),
            new ComponentBuilder<Size>(new Size(10, 10)),
            new ComponentBuilder<Texture>(new Texture(Enums.TextureEnum.Block, Color.LightBlue)),
            new ComponentBuilder<Health>(new Health()),
            new ComponentBuilder<Collidable>(new Collidable()),
            new ComponentBuilder<Destructable>(new Destructable())
        ];
    }
}