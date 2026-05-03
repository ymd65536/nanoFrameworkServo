using nanoFramework.M5Stack; // Fireクラスに必要
using System.Threading;

Fire.InitializeScreen();
nanoFramework.M5Stack.Console.Clear();
nanoFramework.M5Stack.Console.WriteLine("Hello Fire!");

Thread.Sleep(Timeout.Infinite);
