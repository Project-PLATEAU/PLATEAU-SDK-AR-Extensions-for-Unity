# PLATEAU SDK-AR-Extensions for Unity 利用マニュアル


https://github.com/SeiTakeuchi/PLATEAU-SDK-AR-Extensions-for-Unity-drafts/assets/127069970/45df5006-6d1a-469f-992f-1c842f6a9c9e



PLATEAUの3D都市モデルを使ったARアプリケーション開発を行うための支援機能を提供します。

- PLATEAU-SDK-AR-Extensions-for-Unityで提供される機能
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

|  2023/12/13  |  マーカーによるPLATEAUモデルの位置合わせ機能　|
| :--- | :--- |
|  2023/10/28  |  AR Extensions初回リリース|

     
# 目次
- [検証済環境](#検証済環境)
- [事前準備](#事前準備)
  * [PLATEAU SDK-Toolkits for Unity のインストール](#plateau-sdk-toolkits-for-unity-のインストール)
  * [AR用テンプレートプロジェクトのセットアップ](#ar用テンプレートプロジェクトのセットアップ)
 
- [利用手順](#利用手順)
  * [1. サンプルを用いたARアプリケーションの体験](#1-サンプルを用いたarアプリケーションの体験)
    + [1-1. AR Extensions サンプルのインポート](#1-1-ar-extensions-サンプルのインポート)
    + [1-2. サンプルシーンを設定する](#1-2-サンプルシーンを設定する)
    + [1-3. PLATEAU SDKでインポートした3D都市モデルを設定する](#1-3-plateau-sdkでインポートした3d都市モデルを設定する)
    + [1-4. Geospatial API (ARCore Extensions) の設定](#1-4-geospatial-api-arcore-extensions-の設定)
    + [1-5. ビルド設定にシーンを追加する](#1-5-ビルド設定にシーンを追加する)
    + [1-6. アプリケーションをビルドして端末にインストールする](#1-6-アプリケーションをビルドして端末にインストールする)
    + [1-7. ARサンプルシーンの操作方法](#1-7-arサンプルシーンの操作方法)
  * [2. ウィンドウの利用方法](#2-ウィンドウの利用方法)
    + [2-1. 3D都市モデルのマテリアル変更](#2-1-3d都市モデルのマテリアル変更)

  * [3. 3D都市モデルのAR空間内位置合わせ機能](#3-3d都市モデルのar空間内位置合わせ機能)
    + [3-1. `PlateauARPositioning` を設定する](#3-1-plateauarpositioning-を設定する)
    + [3-2. PLATEAU SDKでインポートした3D都市モデルを使用する場合](#3-2-plateau-sdkでインポートした3d都市モデルを使用する場合)
    + [3-3. PLATEAUストリーミング(Cesium for Unity)を使用する場合](#3-3-plateauストリーミングcesium-for-unityを使用する場合)
    + [3-4. 手動位置合わせ機能](#3-4-手動位置合わせ機能)
    + [3-5. ARマーカーによる高さ合わせ機能](#3-5-arマーカーによる高さ合わせ機能)
 
  * [4. ARマーカーを使ったPLATEAUモデルの位置合わせ機能](#4-arマーカーを使ったplateauモデルの位置合わせ機能)
    + [4-1. UnityEditor上でマーカーオブジェクトを配置する](#4-1-unityeditor上でマーカーオブジェクトを配置する)
    + [4-2. 物理ARマーカーを印刷して用意する](#4-2-物理arマーカーを印刷して用意する)
    + [4-3. 物理ARマーカーをカメラでスキャンする](#4-3-物理arマーカーをカメラでスキャンする)
    
  * [5. ARオクルージョン機能の利用方法](#5-arオクルージョン機能の利用方法)
    + [5-1. 遮蔽オブジェクトマテリアルの作成](#5-1-遮蔽オブジェクトマテリアルの作成)
    + [5-2. レイヤーの作成](#5-2-レイヤーの作成)
    + [5-3. URP描画設定を開く](#5-3-urp描画設定を開く)
    + [5-4. URP描画設定にレイヤーを設定](#5-4-urp描画設定にレイヤーを設定)
    + [5-5. Plateau AR Occlusion Renderer Featureの追加](#5-5-plateau-ar-occlusion-renderer-featureの追加)
    + [5-6. 遮蔽するオブジェクトのレイヤーを変更](#5-6-遮蔽するオブジェクトのレイヤーを変更)
    + [5-7. 遮蔽するオブジェクトのマテリアルをZWriteに変更](#5-7-遮蔽するオブジェクトのマテリアルをzwriteに変更)
    + [5-8. 遮蔽されるオブジェクトのレイヤーを変更](#5-8-遮蔽されるオブジェクトのレイヤーを変更)
  
  * [ライセンス](#ライセンス)
  * [注意事項/利用規約](#注意事項利用規約)


# 検証済環境
### OS環境
- Windows11
- macOS Ventura 13.2
- Android 13
- iOS 16.7.1

### Unity Version
- Unity 2021.3.31f1 (2023/10/10現在 2021 LTSバージョン)
    - Unity 2021.3系であれば問題なく動作する見込みです。

### Rendering Pipeline
- URP

HDRP, Built-in Rendering Pipelineでは動作しません。

## PLATEAU SDKバージョン
- [version 2.0.3-alpha](https://github.com/Synesthesias/PLATEAU-SDK-for-Unity/releases/tag/v2.0.3-alpha)

# 事前準備

## PLATEAU SDK-Toolkits for Unity のインストール

AR Extensions は PLATEAU SDK-Toolkits for Unity の機能を利用しているため、AR Toolkitを利用するためにはこちらをインストールする必要があります。

[こちら](https://github.com/Project-PLATEAU/PLATEAU-SDK-Toolkits-for-Unity#3-plateau-sdk-toolkits-for-unity-%E3%81%AE%E3%82%A4%E3%83%B3%E3%82%B9%E3%83%88%E3%83%BC%E3%83%AB)を参照して PLATEAU SDK Toolkits for Unity をインストールしてください。

## AR用テンプレートプロジェクトのセットアップ

本リポジトリでは構築済みのテンプレートプロジェクトを提供しています。  
以下の利用ではこのテンプレートプロジェクトを基本としたARアプリの構築手順を説明します。

AR Extensions リポジトリの “/~Templates” ディレクトリの中にある “AR Template” ディレクトリを任意の場所にコピーし、Unity Hubで AR Template プロジェクトを開いてください。

テンプレートプロジェクトのUnityバージョンは2021.3.22f1です。手元にない場合はUnity Hub よりインストールしてください。

# 利用手順

## 1. サンプルを用いたARアプリケーションの体験

<img width="600" alt="ar_manual_1_AR-Streaming" src="https://github.com/Project-PLATEAU/PLATEAU-SDK-AR-Extensions-for-Unity/assets/137732437/2c35dec7-caae-4fc2-a14d-73d9446c106e">




AR Extensions では各機能を使用したサンプルアセットおよび構築済みのARシーンを提供しています。このサンプルに含まれるシーンを用いることで、PLATEAUの3D都市モデルを使ったARアプリケーションをすぐに体験することができます。  
また、構築済みのアセットを見ることで、各機能の具体的な使い方を理解することもできます。

### 1-1. AR Extensions サンプルのインポート

メニューからPackage Managerを開き、AR Extensions のサンプルをインポートしてください。

<img width="600" alt="ar_manual_1_3_packagemanager" src="https://github.com/Project-PLATEAU/PLATEAU-SDK-AR-Extensions-for-Unity/assets/137732437/7bcc01f2-eafc-471c-b27a-dd7decfe63ea">


インポートされたサンプルは “Assets/Samples” ディレクトリに配置されます。以下の手順では Assets/Samples/PLATEAU AR Toolkit for Unity/0.1.0/AR Samples のディレクトリを「サンプルフォルダ」とします。

<img width="400" alt="ar_manual_2_arsample_hierarchy" src="https://github.com/Project-PLATEAU/PLATEAU-SDK-AR-Extensions-for-Unity/assets/137732437/8bb59249-acdb-4d43-98b1-b0b7f8447dbe">



### 1-2. サンプルシーンを設定する

サンプルフォルダの Scenes ディレクトリには以下のシーンが含まれています。

- Sample01_PlateauSdkAR
    - PLATEAU SDKでインポートした3D都市モデルを用いたARシーンです。
    - 銀座の周辺の3D都市モデルを用いたサンプルとなっているため、このシーンを銀座以外の場所で利用する際には別途3D都市モデルをインポートして差し替える必要があります。
- Sample02_PlateauCesiumAR
    - ストリーミングにより配置されたPLATEAUの3D都市モデル（3DTiles）を用いたARシーンです。
    - ストリーミングでは対象の地域を選択する必要がありますが、サンプルでは地域を選択するUIを用意しています。
    - ストリーミングを用いた3D都市モデルの利用方法については[PLATEAU SDK Maps Toolkit](！！ダミー！！)を参照してください。
- Boot
    - シーン選択のサンプルとしてBootシーンを用意しています。このシーンを起動シーンに選択し、他のシーンを合わせて登録するとシーン選択画面を表示し、任意のシーンを起動することができます。

<img width="400" alt="ar_manual_4_selectui" src="https://github.com/Project-PLATEAU/PLATEAU-SDK-AR-Extensions-for-Unity/assets/137732437/a227cc93-db94-4101-b386-e5e3ea0e758c)">


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

<img width="600" alt="ar_manual_5_buildsettings" src="https://github.com/Project-PLATEAU/PLATEAU-SDK-AR-Extensions-for-Unity/assets/137732437/3a1d661b-a4ca-4d00-b312-003bd9a10323">


### 1-6. アプリケーションをビルドして端末にインストールする

Build Settingsに表示されているプラットフォームからAndroidもしくはiOSを選択し、「Switch Platform」を押下してプラットフォームを切り替えます（この操作には数分かかります）。

その後、Build Settingsの「Build」ボタンを押下し、出力先を選択してビルドを開始します（iOSの場合はフォルダ、Androidの場合は.apkファイルもしくはAndroid Gradleプロジェクト）。

ビルドが完了したら、各プラットフォームに合わせて端末へアプリケーションをインストールしてください。

### 1-7. ARサンプルシーンの操作方法

<img width="600" alt="ar_manual_5_buildsettings" src="https://github.com/Project-PLATEAU/PLATEAU-SDK-AR-Extensions-for-Unity/assets/137732437/1d35242c-3c5f-4c63-bd83-18419af2fabc">



サンプルアプリの右上の設定メニューでは下記のような設定を行うことが可能です。

**手動位置合わせ**

3D都市モデルの座標位置をxyzの三軸に沿って移動させ、位置を調整することが可能です。

**建物マテリアルの色設定**

表示する3D都市モデルの色をRGBAスライダーを動かすことで変更することが可能です。
<img width="400" alt="ar_manual_7_manualui" src="https://github.com/Project-PLATEAU/PLATEAU-SDK-AR-Extensions-for-Unity/assets/137732437/06f2e5b3-5636-4bce-9d18-ce3637c96e5c">
<img width="400" alt="ar_manual_8_runtime_red" src="https://github.com/Project-PLATEAU/PLATEAU-SDK-AR-Extensions-for-Unity/assets/137732437/2ede7b0a-2ce2-4049-a4be-f156df5e0243">


## 2. ウィンドウの利用方法

メニューより PLATEAU > PLATEAU Toolkit > AR Toolkit を選択し、AR Toolkit ウィンドウを開いて、それぞれの機能を利用することができます。

<img width="600" alt="ar_manual_9_occulusionmenu" src="https://github.com/Project-PLATEAU/PLATEAU-SDK-AR-Extensions-for-Unity/assets/137732437/68df4b49-0605-497f-a5a6-0ab8ad2c0b9e">



### 2-1. 3D都市モデルのマテリアル変更

> **Note**
> この機能は現時点ではインポートされた3D都市モデルにのみ対応しています（今後実装予定）。  
> Cesium for Unityによってストリーミングされた3D都市モデルを利用する場合は `Cesium3DTileset` コンポーネントの `Opaque Material` フィールドを変更することでマテリアルを変更することができます。（詳細方法については後日追記予定）

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

> **備考**
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

> **Note**
> ARマーカーを用いた高さ合わせ機能は後述のARマーカーを用いた位置合わせ機能とは異なる機能で、単体で使用することはできません。
> 高さ合わせ機能はGeospatial APIを使用する際に、表示高さをARマーカーを用いて補正する機能です。
> 位置合わせ機能はGeospatial APIの代わりにマーカーを使用して位置合わせを行う機能なので、Geospatial APIと併用することができません。

<br/>

## 4. ARマーカーを使ったPLATEAUモデルの位置合わせ機能

GeospatialAPIなどを利用する代わりにARマーカーを利用することで、オフライン環境やGPSが取得できないトンネル・屋内などの環境でもPLATEAUモデルを地形に合わせることが可能となります。

### 4-1. UnityEditor上でマーカーオブジェクトを配置する

実空間でマーカーを配置したい場所に、PLATEAUモデル上でマーカーを配置します。
"ARMarkerCityModelDev"の中にある"ARMarkerPoint"を動かして、配置し直してください。

<img width="800" alt="スクリーンショット 2023-11-28 0 30 17" src="https://github.com/unity-takeuchi/PLATEAU-SDK-AR-Extensions-for-Unity-drafts/assets/137732437/098983f9-212d-47a8-8e03-a4dd9ab24da0">
<img width="800" alt="スクリーンショット 2023-11-28 0 30 26" src="https://github.com/unity-takeuchi/PLATEAU-SDK-AR-Extensions-for-Unity-drafts/assets/137732437/4b4f54c7-99e2-46cd-966e-970e24b7d081">


### 4-2. 物理ARマーカーを印刷して用意する
物理ARマーカーを印刷して用意してください。本サンプルのデフォルトのマーカーは　Assets > Samples > PLATEAU SDK AR Extensions for Unity > 0.1.1 > ARSamplesの中のar-marker.pdfです。

### 4-3. 物理ARマーカーをカメラでスキャンする
サンプルアプリを起動し、対象となる物理ARマーカーをカメラでスキャンします。<br>
すると、ARマーカーから相対位置を解決し、PLATEAUモデルが表示されます。

<img width="340" alt="スクリーンショット 2023-11-28 0 22 49" src="https://github.com/unity-takeuchi/PLATEAU-SDK-AR-Extensions-for-Unity-drafts/assets/137732437/1b31a904-b589-4d8c-bc83-59bcc0aa4755">

<img width="481" alt="スクリーンショット 2023-11-28 0 23 29" src="https://github.com/unity-takeuchi/PLATEAU-SDK-AR-Extensions-for-Unity-drafts/assets/137732437/cb77ddce-f6c0-41ac-98e9-5d378958e8b7">

※なお、ARマーカーの画像を変更したい場合はReference Image Libraryの画像を変更してください。

<img width="800" alt="スクリーンショット 2023-11-28 0 32 44" src="https://github.com/unity-takeuchi/PLATEAU-SDK-AR-Extensions-for-Unity-drafts/assets/137732437/e014b381-8146-4cca-a364-013a33d8aed1">


## 5. ARオクルージョン機能の利用方法

> **備考**
> ARテンプレートプロジェクトではARオクルージョンのための設定が構築されています。そのため、ARテンプレートをベースに開発をする場合はARオクルージョンのプロジェクト設定は不要です。

### 5-1. 遮蔽オブジェクトマテリアルの作成

遮蔽するオブジェクトが使用するマテリアルを用意します。このマテリアルが遮蔽する側の透明のマテリアルになります。このマテリアルは後述するAR Occlusion Renderer Featureによって描画時に差し替えられます。  
※このマテリアルを直接遮蔽する側のオブジェクトに設定しないのでご注意ください。

<img width="400" alt="ar_manual_10arroccluder" src="https://github.com/Project-PLATEAU/PLATEAU-SDK-AR-Extensions-for-Unity/assets/137732437/735a4bcb-6ebd-4873-8c28-2aefe2d70fbb">


### 5-2. レイヤーの作成

ARオクルージョンを設定するためには、遮蔽する側と遮蔽される側の2つのレイヤーを用意する必要があります。ここでは遮蔽する側をAR Occluder、遮蔽される側をAR Occludeeとして説明しますが、必ずしも名前が一致している必要はありません。レイヤーの順序は他の要件を考慮の上設定してください。

<img width="400" alt="ar_manual_11_layer" src="https://github.com/Project-PLATEAU/PLATEAU-SDK-AR-Extensions-for-Unity/assets/137732437/14a15f71-6667-44c6-ace4-a4d0b49c7fc4">
<img width="400" alt="ar_manual_12_userlayer" src="https://github.com/Project-PLATEAU/PLATEAU-SDK-AR-Extensions-for-Unity/assets/137732437/9b30878d-c91b-4aac-b6f3-9e88206eab96">


### 5-3. URP描画設定を開く

Universal Renderer DataはURPプロジェクトの描画の設定をするファイルです。URP描画設定は状況に合わせた複数のURP設定が用意されていることもあり、UnityでURPプロジェクトを作成すると、3つのUniversal Renderer Dataとそれらに対応したUniversal Renderer Pipeline Assetがデフォルトで作成されます（HighFidelity、Balanced、Performant）。

<img width="400" alt="ar_manual_13_selectrenderfeature" src="https://github.com/Project-PLATEAU/PLATEAU-SDK-AR-Extensions-for-Unity/assets/137732437/34e9c0ec-718f-4bb1-9b72-f8dc1b3d6f5c">

それぞれ、想定されるプラットフォームのスペックなどに合わせた描画設定がされています。例えば、モバイル向けにビルドする場合はBalancedやPerformantを使用し、PC向けにはHighFidelityやBalanced を使用します（これらはProject SettingsのQualityタブからプラットフォームごとに設定することができます）。

<img width="400" alt="ar_manual_14" src="https://github.com/Project-PLATEAU/PLATEAU-SDK-AR-Extensions-for-Unity/assets/137732437/8448e9b1-9427-4510-b279-3ba87094c90a">


### 5-4. URP描画設定にレイヤーを設定

ARオクルージョンによって遮蔽されるオブジェクト（AR Occludeeレイヤー）は後述するARオクルージョン用のRenderer Featureによって描画されます。そのため、デフォルトで描画されるレイヤーからAR Occludeeレイヤーを削除します。描画はOpaque Layer MaskとTransparent Layer Maskの2つがあるため、それぞれからAR Occludeeレイヤーのチェックを解除してください。

<img width="400" alt="ar_manual_15_filtering" src="https://github.com/Project-PLATEAU/PLATEAU-SDK-AR-Extensions-for-Unity/assets/137732437/3f2c848f-fedc-40ec-8010-7b13e0c2ac84">


### 5-5. Plateau AR Occlusion Renderer Featureの追加

ARオクルージョンを動作させるためにはRenderer Featureに AR Extensions が用意する `PlateauAROcclusionRendererFeature` を追加する必要があります。

<img width="400" alt="ar_manual_16_occlusion_renderfeature" src="https://github.com/Project-PLATEAU/PLATEAU-SDK-AR-Extensions-for-Unity/assets/137732437/7cd2b30c-7574-478a-908b-386eb29edc50">


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

AR Extensions パッケージのMaterialsフォルダに遮蔽するオブジェクトに設定するための “ZWrite” というマテリアルを用意しています。遮蔽する側のオブジェクトにはこのマテリアルを設定してください。


<img width="400" alt="ar_manual_17_zwrite" src="https://github.com/Project-PLATEAU/PLATEAU-SDK-AR-Extensions-for-Unity/assets/137732437/bd2238b2-6ed9-4f80-8ab0-e995f42a4080">


AR Extensions ウィンドウに用意している機能を使うことで、シーン内の都市モデルのマテリアルを一括で変更することができます。ストリーミングを用いる場合は `Cesium3DTileset` コンポーネントの Opaque Material フィールドを設定してください。

<img width="400" alt="ar_manual_18_zwrite_mat" src="https://github.com/Project-PLATEAU/PLATEAU-SDK-AR-Extensions-for-Unity/assets/137732437/3c76f2fb-6de3-4314-a8ae-84dbe54a9213">

### 5-8. 遮蔽されるオブジェクトのレイヤーを変更

遮蔽されるオブジェクトのレイヤーを作成したAR Occludeeレイヤーに変更します。

以上で、ARオクルージョンをオブジェクトに設定することができます。
![2023-10-19-20-16-22](https://github.com/Project-PLATEAU/PLATEAU-SDK-AR-Extensions-for-Unity/assets/137732437/514f0470-1120-408d-b2fb-ebf5fc77471e)


# ライセンス
- 本リポジトリはMITライセンスで提供されています。
- 本システムの開発はユニティ・テクノロジーズ・ジャパン株式会社が行っています。
- ソースコードおよび関連ドキュメントの著作権は国土交通省に帰属します。

# 注意事項/利用規約
- 本ツールはベータバージョンです。バグ、動作不安定、予期せぬ挙動等が発生する可能性があり、動作保証はできかねますのでご了承ください。
- 処理をしたあとにToolkitsをアンインストールした場合、建物の表示が壊れるなど挙動がおかしくなる場合がございます。
- 本ツールをアップデートした際は、一度Unity エディタを再起動してご利用ください。
- パフォーマンスの観点から、3D都市モデルをダウンロード・インポートする際は、3㎞2範囲内とすることを推奨しています。
- インポートエリアの広さや地物の種類（建物、道路、災害リスクなど）が増えると処理負荷が高くなる可能性があります。
- 本リポジトリの内容は予告なく変更・削除する可能性があります。
- 本リポジトリの利用により生じた損失及び損害等について、国土交通省はいかなる責任も負わないものとします。
