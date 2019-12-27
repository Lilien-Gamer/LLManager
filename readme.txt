1. 除了Editor文件夹 其余文件夹都放置Scripts下
2. excel配置表放置在 Assets/Configs下
3. UI窗体预制体路径在ConstConfig.cs中配置
4. 当前资源加载采用的resources的相关api

(1)ui框架
(2)对象池
(3)定时回调系统
(4)事件系统
(5)解析excel生成对应数据资源代码,在开发环境中生成

文件说明:
DataAsset: 用于存放生成的 数据代码
Editor:直接放入Editor目录后， 在编辑器生成对应菜单
Effect:存放了一个monobehavior用于生成 粒子系统后 挂载，执行回调
Event:事件系统相关代码
（1）EventManager:事件中心， 提供派发、注册、移除等事件方法（暂未对拆箱装箱做处理)
（2）EventType:用于事件的枚举
ModelBase:暂时无用
Resource:资源相关
（1）ConstConfig：一些配置信息，此时已有UI预制体的路径配置
（2）GameObjectPoor：Go对象的对象池，因为Go不能new，所以单独区别ObjectPool
（3）GoPoolManager：存有多个Go对象池的manager
（4）ObjectPool：一般可以直接new出来的对象的对象池
（5）ResourceManager：加载资源，存储资源，使用Resources.Load等api，暂时无卸载资源
UI:
（1）ItemView：与其他代码无耦合，点看查看源码根据需求是否适用。
（2）用于解决滚动视图中，复用子Go，以及解决布局带来的消耗，自写的工具类，具体说明看源码内注释，以及TestLoopList。
（3）MyMask，一个完全透明无色，并且不会被rebuild的遮罩组件
（4）OutRectClose挂载后当鼠标点击范围不在该UI范围内，则执行一个回调（没有为该回调赋值，则禁用该UI）
（5）RadarImage雷达图：看源码注释
（6）UIDrag：挂载该组件 为对应委托赋值，执行拖拽相关
（7）UIListener：挂载该组件，为对应委托赋值，执行点击相关。
（8）ViewBase：所有的窗口的基类。具体功能看源码。
（9）ViewManager：所有窗口的单例通过该类的相关api获取
（10）WndTips：因为发现几乎所有的游戏 都有弹出 tips信息的需求，想把它框架化，但是还没有完成。
Util：工具
（1）AudioManager：与项目剥离后的代码还未框架化。
（2）DataHelp：封装了一些处理数据的常用方法
（3）TimeManger：定时回调，提供了注册与注销等方法。
（4）UIUtil：UI的一些常用方法的封装