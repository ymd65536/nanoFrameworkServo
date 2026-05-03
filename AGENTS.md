# M5StackをnanoFrameworkで使う方法

このドキュメントでは、M5StackデバイスをnanoFrameworkで使用するための手順とガイドを提供します。

ユーザーとの会話には日本語で回答してください。

## nanoFramework プログラミングガイド

- 名前空間
- 標準出力

### 名前空間

M5StackをnanoFrameworkで使用するには、以下の名前空間をインポートしてください。

```csharp
using nanoFramework.M5Stack;
```

### 標準出力

Console.WriteLineメソッドを使用して標準出力ができます。

```csharp
Console.WriteLine("Hello, nanoFramework!");
```

M5Stackのスクリーンを使用する場合はInitializeScreenメソッドを呼び出してから、Console.Clearメソッドで画面をクリアします。以下の例はM5Stack Fireの場合です。

```csharp
Fire.InitializeScreen();
Console.Clear();
```

名前空間の`using System`を使うと、Consoleクラスが競合する可能性があります。

出力されるエラーの例

```text
error CS0104: 'Console' is an ambiguou
s reference between 'nanoFramework.M5Stack.Console' and 'System.Console'
```

エラーが発生した場合は、次の選択肢のいずれかを使用して解決してください。

PCのコンソールに出力する場合は、`using Console = System.Console;`のようにエイリアスを設定してください。

```csharp
using Console = System.Console;
```

M5Stackの画面に出力する場合は、`using Console = nanoFramework.M5Stack.Console;`のようにエイリアスを設定してください。

```csharp
using Console = nanoFramework.M5Stack.Console;
```

### Math

System.Math名前空間を使用する場合は、以下のようにインポートしてください。

```csharp
using Math = System.Math;
```

## M5StackをnanoFrameworkで使うための指示

- 前提
- デバイス情報を確認する
- ターゲットを確認する
- ブートローダの書き込み
- デプロイ
- パッケージのリストア

## 前提

- **必ず名前空間をインポートしてください** - `using nanoFramework.M5Stack;` をProgram.csの先頭に記載する必要があります。これがないとビルドエラーが発生します
- 最初にデバイス情報を確認してください
  - `ESP32_PSRAM_REV3`あるいは`ESP32_REV3`というターゲット名が表示された場合は`M5Core2`として認識してください
- macOSでプログラムをデプロイしてもうまくいかない可能性があります
- **デプロイにはM5Stackで動作する.binファイルを使用してください**。.exeファイルはM5Stackで動作しません

## デバイス情報を確認する

`--serialport`は、接続されているM5Stackデバイスのシリアルポートの番号を指定します。

実機で確認した例（ESP32_PSRAM_REV3でM5Core2扱い）：

```bash
nanoff --serialport COM6 --identifyfirmware
```

```bash
nanoff --serialport [シリアルポートの番号] --identifyfirmware
```

シリアルポート番号がわからない場合は、以下のコマンドで接続されているシリアルデバイスの一覧を表示できます。

```bash
nanoff --listports
```

## ターゲットを認識する

nanoff利用できるターゲットの一覧を表示するには、以下のコマンドを実行して確認してください。

```bash
nanoff --listtargets
```

## ブートローダの書き込み

M5Stackデバイスに対応したブートローダを書き込むには、以下のコマンドを使用します。
書き込みの際は、デバイスが接続されているシリアルポートを指定する必要があります。

デバイス情報を確認するセクションで確認したターゲット名を`--target`オプションに指定してください。

ターゲット名の一覧は`ターゲットを認識する`セクションを参照してください。

`[serialport]`には`nanoff --listports`で確認したシリアルポートを指定してください。

--fwversionはわからない場合は省略可能です。

```bash
nanoff --update --target M5Core2 --fwversion 1.14.0.179 --serialport [serialport] --baud 115200 --masserase
```

## ビルド

### 必須要件

ビルドを実行する前に、以下の確認をしてください：

- **Program.csに名前空間をインポート** - ファイルの先頭に `using nanoFramework.M5Stack;` を記載
- **VS Code拡張のパスを確認** - `~/.vscode/extensions` ディレクトリから `nanoframework.vscode-nanoframework-*` フォルダを確認し、バージョン番号を把握してください

