using nanoFramework.M5Stack; // Fireクラスに必要
using System.Threading;
using System.Device.Pwm;
using nanoFramework.Hardware.Esp32;

Fire.InitializeScreen();
nanoFramework.M5Stack.Console.Clear();
nanoFramework.M5Stack.Console.WriteLine("Hello Fire!");

// 2. サーボ用ピン(26)の設定
// タイマー競合を避けるため PWM2 を指定
Configuration.SetPinFunction(26, DeviceFunction.PWM2);

using (PwmChannel servo = PwmChannel.CreateFromPin(26, 50))
{
    if (servo != null)
    {
        servo.Start();

        while (true)
        {
            // 0度
            Console.Clear();
            Console.WriteLine("Angle: 0 deg");
            servo.DutyCycle = 0.025;
            Thread.Sleep(2000);

            // 180度
            Console.Clear();
            Console.WriteLine("Angle: 180 deg");
            servo.DutyCycle = 0.125;
            Thread.Sleep(2000);
        }
    }
    else
    {
        Console.WriteLine("PWM Error!");
    }
}

Thread.Sleep(Timeout.Infinite);
