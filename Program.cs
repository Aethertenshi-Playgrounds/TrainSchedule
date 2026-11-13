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

            List<Vector2> gridmapTemp = [];
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
                        float half = CellSize * 0.5f;
                        float quarter = CellSize * 0.25f;
                        float threeQuarter = CellSize * 0.75f;

                        if (!gridmapTemp.Contains(snappedWorld))
                        {
                            gridmapTemp.Add(snappedWorld);
                        }

                        // Draw the cell's centered core (optional, if not already drawn elsewhere)
                        DrawRectangleRec(new(snappedWorld.X + quarter, snappedWorld.Y + quarter, half, half), Color.White);

                        // Check Left (-X)
                        if (gridmapTemp.Contains(new(snappedWorld.X - CellSize, snappedWorld.Y)))
                            DrawRectangleRec(new(snappedWorld.X - quarter, snappedWorld.Y + quarter, half, half), Color.White);

                        // Check Right (+X)
                        if (gridmapTemp.Contains(new(snappedWorld.X + CellSize, snappedWorld.Y)))
                            DrawRectangleRec(new(snappedWorld.X + threeQuarter, snappedWorld.Y + quarter, half, half), Color.White);

                        // Check Up (-Y in Raylib)
                        if (gridmapTemp.Contains(new(snappedWorld.X, snappedWorld.Y - CellSize)))
                            DrawRectangleRec(new(snappedWorld.X + quarter, snappedWorld.Y - quarter, half, half), Color.White);

                        // Check Down (+Y in Raylib)
                        if (gridmapTemp.Contains(new(snappedWorld.X, snappedWorld.Y + CellSize)))
                            DrawRectangleRec(new(snappedWorld.X + quarter, snappedWorld.Y + threeQuarter, half, half), Color.White);
                        
                        DrawRectangleRec(new(snappedWorld.X + (CellSize*0.25f), snappedWorld.Y + (CellSize*0.25f), CellSize*0.5f, CellSize*0.5f), Color.White);
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
                    DrawRectangleRec(new(snappedScreen.X, snappedScreen.Y, CellSize, CellSize), new(255, 255, 255, 50));
                }

                EndDrawing();
            }

            UnloadRenderTexture(gridmap2d);
            CloseWindow();
        }
    }
}
