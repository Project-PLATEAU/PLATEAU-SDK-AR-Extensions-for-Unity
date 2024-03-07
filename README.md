# PLATEAU SDK-AR-Extensions for Unity 利用マニュアル

https://github.com/Project-PLATEAU/PLATEAU-SDK-AR-Extensions-for-Unity/assets/127069970/1bb0cea8-c388-41fc-abcc-8462fa8473fd

PLATEAUの3D都市モデルを使ったARアプリケーション開発を行うための支援機能を提供します。

- PLATEAU SDK-AR-Extensions for Unityで提供される機能
    - Geospatial API を用いたAR空間での3D都市モデルの位置合わせ
        - インポート (PLATEAU SDK) もしくはストリーミング (Cesium for Unity) により配置された3D都市モデルを、ビルドされたAR空間内で実際の建物の位置に配置します。
        - 3D都市モデルの位置がずれる場合にUIにより位置を調整することができます。
        - ARマーカー（現実空間に配置された特定の画像をカメラで認識させて位置を取得する機能）を用いて位置を調整することができます。
    - オクルージョンのためのアセットや調整機能
        - AR空間内で3D都市モデルを用いて3Dオブジェクトを遮蔽する機能をオクルージョンと呼びます。
            - 参考: [PLATEAU Tutorials TOPIC14｜VR・ARでの活用](https://www.mlit.go.jp/plateau/learning/tpc14-2/#p14_2_7)
        - AR Extensions ではオクルージョンマスクを設定するためのマテリアルやスクリプトを用意しています。

[ダウンロードリンクはこちら](https://github.com/Project-PLATEAU/PLATEAU-SDK-AR-extensions-for-Unity/releases/tag/v0.1.0)
     
### 更新履歴
| 更新日時 | 更新内容 |
| :--- | :--- |
|  2024/01/30  |  テンプレートプロジェクトの公開　|
|  2023/12/25  |  マーカーによるPLATEAUモデルの位置合わせ機能　|
|  2023/10/28  |  AR Extensions 初回リリース |
|  2024/2/20   |  インストール方法に関する手順をREADMEに追記 |

     
# 目次


<!-- @import "[TOC]" {cmd="toc" depthFrom=1 depthTo=6 orderedList=false} -->

<!-- code_chunk_output -->

- [PLATEAU SDK-AR-Extensions for Unity 利用マニュアル](#plateau-sdk-ar-extensions-for-unity-利用マニュアル)
    - [更新履歴](#更新履歴)
- [目次](#目次)
- [検証済環境](#検証済環境)
    - [OS環境](#os環境)
    - [Unity バージョン](#unity-バージョン)
    - [レンダリングパイプライン](#レンダリングパイプライン)
    - [PLATEAU SDK バージョン](#plateau-sdk-バージョン)
- [事前準備](#事前準備)
  - [AR用テンプレートプロジェクトのセットアップ](#ar用テンプレートプロジェクトのセットアップ)
  - [PLATEAU SDK-Toolkits for Unity のインストール](#plateau-sdk-toolkits-for-unity-のインストール)
  - [Google ARCore Extensions のインストール](#google-arcore-extensions-のインストール)
  - [PLATEAU SDK-AR-Extensions for Unity のインストール](#plateau-sdk-ar-extensions-for-unity-のインストール)
- [利用手順](#利用手順)
  - [1. サンプルを用いたARアプリケーションの体験](#1-サンプルを用いたarアプリケーションの体験)
    - [1-1. AR Extensions サンプルのインポート](#1-1-ar-extensions-サンプルのインポート)
    - [1-2. サンプルシーンを設定する](#1-2-サンプルシーンを設定する)
    - [1-3. PLATEAU SDKでインポートした3D都市モデルを設定する](#1-3-plateau-sdkでインポートした3d都市モデルを設定する)
    - [1-4. Geospatial API (ARCore Extensions) の設定](#1-4-geospatial-api-arcore-extensions-の設定)
    - [1-5. ビルド設定にシーンを追加する](#1-5-ビルド設定にシーンを追加する)
    - [1-6. アプリケーションをビルドして端末にインストールする](#1-6-アプリケーションをビルドして端末にインストールする)
    - [1-7. ARサンプルシーンの操作方法](#1-7-arサンプルシーンの操作方法)
  - [2. ウィンドウの利用方法](#2-ウィンドウの利用方法)
    - [2-1. 3D都市モデルのマテリアル変更](#2-1-3d都市モデルのマテリアル変更)
  - [3. 3D都市モデルのAR空間内位置合わせ機能](#3-3d都市モデルのar空間内位置合わせ機能)
    - [3-1. `PlateauARPositioning` を設定する](#3-1-plateauarpositioning-を設定する)
    - [3-2. PLATEAU SDKでインポートした3D都市モデルを使用する場合](#3-2-plateau-sdkでインポートした3d都市モデルを使用する場合)
    - [3-3. PLATEAUストリーミング(Cesium for Unity)を使用する場合](#3-3-plateauストリーミングcesium-for-unityを使用する場合)
    - [3-4. 手動位置合わせ機能](#3-4-手動位置合わせ機能)
    - [3-5. ARマーカーによる高さ合わせ機能](#3-5-arマーカーによる高さ合わせ機能)
  - [4. ARマーカーを使った3D都市モデルの位置合わせ機能](#4-arマーカーを使った3d都市モデルの位置合わせ機能)
    - [4-1. 事前準備](#4-1-事前準備)
    - [4-2. ARマーカー位置合わせコンポーネントの設定](#4-2-arマーカー位置合わせコンポーネントの設定)
    - [4-3. ARマーカーの位置設定](#4-3-arマーカーの位置設定)
    - [4-4. 読み取りに使用するARマーカーの用意](#4-4-読み取りに使用するarマーカーの用意)
    - [4-3. 動作確認](#4-3-動作確認)
  - [5. ARオクルージョン機能の利用方法](#5-arオクルージョン機能の利用方法)
    - [5-1. 遮蔽オブジェクトマテリアルの作成](#5-1-遮蔽オブジェクトマテリアルの作成)
    - [5-2. レイヤーの作成](#5-2-レイヤーの作成)
    - [5-3. URP描画設定を開く](#5-3-urp描画設定を開く)
    - [5-4. URP描画設定にレイヤーを設定](#5-4-urp描画設定にレイヤーを設定)
    - [5-5. Plateau AR Occlusion Renderer Featureの追加](#5-5-plateau-ar-occlusion-renderer-featureの追加)
    - [5-6. 遮蔽するオブジェクトのレイヤーを変更](#5-6-遮蔽するオブジェクトのレイヤーを変更)
    - [5-7. 遮蔽するオブジェクトのマテリアルをZWriteに変更](#5-7-遮蔽するオブジェクトのマテリアルをzwriteに変更)
    - [5-8. 遮蔽されるオブジェクトのレイヤーを変更](#5-8-遮蔽されるオブジェクトのレイヤーを変更)
- [ライセンス](#ライセンス)
- [注意事項/利用規約](#注意事項利用規約)

<!-- /code_chunk_output -->

# 検証済環境
### OS環境
- Windows11
- macOS Ventura 13.2
- Android 13
- iOS 16.7.1

### Unity バージョン
- Unity 2021.3.35


### レンダリングパイプライン
- URP (Universal Rendering Pipeline)

> [!Warning]
> HDRP (High Definition Rednering Pipeline) および Built-in Rendering Pipeline では動作しません。

### PLATEAU SDK バージョン
- [version 2.3.2](https://github.com/Synesthesias/PLATEAU-SDK-for-Unity/releases/tag/v2.3.2)

# 事前準備

## AR用テンプレートプロジェクトのセットアップ

本リポジトリでは構築済みのテンプレートプロジェクトを提供しています。以下の利用手順ではこのテンプレートプロジェクトを基本としたARアプリの構築手順を説明します。

AR Extensions リポジトリの “/~Templates” ディレクトリの中にある “AR Template” ディレクトリを任意の場所にコピーし、Unity Hub でコピーしたプロジェクトを開いてください。<br>
テンプレートプロジェクトの詳細は[こちら](https://github.com/Project-PLATEAU/PLATEAU-SDK-AR-Extensions-for-Unity/blob/main/Templates~/PLATEAU%20AR%20Unity%20Project/README.md)のドキュメントをご参照ください。

テンプレートプロジェクトのUnityバージョンは2021.3.35f1です。手元にない場合は Unity Hub よりインストールしてください。

AR用テンプレートプロジェクトを利用せず、既存のプロジェクト等で PLATEAU SDK-AR-Extensions for Unity を利用する場合は以降の手順を参考に各種パッケージをインストールしてください。

## PLATEAU SDK-Toolkits for Unity のインストール

AR Extensions は PLATEAU SDK-Toolkits for Unity の機能を利用しているため、AR Extensionsを利用するためにはこちらをインストールする必要があります。

[こちら](https://github.com/Project-PLATEAU/PLATEAU-SDK-Toolkits-for-Unity#3-plateau-sdk-toolkits-for-unity-%E3%81%AE%E3%82%A4%E3%83%B3%E3%82%B9%E3%83%88%E3%83%BC%E3%83%AB)を参照して PLATEAU SDK-Toolkits for Unity をインストールしてください。

## Google ARCore Extensions のインストール

PLATEAU SDK-AR-Extensions では Google ARCore Extensions の機能である Geospatial API を利用するため、こちらをインストールする必要があります。

Unityプロジェクトに Google ARCore Extensions をインストールする方法は公式ドキュメントに記載されているため、[こちら](https://developers.google.com/ar/develop/unity-arf/getting-started-extensions?hl=ja)を参照して Google ARCore Extensions をインストールしてください。

## PLATEAU SDK-AR-Extensions for Unity のインストール

1. Unityエディターを開き、「Window」メニューから「Package Manager」を選択します。
2. 「Package Manager」ウィンドウの左上にある「＋」ボタンをクリックし、「Add package from tarball...」を選択します。
3. ファイル選択ダイアログで PLATEAU SDK-AR-Extensions for Unity パッケージの tarball (.tgzファイル) を選択します。

<img width="493" alt="ar_tgz_install" src="/Documentation~/Images/ar_tgz_install.png">

[ダウンロードリンクはこちら](https://github.com/Project-PLATEAU/PLATEAU-SDK-Maps-Toolkit-for-Unity/releases)

# 利用手順

## 1. サンプルを用いたARアプリケーションの体験

<img width="600" alt="ar_manual_1_AR-Streaming" src="/Documentation~/Images/ar_manual_1_AR-Streaming.jpeg">

AR Extensions では各機能を使用したサンプルアセットおよび構築済みのARシーンを提供しています。このサンプルに含まれるシーンを用いることで、PLATEAUの3D都市モデルを使ったARアプリケーションをすぐに体験することができます。  
また、構築済みのアセットを見ることで、各機能の具体的な使い方を理解することもできます。

### 1-1. AR Extensions サンプルのインポート

メニューからPackage Managerを開き、AR Extensions のサンプルをインポートしてください。

<img width="600" alt="ar_manual_1_3_packagemanager" src="/Documentation~/Images/ar_manual_1_3_packagemanager.png">

インポートされたサンプルは “Assets/Samples” ディレクトリに配置されます。以下の手順では Assets/Samples/PLATEAU AR Extensions for Unity/${AR Extensions バージョン}/AR Samples のディレクトリを「サンプルフォルダ」とします。

<img width="400" alt="ar_manual_2_arsample_hierarchy" src="/Documentation~/Images/ar_manual_2_arsample_hierarchy.png">


### 1-2. サンプルシーンを設定する

サンプルフォルダの Scenes ディレクトリには以下のシーンが含まれています。

- Sample01_PlateauSdkAR.unity
    - PLATEAU SDKでインポートした3D都市モデルを用いたARシーンです。
    - 銀座の周辺の3D都市モデルを用いたサンプルとなっているため、このシーンを銀座以外の場所で利用する際には別途3D都市モデルをインポートして差し替える必要があります。
- Sample02_PlateauCesiumAR.unity
    - ストリーミングにより配置されたPLATEAUの3D都市モデル（3DTiles）を用いたARシーンです。
    - ストリーミングでは対象の地域を選択する必要がありますが、サンプルでは地域を選択するUIを用意しています。
    - ストリーミングを用いた3D都市モデルの利用方法については[PLATEAU SDK Maps Toolkit](https://github.com/Project-PLATEAU/PLATEAU-SDK-Maps-Toolkit-for-Unity)を参照してください。
- Boot.unity
    - シーン選択のサンプルとしてBootシーンを用意しています。このシーンを起動シーンに選択し、他のシーンを合わせて登録するとシーン選択画面を表示し、任意のシーンを起動することができます。

<img width="400" alt="ar_manual_4_selectui" src="/Documentation~/Images/ar_manual_4_selectui.png">

### 1-3. PLATEAU SDKでインポートした3D都市モデルを設定する

以下のドキュメントを参考にPLATEAU SDKでARアプリケーションを利用する付近の3D都市モデルをインポートします。

**都市モデルのインポート**

- [PLATEAU SDK for Unity | Manual](https://project-plateau.github.io/PLATEAU-SDK-for-Unity/manual/ImportCityModels.html)

インポートが完了したら、「3D都市モデルのAR空間内位置合わせ機能」の項目を参考に、位置合わせコンポーネント ( `PlateauARPositioning` )を設定してください。

### 1-4. Geospatial API (ARCore Extensions) の設定

Geospatial API を利用するためには、 Google Cloud プロジェクトを用意し、ARCore API の認証を設定する必要があります。ARCore API を有効化し、API認証を設定することで端末から Geospatial API を利用することができるようになります。設定方法については別のドキュメントにて解説されているため、そちらを参考に設定してください。

****PLATEAU Tutorials TOPIC14｜VR・ARでの活用[3/3]｜Google Geospatial APIで位置情報による3D都市モデルのARを作成する****

- APIキーの作成
    - [https://www.mlit.go.jp/plateau/learning/tpc14-3/#p14_3_2](https://www.mlit.go.jp/plateau/learning/tpc14-3/#p14_3_2)

### 1-5. ビルド設定にシーンを追加する

Unityエディターメニューの File > Build Settings を開き、ビルドするサンプルシーンをシーン一覧に登録します。

<img width="600" alt="ar_manual_5_buildsettings" src="/Documentation~/Images/ar_manual_5_buildsettings.png">


### 1-6. アプリケーションをビルドして端末にインストールする

Build Settingsに表示されているプラットフォームからAndroidもしくはiOSを選択し、「Switch Platform」を押下してプラットフォームを切り替えます（この操作には数分かかります）。

その後、Build Settingsの「Build」ボタンを押下し、出力先を選択してビルドを開始します（iOSの場合はフォルダ、Androidの場合は.apkファイルもしくはAndroid Gradleプロジェクト）。

ビルドが完了したら、各プラットフォームに合わせて端末へアプリケーションをインストールしてください。

### 1-7. ARサンプルシーンの操作方法

<img width="600" alt="ar_manual_6_defaultview" src="/Documentation~/Images/ar_manual_6_defaultview.jpeg">

サンプルアプリの右上の設定メニューでは下記のような設定を行うことが可能です。

**手動位置合わせ**

3D都市モデルの座標位置をxyzの三軸に沿って移動させ、位置を調整することが可能です。

**建物マテリアルの色設定**

表示する3D都市モデルの色をRGBAスライダーを動かすことで変更することが可能です。<br>
<img width="400" alt="ar_manual_7_manualui" src="/Documentation~/Images/ar_manual_7_manualui.png">
<img width="400" alt="ar_manual_8_runtime_red" src="/Documentation~/Images/ar_manual_8_runtime_red.png">

## 2. ウィンドウの利用方法

メニューより PLATEAU > PLATEAU Toolkit > AR Extensions を選択し、AR Extensions ウィンドウを開いて、それぞれの機能を利用することができます。<br>

<img width="600" alt="ar_manual_9_occulusionmenu" src="/Documentation~/Images/ar_manual_9_occulusionmenu.png">

### 2-1. 3D都市モデルのマテリアル変更

PLATEAU SDKを用いてシーンにインポートされた3D都市モデルオブジェクト ( `PLATEAUInstancedCityModel` )のマテリアルを一括で変更します。

設定するマテリアルを選択し、「シーン上の都市モデルのマテリアルを変更」を押下することでマテリアルが変更されます。

「ARオクルージョン遮蔽用マテリアルの参照を取得」を押下すると、遮蔽用マテリアルの参照がフィールドに設定されます。ARオクルージョン機能の利用方法については後述する「ARオクルージョン機能の利用方法」を参照してください。

## 3. 3D都市モデルのAR空間内位置合わせ機能

AR Extensions が提供する `PlateauARPositioning` コンポーネントを3D都市モデルにアタッチして設定することで、その3D都市モデルをAR空間内の実際の位置に自動的に配置することができます。

サンプルプロジェクトに設定済みのコンポーネントが含まれているので、以下の手順の参考にしてください。

### 3-1. `PlateauARPositioning` を設定する

新しくゲームオブジェクトを作成し、 `PlateauARPositioning` コンポーネントをアタッチします。以下ではこのオブジェクトを「位置合わせオブジェクト」と呼ぶことにします。

`Geospatial Controller` ( `PlateauARGeospatialController` ) はサンプルに含まれる「AR」プレハブの中にある「GeospatialController」を利用するか参考にして新しく作成したものを設定します。 

`Geoid Height Provider` は新たにゲームオブジェクトを作成し、サンプルの `GsiGeoidHeightProvider` をアタッチして設定します。

> [!Note]
> これらのオブジェクトは位置合わせオブジェクトが必要とするコンポーネントですが、具体的な実装は AR Extensions に含まれておらず、これらは抽象的なインターフェースとして提供されています。これはそれぞれの機能が利用されるアプリケーションに大きく依存するためです。そのため、これらのインターフェースの具体的な実装はサンプルとしてそれぞれ `GeospatialController` クラスと `GsiGeoidHeightProvider` クラスとして提供されています。

### 3-2. PLATEAU SDKでインポートした3D都市モデルを使用する場合

- 3D都市モデルオブジェクトを位置合わせオブジェクトの子オブジェクトとして配置します。
- `PlateauARPositioning` の `Plateau City Model` に3D都市モデルオブジェクトをセットします。

### 3-3. PLATEAUストリーミング(Cesium for Unity)を使用する場合

- `CesiumGeoreference` がアタッチされているオブジェクトを位置合わせオブジェクトの子オブジェクトとして配置します。
- `PlateauARPositioning` の `Cesium Georeference` と `Cesium 3D Tileset` にそれぞれ対象のオブジェクトをセットします。

### 3-4. 手動位置合わせ機能

`PlateauARPositioning` は `SetOffset(Vector3 offset)` というメソッドを公開しています。このメソッドにオフセット値を渡すことで、Geospatial API によって配置される位置を調整することができます。

### 3-5. ARマーカーによる高さ合わせ機能

現実世界に配置したARマーカー画像を用いた地面の高さの位置合わせ機能です。

`PlateauARMarkerGroundController` を位置合わせオブジェクトにアタッチして使用します。

`Building Layer` に3D都市モデルのレイヤーを指定します。ARオクルージョンを設定している場合、3D都市モデルのレイヤーは遮蔽するオブジェクト用レイヤーが設定されているはずなので、そちらを設定します。

`ARTrackedImageManager` を以下のドキュメントを参照して設定します。その後、設定した `ARTrackedImageManager` を `PlateauARMarkerGroundController` にセットします。

**AR Tracked Image Manager コンポーネント**

- [https://docs.unity3d.com/ja/Packages/com.unity.xr.arfoundation@5.1/manual/features/image-tracking.html](https://docs.unity3d.com/ja/Packages/com.unity.xr.arfoundation@5.1/manual/features/image-tracking.html)

`ARTrackedImageManager` の `XRReferenceImageLibrary` に登録したARマーカー画像をARアプリケーションの実行時に認識させることで、その画像を認識した場所を地面として高さを調整することができます。

地面の位置合わせを適用するためには、次のように取得できる高さの差を位置合わせオブジェクトのオフセットに指定することで建物の地面の高さを調整することができます。

```csharp
Vector3 offset = Vector3.zero;
offset.y = -m_ARMarkerGroundController.HeightGap;
m_ARPositioning.SetOffset(offset);
```

> [!Note]
> ARマーカーを用いた高さ合わせ機能は後述のARマーカーを用いた位置合わせ機能とは異なる機能で、単体で使用することはできません。
> 高さ合わせ機能はGeospatial APIを使用する際に、表示高さをARマーカーを用いて補正する機能です。
> 位置合わせ機能はGeospatial APIの代わりにマーカーを使用して位置合わせを行う機能なので、Geospatial APIと併用することができません。

<br/>

## 4. ARマーカーを使った3D都市モデルの位置合わせ機能

Geospatial APIなどを利用する代わりにARマーカーを利用することで、オフライン環境やGPSが取得できないトンネル・屋内などの環境でも3D都市モデルをAR空間上に位置を合わせて表示させることができます。

### 4-1. 事前準備

- AR環境の構築
    - AR Extensions のサンプルシーンを参考に、 `AR Session` や `AR Session Origin` をシーンに用意します。
    - サンプルシーンで使用しているプレハブをそのまま利用しても問題ありません。
- 利用する3D都市モデルのインポート
    - PLATEAU SDKを用いてシーン上に任意の3D都市モデルをインポートしてください。

### 4-2. ARマーカー位置合わせコンポーネントの設定

1. 空のゲームオブジェクトを作成し、位置と回転はそれぞれ (0, 0, 0) に設定します。
2. 分かりやすいように、 "ARMarkerCityModel" などの名前を設定してください。
3. 作成したゲームオブジェクトに `PlateauARMarkerCityModel` コンポーネントをアタッチします。
    - 以降の手順では作成したゲームオブジェクトをARマーカー位置合わせオブジェクトと呼びます。
4. `PlateauARMarkerCityModel` の各フィールドを設定します。
    - `都市モデルオブジェクト` にインポートした3D都市モデルを設定します。
    - `マーカー画像ライブラリ` に `AR Tracked Image Manager` がアタッチされたオブジェクトを設定します。
5. `マーカー画像ライブラリ` を設定すると、ARマーカー設定が行えるようになります。「+」ボタンを押下し、マーカー設定を追加します。
6. マーカー設定のプルダウンから位置合わせに使用するARマーカーを選択します。
    - 選択するとマーカー設定の右側に使用するARマーカーのプレビューが表示されます。
7. マーカー設定のトランスフォームに設定するためのゲームオブジェクトをARマーカー位置合わせオブジェクトの子オブジェクトとして新しく作成し、設定します。
    - このオブジェクトはARマーカーを配置する場所を示すために使用されます。
    - ここでは "ARMarkerPoint" という名前を設定し、以降の手順ではARマーカー位置オブジェクトと呼びます。

<img width="400" alt="ar_manual_8_markerpointsetting" src="/Documentation~/Images/ar_manual_8_markerpointsetting.png">

### 4-3. ARマーカーの位置設定

1. 前の手順で作成したARマーカー位置オブジェクトを実空間でARマーカーを配置したい場所に一致する、3D都市モデル上の位置に配置します。
    - マーカー設定に指定したゲームオブジェクトはシーン上で以下の画像のようにプレビューが表示されます。
<img width="400" alt="ar_manual_9_scenepreview1" src="/Documentation~/Images/ar_manual_9_scenepreview1.png">
<img width="400" alt="ar_manual_10_scenepreview2" src="/Documentation~/Images/ar_manual_10_scenepreview2.png">

### 4-4. 読み取りに使用するARマーカーの用意

1. 端末で読み取るためのARマーカーを印刷して用意します。
    - AR Extensions のサンプルで提供しているマーカーは "Assets/Samples/PLATEAU SDK AR Extensions for Unity/{AR Extensions バージョン}/ARSamples/ar-marker.pdf" から印刷することができます。

以上でARマーカー位置合わせの設定は完了です。

### 4-3. 動作確認

ARマーカー位置合わせを設定したシーンをビルド設定に追加し、アプリケーションをビルドして端末にインストールしてください。

印刷したARマーカーを設定したARマーカー位置オブジェクトに対応する場所に置き、アプリケーションで読み取ると、ARマーカーから計算した相対位置に3D都市モデルが表示されます。

<img width="374" alt="ar_extensions_armarker_real_marker_0" src="/Documentation~/Images/ar_extensions_armarker_real_marker_0.png">
<img width="481" alt="ar_extensions_armarker_real_marker_1" src="/Documentation~/Images/ar_extensions_armarker_real_marker_1.png">

<img width="800" alt="ar_extensions_armarker_real_marker_2" src="/Documentation~/Images/ar_extensions_armarker_real_marker_2.png">

## 5. ARオクルージョン機能の利用方法

> [!Note]
> ARテンプレートプロジェクトではARオクルージョンのための設定が構築されています。そのため、ARテンプレートをベースに開発をする場合はARオクルージョンのプロジェクト設定は不要です。

### 5-1. 遮蔽オブジェクトマテリアルの作成

遮蔽するオブジェクトが使用するマテリアルを用意します。このマテリアルが遮蔽する側の透明のマテリアルになります。このマテリアルは後述するAR Occlusion Renderer Featureによって描画時に差し替えられます。  

> [!Warning]
> このマテリアルを直接遮蔽する側のオブジェクトに設定しないのでご注意ください。

<img width="400" alt="ar_manual_10arroccluder" src="/Documentation~/Images/ar_manual_10arroccluder.png">

### 5-2. レイヤーの作成

ARオクルージョンを設定するためには、遮蔽する側と遮蔽される側の2つのレイヤーを用意する必要があります。ここでは遮蔽する側をAR Occluder、遮蔽される側をAR Occludeeとして説明しますが、必ずしも名前が一致している必要はありません。レイヤーの順序は他の要件を考慮の上設定してください。

<img width="400" alt="ar_manual_11_layer" src="/Documentation~/Images/ar_manual_11_layer.png">
<img width="400" alt="ar_manual_12_userlayer" src="/Documentation~/Images/ar_manual_12_userlayer.png">

### 5-3. URP描画設定を開く

Universal Renderer DataはURPプロジェクトの描画の設定をするファイルです。URP描画設定は状況に合わせた複数のURP設定が用意されていることもあり、UnityでURPプロジェクトを作成すると、3つのUniversal Renderer Dataとそれらに対応したUniversal Renderer Pipeline Assetがデフォルトで作成されます（HighFidelity、Balanced、Performant）。

<img width="400" alt="ar_manual_13_selectrenderfeature" src="/Documentation~/Images/ar_manual_13_selectrenderfeature.png">

それぞれ、想定されるプラットフォームのスペックなどに合わせた描画設定がされています。例えば、モバイル向けにビルドする場合はBalancedやPerformantを使用し、PC向けにはHighFidelityやBalanced を使用します（これらはProject SettingsのQualityタブからプラットフォームごとに設定することができます）。

<img width="400" alt="ar_manual_14" src="/Documentation~/Images/ar_manual_14.png">

### 5-4. URP描画設定にレイヤーを設定

ARオクルージョンによって遮蔽されるオブジェクト（AR Occludeeレイヤー）は後述するARオクルージョン用のRenderer Featureによって描画されます。そのため、デフォルトで描画されるレイヤーからAR Occludeeレイヤーを削除します。描画はOpaque Layer MaskとTransparent Layer Maskの2つがあるため、それぞれからAR Occludeeレイヤーのチェックを解除してください。

<img width="400" alt="ar_manual_15_filtering" src="/Documentation~/Images/ar_manual_15_filtering.png">

### 5-5. Plateau AR Occlusion Renderer Featureの追加

ARオクルージョンを動作させるためにはRenderer Featureに AR Extensions が用意する `PlateauAROcclusionRendererFeature` を追加する必要があります。

<img width="400" alt="ar_manual_16_occlusion_renderfeature" src="/Documentation~/Images/ar_manual_16_occlusion_renderfeature.png">

“Add Renderer Feature” から上記のRenderer Featureを追加し、それぞれのフィールドを設定してください。

| |  |
| --- | --- |
| AR Occludee Mask | 作成した遮蔽されるオブジェクトのレイヤーマスク |
| AR Occluder Mask | 作成した遮蔽するオブジェクトのレイヤーマスク |
| AR Occluder Material | 作成した遮蔽オブジェクト用の透明マテリアルの参照 |

ここまでで、設定ファイルの変更は完了です。

### 5-6. 遮蔽するオブジェクトのレイヤーを変更

遮蔽する側のオブジェクトのレイヤーを作成したAR Occluderレイヤーに変更します。

親オブジェクトだけでなく、すべての子オブジェクトのレイヤーも一括で変更してください（親オブジェクトのレイヤーを変更すると、子オブジェクトのレイヤーを変更するかどうかのウィンドウが表示されます）。

### 5-7. 遮蔽するオブジェクトのマテリアルをZWriteに変更

AR Extensions パッケージの Materials フォルダに遮蔽するオブジェクトに設定するための “ZWrite” というマテリアルを用意しています。遮蔽する側のオブジェクトにはこのマテリアルを設定してください。

<img width="400" alt="ar_manual_17_zwrite" src="/Documentation~/Images/ar_manual_17_zwrite.png">

AR Extensions ウィンドウに用意している機能を使うことで、シーン内の都市モデルのマテリアルを一括で変更することができます。ストリーミングを用いる場合は `Cesium3DTileset` コンポーネントの `Opaque Material` フィールドを設定してください。

<img width="400" alt="ar_manual_18_zwrite_mat" src="/Documentation~/Images/ar_manual_18_zwrite_mat.png">

### 5-8. 遮蔽されるオブジェクトのレイヤーを変更

遮蔽されるオブジェクトのレイヤーを作成したAR Occludeeレイヤーに変更します。

以上で、ARオクルージョンをオブジェクトに設定することができます。<br>
<img width="800" alt="ar_manual_19_UnityChan" src="/Documentation~/Images/ar_manual_19_UnityChan.gif">

# ライセンス
- 本リポジトリはMITライセンスで提供されています。
- 本システムの開発はユニティ・テクノロジーズ・ジャパン株式会社が行っています。
- ソースコードおよび関連ドキュメントの著作権は国土交通省に帰属します。

# 注意事項/利用規約
- 本ツールをアンインストールした場合、本ツールの機能で作成されたアセットの動作に不備が発生する可能性があります。
- 本ツールをアップデートした際は、一度 Unity エディターを再起動することを推奨しています。
- パフォーマンスの観点から、3km²の範囲に収まる3D都市モデルをダウンロード・インポートすることを推奨しています。
- インポートする範囲の広さや地物の種類（建物、道路、災害リスクなど）が量に比例して処理負荷が高くなる可能性があります。
- 本リポジトリの内容は予告なく変更・削除される可能性があります。
- 本リポジトリの利用により生じた損失及び損害等について、国土交通省はいかなる責任も負わないものとします。
