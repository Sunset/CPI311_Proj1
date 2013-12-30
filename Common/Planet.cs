using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Common
{
    public class Planet : ModelObject
    {
        public Object Parent { get; set; }
        public float Radius { get; set; }
        public float RevolutionRate { get; set; }
        public float RotationRate { get; set; }
        public float RevolutionPosition { get; set; }
        public float RotationPosition { get; set; }

        public Planet()
        {
            RevolutionRate = 1;
            RotationRate = 0;
        }

        public virtual void Update(GameTime gameTime, float gameSpeed)
        {
            RevolutionPosition = (RevolutionPosition + RevolutionRate * gameTime.ElapsedGameTime.Milliseconds * gameSpeed/ 1000f) % (MathHelper.Pi * 2);
            RotationPosition = (RotationPosition + RotationRate * gameTime.ElapsedGameTime.Milliseconds / 1000f) % (MathHelper.Pi * 2);
            
        }

        public override Matrix World
        {
            get
            {
                Position = Vector3.Transform(new Vector3(Radius, 0, 0),
                    Matrix.CreateRotationY(RevolutionPosition) *
                    Matrix.CreateTranslation(Parent.Position));
                return Matrix.CreateScale(Scale)*
                    Matrix.CreateRotationY(RotationPosition)* 
                    Matrix.CreateTranslation(Radius,0,0) *
                    Matrix.CreateRotationY(RevolutionPosition)*
                    Matrix.CreateTranslation(Parent.Position);
            }
        } 
    }
}
