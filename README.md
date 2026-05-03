## Overview

このリポジトリはnanoFrameworkのプロジェクトをVisual Studio Code(VSCode)で使うためのリポジトリテンプレートです。`Use This Template`をクリックして、VSCodeでプロジェクトを作成してください。

## Target

このプロジェクトはM5Stack Core2をターゲットにしています。
M5Stack Core2はESP32を搭載したマイクロコントローラーで、nanoFrameworkを使用してプログラムできます。また、M5Stack Fire v2.7も同様にESP32を搭載しているため、同じプロジェクトで動作します。

具体的には以下の環境で動作します。

- OS
  - Windows 11 Pro 25H2
  - OS build 26200.8246

※macOS(Macbook Apple Silicon)ではビルドは可能ですが、ビルドしたファイルはM5Stack Core2やM5Stack Fire v2.7では動作しません。

- hardware
  - [M5Stack Core2](https://docs.m5stack.com/ja/core/core2)
  - [M5Stack Fire v2.7](https://docs.m5stack.com/ja/core/fire)

- deploy
  - nanoff 2.5.143+9f97ac82d9  

- runtime
  - dotnet --version
  - 10.0.107

- VSCode Extension
  - nanoframework.vscode-nanoframework

## Setup bootloader

この手順ではnanoFrameworkでビルドしたプログラムを動かすためのブートローダをM5Stackデバイスに書き込む方法を説明します。

M5Stackデバイスに対応したブートローダを書き込むには、以下のコマンドを使用します。
書き込みの際は、デバイスが接続されているシリアルポートを指定する必要があります。

デバイス情報を確認するセクションで確認したターゲット名を`--target`オプションに指定してください。

ターゲット名の一覧は`ターゲットを認識する`セクションを参照してください。

`[serialport]`には`nanoff --listports`で確認したシリアルポートを指定してください。

```bash
nanoff --update --target M5Core2 --fwversion 1.16.0.568 --serialport [serialport] --baud 115200 --masserase
```

--fwversionはわからない場合は`--listtargets`で確認してください。具体的には以下のコマンドを実行して、ターゲット名と対応するブートローダのバージョンを確認してください。

```bash
nanoff --listtargets
```

実行結果

```text
  M5Core2
    1.16.0.568
    1.16.0.567
    1.16.0.563
```

## Setup

VSCodeでnanoFrameworkのプロジェクトを作成するには、まずは`nanoff`というコマンドラインツールをインストールする必要があります。以下のコマンドを実行して、`nanoff`をインストールしてください。（インストールのため最初の一度だけ実行）

```bash
dotnet tool install -g nanoff
```

次に、VSCodeでプロジェクトを作成するための拡張機能をインストールします。VSCodeの拡張機能マーケットプレイスで`nanoframework.vscode-nanoframework`を検索して、インストールしてください。

最後にプロジェクトを設定するために`nfproj`と`sln`を修正します。

まずは`nfproj`の`RootNamespace`と`AssemblyName`を変更します。

```xml
<RootNamespace>nanoFrameworkM5StackTemplate</RootNamespace>
<AssemblyName>nanoFrameworkM5StackTemplate</AssemblyName>
```

`sln`では`nfproj`ファイルが読み込まれます。`nfproj`ファイルの名前を変更してください。
以下の例では`nanoFrameworkM5StackTemplate.nfproj`を`MyProject.nfproj`に変更しています。

変更前

```sln
Project("{11A8DD76-328B-46DF-9F39-F559912D0360}") = "nanoFrameworkM5StackTemplate", "nanoFrameworkM5StackTemplate.nfproj", "{AE52F6E3-F33F-4818-A982-44922EA0060F}"
```

変更後

```sln
Project("{11A8DD76-328B-46DF-9F39-F559912D0360}") = "MyProject", "MyProject.nfproj", "{AE52F6E3-F33F-4818-A982-44922EA0060F}"
```

これでプロジェクトのセットアップは完了です。VSCodeでプロジェクトを開いて、ビルドやデプロイを行うことができます。

## Usage

VSCodeでは主にnanoffを使用してビルドやデプロイを行います。

おおまかな流れは以下の通りです。

1. 拡張機能によるビルド
2. シリアルポート番号（COM番号）の確認
3. nanoffでデプロイ

## Build

VSCodeの拡張機能を使用してビルドするには、コマンドパレットを開いて、`nanoFramework: Build Project`を選択してください。

ビルドする`sln`ファイルを選択するように求められます。プロジェクトのルートディレクトリにある`sln`ファイルを選択してください。

`debug`や`release`などのビルド構成を選択するように求められます。通常は`debug`を選択してください。


ビルドが成功すると、以下のファイルが生成されます：
- `bin\Debug\NFApp2.exe` - 中間生成ファイル（M5Stackへのデプロイには使用できません）
- `bin\Debug\NFApp2.pdbx` - デバッグシンボルファイル
- `bin\Debug\` 配下の多数のDLLファイル - 依存ライブラリ

**注意：** M5Stackへのデプロイには、VS Code拡張が生成する.binファイルが必要です。.exeファイルはM5Stackで動作しません


## Show serial port

デプロイ前に、接続されているM5Stackデバイスのシリアルポート番号を確認してください。

確認する方法は2つあります。

コマンドのみを使用して確認する。

```bash
nanoff --listports
```

デバイス名を含めて確認する。

```bash
nanoff --listdevices
```

実行結果

```text
.NET nanoFramework Firmware Flasher v2.5.143+9f97ac82d9
Copyright (C) 2019 .NET Foundation and nanoFramework project contributors


-- Connected .NET nanoFramework devices --
M5Core2 @ COM6
```

表示されているシリアルポート番号をデプロイの際に使用してください。

## Deploy

M5Stackのデプロイには、nanoffコマンドを使用します。VS Code拡張が生成した .bin ファイルをM5Stack デバイスにデプロイします。

```bash
nanoff --nanodevice --deploy --serialport COM6 --image .\bin\Debug\nanoFrameworkM5StackTemplate.bin
```

## Troubleshooting

うまく動作しないあるいはデバイスの情報を確認することで問題の原因を特定できることがあります。以下のコマンドを実行して、デバイス情報を確認してください。

なお、コマンドを実行するとデバイスがリセットされるため、デバイスがリセットされても問題ない状態で実行してください。

```bash
nanoff --serialport COM6 --identifyfirmware
```

実行結果

```text
.NET nanoFramework Firmware Flasher v2.5.143+9f97ac82d9
Copyright (C) 2019 .NET Foundation and nanoFramework project contributors


Reading details from chip...OK

Connected to:
ESP32 (ESP32-D0WDQ6-V3 (revision v3.1))
Features Wi-Fi, BT, Dual Core + LP Core, 240MHz, Vref calibration in eFuse, Coding Scheme None
Flash size 16MB unknown from  (manufacturer 0x46 device 0x1840)
PSRAM: undetermined
Crystal 40MHz
MAC 3C:8A:1F:D6:0A:74


Target 'ESP32_REV3' best matches the device characteristics.
Target: ESP32_REV3
```

### CS0518 エラー（基本型が見つからない）について

ビルド時に以下のようなエラーが発生する場合があります：

```text
CSC : error CS0518: Predefined type 'System.Void' is not defined or imported
CSC : error CS0518: Predefined type 'System.String' is not defined or imported
```

**原因:** `nanoFrameworkM5StackTemplate.nfproj` ファイル内の `mscorlib.dll` への参照パスが正しくないために発生します。

**症状:** 
- System.Void、System.String、System.Object などの基本型が見つからないというエラー
- AssemblyInfo.cs 内のアトリビュート（AssemblyTitle、AssemblyVersion など）が見つからないエラー
- System.Threading、System.Reflection などの名前空間が存在しないというエラー

**解決方法:**

`.nfproj` ファイルをテキストエディタで開き、`mscorlib` の参照パスを修正してください。

**誤ったパス:**
```xml
<Reference Include="mscorlib">
  <HintPath>packages\nanoFramework.CoreLibrary.2.0.0-preview.43\lib\mscorlib.dll</HintPath>
</Reference>
```

**正しいパス:**
```xml
<Reference Include="mscorlib">
  <HintPath>packages\nanoFramework.CoreLibrary.2.0.0-preview.43\lib\netnano1.0\mscorlib.dll</HintPath>
</Reference>
```

パスに `\lib\netnano1.0\` を含める必要があります。CoreLibrary のバージョン番号は実際のプロジェクトに合わせて変更してください。

修正後、プロジェクトを再ビルドしてください。

## About M5Stack Fire

M5Stack Fireを動かす場合の注意点について記載します。

### パッケージの競合について

M5Stack Fireを使用する場合、`nanoFramework.M5Core`と`nanoFramework.Fire`パッケージの両方を同時に参照すると`nanoFramework.Fire`パッケージしかないメソッドやクラスが存在するため、ビルドエラーが発生します。※M5Coreが先に参照されるため

### 解決方法

M5Stack Fireを使用する場合は、以下の対応が必要です：

1. **`nanoFramework.M5Core`パッケージを削除する**
   - `nanoFrameworkM5StackTemplate.nfproj`から`nanoFramework.M5Core`の参照を削除
   - `packages.config`から`nanoFramework.M5Core`のエントリを削除

2. **`nanoFramework.Fire`パッケージのみを使用する**
   - プロジェクトファイルには`nanoFramework.Fire`パッケージの参照のみを残す

3. **Program.csで正しい名前空間を使用する**
   ```csharp
   using nanoFramework.M5Stack;
   using Console = nanoFramework.M5Stack.Console;
   using Fire = nanoFramework.M5Stack.Fire;
   ```

4. **存在しない名前空間をインポートしない**
   - `using nanoFramework.Fire;`という名前空間は存在しないため、記載しない
   - `Fire`クラスは`nanoFramework.M5Stack`名前空間に含まれています

### 正しいプロジェクト構成

M5Stack Fire用のプロジェクトでは、以下の構成にしてください：

- **nanoFrameworkM5StackTemplate.nfproj**: `nanoFramework.Fire`パッケージのみを参照
- **packages.config**: `nanoFramework.Fire`パッケージのみを含める（`nanoFramework.M5Core`は含めない）
- **Program.cs**: `nanoFramework.M5Stack`名前空間を使用


## ビルド時の MSB3277 警告（バージョン競合）について

ビルド時に nanoFramework.Graphics.Core などのライブラリでバージョンの不一致（MSB3277）が発生した場合は、以下の手順で依存関係をクリーンアップしてください。

原因: nanoFramework.M5Core や Iot.Device.Ws28xx.Esp32 など、複数のライブラリが異なるバージョンの Graphics.Core を参照しているために発生します。

対処法:NuGetパッケージの更新

Visual Studio の NuGet マネージャーから、すべてのパッケージを最新バージョンに更新します。特に依存元となるコアライブラリのバージョンを揃えることが重要です。

プロジェクトファイルの修正:

.nfproj ファイルをテキストエディタで開き、<PropertyGroup> セクションに以下の設定を追加して、アセンブリのリダイレクトを有効にします。

```xml
<AutoGenerateBindingRedirects>true</AutoGenerateBindingRedirects>
```

ビルドキャッシュの削除としてVisual Studio を閉じ、プロジェクト直下の bin および obj フォルダを物理削除してから再ビルドを行ってください。
