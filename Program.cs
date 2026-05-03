using System;
using System.Threading;
using nanoFramework.M5Stack;
using Console = nanoFramework.M5Stack.Console;

M5Core.InitializeScreen();
Console.Clear();

Console.WriteLine("Hello from nanoFramework!");
Thread.Sleep(Timeout.Infinite);
