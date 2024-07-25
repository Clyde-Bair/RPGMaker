namespace RPGMaker {
    internal class Program {
        static void Main(string[] args) {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.CursorVisible = false;
            string[,] player = Player();
            Camera(player);
        }
        static void Camera(string[,] player) {
            int[] coords = {5,10,2,0,0,0,6,0,0}; //Y pos, X pos, Direction, Layer, Map, Tileset, Color, Entity, Collision
            CoordConvert(coords);
            TileData tileData = new TileData();
            TileSet tileSet = new TileSet();
            Entities entities = new Entities();
            while (true) {
                Console.SetCursorPosition(0,0);
                CoordShrink(coords);
                bool canWarp = entities.WarpCheck();
                if (canWarp) { entities.Warp(coords); }
                CoordConvert(coords);
                tileData.DataPointer = coords[4];
                tileSet.DataPointer = coords[5];
                entities.DataPointer = coords[4];
                string[] map = new string[28];
                string[,] data = tileData.GetData();
                string[,,] tile = tileSet.GetTiles();
                map = Map(map);
                entities.GetEntData();
                entities.GetEntTxtData();
                entities.EntityCheck(coords);
                CoordShrink(coords);
                Console.WriteLine($"X: {coords[1]} Y: {coords[0]} D: {coords[2]} R: {data[1, (coords[0] - 16) / -4][(coords[1] - 56) / -8]} L: {coords[3]} A: {coords[4]} T: {coords[5]} C: {coords[6]} E: {coords[7]} G: {coords[8]}   ");
                CoordConvert(coords);
                TileDraw(map, tile, coords, data, player);
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Enter) {
                    if (key.Key == ConsoleKey.W || key.Key == ConsoleKey.S || key.Key == ConsoleKey.A || key.Key == ConsoleKey.D) {
                        if (key.Key == ConsoleKey.W) {
                            coords[2] = 0;
                        }
                        if (key.Key == ConsoleKey.S) {
                            coords[2] = 2;
                        }
                        if (key.Key == ConsoleKey.A) {
                            coords[2] = 3;
                        }
                        if (key.Key == ConsoleKey.D) {
                            coords[2] = 1;
                        }
                        CollisionDetection(coords, map, tile, data, player);
                    }
                } else {
                    Interface menu = new Interface();
                    CoordShrink(coords);
                    coords = Interface.Prompt(coords, entities.GetTextData(), entities.GetEntPointer());
                    CoordConvert(coords);
                }
            }
        }
        static void CollisionDetection(int[] coords, string[] map, string[,,] tile, string[,] data, string[,] player) {
            char symb = data[1, (coords[0] - 16) / -4][(coords[1] - 56) / -8];
            if (coords[2] == 0 || coords[2] == 2) {
                if (coords[2] == 0) { //North
                    symb = data[1, ((coords[0] - 16) / -4)][(coords[1] - 56) / -8];
                    if (symb == '1' || symb == '4' || symb == '6' || symb == 'b' || symb == 'c' || symb == 'd' || symb == 'e' || symb == 'f') {
                        if (coords[8] != 1) {
                            MovementF(coords, map, tile, data, player);
                        } else {
                            Movement(coords, map, tile, data, player);
                        }
                    } else {
                        Movement(coords, map, tile, data, player);
                    }
                } else { //South
                    symb = data[1, ((coords[0] - 16) / -4)][(coords[1] - 56) / -8];
                    if (symb == '1' || symb == '2' || symb == '6' || symb == '8' || symb == '9' || symb == 'a' || symb == 'b' || symb == 'e') {
                        if (coords[8] != 1) {
                            MovementF(coords, map, tile, data, player);
                        } else {
                            Movement(coords, map, tile, data, player);
                        }
                    } else {
                        Movement(coords, map, tile, data, player);
                    }
                }
            } else {
                if (coords[2] == 3) { //West
                    symb = data[1, (coords[0] - 16) / -4][((coords[1] - 56) / -8)];
                    if (symb == '1' || symb == '3' || symb == '7' || symb == '9' || symb == 'a' || symb == 'd' || symb == 'e' || symb == 'f') {
                        if (coords[8] != 1) {
                            MovementF(coords, map, tile, data, player);
                        } else {
                            Movement(coords, map, tile, data, player);
                        }
                    } else {
                        Movement(coords, map, tile, data, player);
                    }
                } else { //East
                    symb = data[1, (coords[0] - 16) / -4][((coords[1] - 56) / -8)];
                    if (symb == '1' || symb == '5' || symb == '7' || symb == '8' || symb == 'a' || symb == 'b' || symb == 'c' || symb == 'f') {
                        if (coords[8] != 1) {
                            MovementF(coords, map, tile, data, player);
                        } else {
                            Movement(coords, map, tile, data, player);
                        }
                    } else {
                        Movement(coords, map, tile, data, player);
                    }
                }
            }
        }
        static int[] Movement(int[] coords, string[] map, string[,,] tile, string[,] data, string[,] player) {
            if (coords[2] == 0 || coords[2] == 2) {
                for (int i = 0; i < 4; i++) {
                    if (coords[2] == 0) {
                        coords[0]++;
                    } else {
                        coords[0]--;
                    }
                    TileDraw(map, tile, coords, data, player);
                    Thread.Sleep(50);
                }
            } else {
                for (int i = 0; i < 4; i++) {
                    if (coords[2] == 3) {
                        coords[1]+=2;
                    } else {
                        coords[1]-=2;
                    }
                    TileDraw(map, tile, coords, data, player);
                    Thread.Sleep(50);
                }
            }
            return coords;
        }
        static void MovementF(int[] coords, string[] map, string[,,] tile, string[,] data, string[,] player) {
            if (coords[2] == 0 || coords[2] == 2) {
                for (int i = 0; i < 2; i++) {
                    if (coords[2] == 0) {
                        coords[0]++;
                    } else {
                        coords[0]--;
                    }
                    TileDraw(map, tile, coords, data, player);
                    Thread.Sleep(50);
                }
                for (int i = 0; i < 2; i++) {
                    if (coords[2] == 0) {
                        coords[0]--;
                    } else {
                        coords[0]++;
                    }
                    TileDraw(map, tile, coords, data, player);
                    Thread.Sleep(50);
                }
            } else {
                for (int i = 0; i < 2; i++) {
                    if (coords[2] == 3) {
                        coords[1] += 2;
                    } else {
                        coords[1] -= 2;
                    }
                    TileDraw(map, tile, coords, data, player);
                    Thread.Sleep(50);
                }
                for (int i = 0; i < 2; i++) {
                    if (coords[2] == 3) {
                        coords[1] -= 2;
                    } else {
                        coords[1] += 2;
                    }
                    TileDraw(map, tile, coords, data, player);
                    Thread.Sleep(50);
                }
            }
            return;
        }
        static int[] CoordConvert(int[] coords) {
            coords[0] *= -4; coords[1] *= -8; coords[0] += 16; coords[1] += 56;
            return coords;
        }
        static int[] CoordShrink(int[] coords) {
            coords[0] -= 16; coords[1] -= 56;  coords[0] /= -4; coords[1] /= -8;
            return coords;
        }
        static void TileDraw(string[] map, string[,,] tile, int[] coords, string[,] data, string[,] player) {
            for (int m = 0; m < 4; m++) {
                if (m == 0 || m == 2) {
                    for (int yy = 0; yy < data[0,0].Count() * 4; yy += 4) {
                        for (int yx = 0; yx < data[0,0].Length * 8; yx += 8) {
                            for (int y = 0; y < 4; y++) {
                                for (int x = 0; x < 8; x++) {
                                    bool inBounds = OffScreen(map, x + yx + coords[1], y + yy + coords[0]);
                                    if (inBounds) {
                                        char datapoint = data[0,yy / 4][yx / 8];
                                        byte i = TileIndex(datapoint);
                                        char symb = tile[0, i, y][x];
                                        char scan;
                                        switch (m) {
                                            case 0: scan = tile[1, i, y][x]; if (scan == '0') { MapUpdate(map, symb, y + yy + coords[0], x + yx + coords[1]); } break;
                                            case 2: scan = tile[1, i, y][x]; if (scan == '1') { MapUpdate(map, symb, y + yy + coords[0], x + yx + coords[1]); } break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                } else {
                    if (m == 1) {
                        PlayerDraw(player, coords, map);
                    }
                    if (m == 3 && coords[3] == 1) {
                        PlayerDraw(player, coords, map);
                    }
                }
            }
            Refresh(map,coords);
        }
        static string[,] Player() {
            string[,] Protag = {{ // North (0)
                    "@@@@@@@@",
                    "@@▄▀▀▀▄@",
                    "@▀█▀▀▀█@",
                    "@@█   █@",
                    "@@@▀█▀@@",
                    "@@▀▀█▀▀@",
                    "@@@@█@@@",
                    "@@@█@█@@"
                },{ // East (1)
                    "@@@@@@@@",
                    "@▄▀▀▀▄@@",
                    "@█▀▀▀█▀@",
                    "@█  '█@@",
                    "@@▀█▀@@@",
                    "@@@█▀▀@@",
                    "@@@█@@@@",
                    "@@@█@@@@"
                },{ // South (2)
                    "@@@@@@@@",
                    "@▄▀▀▀▄@@",
                    "@█▀▀▀█▀@",
                    "@█'_'█@@",
                    "@@▀█▀@@@",
                    "@▀▀█▀▀@@",
                    "@@@█@@@@",
                    "@@█@█@@@"
                },{ // West (3)
                    "@@@@@@@@",
                    "@@▄▀▀▀▄@",
                    "@▀█▀▀▀█@",
                    "@@█'  █@",
                    "@@@▀█▀@@",
                    "@@▀▀█@@@",
                    "@@@@█@@@",
                    "@@@@█@@@"
                }};
            return Protag;
        }
        static void PlayerDraw(string[,] player, int[] coords, string[] map) {
            //int[] pos = { (map.Count() / 2) - 1, map[0].Length - 2};
            int[] pos = {11,112};
            float l = pos[1];
            if (l % 2 == 0.0f) {
                l /= 2;
            } else {
                l /= 2;
                l -= 1.5f;
            }
            pos[1] = (int)l;
            for (int y = 0; y < 8; y++) {
                pos[1] = (int)l;
                string c = player[coords[2],y];
                for (int x = 0; x < c.Length; x++) {
                    char symb = player[coords[2],y][x];
                    if (symb != '@') {
                        MapUpdate(map, symb, pos[0] + y, pos[1] + x);
                    }
                }
            }
        }
        static byte TileIndex(char datapoint) {
            switch (datapoint) {
                case '1': return 0;
                case '2': return 1;
                case '3': return 2;
                case '4': return 3;
                case '5': return 4;
                case '6': return 5;
                case '7': return 6;
                case '8': return 7;
                case '9': return 8;
                case '0': return 9;
                case 'a': return 10;
                case 'b': return 11;
                case 'c': return 12;
                case 'd': return 13;
                case 'e': return 14;
                case 'f': return 15;
                case 'g': return 16;
                case 'h': return 17;
                case 'i': return 18;
                case 'j': return 19;
                case 'k': return 20;
                case 'l': return 21;
                case 'm': return 22;
                case 'n': return 23;
                case 'o': return 24;
                case 'p': return 25;
                case 'q': return 26;
                case 'r': return 27;
                case 's': return 28;
                case 't': return 29;
                case 'u': return 30;
                case 'v': return 31;
                case 'w': return 32;
                case 'x': return 33;
                case 'y': return 34;
                case 'z': return 35;
                default: return 0;
            }
        }
        static void ScreenColor(int[] coords) {
            switch (coords[6]) {
                case 0: Console.ForegroundColor = ConsoleColor.White; Console.BackgroundColor = ConsoleColor.Black; break;
                case 1: Console.ForegroundColor = ConsoleColor.Black; Console.BackgroundColor = ConsoleColor.White; break;
                case 2: Console.ForegroundColor = ConsoleColor.Black; Console.BackgroundColor = ConsoleColor.DarkGreen; break;
                case 3: Console.ForegroundColor = ConsoleColor.Black; Console.BackgroundColor = ConsoleColor.DarkGray; break;
                case 4: Console.ForegroundColor = ConsoleColor.Green; Console.BackgroundColor = ConsoleColor.Black; break;
                case 5: Console.ForegroundColor = ConsoleColor.DarkRed; Console.BackgroundColor = ConsoleColor.Black; break;
                case 6: Console.ForegroundColor = ConsoleColor.DarkMagenta; Console.BackgroundColor = ConsoleColor.Black; break;
            }
        }
        static string[] Map(string[] map) {
            for (int y = 0; y < 28; y++) {
                for (int x = 0; x < 120; x++) {
                    map[y] += " ";
                }
            }
            return map;
        }
        static void Refresh(string[] map, int[] coords) {
            string LightningMcScreen = "";
            for (int y = 0; y < map.Length; y++) {
                LightningMcScreen += map[y];
                LightningMcScreen += "\n";
            }
            Console.SetCursorPosition(0,1);
            ScreenColor(coords);
            Console.Write(LightningMcScreen);
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }
        static bool OffScreen(string[] map, int x, int y) {
            bool inBounds;
            if (x >= map[0].Length || x < 0) {
                inBounds = false;
            } else if (y > map.Count() - 1 || y < 0) {
                inBounds = false;
            } else {
                inBounds = true;
            }
            return inBounds;
        }
        static string[] MapUpdate(string[] map, char symb, int y, int x) {
            string line = "";
            for (int i = 0; i < map[y].Length; i++) {
                if (i < x) {
                    line += map[y][i];
                } else if (i == x) {
                    line += symb;
                } else if (i > x) {
                    line += map[y][i];
                }
            }
            map[y] = line;
            return map;
        }
    }
    public class Interface {
        public static int[] Prompt(int[] coords, string text, int textPointer) {
            ConsoleKeyInfo key;
            byte[] pointer = { 0, 0, 0 };
            string[] textbox1 = {
            "╔══════════════════════════════╗",
            "║           COMMANDS           ║",
            "║                              ║",
            "║     CHECK          ITEMS     ║",
            "║                              ║",
            "║     ?????          DEBUG     ║",
            "║                              ║",
            "╚══════════════════════════════╝",
            };
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            for (int i = 0; i < 8; i++) {
                Console.SetCursorPosition(0, 1 + i);
                Console.Write(textbox1[i]);
                Thread.Sleep(25);
            }
            do {
                Console.SetCursorPosition(6, 4);
                if (pointer[0] == 0) {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }
                Console.WriteLine("CHECK");
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(21, 4);
                if (pointer[0] == 1) {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }
                Console.WriteLine("ITEMS");
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(6, 6);
                if (pointer[0] == 2) {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }
                Console.WriteLine("?????");
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(21, 6);
                if (pointer[0] == 3) {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }
                Console.WriteLine("DEBUG");
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.W) {
                    if (pointer[0] == 2) {
                        pointer[0] = 0;
                    } else if (pointer[0] == 3) {
                        pointer[0] = 1;
                    }
                }
                if (key.Key == ConsoleKey.D) {
                    if (pointer[0] == 0) {
                        pointer[0] = 1;
                    } else if (pointer[0] == 2) {
                        pointer[0] = 3;
                    }
                }
                if (key.Key == ConsoleKey.S) {
                    if (pointer[0] == 0) {
                        pointer[0] = 2;
                    } else if (pointer[0] == 1) {
                        pointer[0] = 3;
                    }
                }
                if (key.Key == ConsoleKey.A) {
                    if (pointer[0] == 1) {
                        pointer[0] = 0;
                    } else if (pointer[0] == 3) {
                        pointer[0] = 2;
                    }
                }
            } while (key.Key != ConsoleKey.Enter);
            if (pointer[0] == 0) {
                Check(coords, text, textPointer);
            } else if (pointer[0] == 1) {
                Items(coords);
            } else if (pointer[0] == 2) {
                Unknown(coords);
            } else if (pointer[0] == 3) {
                Debug(coords);
            }
            return coords;
        }
        public static int[] Check(int[] coords, string text, int textPointer) {
            string[] textbox2 = {
                    "╔══════════════════════════════════════════════╗",
                    "║                                              ║",
                    "║                                              ║",
                    "║                                              ║",
                    "║                                              ║",
                    "║                                              ║",
                    "║                                              ║",
                    "╚══════════════════════════════════════════════╝",
                };
            for (int i = 0; i < 8; i++) {
                Console.SetCursorPosition(0, 21 + i);
                Console.Write(textbox2[i]);
                Thread.Sleep(5);
            }
            if (coords[7] == 0) {
                Console.SetCursorPosition(1, 22);
                string error = "There is nothing interesting to check.";
                for (int i = 0; i < error.Length; i++) {
                    Console.Write(error[i]);
                    Thread.Sleep(25);
                }
                Console.ReadKey(true);
                return coords;
            } else {
                if (coords[4] == 0) {
                    switch (textPointer) {
                        case 2:
                            Console.SetCursorPosition(1, 22);
                            for (int i = 0; i < 43; i++) {
                                Console.Write(text[i]);
                                Thread.Sleep(25);
                            }
                            Console.ReadKey(true);
                            Console.SetCursorPosition(1, 23);
                            for (int i = 0; i < 34; i++) {
                                Console.Write(text[i + 43]);
                                Thread.Sleep(25);
                            }
                            Console.ReadKey(true);
                            return coords;
                        case 3:
                            Console.SetCursorPosition(1, 22);
                            for (int i = 0; i < 43; i++) {
                                Console.Write(text[i]);
                                Thread.Sleep(25);
                            }
                            Console.ReadKey(true);
                            Console.SetCursorPosition(1, 23);
                            for (int i = 0; i < 41; i++) {
                                Console.Write(text[i + 43]);
                                Thread.Sleep(25);
                            }
                            Console.ReadKey(true);
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Clear();
                            Thread.Sleep(750);
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                            Console.Clear();
                            Thread.Sleep(750);
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.Clear();
                            Thread.Sleep(750);
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.Clear();
                            Thread.Sleep(2000);
                            Console.BackgroundColor = ConsoleColor.Black;
                            coords[0] = 7; coords[1] = 11; coords[2] = 2; coords[4] = 1; coords[5] = 1; coords[6] = 4;
                            Console.Clear();
                            return coords;
                        default:
                            Console.SetCursorPosition(1, 22);
                            for (int i = 0; i < text.Length; i++) {
                                Console.Write(text[i]);
                                Thread.Sleep(25);
                            }
                            Console.ReadKey(true);
                            return coords;
                    }
                } else if (coords[4] == 1) {
                    switch (textPointer) {
                        case 0:
                            Console.SetCursorPosition(1, 22);
                            for (int i = 0; i < 20; i++) {
                                Console.Write(text[i]);
                                Thread.Sleep(25);
                            }
                            Console.ReadKey(true);
                            Console.SetCursorPosition(1, 23);
                            for (int i = 0; i < 19; i++) {
                                Console.Write(text[i+20]);
                                Thread.Sleep(25);
                            }
                            Console.ReadKey(true);
                            Console.SetCursorPosition(1, 24);
                            for (int i = 0; i < 22; i++) {
                                Console.Write(text[i+39]);
                                Thread.Sleep(25);
                            }
                            Console.ReadKey(true);
                            Console.SetCursorPosition(1, 25);
                            for (int i = 0; i < 32; i++) {
                                Console.Write(text[i+61]);
                                Thread.Sleep(25);
                            }
                            Console.ReadKey(true);
                            return coords;
                        case 3:
                            Console.SetCursorPosition(1, 22);
                            for (int i = 0; i < 41; i++) {
                                Console.Write(text[i]);
                                Thread.Sleep(25);
                            }
                            Console.ReadKey(true);
                            Console.SetCursorPosition(1, 23);
                            for (int i = 0; i < 15; i++) {
                                Console.Write(text[i+41]);
                                Thread.Sleep(25);
                            }
                            Console.ReadKey(true);
                            return coords;
                        default:
                            Console.SetCursorPosition(1, 22);
                            for (int i = 0; i < text.Length; i++) {
                                Console.Write(text[i]);
                                Thread.Sleep(25);
                            }
                            Console.ReadKey(true);
                            return coords;
                    }
                } else if (coords[4] == 2) {
                    switch (textPointer) {
                        default:
                            Console.SetCursorPosition(1, 22);
                            for (int i = 0; i < text.Length; i++) {
                                Console.Write(text[i]);
                                Thread.Sleep(25);
                            }
                            Console.ReadKey(true);
                            return coords;
                    }
                } else if (coords[4] == 3) {
                    switch (textPointer) {
                        case 1:
                            Unknown(coords);
                            return coords;
                        default:
                            Console.SetCursorPosition(1, 22);
                            for (int i = 0; i < text.Length; i++) {
                                Console.Write(text[i]);
                                Thread.Sleep(25);
                            }
                            Console.ReadKey(true);
                            return coords;
                    }
                } else {
                    Console.SetCursorPosition(1, 22);
                    for (int i = 0; i < text.Length; i++) {
                        Console.Write(text[i]);
                        Thread.Sleep(25);
                    }
                    Console.ReadKey(true);
                }
                return coords;
            }
        }
        public static int[] Items(int[] coords) {
            string[] textbox2 = {
                    "╔═════════════════════════╗",
                    "║        INVENTORY        ║",
                    "╟─────────────────────────╢",
                    "║ Stick - x6              ║",
                    "║ Banana - x2             ║",
                    "║ ARG Juice - x7          ║",
                    "╚═════════════════════════╝",
                };
            for (int i = 0; i < 7; i++) {
                Console.SetCursorPosition(46, 1 + i);
                Console.Write(textbox2[i]);
                Thread.Sleep(5);
            }
            Console.ReadKey(true);
            return coords;
        }
        public static int[] Unknown(int[] coords) {
            string[] textbox2 = {
                    "╔══════╗",
                    "║      ║",
                    "╚══════╝",
                };
            for (int i = 0; i < 3; i++) {
                Console.SetCursorPosition(56, 24 + i);
                Console.Write(textbox2[i]);
            }
            Thread.Sleep(300);
            Console.SetCursorPosition(58, 25);
            for (int i = 0; i < 4; i++) {
                Random random = new Random();
                Console.Write(random.Next(10));
                Thread.Sleep(300);
            }
            Console.ReadKey(true);
            return coords;
        }
        public static int[] Debug(int[] coords) {
            string[] textbox2 = {
                    "╔══════════════════════╗",
                    "║      DEBUG MENU      ║",
                    "║                      ║",
                    "║ Change Map           ║",
                    "║ Change Tiles         ║",
                    "║ Change Coords        ║",
                    "║ Change Layer         ║",
                    "║ Change Color         ║",
                    "║                      ║",
                    "║ Go Ghost             ║",
                    "║                      ║",
                    "╚══════════════════════╝",
                };
            for (int i = 0; i < 12; i++) {
                Console.SetCursorPosition(96, 1 + i);
                Console.Write(textbox2[i]);
                Thread.Sleep(25);
            }
            byte pointer = 0;
            ConsoleKeyInfo key;
            do {
                Console.SetCursorPosition(98, 4);
                if (pointer == 0) {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }
                Console.WriteLine("Change Map          ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(98, 5);
                if (pointer == 1) {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }
                Console.WriteLine("Change Tiles        ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(98, 6);
                if (pointer == 2) {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }
                Console.WriteLine("Change Coords       ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(98, 7);
                if (pointer == 3) {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }
                Console.WriteLine("Change Layer        ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(98, 8);
                if (pointer == 4) {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }
                Console.WriteLine("Change Color        ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(98, 10);
                if (pointer == 5) {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }
                Console.WriteLine("Go Ghost            ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.W) {
                    if (pointer > 0) {
                        pointer--;
                    }
                } else if (key.Key == ConsoleKey.S) {
                    if (pointer < 5) {
                        pointer++;
                    }
                }
            } while (key.Key != ConsoleKey.Enter);
            string[] textbox3 = {
                    "╔══════════════════════╗",
                    "║                      ║",
                    "╚══════════════════════╝",
                };
            for (int i = 0; i < 3; i++) {
                Console.SetCursorPosition(96, 13 + i);
                Console.Write(textbox3[i]);
                Thread.Sleep(25);
            }
            Console.SetCursorPosition(98, 14);
            if (pointer != 5) {
                Console.Write("Set Value: ");
                Console.CursorVisible = true;
                string input = Console.ReadLine() ?? "0";
                Console.CursorVisible = false;
                int value = 0;
                bool isValid = int.TryParse(input,out value);
                if (isValid) {
                    value = int.Parse(input);
                }
                switch (pointer) {
                    case 0: coords[0] = 0; coords[1] = 0; coords[2] = 2; coords[4] = value; break;
                    case 1: coords[5] = value; break;
                    case 2: break;
                    case 3: if (coords[3] == 0) { coords[3] = 1; break; } if (coords[3] == 1) { coords[3] = 0; break; } break;
                    case 4: coords[6] = value; break;
                }
            } else {
                string funny = "Im going Ghost!";
                for (int i = 0; i < funny.Length; i++) {
                    Console.Write(funny[i]);
                    Thread.Sleep(100);
                }
                Thread.Sleep(500);
                if (coords[8] == 0) { coords[8] = 1; return coords; }
                if (coords[8] == 1) { coords[8] = 0; return coords; }
            }
            return coords;
        }
    }
    public class TileData {
        public int DataPointer = 0;
        public string[,] TileMap = { };
        public string[,] GetData() {
            string[,] BlockWorld = {{//Visual Data
            "111111111111111111111",
            "1d4811d48111111111111",
            "10a7110ag111d48111111",
            "1nce11fgg1110ag111111",
            "111111fg7111677111111",
            "11d481bce111bce111111",
            "110a71111111111111111",
            "11bce11k4444444811111",
            "1111111b044444ac11111",
            "11191111immmmmj111111",
            "111111111111111111111",
            "111111111111111111111",
            ".....................",
            ".....................",
            "....................."
            },{//Collision Data
            "d4444444444444444444c",
            "322200000000000000005",
            "700030000000000000005",
            "700d00222000222000005",
            "344005000305000300005",
            "30222500d00500d000005",
            "350003440000440000005",
            "3500d0022222222200005",
            "304600522222222230005",
            "305030046666666400005",
            "300400004444444000005",
            "922222222222222222228",
            ".....................",
            ".....................",
            "....................."
            }};
            string[,] PaperWorld = {{
                "00000000000000000000000000",
                "0000000ghfij0000ghfij00000",
                "0000000dfffe0000dfffe00000",
                "000000080ba6000080b0600000",
                "000000035c54000035c5400000",
                "0ghfij00000000000000000000",
                "0dfffe00000000l00000000000",
                "080ba600000000000000000000",
                "035c5400000000000000000000",
                "00000000000000000000000000",
                "00000000000000000000000000",
                "00000000000000000000000000", //Ignore Row
                "00000000000000000000000000", //Ignore Row
                "00000000000000000000000000"  //Ignore Row
                },{
                "d444444444444444444444444c",
                "30000000000000000000000005",
                "30000002222200002222200005",
                "30000050000030050000030005",
                "300000500f0030050000030005",
                "30000004404400204444400005",
                "32222200000005030000000005",
                "70000030000000400000000005",
                "700f0030000000000000000005",
                "34404400000000000000000005",
                "92222222222222222222222228",
                "..........................",
                "..........................",
                ".........................."
                }};
            string[,] House1 = {{
                "00000000000",
                "01777777720",
                "08000000060",
                "03555055540",
                "00000k00000",
                "00000000000",//
                "00000000000",//
                "00000000000",//
                },{
                "00000000000",
                "0d4444444c0",
                "03000000050",
                "09222022280",
                "00000a00000",
                "00000000000",//
                "00000000000",//
                "00000000000",//
                }};
            string[,] House2 = {{
                "00000000000",
                "01777777720",
                "08000000060",
                "03555055540",
                "00000k00000",
                "00000000000",//
                "00000000000",//
                "00000000000",//
                },{
                "00000000000",
                "0d4444444c0",
                "03000000050",
                "09222022280",
                "00000a00000",
                "00000000000",//
                "00000000000",//
                "00000000000",//
                }};
            switch (DataPointer) {
                case 0: return BlockWorld;
                case 1: return PaperWorld;
                case 2: return House1;
                case 3: return House2;
                default: return BlockWorld;
            }
        }
    }
    public class  TileSet {
        public int DataPointer = 0;
        public string[,,] Tiles = { };
        public string[,,] GetTiles() {
            string[,,] BlockWorld ={{{
                            "        ",//1
                            "        ",
                            "        ",
                            "        "
            },
                          { "      ▄▀",//2
                            "    ▄▀  ",
                            "  ▄▀    ",
                            "▄▀      "
            },
                          { "▀▄      ",//3
                            "  ▀▄    ",
                            "    ▀▄  ",
                            "      ▀▄"
            },
                          { "▀▀▀▀▀▀▀▀",//4
                            "        ",
                            "        ",
                            "        "
            },
                          { "        ",//5
                            "        ",
                            "        ",
                            "▄▄▄▄▄▄▄▄"
            },
                          { "█       ",//6
                            "█       ",
                            "█       ",
                            "█       "
            },
                          { "       █",//7
                            "       █",
                            "       █",
                            "       █"
            },
                          { "▀▀▀▀▀▀██",//8
                            "    ▄▀ █",
                            "  ▄▀   █",
                            "▄▀     █"
            },
                          { "█▀▀▀▀▀▀█",//9
                            "█      █",
                            "█      █",
                            "█▄▄▄▄▄▄█"
            },
                          { "█▀▀▀▀▀▀▀",//0
                            "█       ",
                            "█       ",
                            "█       "
            },
                          { "▀▀▀▀▀▀▀█",//a
                            "       █",
                            "       █",
                            "       █"
            },
                          { "█       ",//b
                            "█       ",
                            "█       ",
                            "█▄▄▄▄▄▄▄"
            },
                          { "       █",//c
                            "       █",
                            "       █",
                            "▄▄▄▄▄▄▄█"
            },
                          { "      ▄▀",//d
                            "    ▄▀  ",
                            "  ▄▀    ",
                            "▄▀      "
           },
                          { "      ▄▀",//e
                            "    ▄▀  ",
                            "  ▄▀    ",
                            "▄▀      "
           },
                          { "█       ",//f
                            "█       ",
                            "█       ",
                            "█       "
            },
                          { "       █",//g
                            "       █",
                            "       █",
                            "       █"
            },
                          { "▀▀▀▀▀▀██",//h
                            "    ▄▀ █",
                            "  ▄▀   █",
                            "▄▀     █"

           },
                          { "█       ",//i
                            "█       ",
                            "█       ",
                            "█▄▄▄▄▄▄▄"
            },
                          { "       █",//j
                            "       █",
                            "       █",
                            "▄▄▄▄▄▄▄█"
            },
                          { "██▀▀▀▀▀▀",//k
                            "█ ▀▄    ",
                            "█   ▀▄  ",
                            "█     ▀▄"

           },
                          { "▀▄      ",//l
                            "  ▀▄    ",
                            "    ▀▄  ",
                            "      ▀▄"
            },
                          { "        ",//m
                            "        ",
                            "        ",
                            "▄▄▄▄▄▄▄▄"
            },
                          { "█       ",//n
                            "█  WARP ",
                            "█       ",
                            "█▄▄▄▄▄▄▄"
            }
           },{
                    //Layer Data
           {                "00000000",//1
                            "00000000",
                            "00000000",
                            "00000000"
           },
                          { "11111111",//2
                            "11111100",
                            "11110000",
                            "11000000"
           },
                          { "11111111",//3
                            "00111111",
                            "00001111",
                            "00000011"
           },
                          { "11111111",//4
                            "11111111",
                            "11111111",
                            "11111111"
           },
                          { "00000000",//5
                            "00000000",
                            "00000000",
                            "00000000"
           },
                          { "00000000",//6
                            "00000000",
                            "00000000",
                            "00000000"
           },
                          { "00000000",//7
                            "00000000",
                            "00000000",
                            "00000000"
           },
                          { "11111111",//8
                            "11111111",
                            "11111111",
                            "11111111"
           },
                          { "11111111",//9
                            "11111111",
                            "11111111",
                            "11111111"
           },
                          { "11111111",//0
                            "11111111",
                            "11111111",
                            "11111111"
           },
                          { "11111111",//a
                            "11111111",
                            "11111111",
                            "11111111"
           },
                          { "00000000",//b
                            "00000000",
                            "00000000",
                            "00000000"
           },
                          { "00000000",//c
                            "00000000",
                            "00000000",
                            "00000000"
           },
                          { "00000000",//d
                            "00000011",
                            "00001111",
                            "00111111"
            },
                          { "00000000",//e
                            "00000000",
                            "00000000",
                            "00000000"
           },
                          { "11111111",//f
                            "11111111",
                            "11111111",
                            "11111111"
            },
                          { "11111111",//g
                            "11111111",
                            "11111111",
                            "11111111"
            },
                          { "11111111",//h
                            "11111100",
                            "11110000",
                            "11000000"

            },
                          { "11111111",//i
                            "11111111",
                            "11111111",
                            "11111111"
            },
                          { "11111111",//j
                            "11111111",
                            "11111111",
                            "11111111"
            },
                          { "11111111",//k
                            "11111111",
                            "11111111",
                            "11111111"

           },
                          { "00000000",//l
                            "00000000",
                            "00000000",
                            "00000000"
            },
                          { "11111111",//m
                            "11111111",
                            "11111111",
                            "11111111"
            },
                          { "00000000",//n
                            "00000000",
                            "00000000",
                            "00000000"
            }
                    }};
            string[,,] PaperWorld = {{
                    {
                        "█▀▀▀▀▀▀▀",
                        "█       ",
                        "█       ",
                        "█       "//1
                    }, {
                        "▀▀▀▀▀▀▀█",
                        "       █",
                        "       █",
                        "       █"//2
                    }, {
                        "█       ",
                        "█       ",
                        "█       ",
                        "█▄▄▄▄▄▄▄"//3
                    }, {
                        "       █",
                        "       █",
                        "       █",
                        "▄▄▄▄▄▄▄█"//4
                    }, {
                        "        ",
                        "        ",
                        "        ",
                        "▄▄▄▄▄▄▄▄"//5
                    }, {
                        "       █",
                        "       █",
                        "       █",
                        "       █"//6
                    }, {
                        "▀▀▀▀▀▀▀▀",
                        "        ",
                        "        ",
                        "        "//7
                    }, {
                        "█       ",
                        "█       ",
                        "█       ",
                        "█       "//8
                    }, {
                        "█▀▀▀▀▀▀█",
                        "█      █",
                        "█      █",
                        "█▄▄▄▄▄▄█"//9
                    }, {
                        "        ",
                        "        ",
                        "        ",
                        "        "//0
                    }, {
                        "  ▄▄▄▄▄▄",
                        "  █ █  █",
                        "  █▀█▀▀█",
                        "  ▀▀▀▀▀▀"//a
                    }, {
                        " ▄▄▀▀▄▄ ",
                        "█      █",
                        "█      █",
                        "█      █"//b
                    }, {
                        "█    O █",
                        "█      █",
                        "█      █",
                        "█▄▄▄▄▄▄█"//c
                    },{
                        "█▀▀▀▀▀▀▀",
                        "█       ",
                        "█       ",
                        "█       "//d
                    }, {
                        "▀▀▀▀▀▀▀█",
                        "       █",
                        "       █",
                        "       █"//e
                    }, {
                        "▀▀▀▀▀▀▀▀",
                        "        ",
                        "        ",
                        "        "//f
                    }, {
                        "        ",
                        "       ▄",
                        "   ▄▄▀▀ ",
                        "▄▀▀     "//g
                    }, {
                        "   ▄▄▄▄▄",
                        "▄▀▀     ",
                        "        ",
                        "        "//h
                    },{
                        "▄▄▄▄▄   ",
                        "     ▀▀▄",
                        "        ",
                        "        "//i
                    }, {
                        "        ",
                        "▄       ",
                        " ▀▀▄▄   ",
                        "     ▀▀▄"//j
                    }, {
                        "█      █",
                        "█      █",
                        "█      █",
                        "█▄▄▄▄▄▄█"//k
                    }, {
                        "█▀▀▀▀▀▀█",
                        "█ SIGN █",
                        "█▀▀▀▀▀▀█",
                        "█      █"//l
                    }
                },{
                    {
                        "00000000",
                        "00000000",
                        "00000000",
                        "00000000"//1
                    }, {
                        "00000000",
                        "00000000",
                        "00000000",
                        "00000000"//2
                    }, {
                        "00000000",
                        "00000000",
                        "00000000",
                        "00000000"//3
                    }, {
                        "00000000",
                        "00000000",
                        "00000000",
                        "00000000"//4
                    }, {
                        "00000000",
                        "00000000",
                        "00000000",
                        "00000000"//5
                    }, {
                        "00000000",
                        "00000000",
                        "00000000",
                        "00000000"//6
                    }, {
                        "00000000",
                        "00000000",
                        "00000000",
                        "00000000"//7
                    }, {
                        "00000000",
                        "00000000",
                        "00000000",
                        "00000000"//8
                    }, {
                        "00000000",
                        "00000000",
                        "00000000",
                        "00000000"//9
                    }, {
                        "00000000",
                        "00000000",
                        "00000000",
                        "00000000"//0
                    }, {
                        "00000000",
                        "00000000",
                        "00000000",
                        "00000000"//a
                    }, {
                        "00000000",
                        "00000000",
                        "00000000",
                        "00000000"//b
                    }, {
                        "00000000",
                        "00000000",
                        "00000000",
                        "00000000"//c
                    },{
                        "11111111",
                        "11111111",
                        "11111111",
                        "11111111"//d
                    }, {
                        "11111111",
                        "11111111",
                        "11111111",
                        "11111111"//e
                    }, {
                        "11111111",
                        "11111111",
                        "11111111",
                        "11111111"//f
                    }, {
                        "00000000",
                        "00000001",
                        "00011111",
                        "11111111"//g
                    }, {
                        "00011111",
                        "11111111",
                        "11111111",
                        "11111111"//h
                    },{
                        "11111000",
                        "11111111",
                        "11111111",
                        "11111111"//i
                    }, {
                        "00000000",
                        "10000000",
                        "11111000",
                        "11111111"//j
                    }, {
                        "00000000",
                        "00000000",
                        "00000000",
                        "00000000"//k
                    }, {
                        "00000000",
                        "00000000",
                        "00000000",
                        "00000000"//l
                    }
                }};
            switch (DataPointer) {
                case 0: return BlockWorld;
                case 1: return PaperWorld;
                default: return BlockWorld;
            }
        }
    }
    public class Entities {
        public int DataPointer = 0;
        public int[,] entLocation = {};
        public string[] entTextData = {};
        public int entPointer = 0;
        private bool[] validity = {false,false,false};
        public int[,] GetEntData() {
            int[,] BlockWorld = {{5,14,0},{3,9,0},{9,4,3},{4,1,0},{9,11,0}};
            int[,] PaperWorld = {{5,18,0},{4,9,0},{8,3,0},{7,14,0}};
            int[,] House1 = {{4,5,2}};
            int[,] House2 = {{4,5,2},{1,9,0}};
            switch (DataPointer) {
                case 0: entLocation = BlockWorld; break;
                case 1: entLocation = PaperWorld; break;
                case 2: entLocation = House1; break;
                case 3: entLocation = House2; break;
                default: entLocation = BlockWorld; break;
            }
            return entLocation;
        }
        public string[] GetEntTxtData() {
            string[] BlockWorld = { "This is my favorite spot to hang out on!", "\"Hello World!\" said the invisible man.", "There is a 2D Square in the 3D Block World.So many mysteries of this world...", "You noticed the \"Warp\" written on the wall....And for some reason it began to flash!", "Bolternon was here!" };
            string[] PaperWorld = { "This door is locked.Nobody is inside...Is this house haunted?Is there a ghost in the machine?","warp","warp","Welcome to Paper World, it is very empty.Come back soon." };
            string[] House1 = {"warp"};
            string[] House2 = {"warp","spooky"};
            switch (DataPointer) {
                case 0: entTextData = BlockWorld; break;
                case 1: entTextData = PaperWorld; break;
                case 2: entTextData = House1; break;
                case 3: entTextData = House2; break;
                default: entTextData = BlockWorld; break;
            }
            return entTextData;
        }
        public int[] EntityCheck(int[] coords) {
            CoordShrink(coords);
            entPointer = 0;
            for (int y = 0; y < entLocation.GetLength(0); y++) {
                for (int x = 0; x < 3; x++) {
                    if (coords[x] == entLocation[y,x]) {
                        validity[x] = true;
                    } else {
                        validity[x] = false;
                    }
                }
                if (validity[0] == true && validity[1] == true && validity[2] == true) {
                    coords[7] = 1;
                    CoordConvert(coords);
                    return coords;
                } else {
                    coords[7] = 0;
                }
                entPointer++;
            }
            entPointer = 0;
            CoordConvert(coords);
            return coords;
        }
        public bool WarpCheck() {
            if (validity[0] == true && validity[1] == true && validity[2] == true && entTextData[entPointer] == "warp") {
                return true;
            } else {
                return false;
            }
        }
        public int[] Warp(int[] coords) {
            if (coords[4] == 0) {
                return coords;
            }
            if (coords[4] == 1) {
                switch (entPointer) {
                    case 1: coords[0] = 3; coords[1] = 5; coords[2] = 0; coords[4] = 2; coords[5] = 1; break;
                    case 2: coords[0] = 3; coords[1] = 5; coords[2] = 0; coords[4] = 3; coords[5] = 1; break;
                }
                return coords;
            }
            if (coords[4] == 2) {
                switch (entPointer) {
                    case 0: coords[0] = 5; coords[1] = 9; coords[2] = 2; coords[4] = 1; coords[5] = 1; break;
                }
                return coords;
            }
            if (coords[4] == 3) {
                switch (entPointer) {
                    case 0: coords[0] = 9; coords[1] = 3; coords[2] = 2; coords[4] = 1; coords[5] = 1; break;
                }
                return coords;
            }
            return coords;
        }
        public string GetTextData() {
            return entTextData[entPointer];
        }
        public int GetEntPointer() {
            return entPointer;
        }
        static int[] CoordShrink(int[] coords) {
            coords[0] -= 16; coords[1] -= 56; coords[0] /= -4; coords[1] /= -8;
            return coords;
        }
        static int[] CoordConvert(int[] coords) {
            coords[0] *= -4; coords[1] *= -8; coords[0] += 16; coords[1] += 56;
            return coords;
        }
    }
}
