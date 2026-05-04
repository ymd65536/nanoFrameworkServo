using System;
using System.Threading;
using System.Device.Pwm;
using nanoFramework.Hardware.Esp32;
using nanoFramework.M5Stack;

using Console = nanoFramework.M5Stack.Console;

// 1. 画面初期化（ここは通っているはず）
Fire.InitializeScreen();
Console.Clear();
Console.WriteLine("Step 1: Init Screen OK");
Thread.Sleep(500);

int verticalPinNumber = 26;
int frequency = 50;

try
{
    Configuration.SetPinFunction(verticalPinNumber, DeviceFunction.PWM1);

    // 3. チャンネル作成
    // CreateFromPinがNullを返すならタイマーの空きがない
    Console.WriteLine("Step 3: Create Channel...");
    using (PwmChannel servo = PwmChannel.CreateFromPin(verticalPinNumber, frequency))
    {
        Console.WriteLine("Step 4: PWM Start!");
        while (true)
        {
            servo.Start();
            servo.DutyCycle = 0.025;
            Console.WriteLine("DutyCycle: " + servo.DutyCycle);
            Thread.Sleep(2000);
            Console.Clear();

            servo.DutyCycle = 0.035;
            Console.WriteLine("DutyCycle: " + servo.DutyCycle);
            Thread.Sleep(2000);
            Console.Clear();

            servo.DutyCycle = 0.045;
            Console.WriteLine("DutyCycle: " + servo.DutyCycle);
            Thread.Sleep(2000);
            Console.Clear();

            servo.DutyCycle = 0.055;
            Console.WriteLine("DutyCycle: " + servo.DutyCycle);
            Thread.Sleep(2000);
            Console.Clear();

            servo.DutyCycle = 0.025;
            Console.WriteLine("DutyCycle: " + servo.DutyCycle);
            Thread.Sleep(2000);
            Console.Clear();

            Thread.Sleep(500);
            servo.Stop();

        }
    }
}
catch (Exception ex)
{
    // 例外が発生したら型名とメッセージを画面に出す
    Console.WriteLine("Ex: " + ex.GetType().Name);
    Console.WriteLine(ex.Message);
}

Thread.Sleep(Timeout.Infinite);
