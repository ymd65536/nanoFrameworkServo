using nanoFramework.M5Stack; // Fireクラスに必要
using System.Threading;

// 1. 初期化
Fire.InitializeScreen();

// 3. Clearを呼んで画面全体をBackgroundColorで塗りつぶす
nanoFramework.M5Stack.Console.Clear();

// 4. 少し待ってから表示
Thread.Sleep(500);
nanoFramework.M5Stack.Console.WriteLine("Hello Fire!");

Thread.Sleep(Timeout.Infinite);
