# InGame_Play
## Ui设计（暂时完成）
### 摆菜台
### x道菜展品
### 出菜口
### 配菜

## 逻辑
### 点击获取菜肴   菜肴在tag:Food中
### 移动菜品入盘  （入盘操作已完成 代码FoodClickHandler）
### 提交菜盘，检测是否与订单匹配（ 只完成了部分功能 ：实现了餐盘清空，代码FoodClickHandler）
在Gamemanager中orderlist里面存储着订单的内容

需要匹配的数据结构为orderInfo类下的orderContent

时间一过会自动销毁订单，自然无法提交

包括菜盘消失
### 获取对应金钱
修改Gamemanager中的money数据
### 菜肴解锁与否
根据从Gamemanager的foodLock中获取的int数据判断，0为未解锁，1为已解锁

解锁则可以点击，未解锁则点击无效且显示加锁标志
### 时间限制（已完成）
结束后传递金钱数据

跳转商店页面

## 音效
### 获取菜肴音效
### 提交菜盘音效
### 匹配成功与失败音效

## 数据格式
### 菜肴数据————菜肴、出售价格、图标
举例：
```CommonDesign.cs```中的Class: ```FoodInfo```

建议，菜肴以预制体形式存放，临时生成也行

### 菜盘数据————Dictionary<菜肴数据，数量int类型>列表类型
举例：
```CommonDesign.cs```中的Class: ```OrderInfo```

菜盘以数据形式临时建立，在提交后销毁

### 订单数据————菜品目录Dictionary<菜肴数据，数量int类型>列表类型、可得金钱int类型
举例：
```CommonDesign.cs```中的Class: ```OrderInfo```

订单以预制体形式存放

### 匹配成功返回值———bool类型


