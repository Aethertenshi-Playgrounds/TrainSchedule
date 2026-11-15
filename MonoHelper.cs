using System.ComponentModel;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace TrainSchedule {
    public static class Helper
    {
        public struct AssetContainer(Vector2? position, Vector2? containerSize)
        {
            public Vector2 Position { get; set; } = position ?? Vector2.Zero;
            public Vector2 ContainerSize { get; set; } = containerSize ?? Vector2.Zero;
            public readonly RenderTexture2D Asset => texture;

            private bool containerHighlight = false;
            private RenderTexture2D texture = LoadRenderTexture((int)(containerSize?.X ?? 200), (int)(containerSize?.Y ?? 200));

            public void EnableContainerHighlight(bool state) => containerHighlight = state;
            public readonly void StartRecording()
            {
                BeginTextureMode(texture);
                    if (containerHighlight) DrawRectangle(0, 0, texture.Texture.Width, texture.Texture.Height, new(255, 255, 255, 100));
            }
            public readonly void EndRecording() => EndTextureMode();
            public readonly void DrawAsset(Vector2? position, Color? color)
            {
                DrawTextureRec(
                    texture.Texture,
                    new(0, 0, texture.Texture.Width, -texture.Texture.Height),
                    position ?? Vector2.Zero,
                    color ?? Color.White
                );
            }
        }
    }
}