### Windowsでのビルド

`dotnet msbuild` コマンドを使用しないで、Visual StudioのMSBuildを直接呼び出してビルドを行います。

NanoFrameworkProjectSystemPathは実際のextensionのパスに合わせて変更してください。
パス内のバックスラッシュ `\` とスラッシュ `/` の混在に注意してください。

以下のコマンド例です（実際のバージョンとユーザーパスに合わせて調整してください）。`GenerateDeploymentImage=true` を付与すると `bin\Debug\NFApp2.bin` が生成されます。

```bash
$path = & "${env:ProgramFiles(x86)}\microsoft visual studio\installer\vswhere.exe" -products * -latest -prerelease -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\amd64\MSBuild.exe | select-object -first 1; & $path "c:\Users\Yamada\Desktop\NFApp2\NFApp2\NFApp2.nfproj" -t:Rebuild -p:platform="Any CPU" /p:NanoFrameworkProjectSystemPath="C:\Users\Yamada\.vscode\extensions\nanoframework.vscode-nanoframework-1.0.185\dist\utils\nanoFramework\v1.0\" /p:GenerateDeploymentImage=true /p:Configuration=Debug -verbosity:minimal
```

従来のsln指定例（.exeのみ生成する場合。デプロイ用.binは出ません）:

```bash
$path = & "${env:ProgramFiles(x86)}\microsoft visual studio\installer\vswhere.exe" -products * -latest -prerelease -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\amd64\MSBuild.exe | select-object -first 1; nuget restore "c:\Users\[ユーザー名]\Desktop\NFApp2\NFApp2\NFApp2.sln"; & $path "c:\Users\[ユーザー名]\Desktop\NFApp2\NFApp2\NFApp2.sln" -p:platform="Any CPU" /p:NanoFrameworkProjectSystemPath="c:\Users\[ユーザー名]\.vscode\extensions\nanoframework.vscode-nanoframework-1.0.185\dist\utils\nanoFramework\v1.0\" -p:NFMDP_PE_Verbose=false -p:NFMDP_PE_VerboseMinimize=false -verbosity:minimal /p:OutDir="c:\Users\[ユーザー名]\Desktop\NFApp2\NFApp2\bin\Debug"
```

**注意：**
- `[ユーザー名]` を実際のユーザー名に置き換えてください
- VS Code拡張のバージョン番号 `1.0.185` が異なる場合は、実際の番号に置き換えてください
- このコマンドは `bin\Debug\` ディレクトリに `NFApp2.exe` を生成します

### ビルド成功の確認

ビルドが成功すると、以下のファイルが生成されます：
- `bin\Debug\NFApp2.exe` - 中間生成ファイル（M5Stackへのデプロイには使用できません）
- `bin\Debug\NFApp2.pdbx` - デバッグシンボルファイル
- `bin\Debug\` 配下の多数のDLLファイル - 依存ライブラリ

**注意：** M5Stackへのデプロイには、VS Code拡張が生成する.binファイルが必要です。.exeファイルはM5Stackで動作しません

ビルドが失敗したら、作業を中断してください。

## デプロイ

M5Stackのデプロイには、nanoffコマンドを使用します。VS Code拡張が生成した .bin ファイルをM5Stack デバイスにデプロイします。

### シリアルポート番号（COM番号）の確認

デプロイ前に、接続されているM5Stackデバイスのシリアルポート番号を確認してください。

```bash
nanoff --listports
```

デバイス情報を確認するセクションで確認したターゲット名を確認し、以下のコマンド例を参考にしてください。

### Windowsでのデプロイ

M5Stackへのデプロイには、VS Code拡張が生成する `.bin` ファイルを使用してください。.exeファイルではM5Stackで動作しません。

実際の成功例（M5Core2相当デバイス、COM6、ターゲット未指定で`--nanodevice --deploy`を併用）：

```bash
nanoff --nanodevice --deploy --serialport COM6 --image "c:\Users\Yamada\Desktop\NFApp2\NFApp2\bin\Debug\NFApp2.bin"
```

`--nanodevice` と `--target` の併用はエラーになるため、nanodevice指定に統一してください。

```bash
nanoff --nanodevice --deploy --serialport COM6 --image "c:\Users\[ユーザー名]\Desktop\NFApp2\NFApp2\bin\Debug\NFApp2.bin"
```

**置き換えるもの：**
- `COM6` - 確認したシリアルポート番号に置き換えてください
- `[ユーザー名]` - 実際のユーザー名に置き換えてください
- `M5Core2` - 必要に応じてターゲット名を変更してください

### デプロイ成功の確認

デプロイが成功すると、以下のメッセージが表示されます：
```
Wrote ... bytes ... at ... in ... s
Hash of data verified.
Hard resetting via RTS pin...
OK
```

M5Stackのスクリーンに設定されたアプリケーション出力が表示されます

## 端末のリセット

`[serialport]`には`nanoff --listports`で確認したシリアルポートを指定してください。

```bash
nanoff --serialport [serialport] --reset
```

## デバイスの詳細情報を確認する

```bash
nanoff --serialport [serialport] --devicedetails
```

### パッケージのリストア

```bash
nuget restore "c:\Users\Yamada\Desktop\NFApp2\NFApp2\NFApp2.sln"
```

## トラブルシューティング

### M5CoreとM5Core2パッケージについて

M5StackのM5Core2デバイスを使用する場合は、`nanoFramework.M5Core`パッケージを使用してください。なお、`nanoFramework.M5Stack`パッケージは廃止されています。

`nanoFramework.M5Core2`を使用すると、M5Core2デバイスでビルドエラーやデプロイエラー（`Error: a3000000`）が発生する可能性があります。M5Core2デバイスを使用する場合は、`nanoFramework.M5Core`パッケージを使用してください。

### .NET nanoFrameworkのバージョン不整合

.NET nanoFrameworkのバージョンがプロジェクトで使用されているバージョンと一致していない場合、ビルドやデプロイに失敗する可能性があります。

M5Core2においてアセンブリのロードエラー（`Error: a3000000`）や、デプロイ後の「Assemblies count is 0」という不具合を回避するための構成案です。

### 1. 動作確認済みの安定構成

メジャーアップデート（v2.0.0-preview）による依存関係の混乱を避け、実績のある **v1.x 系列** で統一します。

| 項目 | 指定バージョン / 値 |
| :--- | :--- |
| **Firmware (CLR)** | `1.16.0.568` |
| **Native mscorlib** | `v100.5.0.24` |
| **NuGet: CoreLibrary** | `1.17.11` |
| **NuGet: M5Core2** | `1.1.291` (安定版) |

### 2. ファームウェアの書き込み（リカバリ手順）

ファームウェアを特定のバージョンにダウングレードすることで、安定した環境を構築できます。以下のコマンド例は、M5Core2にファームウェアバージョン `1.16.0.568` を書き込む方法です。

```bash
nanoff --update --target M5Core2 --fwversion 1.16.0.568 --serialport COM6 --baud 115200 --masserase
```

### 廃止となったパッケージ

2026年4月現在、`nanoFramework.M5Stack`は廃止されており、代わりに`
nanoFramework.M5Core`を使用することが推奨されています。

- [nanoFramework.M5Stack](https://www.nuget.org/packages/nanoFramework.M5Stack/)
  - 廃止されたパッケージ
- [nanoFramework.M5Core](https://www.nuget.org/packages/nanoFramework.M5Core/)
  - 推奨のパッケージ

### CS0103: 名前が現在のコンテキストに存在しません

**エラーメッセージ：**
```
error CS0103: The name 'Fire' does not exist in the current context
error CS0103: The name 'Console' does not exist in the current context
```

**原因：** Program.csに必要な名前空間がインポートされていません。

**解決方法：** Program.csの先頭に以下の行を追加してください。

```csharp
using nanoFramework.M5Stack;
```

### デプロイファイルが見つからない

**エラーメッセージ：**
```
Error E2002: Error executing operation with nano device. 
(Couldn't find the deployment file at the specified path.)
```

**原因：** 指定されたパスに .exe または .bin ファイルが存在しません。

**解決方法：**
1. ビルドが正常に完了していることを確認してください（`bin\Debug\NFApp2.exe` が存在するか確認）
2. デプロイコマンドのパスが正しいことを確認してください
3. パスにスペースが含まれている場合は、ダブルクォーテーションで囲んでください

### System.Drawing名前空間が競合する場合

nanoFramework.Graphics 名前空間とSystem.Drawing名前空間が競合する場合があります。
**解決方法：** nanoFramework.Graphicsをインストールされているパッケージから削除してください。NFApp2.nfprojを修正し、nugetで再インストールしてください。

### nanoFramework系のパッケージが競合する場合

複数のnanoFramework系パッケージが競合する場合があります。
この場合はSystemのパッケージを利用するようにし、nanoFrameworkのパッケージを削除するようにしてください。

## IP5306の使用方法

IP5306はM5Stackの電源管理ICで、バッテリーの充電や電源供給を制御します。
サンプルコードは以下のようになります。

```csharp
using System.Device.I2c;
using Iot.Device.Ip5306;

I2cDevice i2cSettings = new(new I2cConnectionSettings(1, Ip5306.DefaultI2cAddress));
Ip5306 power = new(i2cSettings);

// Configuration for M5Stack
power.ButtonOffEnabled = true;
power.BoostOutputEnabled = false;
power.AutoPowerOnEnabled = true;
power.ChargerEnabled = true;
power.BoostEnabled = true;
power.LowPowerOffEnabled = true;
power.FlashLightBehavior = ButtonPress.Doubleclick;
power.SwitchOffBoostBehavior = ButtonPress.LongPress;
power.BoostWhenVinUnpluggedEnabled = true;
power.ChargingUnderVoltage = ChargingUnderVoltage.V4_55;
power.ChargingLoopSelection = ChargingLoopSelection.Vin;
power.ConstantChargingVoltage = ConstantChargingVoltage.Vm28;
power.ChargingCuttOffVoltage = ChargingCutOffVoltage.V4_17;
power.LightDutyShutdownTime = LightDutyShutdownTime.S32;
power.ChargingCutOffCurrent = ChargingCutOffCurrent.C500mA;
power.ChargingCuttOffVoltage = ChargingCutOffVoltage.V4_2;
```

参考

- [Ip5306 クラス](https://learn.microsoft.com/ja-jp/dotnet/api/iot.device.ip5306.ip5306?view=iot-dotnet-latest)
- [Iot.Device.Ip5306](https://docs.nanoframework.net/devices/Iot.Device.Ip5306.Ip5306.html)
- [IP5306 - Power management](https://github.com/nanoframework/nanoframework.IoT.Device/blob/main/devices/Ip5306/README.md/)
- [I2cConnectionSettings クラス](https://learn.microsoft.com/ja-jp/dotnet/api/system.device.i2c.i2cconnectionsettings?view=iot-dotnet-latest)

## LED(Sk6812)ユニットの使用方法

M5StackのユニットにはSk6812というLEDユニットがあります。

- [RGB LED Unit (SK6812)– m5stack-store](https://docs.m5stack.com/en/unit/rgb)

以下のコードはSk6812をM5StackのPORT Bに接続してLEDを制御する例です。

```csharp
using System.Drawing;
using System.Threading;
using Iot.Device.Ws28xx.Esp32;
using nanoFramework.M5Stack;
using Console = nanoFramework.M5Stack.Console;

// PORT BのGPIO 26を使用するように設定
int pinNumber = 26;
int ledCount = 3; // 接続しているLEDの数

// SK6812のインスタンス作成
// RGBモデルかRGBWモデルかに合わせてクラスを選びます
var ledStrip = new Sk6812(pinNumber, ledCount);

// 全体を赤色にする例
// 1番目のLEDを赤、2番目を緑、3番目を青にする
ledStrip.Image.SetPixel(0, 0, Color.Red);
ledStrip.Image.SetPixel(1, 0, Color.Green);
ledStrip.Image.SetPixel(2, 0, Color.Blue);

// 反映
ledStrip.Update();

Color dimRed = Color.FromArgb(25, 0, 0);
ledStrip.Image.SetPixel(0, 0, dimRed);
ledStrip.Image.SetPixel(1, 0, dimRed);
ledStrip.Image.SetPixel(2, 0, dimRed);
ledStrip.Update();

M5Core.InitializeScreen();
Console.Clear();

Console.WriteLine("Hello from nanoFramework!");
Console.WriteLine("on M5Stack with Sk6812 LEDs!");
Thread.Sleep(Timeout.Infinite);
```
