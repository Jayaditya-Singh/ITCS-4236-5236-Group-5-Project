# Ghost Postmaster — Art Pack v1

16×16 pixel art on a 16 px grid (buildings are 32×32, post office 48×32).

## Unity import settings (select all PNGs → Inspector)
- Texture Type: Sprite (2D and UI)
- Pixels Per Unit: 16  (1 tile = 1 Unity unit)
- Filter Mode: Point (no filter)
- Compression: None
- Sheets (`*_sheet.png`): Sprite Mode = Multiple → Sprite Editor → Slice → Grid By Cell Size 16×16

## Contents
**Characters/**
- `player_sheet.png` — 8 frames: down0, down1, up0, up1, left0, left1, right0, right1 (2-frame float loop per direction)
- `npc_<mint|lavender|peach>_sheet.png` — wander0, wander1, sleep (for the nighttime "return home" state)

**Tiles/** (drag into a Tile Palette)
- `grass_0-2` walkable, `path` walkable road
- `block_gravestone`, `block_pumpkin`, `block_fence` — unwalkable obstacle tiles
- `block_fog_overlay` — transparent fog to place on a second Tilemap layer for tiles that become blocked at runtime

**Buildings/**
- `post_office.png`, `house_purple/teal/rose.png`
- `*_fading.png` — semi-transparent version for houses appearing/vanishing

**Items_UI/**
- `letter` (pickup + HUD counter), `mail_bubble` (shows above a house waiting for mail), `clock` (timer HUD), `path_dot` (draw the A* route), `target_highlight` (destination tile)

`scene_mockup.png` shows everything together; `preview_sheet.png` is the full asset overview.
