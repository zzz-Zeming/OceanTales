# OceanStories

**2D 像素风钓鱼 RPG · Unity 6000.3.21f1**

你来到一座海岛，岛上有一座传说中的古老水族馆——但所有鱼缸都是空的。
作为新晋渔夫，你要从海岸线出发，一路钓向深海，捕捉全世界最稀有的鱼，
重新点亮这座水族馆，并集齐《全鱼类图鉴》。

> 策划案中的作品名为《OceanTales》，工程与仓库沿用了 **OceanStories** 这个名字。

---

## 玩法循环

```
抛竿  →  鱼钩入水下沉  →  水下搜寻鱼群  →  鱼咬钩  →  收线出水  →  鱼获入库  →  布置水族馆
```

- **陆地与船只**：在沙滩或木屋里走动，靠近船按 `F` 上船，之后由船只接管移动。
- **抛竿**：给鱼钩一个向右上方的冲量，划出抛物线落水；入水后自动切换为"低重力 + 高阻力"的水下物理。
- **水下**：鱼钩持续下沉，玩家在左右方向微调位置，用鱼线把钩子"牵"向游动的鱼影。
- **捕获**：鱼碰到钩子即判定咬钩，AI 停止、贴图挂到钩下，变成钩子的子物体。
- **结算**：鱼钩浮回海平面以上才触发结算，鱼获写入全局背包并存档。

### 进度目标

| 里程碑 | 解锁 |
|---|---|
| 收集 4 条鱼 | 可以上船 |
| 收集 10 条鱼 | 可以出海，前往更深的钓点 |

---

## 操作

| 按键 | 动作 |
|---|---|
| `A` / `D`（或 ← →） | 左右行走 / 划船 / 控制鱼钩左右漂移 |
| `F` | 上船、下船、进入木屋 |
| `Space` | 抛竿；连按控制上钩；按住收线 |

---

## 场景流程

`Home`（主菜单）→ `Land`（沙滩 / 木屋）→ `Room`（室内 · 水族馆）→ `Ocean`（海面）→ `Inshore` → `Offshore` → `DeepOcean`

构建列表（`ProjectSettings/EditorBuildSettings.asset`）里按上面的顺序登记了 7 个场景，
从 `Assets/Scenes/Home.unity` 开始运行。

---

## 主要系统

### 1. 钓鱼核心
物理抛竿、入水后的浮力与阻力切换、用 `LineRenderer` 实时绘制"船底 → 鱼钩"的动态鱼线，
以及基于 `OnTriggerEnter2D` 的咬钩判定与单钩单鱼的载荷限制。

### 2. 玩家与载具
`A`/`D` 行走；`F` 键通过 `SetParent` 把玩家绑到船上，控制权随之转移给船只，
玩家在船上时不再能自由走动（局部坐标 / 世界坐标的转换在这里是关键）。

### 3. 全局数据与场景管理
`StatusTable` 单例作为跨场景的数据中心：切换"海面 ↔ 水下"时不会丢数据，
并用 `PlayerState` / `BoatState` 这样的可序列化对象记住渔夫与船只离开时的坐标，
返回海面时精确还原，避免位置被重置。鱼获背包是一个全局列表。

### 4. AI 与生态
鱼类在指定区域内随机巡逻，按移动方向自动翻转贴图；
水族馆场景读取背包数据，动态生成玩家已经钓到的鱼。

### 5. UI 与表现
主界面标题与按钮带正弦呼吸浮动；Canvas 采用 `Scale With Screen Size` 适配不同分辨率；
"抛竿—下沉—收钩"整体是一套状态机，保证动作在正确的时间点发生。

### 6. 摄像机
采用"重心切换"：抛竿前跟随渔夫，抛竿后平滑过渡到水下跟随鱼钩，
鱼钩回到水面后再拉回渔夫，避免玩家在场景里迷路。

---

## 技术栈

| 项 | 说明 |
|---|---|
| 引擎 | Unity **6000.3.21f1** |
| 模板 | 2D（`com.unity.feature.2d`） |
| 语言 | C# |
| UI | UGUI + TextMesh Pro |
| 物理 | 2D 物理（Rigidbody2D / Collider2D / Trigger） |
| 渲染 | Sprite Renderer + LineRenderer |
| 平台 | Windows |

---

## 项目结构

```
Assets/
├── Scenes/        Home / Land / Room / Ocean / Inshore / Offshore / DeepOcean
├── Scripts/       玩法脚本（StatusTable、HookControl、UWLineControl、BoatControl…）
├── Prefabs/       Player、Boat、Hook、Hook-UnderWater、UWLine、Camera、Fish1~8…
├── Animations/    角色与鱼的动作
├── Resources/     像素素材包（钓鱼、海底、UI、地图）
├── TextMesh Pro/  TMP 资源
├── *.mp3          背景音乐与脚步声、海浪声
└── Pixeled.ttf    像素字体
```

核心脚本一览：

| 脚本 | 作用 |
|---|---|
| `StatusTable.cs` | 跨场景单例，保存玩家 / 船只状态与鱼获背包 |
| `HookControl.cs` · `Hook_UWControl.cs` | 抛竿、下沉、收线，水面与水下两套控制 |
| `UWLineControl.cs` | 鱼线的 `LineRenderer` 绘制 |
| `BoatControl.cs` · `PlayerControl.cs` · `OnDockControl.cs` | 行走、划船、上下船 |
| `CameraControl.cs` · `Camera_UWControl.cs` | 摄像机重心切换与水下跟随 |
| `Fish1Control.cs` | 鱼类游动与朝向翻转 |
| `OceanControl.cs` · `RoomControl.cs` · `SP_RoomControl.cs` | 海面 / 室内 / 水族馆场景逻辑 |
| `MainMenuLogic.cs` · `StartControl.cs` · `QuitControl.cs` · `BackControl.cs` · `HelpControl.cs` | 菜单与界面流转 |

---

## 运行方式

1. 用 **Unity 6000.3.21f1** 打开本工程（`Library/` 未提交，首次导入会重新生成，需要几分钟）。
2. 打开 `Assets/Scenes/Home.unity`。
3. 点 Play。

> 工程里的包依赖（TextMesh Pro 3.0.6、Timeline 1.6.5 等）还是 Unity 2021/2022 时期的版本，
> 用 6000.3.21f1 首次打开时 Unity 可能会提示升级部分包，按提示确认即可。

---

## 素材来源

- 免费钓鱼像素艺术包 —— Free Game Assets
- [Ocean and Clouds Free Pixel Art Backgrounds](https://craftpix.net/) · [Beach Pixel Art Tileset](https://craftpix.net/) —— CraftPix.net
- 音乐 —— 官方中文泰拉瑞亚维基
- 音效 —— [Freesound](https://freesound.org/)
- 字体 —— [Omega PC777](https://www.dafont.com/) · dafont.com

以上素材版权归各自作者所有，本项目仅用于课程作业与学习交流。

---

## 团队

| 成员 | 分工 |
|---|---|
| 李泽铭 | 主体开发：脚本编写、动画制作、功能实现、整体结构与导出 |
| 杨政达 | 策划案整理、资源收集、主界面 UI 与部分背景 |

---

## License

课程大作业项目，代码仅供学习参考；第三方美术与音频素材的版权归原作者，请勿商用。
