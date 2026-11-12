using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace ModernIndustrial {
    public class MainGame {
        private const int CellSize = 60;

        private static float Snap(float value) => MathF.Floor(value / CellSize) * CellSize;

        public static void Main() {
            InitWindow(1280, 720, "MI Game");
            SetTargetFPS(60);

            Vector2 lastMousePos = Vector2.Zero;
            Vector2 originRelPos = Vector2.Zero;
            Vector2 relPos = Vector2.Zero;

            RenderTexture2D gridmap2d = LoadRenderTexture(1920, 1080);
            BeginTextureMode(gridmap2d);
            ClearBackground(Color.Blank);
            EndTextureMode();

            while (!WindowShouldClose()) {
                Vector2 mousePos = GetMousePosition();

                // Panning with right-click
                if (IsMouseButtonDown(MouseButton.Right)) {
                    relPos = originRelPos + (mousePos - lastMousePos);
                } else {
                    lastMousePos = mousePos;
                    originRelPos = relPos;
                }

                // Convert screen mouse to texture/world space and snap
                Vector2 worldMouse = mousePos - relPos;
                Vector2 snappedWorld = new(Snap(worldMouse.X), Snap(worldMouse.Y));

                // Paint onto RenderTexture
                if (IsMouseButtonDown(MouseButton.Left)) {
                    BeginTextureMode(gridmap2d);
                    DrawRectangleRec(new(snappedWorld.X, snappedWorld.Y, CellSize, CellSize), Color.Yellow);
                    EndTextureMode();
                }

                BeginDrawing();
                ClearBackground(Color.Black);

                DrawTextureRec(
                    gridmap2d.Texture,
                    new(0, 0, gridmap2d.Texture.Width, -gridmap2d.Texture.Height),
                    relPos,
                    Color.White
                );

                // Hover cursor aligned to the panned grid
                if (IsWindowFocused()) {
                    Vector2 snappedScreen = snappedWorld + relPos;
                    DrawRectangleRec(new(snappedScreen.X, snappedScreen.Y, CellSize, CellSize), Color.Red);
                }

                EndDrawing();
            }

            UnloadRenderTexture(gridmap2d);
            CloseWindow();
        }
    }
}
