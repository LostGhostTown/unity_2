# Shop
## UI设计
### 商店背景
### 商店项目UI
### 商店可购买项目价格UI
### 购买按钮
### 剩余金钱UI

## 逻辑
### 购买物品，扣除金钱
不够时显示提示
### 购买物品，增加物品
通过修改Gamemanager脚本中的数据实现

比如购买服务员，则waiterNum数量增加

比如购买解锁菜肴，则foodLock中对应key=food的value值改为1，表示解锁

## 退出游戏按钮

## 下一关按钮
跳转到InGame【场景】


## 音效
### 购买成功音效
### 购买失败音效

## 数据结构
### 商店项目数据————菜肴、价格、图标、描述
举例：
```CommonDesign.cs```中的Class: ```ShopItemInfo```

建议以预制体形式存放，临时生成也行




