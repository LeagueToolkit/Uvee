using Fantome.Libraries.League.IO.SimpleSkin;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using System.Collections.Generic;
using System.IO;

namespace Uvee
{
    public static class Processor
    {
        private static readonly SolidBrush BRUSH = Brushes.Solid(Color.White);

        public static void Process(string fileLocation, SKNFile skn)
        {
            string exportDirectory = string.Format(@"{0}\Uvee_{1}", Path.GetDirectoryName(fileLocation), Path.GetFileNameWithoutExtension(fileLocation));
            Directory.CreateDirectory(exportDirectory);

            foreach (SKNSubmesh submesh in skn.Submeshes)
            {
                Image image = CreateImage();
                List<ushort> indices = submesh.GetNormalizedIndices();

                //Loop through all submesh faces and draw the lines for them using vertex UV
                for (int i = 0; i < indices.Count;)
                {
                    SKNVertex vertex1 = submesh.Vertices[indices[i++]];
                    SKNVertex vertex2 = submesh.Vertices[indices[i++]];
                    SKNVertex vertex3 = submesh.Vertices[indices[i++]];

                    PointF[] points = new PointF[]
                    {
                        new PointF(1024 * vertex1.UV.X, 1024 * vertex1.UV.Y),
                        new PointF(1024 * vertex2.UV.X, 1024 * vertex2.UV.Y),
                        new PointF(1024 * vertex3.UV.X, 1024 * vertex3.UV.Y),
                        new PointF(1024 * vertex1.UV.X, 1024 * vertex1.UV.Y) //4th point to close the edge loop
                    };

                    DrawLines(image, points);
                }

                string submeshLocation = string.Format(@"{0}\{1}.png", exportDirectory, submesh.Name);
                image.SaveAsPng(File.Create(submeshLocation));
            }
        }

        private static Image CreateImage()
        {
            return new Image<Rgba32>(1024, 1024);
        }
        private static void DrawLines(Image image, PointF[] points)
        {
            image.Mutate(x => x.DrawLines(BRUSH, 0.75f, points));
        }
    }
}
