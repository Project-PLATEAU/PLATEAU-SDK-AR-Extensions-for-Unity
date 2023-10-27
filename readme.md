# Release2 AR Toolkit README draft

- 目次

# AR Toolkit・AR テンプレート利用マニュアル

PLATEAUの3D都市モデルを使ったARアプリケーション開発を行うための支援機能を提供します。

PLATEAU SDK Toolkits for Unity では PLATEAU 3D都市モデルを用いた開発に利用できる様々なツールキットを一つのパッケージで提供していますが、ARに関するツールキットのみは依存するライブラリが異なるため、別のパッケージとして提供しています。

- AR Toolkit で提供される機能
    - Geospatial API を用いたAR空間での3D都市モデルの位置合わせ
        - インポート (PLATEAU SDK) もしくはストリーミング (Cesium 3D Tiles) された3D都市モデルを、ビルドされたAR空間内で実際の建物の位置に配置します。
        - AR精度の問題で配置された3D都市モデルの位置がずれる場合にオフセットを指定して位置を調整することができます。
        - ARマーカー（現実空間に配置された特定の画像をカメラで認識させて位置を取得する機能）を用いて地面の高さを調整することができます。
    - オクルージョンのためのアセットや調整機能
        - AR空間内で3D都市モデルを用いて3Dオブジェクトを遮蔽する機能をオクルージョンと呼びます。
            - 参考: [https://www.mlit.go.jp/plateau/learning/tpc14-2/#p14_2_7](https://www.mlit.go.jp/plateau/learning/tpc14-2/#p14_2_7)
        - AR Toolkit ではオクルージョンを設定するためのマテリアルやスクリプトを用意しています。

# 事前準備

## PLATEAU SDK Toolkits for Unity のインストール

AR Toolkit は PLATEAU SDK Toolkits for Unity の機能を利用しているため、AR Toolkitを利用するためにはこちらをインストールする必要があります。

[こちら](https://github.com/Project-PLATEAU/PLATEAU-SDK-Toolkits-for-Unity#3-plateau-sdk-toolkits-for-unity-%E3%81%AE%E3%82%A4%E3%83%B3%E3%82%B9%E3%83%88%E3%83%BC%E3%83%AB)を参照して PLATEAU SDK Toolkits for Unity をインストールしてください。

## AR用テンプレートプロジェクトのセットアップ

本リポジトリではARに関連する設定が構築済みのテンプレートプロジェクトを提供しているため、以下の利用手順ではこのテンプレートプロジェクトを基本とした手順を説明します。

AR Toolkit リポジトリの “/~Templates” ディレクトリの中にある “AR Template” ディレクトリを任意の場所にコピーし、Unity Hubで AR Template プロジェクトを開いてください。

テンプレートプロジェクトのUnityバージョンは2021.3.22f1です。手元にない場合はUnity Hub よりインストールしてください。

## サンプルを用いたARアプリケーションの体験

<img width="600" alt="ar_manual_1_AR-Streaming" src="https://github.com/Project-PLATEAU/PLATEAU-SDK-AR-extensions-for-Unity/assets/137732437/e73e50b0-c5ee-4684-943a-e23647eea368">

AR Toolkit では各機能を使用したサンプルアセットおよび構築済みのARシーンを提供しています。このサンプルに含まれるシーンを用いることで、PLATEAU 3D都市モデルを使ったARアプリケーションをすぐに体験することができます。また、構築済みのアセットを見ることで、各機能の具体的な使い方を理解することもできます。

### AR Toolkit サンプルのインポート

メニューからPackage Managerを開き、AR Toolkitのサンプルをインポートしてください。

![ar_manual_1_2_packagemanager]()

<img width="600" alt="ar_manual_1_2_packagemanager" src="https://github.com/Project-PLATEAU/PLATEAU-SDK-AR-extensions-for-Unity/assets/137732437/026868d8-b7ce-4946-bed7-42f9aed3638d">


インポートされたサンプルは “Assets/Samples” ディレクトリに配置されます。以下の手順では Assets/Samples/PLATEAU AR Toolkit for Unity/0.1.0/AR Samples のディレクトリを「サンプルフォルダ」とします。

<img width="600" alt="ar_manual_1_2_packagemanager" src="[https://github.com/Project-PLATEAU/PLATEAU-SDK-AR-extensions-for-Unity/assets/137732437/026868d8-b7ce-4946-bed7-42f9aed3638d](https://github.com/Project-PLATEAU/PLATEAU-SDK-AR-extensions-for-Unity/assets/137732437/f3559a7a-040f-40e9-b9f0-35690080cc3c)">


### サンプルシーンを設定する

サンプルフォルダの Scenes ディレクトリには以下のシーンが含まれています。

- Sample01_PlateauSdkAR
    - PLATEAU SDKでインポートした3D都市モデルを用いたARシーンです。
    - 銀座の周辺の3D都市モデルを用いたサンプルとなっているため、このシーンを利用する際は別途3D都市モデルをインポートして差し替える必要があります。
- Sample02_PlateauCesiumAR
    - PLATEAUストリーミング (Cesium 3D Tiles) の3D都市モデルを用いたARシーンです。
    - PLATEAUストリーミングでは対象の地域を選択する必要がありますが、サンプルでは地域を選択するUIを用意しています。
- Boot
    - シーン選択のサンプルとしてBootシーンを用意しています。このシーンを起動シーンに選択し、他のシーンを合わせて登録するとシーン選択画面を表示し、任意のシーンを起動することができます。
      
        <img width="600" alt="ar_manual_1_2_packagemanager" src="https://github.com/Project-PLATEAU/PLATEAU-SDK-AR-extensions-for-Unity/assets/137732437/12030dd8-3b41-4eb6-ae63-9c359486494d">


### (任意) PLATEAU SDKでインポートした3D都市モデルを設定する

以下のドキュメントを参考にPLATEAU SDKでARアプリケーションを利用する付近の3D都市モデルをインポートします。

**都市モデルのインポート**

- [https://project-plateau.github.io/PLATEAU-SDK-for-Unity/manual/ImportCityModels.html](https://project-plateau.github.io/PLATEAU-SDK-for-Unity/manual/ImportCityModels.html)

インポートが完了したら、「3D都市モデルのAR空間内位置合わせ機能」の項目を参考に、位置合わせコンポーネント ( `PlateauARPositioning` )を設定してください。

### Geospatial API (ARCore Extensions) の設定

Geospatial API を利用するためには、 Google Cloud プロジェクトを用意し、ARCore API の認証を設定する必要があります。ARCore API を有効化し、API認証を設定することで端末から Geospatial API を利用することができるようになります。設定方法については別のドキュメントにて解説されているため、そちらを参考に設定してください。

****TOPIC14｜VR・ARでの活用[3/3]｜Google Geospatial APIで位置情報による3D都市モデルのARを作成する****

- APIキーの作成
    - [https://www.mlit.go.jp/plateau/learning/tpc14-3/#p14_3_2](https://www.mlit.go.jp/plateau/learning/tpc14-3/#p14_3_2)

### ビルド設定にシーンを追加する

Unityエディターメニューの File > Build Settings を開き、ビルドするサンプルシーンをシーン一覧に登録します。

<img width="600" alt="ar_manual_1_2_packagemanager" src="https://github.com/Project-PLATEAU/PLATEAU-SDK-AR-extensions-for-Unity/assets/137732437/dbb5534d-1bbc-4516-8f5f-89b2c7c328a0">

### アプリケーションをビルドして端末にインストールする

Build Settingsに表示されているプラットフォームからAndroidもしくはiOSを選択し、「Switch Platform」を押下してプラットフォームを切り替えます（この操作には数分かかります）。

その後、Build Settingsの「Build」ボタンを押下し、出力先を選択してビルドを開始します（iOSの場合はフォルダ、Androidの場合は.apkファイルもしくはAndroid Gradleプロジェクト）。

ビルドが完了したら、各プラットフォームに合わせて端末へアプリケーションをインストールしてください。

### ARサンプルシーンの操作方法

<img width="600" alt="ar_manual_6_runtime_green" src="[https://github.com/Project-PLATEAU/PLATEAU-SDK-AR-extensions-for-Unity/assets/137732437/dbb5534d-1bbc-4516-8f5f-89b2c7c328a0](https://github.com/Project-PLATEAU/PLATEAU-SDK-AR-extensions-for-Unity/assets/137732437/cf1bb77e-df36-4a82-b7f0-20b604c86998)">


**設定メニュー**

サンプルアプリの右上の設定メニューでは下記のような設定を行うことが可能です。

**手動位置合わせ**

3D都市モデルの座標位置を３に沿って移動させ、位置を調整することが可能です。

**建物マテリアルの色設定**

表示する3D都市モデルの色をRGBAスライダーを動かすことで変更することが可能です。
<img width="400" alt="ar_manual_7_manualui" src="https://github.com/Project-PLATEAU/PLATEAU-SDK-AR-extensions-for-Unity/assets/137732437/e41e5627-6697-4610-877c-d1117a804f83">

<img width="400" alt="ar_manual_8_runtime_red" src="https://github.com/Project-PLATEAU/PLATEAU-SDK-AR-extensions-for-Unity/assets/137732437/3c1d66bc-1014-4072-a45d-0265b497bc4a">

## AR Toolkit ウィンドウ

上部のメニューより PLATEAU > PLATEAU Toolkit > AR Toolkit を選択し、AR Toolkit ウィンドウを開いて、それぞれの機能を利用することができます。

<img width="600" alt="ar_manual_9_occulusionmenu" src="https://github.com/Project-PLATEAU/PLATEAU-SDK-AR-extensions-for-Unity/assets/137732437/6b57dec3-0394-4335-84e0-7f55ef4fd426">

### 3D都市モデルのマテリアル変更

*(注意) この機能はインポートされた3D都市モデルにのみ対応しています。Cesium 3D Tiles を使用する場合は `Cesium3DTileset` コンポーネントの `Opaque Material` フィールドを変更することでストリーミングされる3D都市モデルのマテリアルを変更することができます。*

PLATEAU SDKを用いてシーンにインポートされた3D都市モデルオブジェクト ( `PLATEAUInstancedCityModel` )のマテリアルを一括で変更します。

設定するマテリアルを選択し、「シーン上の都市モデルのマテリアルを変更」を押下することでマテリアルが変更されます。

「ARオクルージョン遮蔽用マテリアルの参照を取得」を押下すると、遮蔽用マテリアルの参照がフィールドに設定されます。ARオクルージョンおよび遮蔽用マテリアルは後述するARオクルージョンの説明を参照してください。

## 3D都市モデルのAR空間内位置合わせ機能

AR Toolkit が提供する `PlateauARPositionig` コンポーネントを3D都市モデルにアタッチして設定することで、その3D都市モデルをAR空間内の実際の位置に自動的に配置することができます。

設定済みのコンポーネントはサンプルに含まれているので、以下の手順の詳細はそちらを参照して下さい。

### `PlateauARPositioning` を設定する

新しくゲームオブジェクトを作成し、 `PlateauARPositioning` コンポーネントをアタッチします。以下ではこのオブジェクトを「位置合わせオブジェクト」と呼ぶことにします。

`Geospatial Controller` ( `PlateauARGeospatialController` ) はサンプルに含まれる「AR」プレハブの中にある「GeospatialController」を利用するか参考にして新しく作成したものを設定します。 

`Geoid Height Provider` は新たにゲームオブジェクトを作成し、サンプルの `GsiGeoidHeightProvider` をアタッチして設定します。

**(備考) `PlateauARGeospatialController` と `GeoidHeightProvider` について**

これらのオブジェクトは位置合わせオブジェクトが必要とするコンポーネントですが、具体的な実装は AR Toolkit に含まれておらず、これらは抽象的なインターフェースとして提供されています。これはそれぞれの機能が利用されるアプリケーションに大きく依存するためです。そのため、これらのインターフェースの具体的な実装はサンプルとしてそれぞれ `GeospatialController` クラスと `GsiGeoidHeightProvider` クラスとして提供されています。

### PLATEAU SDKでインポートした3D都市モデルを使用する場合

- 3D都市モデルオブジェクトを位置合わせオブジェクトの子オブジェクトとして配置します。
- `PlateauARPositioning` の `Plateau City Model` に3D都市モデルオブジェクトをセットします。

### PLATEAUストリーミング(Cesium 3D Tiles)を使用する場合

- `CesiumGeoreference` がアタッチされているオブジェクトを位置合わせオブジェクトの子オブジェクトとして配置します。
- `PlateauARPositioning` の `Cesium Georeference` と `Cesium 3D Tileset` にそれぞれ対象のオブジェクトをセットします。

### 手動位置合わせ機能

`PlateauARPositionig` は `SetOffset(Vector3 offset)` というメソッドを公開しています。このメソッドにオフセット値を渡すことで、Geospatial API によって配置される位置を調整することができます。

## ARマーカーによる高さ合わせ機能

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

## ARオクルージョン機能の利用方法

(備考) ARテンプレートプロジェクトではARオクルージョンのための設定が構築されています。そのため、ARテンプレートをベースに開発をする場合はARオクルージョンのプロジェクト設定は不要です。

### 遮蔽オブジェクトマテリアルの作成

遮蔽するオブジェクトが使用するマテリアルを用意します。このマテリアルが遮蔽する側の透明のマテリアルになります。このマテリアルは後述するAR Occlusion Renderer Featureによって描画時に差し替えられます。
※このマテリアルを直接遮蔽する側のオブジェクトに設定しないのでご注意ください。

<img width="400" alt="ar_manual_10arroccluder" src="https://github.com/Project-PLATEAU/PLATEAU-SDK-AR-extensions-for-Unity/assets/137732437/4d6ef4c6-27eb-4f64-9cc4-6b44da74a594">


### レイヤーの作成

ARオクルージョンを設定するためには、遮蔽する側と遮蔽される側の2つのレイヤーを用意する必要があります。ここでは遮蔽する側をAR Occluder、遮蔽される側をAR Occludeeとして説明しますが、必ずしも名前が一致している必要はありません。レイヤーの順序は他の要件を考慮の上設定してください。

<img width="400" alt="ar_manual_11_layer" src="https://github.com/Project-PLATEAU/PLATEAU-SDK-AR-extensions-for-Unity/assets/137732437/438948d5-2f17-4f8b-aa77-a6eea1f04b95">

<img width="400" alt="ar_manual_12_userlayer" src="https://github.com/Project-PLATEAU/PLATEAU-SDK-AR-extensions-for-Unity/assets/137732437/01da71cd-787a-4e7a-bcfd-ee4043a12a95">


### URP描画設定を開く

Universal Renderer DataはURPプロジェクトの描画の設定をするファイルです。URP描画設定は状況に合わせた複数のURP設定が用意されていることもあり、UnityでURPプロジェクトを作成すると、3つのUniversal Renderer Dataとそれらに対応したUniversal Renderer Pipeline Assetがデフォルトで作成されます（HighFidelity、Balanced、Performant）。

<img width="400" alt="ar_manual_13_selectrenderfeature" src="https://github.com/Project-PLATEAU/PLATEAU-SDK-AR-extensions-for-Unity/assets/137732437/38df325f-b912-48da-810c-8c1b9432aeff">


それぞれ、想定されるプラットフォームのスペックなどに合わせた描画設定がされています。例えば、モバイル向けにビルドする場合はBalancedやPerformantを使用し、PC向けにはHighFidelityやBalanced を使用します（これらはProject SettingsのQualityタブからプラットフォームごとに設定することができます）。
<img width="483" alt="ar_manual_14" src="https://github.com/Project-PLATEAU/PLATEAU-SDK-AR-extensions-for-Unity/assets/137732437/4cb513a6-bb1f-4ce4-8bb4-3aded56e54b5">


### URP描画設定にレイヤーを設定

ARオクルージョンによって遮蔽されるオブジェクト（AR Occludeeレイヤー）は後述するARオクルージョン用のRenderer Featureによって描画されます。そのため、デフォルトで描画されるレイヤーからAR Occludeeレイヤーを削除します。描画はOpaque Layer MaskとTransparent Layer Maskの2つがあるため、それぞれからAR Occludeeレイヤーのチェックを解除してください。


### Plateau AR Occlusion Renderer Featureの追加

ARオクルージョンを動作させるためにはRenderer FeatureにAR Toolkitが用意する `PlateauAROcclusionRendererFeature` を追加する必要があります。

![Untitled](Release2%20AR%20Toolkit%20README%20draft%20db4aa4f5a6644676a9bcb415b340407e/Untitled%207.png)

“Add Renderer Feature” から上記のRenderer Featureを追加し、それぞれのフィールドを設定してください。

| AR Occludee Mask | 作成した遮蔽されるオブジェクトのレイヤーマスク |
| --- | --- |
| AR Occluder Mask | 作成した遮蔽するオブジェクトのレイヤーマスク |
| AR Occluder Material | 作成した遮蔽オブジェクト用の透明マテリアルの参照 |

ここまでで、設定ファイルの変更は完了です。

### 遮蔽するオブジェクトのレイヤーを変更

遮蔽する側のオブジェクトのレイヤーを作成したAR Occluderレイヤーに変更します。

親オブジェクトだけでなく、すべての子オブジェクトのレイヤーも一括で変更してください（親オブジェクトのレイヤーを変更すると、子オブジェクトのレイヤーを変更するかどうかのウィンドウが表示されます）。

### 遮蔽するオブジェクトのマテリアルをZWriteに変更

AR ToolkitパッケージのMaterialsフォルダに遮蔽するオブジェクトに設定するための “ZWrite” というマテリアルを用意しています。遮蔽する側のオブジェクトにはこのマテリアルを設定してください。

![Untitled](Release2%20AR%20Toolkit%20README%20draft%20db4aa4f5a6644676a9bcb415b340407e/Untitled%208.png)

AR Toolkit ウィンドウに用意している機能を使うことで、シーン内の都市モデルのマテリアルを一括で変更することができます。PLATEAUストリーミングを用いる場合は `Cesium3DTileset` コンポーネントの Opaque Material フィールドを設定してください。

![Untitled](Release2%20AR%20Toolkit%20README%20draft%20db4aa4f5a6644676a9bcb415b340407e/Untitled%209.png)

### 遮蔽されるオブジェクトのレイヤーを変更

遮蔽されるオブジェクトのレイヤーを作成したAR Occludeeレイヤーに変更します。

以上で、ARオクルージョンをオブジェクトに設定することができます。

![2023-10-19-20-16-22.gif](Release2%20AR%20Toolkit%20README%20draft%20db4aa4f5a6644676a9bcb415b340407e/2023-10-19-20-16-22.gif)
