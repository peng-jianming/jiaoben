//Global 运行窗口//
Global 线程ID1, 线程ID2, 线程ID3, 线程ID4, 线程ID5, 线程ID6, 线程ID7, 线程ID8, 线程ID9, 线程ID10, 线程ID11, 线程ID12, 线程ID13, 线程ID14, 线程ID15
Global 线程卡ID1, 线程卡ID2, 线程卡ID3, 线程卡ID4, 线程卡ID5, 线程卡ID6, 线程卡ID7, 线程卡ID8, 线程卡ID9, 线程卡ID10, 线程卡ID11, 线程卡ID12, 线程卡ID13, 线程卡ID14, 线程卡ID15//卡判断用
DimEnv 梦幻窗口1外,梦幻窗口2外,梦幻窗口3外,梦幻窗口4外,梦幻窗口5外,梦幻窗口6外,梦幻窗口7外,梦幻窗口8外,梦幻窗口9外,梦幻窗口10外,梦幻窗口11外,梦幻窗口12外,梦幻窗口13外,梦幻窗口14外,梦幻窗口15外
DimEnv  梦幻窗口1内, 梦幻窗口2内, 梦幻窗口3内, 梦幻窗口4内, 梦幻窗口5内, 梦幻窗口6内, 梦幻窗口7内, 梦幻窗口8内, 梦幻窗口9内, 梦幻窗口10内, 梦幻窗口11内, 梦幻窗口12内, 梦幻窗口13内, 梦幻窗口14内, 梦幻窗口15内
Global 调试输出ID
DimEnv 速度
速度 = 1000
DimEnv 窗口数量
TracePrint "先执行"
//======
Call 大漠注册()
Call 句柄()
dm.SetPath ".\MyData"
//Form1.Grid1.SetCellType 1, 1, 1
//
//Form1.Grid1.SetCellType 2, 1, 1
// 启动获取句柄
Sub 按顺序执行()


    Call 控件值()
    Form1.SaveSetting
        Form2.SaveSetting
        Form3.SaveSetting //启动后保存界面
    Set dm = createobject("dm.dmsoft")
    Delay 10
    dm_ret = dm.SetDict(0, "TP\字库\字库.txt")
    Delay 10
    dm_ret = dm.SetDict(1, "TP\字库\捉鬼.txt")
    Delay 10
    dm_ret = dm.SetDict(2, "TP\字库\三界奇缘.txt")
    Delay 10
    dm_ret = dm.SetDict(3, "TP\字库\任务链.txt")
    Delay 10
    dm_ret = dm.SetDict(4, "TP\字库\摆摊出售.txt")
    Delay 10
    dm_ret = dm.SetDict(5, "TP\字库\整理背包.txt")
    Delay 10
//    dm_ret = dm.SetDict(6, "TP\字库\金币识别.txt")
    ver = dm.Ver()
    TracePrint "主程序运行窗口句柄=" & 运行窗口
    Call 获取该线程启动的窗口句柄()
    TracePrint "当前运行窗口句柄="&当前运行窗口
    dm_ret = dm.BindWindowEx(当前运行窗口, "dx.graphic.opengl", "windows", "windows", "", 0)// win10绑定
    Delay 10
    TracePrint "内容="&内容
    For 20
        Delay 100	
        TracePrint "绑定是否成功" & dm_ret
        If dm_ret = 1 Then 
            TracePrint "=1退出循环"
            Call 调试输出("绑定成功开始任务")
            Exit For
        End If
        If dm_ret = 0 Then 
            TracePrint "绑定失败"
            StopThread 线程卡ID1
            dm_ret = dm.ForceUnBindWindow(当前运行窗口)
            Call 调试输出("窗口绑定失败")
        End If
    Next 
    dm_ret = dm.SetDict(0,"TP\字库\字库.txt")
    TracePrint "绑定是否成功"&dm_ret
    If len(ver) = 0 Then
        MessageBox "创建对象失败,检查系统是否禁用了vbs脚本权限"
        EndScript
    End If
    //=======
    列表i=0
    内容 = Form1.ListBox2.List
    TracePrint 内容
    内容 = Split(内容, "|")
    //获取要执行表的第一格
    TracePrint 内容(列表i)
    列表总数 = Form1.ListBox2.ListCount
    TracePrint 列表总数
    For 列表总数
        TracePrint "次数"&Form1.ListBox2.ListCount
        If 内容(列表i) = "师门任务" Then 
            TracePrint "开始师门任务"
            call 调试输出当前任务("师门任务")
            Call 师门任务()
            TracePrint "师门完成"&列表i
        End If
        If 内容(列表i) = "自动排队" Then 
            TracePrint "开始自动排队"
            call 调试输出当前任务("自动排队")
            call 排队()
        End If
        If 内容(列表i) = "打藏宝图" Then 
            call 调试输出当前任务("打藏宝图")
            TracePrint "开始打藏宝图"
            call 打藏宝图()
        End If
        If 内容(列表i) = "运镖" Then 
            call 调试输出当前任务("运镖")
            TracePrint "开始运镖"
            call 运镖()
        End If
        If 内容(列表i) = "挖宝图" Then 
            call 调试输出当前任务("挖藏宝")
            TracePrint "开始挖宝图"
            call 挖宝图()
        End If
        If 内容(列表i) = "带队抓鬼" Then 
            call 调试输出当前任务("带队抓鬼")
            TracePrint "开始带队抓鬼"
            call 抓鬼()
        End If
        If 内容(列表i) = "秘境降妖" Then 
            call 调试输出当前任务("秘境降妖")
            TracePrint "开始秘境降妖"
            call 秘境降妖()
        End If
        If 内容(列表i) = "混队抓鬼" Then 
            call 调试输出当前任务("混队抓鬼")
            TracePrint "开始混队抓鬼"
            call 混队抓鬼()
        End If
        If 内容(列表i) = "三界奇缘" Then 
            call 调试输出当前任务("三界奇缘")
            TracePrint "开始三界奇缘"
            call 三界奇缘()
        End If
        If 内容(列表i) = "帮派任务" Then 
            call 调试输出当前任务("帮派任务")
            TracePrint "开始帮派任务"
            call 帮派任务()
        End If
        If 内容(列表i) = "任务链" Then 
            call 调试输出当前任务("任务链")
            TracePrint "开始任务链任务"
            call 任务链()
        End If
        If 内容(列表i) = "科举答题" Then 
            call 调试输出当前任务("科举任务")
            TracePrint "开始科举任务"
            call 科举答题
        End If
        If 内容(列表i) = "竞技场" Then 
            call 调试输出当前任务("竞技场")
            TracePrint "开始竞技场任务"
            call 竞技场()
        End If
        If 内容(列表i) = "自动喊话" Then 
            call 调试输出当前任务("自动喊话")
            TracePrint "开始自动喊话"
            call 自动喊话()
        End If
        If 内容(列表i) = "钓鱼" Then 
            call 调试输出当前任务("钓鱼")
            TracePrint "开始钓鱼"
            call 钓鱼()
        End If
        If 内容(列表i) = "秒商城" Then 
            call 调试输出当前任务("秒商城")
            TracePrint "开始秒商城"
            call 秒商城()
        End If
        If 内容(列表i) = "秒工坊" Then 
            call 调试输出当前任务("秒工坊")
            TracePrint "开始秒工坊"
            call 秒工坊()
        End If
        If 内容(列表i) = "活力使用" Then 
            call 调试输出当前任务("活力使用")
            TracePrint "开始活力使用"
            call 活力使用()
        End If
        If 内容(列表i) = "摆摊出售" Then 
            call 调试输出当前任务("摆摊出售")
            TracePrint "开始摆摊出售"
            call 摆摊出售()
        End If
        If 内容(列表i) = "整理背包" Then 
            call 调试输出当前任务("整理背包")
            TracePrint "开始整理背包"
            call 整理背包()
        End If
        If 内容(列表i) = "整理背包" Then 
            call 调试输出当前任务("整理背包")
            TracePrint "开始整理背包"
            call 整理背包()
        End If
        If 内容(列表i) = "获取信息" Then 
            call 调试输出当前任务("获取信息")
            TracePrint "开始获取信息"
            call 获取信息()
        End If
             If 内容(列表i) = "一键40级" Then 
            call 调试输出当前任务("一键40级")
            TracePrint "执行一键40级"
            call 一键40级()
        End If
                   If 内容(列表i) = "清理主线" Then 
            call 调试输出当前任务("清理主线")
            TracePrint "执行清理主线"
            call 清理主线
        End If
        
        列表i = 列表i + 1
        //走完一轮 列表+1
    Next
    //完成线程/取消检测/解除绑定
    If 线程ID1 = GetThreadID() Then 
        TracePrint "结束任务关闭检测"
        StopThread 线程卡ID1
        dm_ret = dm.ForceUnBindWindow(当前运行窗口)
        Call 调试输出("所有任务完成已解绑")
        Call 调试输出当前任务("已完成")
        Form1.结束1.Visible = 1
        Form1.Button1.Visible = 1
    End If
    If 线程ID2 = GetThreadID() Then 
        TracePrint "结束任务关闭检测"
        StopThread 线程卡ID2
        dm_ret = dm.ForceUnBindWindow(梦幻窗口2内)
        Call 调试输出("所有任务完成已解绑")
        Call 调试输出当前任务("已完成")
        Form1.结束2.Visible = 1
        Form1.Button2.Visible = 1
    End If
    If 线程ID3 = GetThreadID() Then 
        TracePrint "结束任务关闭检测"
        StopThread 线程卡ID3
        dm_ret = dm.ForceUnBindWindow(梦幻窗口3内)
        Call 调试输出("所有任务完成已解绑")
        Call 调试输出当前任务("已完成")
        Form1.结束3.Visible = 1
        Form1.开始3.Visible = 1
    End If
    If 线程ID4 = GetThreadID() Then 
        TracePrint "结束任务关闭检测"
        StopThread 线程卡ID4
        dm_ret = dm.ForceUnBindWindow(梦幻窗口4内)
        Call 调试输出("所有任务完成已解绑")
        Call 调试输出当前任务("已完成")
        Form1.结束4.Visible = 1
        Form1.开始4.Visible = 1
    End If
    If 线程ID5 = GetThreadID() Then 
        TracePrint "结束任务关闭检测"
        StopThread 线程卡ID5
        dm_ret = dm.ForceUnBindWindow(梦幻窗口5内)
        Call 调试输出("所有任务完成已解绑")
        Call 调试输出当前任务("已完成")
        Form1.结束5.Visible = 1
        Form1.开始5.Visible = 1
    End If
    If 线程ID6 = GetThreadID() Then 
        TracePrint "结束任务关闭检测"
        StopThread 线程卡ID6
        dm_ret = dm.ForceUnBindWindow(梦幻窗口6内)
        Call 调试输出("所有任务完成已解绑")
        Call 调试输出当前任务("已完成")
        Form1.结束6.Visible = 1
        Form1.开始6.Visible = 1
    End If
    If 线程ID7 = GetThreadID() Then 
        TracePrint "结束任务关闭检测"
        StopThread 线程卡ID7
        dm_ret = dm.ForceUnBindWindow(梦幻窗口7内)
        Call 调试输出("所有任务完成已解绑")
        Call 调试输出当前任务("已完成")
        Form1.结束7.Visible = 1
        Form1.开始7.Visible = 1
    End If
    If 线程ID8 = GetThreadID() Then 
        TracePrint "结束任务关闭检测"
        StopThread 线程卡ID8
        dm_ret = dm.ForceUnBindWindow(梦幻窗口8内)
        Call 调试输出("所有任务完成已解绑")
        Call 调试输出当前任务("已完成")
        Form1.结束8.Visible = 1
        Form1.开始8.Visible = 1
    End If
    If 线程ID9 = GetThreadID() Then 
        TracePrint "结束任务关闭检测"
        StopThread 线程卡ID9
        dm_ret = dm.ForceUnBindWindow(梦幻窗口9内)
        Call 调试输出("所有任务完成已解绑")
        Call 调试输出当前任务("已完成")
        Form1.结束9.Visible = 1
        Form1.开始9.Visible = 1
    End If
    If 线程ID10 = GetThreadID() Then 
        TracePrint "结束任务关闭检测"
        StopThread 线程卡ID10
        dm_ret = dm.ForceUnBindWindow(梦幻窗口10内)
        Call 调试输出("所有任务完成已解绑")
        Call 调试输出当前任务("已完成")
        Form1.结束10.Visible = 1
        Form1.开始10.Visible = 1
    End If
    If 线程ID11 = GetThreadID() Then 
        TracePrint "结束任务关闭检测"
        StopThread 线程卡ID11
        dm_ret = dm.ForceUnBindWindow(梦幻窗口11内)
        Call 调试输出("所有任务完成已解绑")
        Call 调试输出当前任务("已完成")
        Form1.结束11.Visible = 1
        Form1.开始11.Visible = 1
    End If
    If 线程ID12 = GetThreadID() Then 
        TracePrint "结束任务关闭检测"
        StopThread 线程卡ID12
        dm_ret = dm.ForceUnBindWindow(梦幻窗口12内)
        Call 调试输出("所有任务完成已解绑")
        Call 调试输出当前任务("已完成")
        Form1.结束12.Visible = 1
        Form1.开始12.Visible = 1
    End If
    If 线程ID13 = GetThreadID() Then 
        TracePrint "结束任务关闭检测"
        StopThread 线程卡ID13
        dm_ret = dm.ForceUnBindWindow(梦幻窗口13内)
        Call 调试输出("所有任务完成已解绑")
        Call 调试输出当前任务("已完成")
        Form1.结束13.Visible = 1
        Form1.开始13.Visible = 1
    End If
    If 线程ID14 = GetThreadID() Then 
        TracePrint "结束任务关闭检测"
        StopThread 线程卡ID14
        dm_ret = dm.ForceUnBindWindow(梦幻窗口14内)
        Call 调试输出("所有任务完成已解绑")
        Call 调试输出当前任务("已完成")
        Form1.结束14.Visible = 1
        Form1.开始14.Visible = 1
    End If
    If 线程ID15 = GetThreadID() Then 
        TracePrint "结束任务关闭检测"
        StopThread 线程卡ID15
        dm_ret = dm.ForceUnBindWindow(梦幻窗口15内)
        Call 调试输出("所有任务完成已解绑")
        Call 调试输出当前任务("已完成")
        Form1.结束15.Visible = 1
        Form1.开始15.Visible = 1
    End If
    //这是第一个窗口例子
End Sub
Sub 控件值()
    秘境战败停止 = Form1.战败停止.Value
    固定关卡退出 = Form1.固定关卡退出.Value
    秘境关卡数 = form1.秘境关卡.ListIndex + 1
    秘境每周选择 = Form1.秘境选择1.Value
    //抓鬼
    队伍等级上限 = Form1.队伍等级上限.Text
    队伍等级下限 = Form1.队伍等级下限.Text
    喊话选择 = Form1.一键喊话.ListIndex + 1
    抓鬼次数 = Form1.抓鬼次数.Text
    混队只数=Form1.混队只数.Text
    卡双倍=form1.抓鬼双倍下拉框.ListIndex 
    TracePrint "喊话="&喊话选择
    TracePrint 队伍等级上限
    TracePrint 队伍等级下限
    //帮派任务
    帮派任务选择 = Form1.帮派任务2.Value
    //任务链
    任务链不传说 = Form1.不传说.Value
    任务链所有物品传说 = Form1.所有物品传说.Value
    任务链指定价格传说 = Form1.指定价格传说.Value
    任务链传说价格 = Form1.指定价格传说1.Text
    //=======自动喊话
    喊话频道= Form1.自动喊话频道.ListIndex
    内容1 = Form1.内容1.Value
    内容2 = Form1.内容2.Value
    内容3 = Form1.内容3.Value
    喊话内容1 = Form1.喊话内容1.Text
    喊话内容2 = Form1.喊话内容2.Text
    喊话内容3 = Form1.喊话内容3.Text
    喊话间隔 = Form1.喊话间隔.Text
    喊话间隔 = 喊话间隔 * 1000
    喊话次数 = Form1.喊话次数.Text
    //秒商城
    秒货次数 = Form1.秒货数量.Text
    抢关注1 = Form1.抢关注.Value
    抢指定商品 = Form1.抢指定商品.Value
    抢指定金额 = Form1.抢指定金额.Text
    //秒工坊
    工坊次数 = Form1.工坊数量.Text
    工坊金额 = Form1.工坊金额.Text
    //
    //==========活力使用
    打工  = Form1.打工.Value
    制作临时符  = Form1.制作临时符.Value
    制作同心结  = Form1.制作同心结.Value
    炼药  = Form1.炼药.Value
    烹饪 = Form1.烹饪.Value
    //摆摊
    保留宝石 = Form1.保留宝石.Value
    大于保留 = Form1.摆摊大于保留.Text
    出价下调 = Form1.出价下调.Value
    保留大于勾选= Form1.保留大于物品.Value
    //钓鱼
    送大嘴熊鱼 = Form1.送大嘴熊鱼.Value
    双倍卖鱼 = Form1.双倍卖鱼.Value
    //整理背包
    九幽雅集 = Form1.九幽雅集.Value
    洞冥记 = Form1.洞冥记.Value
    日常药物= Form1.日常药物.Value
    培养材料 = Form1.培养材料.Value
    心魔宝珠= Form1.心魔宝珠.Value 
    阵法残卷 = Form1.阵法残卷.Value
    种子 = Form1.种子.Value
    帮派道具 = Form1.帮派道具.Value
    过期物品= Form1.过期物品.Value
    //结束后
End sub
//>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>=师门
Sub 师门任务()
    dm_ret = dm.UseDict(0)
    Dim 点掉左右叉
    点掉左右叉=0
    TracePrint "进入师门程序"
    Call 调试输出("开始师门任务")
    Rem 开始
    Call 回到主界面
    Call 点击活动
    Delay 速度
    For 3
        dm_ret = dm.FindStr(220, 132, 972, 516, "师门", "816955-2D3133", 0.9, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            师门X = intx
            师门Y = INTY
            TracePrint "找到师门任务"
            TracePrint intx
            TracePrint inty
            dm_ret = dm.FindStr(师门X, 师门Y, 师门X + 262, 师门Y + 62, "参", "6c310a-606060", 0.9, intX, intY)
            If intx >= 0 and inty >= 0 Then 
                TracePrint "找到参加，师门任务未完成"
                dm.MoveTo intx, inty
                Delay 50
                dm.LeftClick 
                call 找到师门选择()
                Exit For
            Else 
                dm_ret = dm.FindStr(师门X, 师门Y, 师门X + 262, 师门Y + 62, "完成", "4a8216-505050", 0.9, intX, intY)
                If intx >= 0 and inty >= 0 Then 
                    TracePrint "找到师门完成"
                    Call 调试输出("师门任务已完成")
                    Exit Sub
                End If
                TracePrint "没找到参加,师门任务结束"
            End If
        Else 
            TracePrint "没找到师门任务 开始下翻"
            call 活动内向上移动()
            Delay 1500
        End If
        Delay 1000
    Next 
    For 1000
        Call 不动检测()
        For 7
            Delay 1000
            dm_ret = dm.FindPic(825,610,1009,763, "TP\师门\对话框.bmp", "000000", 0.9, 0, intX, intY)
            If intx >= 0 and inty >= 0 Then 
                TracePrint "出现对话框"
                Call 调试输出("执行师门任务中~~")
                Call 判断有没有任务()
            Else 
                Call 调试输出("执行师门任务中~~")
                TracePrint "没有对话框"
                Delay 10
                Call 师门召唤兽购买上交()
                Delay 10
                Call 师门使用
                Delay 10
                Call 摆摊购买()
                Delay 10
                call  工坊购买()
                Delay 10
                Call 点击(407, 339, 585, 439, "TP\师门\完成弹窗.bmp", "000000", 0.9, 0, 1, 500)
                Delay 10
                Call 跳过对话()
                Delay 10
                Call 师门战斗中()
                Delay 1000
                Call 找到右侧师门2个字()
                //                Call 点击所有打叉()
            End If
            //-----------判断师门任务是否完成
            if 找图判断(101,170,270,306,"TP\师门\选择任务界面.bmp","000000",0.9,0,1,200)=1 Then
                TracePrint"出现师门任务选择界面"
                If 找图判断(290, 395, 925, 498, "TP\师门\去完成.bmp|TP\师门\继续任务.bmp|TP\师门\选择.bmp", "000000", 0.9, 0, 3, 200) = 1 Then 
                    TracePrint "找到去选择等（任务没完成）"
                Else 
                    TracePrint "3次没找到选择，师门任务已完成"
                    Call 调试输出("师门任务已完成")
                    Exit Sub
                End If
            End If
            If 点掉所有叉 = 3 Then 
                //运行两次 进行一次点叉判断
                Call 点击所有打叉
                点掉所有叉=0
            End If
            点掉所有叉 = 点掉所有叉 + 1
            TracePrint "执行1次====================================================="
        Next 
        If 不动判断 = True Then 
            TracePrint "师门任务卡了"
            Goto 开始
        End If
    Next  
End Sub
Sub 判断有没有任务()
    For 20
        dm_ret = dm.FindPic(715,127,999,620, "TP\任务框.bmp", "000000", 0.9, 0, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            TracePrint "有任务"
            // Call 师门右下角师门任务()
            Delay 1000
            dm_ret = dm.FindPic(715, 127, 999, 620, "TP\任务框.bmp", "000000", 0.9, 0, intX, intY)
            If intx >= 0 and inty >= 0 Then 
                TracePrint "第一行"&intx&","&inty
                TracePrint "点击第一行的任务"
                dm.MoveTo intx+50, inty+3
                Delay 100
                dm.LeftClick 
                Delay 1000
            End if
        Else 
            dm_ret = dm.FindPic(825,610,1009,763, "TP\师门\对话框.bmp", "000000", 0.9, 0, intX, intY)
            If intx >= 0 and inty >= 0 Then 
                TracePrint "出现对话框"
                TracePrint "没任务"
                dm.MoveTo 177, 626
                Delay 50 
                dm.LeftClick 
                Delay 10
                Exit For
            End if
        End If
    Next 
End Sub
Sub 截取掉的()
    Delay 10
    for 10
        Call 师门答题()
        dm_ret = dm.FindStr(699, 106, 814, 595, "请选择要", "eee1cf-303030", 0.8, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            TracePrint "有任务"
            If 点击(698, 155, 783, 607, "TP\师门\师门对话.bmp", "000000", 0.9, 0, 1, 200) = 1 Then 
                TracePrint "点击师门对话"
            Else 
                TracePrint"没出现师门对话"
            End If
        Else 
            dm_ret = dm.FindStr(699, 106, 814, 595, "请选择要", "eee1cf-303030", 0.8, intX, intY)
            If intx >= 0 and inty >= 0 Then 
                TracePrint "还有任务"
            Else 
                TracePrint "没任务，点掉"
                dm.MoveTo 177, 626
                Delay 50 
                dm.LeftClick 
                Delay 10
                TracePrint "点掉后执行到这"
                Exit For
            End if
        End If
        //选择答案 
        //===
        TracePrint "点掉后执行到这1"
        Delay 10
        Call 师门右下角师门任务()
        TracePrint "点掉后执行到这2"
        Delay 10
        Delay 1000
        dm_ret = dm.FindPic(825,610,1009,763, "TP\师门\对话框.bmp", "000000", 0.9, 0, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            TracePrint "对话框还在继续循环"
        Else 
            Exit For
            TracePrint "对话框不见了 结束循环"
        End If 
    Next 
End Sub
Sub 师门答题()
    dm_ret = dm.UseDict(2)
    For 5
        dm_ret = dm.FindStr(699, 106, 900, 595, "请选择正", "eee1cf-404040", 0.9, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            TracePrint "出现答题"
            If 点击(698, 155, 783, 607, "TP\师门\师门对话.bmp", "000000", 0.9, 0, 1, 200) = 1 Then 
                TracePrint "点击师门答案"
                Delay 1000
            Else 
                TracePrint"没出现师门答案"
            End If
        Else 
            TracePrint "没出现选择正确答案"
            Delay 100
            Exit For
        End If
        dm_ret = dm.FindStr(699, 106, 900, 595, "请选择正", "eee1cf-404040", 0.9, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            TracePrint "打错了，再答一次"
        Else 
            TracePrint "没出现选择正确答案，说明答对了"
            Exit Sub
        End If
    next           
End Sub
Sub 排队()
    //hwnds = dm.EnumWindow(0,"TheRender","RenderWindow",1+2)
    Call 获取信息()
End Sub
Sub 找右侧的师门任务()
    Delay 速度
    For 3
        dm_ret = dm.FindPic(433,82   ,595,320,"师门任务.bmp","101010",0.9,0,intX,intY)
        If intX >= 0 and intY >= 0 Then
            TracePrint "找到右侧师门任务"
            dm.MoveTo intx, inty
            Delay 500
            dm.LeftClick 
            Exit Sub
        Else 
            TracePrint "没找到右侧师门任务"
            Delay 100
        End If
    Next 
End Sub
Sub 找到师门选择()
    If 点击(290, 395, 925, 498, "TP\师门\去完成.bmp|TP\师门\继续任务.bmp|TP\师门\选择.bmp", "000000", 0.9, 0, 5, 200) = 1 Then 
        TracePrint "点击去完成"
        Call 调试输出("点击去完成开始师门")
    Else 
        TracePrint "没有找到去完成"
    End If
End Sub
Sub 找到右侧师门2个字()
    Call 队伍未选中就点击()
    dm_ret = dm.FindStr(789,87,1024,462, "师门", "B2AC34-4A4728", 0.9, intX, intY) 
    If intx >= 0 and inty >= 0 Then 
        TracePrint "第一次找到右侧师门"
        点掉所有叉 =0 //找到字  循环判断找叉=0
        Call 调试输出("点击师门任务")
        dm.MoveTo intx, inty
        Delay 10
        dm.LeftClick 
        call 等会对话框出现()
    Else 
        TracePrint "第一次没找到右侧师门2字"
        Delay 300
    End If
End Sub
Sub 师门战斗中()
    dm_ret = dm.UseDict(0)
    TracePrint "执行战斗中"
    For 50
        dm_ret = dm.FindStr(462,5,563,59, "战斗中1|战斗中2|战斗中3|战斗中4|战斗中5|战斗中6|战斗中7|战斗中8|战斗中9|战斗中0", "ffffee-777777", 0.9, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "出现战斗中数字"
            Call 调试输出("师门战斗中~~")
            //战斗中检查自动 如果没开 点击开
            If 找图判断(945, 302, 1021, 479, "TP\阵法\法术.bmp", "000000", 0.9, 1, 1, 100)=1 Then 
                If 点击(919,637,1018,755, "TP\阵法\自动.bmp", "000000", 0.9, 1, 1, 100) = 1 Then 
                    TracePrint "点击自动"
                End If
            End If
            Call 点击所有打叉
            Delay 2000
        Else 
            Delay 200
            Exit for
        End If
    Next
    TracePrint "没出现战斗 结束"
End Sub
Sub 队伍未选中就点击()
    dm_ret = dm.FindPic(800, 89, 901, 156, "TP\抓鬼\任务0.bmp", "000000", 0.9, 0, intX, intY)
    If intx >= 0 and inty >= 0 Then 
        dm.MoveTo intx,inty
        Delay 10
        dm.LeftClick 
        TracePrint "任务栏灰色点击"
        Delay 500
    Else 
        TracePrint "任务栏正常"
    End if
End Sub
Sub 等会对话框出现()
    For 5
        dm_ret = dm.FindPic(825, 610, 1009, 763, "TP\师门\对话框.bmp", "000000", 0.9, 0, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            TracePrint "出现对话框，等待结束"
            Exit For
        Else 
            TracePrint "没出现对话框,继续等待"
            Delay 500
            Call 师门召唤兽购买上交()
            Delay 10
            Call 师门使用
            Delay 10
            Call 摆摊购买()
            Delay 10
            call  工坊购买()
            Delay 10
            Call 点击(407, 339, 585, 439, "TP\师门\完成弹窗.bmp", "000000", 0.9, 0, 1, 500)
            Delay 10
            Call 跳过对话()
            Delay 10
            Call 战斗中()
            Delay 10
        End if
    Next 
End Sub
Sub 摆摊购买()
    Rem 开始
    dm_ret = dm.FindPic(424,73,578,133 ,"TP\师门\摆摊.bmp","000000",0.9,0,intX,intY)
    If intX >= 0 and intY >= 0 Then
        TracePrint "找到摆摊购买"
        dm_ret = dm.FindPic(287,203,925,610 ,"TP\师门\需求选中.bmp","000000",0.9,0,intX,intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "需求物品已经选中"
            dm.MoveTo 857, 643
            dm.LeftClick 
            TracePrint "点击购买"
            Delay 1000
        Else 
            TracePrint "需求物品没选中"
            dm_ret = dm.FindPic(287,203,925,610 ,"TP\师门\需求未选中.bmp|TP\师门\需求未选中1.bmp","101010",0.9,0,intX,intY)
            If intX >= 0 and intY >= 0 Then 
                Delay 100
                TracePrint "没选中进行选中"
                dm.MoveTo intx, inty
                Delay 10
                dm.LeftClick 
                TracePrint "点击选中"
                Delay 1500
                Goto 开始
            Else 
                Delay 1000
            End if
        End If
    Else 
        TracePrint "没找到摆摊购买"
        Delay 100
    End If
    dm_ret = dm.FindPic(424, 73, 578, 133, "TP\师门\摆摊.bmp", "000000", 0.9, 0, intX, intY)
    If intX >= 0 and intY >= 0 Then //在判断一次 买成功就找不到摆摊
        TracePrint "没买成功"
        Delay 100
        Goto 开始
    Else 
        TracePrint "买成功了"
    End If
End Sub
Sub 工坊购买()
    Rem 开始
    dm_ret = dm.FindPic(424,73,578,133 ,"TP\师门\工坊摆摊.bmp","000000",0.9,0,intX,intY)
    If intX >= 0 and intY >= 0 Then
        TracePrint "找到工坊摆摊购买"
        dm_ret = dm.FindPic(287,203,925,610 ,"TP\师门\工坊选中.bmp","000000",0.9,0,intX,intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "需求物品已经选中"
            dm.MoveTo 857, 643
            dm.LeftClick 
            TracePrint "点击购买"
            Delay 500
        Else 
            TracePrint "需求物品没选中"
            dm_ret = dm.FindPic(287,203,925,610 ,"TP\师门\需求未选中.bmp|TP\师门\需求未选中1.bmp|TP\师门\工坊未选中1.bmp","000000",0.9,0,intX,intY)
            If intX >= 0 and intY >= 0 Then 
                Delay 100
                TracePrint "没选中进行选中"
                dm.MoveTo intx, inty
                dm.LeftClick 
                TracePrint "点击选中"
                Goto 开始
            Else 
                Delay 100
            End if
        End If
    Else 
        TracePrint "没找到摆摊购买"
        Delay 100
    End If
    dm_ret = dm.FindPic(424, 73, 578, 133, "TP\师门\工坊摆摊.bmp", "000000", 0.9, 0, intX, intY)
    If intX >= 0 and intY >= 0 Then //在判断一次 买成功就找不到摆摊
        TracePrint "没买成功"
        Delay 100
        Goto 开始
    Else 
        TracePrint "买成功了"
    End If
End Sub
Sub 师门右下角师门任务()
    dm_ret = dm.FindStr(702,164,996,572, "师门|首席目标|交付召唤灵|岂有此理|听妨|我是来惩|孔雀石", "553923-7c6046", 0.8, intX, intY)
    If intX >= 0 and intY >= 0 Then
        TracePrint "找到师门右下角师门任务"
        dm.MoveTo intx, inty
        TracePrint "右下角" & intx & "," & inty
        dm.LeftClick 
        Exit Sub
    Else 
        TracePrint "没找到师门右下角师门任务"
        Delay 100
    End If
End Sub
Sub 师门使用
    For 1
        dm_ret = dm.FindPic(740,599,938,687 ,"TP\师门\使用.bmp","000000",0.9,0,intX,intY)
        If intX >= 0 and intY >= 0 Then
            TracePrint "出现师门使用"
            Call 调试输出("（点击使用）")
            dm.MoveTo intx, inty
            Delay 500
            dm.LeftClick 
            Delay 2000
            //使用后等待
            Exit Sub
        Else 
            TracePrint "没找到师门使用"
            Delay 100
        End If
    Next 
End Sub
Sub 师门兵器铺购买()
    师门兵器铺购买 = lw.findpic(641, 510, 957, 677, 程序路径 & "TP\师门\兵器铺购买.bmp", "000000", 1, 0.9, 0, 1, 0, 0, 100)
    If 师门兵器铺购买 = 1 Then 
        TracePrint "点击师门兵器铺购买"
        Call 调试输出("兵器购买")
        Delay 速度
    Else 
        Delay 100
    End If
End Sub
Sub 师门召唤兽购买上交()
    dm_ret = dm.FindPic(556,396,1016,754 ,"TP\师门\召唤兽购买.bmp|TP\师门\召唤兽上交.bmp","000000",0.7,0,intX,intY)
    If intX >= 0 and intY >= 0 Then
        TracePrint "出现师门动物购买"
        dm.MoveTo intx, inty
        Delay 500
        dm.LeftClick 
        Delay 速度
        Call 调试输出("师门召唤兽任务")
        //使用后等待
        Exit Sub
    Else 
        TracePrint "没出现师门动物购买"
        Delay 100
    End If
End Sub
Sub 师门出现上交()
    Delay 速度
    For 1
        dm_ret = dm.FindPic(430, 346  ,540, 410 ,"师门上交.bmp","101010",0.9,0,intX,intY)
        If intX >= 0 and intY >= 0 Then
            TracePrint "出现师门上交"
            dm.MoveTo intx, inty
            Delay 500
            dm.LeftClick 
            Delay 2000
            //使用后等待
            Exit Sub
        Else 
            TracePrint "没出现师门上交"
            Delay 100
        End If
    Next 
End Sub
//>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>=打宝图功能
Sub 打藏宝图()
    dm_ret = dm.UseDict(0)
    Dim 挖宝点叉i
    挖宝点叉i=0
    TracePrint "进入打宝图程序"
    Call 调试输出("开始藏宝图任务")
    Rem 开始
    TracePrint "跳到开始"
    当前脚本进度 = 0
    Call 回到主界面
    Call 点击活动
    Delay 速度
    For 3
        dm_ret = dm.FindStr(220, 132, 972, 516, "宝图", "816955-2D3133", 0.9, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            宝图X = intx
            宝图Y = INTY
            TracePrint "找到打宝图任务"
            TracePrint intx
            TracePrint inty
            dm_ret = dm.FindStr(宝图X, 宝图Y, 宝图X + 262, 宝图Y + 62, "参", "6c310a-606060", 0.9, intX, intY)
            If intx >= 0 and inty >= 0 Then 
                TracePrint "找到宝图任务，宝图任务未完成"
                dm.MoveTo intx, inty
                Delay 50
                dm.LeftClick 
                Exit For
            Else 
                dm_ret = dm.FindStr(宝图X, 宝图Y, 宝图X + 262, 宝图Y + 62, "完成", "4a8216-505050", 0.9, intX, intY)
                If intx >= 0 and inty >= 0 Then 
                    TracePrint "找到宝图完成"
                    Call 调试输出("宝图任务已完成")
                    Exit Sub
                End If
                TracePrint "没找到参加,宝图任务结束"
            End If
        Else 
            TracePrint "没找到宝图任务 开始下翻"
            call 活动内向上移动()
            Delay 1000
        End If
        Delay 1000
    Next
    For 10000
        Call 不动检测()
        For 5
            dm_ret = dm.FindPic(952,606,1008,741, "TP\师门\对话框.bmp", "000000", 0.9, 0, intX, intY)
            If intx >= 0 and inty >= 0 Then 
                TracePrint "出现对话框"
                Delay 500
                dm_ret = dm.FindStr(699, 106, 814, 595, "请选择", "eee1cf-303030", 0.8, intX, intY)
                If intx >= 0 and inty >= 0 Then 
                    TracePrint "有任务"
                    Call 师门右下角师门任务()//调用师门右下角弹窗
                Else 
                    TracePrint "没任务，点掉"
                    dm.MoveTo 177, 626
                    Delay 50 
                    dm.LeftClick 
                End If
            Else 
                TracePrint"没出现宝图对话"
            End If
            TracePrint "没有对话框"
            call 找到右侧宝图2个字()
            Call 战斗中()
            TracePrint "宝图进程中=战斗中"
            Call 跳过对话()
            If 当前脚本进度 ()= 1 Then 
                TracePrint "脚本不动了判断下是不是卡了或者已经完成"
                TracePrint"脚本进度=1"
                Goto 开始
            End If
            If 挖宝点叉i = 3 Then 
                TracePrint "三次后点击一次打叉"
                Call 点击所有打叉()
                挖宝点叉i=0
            End If
            挖宝点叉i=挖宝点叉i+1
        Next 
        If 不动判断 = True Then 
            TracePrint "主程序卡了"
            Goto 开始
        End If
    Next    
End Sub
Sub 判断宝图是否完成()
    call 点击活动()
    For 5
        Delay 1000
        dm_ret = dm.FindStr(128, 108,572, 344, "宝图", "5b402a-98826f", 0.8, intX, intY) or   dm_ret = dm.FindStr(128, 108,572, 344, "宝图", "6f5846-998a7e", 0.8, intX, intY)
        If intX >= 0 and intY >= 0 Then
            TracePrint "出现打宝图"
            TracePrint "intx,inty="&intx&","&inty
            dm_ret = dm.FindPic(intx,inty,intx+155,inty+42,"参加.bmp","101010",0.7,0,intX,intY)
            If intX >= 0 and intY >= 0 Then 
                TracePrint "宝图任务还没做点击参加"
                dm.MoveTo intx, inty
                Delay 500
                dm.LeftClick 
                宝图进度 = 0
                Exit Sub
            Else 
                宝图进度 = 1
                TracePrint "宝图任务完成了"
                Exit Sub
            End if
            //使用后等待
            Exit Sub
        Else 
            TracePrint "没找到藏宝图"
            i=294
            dm.MoveTo 349,294
            Delay 100
            dm.LeftDown 
            Delay 100
            For 10
                dm.MoveTo 349, i
                i = i - 20
                Delay 300
            Next
            Delay 100
            dm.LeftUp
            Delay 1000
            //            dm_ret = dm.FindPic(139, 117, 567, 336, "宝图任务完成.bmp", "101010", 0.9, 0, intX, intY)
            //            If intX >= 0 and intY >= 0 Then 
            //                宝图进度=1
            //                TracePrint "宝图任务已经完成"
            //                Exit Sub
            //            End If 
            //            TracePrint "没出现打宝图"
            //            Delay 500
        End If
    Next 
End Sub
Sub 打宝图过程()
    For 5000
        i=i+1
        Call 领取藏宝图()
        Delay 100
        Call 找到右侧宝图任务()
        Delay 100
        Call 战斗中()
        TracePrint "正在执行打宝图程序"	
        Delay 1000
        If 脚本进度 = 1 Then 
            TracePrint "打宝图不动了重新判断"
            Exit Sub
        End If
    Next 
End Sub
Sub 领取藏宝图()
    For 3
        dm_ret = dm.FindPic(398, 219   ,589, 400 ,"领取藏宝图.bmp","000000",0.9,0,intX,intY)
        If intX >= 0 and intY >= 0 Then
            TracePrint "找到领取藏宝图"
            Call 调试输出("【打宝图】领取宝图任务中....")
            dm.MoveTo intx, inty
            Delay 500
            dm.LeftClick 
            Exit Sub
        Else 
            TracePrint "没找到领取藏宝图"
            Delay 100
        End If
    Next 
End Sub
Sub 找到右侧宝图2个字()
    Call 队伍未选中就点击()
    dm_ret = dm.FindStr(789,87,1024,462, "宝", "B2AC34-4A4728", 0.9, intX, intY)
    If intx >= 0 and inty >= 0 Then 
        TracePrint "找到右侧宝图任务"
        Call 调试输出("点击宝图任务")
        dm.MoveTo intx, inty
        dm.LeftClick 
    Else 
        TracePrint "没找到宝图任务"
        Delay 300
    End If
End Sub
//>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>=挖宝图
Sub 挖宝图()
    TracePrint "进入挖宝图宝图程序"
    Call 调试输出("开始挖宝图")
    Rem 开始
    Call 回到主界面
    Delay 速度
    Call 点击包裹()
    Call 找到藏宝图并且使用()
    If 藏宝图进度 = 1 Then 
        TracePrint "宝图任务已完成"
        Call 调试输出("宝图任务已完成")
        Exit Sub
    End If
    For 10000
        Call 不动检测()
        For 5
            Call 不动检测()
            Call 师门使用()
            Delay 速度
            Call 战斗中
            Delay 1000
        Next
        If 不动判断 = True Then 
            TracePrint "挖宝卡了"
            Goto 开始
        End If
    Next 
End Sub
Sub 找到藏宝图并且使用()
    For 4
        dm_ret = dm.FindPic(512,198,910,600, "TP\宝图.bmp", "101010", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "点击藏宝图"
            dm.MoveTo intx, inty
            Delay 50
            dm.LeftClick 
        Else 
            Delay 500
            TracePrint "没有藏宝图"
            call 包裹上翻()
        End If
        Delay 1000
        dm_ret = dm.FindPic(147,252,496,551, "TP\使用.bmp", "101010", 0.9, 0, intX, intY)    
        If intX >= 0 and intY >= 0 Then 
            TracePrint "点击使用藏宝图"
            dm.MoveTo intx, inty
            Delay 50
            dm. LeftClick
            藏宝图进度 = 0
            Exit Sub
        End If
    Next
    藏宝图进度 = 1
    Exit Sub
    TracePrint "找了5遍没有"
End Sub
Sub 使用藏宝图()
    For 1
        dm_ret = dm.FindPic(427, 390, 556, 446, "藏宝图使用.bmp", "101010", 0.9, 0, intX, intY) //这个是找到标点后右下角弹出来的使用   
        If intX >= 0 and intY >= 0 Then 
            TracePrint "开始挖藏宝图"
            dm.MoveTo intx, inty
            Delay 50
            dm.LeftClick 
        Else 
            TracePrint "没有找到使用藏宝图"
        End If
    Next 
End Sub
//>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>=运镖
Sub 运镖()
    Call 调试输出("开始运镖~~~")
    运镖次数=0
    dm_ret = dm.UseDict(0)
    rem 开始
    Call 回到主界面
    Call 退出队伍()
    Call 点击活动
    call 判断活跃度
    If 判断活跃度 = False Then 
        TracePrint "活跃度不足结束任务"
        Call 调试输出("活跃度不足！！！结束押镖")
        Exit Sub
    Else 
        TracePrint "活跃度超过50，继续任务"
    End If
    For 3
        dm_ret = dm.FindStr(220, 132, 972, 516, "运镖", "816955-2D3133", 0.9, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            运镖X = intx
            运镖Y = INTY
            TracePrint "找到运镖任务"
            TracePrint intx
            TracePrint inty
            dm_ret = dm.FindStr(运镖X, 运镖Y, 运镖X + 262, 运镖Y + 62, "参", "6c310a-606060", 0.9, intX, intY)
            If intx >= 0 and inty >= 0 Then 
                TracePrint "找到运镖任务，运镖任务未完成"
                dm.MoveTo intx, inty
                Delay 50
                dm.LeftClick 
                Exit For
            Else 
                dm_ret = dm.FindStr(运镖X, 运镖Y, 运镖X + 262, 运镖Y + 62, "完成", "4a8216-505050", 0.9, intX, intY)
                If intx >= 0 and inty >= 0 Then 
                    TracePrint "找到运镖完成"
                    Call 调试输出("运镖任务已完成")
                    Exit Sub
                End If
                TracePrint "没找到参加,运镖任务结束"
            End If
        Else 
            TracePrint "没找到运镖任务 开始下翻"
            call 活动内向上移动()
            Delay 1000
        End If
    Next
    //判断是否在押镖
    For 100
        Call 不动检测()
        For 4
            dm_ret = dm.FindPic(223,49,343,106, "TP\运镖\运镖中.bmp", "000000", 0.9, 0, intX, intY)
            If intX >= 0 and intY >= 0 Then 
                TracePrint "运镖中"
                Call 调试输出("运镖中~~~~~")
                Call 运镖中()
            Else 
                Call 右下角选择普通镖()
                Call 确定押镖()
            End If
            Delay 2000
            TracePrint "不动1次"
        Next
        TracePrint "运镖次数="&运镖次数
        If 不动判断 = True Then 
            TracePrint "运镖次数="&运镖次数
            If 运镖次数 = 3 Then 
                TracePrint "运镖满3次结束"
                Call 调试输出("运镖三次完成")
                Delay 2000
                Call 运镖中()
                Exit Sub
            Else 
                Goto 开始
            End If
            TracePrint "卡了"
            Goto 开始
        End If
    Next 
End Sub
Sub 运镖中()
    For 120
        dm_ret = dm.FindPic(223, 49, 343, 106, "TP\运镖\运镖中.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "运镖中"
            Delay 1000
            Call 战斗中()
            Call 点击所有打叉()
        Else 
            TracePrint "运镖结束"
            Exit For
        End If
    Next 
End Sub
Sub 右下角选择普通镖()
    For 2
        dm_ret = dm.FindPic (699,257,1011,611, "TP\运镖\选择普通镖.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then
            TracePrint "找右下角弹出的运镖任务"
            dm. MoveTo intx, inty
            dm.LeftClick 
            运镖次数=运镖次数+1
            Exit For
        Else 
            TracePrint "没找到右下角弹出的运镖任务"
            Delay 1000
        End If
    Next 
End Sub
Sub 确定押镖()
    For 1
        dm_ret = dm.FindPic(296, 323, 719, 390, "TP\运镖\押金.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "出现押金"
            dm_ret = dm.FindPic(518, 407, 695, 496, "TP\运镖\确定.bmp", "000000", 0.9, 0, intX, intY)
            If intX >= 0 and intY >= 0 Then 
                TracePrint "点击确定"
                dm.MoveTo intx,inty
                dm.LeftClick 
                Exit Sub
            End If
        Else 
            Delay 1000
            TracePrint "等待押镖确定出现"
        End If
    Next 
End Sub
Function 判断活跃度()
    For 5
        TracePrint "判断活跃度"
        dm_ret = dm.FindPic(240, 617, 967, 631, "TP\运镖\活跃度.bmp", "000000", 0.8, 0, intX, intY)
        TracePrint "活跃度="&intx
        If intx >= 0 and inty >= 0 Then 
            If intx <= 548 Then 
                TracePrint "活跃不足50"
                判断活跃度 = False
                TracePrint "活跃度="&intx
            Else 
                TracePrint "活跃超过50"
                TracePrint "活跃度="&intx
                判断活跃度 = True
            End If
        End If
    Next 
End Function 
//>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>秘境降妖
Sub 秘境降妖
    Dim 正常
    dm_ret = dm.UseDict(0)
    TracePrint "进入秘境降妖程序"
    Call 调试输出("正在进行~秘境降妖")
    Rem 开始1
    Call 回到主界面
    Call 退出队伍()
    Call 点击活动
    Delay 速度
    Rem 开始
    For 3
        dm_ret = dm.FindStr(220, 132, 972, 516, "秘|境|降|妖", "553923-707070", 0.8, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            降妖X = intx
            降妖Y = inty
            TracePrint "y坐标="&inty
            If inty > 399 Then 
                TracePrint "太靠下移动一下"
                Call 活动内向上移动()
                Delay 1000
                Goto 开始
            End If    	
            TracePrint "找到秘境降妖"
            TracePrint intx
            TracePrint inty
            dm_ret = dm.FindPic (降妖X, 降妖Y, 降妖X + 262, 降妖Y+62, "TP\参加1.bmp|TP\参加.bmp", "000000", 0.8, 0, intX, intY)
            If intx >= 0 and inty >= 0 Then 
                TracePrint "找到参加，秘境降妖任务未完成"
                dm.MoveTo intx, inty
                Delay 50
                dm.LeftClick 
                Exit For
            Else 
                dm_ret = dm.FindStr(降妖X, 降妖Y, 降妖X + 262, 降妖Y + 62, "完成", "4a8216-505050", 0.9, intX, intY)
                If intx >= 0 and inty >= 0 Then 
                    TracePrint "找到秘境完成"
                    Call 调试输出("秘境降妖~已完成")
                    Exit Sub
                End If
                TracePrint "没找到参加,秘境任务结束"
            End If
        Else 
            TracePrint "没找到秘境务 开始下翻"
            call 活动内向上移动()
            Delay 1000
        End If
        Delay 1000
    Next
    //    上面的是活动中去找秘境降妖判断是否完成
    Call 右下角弹窗秘境降妖()
    call 秘境选择()
    call 秘境选择战斗()
    Call 秘境点击挑战()
    For 10000
        //点击右侧任务 没看到
        dm_ret = dm.FindPic(952,606,1008,741, "TP\师门\对话框.bmp", "101010", 0.9, 0, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            TracePrint "出现对话框"
            Delay 1000
            Call 点击所有打叉
            Call 秘境点击进入战斗()
        Else 
            Call 点击所有打叉
            TracePrint "没出现对话框执行 "
            //===================
            If 固定关卡退出 = 1 Then 
                For 3
                    s = dm.Ocr(772,206,862,433,"0BCE19-353535",0.9)
                    TracePrint "当前第" & s & "关"
                    TracePrint "指定关卡关闭="&秘境关卡数
                    If   s>= 0 Then 
                        TracePrint "目前秘境处于第" & S & "关"
                        Call 调试输出("【秘境降妖】第" & S & "关")
                        If int(s)= int(秘境关卡数) Then 
                            TracePrint "到达指定关卡"
                            Call 秘境离开()
                            Call 调试输出("【秘境降妖】到达指定关卡退出-秘境结束....")
                            Exit Sub
                        End If
                        Exit For
                    Else 
                        //                  
                        Delay 1000
                        TracePrint "当前关卡不退出="&S
                    End If
                Next
            Else 
            End If
            //================  
            Delay 200
            dm_ret = dm.FindPic (337,231,726,430, "TP\秘境降妖\失败.bmp", "000000", 0.9, 0, intX, intY)
            If intx >= 0 and inty >= 0 Then 
                TracePrint "出现秘境失败"
                Call 调试输出("【秘境降妖】-战斗失败-退出任务")
                dm.MoveTo 448,569
                dm.LeftClick 
                Delay 速度
                dm.MoveTo 448,569
                dm.LeftClick 
                Delay 速度
                call 秘境离开()
                If 秘境战败停止 = 1 Then 
                    Exit Sub
                End If
            Else 
                TracePrint "没出现战斗失败"
                Delay 1000
            End If
            Delay 200
            Call 秘境战斗中()
            If 固定关卡退出 = 1 Then 
                For 3
                    s = dm.Ocr(772,206,862,433,"0BCE19-353535",0.9)
                    TracePrint "当前第" & s & "关"
                    TracePrint "指定关卡关闭="&秘境关卡数
                    If   s>= 0 Then 
                        TracePrint "目前秘境处于第" & S & "关"
                        Call 调试输出("【秘境降妖】第" & S & "关")
                        If int(s)= int(秘境关卡数) Then 
                            TracePrint "到达指定关卡"
                            Call 秘境离开()
                            Call 调试输出("【秘境降妖】到达指定关卡退出-秘境结束....")
                            Exit Sub
                        End If
                        Exit For
                    Else 
                        //                  
                        Delay 1000
                        TracePrint "当前关卡不退出="&S
                    End If
                Next
            Else 
            End If
            //战斗中
            Delay 200
            Call 秘境右侧任务点击()
            Delay 200
        End if
    Next 
End Sub
Sub 秘境选择()
    For 3 
        dm_ret = dm.FindPic(338, 65, 690, 182, "TP\秘境降妖\每周选择.bmp", "000000", 0.9, 0, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            TracePrint "出现每周选择"
            If 秘境每周选择 = 1 Then  //=1 进入海底秘境 =0进入日月之井
                Call 海底秘境()
            Else 
                Call 日月之井
            End If
            TracePrint "每周选择结束"
            Exit Sub
        Else 
            Delay 1000
        End If
    Next 
End Sub
Sub 海底秘境()
    dm.MoveTo 285,628
    Delay 10
    dm.LeftClick 
    Delay 2000
    Call 确定秘境()
End Sub
Sub 日月之井()
    dm.MoveTo 722,622
    Delay 10
    dm.LeftClick 
    Delay 2000
    Call 确定秘境()
End Sub
Sub 确定秘境()
    dm_ret = dm.FindPic(283,243,754,504, "TP\秘境降妖\每周确定.bmp", "000000", 0.9, 0, intX, intY)
    If intx >= 0 and inty >= 0 Then 
        TracePrint "出现秘境每周选择确定"
        dm.MoveTo 604,451
        Delay 10
        dm.LeftClick 
    End if
End Sub
Sub 秘境离开()
    For 5
        dm_ret = dm.FindPic (765,139,1020,567, "TP\秘境降妖\离开1.bmp", "000000", 0.9, 0, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            TracePrint "秘境离开操作"
            Delay 10
            TracePrint "离开坐标="&intx&","&inty
            dm.MoveTo intx, inty
            Delay 10
            dm.LeftClick 
            TracePrint "点击离开"
            Exit For
        Else 
            TracePrint "还没出现地图离开"
            Delay 500
        End If
    Next 
End Sub
Sub 秘境右侧任务点击()
    // dm_ret = dm.FindStr(751,224,1020,477, "秘境降妖", "f9f900-707070", 0.9, intX, intY)
    dm_ret = dm.FindPic (787,94,1017,543, "TP\秘境降妖\秘境降妖1.bmp", "000000", 0.9, 0, intX, intY)
    If intx  >= 0 and inty >= 0 Then 
        TracePrint "出现右侧秘境任务"
        dm.MoveTo intx, inty
        Delay 10
        dm.LeftClick 
        Delay 10
        //        TracePrint intx & "," & inty
        //        地图X = intx
        //        地图Y=inty
        //        TracePrint "出现右侧秘境任务"
        //        TracePrint dm_ret
        //          dm_ret = dm.FindPic (77,14,215,59, "TP\秘境降妖\月之井.bmp", "000000", 0.8, 0, intX, intY)
        //             If intx >= 0 and inty >= 0 Then 
        //             TracePrint "月之井点击右侧任务"
        //    Delay 10
        //        dm.MoveTo 地图X, 地图Y-197
        //        Delay 10
        //        dm.LeftClick 
        //        Delay 10
        //            
        //            Else 
        //            TracePrint "不在月之井"
        //                   Delay 10
        //        dm.MoveTo 地图X, 地图Y-50
        //        Delay 10
        //        dm.LeftClick 
        //        Delay 10  
        //                 
        //            End If
    Else 
        TracePrint "没找到右侧秘境数字"
        Delay 1000
    End If
    Delay 3000
End Sub
Sub 秘境点击进入战斗()
    dm_ret = dm.FindStr(773,361,1041,499, "进入站斗", "553923-606060", 0.9, intX, intY)//战字写错
    If intx >= 0 and inty >= 0 Then //点击进入战斗
        TracePrint "找到进入战斗"
        dm.MoveTo intx, inty
        Delay 10
        dm.LeftClick 
    Else 
        Delay 1000
    End If
End Sub
Sub 右下角弹窗秘境降妖()
    For 20
        dm_ret = dm.FindStr(702,164,996,572, "秘境降妖", "553923-985f46", 0.8, intX, intY)
        If intX >= 0 and intY >= 0 Then
            TracePrint "找右下角弹出的秘境任务"
            dm. MoveTo intx, inty
            dm.LeftClick 
            正常=True
            Exit For
        Else 
            TracePrint "没找到右下角弹出的秘境任务"
            Delay 1000
        End If
    Next 
End Sub
Sub 秘境选择战斗()
    For 20
        dm_ret = dm.FindPic (30,36,1011,720, "TP\秘境降妖\排行.bmp|TP\秘境降妖\秘境降妖.bmp", "000000", 0.9, 0, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            Delay 1000
            TracePrint "已经打开秘境降妖界面"
            //          dm_ret = dm.FindPic(68,195,576,678, "TP\秘境降妖\选择.bmp|TP\秘境降妖\选择1.bmp", "000000", 0.9, 0, intX, intY)
            //
            //            If intx >= 0 and inty >= 0 Then 
            //                TracePrint "选择坐标"&intx&","&inty
            //                TracePrint "找到秘籍内要选择的"
            //                dm.MoveTo intx - 62, inty - 40
            //                dm.leftclick 
            //                Exit For
            //                Else 
            //                TracePrint "没找到怪物选择"
            //                Delay 10
            //            End If
            Call 找怪()
            Exit For
        Else 
            TracePrint "没打开秘境界面"
            Delay 1000
        End If
    Next 
End Sub
Sub 找怪()
    Dim 怪坐标X
    Dim 怪坐标y
    怪坐标X = 70
    怪坐标y= 200
    For 15
        For 15
            dm.moveto 怪坐标X, 怪坐标Y
            Delay 10
            dm.LeftClick 
            TracePrint "怪物点击坐标="&怪坐标X&","&怪坐标Y
            Delay 10
            dm_ret = dm.FindPic (285,233,832,589, "TP\秘境降妖\录像.bmp", "000000", 0.9, 0, intX, intY)
            If intx >= 0 and inty >= 0 Then 
                TracePrint "找到录像"
                TracePrint "已经找到怪"
                Exit Sub
            Else 
            end if 
            怪坐标Y=怪坐标Y+30
        Next
        怪坐标X = 怪坐标X + 30
        怪坐标Y=200
    Next 
End Sub
Sub 秘境点击挑战()
    For 20
        dm_ret = dm.FindPic (285,233,832,589, "TP\秘境降妖\录像.bmp", "000000", 0.9, 0, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            Delay 1000
            dm_ret = dm.FindPic(409,483,679,573, "TP\秘境降妖\挑战.bmp", "000000", 0.9, 0, intX, intY)
            If intx >= 0 and inty >= 0 Then 
                TracePrint "出现挑战并点击"
                dm.MoveTo intx , inty 
                dm.leftclick 
                Exit For
            End If
        Else 
            TracePrint "没打开秘境挑战界面"
            Delay 1000
        End If
    Next 
End Sub
Sub 退出队伍()
    Call 打开队伍()
    For 3
        dm_ret = dm.FindPic(54,621,263,682, "TP\退出队伍.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "退出队伍"
            dm.MoveTo intx, inty
            Delay 10
            dm.LeftClick 
            Delay 1000
        Else 
            TracePrint "没有队伍"
            Delay 500
        End If
        dm_ret = dm.FindPic(54,621,263,682, "TP\创建队伍.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "再次确定没有队伍，目前是创建队伍"
            dm.MoveTo 920,103
            Delay 10
            dm.LeftClick 
            Exit For
        Else 
            Delay 200
        End If
    Next 
End Sub
sub 秘境战斗中()
    dm_ret = dm.UseDict(0)
    TracePrint "执行战斗中"
    For 50
        dm_ret = dm.FindStr(462,5,563,59, "战斗中1|战斗中2|战斗中3|战斗中4|战斗中5|战斗中6|战斗中7|战斗中8|战斗中9|战斗中0", "ffffee-777777", 0.9, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "出现战斗中数字"
            Call 调试输出("【战斗中】第"&s&"关")
            //战斗中检查自动 如果没开 点击开
            If 找图判断(945, 302, 1021, 479, "TP\阵法\法术.bmp", "000000", 0.9, 1, 1, 100)=1 Then 
                If 点击(919,637,1018,755, "TP\阵法\自动.bmp", "000000", 0.9, 1, 1, 100) = 1 Then 
                    TracePrint "点击自动"
                End If
            End If
            Call 点击所有打叉
            Delay 2000
        Else 
            dm_ret = dm.FindPic (337,231,726,430, "TP\秘境降妖\失败.bmp", "000000", 0.9, 0, intX, intY)
            If intx >= 0 and inty >= 0 Then 
                TracePrint "出现秘境失败"
                Exit Sub
            Else 
                TracePrint "没出现战斗失败"
                dm.MoveTo 472,366
                Delay 10
                dm.LeftClick   
            End If
            For 3
                s = dm.Ocr(772,206,862,433,"0BCE19-353535",0.9)
                TracePrint "当前第" & s & "关"
                TracePrint "指定关卡关闭="&秘境关卡数
                If   int(s)>= 0 Then 
                    TracePrint "目前秘境处于第" & S & "关"
                    Call 调试输出("【寻怪中】第" & S & "关")
                    Exit For
                Else 
                    //                  
                    Delay 300
                    TracePrint "当前关卡不退出="&S
                End If
            Next
            Delay 200
            Exit for
        End If
    Next
    TracePrint "没出现战斗 结束"
End Sub
//>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>抓鬼
Sub 抓鬼()
    dm_ret = dm.UseDict(0)
    Dim 抓鬼点叉i
    抓鬼点叉i=0
    抓鬼一轮 = 0
    鬼数 = 0
    rem 开始1
    Call 回到主界面
    Call 战斗中()
    For 6
        dm_ret = dm.FindStr(789,87,1024,462, "捉鬼", "B2AC34-4A4728", 0.9, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "发现已经领取任务"
            Goto 跳过创建队伍
        Else 
            TracePrint "没找到右侧捉鬼2个字"
            Delay 200
        End If
    Next
    For 3
        dm_ret = dm.FindPic(0,0,155,199, "TP\抓鬼\回到长安.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "正在长安，不用去领鬼任务"
            Delay 1000
            Exit For
        Else 
            TracePrint "领取抓鬼任务"
            Call 领取抓鬼任务()
            Call 关闭抓鬼对话框()
            Exit For
        End If
    Next
    Call 打开队伍()
    Delay 1000
    call  队伍中判断踢离线()
    dm_ret = dm.FindPic(848, 184, 928, 254, "TP\抓鬼\人满.bmp", "000000", 0.9, 0, intX, intY)
    If intX >= 0 and intY >= 0 Then 
        TracePrint "队伍人满的"
        Goto 人满的跳
    End if
    Call 创建队长
    Call 调整队伍目标()
    Call 行动目标()
    Call 队伍要求下限()
    Call 队伍要求上限()
    Call 当前喊话()
    Rem 人满的跳
    Call 领取抓鬼任务()
    Rem 满一轮
    Call 师门右下角抓鬼任务()
    call  信息框()
    Rem 跳过创建队伍
    For 100000
        Delay 1000
        Call 不动检测()
        Rem 战斗后退出重新判断
        For 10
            Call 找到右侧抓鬼2个字()
            call 抓鬼失败()
            Call 抓鬼战斗中()
            If 进入战斗鬼 = 1 Then //战斗执行完如果进入战斗过，那么重新执行卡顿判断
                进入战斗鬼 = 0
                Goto 战斗后退出重新判断
            End If
            Call 不足5人()
            Call 双倍不足()
            //抓完一轮
            //            For 4
            //                dm_ret = dm.FindPic(333,401,492,466, "TP\信息4.bmp", "000000", 0.9, 0, intX, intY)
            //                If intx >= 0 and inty >= 0 Then 
            //                    TracePrint "出现弹窗"
            //                    For 20
            //                      Call 不足5人()
            //            Call 双倍不足()
            //                        dm_ret = dm.FindPic(312,333,729,392, "TP\抓鬼\抓完.bmp|TP\抓鬼\抓完1.bmp|TP\抓鬼\继续抓鬼.bmp", "101010", 0.9, 0, intX, intY)
            //                        If intx >= 0 and inty >= 0 Then 
            //                            TracePrint "出现抓鬼抓完"
            //                            dm_ret = dm.FindPic(513,385,695,471, "TP\抓鬼\确定.bmp", "101010", 0.9, 0, intX, intY)
            //                            If intx >= 0 and inty >= 0 Then 
            //                                dm.moveto intx, inty
            //                                dm.LeftClick 
            //                                TracePrint "点击确定"
            //                                TracePrint "抓鬼次数=" & 抓鬼次数
            //                                TracePrint "抓鬼一轮=" & 抓鬼一轮
            //                                If int(抓鬼一轮) = int(抓鬼次数) Then 
            //                                    TracePrint "完成指定次数，任务结束"
            //                                    TracePrint "抓鬼次数=" & 抓鬼次数
            //                                    TracePrint "抓鬼一轮=" & 抓鬼一轮
            //                                    call 关闭抓鬼对话框()
            //                                    Exit Sub
            //                                End If
            //                                抓鬼一轮 = 抓鬼一轮 + 1
            //                                鬼数 = 0
            //                                Goto 满一轮
            //                            End If
            //                        Else 
            //                            Delay 1000
            //                            TracePrint "出现弹窗但是  没找到抓鬼一轮"
            //                        End If
            //                    Next 
            //                Else 
            //                    TracePrint "没出现弹框"
            //                    Delay 500
            //                End If
            //            Next 
            For 5
                dm_ret = dm.FindPic(333,401,492,466, "TP\信息4.bmp", "000000", 0.9, 0, intX, intY)
                If intx >= 0 and inty >= 0 Then 
                    TracePrint "出现信息框"
                    If 抓鬼完成一轮 = True Then 
                        TracePrint "点击成功"
                        If int(抓鬼一轮) = int(抓鬼次数) Then 
                            TracePrint "完成指定次数，任务结束"
                            TracePrint "抓鬼次数=" & 抓鬼次数
                            TracePrint "抓鬼一轮=" & 抓鬼一轮
                            call 关闭抓鬼对话框()
                            Exit Sub
                        End If
                        Goto 满一轮
                    End If
                    Delay 100
                    If 双倍不足 = True Then 
                        TracePrint "点击双倍不足成功"
                        Exit For
                    End If
                    Delay 100
                    If 不足5人() = True Then 
                        TracePrint "点击不足五人取消"
                        Exit For
                    End If
                End If
                Delay 1000
            Next 
            //=========
            If 抓鬼点叉i = 5 Then 
                //运行三次后点击叉
                Call 点击所有打叉()
                抓鬼点叉i=0
            End If
            抓鬼点叉i=抓鬼点叉i+1
        Next
        Delay 1000
        If 不动判断 = True Then 
            TracePrint "卡了"
            if 卡住在任务中传送()=False then
                TracePrint "抓鬼任务没了"
                Goto 开始1
            End If 
            If 当前任务没有鬼 = 1 Then 
                当前任务没有鬼 = 0
                Goto  开始1
            End If
        End If
    Next  
End Sub
Sub 抓鬼失败()
    dm_ret = dm.FindPic (337,231,726,430, "TP\秘境降妖\失败.bmp", "000000", 0.9, 0, intX, intY)
    If intx >= 0 and inty >= 0 Then 
        TracePrint "战斗失败"
        dm.MoveTo 512, 582
        Delay 10
        dm.LeftClick 
    End if
End Sub
Sub 抓鬼卡了()
    Call 抓鬼战斗中()
    Call 点击所有打叉()
End Sub
Function 出现弹窗()
    For 5
        dm_ret = dm.FindPic(333,401,492,466, "TP\信息4.bmp", "000000", 0.9, 0, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            TracePrint "出现信息框"
            If 抓鬼完成一轮 = True Then 
                TracePrint "点击成功"
            End If
            If 双倍不足 = True Then 
                TracePrint "点击双倍不足成功"
            End If
            If 不足5人() = True Then 
                TracePrint "点击不足五人取消"
            End If
        End If
        Delay 1000
    Next 
End Function
Sub 关闭抓鬼对话框()
    For 30
        dm_ret = dm.FindPic(825, 610, 1009, 763, "TP\师门\对话框.bmp", "000000", 0.9, 0, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            dm.MoveTo intx - 300, inty
            Delay 10
            dm.LeftClick 
            Delay 速度
            Exit Sub
        Else 
            Delay 1000
        End If
    Next  
End Sub
Sub 混队抓鬼()
    Call 调试输出("【混队抓鬼】开始混队抓鬼.....")
    鬼数 = 0
    混鬼=0
    Call 战斗中
    For 10
        dm_ret = dm.FindStr(789,87,1024,462, "捉鬼", "B2AC34-4A4728", 0.9, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "发现已经领取任务"
            Goto 跳过创建队伍
        Else 
            TracePrint "没找到右侧捉鬼2个字"
            Delay 200
        End If
    Next 
    rem 开始
    Call 回到主界面
    Call 退出队伍()
    Call 领取抓鬼任务()
    Call 抓鬼右下角便携组队()
    Call 点击便携组队自动匹配()
    Call 判断进队成功()
    Rem 跳过创建队伍
    For 10000
        For 40
            Call 不动检测
            Call 双倍不足()
            Call 混队抓鬼战斗中()
            Call 回到长安()
            TracePrint "鬼数=" & 鬼数
            TracePrint "混队只数="&混队只数
            If int(鬼数) = int(混队只数) Then 
                TracePrint "退出抓鬼"
                Call 退出队伍()
                Exit Sub
            End If
            Delay 1000
        Next
        If 不动判断 = True Then 
            Call 回到主界面()
            Call 退出队伍()
            Goto  开始
        End If
    Next 
End Sub
Sub 回到长安()
    dm_ret = dm.FindPic(0,0,155,199, "TP\抓鬼\回到长安.bmp", "000000", 0.9, 0, intX, intY)
    If intX >= 0 and intY >= 0 Then 
        抓鬼一轮 = 抓鬼一轮 + 1 
        混鬼 = 0
    End if 
End Sub
Sub 抓鬼右下角便携组队()
    For 50
        dm_ret = dm.FindPic(700,164,986,616, "TP\抓鬼\便携组队1.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then
            TracePrint "找到右下角便携组队"
            dm.MoveTo intx, inty
            TracePrint "右下角" & intx & "," & inty
            Delay 10
            dm.LeftClick 
            Delay 速度
        Else 
            TracePrint "没找到右下角便携组队"
            Delay 500
        End If
        dm_ret = dm.FindPic(80,130,293,683, "TP\抓鬼\便携组队鬼.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "成功打开便携抓鬼界面"
            Exit Sub
            Delay 200
        Else 
            TracePrint "没打开便携组队"
        End if
    Next  
End Sub
Sub 点击便携组队自动匹配()
    For 20
        dm_ret = dm.FindPic(530, 556, 743, 719, "TP\抓鬼\自动匹配2.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "找到自动匹配点击"
            dm.MoveTo intx, inty
            Delay 10
            dm.LeftClick 
            Delay 500
        Else 
            Delay 500
        End If
        dm_ret = dm.FindPic(530, 556, 743, 719, "TP\抓鬼\取消自动匹配.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "成功点击自动匹配"
            Exit Sub
        End If
    Next 
End Sub
Sub 判断进队成功()
    Rem 开始
    For 500
        dm_ret = dm.FindPic(410,63,589,168, "TP\抓鬼\队伍3.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "进队成功"
            dm.MoveTo 930, 105
            Delay 10
            dm.LeftClick 
            Delay 1000
            Exit For
        Else 
            Delay 1000
            TracePrint "等待进队中"
            Call 调试输出("【混队抓鬼】等待队伍中.....")
        End If
    Next 
    dm_ret = dm.FindPic(410,63,589,168, "TP\抓鬼\队伍3.bmp", "000000", 0.9, 0, intX, intY)
    If intX >= 0 and intY >= 0 Then 
        TracePrint "还在队伍中"
        Goto 开始
    Else 
        TracePrint "关闭成功"
        Exit Sub
    End If
End Sub
Sub 混队抓鬼战斗中()
    dm_ret = dm.UseDict(0)
    dm_ret = dm.FindStr(462, 5, 563, 59, "战斗中1|战斗中1|战斗中2|战斗中3|战斗中4|战斗中5|战斗中6|战斗中7|战斗中8|战斗中9|战斗中0", "ffffee-777777", 0.9, intX, intY)
    If intX >= 0 and intY >= 0 Then 
        TracePrint 进入战斗
        进入战斗鬼=1
        鬼数 = 鬼数 + 1
        混鬼=混鬼+1
        Call 调试输出("【混队捉鬼中】"&"第"&鬼数&"只"&"-正在战斗")
        Call 混队判断卡双()
        Delay 1000
        For 150
            dm_ret = dm.FindStr(462,5,563,59, "战斗中1|战斗中2|战斗中3|战斗中4|战斗中5|战斗中6|战斗中7|战斗中8|战斗中9|战斗中0", "ffffee-777777", 0.9, intX, intY)
            If intX >= 0 and intY >= 0 Then 
                TracePrint "出现战斗中数字"
                //战斗中检查自动 如果没开 点击开
                If 找图判断(945, 302, 1021, 479, "TP\阵法\法术.bmp", "000000", 0.9, 1, 1, 100)=1 Then 
                    If 点击(919,637,1018,755, "TP\阵法\自动.bmp", "000000", 0.9, 1, 1, 100) = 1 Then 
                        TracePrint "点击自动"
                    End If
                End If
                call 点击所有打叉()
                Delay 1000
            Else 
                Delay 300
                Call 调试输出("【混队捉鬼中】"&"第"&鬼数&"只"&"-战斗结束")
                Exit for
            End If
        Next
        Delay 1000
        TracePrint "没出现战斗 结束"
    End If
End Sub
//混队抓鬼上方的
Sub 打开队伍()
    For 100
        dm_ret = dm.FindPic(915,100,1018,147, "TP\抓鬼\队伍0.bmp|TP\抓鬼\队伍1.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint intx&","&inty
            TracePrint "成功点击队伍"
            dm.MoveTo intx, inty
            dm.LeftClick 
            Delay 1000
        Else 
            TracePrint "没找到队伍"
            Delay 1000
        End if
        dm_ret = dm.FindPic(246,623,339,673, "TP\抓鬼\便携组队.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "成功打开队伍界面"
            Delay 速度
            Exit For
        Else 
            TracePrint "没找到便携组队"
        End If
    Next 
End Sub
Sub 创建队长()
    dm_ret = dm.FindPic(45, 615, 245, 685, "TP\抓鬼\创建队伍.bmp", "000000", 0.9, 0, intX, intY)
    If intX >= 0 and intY >= 0 Then 
        TracePrint "创建队长"
        dm.MoveTo intx, inty
        dm.LeftClick 
        Delay 1000
    Else 
        TracePrint "已经是队长更不需要创建"
    End If
End Sub
Sub 调整队伍目标()
    For 10
        dm_ret = dm.FindPic(636,138,733,236, "TP\抓鬼\调整目标.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "成功点击调整目标"
            dm.MoveTo intx, inty
            dm.LeftClick 
            Delay 1000
        End If
        dm_ret = dm.FindPic(250,622,434,677, "TP\抓鬼\调整目标界面.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "成功打开调整目标界面"
            Exit For
        End If
    Next 
End Sub
Sub 行动目标()
    For 5
        dm_ret = dm.FindPic(276,187,491,598, "TP\抓鬼\目标抓鬼.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "点击目标抓鬼任务"
            dm.MoveTo intx, inty
            dm.LeftClick 
            Delay 1000
            Exit For
        Else 
            Delay 1000
        End If
    Next 
End Sub
Sub 队伍要求下限()
    Call 调试输出("【带队抓鬼】调整等级下限中....")
    For 100
        队伍等级 = dm.Ocr(533,267,577,305, "6E533C-191A19", 0.9)
        TracePrint 队伍等级
        TracePrint  队伍等级下限 
        If int(队伍等级) = int(队伍等级下限) Then 
            TracePrint "等级已匹配"
            Exit For
        End If
        If int(队伍等级) < int(队伍等级下限) Then 
            Call 队伍下限等级加1
            Delay 1500
        End If
        If int(队伍等级) > int(队伍等级下限) Then 
            Call 队伍下限等级减1
            Delay 1500
        End If
        Delay 500
    Next
End Sub
Sub 队伍要求上限
    Call 调试输出("【带队抓鬼】调整等级上限中....")
    For 140
        队伍等级 = dm.Ocr(641,264,693,303, "6E533C-191A19", 0.9)
        TracePrint 队伍等级
        TracePrint 队伍等级上限
        If int(队伍等级) = int(队伍等级上限) Then 
            TracePrint "等级已匹配"
            Exit For
        End if
        If int(队伍等级) < int(队伍等级上限) Then 
            TracePrint "执行+1"
            Call 队伍上限等级加1
            Delay 1500
        End If
        If int(队伍等级) > int(队伍等级上限) Then
            TracePrint "执行-1"
            Call 队伍上限等级减1
            Delay 1500
        End If
    Next
    For 5
        dm_ret = dm.FindPic(560, 617, 713, 674, "TP\抓鬼\确定0.bmp", "101010", 0.9, 0, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            TracePrint "出现确定并点击"
            dm.MoveTo intx, inty
            dm.LeftClick 
            Exit For
        Else 
            Delay 1000
            TracePrint "没出现确定"
        End If
    Next 
End Sub
Sub 队伍下限等级减1
    Dim 坐标y
    坐标y=265
    TracePrint "-1"
    dm.MoveTo 563,坐标Y
    Delay 100
    dm.LeftDown 
    Delay 100
    For 7
        坐标Y=坐标Y+5
    Next
    dm.MoveTo 563, 坐标Y
    Delay 10
    Delay 100
    dm.LeftUp
End Sub
Sub 队伍下限等级加1
    dim 坐标Y
    坐标y=308
    TracePrint "+1"
    dm.MoveTo 559,坐标Y
    Delay 100
    dm.LeftDown 
    Delay 100
    For 7
        坐标y=坐标Y-5
        dm.MoveTo 559, 坐标y
        Delay 10
    Next 
    Delay 100
    dm.LeftUp
End Sub
Sub 队伍上限等级减1
    Dim 坐标y
    坐标y=265
    TracePrint "-1"
    dm.MoveTo 665,坐标Y
    Delay 100
    dm.LeftDown 
    Delay 100
    For 7
        坐标Y = 坐标Y +5
        dm.MoveTo 665, 坐标Y
        Delay 10
    Next
    Delay 100
    dm.LeftUp
End Sub
Sub 队伍上限等级加1
    dim 坐标Y
    坐标y=305
    TracePrint "+1"
    dm.MoveTo 669,坐标Y
    Delay 100
    dm.LeftDown 
    Delay 100
    For 7
        坐标y=坐标Y-5
        dm.MoveTo 669, 坐标y
        Delay 10
    Next 
    Delay 100
    dm.LeftUp
End Sub
Sub 当前喊话()
    For 10
        For 20
            dm_ret = dm.FindPic(724, 621, 934, 685, "TP\抓鬼\一键喊话.bmp", "000000", 0.9, 0, intX, intY)
            If intx >= 0 and inty >= 0 Then 
                dm.MoveTo intx,inty
                dm.LeftClick 
                TracePrint "一键喊话"
                Delay 1000
                Exit For 
            Else 
                Delay 1000
            End if
        Next
        If 喊话选择 = 1 Then //当前喊话
            Call 点击(760, 313, 923, 602, "TP\抓鬼\当前.bmp", "000000", 0.9, 0, 10, 1000)
            Call 调试输出("【带队抓鬼】对当前频道喊话....")
        End If
        If 喊话选择 = 2 Then //世界喊话
            Call 点击(760, 313, 923, 602, "TP\抓鬼\世界.bmp", "000000", 0.9, 0, 10, 1000)
            Call 调试输出("【带队抓鬼】对世界频道喊话....")
        End If
        If 喊话选择 = 3 Then //帮派喊话
            Call 点击(760, 313, 923, 602, "TP\抓鬼\帮派.bmp", "000000", 0.9, 0, 10, 1000)
            Call 调试输出("【带队抓鬼】对帮派频道喊话....")
        End If
        For 30
            dm_ret = dm.FindPic(848,184,928,254, "TP\抓鬼\人满.bmp", "000000", 0.9, 0, intX, intY)
            If intx >= 0 and inty >= 0 Then 
                TracePrint "人满了"
                dm.MoveTo 923,109 //发现人满了 点击叉
                dm.LeftClick 
                Exit sub
            Else 
                Delay 1000
            End If
        Next 
    Next 
End Sub//喊话完成后 得等人满了 去做抓鬼任务
Sub 领取抓鬼任务()
    Call 回到主界面
    For 5
        Call 点击(781, 87, 918, 159, "TP\抓鬼\任务1.bmp|TP\抓鬼\任务0.bmp", "000000", 0.9, 0, 3, 500)
        call 点击(781, 87, 918, 159, "TP\抓鬼\任务1.bmp|TP\抓鬼\任务0.bmp", "000000", 0.9, 0, 3, 500) 
        If 找图判断(422,72,564,135, "TP\抓鬼\任务2.bmp", "000000", 0.9, 0, 5, 500) = 1 Then 
            TracePrint "打开任务成功"
            Exit For
        Else 
            Delay 1000
        End If
    Next
    call 点击(928,268,997,407, "TP\抓鬼\可接0.bmp|TP\抓鬼\可接1.bmp", "000000", 0.9, 0, 10, 500) 
    If 点击(74, 157, 294, 677, "TP\抓鬼\常规任务0.bmp|TP\抓鬼\常规任务1.bmp", "000000", 0.9, 0, 5, 500) = 0 Then 
        Call 没有可接任务()
        Exit Sub
    End If
    Call 点击(74, 157, 294, 677, "TP\抓鬼\抓鬼任务1.bmp|TP\抓鬼\抓鬼任务2.bmp|TP\抓鬼\抓鬼任务3.bmp|TP\抓鬼\抓鬼任务4.bmp", "000000", 0.9, 0, 10, 500)
    Call 点击(698,599,896,672, "TP\抓鬼\前往.bmp", "000000", 0.9, 0, 10, 500)
End Sub
Sub 抓鬼战斗中()
    dm_ret = dm.UseDict(0)
    dm_ret = dm.FindStr(462, 5, 563, 59, "战斗中1", "ffffee-777777", 0.9, intX, intY)
    If intX >= 0 and intY >= 0 Then 
        TracePrint 进入战斗
        鬼数 = 鬼数 + 1
        显示轮数=抓鬼一轮+1
        Call 调试输出("【捉鬼中】第"&显示轮数&"轮|"&"第"&鬼数&"只"&"-正在战斗")
        Call 判断卡双()
        Call 踢离线
        Delay 1000
        For 150
            dm_ret = dm.FindStr(462,5,563,59, "战斗中1|战斗中2|战斗中3|战斗中4|战斗中5|战斗中6|战斗中7|战斗中8|战斗中9|战斗中0", "ffffee-777777", 0.9, intX, intY)
            If intX >= 0 and intY >= 0 Then 
                TracePrint "出现战斗中数字"
                //战斗中检查自动 如果没开 点击开
                If 找图判断(945, 302, 1021, 479, "TP\阵法\法术.bmp", "000000", 0.9, 1, 1, 100)=1 Then 
                    If 点击(919,637,1018,755, "TP\阵法\自动.bmp", "000000", 0.9, 1, 1, 100) = 1 Then 
                        TracePrint "点击自动"
                    End If
                End If
                Delay 1000
            Else 
                Delay 300
                Call 调试输出("【抓鬼中】第"&显示轮数&"轮|"&"第"&鬼数&"只"&"-战斗结束")
                Exit for
            End If
        Next
        Delay 1000
        TracePrint "没出现战斗 结束"
        If 鬼数 = 10 Then 
            鬼数=0
        End If
    End If
End Sub
Sub 师门右下角抓鬼任务()
    For 50
        dm_ret = dm.FindPic(706,92,998,608, "TP\抓鬼\右下角抓鬼.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then
            TracePrint "找到右下角捉鬼任务"
            dm.MoveTo intx, inty
            TracePrint "右下角" & intx & "," & inty
            dm.LeftClick 
            Exit Sub
        Else 
            TracePrint "没找到右下角捉鬼任务"
            Delay 500
        End If
    Next  
End Sub
Sub 找到右侧抓鬼2个字()
    Call 队伍未选中就点击()
    dm_ret = dm.UseDict(1)
    dm_ret = dm.FindStr(789,87,1024,462, "捉鬼", "B2AC34-4A4728", 0.9, intX, intY)
    If intx >= 0 and inty >= 0 Then 
        s = dm.Ocr(intx-10,inty-10,intx+950,inty+30,"B2AC34-4A4728",0.9)
        Call 调试输出(s)
        TracePrint "找到右侧捉鬼任务"
        dm.MoveTo intx, inty
        dm.LeftClick 
    Else 
        TracePrint "没找到右侧捉鬼任务"
        Delay 300
    End If
End Sub
Function  抓鬼完成一轮()
    dm_ret = dm.FindPic(312, 333, 729, 392, "TP\抓鬼\抓完.bmp|TP\抓鬼\抓完1.bmp|TP\抓鬼\继续抓鬼.bmp", "101010", 0.9, 0, intX, intY)
    If intx >= 0 and inty >= 0 Then 
        TracePrint "出现抓鬼抓完"
        dm_ret = dm.FindPic(513,385,695,471, "TP\抓鬼\确定.bmp", "101010", 0.9, 0, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            dm.moveto intx, inty
            Delay 10
            dm.LeftClick 
            TracePrint "点击确定"
            抓鬼一轮 = 抓鬼一轮 + 1
            鬼数 = 0
            抓鬼完成一轮 = True
        Else 
            抓鬼完成一轮 = False 
        End If
    Else 
        TracePrint "没找到一轮"
    End If
End Function
Function 不足5人()//任务中点取消
    dm_ret = dm.FindPic(237,207,744,606, "TP\抓鬼\不足5人.bmp", "101010", 0.9, 0, intX, intY)
    If intx >= 0 and inty >= 0 Then 
        TracePrint "出现抓鬼抓完"
        dm_ret = dm.FindPic(237,207,744,606, "TP\抓鬼\取消.bmp", "101010", 0.9, 0, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            dm.moveto intx, inty
            dm.LeftClick 
            TracePrint "点击取消"
            不足5人 = True
        Else 
            不足5人 = False
        End if
    End If
End Function
Function 双倍不足()//任务中弹窗双倍 点取消
    dm_ret = dm.FindPic(237,207,744,606, "TP\抓鬼\双倍点数不足.bmp", "101010", 0.9, 0, intX, intY)
    If intx >= 0 and inty >= 0 Then 
        TracePrint "出现双倍点数不足"
        dm_ret = dm.FindPic(311,402,499,487, "TP\抓鬼\双倍取消.bmp", "101010", 0.9, 0, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            dm.moveto intx, inty
            dm.LeftClick 
            TracePrint "点击双倍取消"
            双倍不足 = True
        Else 
            双倍不足 = False
        End If
    Else 
    End If
End Function
Function 不足5人1()//任务前点取消
    For 5
        dm_ret = dm.FindPic(237,207,744,606, "TP\抓鬼\不足5人1.bmp", "101010", 0.9, 0, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            TracePrint "出现领取任务时不满5人"
            dm_ret = dm.FindPic(311,402,499,487, "TP\抓鬼\取消.bmp", "101010", 0.9, 0, intX, intY)
            If intx >= 0 and inty >= 0 Then 
                dm.moveto intx, inty
                dm.LeftClick 
                TracePrint "点击取消"
                Exit For
            End If
        Else 
            Delay 300
            TracePrint "没有不足5人"
        End If
    Next 
End Function
Function 没有辅助()//任务前没有辅助 点取消
    For 5
        dm_ret = dm.FindPic(237,207,744,606, "TP\抓鬼\没有辅助.bmp", "101010", 0.9, 0, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            TracePrint "出现没有辅助"
            dm_ret = dm.FindPic(311,402,499,487, "TP\抓鬼\取消.bmp", "101010", 0.9, 0, intX, intY)
            If intx >= 0 and inty >= 0 Then 
                dm.moveto intx, inty
                dm.LeftClick 
                TracePrint "点击取消"
            End If
        Else 
            TracePrint "没有出现没辅助"
            Delay 1000
        End If
    Next 
End Function
Sub 信息框()//提示不足五人没有辅助
    For 5
        dm_ret = dm.FindPic(175, 267, 474, 552, "TP\信息4.bmp", "000000", 0.9, 0, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            TracePrint "出现弹窗"
            Call 不足5人1()
            Call 没有辅助()
        Else 
            TracePrint "没出现弹窗"
            Delay 300
        End If
    Next 
End Sub
Sub 卡双()
    For 3
        dm_ret = dm.FindPic(4,18,73,97, "TP\抓鬼\卡双1.bmp", "000000", 0.9, 0, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            TracePrint "开始卡双"
            TracePrint intx&","&inty
            dm.MoveTo intx+13,inty+11
            dm.LeftClick 
            Delay 速度
        Else 
        End If
        Delay 1000
        dm_ret = dm.FindPic(357,24,409,73, "TP\抓鬼\卡双2.bmp", "000000", 0.9, 0, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            TracePrint "打开卡双面板成功"
            Exit For
        Else 
            TracePrint "没打开卡双"
        End If
    Next 
End Sub
Sub 点击挂机()
    For 5
        dm_ret = dm.FindPic(6,0,374,349, "TP\抓鬼\挂机0.bmp", "000000", 0.9, 0, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            TracePrint "开始卡双"
            dm.MoveTo intx,inty
            dm.LeftClick 
            Delay 1000
        Else 
            Delay 1000
        End If
        dm_ret = dm.FindPic(443,58,596,134, "TP\抓鬼\挂机1.bmp", "000000", 0.9, 0, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            TracePrint "打开卡双面板成功"
            Exit For
        End If
    Next 
End Sub
Sub 领取双倍()
    Call 卡双()
    Call 点击挂机()
    For 3
        dm_ret = dm.FindPic(443,58,596,134, "TP\抓鬼\挂机1.bmp", "000000", 0.9, 0, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            dm_ret = dm.FindPic(672,603,955,672, "TP\抓鬼\领取双倍.bmp", "000000", 0.9, 0, intX, intY)
            If intx >= 0 and inty >= 0 Then 
                TracePrint "点击领取双倍"
                dm.MoveTo intx,inty
                dm.LeftClick 
                Delay 速度
                Exit For
            End if
        Else 
            Delay 1000
        End If
    Next
    dm_ret = dm.FindPic(428,597,717,677, "TP\抓鬼\冻结双倍.bmp", "000000", 0.9, 0, intX, intY)
    If intx >= 0 and inty >= 0 Then 
        TracePrint "领取完成关闭窗口"
        dm.MoveTo 954, 107
        dm.LeftClick 
        Delay 速度
    End If
End Sub
Sub 冻结双倍()
    Call 卡双()
    Call 点击挂机()
    For 3
        dm_ret = dm.FindPic(443,58,596,134, "TP\抓鬼\挂机1.bmp", "000000", 0.9, 0, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            dm_ret = dm.FindPic(428,597,717,677, "TP\抓鬼\冻结双倍.bmp", "000000", 0.9, 0, intX, intY)
            If intx >= 0 and inty >= 0 Then 
                TracePrint "点击冻结双倍"
                dm.MoveTo intx,inty
                dm.LeftClick 
                Exit For
                Delay 速度
            Else 
                TracePrint "没出现冻结 退出"
                Exit For
            End If
        Else 
            Delay 1000
        End If
    Next
    For 5
        dm_ret = dm.FindPic(884,69,1002,173, "TP\抓鬼\挂机关闭.bmp", "000000", 0.9, 0, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            TracePrint "关闭冻结窗口"
            dm.MoveTo intx, inty
            dm.LeftClick 
            Exit For
        Else 
            Delay 1000
        End If
    Next 
End Sub
sub  混队判断卡双()
    If 混鬼 = 1 Then 
        Call 冻结双倍
    End If
    TracePrint "卡双倍=" & 卡双倍
    TracePrint "鬼数=" & 鬼数
    If int(卡双倍) = 0 Then 
        TracePrint "不领双"
        Exit Sub
    End If
    If int(卡双倍) = 1 Then 
        TracePrint "领双不卡"
        Call 领取双倍()
    End If
    If int(卡双倍) - 1 = 混鬼 Then 
        Call 调试输出("【带队抓鬼】领取双倍....")
        TracePrint "领取双倍"
        Call 领取双倍()
    End If
End Sub
Sub 判断卡双()
    If 鬼数 = 1 Then 
        Call 冻结双倍
    End If
    TracePrint "卡双倍=" & 卡双倍
    TracePrint "鬼数=" & 鬼数
    If int(卡双倍) = 0 Then 
        TracePrint "不领双"
        Exit Sub
    End If
    If int(卡双倍) = 1 Then 
        TracePrint "领双不卡"
        Call 领取双倍()
    End If
    If int(卡双倍) - 1 = 鬼数 Then 
        Call 调试输出("【带队抓鬼】领取双倍....")
        TracePrint "领取双倍"
        Call 领取双倍()
    End If
End Sub
Sub 踢离线
    Call 卡双()//打开战斗中面板
    Call 点击队伍()
    Delay 1000
    For 3
        dm_ret = dm.FindPic(72,370,923,543, "TP\抓鬼\暂离.bmp|TP\抓鬼\离线.bmp", "000000", 0.9, 0, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            TracePrint "出现离线队员"
            dm.MoveTo intx, inty
            dm.LeftClick 
            Call 调试输出("【带队抓鬼】清理离线队友中....")
            Call 点击请离()
            Delay 速度
        Else 
            TracePrint "没有要请离的队员"
        End If
    Next
    Delay 2000
    Call 点击自动匹配()
End Sub
Sub 队伍中判断踢离线()
    For 3
        dm_ret = dm.FindPic(72,370,923,543, "TP\抓鬼\暂离.bmp|TP\抓鬼\离线.bmp", "000000", 0.9, 0, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            TracePrint "出现离线队员"
            dm.MoveTo intx, inty
            dm.LeftClick 
            Call 点击请离()
            Delay 速度
        Else 
            TracePrint "没有要请离的队员"
        End If
    Next
End Sub
Sub 点击自动匹配()
    For 5
        dm_ret = dm.FindPic(785,135,941,204, "TP\抓鬼\自动匹配.bmp", "000000", 0.9, 0, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            TracePrint "点击自动匹配"
            自动匹配X = intx
            TracePrint "自动匹配x="&自动匹配X
            自动匹配Y = inty
            TracePrint "自动匹配y="&自动匹配y
            dm_ret = dm.FindPic(848,184,928,254, "TP\抓鬼\人满.bmp", "000000", 0.9, 0, intX, intY)
            If intx >= 0 and inty >= 0 Then 
                TracePrint "人满了"
            Else 
                TracePrint "人没满"
                dm.MoveTo 自动匹配X, 自动匹配Y
                dm.LeftClick 
            End if
            For 5
                dm_ret = dm.FindPic(884,69,1002,173, "TP\抓鬼\挂机关闭.bmp", "000000", 0.9, 0, intX, intY)
                If intx >= 0 and inty >= 0 Then 
                    TracePrint "关闭冻结窗口"
                    dm.MoveTo intx, inty
                    dm.LeftClick 
                    Exit For
                Else 
                    Delay 1000
                End If
            Next 
            Exit For
        Else 
            Delay 1000
        End If
    Next
End Sub
Sub 点击请离()
    For 5
        dm_ret = dm.FindPic(76,261,935,523, "TP\抓鬼\请离.bmp", "000000", 0.9, 0, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            TracePrint "出现请离"
            dm.MoveTo intx, inty
            dm.LeftClick 
            Exit For
        Else 
            Delay 1000
        End If
    Next 
End Sub
Sub 点击队伍()
    For 5
        dm_ret = dm.FindPic(6,0,374,349, "TP\抓鬼\战斗中队伍.bmp", "000000", 0.9, 0, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            TracePrint "点击战斗中的队伍"
            dm.MoveTo intx,inty
            dm.LeftClick 
            For 8
                dm_ret = dm.FindPic(6, 0, 374, 349, "TP\抓鬼\战斗中队伍.bmp", "000000", 0.9, 0, intX, intY)
                If intx >= 0 and inty >= 0 Then 
                    TracePrint "找到了就等"
                    Delay 500
                Else 
                    TracePrint "没找到就退出等待"
                    Exit For
                End if
            Next  
        Else 
            Delay 1000
        End If
        dm_ret = dm.FindPic(430,57,499,137, "TP\抓鬼\队伍3.bmp", "000000", 0.9, 0, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            TracePrint "打开队伍面板成功"
            Exit For
        End If
    Next 
End Sub
Function 卡住在任务中传送()
    Call 打开任务栏
    Call 任务内点击当前()
    Call 任务内点击常规任务()
    If 点击抓鬼任务() = True Then 
        TracePrint "抓鬼任务还在，判断队伍人数"
        Call 点击所有打叉
        Call 打开队伍
        dm_ret = dm.FindPic(410,196,584,267, "TP\抓鬼\助战.bmp", "000000", 0.9, 0, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            TracePrint "人没满"
            Delay 10
            Call 点击自动匹配()
            卡住在任务中传送=True
        End If
    Else 
        TracePrint "抓鬼任务没了"
        卡住在任务中传送=False
    End If
    // 
    //    Call 点击(698, 599, 896, 672, "TP\抓鬼\前往1.bmp", "000000", 0.9, 0, 10, 1000)
    //    For 5
    //        dm_ret = dm.FindPic(884,69,1002,173, "TP\抓鬼\挂机关闭.bmp", "000000", 0.9, 0, intX, intY)
    //        If intx >= 0 and inty >= 0 Then 
    //            TracePrint "关闭冻结窗口"
    //            dm.MoveTo intx, inty
    //            dm.LeftClick 
    //            Exit For
    //        Else 
    //            Delay 1000
    //        End If
    //    Next 
End Function
Function 点击抓鬼任务()
    For 5
        dm_ret = dm.FindPic(45, 89, 312, 689, "TP\抓鬼\捉鬼任务1.bmp|TP\抓鬼\捉鬼任务2.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "点击捉鬼任务"
            dm.MoveTo intx, inty
            Delay 10
            dm.LeftClick 
            点击抓鬼任务 = True
        Else 
            TracePrint "没找到捉鬼任务"
        End If
        Delay 速度
        dm_ret = dm.FindPic(45, 89, 312, 689, "TP\抓鬼\捉鬼任务2.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "已经在抓鬼任务选中"
            Exit Function
        Else 
            Delay 500
            点击抓鬼任务=False
        End If
    Next 
End Function
Sub 没有可接任务()
    Call 点击(923,158,993,274, "TP\抓鬼\当前1.bmp|TP\抓鬼\当前2.bmp", "000000", 0.9, 0, 10, 1000)
    For 5
        call 点击(74,157,294,677, "TP\抓鬼\常规任务0.bmp|TP\抓鬼\常规任务1.bmp", "000000", 0.9, 0, 10, 1000) 
        If 点击(74, 157, 294, 677, "TP\抓鬼\抓鬼任务1.bmp|TP\抓鬼\抓鬼任务2.bmp|TP\抓鬼\抓鬼任务3.bmp|TP\抓鬼\抓鬼任务4.bmp", "000000", 0.9, 0, 2, 1000) = 1 Then 
            call 点击(74,157,294,677, "TP\抓鬼\常规任务0.bmp|TP\抓鬼\常规任务1.bmp", "000000", 0.9, 0, 10, 1000) 
            Exit For
        Else 
            Delay 1000
        End If
    Next 
    Call 点击(698, 599, 896, 672, "TP\抓鬼\前往1.bmp", "000000", 0.9, 0, 10, 1000)
    For 5
        dm_ret = dm.FindPic(884,69,1002,173, "TP\抓鬼\挂机关闭.bmp", "000000", 0.9, 0, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            TracePrint "关闭冻结窗口"
            dm.MoveTo intx, inty
            dm.LeftClick 
            Exit For
        Else 
            Delay 1000
        End If
    Next 
End Sub
Sub 抓鬼卡住()
End Sub
// <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<三界奇缘
Sub 三界奇缘()
    当前时间= lib.网络.获取网络时间()
    当前时间 = Right(当前时间, 8)    '返回 "d"。
    小时=Left(当前时间,2)
    分钟=Mid(当前时间, 4, 2)
    TracePrint 小时
    TracePrint 分钟
    If int(小时) < 11 Then 
        TracePrint "没到时间"
        Call 调试输出("没到时间，跳过任务")
        Exit Sub
    Else 
        TracePrint "三界奇缘在时间范围内，可做"
    End If
    dm_ret = dm.UseDict(2)
    Rem 重置
    Call 回到主界面()
    Call 点击活动()
    Rem 开始
    For 3
        Delay 2000
        dm_ret = dm.FindStr(220, 132, 972, 516, "奇缘", "5e432d-505050", 0.8, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            TracePrint "靠下"&intx&","&inty
            三界X = intx
            三界Y = INTY
            If inty > 436 Then 
                TracePrint "太靠下移动一下"
                Call 活动内向上移动()
                Delay 1000
                Goto 开始
            End If    	
            TracePrint "找到三界任务"
            TracePrint intx
            TracePrint inty
            dm_ret = dm.FindStr(三界X, 三界Y, 三界X + 262, 三界Y + 62, "参", "6c310a-606060", 0.8, intX, intY)
            If intx >= 0 and inty >= 0 Then 
                TracePrint "找到三界任务，三界任务未完成"
                dm.MoveTo intx, inty
                Delay 50
                dm.LeftClick 
                Exit For
            Else 
                dm_ret = dm.FindStr(三界X, 三界Y, 三界X + 262, 三界Y + 62, "完成", "4a8216-505050", 0.9, intX, intY)
                If intx >= 0 and inty >= 0 Then 
                    TracePrint "找到三界完成"
                    Call 调试输出("三界任务已完成")
                    Exit Sub
                End If
                TracePrint "没找到参加,三界任务结束"
            End If
        Else 
            TracePrint "没找到三界任务 开始下翻"
            call 活动内向上移动()
            Delay 1000
        End If
    Next
    For 150
        Call 不动检测
        For 15
            Call 三界答题()
            Delay 1500
            dm_ret = dm.FindPic(15, 387, 228, 532, "TP\三界完成.bmp", "000000", 0.9, 0, intX, intY)
            If intx >= 0 and inty >= 0 Then 
                TracePrint "完成三界"
                Call 调试输出("三界奇缘已经完成")
                Exit Sub
            Else 
                TracePrint "没找到三界奇缘完成"
            End If
        Next
        If 不动判断 = True Then 
            Goto 重置
        End If
    Next 
End Sub
Sub 三界答题()
    dm_ret = dm.FindPic(15, 387, 228, 532, "TP\三界.bmp", "000000", 0.9, 0, intX, intY)
    If intx >= 0 and inty >= 0 Then 
        TracePrint "打开三界界面"
        dm.MoveTo 449, 285
        dm.LeftClick 
    End if
End Sub
//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<帮派任务
Sub 帮派任务()
    Rem 开始
    dm_ret = dm.UseDict(2)
    Call 回到主界面
    Call 打开任务栏()
    Call 任务内点击当前()
    Call 任务内点击常规任务()
    dm_ret = dm.FindPic(45, 89, 312, 689, "TP\帮派任务\帮派1.bmp|TP\帮派任务\帮派2.bmp", "000000", 0.9, 0, intX, intY)
    If intX >= 0 and intY >= 0 Then 
        TracePrint "有帮派任务"
        Call  点击帮派任务()
        Goto 判断任务1
    Else 
        TracePrint "没有帮派任务"
        call   点击(879,83,969,141,"TP\帮派任务\关闭.bmp","000000",0.9,0,1,200)
    End if
    Call 点击活动
    For 3
        dm_ret = dm.FindStr(220, 132, 972, 516, "帮派任务", "816955-2D3133", 0.9, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            宝图X = intx
            宝图Y = INTY
            TracePrint "找到帮派任务"
            TracePrint intx
            TracePrint inty
            dm_ret = dm.FindStr(宝图X, 宝图Y, 宝图X + 262, 宝图Y + 62, "参", "6c310a-606060", 0.9, intX, intY)
            If intx >= 0 and inty >= 0 Then 
                TracePrint "找到帮派任务，帮派任务未完成"
                dm.MoveTo intx, inty
                Delay 50
                dm.LeftClick 
                Exit For
            Else 
                dm_ret = dm.FindStr(宝图X, 宝图Y, 宝图X + 262, 宝图Y + 62, "完成", "4a8216-505050", 0.9, intX, intY)
                If intx >= 0 and inty >= 0 Then 
                    TracePrint "找到宝图完成"
                    Call 调试输出("帮派任务已完成")
                    Exit Sub
                End If
                TracePrint "没找到参加,帮派任务结束"
            End If
        Else 
            TracePrint "没找到帮派任务 开始下翻"
            call 活动内向上移动()
            Delay 1000
        End If
        Delay 1000
    Next
    Call 领取帮派任务()
    Rem 判断任务
    Call 打开任务栏内帮派任务()
    Rem 判断任务1
    dm_ret = dm.FindPic(45, 89, 312, 689, "TP\帮派任务\帮派1.bmp|TP\帮派任务\帮派2.bmp", "000000", 0.9, 0, intX, intY)
    If intX >= 0 and intY >= 0 Then 
        TracePrint "有帮派任务"
    Else 
        TracePrint "没有帮派任务"
        Goto 开始
    End if
    If 帮派任务选择 = 0 Then 
        dm_ret = dm.FindPic(305,149,453,219, "TP\帮派任务\青龙.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "找到青龙"
            Call 马上传送
        Else 
            TracePrint "不是青龙"
            Call 放弃任务
            Goto 开始
        End If
    Else 
        TracePrint "做任务任务打开"   
        Call 马上传送 
    End If
    For 2000
        If 寻路中 = False Then 
            TracePrint "没有在寻路"
            For 3
                Delay 10
                Call 判断有没有任务()
                Delay 10
                Call 师门召唤兽购买上交()
                Delay 10
                Call 师门使用
                Delay 10
                Call 摆摊购买()
                Delay 10
                call  工坊购买()
                Delay 10
                // Call 点击(407, 339, 585, 439, "TP\师门\完成弹窗.bmp", "000000", 0.9, 0, 1, 500)
                Delay 10
                Call 跳过对话()
                Delay 10
                Call 战斗中()
                Delay 10
                Call 商会购买()
                Delay 1000
            Next 
        Else 
            TracePrint "寻路中"
            Delay 1000
        End If
        dm_ret = dm.FindStr(789,87,1024,462, "朱雀|青龙|玄武", "B2AC34-4A4728", 0.9, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            TracePrint "找到右侧帮派任务"
            dm.MoveTo intx, inty
            Delay 10
            dm.LeftClick 
        Else 
            TracePrint "没找到帮派任务"
            Delay 300
            Goto 判断任务
        End If
    Next   
End Sub
Sub 商会购买()
    dm_ret = dm.FindPic(733,603,953,700, "TP\帮派任务\商会购买.bmp", "000000", 0.9, 0, intX, intY)
    If intX >= 0 and intY >= 0 Then 
        TracePrint "商会购买"
        dm.MoveTo intx, inty
        Delay 10
        dm.LeftClick 
    End if
End Sub
Sub 右侧帮派任务()
    dm_ret = dm.UseDict(2)
    dm_ret = dm.FindStr(789, 87, 1024, 462, "朱雀|青龙|玄武", "B2AC34-4A4728", 0.9, intX, intY)
    If intX >= 0 and intY >= 0 Then 
        TracePrint "成功点击右侧帮派任务"
        dm.MoveTo intx, inty
        Delay 10
        dm.LeftClick 
    Else 
        TracePrint "没找到右侧帮派任务"
    End If
End Sub
Sub 领取帮派任务()
    For 20
        dm_ret = dm.FindPic(706,233,998,590, "TP\帮派任务\领取帮派任务.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "点击领取帮派任务"
            dm.MoveTo intx, inty
            Delay 10
            dm.LeftClick 
            TracePrint "点击领取"
            Call 调试输出("领取帮派任务")
            Delay 速度
            Call 回到主界面
            Exit Sub
        Else 
            Delay 1000
            TracePrint "等待帮派任务领取"
        End If
    Next 
End Sub
Sub 打开任务栏内帮派任务()
    Call 打开任务栏()
    Call 任务内点击当前()
    Call 任务内点击常规任务()
    Call 点击帮派任务()
End Sub
Sub 打开任务栏()
    Rem 开始
    dm_ret = dm.FindPic(791,91,916,168, "TP\帮派任务\任务栏明.bmp", "000000", 0.9, 0, intX, intY)
    If intX >= 0 and intY >= 0 Then 
        TracePrint "点击任务栏"
        dm.MoveTo intx, inty
        Delay 10
        dm.LeftClick 
    End If
    dm_ret = dm.FindPic(791,91,916,168, "TP\帮派任务\任务栏暗.bmp", "000000", 0.9, 0, intX, intY)
    If intX >= 0 and intY >= 0 Then 
        TracePrint "点击2次任务栏"
        dm.MoveTo intx, inty
        Delay 10
        dm.LeftClick 
        Delay 200
        dm.LeftClick 
    End If
    //===========开始判断打开没
    Delay 1000
    dm_ret = dm.FindPic(410,44,608,174, "TP\帮派任务\任务界面.bmp", "000000", 0.9, 0, intX, intY)
    If intX >= 0 and intY >= 0 Then 
        TracePrint "成功打开任务界面"
        Exit Sub
    Else 
        TracePrint "打开界面失败"
        Goto 开始
    End If
End Sub
Sub 任务内点击当前()
    Rem 开始
    dm_ret = dm.FindPic(925,83,1014,457, "TP\帮派任务\当前暗.bmp", "000000", 0.9, 0, intX, intY)
    If intX >= 0 and intY >= 0 Then 
        TracePrint "没在当前暗要点击"
        dm.MoveTo intx, inty
        Delay 10
        dm.LeftClick 
    Else 
        TracePrint "没找到当前暗直接说明已经找到"
        Exit Sub
    End If
    Delay 速度
    dm_ret = dm.FindPic(925,83,1014,457, "TP\帮派任务\当前暗.bmp", "000000", 0.9, 0, intX, intY)
    If intX >= 0 and intY >= 0 Then 
        TracePrint "当前暗还在"
        Goto 开始
    Else 
        TracePrint "没找到当前暗直接说明已经找到"
        Exit Sub
    End if
End Sub
Sub 任务内点击常规任务()
    For 5
        dm_ret = dm.FindPic(45,89,312,689, "TP\帮派任务\常规任务1.bmp|TP\帮派任务\常规任务2.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "展开常规任务"
            dm.MoveTo intx, inty
            Delay 10
            dm.LeftClick 
        Else 
        End If
        Delay 速度
        dm_ret = dm.FindPic(45,89,312,689, "TP\帮派任务\常规任务展开1.bmp|TP\帮派任务\常规任务展开2.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "常规任务已经展开"
            Exit Sub
        Else 
            TracePrint "常规任务没展开，再展开一次"
        End If
    Next 
End Sub
Sub 点击帮派任务()
    For 5
        dm_ret = dm.FindPic(45, 89, 312, 689, "TP\帮派任务\帮派1.bmp|TP\帮派任务\帮派2.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "点击帮派任务"
            dm.MoveTo intx, inty
            Delay 10
            dm.LeftClick 
        End If
        Delay 速度
        dm_ret = dm.FindPic(45, 89, 312, 689, "TP\帮派任务\帮派2.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "已经在帮派任务栏"
            Exit Sub
        Else 
            Delay 500
        End If
    Next 
End Sub
//===============================================任务链
Sub 任务链
    dm_ret = dm.UseDict(3)
    Rem 重置
    Call 回到主界面()
    Call 打开任务栏()
    Call 任务内点击当前()
    Call 任务栏合上()
    Delay  1000
    dm_ret = dm.FindPic(45,89,312,689, "TP\任务链\其他00.bmp|TP\任务链\其他11.bmp", "000000", 0.9, 0, intX, intY)
    If intX >= 0 and intY >= 0 Then 
        TracePrint "有其他"
        Call 任务内点击其他任务()
    Else 
        TracePrint "没有其他，说明没有领取经验链或者已经完成"
        Goto 开始2
    End if
    dm_ret = dm.FindPic(45,89,312,689, "TP\任务链\经验任务链0.bmp|TP\任务链\经验任务链1.bmp", "000000", 0.9, 0, intX, intY)
    If intX >= 0 and intY >= 0 Then 
        TracePrint "找到任务"
        dm.MoveTo intx, inty
        Delay 50
        dm.LeftClick 
        Call 马上传送
        Goto 开始3
    Else 
        TracePrint "没有找到任务"
    End if
    Rem 开始2
    Call 回到主界面()
    Call 点击活动()
    Rem 开始
    For 3
        Delay 2000
        dm_ret = dm.FindStr(220, 132, 972, 516, "经验", "5e432d-505050", 0.8, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            TracePrint "靠下"&intx&","&inty
            三界X = intx
            三界Y = INTY
            If inty > 436 Then 
                TracePrint "太靠下移动一下"
                Call 活动内向上移动()
                Delay 1000
                Goto 开始
            End If    	
            TracePrint "找到任务链任务"
            TracePrint intx
            TracePrint inty
            dm_ret = dm.FindStr(三界X, 三界Y, 三界X + 262, 三界Y + 62, "参", "6c310a-606060", 0.8, intX, intY)
            If intx >= 0 and inty >= 0 Then 
                TracePrint "找到任务链任务，任务链任务未完成"
                dm.MoveTo intx, inty
                Delay 50
                dm.LeftClick 
                Exit For
            Else 
                dm_ret = dm.FindStr(三界X, 三界Y, 三界X + 262, 三界Y + 62, "完成", "4a8216-505050", 0.9, intX, intY)
                If intx >= 0 and inty >= 0 Then 
                    TracePrint "找到任务链完成"
                    Call 调试输出("任务链任务已完成")
                    Exit Sub
                End If
                TracePrint "没找到参加,任务链任务结束"
            End If
        Else 
            TracePrint "没找到任务链任务 开始下翻"
            call 活动内向上移动()
            Delay 1000
        End If
    Next
    Call 任务链领取()
    Call 确定任务链领取()
    Call 确定任务链领取1()
    Delay 3000
    dm_ret = dm.FindPic(294,217,691,563, "TP\任务链\消费确认.bmp", "000000", 0.9, 0, intX, intY)
    If intX >= 0 and intY >= 0 Then 
        dm.MoveTo 713,285
        Delay 10
        dm.LeftClick 
        TracePrint "出现银币不足关闭并结束任务"
        Call 调试输出("任务链，银币不足结束任务")
        Exit Sub
    Else 
        TracePrint "没有找到"
    End If
    Rem 开始3
    TracePrint "开始经验连"
    For 1000
        call 不动检测 
        For 5
            Call 判断有没有任务//调用师门内的点击右下角第一行
            Delay 10
            Call 战斗中
            Delay 10
            Call 师门召唤兽购买上交()
            Delay 10
            Call 师门使用
            Delay 10
            //Call 摆摊购买()
            call 任务链摆摊购买()
            Delay 10
            call  工坊购买()
            Delay 10
            Call 点击(407, 339, 585, 439, "TP\师门\完成弹窗.bmp", "000000", 0.9, 0, 1, 500)
            Delay 10
            Call 跳过对话()
            Delay 10
            Call 师门战斗中()
            Delay 10
            Call 关闭任务栏()
            Delay 1000
        Next
        If 不动判断 = True Then 
            Goto 重置
        End If
    Next 
End Sub
Sub 关闭任务栏()
    dm_ret = dm.FindPic(441,65,618,153, "TP\任务链\任务栏.bmp", "000000", 0.9, 0, intX, intY)
    If intX >= 0 and intY >= 0 Then 
        TracePrint "出现任务界面"
        call   点击(879,83,969,141,"TP\帮派任务\关闭.bmp","000000",0.9,0,1,200)
    End if
End Sub
Sub 任务内点击其他任务()
    For 5
        dm_ret = dm.FindPic(45,89,312,689, "TP\任务链\其他任务.bmp|TP\任务链\其他1.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "展开其他任务"
            dm.MoveTo intx, inty
            Delay 10
            dm.LeftClick 
        Else 
        End If
        Delay 速度
        dm_ret = dm.FindPic(45,89,312,689, "TP\任务链\其他展开1.bmp|TP\任务链\其他展开2.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "其他任务已经展开"
            Exit Sub
        Else 
            TracePrint "其他任务没展开，再展开一次"
        End If
    Next 
End Sub
Sub 任务栏合上()
    For 10
        dm_ret = dm.FindPic(345,29,706,213, "TP\任务链\任务栏.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "在任务栏中"
            Exit For
        Else 
            Delay 1000
        End  if
    Next  
    For 5
        dm_ret = dm.FindPic(221,119,359,706, "TP\任务链\合上.bmp|TP\任务链\合上1.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "发现有展开的点击合上"
            dm.MoveTo intx, inty
            Delay 10
            dm.LeftClick 
        End If
        Delay 1000
        dm_ret = dm.FindPic(221,119,359,706, "TP\任务链\合上.bmp|TP\任务链\合上1.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            Delay 1000
            TracePrint "发现没合上的"
        Else 
            TracePrint "没发现没合上的"
            Exit Sub
        End If
    Next
End Sub
Sub 任务链领取()
    For 50
        dm_ret = dm.FindPic(706,92,998,608, "TP\任务链\任务链领取.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then
            TracePrint "找到右下角任务链任务"
            dm.MoveTo intx, inty
            TracePrint "右下角" & intx & "," & inty
            dm.LeftClick 
            Exit Sub
        Else 
            TracePrint "没找到右下角任务链任务"
            Delay 500
        End If
    Next  
End Sub
Sub 确定任务链领取()
    For 50
        dm_ret = dm.FindPic(706,92,998,608, "TP\任务链\任务链领取1.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then
            TracePrint "找到右下角任务链任务"
            dm.MoveTo intx, inty
            TracePrint "右下角" & intx & "," & inty
            dm.LeftClick 
            Exit Sub
        Else 
            TracePrint "没找到右下角任务链任务"
            Delay 500
        End If
    Next  
End Sub
Sub 确定任务链领取1()
    For 50
        dm_ret = dm.FindPic(547,505,727,599, "TP\任务链\确定领取经验任务链.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then
            TracePrint "找到右下角任务链任务"
            dm.MoveTo intx, inty
            TracePrint "右下角" & intx & "," & inty
            dm.LeftClick 
            Exit Sub
        Else 
            TracePrint "没找到右下角任务链任务"
            Delay 500
        End If
    Next  
End Sub
Sub 任务链摆摊购买()
    If 任务链所有物品传说 = 1 Then 
        dm_ret = dm.FindPic(424,73,578,133 ,"TP\师门\摆摊.bmp","000000",0.9,0,intX,intY)
        If intX >= 0 and intY >= 0 Then     
            Call 打传说()
        End If
    End If
    If 任务链不传说 = 1 Then 
        Call 摆摊购买
    End If
    If 任务链指定价格传说 = 1 Then 
        Call 任务链摆摊判断价格购买()
    End If
End Sub
Sub 任务链摆摊判断价格购买()
    dm_ret = dm.UseDict(3)
    Rem 开始
    dm_ret = dm.FindPic(424,73,578,133 ,"TP\师门\摆摊.bmp","000000",0.9,0,intX,intY)
    If intX >= 0 and intY >= 0 Then
        TracePrint "找到摆摊购买"
        dm_ret = dm.FindPic(287,203,925,610 ,"TP\师门\需求选中.bmp","000000",0.9,0,intX,intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "需求物品已经选中"
            当前物品X左=intx-173
            当前物品Y左=inty
            当前物品X右=intx
            当前物品Y右 = inty + 73
            TracePrint 当前物品X左&","&当前物品Y左&","&当前物品X右&","&当前物品Y右
            当前物品金额 = dm.Ocr(当前物品X左,当前物品Y左,当前物品X右,当前物品Y右,"ffffff-303030",0.9)
            TracePrint "当前物品金额=" & 当前物品金额
            TracePrint "设定物品金额=" & 任务链传说价格
            If int(当前物品金额) >= int(任务链传说价格) Then 
                TracePrint "超出设定价格不购买"
                Call 打传说()
            Else 
                TracePrint "没超过指定价格"
                TracePrint "点击购买"
                dm.MoveTo 857, 643
                dm.LeftClick 
                TracePrint "点击购买"
                Delay 1000
            End If
        Else 
            TracePrint "需求物品没选中"
            dm_ret = dm.FindPic(287,203,925,610 ,"TP\师门\需求未选中.bmp|TP\师门\需求未选中1.bmp","101010",0.9,0,intX,intY)
            If intX >= 0 and intY >= 0 Then 
                Delay 100
                TracePrint "没选中进行选中"
                dm.MoveTo intx, inty
                Delay 10
                dm.LeftClick 
                TracePrint "点击选中"
                Delay 1500
                Goto 开始
            Else 
                Delay 1000
            End if
        End If
    Else 
        TracePrint "没找到摆摊购买"
        Delay 100
    End If
    dm_ret = dm.FindPic(424, 73, 578, 133, "TP\师门\摆摊.bmp", "000000", 0.9, 0, intX, intY)
    If intX >= 0 and intY >= 0 Then //在判断一次 买成功就找不到摆摊
        TracePrint "没买成功"
        Delay 100
        Goto 开始
    Else 
        TracePrint "买成功了"
    End If
End Sub
Sub 打传说()
    Call 回到主界面
    Call 首页点击挂机()
    Call 挂机右移动()
    Delay 1000
    Call 点击推荐地区()
    For 11140000
        dm_ret = dm.FindPic(595,468,961,782	, "TP\任务链\上交.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "物品成功出来"
            Call 调试输出("物品传说成功~")
            Exit Sub
        Else 
            Delay 1000
        End If
        Call 战斗中()
    Next
    TracePrint "打传说中"
    Call 调试输出("物品传说中~")
End Sub
Sub 挂机右移动
    For 5
        dm.MoveTo 854, 313
        dm.LeftDown 
        Delay 200
        dm.MoveTo 100, 313
        dm.LeftUp 
        Delay 500
        TracePrint "移动一次"
    Next 
End Sub
Sub 挂机左移动
    dm.MoveTo 82,303
    dm.LeftDown 
    dm.MoveTo 946, 303
    dm.LeftUp 
End Sub
Sub 点击推荐地区()
    Delay 1000
    For 5
        dm_ret = dm.FindPic(77,140,963,519, "TP\任务链\推荐地区.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "出现推荐地区"
            dm.MoveTo intx, inty
            Delay 10
            dm.LeftClick 
            Exit Sub
        Else 
            Call 挂机左移动
            Delay 1000
        End If
    Next 
End Sub
Sub 首页点击挂机()
    For 5
        dm_ret = dm.FindPic(383,0,548,103, "TP\任务链\挂机.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "点击挂机"
            dm.MoveTo intx, inty
            Delay 10
            dm.LeftClick 
        End If
        For 10
            dm_ret = dm.FindPic(383,0,548,103, "TP\任务链\挂机.bmp", "000000", 0.9, 0, intX, intY)
            If intX >= 0 and intY >= 0 Then 
                Delay 300
                TracePrint "点击后等待进入界面"
            Else 
                Delay 500
                Exit For
            End If
        Next 
        dm_ret = dm.FindPic(416,57,598,139, "TP\任务链\挂机1.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "成功打开挂机界面"
            Exit Sub
        Else 
            Delay 1000
        End if
    Next   
End Sub
//=====
Sub 放弃任务()
    Call 点击放弃()
    call 确定放弃
End Sub
Sub 点击放弃()
    For 5
        dm_ret = dm.FindPic(303,592,547,680, "TP\帮派任务\放弃.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "点击放弃"
            dm.MoveTo intx, inty
            Delay 10
            dm.LeftClick 
        End If
        Delay 速度
        dm_ret = dm.FindPic(426,314,739,504, "TP\帮派任务\确定.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "点击成功"
            Exit Sub
        Else 
            Delay 1000
        End If
    Next  
End Sub
Sub 确定放弃()
    For 5
        dm_ret = dm.FindPic(517,353,707,511, "TP\帮派任务\确定.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "点击确定"
            dm.MoveTo intx, inty
            Delay 10
            dm.LeftClick 
            Exit Sub
        Else 
            TracePrint "没出现确定"
            Delay 1000
        End If
    Next 
End Sub
Sub 马上传送()
    For 3
        dm_ret = dm.FindPic(673,545,916,700, "TP\帮派任务\马上传送.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "马上传送"
            dm.MoveTo intx, inty
            Delay 10
            dm.LeftClick 
            call   点击(879,83,969,141,"TP\帮派任务\关闭.bmp","000000",0.9,0,1,200)
            Exit Sub
        Else 
            Delay 1000
        End If
    Next 
End Sub
//=====================================================================科举考试
Sub 科举答题()
    当前时间= lib.网络.获取网络时间()
    当前时间 = Right(当前时间, 8)    '返回 "d"。
    小时=Left(当前时间,2)
    分钟=Mid(当前时间, 4, 2)
    TracePrint 小时
    TracePrint 分钟
    If int(小时) < 17 Then 
        TracePrint "没到时间"
        Call 调试输出("科举没到时间，跳过任务")
        Exit Sub
    Else 
        TracePrint "科举在时间范围内，可做"
    End If
    Dim 打开次数
    dm_ret = dm.UseDict(2)
    Rem 重置
    If 打开次数 = 4 Then 
        TracePrint "4次都没开始任务 结束任务"
        Exit Sub
    End If
    打开次数=打开次数+1
    Call 回到主界面()
    Call 点击活动()
    Rem 开始
    For 3
        Delay 2000
        dm_ret = dm.FindStr(220, 132, 972, 516, "科举", "5e432d-505050", 0.8, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            TracePrint "靠下"&intx&","&inty
            三界X = intx
            三界Y = INTY
            If inty > 436 Then 
                TracePrint "太靠下移动一下"
                Call 活动内向上移动()
                Delay 1000
                Goto 开始
            End If    	
            TracePrint "找到三界任务"
            TracePrint intx
            TracePrint inty
            dm_ret = dm.FindStr(三界X, 三界Y, 三界X + 262, 三界Y + 62, "参", "6c310a-606060", 0.8, intX, intY)
            If intx >= 0 and inty >= 0 Then 
                TracePrint "找到科举任务，科举任务未完成"
                dm.MoveTo intx, inty
                Delay 50
                dm.LeftClick 
                Exit For
            Else 
                dm_ret = dm.FindStr(三界X, 三界Y, 三界X + 262, 三界Y + 62, "完成", "4a8216-505050", 0.9, intX, intY)
                If intx >= 0 and inty >= 0 Then 
                    TracePrint "找到三界完成"
                    Call 调试输出("科举任务已完成")
                    Exit Sub
                End If
                TracePrint "没找到参加,科举任务结束"
            End If
        Else 
            TracePrint "没找到科举任务 开始下翻"
            call 活动内向上移动()
            Delay 1000
        End If
    Next
    For 150
        Call 不动检测
        For 15
            Call 科举考试()
            Delay 1500
            dm_ret = dm.FindPic(245,237,994,612, "TP\科举结束.bmp", "000000", 0.9, 0, intX, intY)
            If intx >= 0 and inty >= 0 Then 
                TracePrint "完成科举"
                Call 调试输出("科举答题已经完成")
                Exit Sub
            Else 
                TracePrint "没找到科举答题完成"
            End If
        Next
        If 不动判断 = True Then 
            Goto 重置
        End If
    Next 
End Sub
Sub 科举考试()
    dm_ret = dm.FindPic(133,539,282,676, "TP\科举.bmp", "000000", 0.9, 0, intX, intY)
    If intx >= 0 and inty >= 0 Then 
        TracePrint "打开三界界面"
        dm.MoveTo 404,381
        dm.LeftClick 
    End if
End Sub
// =============---------------------------====竞技场//周一晚上22：00-23：00
Sub 竞技场()
    dm_ret = dm.UseDict(2)
    Rem 重置
    Call 回到主界面()
    Call 点击活动()
    Rem 开始
    For 3
        Delay 2000
        dm_ret = dm.FindStr(220, 132, 972, 516, "竞技场", "5e432d-505050", 0.8, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            TracePrint "靠下"&intx&","&inty
            三界X = intx
            三界Y = INTY
            If inty > 436 Then 
                TracePrint "太靠下移动一下"
                Call 活动内向上移动()
                Delay 1000
                Goto 开始
            End If    	
            TracePrint "找到竞技场任务"
            TracePrint intx
            TracePrint inty
            dm_ret = dm.FindStr(三界X, 三界Y, 三界X + 262, 三界Y + 62, "参", "6c310a-606060", 0.8, intX, intY)
            If intx >= 0 and inty >= 0 Then 
                TracePrint "找到竞技场任务，竞技场任务未完成"
                dm.MoveTo intx, inty
                Delay 50
                dm.LeftClick 
                Exit For
            Else 
                dm_ret = dm.FindStr(三界X, 三界Y, 三界X + 262, 三界Y + 62, "完成", "4a8216-505050", 0.9, intX, intY)
                If intx >= 0 and inty >= 0 Then 
                    TracePrint "找到竞技场完成"
                    Call 调试输出("竞技场已完成")
                    Exit Sub
                End If
                TracePrint "没找到参加,竞技场任务结束"
            End If
        Else 
            TracePrint "没找到竞技场任务 开始下翻"
            call 活动内向上移动()
            Delay 1000
        End If
    Next
    Call 竞技场右下角参与活动()
    call 竞技场中点击竞技()
    For 10000
        Call 竞技场中点击开始匹配()
        Call 战斗中()
        dm_ret = dm.FindPic(600, 549, 898, 657, "TP\其他活动\竞技场满10场.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "竞技场活动够10场退出"
            Call 竞技场离场()
            Exit Sub
        End if
        Delay 3000
    Next 
End Sub
Sub 竞技场右下角参与活动()
    For 50
        dm_ret = dm.FindPic(706,92,998,608, "TP\其他活动\参与活动.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then
            TracePrint "找到右下角参与活动"
            dm.MoveTo intx, inty
            TracePrint "右下角" & intx & "," & inty
            dm.LeftClick 
            Exit Sub
        Else 
            TracePrint "没找到右下角参与活动"
            Delay 500
        End If
    Next  
End Sub
Sub 竞技场中点击竞技()
    For 5
        dm_ret = dm.FindPic(383,280,671,480, "TP\其他活动\竞技场.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then
            TracePrint "点击竞技场"
            dm.MoveTo intx, inty
            TracePrint "右下角" & intx & "," & inty
            Delay 10
            dm.LeftClick 
        Else 
            TracePrint "没找到竞技场"
            Delay 500
        End If
        Delay 1500
        dm_ret = dm.FindPic(329,55,685,176, "TP\其他活动\竞技场1.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "成功打开竞技场"
            Exit Sub
        Else 
            Delay 1000
        End if
    Next  
End Sub
Sub 竞技场中点击开始匹配()
    For 1
        dm_ret = dm.FindPic(627,554,895,656, "TP\其他活动\开始匹配.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then
            TracePrint "点击开始匹配"
            dm.MoveTo intx, inty
            TracePrint "右下角" & intx & "," & inty
            Delay 10
            dm.LeftClick 
        Else 
            TracePrint "没找到开始匹配"
            Delay 500
        End If
        Delay 1500
        dm_ret = dm.FindPic(4,41,471,216, "TP\其他活动\匹配中.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "已经打开匹配中"
            Exit Sub
        Else 
            Delay 1000
        End if
    Next  
End Sub
Sub 竞技场离场()
    Call 点击缩小()
    Call 点击地图()
    Call 点击长安()
End Sub
Sub 点击缩小()
    For 10
        dm_ret = dm.FindPic(747,68,936,175, "TP\其他活动\缩小.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "找到缩小"
            dm.MoveTo intx, inty
            Delay 10
            dm.LeftClick 
        End if
        Delay 1500
        dm_ret = dm.FindPic(1,1,103,85, "TP\其他活动\地图.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "找到地图"
            Exit Sub
        End if
    Next  
End Sub
Sub 点击地图()
    For 10
        dm_ret = dm.FindPic(1,1,103,85, "TP\其他活动\地图.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "找到地图"
            dm.MoveTo intx, inty
            Delay 10
            dm.LeftClick 
        End If
        Delay 1500
        dm_ret = dm.FindPic(32,90,974,670, "TP\其他活动\长安.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "找到长安"
            Exit Sub
        End if
    Next 
End Sub
Sub 点击长安()
    For 10
        dm_ret = dm.FindPic(32,90,974,670, "TP\其他活动\长安.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "找到长安"
            dm.MoveTo intx, inty
            Delay 10
            dm.LeftClick 
            Exit Sub
        Else 
            Delay 1000
        End If
    Next 
End Sub
//===============================================自动喊话
Sub 自动喊话()
    Call 调试输出("执行喊话操作")
    Call 回到主界面()
    Call 打开消息()
    Delay 1000
    喊话=1	
    For 喊话次数
        Call 调试输出("第"&喊话&"遍喊话中~")
        If 内容1 = 1 Then 
            Call 输入信息1
        End if
        If 内容2 = 1 Then 
            Call 输入信息2
        End if
        If 内容3 = 1 Then 
            Call 输入信息3
        End if
        喊话=喊话+1	
    Next
End Sub
Sub 打开消息()
    For 10
        dm_ret = dm.FindPic(6,586,88,654, "TP\信息1.bmp|TP\信息5.bmp", "000000", 0.9, 0, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            TracePrint "打开信息"
            dm.MoveTo intx, inty
            Delay 10
            dm.LeftClick 
        Else 
            TracePrint  "没找到信息"
        End If
        Delay 2000
        dm_ret = dm.FindPic(516,319,575,420, "TP\关闭\回到主界面12.bmp", "000000", 0.9, 0, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            TracePrint "打开信息框成功"
            Exit Sub 
        End if
    Next 
End Sub
Sub 点击信息输入()
    For 5
        dm_ret = dm.FindPic(293,6,516,100, "TP\其他活动\信息输入.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "找到信息输入"
            dm.MoveTo intx-100, inty
            Delay 10
            dm.LeftClick 
        End If
        Delay 1500
        dm_ret = dm.FindPic(946,0,1022,80, "TP\其他活动\确定.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "已经打开输入框"
            Exit Sub
        End if
    next     
End Sub
Sub 选择频道()
    TracePrint "喊话频道="&喊话频道
    If 喊话频道 = 0 Then 
        TracePrint "当前频道"
        call 点击(5,90,109,506,"TP\其他活动\当前0.bmp|TP\其他活动\当前1.bmp", "000000",0.9,0,3,2000)
    End If
    If 喊话频道 = 1 Then 
        TracePrint "世界频道"
        call 点击(5,90,109,506,"TP\其他活动\世界0.bmp|TP\其他活动\世界1.bmp", "000000",0.9,0,3,2000)
    End If
    If 喊话频道 = 2 Then 
        TracePrint "帮派频道"
        call 点击(5,90,109,506,"TP\其他活动\帮派0.bmp|TP\其他活动\帮派1.bmp", "000000",0.9,0,3,2000)
    End If
    If 喊话频道 = 3 Then 
        TracePrint "门派频道"
        call 点击(5,90,109,506,"TP\其他活动\门派0.bmp|TP\其他活动\门派1.bmp", "000000",0.9,0,3,2000)
    End If
End Sub
Sub 输入信息1()
    Call 选择频道()
    Call 点击信息输入()
    Delay 1000
    If 内容1 = 1 Then 
        dm.SendString 当前运行窗口,喊话内容1
    End If
    Delay 500
    dm_ret = dm.FindPic(946,0,1022,80, "TP\其他活动\确定.bmp", "000000", 0.9, 0, intX, intY)
    If intX >= 0 and intY >= 0 Then 
        dm.MoveTo intx, inty
        Delay 10
        dm.LeftClick 
    End if
    Delay 1000
    call 点击(401,9,532,94,"TP\其他活动\发送.bmp", "000000",0.9,0,3,2000)
    Delay 喊话间隔
End Sub
Sub 输入信息2()
    Call 选择频道()
    Call 点击信息输入()
    Delay 1000
    If 内容2 = 1 Then 
        dm.SendString 当前运行窗口,喊话内容2
    End If
    Delay 500
    dm_ret = dm.FindPic(946,0,1022,80, "TP\其他活动\确定.bmp", "000000", 0.9, 0, intX, intY)
    If intX >= 0 and intY >= 0 Then 
        dm.MoveTo intx, inty
        Delay 10
        dm.LeftClick 
    End if
    Delay 1000
    call 点击(401,9,532,94,"TP\其他活动\发送.bmp", "000000",0.9,0,3,2000)
    Delay 喊话间隔
End Sub
Sub 输入信息3()
    Call 选择频道()
    Call 点击信息输入()
    Delay 1000
    If 内容3 = 1 Then 
        dm.SendString 当前运行窗口,喊话内容3
    End If
    Delay 500
    dm_ret = dm.FindPic(946,0,1022,80, "TP\其他活动\确定.bmp", "000000", 0.9, 0, intX, intY)
    If intX >= 0 and intY >= 0 Then 
        dm.MoveTo intx, inty
        Delay 10
        dm.LeftClick 
    End if
    Delay 1000
    call 点击(401,9,532,94,"TP\其他活动\发送.bmp", "000000",0.9,0,3,2000)
    Delay 喊话间隔
End Sub
//=================================================钓鱼
Sub 钓鱼()
    dm_ret = dm.UseDict(2)
    Rem 重置
    Call 回到主界面()
    Call 点击活动()
    Call 点击(75, 133, 216, 470, "TP\其他活动\休闲竞技.bmp|TP\其他活动\休闲竞技1.bmp", "000000", 0.9, 0, 3, 2000)
    Rem 开始
    For 3
        Delay 2000
        dm_ret = dm.FindStr(220, 132, 972, 516, "钓鱼", "5e432d-505050", 0.8, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            TracePrint "靠下"&intx&","&inty
            三界X = intx
            三界Y = INTY
            If inty > 436 Then 
                TracePrint "太靠下移动一下"
                Call 活动内向上移动()
                Delay 1000
                Goto 开始
            End If    	
            TracePrint "找到三界任务"
            TracePrint intx
            TracePrint inty
            dm_ret = dm.FindStr(三界X, 三界Y, 三界X + 262, 三界Y + 62, "参", "6c310a-606060", 0.8, intX, intY)
            If intx >= 0 and inty >= 0 Then 
                TracePrint "找到三界任务，三界任务未完成"
                dm.MoveTo intx, inty
                Delay 50
                dm.LeftClick 
                Exit For
            Else 
                dm_ret = dm.FindStr(三界X, 三界Y, 三界X + 262, 三界Y + 62, "完成", "4a8216-505050", 0.9, intX, intY)
                If intx >= 0 and inty >= 0 Then 
                    TracePrint "找到钓鱼完成"
                    Call 调试输出("三界任务已完成")
                    Exit Sub
                End If
                TracePrint "没找到参加,三界任务结束"
            End If
        Else 
            TracePrint "没找到三界任务 开始下翻"
            call 活动内向上移动()
            Delay 1000
        End If
    Next
    Call 等待进入渔场()
    Rem 开始1
    Call 抛竿()
    Call 买鱼竿()
    Call 使用()
    For 500
        Call 抛竿()
        //判断还有抛竿结束任务
        dm_ret = dm.FindPic(379,321,622,472, "TP\其他活动\抛竿.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "多次操作后还有抛竿任务说明掉满了"
            Call 退出渔场()
            Exit Sub
        Else 
        End if
        For 4
            dm_ret = dm.FindPic(534,402,674,478, "TP\其他活动\丢弃.bmp", "000000", 0.9, 0, intX, intY)
            If intX >= 0 and intY >= 0 Then 
                TracePrint "出现丢弃"
                dm.MoveTo intx, inty
                Delay 10
                dm.LeftClick 
                Delay 1000
                Goto 开始1
                Delay 4000
            Else 
                Delay 500
                TracePrint "没出现丢弃"
                dm_ret = dm.FindPic(124,107,926,582, "TP\其他活动\打开箱子.bmp|TP\其他活动\多谢老人家.bmp", "000000", 0.9, 0, intX, intY)
                If intX >= 0 and intY >= 0 Then 
                    TracePrint "出现打开箱子"
                    dm.MoveTo intx, inty
                    Delay 10
                    dm.LeftClick 
                    Delay 1000
                    Goto 开始1
                    Delay 4000
                Else 
                    Delay 500
                    TracePrint "没出现打开箱子"
                End If
            End If
        Next
        Call 收杆()
        Call 移动浮漂()
        call 遇到大嘴熊()
    Next 
End Sub
Sub 双倍出售()
    dm_ret = dm.FindPic(309,294,551,499, "TP\其他活动\双倍出售.bmp", "000000", 0.9, 0, intX, intY)
    If intX >= 0 and intY >= 0 Then 
        TracePrint "出现双倍出售"
        If 双倍卖鱼 = 1 Then 
            TracePrint "点击双倍出售"
            dm.MoveTo intx, inty
            Delay 10
            dm.LeftClick 
            Call 双倍卖鱼点击满()
            Call 双倍卖鱼点击出售()
        Else 
            TracePrint "不双倍出售"
            dm.MoveTo 682,438
            Delay 10
            dm.LeftClick 
        End If
    Else 
        Delay 500
    End if
End Sub
Sub 双倍卖鱼点击满()
    For 5
        dm_ret = dm.FindPic(524,334,673,474, "TP\其他活动\满.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            dm.MoveTo intx, inty
            Delay 10
            dm.LeftClick 
            Exit Sub
        Else 
            Delay 1000
        End If
    Next 
End Sub
Sub 双倍卖鱼点击出售()
    For 5
        dm_ret = dm.FindPic(394,504,662,614, "TP\其他活动\出售双倍.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            dm.MoveTo intx, inty
            Delay 10
            dm.LeftClick 
            Exit Sub
        Else 
            Delay 1000
        End If
    Next 
End Sub
Sub 退出渔场()
    dm_ret = dm.FindPic(648,18,752,74, "TP\其他活动\退出渔场.bmp", "000000", 0.9, 0, intX, intY)
    If intX >= 0 and intY >= 0 Then 
        dm.MoveTo intx, inty
        Delay 10
        dm.LeftClick 
        Delay 1500
        dm.MoveTo 620,430
        Delay 10
        dm.LeftClick 
    End if
End Sub
Sub 遇到大嘴熊()
    for 3
        dm_ret = dm.FindPic(102,222,407,547, "TP\其他活动\大嘴熊.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "出现大嘴熊"
            Delay 1000
            If 送大嘴熊鱼 = 1 Then 
                TracePrint "送大嘴熊"
                dm.MoveTo 452,445
                Delay 10
                dm.LeftClick 
            Else 
                TracePrint "留着自己吃"
                dm.MoveTo 675,438
                Delay 10
                dm.LeftClick 	
            End If
        Else 
            call 双倍出售()
            Delay 500
        End if
    Next 
End Sub
Sub 等待进入渔场()
    For 20
        dm_ret = dm.FindPic(826,601,1010,727, "TP\其他活动\渔场.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "进入渔场成功"
            Delay 1000
            Exit Sub
        Else 
            TracePrint "等待进入渔场"
            Delay 2000
        End If
    Next 
End Sub
Sub 抛竿()
    For 4
        dm_ret = dm.FindPic(379,321,622,472, "TP\其他活动\抛竿.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "出现抛竿点击"
            dm.MoveTo intx, inty
            Delay 10
            dm.LeftClick 
        Else 
            Delay 1000
            TracePrint "没有找到抛竿"
        End If
        Delay 1500
        dm_ret = dm.FindPic(379,321,622,472, "TP\其他活动\抛竿.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "出现抛竿,那就再点击"
        Else 
            Delay 1000
            TracePrint "再次没有找到抛竿"
            Exit Sub
        End if
    Next
End Sub
Sub 买鱼竿()
    Delay 1500
    For 5
        dm_ret = dm.FindPic(276, 247, 344, 315, "TP\其他活动\买鱼竿.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "没有鱼竿要买"
            dm.MoveTo intx, inty
            Delay 10
            dm.LeftClick 
        Else 
            TracePrint "不需要买鱼竿"
            Exit Sub
        End If
        Delay 1500
        dm_ret = dm.FindPic(714, 592, 949, 702, "TP\其他活动\购买.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "出现商会"
            call 购买鱼竿()
            Exit Sub
        End If
    Next 
End Sub
Sub 购买鱼竿()
    For 5
        dm_ret = dm.FindPic(714, 592, 949, 702, "TP\其他活动\购买.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "出现购买"
            dm.MoveTo intx, inty
            Delay 10
            dm.LeftClick 
            Exit Sub
        Else 
            Delay 1000
            TracePrint "没出现购买"
        End If
    Next 
End Sub
Sub 使用()
    Delay 2000
    dm_ret = dm.FindPic(530, 476, 742, 565, "TP\其他活动\使用.bmp", "000000", 0.9, 0, intX, intY)
    If intX >= 0 and intY >= 0 Then 
        TracePrint "出现使用"
        使用X = intx
        使用Y = inty
        dm_ret = dm.FindPic(276, 247, 344, 315, "TP\其他活动\买鱼竿.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "没有鱼竿"
        Else 
            TracePrint "有鱼竿"
            dm.MoveTo 298,289
            Delay 10
            dm.LeftClick 
            Delay 1500
            dm.MoveTo 使用X,使用Y
            Delay 10
            dm.LeftClick 
        End if
    End If
End Sub
Sub 收杆()
    For 10000
        dm_ret = dm.FindPic(424,268,635,459, "TP\其他活动\钓鱼.bmp", "000000", 0.7, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "点击收杆"
            dm.MoveTo 497,501
            Delay 300
            dm.LeftClick 
            Exit Sub
        Else 
            TracePrint "范围不够"
            Delay 100
        End if
    Next 
End Sub
Sub 移动浮漂()
    For 5
        dm_ret = dm.FindPic(221,637,697,750, "TP\其他活动\钓鱼按钮.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            dm.MoveTo 563, 695
            Delay 50
            dm.LeftDown 
            TracePrint " 找到鱼按钮就"
            Exit For
        Else 
            TracePrint "没找到"
            Delay  500
        End If
    Next  
    For 10000
        dm_ret = dm.FindPic(101,231,1010,609, "TP\其他活动\浮漂1.bmp", "000000", 0.8, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "找到浮漂"
            TracePrint "浮漂位置="&intx
            浮漂X=intx
            If intx < 496 Then 
                TracePrint "向右边拉"
                Call 向右边拉()
            Elseif intx >619 
                TracePrint "向左边拉"
                Call 向左边拉()
            Else 
                TracePrint "不需要啦"
                Delay 10
            End If
        Else 
            TracePrint "没找浮漂"
        End If
        Delay 10
        dm_ret = dm.FindPic(545, 500, 840, 669, "TP\其他活动\收取.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            dm.LeftUp 
            TracePrint "成功钓起"
            Delay 500
            TracePrint intx
            TracePrint inty
            dm.MoveTo intx, inty
            Delay 10
            dm.LeftClick 
            Delay 5000
            Exit Sub
        Else 
            Delay 10
        End If
        dm_ret = dm.FindPic(379,321,622,472, "TP\其他活动\抛竿.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "出现抛竿 可能脱钩了，或者没掉到"
            dm.LeftUp 
            Exit Sub
            Exit sub
        Else 
            Delay 10
            TracePrint "没有找到抛竿"
        End if
    Next  
End Sub
Sub 向右边拉()
    TracePrint "右浮漂="&浮漂x 
    If 浮漂X < 340 Then
        TracePrint "右力度4"
        dm.MoveTo 787, 692
    ElseIf 浮漂X < 394 Then
        TracePrint "右力度3"
        dm.MoveTo 731, 692
    ElseIf 浮漂X < 447 Then
        TracePrint "右力度2"
        dm.MoveTo 682, 692
    ElseIf 浮漂x < 496 Then 
        TracePrint "右力度1"
        dm.MoveTo 623, 692
    End if
End Sub
Sub 向左边拉()
    TracePrint "左浮漂="&浮漂x 
    If 浮漂X > 787 Then 
        TracePrint "左力度4"
        dm.MoveTo 	336,692
    ElseIf 浮漂X > 733 Then
        TracePrint "左力度3"
        dm.MoveTo 391, 692
    ElseIf 浮漂X > 682 Then
        TracePrint "左力度2"
        dm.MoveTo 442, 692
    ElseIf 浮漂X > 619 Then
        TracePrint "左力度1"
        dm.MoveTo 498, 692
    End if
End Sub
//=============================秒商城
Sub 秒商城()
    If 抢关注1 = 1 Then 
        dm_ret = dm.FindPic(91,235,268,397, "TP\其他活动\关注选中.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "已经在关注页面"
        Else 
            MessageBox "请手动打开到摆摊商城，并在关注页面，否则无法运行"
            Exit Sub
        End if
        Call 秒关注()
    Else 
        dm_ret = dm.FindPic(409,56,533,157, "TP\其他活动\摆摊.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "已经在摆摊页面"
        Else 
            MessageBox "请手动打开摆摊商城，并在需要抢购的物品界面，否则无法运行"
            Exit Sub
        End if
        Call 抢指定商品1()
    End If
End Sub
Sub 秒关注()
    For 秒货次数
        Do 
            Call 点击关注()
            dm_ret = dm.FindPic(292,204,606,307, "TP\其他活动\金币.bmp", "000000", 0.9, 0, intX, intY)	
            If intX >= 0 and intY >= 0 Then 
                TracePrint "出现金币"
                Exit Do
            Else 
                Delay 10
            End If
        Loop
        Call 点击第一个物品()
        Call 点击购买()
        Call 确定购买()
    Next 
End Sub
Sub 抢指定商品1()
    dm_ret = dm.UseDict(3)
    For 秒货次数
        Do
            Rem 开始
            Call 点击刷新()
            Delay 100
             Call 点击刷新()
             
            dm_ret = dm.FindPic(292,204,606,307, "TP\其他活动\金币.bmp", "000000", 0.9, 0, intX, intY)	
            If intX >= 0 and intY >= 0 Then 
                TracePrint "出现金币"
                If int(抢指定金额) <> 0 Then 
                    当前物品金额 = dm.Ocr(374,207,598,302,"ffffff-303030",0.9)
                    TracePrint 当前物品金额
                    If int(当前物品金额) >= int(抢指定金额) Then 
                        TracePrint "超出设定价格  不购买"
                        Call 调试输出("超出设定金额，继续刷新")
                        Goto 开始
                        Delay 200
                    Else 
                        TracePrint "没超过指定价格"
                        Exit Do 
                    End If
                Else 
                    Delay 100
                End If
                Exit Do
            Else 
                Delay 10
            End If
        Loop
        Call  指定商品点击第一个物品()
        Call 指定商品点击购买
    Call 点购买后弹出购买()
    Delay 1000
    Next 
End Sub
Sub 点购买后弹出购买()
For 100
	   dm_ret = dm.FindPic(501,536,686,657, "TP\其他活动\最终购买.bmp", "000000", 0.9, 0, intX, intY)	
        If intX >= 0 and intY >= 0 Then 
            TracePrint "弹出窗口后点击购买"
             dm.MoveTo intx, inty
            Delay 10
            dm.LeftClick 
            Exit Sub 
            Else 
            TracePrint "没找到最终购买"
            Delay 10
            End If
            Next 
End Sub

Sub 确定购买()
    For 100
        dm_ret = dm.FindPic(273,467,382,519, "TP\其他活动\确定1.bmp", "000000", 0.9, 0, intX, intY)	
        If intX >= 0 and intY >= 0 Then 
            TracePrint "点击确定"
            dm.MoveTo intx, inty
            Delay 10
            dm.LeftClick 
        Else 
        TracePrint "没找到确定购买"
            Delay 10
        End If
    Next 
End Sub
Sub 点击购买()
    For 10
        dm_ret = dm.FindPic(791,627,840,671, "TP\其他活动\购买1.bmp", "000000", 0.9, 0, intX, intY)	
        If intX >= 0 and intY >= 0 Then 
            dm.MoveTo intx, inty
            Delay 10
            dm.LeftClick 
        End If
        Delay 10
        dm_ret = dm.FindPic(273,467,382,519, "TP\其他活动\确定1.bmp", "000000", 0.9, 0, intX, intY)	
        If intX >= 0 and intY >= 0 Then 
            TracePrint "购买点击成功"
            Exit Sub
        End If
    Next 
End Sub

Function 有数量的物品弹出购买()
    dm_ret = dm.FindPic(514,558,665,628, "TP\其他活动\弹出购买.bmp", "000000", 0.9, 0, intX, intY)	
    If intX >= 0 and intY >= 0 Then 
        TracePrint "点击弹出购买"
        dm.MoveTo intx, inty
        Delay 10
        dm.LeftClick 
        有数量的物品弹出购买 = True
    Else 
        有数量的物品弹出购买 = False
    End if
End Function 
Sub 点击第一个物品()
    For 10 
        dm_ret = dm.FindPic(292,204,606,307, "TP\其他活动\金币.bmp", "000000", 0.9, 0, intX, intY)	
        If intX >= 0 and intY >= 0 Then 
            TracePrint "点击金币"
            dm.MoveTo intx, inty
            Delay 10
            dm.LeftClick 
            Delay 10
            dm.LeftClick 
            Delay 10
        End If
        dm_ret = dm.FindPic(365,195,494,234, "TP\其他活动\点击成功.bmp", "000000", 0.9, 0, intX, intY)	
        If intX >= 0 and intY >= 0 Then 
            TracePrint "已经点击成功"
            Exit Sub
        Else 
            Delay 10
        End If
    Next 	 
End Sub
Sub 指定商品点击第一个物品()
    For 10 
        dm_ret = dm.FindPic(292,204,606,307, "TP\其他活动\金币.bmp", "000000", 0.9, 0, intX, intY)	
        If intX >= 0 and intY >= 0 Then 
            TracePrint "点击金币"
            dm.MoveTo intx, inty
            Delay 10
            dm.LeftClick 
            Call 点击商品等待()
            Delay 150
            dm.LeftClick 
            Delay 10
            dm.LeftClick 
            Delay 10
        End If
        Delay 100
        dm_ret = dm.FindPic(365,195,494,234, "TP\其他活动\点击成功.bmp", "000000", 0.9, 0, intX, intY)	
        If intX >= 0 and intY >= 0 Then 
            TracePrint "已经点击成功"
            Exit Sub
        Else 
            Delay 10
        End If
    Next 	 
End Sub
Sub 指定商品点击购买()
    For 50
    
     dm_ret = dm.FindPic(277,195,605,306, "TP\其他活动\商品确定.bmp", "000000", 0.9, 0, intX, intY)	
        If intX >= 0 and intY >= 0 Then 
    
   
        dm_ret = dm.FindPic(709,591,959,711, "TP\其他活动\购买1.bmp", "000000", 0.9, 0, intX, intY)	
        If intX >= 0 and intY >= 0 Then 
            TracePrint "点击购买"
            dm.MoveTo intx, inty
            Delay 10
            dm.LeftClick 
            Exit Sub
            
        Else 
            TracePrint "没出现购买"
            If 有数量的物品弹出购买 = True Then 
                Exit Sub 
            End If
            Delay 20
        End If
//        Delay 50
//        dm_ret = dm.FindPic(278,194,630,308, "TP\其他活动\购买成功.bmp", "000000", 0.9, 0, intX, intY)	
//        If intX >= 0 and intY >= 0 Then 
//            TracePrint "在次判断购买点击成功"
//            Exit Sub
//        End If
        Else 
        Call  指定商品点击第一个物品()
         End if
        
    Next 
End Sub
Sub 点击商品等待()
    For 400
        dm_ret = dm.FindPic(296,207,604,302, "TP\其他活动\点击成功1.bmp", "000000", 0.9, 0, intX, intY)	
        If intX >= 0 and intY >= 0 Then 
            TracePrint "结束等待，继续点击"
            Delay 20
            Exit Sub 
        Else 
            Delay 10
            TracePrint "等待中"
        End If
    Next 
End Sub
Sub 点击关注()
    dm_ret = dm.FindPic(81,212,270,589, "TP\其他活动\关注0.bmp|TP\其他活动\关注1.bmp", "000000", 0.9, 0, intX, intY)	
    If intX >= 0 and intY >= 0 Then 
        TracePrint "点击关注刷新"
        dm.MoveTo intx, inty
        Delay 10
        dm.LeftClick 
    End if	
End Sub
Sub 点击刷新()
    dm_ret = dm.FindPic(326,621,426,668, "TP\其他活动\刷新.bmp", "000000", 0.9, 0, intX, intY)	
    If intX >= 0 and intY >= 0 Then 
        TracePrint "点击关注刷新"
        dm.MoveTo intx+80, inty
        Delay 10
        dm.LeftClick 
    End if	
End Sub
//==================================秒工坊
Sub 秒工坊
    dm_ret = dm.FindPic(349,50,632,156, "TP\其他活动\工坊.bmp", "000000", 0.9, 0, intX, intY)
    If intX >= 0 and intY >= 0 Then 
        TracePrint "已经在工坊页面"
    Else 
        MessageBox "请手动打开工坊商城，并在需要抢购的物品界面，否则无法运行"
        Exit Sub
    End if
    Call 抢工坊商品()
End Sub
Sub 抢工坊商品()
    dm_ret = dm.UseDict(3)
    For 工坊次数
        Do
            Rem 开始
            Call 工坊刷新()
            Call 调试输出("工坊秒杀中~")
            Delay 100
            dm_ret = dm.FindPic(292,204,606,307, "TP\其他活动\金币.bmp", "000000", 0.9, 0, intX, intY)	
            If intX >= 0 and intY >= 0 Then 
                TracePrint "出现金币"
                If int(工坊金额) <> 0 Then 
                    当前物品金额 = dm.Ocr(374,207,598,302,"ffffff-303030",0.9)
                    TracePrint 当前物品金额
                    If int(当前物品金额) >= int(工坊金额) Then 
                        TracePrint "超出设定价格不购买"
                        Goto 开始
                        Delay 200
                    Else 
                        TracePrint "没超过指定价格"
                        Exit Do 
                    End If
                Else 
                    Delay 100
                End If
                Exit Do
            Else 
                Delay 10
            End If
        Loop
        Call  工坊第一个物品()
        call 工坊点击购买()
        Call 工坊确定购买()
    Next 
End Sub
Sub 工坊刷新()
    dm_ret = dm.FindPic(307,607,419,669, "TP\其他活动\工坊刷新.bmp", "000000", 0.9, 0, intX, intY)	
    If intX >= 0 and intY >= 0 Then 
        TracePrint "点击关注刷新"
        dm.MoveTo intx+80, inty
        Delay 10
        dm.LeftClick 
    End if	
End Sub
Sub 工坊第一个物品()
    For 10 
        dm_ret = dm.FindPic(292,204,606,307, "TP\其他活动\金币.bmp", "000000", 0.9, 0, intX, intY)	
        If intX >= 0 and intY >= 0 Then 
            TracePrint "点击金币"
            dm.MoveTo intx, inty
            Delay 10
            dm.LeftClick 
            Delay 10
            dm.LeftClick 
            Delay 10
            dm.LeftClick 
            Delay 10
            dm.LeftClick 
            Delay 10
        End If
        Delay 100
        dm_ret = dm.FindPic(289,203,610,304, "TP\其他活动\工坊选中.bmp", "000000", 0.9, 0, intX, intY)	
        If intX >= 0 and intY >= 0 Then 
            TracePrint "已经点击成功"
            Exit Sub
        Else 
            Delay 10
        End If
    Next 	 
End Sub
Sub 工坊点击购买()
    For 2
        dm_ret = dm.FindPic(638,543,937,701, "TP\其他活动\工坊购买.bmp", "000000", 0.9, 0, intX, intY)	
        If intX >= 0 and intY >= 0 Then 
            dm.MoveTo intx, inty
            Delay 10
            dm.LeftClick 
        Else 
            TracePrint "没找到工坊购买"
        End If
        Delay 10
        dm_ret = dm.FindPic(286,202,605,314, "TP\其他活动\工坊成功.bmp", "000000", 0.9, 0, intX, intY)	
        If intX >= 0 and intY >= 0 Then 
            TracePrint "购买点击成功"
            Exit Sub
        End If
    Next 
End Sub
Sub 工坊确定购买()
    For 200
        dm_ret = dm.FindPic(525,550,690,640, "TP\其他活动\工坊确定.bmp", "000000", 0.9, 0, intX, intY)	
        If intX >= 0 and intY >= 0 Then 
            TracePrint "点击确定"
            dm.MoveTo intx, inty
            Delay 10
            dm.LeftClick 
            Exit For
        Else 
            Delay 10
        End If
    Next 
End Sub
//=============================================使用活力
Sub 活力使用()
    Call 回到主界面
    Call 点击头像()
    Call 点击活力()
    Call 活力界面复位()
    If 打工 = 1 Then 
        Call 开始打工()
    ElseIf 炼药 = 1 Then
        Call 开始炼药()
    ElseIf 烹饪 = 1 Then
        call 开始烹饪()
    ElseIf 制作临时符 = 1 Then
        Call 点击制作临时符
    ElseIf 制作同心结 = 1 Then
        Call 点击制作同心结()
    End If
End Sub
Sub 点击头像()
    For 5
        dm_ret = dm.FindPic(866, 0, 977, 48, "TP\其他活动\头像.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "点击头像"
            dm.MoveTo 974,51
            Delay 10
            dm.LeftClick 
        End If
        Delay 2000
        dm_ret = dm.FindPic(421,57,594,155, "TP\其他活动\人物界面.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "已经打开到任务界面"
            Exit Sub
        Else 
        End If
    Next 
End Sub
Sub 点击活力()
    For 5
        dm_ret = dm.FindPic(805,266,916,325, "TP\其他活动\使用活力.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "点击使用活力"
            dm.MoveTo intx,inty
            Delay 10
            dm.LeftClick 
        End If
        Delay 1000
        dm_ret = dm.FindPic(449,101,651,175, "TP\其他活动\活力界面.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "已经打开到活力界面"
            Exit Sub
        Else 
        End If
    Next 
End Sub
Sub 活力界面复位()
    dm.MoveTo 695,263
    Delay 10
    dm.LeftDown 
    Delay 10
    dm.MoveTo 695,595
    Delay 10
    dm.LeftUp 
    Delay 1500
End Sub
Sub 开始打工()
    For 30
        dm_ret = dm.FindPic(712,245,863,619, "TP\其他活动\打工.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            dm.MoveTo intx, inty
            Delay 10
            dm.LeftClick 
            TracePrint "点击打工"
        End if
        Delay 1000
        dm_ret = dm.FindPic(219,155,378,202, "TP\其他活动\活力.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
        Else 
            TracePrint "活力不足"
            Exit Sub
        End if
    Next 
End Sub
Sub 开始炼药()
    Call 点击帮派技能()
    Call 点击炼药()
    Call 点击打开炼药界面()
    For 5
        Call 开始炼药中()
    Next 
End Sub
Sub 点击帮派技能()
    For 5
        dm_ret = dm.FindPic(510,235,867,628, "TP\其他活动\帮派技能.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "点击帮派技能"
            dm.MoveTo intx+200, inty+30
            Delay 10
            dm.LeftClick 
        End If
        Delay 1000
        dm_ret = dm.FindPic(363,74,567,132, "TP\其他活动\帮派技能界面.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "成功打开帮派技能界面"
            Exit Sub 
        End If
    Next    
End Sub
Sub 点击炼药()
    For 5
        dm_ret = dm.FindPic(75,114,285,680, "TP\其他活动\炼药.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "点击炼药"
            dm.MoveTo intx, inty
            Delay 10
            dm.LeftClick 
        End If
        Delay 1000
        dm_ret = dm.FindPic(297,123,604,245, "TP\其他活动\炼药选中.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "成功选中炼药"
            Exit Sub 
        End If
    Next    
End Sub
Sub 点击打开炼药界面()
    For 5
        dm_ret = dm.FindPic(641,549,895,674, "TP\其他活动\打开炼药界面.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "点击打开炼药界面"
            dm.MoveTo intx, inty
            Delay 10
            dm.LeftClick 
        End If
        Delay 1000
        dm_ret = dm.FindPic(369,115,682,182, "TP\其他活动\炼药界面1.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "成功打开炼药界面"
            Exit Sub 
        End If
    Next
End sub
Sub 开始炼药中()
    dm_ret = dm.FindPic(676,537,822,610, "TP\其他活动\开始炼药.bmp", "000000", 0.9, 0, intX, intY)
    If intX >= 0 and intY >= 0 Then 
        TracePrint "点击开始炼药界面"
        dm.MoveTo intx, inty
        Delay 10
        dm.LeftClick 
    End If
End Sub
Sub 开始烹饪()
    Call 点击帮派技能()
    Call 点击烹饪()
    For 5
        Call 开始烹饪中()
    Next 
End Sub	
Sub 点击烹饪()
    For 5
        dm_ret = dm.FindPic(75,114,285,680, "TP\其他活动\烹饪.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "点击烹饪"
            dm.MoveTo intx, inty
            Delay 10
            dm.LeftClick 
        End If
        Delay 1000
        dm_ret = dm.FindPic(297,123,604,245, "TP\其他活动\烹饪选中.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "成功选中烹饪"
            Exit Sub 
        End If
    Next    
End Sub
Sub 开始烹饪中()
    dm_ret = dm.FindPic(637,558,894,670, "TP\其他活动\烹饪中.bmp", "000000", 0.9, 0, intX, intY)
    If intX >= 0 and intY >= 0 Then 
        TracePrint "点击开始烹饪"
        dm.MoveTo intx, inty
        Delay 10
        dm.LeftClick 
    End If
End Sub
Sub 点击制作同心结()
    For 30
        dm_ret = dm.FindPic(510,235,867,628, "TP\其他活动\制作同心结.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "点击制作同心结"
            dm.MoveTo intx+200, inty+30
            Delay 10
            dm.LeftClick 
        End If
        Delay 1000
        dm_ret = dm.FindPic(219,155,378,202, "TP\其他活动\活力.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
        Else 
            TracePrint "活力不足"
            Exit Sub
        End if
    Next 
End Sub
Sub 点击制作临时符()
    For 30
        dm_ret = dm.FindPic(510,235,867,628, "TP\其他活动\制作临时符.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "点击制作临时符"
            dm.MoveTo intx+200, inty+30
            Delay 10
            dm.LeftClick 
        End If
        Delay 1000
        dm_ret = dm.FindPic(219,155,378,202, "TP\其他活动\活力.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
        Else 
            TracePrint "活力不足"
            Exit Sub
        End If
    Next 
End Sub
//===============================摆摊出售()
Sub 摆摊出售()
    Call 调试输出("摆摊出售中~")
    Call 回到主界面()
    Call 点击包裹()
    Call 回到主界面()
    Call 点击商城()
    Call 点击商会()
    Call 点击商会出售()
    Call 判断是否要卖()
    //====以下开始金币摆摊
    Call 回到主界面()
    Call 点击商城()
    Call 点击摆摊()
    Call 点击商会出售()
    Call 金币出售()
End Sub
Sub 点击商城()
    For 5
        dm_ret = dm.FindPic(5,95,134,358, "TP\其他活动\商城.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "点击商城"
            dm.MoveTo intx, inty
            Delay 10
            dm.LeftClick 
        End If
        Delay 1000
        dm_ret = dm.FindPic(118,56,425,193, "TP\其他活动\商城1.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "点击商城"
            TracePrint "成功打开城"
            Exit Sub
        End If
    Next 
End Sub
Sub 点击商会()
    For 5
        dm_ret = dm.FindPic(906,185,1002,293, "TP\其他活动\商会0.bmp|TP\其他活动\商会1.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "点击商城"
            dm.MoveTo intx, inty
            Delay 10
            dm.LeftClick 
        End If
        Delay 1000
        dm_ret = dm.FindPic(411,67,668,176, "TP\其他活动\商会3.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "成功打开商会"
            Exit Sub
        End If
    Next 
End Sub
Sub 点击摆摊()
    For 5
        dm_ret = dm.FindPic(893,274,1018,425, "TP\其他活动\摆摊0.bmp|TP\其他活动\摆摊1.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "点击摆摊"
            dm.MoveTo intx, inty
            Delay 10
            dm.LeftClick 
        End If
        Delay 1000
        dm_ret = dm.FindPic(180,34,663,160, "TP\其他活动\摆摊.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "成功打开摆摊"
            Exit Sub
        End If
    Next 
End Sub
Sub 点击商会出售()
    For 5
        dm_ret = dm.FindPic(257,125,445,189, "TP\其他活动\我要出售0.bmp|TP\其他活动\我要出售1.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "点击我要出售"
            dm.MoveTo intx, inty
            Delay 10
            dm.LeftClick 
        End If
        Delay 1000
        dm_ret = dm.FindPic(257,125,445,189, "TP\其他活动\我要出售1.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "成功打开我要出售"
            Exit Sub
        End If
    Next 
End Sub
Function 判断是否要卖()
    格子I=1
    格子X左 =94
    格子Y左 =199
    格子X右 =301
    格子Y右 = 387
    For 50
        dm_ret = dm.FindPic(格子X左,格子Y左,格子X右,格子Y右, "TP\其他活动\银币.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "找到物品"
            物品X = intx
            物品Y=inty
            Call 判断宝石()
            Delay 1000
        Else 
            Delay 1000
            dm_ret = dm.FindPic(350,603,604,696 ,"TP\其他活动\没物品.bmp", "000000", 0.9, 0, intX, intY)
            If intX >= 0 and intY >= 0 Then 
                TracePrint "结束商城出售"
                Exit Function 
            End if
        End If
    Next 
End Function
Sub 判断宝石()
    If 保留宝石 = 1 Then 
        Delay 500
        TracePrint "需要判断宝石"
        dm_ret = dm.FindPic(格子X左,格子Y左,格子X右,格子Y右, "TP\其他活动\光芒石.bmp|TP\其他活动\太阳石.bmp|TP\其他活动\月亮石.bmp|TP\其他活动\舍利子.bmp|TP\其他活动\翡翠石.bmp|TP\其他活动\黑宝石.bmp|TP\其他活动\红纹石.bmp|TP\其他活动\神秘石.bmp|TP\其他活动\昆仑玉.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "出现宝石格子i="&格子I
            If 格子I <= 3 Then 
                格子X左 = 格子X左 + 200
                格子X右 = 格子X右 + 200
                格子I = 格子I + 1
            ElseIf 格子I = 4 Then
                格子X左 =100
                格子Y左 =392
                格子X右 =301
                格子Y右 = 581
                格子I = 格子I + 1
            ElseIf 格子I >= 5 Then
                格子X左 = 格子X左 + 200
                格子X右 = 格子X右 + 200
                格子I = 格子I + 1
            End If
        Else 
            TracePrint "不是宝石"
            TracePrint "不用保留宝石进行出售"
            dm.MoveTo 物品X, 物品Y
            Delay 10
            dm.LeftClick 
            Call 出售
        End If
    Else 
        TracePrint "不用保留宝石进行出售"
        dm.MoveTo 物品X, 物品Y
        Delay 10
        dm.LeftClick 
        Call 出售
    End If
End Sub
Sub 出售()
    Delay 500
    dm_ret = dm.FindPic(766,598,930,683 ,"TP\其他活动\出售.bmp", "000000", 0.9, 0, intX, intY)
    If intX >= 0 and intY >= 0 Then 
        TracePrint "点击出售"
        dm.MoveTo intx, inty
        Delay 10
        dm.LeftClick 
        Delay 1500
    Else 
        TracePrint "没找到出售"
    End if
End Sub
Sub 金币出售()
    dm_ret = dm.FindPic(410, 536, 586, 593, "TP\其他活动\上架.bmp", "000000", 0.9, 0, intX, intY)
    If intX >= 0 and intY >= 0 Then 
        TracePrint "还有空位可以上架"
        Call 点击物品 
    End if
End Sub
Sub 点击物品()
    坐标X=649
    坐标Y = 232
    坐标I=1
    For 30
        Delay 1000
        dm_ret = dm.FindPic(坐标X-34, 坐标Y-30, 坐标X+34, 坐标Y+30, "TP\其他活动\没有物品.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "没有物品了，结束任务"
            Exit Sub
        Else 
            TracePrint "还有物品"
        End if
        TracePrint "坐标X="&坐标X&"坐标Y="&坐标Y
        dm.MoveTo 坐标X, 坐标Y
        Delay 10
        dm.LeftClick 
        Delay 1000
        dm_ret = dm.FindPic(165,108,916,617, "TP\其他活动\金币.bmp|TP\其他活动\出售3.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            取消X=intx 
            取消Y=inty
            TracePrint "成功打开售卖界面"
            Delay 500
            dm_ret = dm.FindPic(0,0,1024,768, "TP\其他活动\出售4.bmp|TP\其他活动\出售5.bmp|TP\其他活动\出售6.bmp|TP\其他活动\出售7.bmp", "000000", 0.8, 0, intX, intY)
            If intX >= 0 and intY >= 0 Then 
                TracePrint "出现推荐价格"
                Delay 1500
                Call 比价上架()
            Else 
                TracePrint "取消上架"
                Call  取消上架()
                TracePrint "坐标I"&坐标I
                If 坐标I <= 3 Then 
                    坐标X = 坐标X + 79
                    坐标I = 坐标I + 1
                ElseIf 坐标I = 4 Then
                    坐标Y = 坐标Y + 85
                    坐标X = 649
                    坐标I=1
                End If
            End if
        Else 
            TracePrint "没打开售卖界面"
            dm_ret = dm.FindPic(349,510,591,601, "TP\其他活动\金币1.bmp", "000000", 0.9, 0, intX, intY)
            If intX >= 0 and intY >= 0 Then 
                TracePrint "上架位置满了 结束上架"
                Exit Sub
            End if
            If 坐标I < 3 Then 
                坐标X = 坐标X + 35
                坐标I = 坐标I + 1
            ElseIf 坐标I = 4 Then
                坐标Y = 坐标Y + 85
                坐标X = 649
                坐标I=1
            End If
            Delay 500
        End If
    Next 
End Sub
Sub 取消上架()
    dm_ret = dm.FindPic(155, 59, 939, 687, "TP\其他活动\取消.bmp|TP\其他活动\取消1.bmp", "000000", 0.9, 0, intX, intY)
    If intX >= 0 and intY >= 0 Then 
        TracePrint "点击取消"
        dm.MoveTo intx, inty
        Delay 10
        dm.LeftClick 
        Delay 500
    End if
End Sub
Sub 先调高()
    dm_ret = dm.UseDict(3)
    For 10
        物品售价 = dm.Ocr(683, 467, 838, 511, "ffffff-303030", 0.9)
        TracePrint "物品售价"&物品售价
        Delay 100
        最低售价 = dm.Ocr(205, 191, 542, 291, "ffffff-303030", 0.9)
        TracePrint "最低售价"&最低售价
        If int(物品售价) <= int(最低售价) Then 
            dm.MoveTo 658,489
            Delay 10
            dm.LeftClick 
            Delay 1000
        ElseIf int(物品售价) < int(最低售价) Then
            TracePrint "点击上架"
            dm.MoveTo 799,616
            Delay 10
            dm.LeftClick 
            Delay 1000
            Call 确定上架()
            Exit Sub
        End if
    Next 
End Sub
Sub 比价上架()
    判断上架次数I
    dm_ret = dm.UseDict(3)
    dm_ret = dm.FindPic(585,312,909,595, "TP\其他活动\出售4.bmp", "505050", 0.9, 1, intX, intY)
    If intX >= 0 and intY >= 0 Then 
        TracePrint "找到价格哪一行"
        TracePrint intx
        TracePrint  inty 
        价格X = intx
        价格Y = inty
    Else 
        TracePrint "没找到价格那一行"
    End if
    物品售价 = dm.Ocr(价格X, 价格Y, 价格X+250, 价格Y+40, "ffffff-303030", 0.9)
    TracePrint 物品售价
    If 保留大于勾选 = 1 Then 
        TracePrint "1"
        If int(物品售价) > int(大于保留) Then 
            TracePrint "2"
            Call  取消上架()
            TracePrint "坐标I"&坐标I
            If 坐标I <= 3 Then 
                坐标X = 坐标X + 79
                坐标I = 坐标I + 1
            ElseIf 坐标I = 4 Then
                坐标Y = 坐标Y + 85
                坐标X = 649
                坐标I=1
            End If
            Exit Sub
        end if 
    End If
    If 出价下调 = 1 Then 
        For 5
            物品售价 = dm.Ocr(价格X, 价格Y, 价格X+250, 价格Y+40, "ffffff-303030", 0.9)
            最低售价 = dm.Ocr(205, 191, 542, 291, "ffffff-303030", 0.9)
            If int(物品售价) < int(最低售价) Then 
                TracePrint "少了 先加钱"
                call 点击加价()
                Delay 800
            Else 
                TracePrint "没有小于那就退出循环"
                Exit For
            End If
        Next 
        For 10
            物品售价 = dm.Ocr(价格X, 价格Y, 价格X+250, 价格Y+40, "ffffff-303030", 0.9)
            TracePrint "物品售价"&物品售价
            Delay 100
            最低售价 = dm.Ocr(205, 191, 542, 291, "ffffff-303030", 0.9)
            TracePrint "最低售价"&最低售价
            If int(物品售价) >= int(最低售价) Then 
                判断上架次数I =  判断上架次数I  + 1
                If 判断上架次数I = 3 Then 
                    TracePrint "点击上架"
                    dm.MoveTo 799,616
                    Delay 10
                    dm.LeftClick 
                    Delay 1000
                    Call 确定上架()
                    判断上架次数I = 0
                    call 商品没卖掉框()
                    Exit Sub
                End If
                call   点击减价()
                Delay 1000
            ElseIf int(物品售价) < int(最低售价) Then
                TracePrint "点击上架"
                dm.MoveTo 799,616
                Delay 10
                dm.LeftClick 
                Delay 1000
                Call 确定上架()
                call 商品没卖掉框()
                Exit Sub
            ElseIf int(物品售价) = int(最低售价) Then
            End if
        Next
    Else 
        TracePrint "不需要下调点击上架"
        dm.MoveTo 799,616
        Delay 10
        dm.LeftClick 
        Delay 1000
        Call 确定上架()
        call 商品没卖掉框()
    End If
End Sub
Sub 商品没卖掉框()
    Delay 3000
    dm_ret = dm.FindPic(569,144,955,642, "TP\其他活动\商品没卖掉框.bmp", "000000", 0.9, 0, intX, intY)
    If intX >= 0 and intY >= 0 Then 
        TracePrint "出现商品没卖掉框"
        If 坐标I <= 3 Then 
            坐标X = 坐标X + 79
            坐标I = 坐标I + 1
        ElseIf 坐标I = 4 Then
            坐标Y = 坐标Y + 85
            坐标X = 649
            坐标I=1
        End If
    End if
End Sub
Sub 确定上架()
    dm_ret = dm.FindPic(427,522,632,604, "TP\其他活动\确定上架.bmp", "000000", 0.9, 0, intX, intY)
    If intX >= 0 and intY >= 0 Then 
        TracePrint "确定上架"
        dm.MoveTo intx,inty
        Delay 10
        dm.LeftClick 
        Delay 1000
    Else 
    End if
End Sub
Sub 点击减价()
    dm_ret = dm.FindPic(530,261,919,601, "TP\其他活动\减价.bmp", "505050", 0.9, 1, intX, intY)
    If intX >= 0 and intY >= 0 Then 
        dm.MoveTo intx,inty
        Delay 10
        dm.LeftClick 
        TracePrint "减价"
    End if
End Sub
Sub 点击加价()
    dm_ret = dm.FindPic(530,261,919,601, "TP\其他活动\加价.bmp", "505050", 0.9, 1, intX, intY)
    If intX >= 0 and intY >= 0 Then 
        dm.MoveTo intx,inty
        Delay 10
        dm.LeftClick 
        TracePrint "加价"
    End if
End Sub
//===========================================整理背包()
Sub 整理背包()
    Call 回到主界面
    Call 点击包裹
    For 3
        坐标X=549
        坐标Y = 239
        第一排I=1
        For 25
            dm.MoveTo 坐标X, 坐标Y
            Delay 10
            dm.LeftClick 
            Delay 500
            dm_ret = dm.FindPic(坐标X-34, 坐标Y-30, 坐标X+34, 坐标Y+30, "TP\其他活动\没有物品1.bmp", "000000", 0.9, 0, intX, intY)
            If intX >= 0 and intY >= 0 Then 
                TracePrint "没有物品了，结束任务"
                Exit Sub
            Else 
                TracePrint "还有物品"
            End if
            Call 判断是使用还是丢弃()
            Delay 500
            dm.MoveTo 564, 90
            Delay 10
            dm.LeftClick 
            Delay 500
            If 第一排I <=4 Then 
                坐标X = 坐标X + 83
                第一排I = 第一排I + 1
            ElseIf 第一排I = 5 Then
                坐标Y = 坐标Y + 85
                坐标X = 549
                第一排I=1
            End If
        Next 
        Call 包裹上翻
    Next 
End Sub
Sub 判断是使用还是丢弃()
    dm_ret = dm.UseDict(5)//整理背包=5
    //   dm_ret = dm.FindStr(47, 87, 940, 693, "培养助战|九幽雅集|阵法道具|日常药品|有效期至", "e0d421-303030|ff0c00-303030", 0.9, intX, intY)
    //       If intX >= 0 and intY >= 0 Then 
    //      TracePrint "使用"
    //      Else 
    //      TracePrint "不适用"
    //       End If
    If 培养材料 = 1 Then 
        dm_ret = dm.FindStr(47, 87, 940, 693, "培养助战", "e0d421-303030", 0.9, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "使用"
            dm.MoveTo 坐标X, 坐标Y
            Delay 10
            dm.LeftClick 
            Delay 10
            dm.LeftClick 
        Else 
            TracePrint "不使用"
        End If
    End If
    If 九幽雅集 = 1 Then 
        dm_ret = dm.FindStr(47, 87, 940, 693, "九幽雅集", "e0d421-303030", 0.9, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "使用"
            Call 更多()
            call 使用物品()
        Else 
            TracePrint "不使用"
        End If
    End If
    If 阵法残卷 = 1 Then 
        dm_ret = dm.FindStr(47, 87, 940, 693, "阵法道具", "e0d421-303030", 0.9, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "使用"
            Call 使用阵法()
        Else 
            TracePrint "不使用"
        End If
    End If
    If 日常药物 = 1 Then 
        dm_ret = dm.FindStr(47, 87, 940, 693, "日常药品", "e0d421-303030", 0.9, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "使用"
            call 使用物品()
        Else 
            TracePrint "不使用"
        End If
    End If
    If 过期物品 = 1 Then 
        dm_ret = dm.FindStr(47, 87, 940, 693, "有效期至", "ff0c00-404040", 0.9, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "使用"
            dm.MoveTo 坐标X, 坐标Y
            Delay 10
            dm.LeftClick 
            Delay 10
            dm.LeftClick 
        Else 
            TracePrint "不使用"
        End If
    End If
    ///=============丢弃
    If 帮派道具 = 1 Then 
        dm_ret = dm.FindStr(47, 87, 940, 693, "帮派道具", "e0d421-303030", 0.9, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "丢弃"
            call 丢弃物品()
        Else 
            TracePrint "不丢弃"
        End If
    End If
End Sub
Sub 更多()
    For 10
        dm_ret = dm.FindPic(46,89,942,693, "TP\其他活动\更多1.bmp", "000000", 0.9, 0, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            dm.MoveTo intx, inty 
            Delay 10
            dm.LeftClick 
            TracePrint "点击更多"
        End If
        Delay 1000
        dm_ret = dm.FindPic(46,89,942,693, "TP\其他活动\更多.bmp", "000000", 0.9, 0, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            TracePrint "打开更多成功"
            Exit Sub
        Else 
            TracePrint "没打开更多"
            Exit Sub
        End If
    Next    
End Sub
sub 使用物品()
    For 10
        dm_ret = dm.FindPic(46,89,942,693, "TP\其他活动\使用物品.bmp", "000000", 0.9, 0, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            dm.MoveTo intx, inty 
            Delay 10
            dm.LeftClick 
            TracePrint "点击使用"
        End If
        Delay 1000
        dm_ret = dm.FindPic(46,89,942,693, "TP\其他活动\使用物品.bmp", "000000", 0.9, 0, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            TracePrint "物品还在，继续使用"
        Else 
            TracePrint "物品消失"
            Exit Sub
        End If
    Next    
End Sub
Sub 丢弃物品()
    dm_ret = dm.FindPic(46,89,942,693, "TP\其他活动\丢弃物品.bmp", "000000", 0.9, 0, intX, intY)
    If intx >= 0 and inty >= 0 Then 
        dm.MoveTo intx, inty 
        Delay 10
        dm.LeftClick 
        TracePrint "点击丢弃"
    End If
    Delay 1000
    dm_ret = dm.FindPic(46,89,942,693, "TP\其他活动\确定丢弃.bmp", "000000", 0.9, 0, intX, intY)
    If intx >= 0 and inty >= 0 Then 
        dm.MoveTo intx, inty 
        Delay 10
        dm.LeftClick 
        TracePrint "确定丢弃"
    Else 
        Exit Sub
    End If
    Delay 1000 
End Sub
Sub 使用阵法()
    dm_ret = dm.FindPic(46,89,942,693, "TP\其他活动\使用物品.bmp", "000000", 0.9, 0, intX, intY)
    If intx >= 0 and inty >= 0 Then 
        dm.MoveTo intx, inty 
        Delay 10
        dm.LeftClick 
        TracePrint "点击使用"
    End If
    Delay 1000
End Sub
//=============================================获取信息
Sub 获取信息()
    Call 回到主界面
    Call 点击头像()
    Call 个人信息判断是哪个模拟器()
    Call 回到主界面
    call 打开包裹()
    Call 识别金币
End Sub
Sub 识别金币()
    dm_ret = dm.UseDict(4)
    现有金币 = dm.Ocr(61, 632, 273, 662, "ffffff-303030", 0.9)
    TracePrint 现有金币
    call 调试输出金币(现有金币)
End Sub
Sub 个人信息判断是哪个模拟器()
    个人信息ID = GetThreadID()
    If 个人信息ID  = 线程ID1 Then 
        dm_ret = dm.Capture(158, 138, 396, 168, "TP\个人资料\个人信息1.bmp")
        Delay 50
        Form1.个人信息1.Picture="TP\个人资料\个人信息1.bmp"
    End If
    If 个人信息ID = 线程id2 Then 
        dm_ret = dm.Capture(158, 138, 396, 168, "TP\个人资料\个人信息2.bmp")
        Delay 50
        Form1.个人信息2.Picture="TP\个人资料\个人信息2.bmp"
    End If
    If 个人信息ID = 线程id3 Then 
        dm_ret = dm.Capture(158, 138, 396, 168, "TP\个人资料\个人信息3.bmp")
        Delay 50
        Form1.个人信息3.Picture="TP\个人资料\个人信息3.bmp"
    End If
    If 个人信息ID = 线程id4 Then 
        dm_ret = dm.Capture(158, 138, 396, 168, "TP\个人资料\个人信息4.bmp")
        Delay 50
        Form1.个人信息4.Picture="TP\个人资料\个人信息4.bmp"
    End If
    If 个人信息ID = 线程id5 Then 
        dm_ret = dm.Capture(158, 138, 396, 168, "TP\个人资料\个人信息5.bmp")
        Delay 50
        Form1.个人信息5.Picture="TP\个人资料\个人信息5.bmp"
    End If
    If 个人信息ID = 线程id6 Then 
        dm_ret = dm.Capture(158, 138, 396, 168, "TP\个人资料\个人信息6.bmp")
        Delay 50
        Form1.个人信息6.Picture="TP\个人资料\个人信息6.bmp"
    End If
    If 个人信息ID = 线程id7 Then 
        dm_ret = dm.Capture(158, 138, 396, 168, "TP\个人资料\个人信息7.bmp")
        Delay 50
        Form1.个人信息7.Picture="TP\个人资料\个人信息7.bmp"
    End If
    If 个人信息ID = 线程id8 Then 
        dm_ret = dm.Capture(158, 138, 396, 168, "TP\个人资料\个人信息8.bmp")
        Delay 50
        Form1.个人信息8.Picture="TP\个人资料\个人信息8.bmp"
    End If
    If 个人信息ID = 线程id9 Then 
        dm_ret = dm.Capture(158, 138, 396, 168, "TP\个人资料\个人信息9.bmp")
        Delay 50
        Form1.个人信息9.Picture="TP\个人资料\个人信息9.bmp"
    End If
    If 个人信息ID = 线程id10 Then 
        dm_ret = dm.Capture(158, 138, 396, 168, "TP\个人资料\个人信息10.bmp")
        Delay 50
        Form1.个人信息10.Picture="TP\个人资料\个人信息10.bmp"
    End If
    If 个人信息ID = 线程id11 Then 
        dm_ret = dm.Capture(158, 138, 396, 168, "TP\个人资料\个人信息11.bmp")
        Delay 50
        Form1.个人信息11.Picture="TP\个人资料\个人信息11.bmp"
    End If
    If 个人信息ID = 线程id12 Then 
        dm_ret = dm.Capture(158, 138, 396, 168, "TP\个人资料\个人信息12.bmp")
        Delay 50
        Form1.个人信息12.Picture="TP\个人资料\个人信息12.bmp"
    End If
    If 个人信息ID = 线程id13 Then 
        dm_ret = dm.Capture(158, 138, 396, 168, "TP\个人资料\个人信息13.bmp")
        Delay 50
        Form1.个人信息13.Picture="TP\个人资料\个人信息13.bmp"
    End If
    If 个人信息ID = 线程id14 Then 
        dm_ret = dm.Capture(158, 138, 396, 168, "TP\个人资料\个人信息14.bmp")
        Delay 50
        Form1.个人信息14.Picture="TP\个人资料\个人信息14.bmp"
    End If
    If 个人信息ID = 线程id15 Then 
        dm_ret = dm.Capture(158, 138, 396, 168, "TP\个人资料\个人信息15.bmp")
        Delay 50
        Form1.个人信息15.Picture="TP\个人资料\个人信息15.bmp"
    End If
End Sub
//==================>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>一键升级
Sub 一键40级
Do 
	Call 点击我有经验()
	Call 点击框框()
	Delay 1000
	Loop 
End Sub

Sub 点击框框()
	  dm_ret = dm.FindPic(0, 0, 1024, 768, "TP\一键40\框框.bmp", "000000", 0.9, 0, intX, intY)
	      If intX >= 0 and intY >= 0 Then 
	      TracePrint "点击框框"
	       dm.MoveTo intx+50, inty
        Delay 10
        dm.LeftClick 
	      End if
	
End Sub
Sub 点击我有经验()
	   dm_ret = dm.FindPic(0, 0, 1024, 768, "TP\一键40\我有经验.bmp", "000000", 0.9, 0, intX, intY)
	      If intX >= 0 and intY >= 0 Then 
	      TracePrint "点击我有经验"
	       dm.MoveTo intx, inty
        Delay 10
        dm.LeftClick 
	      End if
End Sub

//========>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>清理主线
Sub 清理主线()
Rem 开始
Call 回到主界面()
	Call 打开主线()
	For 1000
Call 不动检测
For 2
	Call 跳过对话()
Call 右下角弹窗()
Call  点击框框()
	  Call 师门召唤兽购买上交()
                Delay 10
                Call 师门使用
                Delay 10
                Call 摆摊购买()
                Delay 10
                call  工坊购买()
                Delay 10
                Call 点击(407, 339, 585, 439, "TP\师门\完成弹窗.bmp", "000000", 0.9, 0, 1, 500)
                Delay 10
  
                Call 师门战斗中()
           
	Delay 500
	Next 
	If 不动判断 = True Then 
		Goto 开始
		
	End If
	Next 
End Sub
Sub 右下角弹窗()
 dm_ret = dm.FindPic(672,171,1006,616, "TP\一键40\右下角弹窗.bmp", "000000", 0.9, 0, intX, intY)

	   If intX >= 0 and intY >= 0 Then 
	   TracePrint "点击弹窗"
	    dm.MoveTo intx, inty
        Delay 10
        dm.LeftClick 
	   
	   	End If
	   	
End Sub
Sub 打开主线()
Call 回到主界面
Call 打开任务栏
   Call 任务内点击当前()
	Call 点击主线任务()
	Call 马上传送()
End Sub
Sub 点击主线任务()
For 5
  dm_ret = dm.FindPic(52,127,298,684, "TP\一键40\展开主线任务.bmp", "000000", 0.9, 0, intX, intY)

	   If intX >= 0 and intY >= 0 Then 
	      TracePrint "展开主线任务"
	       dm.MoveTo intx, inty
        Delay 10
        dm.LeftClick 
	      End If
	      Delay 1000
	      dm_ret = dm.FindPic(52,127,298,684, "TP\一键40\展开主线任务2.bmp", "000000", 0.9, 0, intX, intY)

	   If intX >= 0 and intY >= 0 Then 
	Exit Sub 
	      End If
	Next 
End Sub
//=========================共享操作
Sub 包裹归位()
    TracePrint "包裹归位翻到最上"
    for 3
        包裹下翻I=208
        dm.MoveTo 724,包裹下翻I
        Delay 20
        dm.LeftDown 
        For 18
            包裹下翻I=包裹下翻I+20
            dm.MoveTo 724, 包裹下翻I
            Delay 100
            TracePrint "归位中"
        Next
        Delay 100
        dm.LeftUp 
        Delay 500
    Next 
End Sub
Sub 包裹上翻()
    包裹上翻I=525
    dm.MoveTo 712,包裹上翻i
    Delay 20
    dm.LeftDown 
    For 19
        包裹上翻I=包裹上翻I-20
        dm.MoveTo 712, 包裹上翻I
        Delay 100
    Next
    Delay 100
    dm.LeftUp 
End Sub
Sub 点击包裹()
    For 5
        dm_ret = dm.FindPic(938,589,1017,691, "TP\主界面0.bmp", "101010", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            dm.MoveTo intx,inty
            Delay 50
            dm.LeftClick 
            TracePrint "点击包裹"
        End If
        Delay 1000
        dm_ret = dm.FindPic(730,550,914,705, "TP\包裹内.bmp", "101010", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "包裹打开成功"
            call  包裹归位()
            dm.MoveTo 802,639
            Delay 50
            dm.LeftClick 
            TracePrint "点击整理"
            Delay 1000
            Exit Sub
        Else 
            Delay 100
            TracePrint "等待包裹"
        End If 
    Next  
End Sub
Sub 打开包裹()
    For 5
        dm_ret = dm.FindPic(938,589,1017,691, "TP\主界面0.bmp", "101010", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            dm.MoveTo intx,inty
            Delay 50
            dm.LeftClick 
            TracePrint "点击包裹"
        End If
        Delay 1000
        dm_ret = dm.FindPic(730,550,914,705, "TP\包裹内.bmp", "101010", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "包裹打开成功"
            Exit Sub
        Else 
            Delay 100
            TracePrint "等待包裹"
        End If 
    Next  
End Sub
Sub 右侧任务栏归位()
    If 找图判断(788, 79, 913, 157, "TP\关闭\右侧任务栏.bmp|TP\抓鬼\任务0.bmp|TP\抓鬼\任务1.bmp", "000000", 0.9, 0, 5, 1000) = 1 Then 
        TracePrint "找到任务栏"
        dm.MoveTo 837, 167
        Delay 50
        dm.LeftDown 
        Delay 50
        dm.MoveTo 837, 380
        Delay 50
        dm.LeftUp
        TracePrint"开始滚动"
        Delay 速度
    Else 
        TracePrint "没找到任务栏"
    End If
End Sub
Sub 点击确定()
    For 5
        dm_ret = dm.FindPic(298, 269, 419, 321, "确定.bmp", "000000", 0.9, 0, intX, intY)or dm_ret = dm.FindPic(305, 270, 399, 316, "师门确定.bmp", "000000", 0.9, 0, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            dm.MoveTo intx, inty
            Delay 500
            dm.LeftClick 
            TracePrint "点击确定"
        Else 
            Delay 500
        End If
    Next  
End Sub
Sub 右侧任务栏滑动()
    dm.MoveTo 503, 175
    Delay 100
    dm.LeftDown
    dm.MoveTo 503, 128
    Delay 1000
    dm.LeftUp 
    Delay 1000
End Sub
Sub 右侧任务栏滑到顶()
    dm.MoveTo  	519,125
    dm.LeftDown
    dm.MoveTo  	519,297
    dm.LeftUp 
    Delay 1000
End Sub
Sub 活动内向上移动()
    Dim i
    dm.MoveTo 594, 448
    i=448
    dm.LeftDown 
    For 29
        i=i-10
        dm.MoveTo 596, i
        Delay 50
    Next
    dm.LeftUp 
    Delay 1000
End Sub
Sub 回到主界面
    Rem 开始
    If 找图判断(0,0,1024,768, "TP\主界面0.bmp|TP\主界面1.bmp", "000000", 0.9, 0, 1, 200) 
        TracePrint "回到主界面了"
        Call 点击所有打叉()
        Call 右侧任务栏归位()
        Exit Sub
    Else 
        TracePrint "不在主界面"
        Call 点掉对话框()
        Call 点击所有打叉()
        Goto 开始
    End If
    //        TracePrint ret2
    //        If ret or ret2 = 1 Then 
    //            TracePrint "已在主界面"
    //            call 右侧任务栏归位()
    //        ElseIf ret = 0 or ret2 = 0 Then
    //            TracePrint "不在主界面"
    //            Call   点击所有打叉()
    //            Goto 开始
    //        End If
End Sub
Sub 点掉对话框()
    dm_ret = dm.FindPic(952,606,1008,741, "TP\师门\对话框.bmp", "000000", 0.9, 0, intX, intY)
    If intx >= 0 and inty >= 0 Then 
        TracePrint "出现对话框"
        Delay 500
        dm_ret = dm.FindStr(699, 106, 814, 595, "请选择", "eee1cf-303030", 0.8, intX, intY)
        If intx >= 0 and inty >= 0 Then 
            TracePrint "没出现请选择"
        Else 
            TracePrint "没任务，点掉"
            dm.MoveTo 177, 626
            Delay 50 
            dm.LeftClick 
        End If
    Else 
        Delay 50
    End If
End Sub
Sub 点击所有打叉()
    For 10
        TracePrint 点击(360, 72, 1069, 545, "TP\关闭\活力界面.bmp|TP\关闭\回到主界面11.bmp|TP\关闭\回到主界面10.bmp|TP\关闭\回到主界面9.bmp|TP\关闭\回到主界面8.bmp|TP\关闭\回到主界面7.bmp|TP\关闭\回到主界面6.bmp|TP\关闭\回到主界面5.bmp|TP\关闭\回到主界面4.bmp|TP\关闭\回到主界面3.bmp|TP\关闭\回到主界面2.bmp|TP\关闭\回到主界面1.bmp|TP\关闭\回到主界面16.bmp|TP\关闭\回到主界面17.bmp|TP\关闭\回到主界面12.bmp|TP\关闭\回到主界面13.bmp|TP\关闭\回到主界面14.bmp|TP\关闭\回到主界面15.bmp|TP\关闭\回到主界面16.bmp|TP\关闭\回到主界面17.bmp|TP\关闭\回到主界面18.bmp|TP\关闭\回到主界面19.bmp|TP\关闭\回到主界面20.bmp|TP\关闭\三界热点.bmp|TP\关闭\确认关闭.bmp|TP\关闭\算了.bmp|TP\关闭\取消.bmp|TP\关闭\天罡星.bmp|TP\关闭\擂台大挑战.bmp|TP\关闭\科举.bmp", "000000", 0.9, 0, 1, 1000)
        If   点击(360, 72, 1069, 545, "TP\关闭\活力界面.bmp|TP\关闭\回到主界面11.bmp|TP\关闭\回到主界面10.bmp|TP\关闭\回到主界面9.bmp|TP\关闭\回到主界面8.bmp|TP\关闭\回到主界面7.bmp|TP\关闭\回到主界面6.bmp|TP\关闭\回到主界面5.bmp|TP\关闭\回到主界面4.bmp|TP\关闭\回到主界面3.bmp|TP\关闭\回到主界面2.bmp|TP\关闭\回到主界面1.bmp|TP\关闭\回到主界面16.bmp|TP\关闭\回到主界面17.bmp|TP\关闭\回到主界面12.bmp|TP\关闭\回到主界面13.bmp|TP\关闭\回到主界面14.bmp|TP\关闭\回到主界面15.bmp|TP\关闭\回到主界面16.bmp|TP\关闭\回到主界面17.bmp|TP\关闭\回到主界面18.bmp|TP\关闭\回到主界面19.bmp|TP\关闭\回到主界面20.bmp|TP\关闭\三界热点.bmp|TP\关闭\确认关闭.bmp|TP\关闭\算了.bmp|TP\关闭\取消.bmp|TP\关闭\天罡星.bmp|TP\关闭\擂台大挑战.bmp|TP\关闭\科举.bmp", "000000", 0.9, 0, 1, 100) <> 1 Then 
            TracePrint "已成功关闭所有叉"
            Exit Sub
        End If
        Delay 2000
    Next 
    //地图   1
    //地图小 1
    //活动 1
    //劳动节 1
    //初梦 6
    //成长礼 7
    //师徒 8
    //直播 9
    //指引 10
    //零食道具 11
    //消息框 12
    //强化 13
    //炼药 14
    //家园 14
    //助战 15
    //首冲16
    //师门领任务界面18
    //福利 19
    //三界奇缘 20
End Sub  
Sub 跳过对话()
    If 点击(971 - 20, 37 - 20, 1004 + 20, 71 + 20, "TP\跳过对话.bmp", "000000", 0.9, 1, 1, 100) = 1 Then 
        TracePrint "跳过对话"
        Call 调试输出("对话-跳过")
    End If
End Sub
//回到主界面 
Sub 意外点击()
    set dm = createobject("dm.dmsoft")
    梦幻窗口测试 = 梦幻窗口
    dm.SetPath "c:\test_game"
    dm_ret = dm.BindWindow(梦幻窗口测试, "gdi2", "windows3", "windows", 1)
    Delay 3000
    Do 
        Call 梦幻精灵()
        TracePrint "检测意外 "
        Delay 4000
    Loop 
End Sub
Sub 点击活动()
    Rem 开始
    call 点击(262,3,375,93, "TP\活动新.bmp", "101010", 0.9, 1,5,1000)
    If 找图判断(213, 66, 367, 145,"TP\活动界面内.bmp", "000000",  0.9, 1, 5, 1000) = 1 Then 
        TracePrint "已经在活动页面"
        Call 调试输出("打开活动界面")
        Call 活动界面归位()
    Else 
        Goto 开始
    End If
End Sub
Sub 活动界面归位()
    For 5
        If 找图判断(41, 117, 256, 239, "TP\活动界面归位.bmp", "000000", 0.9, 1, 5, 100) = 1 Then 
            Delay 速度
            dm.MoveTo 593,183
            Delay 50
            dm.LeftDown 
            Delay 50
            dm.MoveTo 594,474
            Delay 50
            dm.LeftUp 
            Delay 1500
            Exit Sub
        Else 
            TracePrint "没找到活动界面，点击一下日常再搜"
            call 点击(41, 117, 256, 239,"TP\活动界面归位1.bmp","000000",0.9,1,5,200)
        End If
    Next 
End Sub
Sub 战斗中()
    dm_ret = dm.UseDict(0)
    TracePrint "执行战斗中"
    For 50
        dm_ret = dm.FindStr(462,5,563,59, "战斗中1|战斗中2|战斗中3|战斗中4|战斗中5|战斗中6|战斗中7|战斗中8|战斗中9|战斗中0", "ffffee-777777", 0.9, intX, intY)
        If intX >= 0 and intY >= 0 Then 
            TracePrint "出现战斗中数字"
            Call 调试输出("战斗中")
            //战斗中检查自动 如果没开 点击开
            If 找图判断(945, 302, 1021, 479, "TP\阵法\法术.bmp", "000000", 0.9, 1, 1, 100)=1 Then 
                If 点击(919,637,1018,755, "TP\阵法\自动.bmp", "000000", 0.9, 1, 1, 100) = 1 Then 
                    TracePrint "点击自动"
                End If
            End If
            Call 点击所有打叉
            Delay 2000
        Else 
            Delay 200
            Exit for
        End If
    Next
    TracePrint "没出现战斗 结束"
End Sub
Sub 梦幻精灵()
    For 2
        dm_ret = dm.FindPic(12, 255  ,322, 479 ,"梦幻精灵.bmp","101010",0.9,0,intX,intY)
        If intX >= 0 and intY >= 0 Then
            TracePrint "出现梦幻精灵"
            dm.MoveTo intx, inty
            Delay 500
            dm.LeftClick 
            Delay 2000
            //使用后等待
            Exit Sub
        Else 
            TracePrint "没出现梦幻精灵"
            Delay 100
        End If
    Next 
End Sub
Sub 窗口统一()
    梦幻窗口测试=梦幻窗口
    Call Plugin.Window.Size(梦幻窗口测试, 645, 488)
End Sub
// ===========================从第一个列表框选择任务到第二个列表框
//Event Form2.ListBox1.DblClick
//    行数=Form2.ListBox1.ListIndex
//    TracePrint 行数
//    内容 = Form2.ListBox1.List
//    内容 = Split(内容, "|")
//    TracePrint 内容(行数)
//    //=========计算出列表内点击后的产物、、
//    //插入新的表格内
//    Form2.ListBox2.AddItem 内容(行数)
//    //===设置跳转
//    If Form2.跳转到任务配置.Value = 1 Then 
//        BeginThread 变化
//    End If
//End Event
Event Form1.ListBox1.DblClick
    行数=Form1.ListBox1.ListIndex
    TracePrint 行数
    内容 = Form1.ListBox1.List
    内容 = Split(内容, "|")
    TracePrint 内容(行数)
    //=========计算出列表内点击后的产物、、
    //插入新的表格内
    Form1.ListBox2.AddItem 内容(行数)
    //===设置跳转
    //    If Form1.跳转到任务配置.Value = 1 Then 
    //        BeginThread 变化
    //    End If
End Event
Sub 变化()
    Form1.TabControl1.Tab = 2
    For 4
        Form1.Container6.BorderColor="FF0000"
        Form1.Container6.TextColor = "FF0000"
        Delay 500
        Form1.Container6.BorderColor="000000"
        Form1.Container6.TextColor = "000000"
        Delay 500
    Next 
End Sub
//-------------------------------q全部清除
Event Form1.Button4.Click
    For 50
        Form1.ListBox2.RemoveItem 0
        Delay 10
    Next 
    TracePrint "清除选好的任务"
End Event
//=============================删除指定任务
Event Form1.Button5.Click
    行数 = Form1.ListBox2.ListIndex
    Form1.ListBox2.RemoveItem 行数
    TracePrint "删除指定任务成功"
End Event
//======================插入任务
Event Form1.Button6.Click
    行数=Form1.ListBox1.ListIndex
    TracePrint 行数
    内容 = Form1.ListBox1.List
    内容 = Split(内容, "|")
    TracePrint 内容(行数)
    //=========计算出列表内点击后的产物、、
    //插入新的表格内
    //算出第一个列表框内需要添加的
    Form1.ListBox2.InsertItem 内容(行数), Form1.ListBox2.ListIndex
    //插入列表2
End Event
//====================================================杂项子程序
Sub 句柄()
    Set dm = createobject("dm.dmsoft")
    //HwndEx = Plugin.Window.SearchEx("LDPlayerMainFrame","",1)
    HwndEx = dm.EnumWindow(0,"TheRender","LDPlayerMainFrame",2+4+32)
    TracePrint HwndEx 
    Dim MyArray
    MyArray = Split(HwndEx, ",")
    窗口数量 = UBound(MyArray)
    窗口数量=    窗口数量+1
    TracePrint "窗口数量="&窗口数量
    If UBound(MyArray)>=0 Then  
        i=0   
        For UBound(MyArray)+1  
            //下面这句将字符串转换成数值   
            TracePrint "第 " & i + 1 & " 个窗口句柄为：" & Clng(MyArray(i))
            TracePrint "i="&i
            If i = 0 Then 
                TracePrint "i=0这里I="&i
                梦幻窗口1外 = MyArray(i)
                梦幻窗口1内 = Plugin.Window.FindEx(梦幻窗口1外, 0, "RenderWindow", "TheRender")
                Form1.Grid1.SetItemText 1, 1, 梦幻窗口1内 & "\" & Plugin.Window.GetText(梦幻窗口1外)//输入到表格中 
            End If
            If i = 1 Then 
                TracePrint "i=1这里I="&i
                梦幻窗口2外 = MyArray(i)
                梦幻窗口2内 = Plugin.Window.FindEx(梦幻窗口2外, 0, "RenderWindow", "TheRender")
                Form1.Grid1.SetItemText 2,1,梦幻窗口2内& "\"&Plugin.Window.GetText(梦幻窗口2外) //输入到表格中    
            End If
            If i = 2 Then 
                梦幻窗口3外 = MyArray(i)
                梦幻窗口3内 = Plugin.Window.FindEx(梦幻窗口3外, 0, "RenderWindow", "TheRender")
                Form1.Grid1.SetItemText 3,1,梦幻窗口3内& "\"&Plugin.Window.GetText(梦幻窗口3外)    //输入到表格中 
            End If
            If i = 3 Then 
                梦幻窗口4外 = MyArray(i)
                梦幻窗口4内 = Plugin.Window.FindEx(梦幻窗口4外, 0, "RenderWindow", "TheRender")
                Form1.Grid1.SetItemText 4,1,梦幻窗口4内& "\"&Plugin.Window.GetText(梦幻窗口4外)    //输入到表格中 
            End If
            If i = 4 Then 
                梦幻窗口5外 = MyArray(i)
                梦幻窗口5内 = Plugin.Window.FindEx(梦幻窗口5外, 0, "RenderWindow", "TheRender")
                Form1.Grid1.SetItemText 5,1,梦幻窗口5内& "\"&Plugin.Window.GetText(梦幻窗口5外)    //输入到表格中 
            End If
            If i = 5 Then 
                梦幻窗口6外 = MyArray(i)
                梦幻窗口6内 = Plugin.Window.FindEx(梦幻窗口6外, 0, "RenderWindow", "TheRender")
                Form1.Grid1.SetItemText 6,1,梦幻窗口6内& "\"&Plugin.Window.GetText(梦幻窗口6外)    //输入到表格中 
            End If
            If i = 6 Then 
                梦幻窗口7外 = MyArray(i)
                梦幻窗口7内 = Plugin.Window.FindEx(梦幻窗口7外, 0, "RenderWindow", "TheRender")
                Form1.Grid1.SetItemText 7,1,梦幻窗口7内 & "\"&Plugin.Window.GetText(梦幻窗口7外)   //输入到表格中 
            End If
            If i = 7 Then 
                梦幻窗口8外 = MyArray(i)
                梦幻窗口8内 = Plugin.Window.FindEx(梦幻窗口8外, 0, "RenderWindow", "TheRender")
                Form1.Grid1.SetItemText 8,1,梦幻窗口8内& "\"&Plugin.Window.GetText(梦幻窗口8外)    //输入到表格中 
            End If
            If i = 8 Then 
                梦幻窗口9外 = MyArray(i)
                梦幻窗口9内 = Plugin.Window.FindEx(梦幻窗口9外, 0, "RenderWindow", "TheRender")
                Form1.Grid1.SetItemText 9,1,梦幻窗口9内& "\"&Plugin.Window.GetText(梦幻窗口9外)    //输入到表格中 
            End If
            If i = 9 Then 
                梦幻窗口10外 = MyArray(i)
                梦幻窗口10内 = Plugin.Window.FindEx(梦幻窗口10外, 0, "RenderWindow", "TheRender")
                Form1.Grid1.SetItemText 10,1,梦幻窗口10内& "\"&Plugin.Window.GetText(梦幻窗口10外)    //输入到表格中 
            End If
            If i = 10 Then 
                梦幻窗口11外 = MyArray(i)
                梦幻窗口11内 = Plugin.Window.FindEx(梦幻窗口11外, 0, "RenderWindow", "TheRender")
                Form1.Grid1.SetItemText 11,1,梦幻窗口11内& "\"&Plugin.Window.GetText(梦幻窗口11外)    //输入到表格中 
            End If
            If i = 11 Then 
                梦幻窗口12外 = MyArray(i)
                梦幻窗口12内 = Plugin.Window.FindEx(梦幻窗口12外, 0, "RenderWindow", "TheRender")
                Form1.Grid1.SetItemText 12,1,梦幻窗口12内 & "\"&Plugin.Window.GetText(梦幻窗口12外)   //输入到表格中 
            End If
            If i = 12 Then 
                梦幻窗口13外 = MyArray(i)
                梦幻窗口13内 = Plugin.Window.FindEx(梦幻窗口13外, 0, "RenderWindow", "TheRender")
                Form1.Grid1.SetItemText 13,1,梦幻窗口13内& "\"&Plugin.Window.GetText(梦幻窗口13外)    //输入到表格中 
            End If
            If i = 13 Then 
                梦幻窗口14外 = MyArray(i)
                梦幻窗口14内 = Plugin.Window.FindEx(梦幻窗口14外, 0, "RenderWindow", "TheRender")
                Form1.Grid1.SetItemText 14,1,梦幻窗口14内& "\"&Plugin.Window.GetText(梦幻窗口4外)    //输入到表格中 
            End If
            If i = 14 Then 
                梦幻窗口15外 = MyArray(i)
                梦幻窗口15内 = Plugin.Window.FindEx(梦幻窗口15外, 0, "RenderWindow", "TheRender")
                Form1.Grid1.SetItemText 15,1,梦幻窗口15内& "\"&Plugin.Window.GetText(梦幻窗口15外)    //输入到表格中 
            End If
            i = i + 1
        Next  
    End If
End Sub
//Function 点击(左X, 左Y, 右X, 右Y, 图片名, 颜色, 相似度, 方向, 返回X, 返回Y,次数)
//    For 次数
//        dm_ret = dm.FindPic(左X, 左Y, 右X, 右Y, 图片名, 颜色, 相似度, 方向, 返回X, 返回Y)
//        If 返回X >= - 1  and 返回Y >= - 1  Then 
//            TracePrint "点击图片" & 图片名
//            dm.MoveTo 返回X,返回Y
//            dm.LeftClick 
//            Exit Function
//        Else 
//            TracePrint "没找到" & 图片名
//            Delay 1000
//        End If
//    End Function
//    
Sub 启动前运行()
End Sub
Function 调试输出当前任务(日志内容)
    调试输出ID = GetThreadID()
    If 调试输出ID = 线程ID1 Then 
        Form1.Grid1.SetItemText 1, 6, 日志内容
    End If
    If 调试输出ID = 线程id2 Then 
        Form1.Grid1.SetItemText 2, 6, 日志内容
    End If
    If 调试输出ID = 线程id3 Then 
        Form1.Grid1.SetItemText 3, 6, 日志内容
    End If
    If 调试输出ID = 线程id4 Then 
        Form1.Grid1.SetItemText 4, 6, 日志内容
    End If
    If 调试输出ID = 线程id5 Then 
        Form1.Grid1.SetItemText 5, 6, 日志内容
    End If
    If 调试输出ID = 线程id6 Then 
        Form1.Grid1.SetItemText 6, 6, 日志内容
    End If
    If 调试输出ID = 线程id7 Then 
        Form1.Grid1.SetItemText 7, 6, 日志内容
    End If
    If 调试输出ID = 线程id8 Then 
        Form1.Grid1.SetItemText 8, 6, 日志内容
    End If
    If 调试输出ID = 线程id9 Then 
        Form1.Grid1.SetItemText 9, 6, 日志内容
    End If
    If 调试输出ID = 线程id10 Then 
        Form1.Grid1.SetItemText 10, 6, 日志内容
    End If
    If 调试输出ID = 线程id11 Then 
        Form1.Grid1.SetItemText 11, 6, 日志内容
    End If
    If 调试输出ID = 线程id12 Then 
        Form1.Grid1.SetItemText 12, 6, 日志内容
    End If
    If 调试输出ID = 线程id13 Then 
        Form1.Grid1.SetItemText 13, 6, 日志内容
    End If
    If 调试输出ID = 线程id14 Then 
        Form1.Grid1.SetItemText 14, 6, 日志内容
    End If
    If 调试输出ID = 线程id15 Then 
        Form1.Grid1.SetItemText 15, 6, 日志内容
    End If
End Function
Function 调试输出(日志内容)
    调试输出ID = GetThreadID()
    If 调试输出ID = 线程ID1 Then 
        Form1.Grid1.SetItemText 1, 7, 日志内容
    End If
    If 调试输出ID = 线程id2 Then 
        Form1.Grid1.SetItemText 2, 7, 日志内容
    End If
    If 调试输出ID = 线程id3 Then 
        Form1.Grid1.SetItemText 3, 7, 日志内容
    End If
    If 调试输出ID = 线程id4 Then 
        Form1.Grid1.SetItemText 4, 7, 日志内容
    End If
    If 调试输出ID = 线程id5 Then 
        Form1.Grid1.SetItemText 5, 7, 日志内容
    End If
    If 调试输出ID = 线程id6 Then 
        Form1.Grid1.SetItemText 6, 7, 日志内容
    End If
    If 调试输出ID = 线程id7 Then 
        Form1.Grid1.SetItemText 7, 7, 日志内容
    End If
    If 调试输出ID = 线程id8 Then 
        Form1.Grid1.SetItemText 8, 7, 日志内容
    End If
    If 调试输出ID = 线程id9 Then 
        Form1.Grid1.SetItemText 9, 7, 日志内容
    End If
    If 调试输出ID = 线程id10 Then 
        Form1.Grid1.SetItemText 10, 7, 日志内容
    End If
    If 调试输出ID = 线程id11 Then 
        Form1.Grid1.SetItemText 11, 7, 日志内容
    End If
    If 调试输出ID = 线程id12 Then 
        Form1.Grid1.SetItemText 12, 7, 日志内容
    End If
    If 调试输出ID = 线程id13 Then 
        Form1.Grid1.SetItemText 13, 7, 日志内容
    End If
    If 调试输出ID = 线程id14 Then 
        Form1.Grid1.SetItemText 14, 7, 日志内容
    End If
    If 调试输出ID = 线程id15 Then 
        Form1.Grid1.SetItemText 15, 7, 日志内容
    End If
End Function
Function 调试输出金币(日志内容)
    调试输出ID = GetThreadID()
    If 调试输出ID = 线程ID1 Then 
        Form1.Grid1.SetItemText 1, 5, 日志内容
    End If
    If 调试输出ID = 线程id2 Then 
        Form1.Grid1.SetItemText 2, 5, 日志内容
    End If
    If 调试输出ID = 线程id3 Then 
        Form1.Grid1.SetItemText 3, 5, 日志内容
    End If
    If 调试输出ID = 线程id4 Then 
        Form1.Grid1.SetItemText 4, 5, 日志内容
    End If
    If 调试输出ID = 线程id5 Then 
        Form1.Grid1.SetItemText 5, 5, 日志内容
    End If
    If 调试输出ID = 线程id6 Then 
        Form1.Grid1.SetItemText 6, 5, 日志内容
    End If
    If 调试输出ID = 线程id7 Then 
        Form1.Grid1.SetItemText 7, 5, 日志内容
    End If
    If 调试输出ID = 线程id8 Then 
        Form1.Grid1.SetItemText 8, 5, 日志内容
    End If
    If 调试输出ID = 线程id9 Then 
        Form1.Grid1.SetItemText 9, 5, 日志内容
    End If
    If 调试输出ID = 线程id10 Then 
        Form1.Grid1.SetItemText 10, 5, 日志内容
    End If
    If 调试输出ID = 线程id11 Then 
        Form1.Grid1.SetItemText 11, 5, 日志内容
    End If
    If 调试输出ID = 线程id12 Then 
        Form1.Grid1.SetItemText 12, 5, 日志内容
    End If
    If 调试输出ID = 线程id13 Then 
        Form1.Grid1.SetItemText 13, 5, 日志内容
    End If
    If 调试输出ID = 线程id14 Then 
        Form1.Grid1.SetItemText 14, 5, 日志内容
    End If
    If 调试输出ID = 线程id15 Then 
        Form1.Grid1.SetItemText 15, 5, 日志内容
    End If
End Function
sub 获取该线程启动的窗口句柄()
    调试输出ID = GetThreadID()
    TracePrint " 调试输出ID=" & 调试输出ID
    If 调试输出ID = 线程ID1 Then 
        当前运行窗口 = 梦幻窗口1内
        TracePrint "梦幻窗口1内=" & 梦幻窗口1内
        TracePrint"当前运行窗口="&当前运行窗口
    End If
    If 调试输出ID = 线程id2 Then 
        当前运行窗口=梦幻窗口2内 
    End If
    If 调试输出ID = 线程id3 Then 
        当前运行窗口=梦幻窗口3内
    End If
    If 调试输出ID = 线程id4 Then 
        当前运行窗口=梦幻窗口4内
    End If
    If 调试输出ID = 线程id5 Then 
        当前运行窗口=梦幻窗口5内
    End If
    If 调试输出ID = 线程id6 Then 
        当前运行窗口=梦幻窗口6内
    End If
    If 调试输出ID = 线程id7 Then 
        当前运行窗口=梦幻窗口7内
    End If
    If 调试输出ID = 线程id8 Then 
        当前运行窗口=梦幻窗口8内
    End If
    If 调试输出ID = 线程id9 Then 
        当前运行窗口=梦幻窗口9内
    End If
    If 调试输出ID = 线程id10 Then 
        当前运行窗口=梦幻窗口10内
    End If
    If 调试输出ID = 线程id11 Then 
        当前运行窗口=梦幻窗口11内
    End If
    If 调试输出ID = 线程id12 Then 
        当前运行窗口=梦幻窗口12内
    End If
    If 调试输出ID = 线程id13 Then 
        当前运行窗口=梦幻窗口13内
    End If
    If 调试输出ID = 线程id14 Then 
        当前运行窗口=梦幻窗口14内
    End If
    If 调试输出ID = 线程id15 Then 
        当前运行窗口 = 梦幻窗口15内
    End if
End sub
Sub 大漠注册()
    // 首先打包dm.dll和RegDll.dll到附件,当然如果你还有其它资源(字库，图片等)也可以一并打包
    // 这个need_ver作为本脚本需要使用的插件版本. 如果要换插件时，记得更改这个值.
    need_ver = "7.2213"
    // 插件需要用到atl系统库,有些XP精简系统会把atl.dll精简掉. 为了防止注册失败，这里手动注册一下atl.dll
    set ws=createobject("Wscript.Shell")
    ws.run "regsvr32 atl.dll /s"
    set ws=nothing
    // 释放附件>>>>>>>>>>>>>>>>>>>
    // 这里选择c盘的test-_game作为插件的基本目录 也就是SetPath对应的目录。所以所有资源都释放在此目录.
    PutAttachment "c:\test_game","*.*"
    // 这里要用到RegDll来注册插件，所以必须释放到Plugin. 但是切记不能把dm.dll释放到Plugin.那会导致插件失效.
    PutAttachment ".\Plugin" ,"RegDll.dll"
    // 插件注册开始>>>>>>>>>>>>>>>>>>> 
    // 下面开始注册插件,先尝试用RegDll来注册.这里必须使用绝对路径。以免有别人把dm.dll释放在系统目录.造成版本错误.
    Call Plugin.RegDll.Reg("dm.dll") 
    // 这里判断是否注册成功
    set dm = createobject("dm.dmsoft")
    ver = dm.Ver()
    if ver <> need_ver then
        // 先释放先前创建的dm
        set dm = nothing
        // 再尝试用regsvr32 来注册. 这里必须使用绝对路径。以免有别人把dm.dll释放在系统目录.造成版本错误.
        set ws=createobject("Wscript.Shell")
        ws.run "regsvr32 c:\test_game\dm.dll /s"
        set ws=nothing
        Delay 1500  
        // 再判断插件是否注册成功
        set dm = createobject("dm.dmsoft")
        ver = dm.Ver()
        if ver <> need_ver then
            // 这时，已经确认插件注册失败了。 弹出一些调试信息，以供分析.
            messagebox "插件版本错误,当前使用的版本是:"&ver&",插件所在目录是:"&dm.GetBasePath()
            messagebox "请关闭程序,重新打开本程序再尝试"
            endscript
        end if
    end if
    // 插件注册结束<<<<<<<<<<<<<<<
    // 收费注册开始,简单游作者也必须要加这一段. 不会重复扣费.
    // ok,这里已经确认插件注册成功，并且创建了对象,下面开始注册收费服务.
    // 当然这里也可以使用高级的RegEx函数.
    dm_ret = dm.Reg("93130883203bc792528fc2ce02f041e701c1a35f4","0001") // abcdefg是您的注册码. 在大漠插件网站后台可以直接获取.
    if dm_ret <> 1 then
        messagebox "注册失败,返回值是:"&dm_ret
        endscript
    end if
    // 收费注册结束<<<<<<<<<<<<<<<<
    //这里设置插件基本目录
    dm.SetPath "c:\test_game"
    // 获取句柄
    hwnd = dm.GetMousePointWindow()
    // 绑定
    dm_ret = dm.BindWindow(hwnd,"dx","dx","dx",0)
    // 检测绑定返回值
    if dm_ret = 0 then
        last_error = dm.GetLastError()
        // 如果是WIN7 WIN8 VISTA WIN2008系统,检测当前系统是否有开启UAC
        if dm.GetOsType() = 3 or dm.GetOsType() = 4 or dm.GetOsType() = 5 then
            // 有开启UAC的话，尝试关闭
            if dm.CheckUAC() = 1 then
                if dm.SetUAC(0) = 1 then
                    // 关闭UAC之后，必须重启系统才可以生效
                    messagebox "已经关闭系统UAC设置，必须重启系统才可以生效。点击确定重启系统"
                    dm.ExitOs 2
                    Delay 2000
                    endscript
                end if
            end if
        end if
        // 具体错误码的含义，可以参考函数GetLastError的说明.
        messagebox "绑定失败，错误码是:"&last_error
        messagebox "如果确定关闭了UAC,很可能是系统的防火墙拦截插件，请暂时关闭360等安全防火墙再尝试"
        endscript
    end if
End Sub
Function 点击(左X, 左Y, 右X, 右Y, 图片名, 颜色, 相似度, 方向,次数,成功点击后等待多少秒)
    For 次数
        dm_ret = dm.FindPic(左X, 左Y, 右X, 右Y, 图片名, 颜色, 相似度,方向, intx, inty)
        If intX >= 0 and intY >= 0 Then 
            点击=1
            TracePrint"点击"&图片名
            dm.MoveTo  intx,inty
            TracePrint intx & "," & inty
            Delay 100
            dm.LeftClick 
            Delay 成功点击后等待多少秒
            Exit Function
        Else 
            TracePrint "没找到" & 图片名
            Delay 1000
        End If
    Next 
End Function
Function 范围点击(X1,Y1,X2,Y2  )
    Randomize
    x = clng((x2 - x1) * rnd + x1)
    y = clng((y2 - y1) * rnd + y1)
    MoveTo x, y
    LeftClick 1
End Function
Function 找图判断(左X, 左Y, 右X, 右Y, 图片名, 颜色, 相似度, 方向,次数,成功点击后等待多少秒)
    For 次数
        dm_ret = dm.FindPic(左X, 左Y, 右X, 右Y, 图片名, 颜色, 相似度,方向, intx, inty)
        If intX >= 0 and intY >= 0 Then 
            找图判断=1
            TracePrint"找到"&图片名
            TracePrint intx&","&inty
            Delay 成功点击后等待多少秒
            Exit Function
        Else 
            找图判断=0
            TracePrint "没找到" & 图片名
            Delay 1000
        End If
    Next 
End Function
//================================================================================================= 点击按钮
Event Form1.Button1.Click//------------------------------开始1
    Set dm = createobject("dm.dmsoft")
    线程ID1 = BeginThread(按顺序执行)
    Delay 10
    TracePrint 线程ID1
    Form1.Button1.Visible=0
    运行窗口 = 梦幻窗口1内
    TracePrint "第一行运行窗口句柄=" & 运行窗口
    dm_ret = dm.SetWindowState(梦幻窗口1外, 1)
    Delay 100
    dm_ret = dm.SetWindowSize(梦幻窗口1外, 1066, 804)
    Delay 10
    TracePrint "窗口统一完成"
    TracePrint "点击的时候的运行窗口="&运行窗口
    //线程卡ID1 = BeginThread(卡1)
    TracePrint 线程卡ID1
End Event
Event Form1.结束1.Click//结束1
    StopThread 线程ID1
    StopThread 线程卡ID1
    Form1.Button1.Visible = 1
    Set dm = createobject("dm.dmsoft")
    dm_ret = dm.ForceUnBindWindow(梦幻窗口1内)
    TracePrint dm_ret
End Event
Event Form1.暂停1.Click//暂停1
    Form1.Button1.Visible = 0
    Form1.结束1.Visible=1
    Form1.暂停1.Visible=0
    PauseThread 线程ID1
    PauseThread 线程卡ID1
End Event
Event Form1.继续1.Click//继续1
    ContinueThread 线程ID1
    ContinueThread 线程卡ID1
    Form1.结束1.Visible = 1
    Form1.暂停1.Visible = 1
    Form1.继续1.Visible = 0
End Event
//---------------------------------------------开始2
Event Form1.Button2.Click
    Set dm = createobject("dm.dmsoft")
    线程ID2 = BeginThread(按顺序执行)
    TracePrint 线程ID2
    Delay 10
    Form1.Button2.Visible=0
    运行窗口 = 梦幻窗口2内
    TracePrint "第二行运行窗口句柄="&运行窗口
    dm_ret = dm.SetWindowState(梦幻窗口2外, 1)
    Delay 100
    dm_ret = dm.SetWindowSize(梦幻窗口2外, 1066, 804)
    TracePrint "窗口统一完成"
    //线程卡ID2 = BeginThread(卡2)
    //TracePrint 检测脚本卡住()
    TracePrint 线程卡ID2
End Event
Event Form1.结束2.Click//结束2
    StopThread 线程ID2
    StopThread 线程卡ID2
    Form1.Button2.Visible = 1
    dm_ret = dm.ForceUnBindWindow(梦幻窗口2内)
    TracePrint dm_ret
End Event
Event Form1.暂停2.Click//暂停2
    Form1.Button2.Visible = 0
    Form1.结束2.Visible=1
    Form1.暂停2.Visible=0
    PauseThread 线程ID2
    PauseThread 线程卡ID2
End Event
Event Form1.继续2.Click//继续2
    ContinueThread 线程ID2
    ContinueThread 线程卡ID2
    Form1.Button2.Visible = 1
    Form1.暂停2.Visible = 1
    Form1.继续2.Visible = 0
End Event
Event Form1.开始3.Click//---------------------------------------开始3
    Set dm = createobject("dm.dmsoft")
    Form1.开始3.Visible=0
    运行窗口 = 梦幻窗口3内
    TracePrint "第三行运行窗口句柄="&运行窗口
    dm_ret = dm.SetWindowState(梦幻窗口3外, 1)
    Delay 100
    dm_ret = dm.SetWindowSize(梦幻窗口3外, 1066, 804)
    TracePrint "窗口统一完成"
    日志1=1 
    线程ID3 = BeginThread(按顺序执行)
    TracePrint 线程ID3
    Delay 10
    //线程卡ID3 = BeginThread(卡3)
    //TracePrint 检测脚本卡住()
    TracePrint 线程卡ID3
End Event
Event Form1.结束3.Click//结束1
    StopThread 线程ID3
    StopThread 线程卡ID3
    Form1.开始3.Visible = 1
    dm_ret = dm.ForceUnBindWindow(梦幻窗口3内)
    TracePrint dm_ret
End Event
Event Form1.暂停3.Click//暂停1
    Form1.开始3.Visible = 0
    Form1.结束3.Visible=1
    Form1.暂停3.Visible=0
    PauseThread 线程ID3
    PauseThread 线程卡ID3
End Event
Event Form1.继续3.Click//继续1
    ContinueThread 线程ID3
    ContinueThread 线程卡ID3
    Form1.开始3.Visible = 1
    Form1.暂停3.Visible = 1
    Form1.继续1.Visible = 0
End Event
Event Form1.开始4.Click//---------------------------------------开始4
    Set dm = createobject("dm.dmsoft")
    Form1.开始4.Visible=0
    运行窗口 = 梦幻窗口4内
    dm_ret = dm.SetWindowState(梦幻窗口4外, 1)
    Delay 100
    dm_ret = dm.SetWindowSize(梦幻窗口4外, 1066, 804)
    //
    TracePrint "窗口统一完成"
    日志1=1 
    线程ID4 = BeginThread(按顺序执行)
    TracePrint 线程ID4
    Delay 10
    //线程卡ID4 = BeginThread(卡4)
    //TracePrint 检测脚本卡住()
    TracePrint 线程卡ID4
End Event
Event Form1.结束4.Click//结束4
    StopThread 线程ID4
    StopThread 线程卡ID4
    Form1.开始4.Visible = 1
    dm_ret = dm.ForceUnBindWindow(梦幻窗口4内)
    TracePrint dm_ret
End Event
Event Form1.暂停4.Click//暂停4
    Form1.开始4.Visible = 0
    Form1.结束4.Visible=1
    Form1.暂停4.Visible=0
    PauseThread 线程ID4
    PauseThread 线程卡ID4
End Event
Event Form1.继续4.Click//继续4
    ContinueThread 线程ID4
    ContinueThread 线程卡ID4
    Form1.开始4.Visible = 1
    Form1.暂停4.Visible = 1
    Form1.继续4.Visible = 0
End Event
Event Form1.开始5.Click//---------------------------------------开始5
    Set dm = createobject("dm.dmsoft")
    Form1.开始5.Visible=0
    运行窗口 = 梦幻窗口5内
    dm_ret = dm.SetWindowState(梦幻窗口5外, 1)
    Delay 100
    dm_ret = dm.SetWindowSize(梦幻窗口5外, 1066, 804)
    //
    TracePrint "窗口统一完成"
    日志1=1 
    线程ID5 = BeginThread(按顺序执行)
    TracePrint 线程ID5
    Delay 10
    //线程卡ID5 = BeginThread(卡5)
    //TracePrint 检测脚本卡住()
    TracePrint 线程卡ID5
End Event
Event Form1.结束5.Click//结束5
    StopThread 线程ID5
    StopThread 线程卡ID5
    Form1.开始5.Visible = 1
    dm_ret = dm.ForceUnBindWindow(梦幻窗口5内)
    TracePrint dm_ret
End Event
Event Form1.暂停5.Click//暂停5
    Form1.开始5.Visible = 0
    Form1.结束5.Visible=1
    Form1.暂停5.Visible=0
    PauseThread 线程ID5
    PauseThread 线程卡ID5
End Event
Event Form1.继续5.Click//继续5
    ContinueThread 线程ID5
    ContinueThread 线程卡ID5
    Form1.开始5.Visible = 1
    Form1.暂停5.Visible = 1
    Form1.继续5.Visible = 0
End Event
Event Form1.开始6.Click//---------------------------------------开始6
    Set dm = createobject("dm.dmsoft")
    Form1.开始6.Visible=0
    运行窗口 = 梦幻窗口6内
    dm_ret = dm.SetWindowState(梦幻窗口6外, 1)
    Delay 100
    dm_ret = dm.SetWindowSize(梦幻窗口6外, 1066, 804)
    //
    TracePrint "窗口统一完成"
    日志1=1 
    线程ID6 = BeginThread(按顺序执行)
    TracePrint 线程ID6
    Delay 10
    //线程卡ID6 = BeginThread(卡6)
    //TracePrint 检测脚本卡住()
    TracePrint 线程卡ID6
End Event
Event Form1.结束6.Click//结束6
    StopThread 线程ID6
    StopThread 线程卡ID6
    Form1.开始6.Visible = 1
    dm_ret = dm.ForceUnBindWindow(梦幻窗口6内)
    TracePrint dm_ret
End Event
Event Form1.暂停6.Click//暂停6
    Form1.开始6.Visible = 0
    Form1.结束6.Visible=1
    Form1.暂停6.Visible=0
    PauseThread 线程ID6
    PauseThread 线程卡ID6
End Event
Event Form1.继续6.Click//继续6
    ContinueThread 线程ID6
    ContinueThread 线程卡ID6
    Form1.开始6.Visible = 1
    Form1.暂停6.Visible = 1
    Form1.继续6.Visible = 0
End Event
Event Form1.开始7.Click//---------------------------------------开始7
    Set dm = createobject("dm.dmsoft")
    Form1.开始7.Visible=0
    运行窗口 = 梦幻窗口7内
    dm_ret = dm.SetWindowState(梦幻窗口7外, 1)
    Delay 100
    dm_ret = dm.SetWindowSize(梦幻窗口7外, 1066, 804)
    //
    TracePrint "窗口统一完成"
    日志1=1 
    线程ID7= BeginThread(按顺序执行)
    TracePrint 线程ID7
    Delay 10
    //线程卡ID7 = BeginThread(卡7)
    //TracePrint 检测脚本卡住()
    TracePrint "线程ID卡7="&线程卡ID7
End Event
Event Form1.结束7.Click //结束7
    TracePrint "7结束"
    StopThread 线程ID7
    StopThread 线程卡ID7
    TracePrint "结束线程ID卡7="&线程卡ID7
    Form1.开始7.Visible = 1
    dm_ret = dm.ForceUnBindWindow(梦幻窗口7内)
    TracePrint dm_ret
End Event
Event Form1.暂停7.Click//暂停7
    Form1.开始7.Visible = 0
    Form1.结束7.Visible=1
    Form1.暂停7.Visible=0
    PauseThread 线程ID7
    PauseThread 线程卡ID7
End Event
Event Form1.继续7.Click//继续7
    ContinueThread 线程ID7
    ContinueThread 线程卡ID7
    Form1.开始7.Visible = 1
    Form1.暂停7.Visible = 1
    Form1.继续7.Visible = 0
End Event
Event Form1.开始8.Click//---------------------------------------开始8
    Set dm = createobject("dm.dmsoft")
    Form1.开始8.Visible=0
    运行窗口 = 梦幻窗口8内
    dm_ret = dm.SetWindowState(梦幻窗口8外, 1)
    Delay 100
    dm_ret = dm.SetWindowSize(梦幻窗口8外, 1066, 804)
    TracePrint "窗口统一完成"
    日志1=1 
    线程ID8= BeginThread(按顺序执行)
    TracePrint 线程ID8
    Delay 10
    //线程卡ID8 = BeginThread(卡8)
    //TracePrint 检测脚本卡住()
    TracePrint "线程ID卡8="&线程卡ID8
End Event
Event Form1.结束8.Click //结束8
    TracePrint "8结束"
    StopThread 线程ID8
    StopThread 线程卡ID8
    TracePrint "结束线程ID卡8="&线程卡ID8
    Form1.开始8.Visible = 1
    dm_ret = dm.ForceUnBindWindow(梦幻窗口8内)
    TracePrint dm_ret
End Event
Event Form1.暂停8.Click//暂停8
    Form1.开始8.Visible = 0
    Form1.结束8.Visible=1
    Form1.暂停8.Visible=0
    PauseThread 线程ID8
    PauseThread 线程卡ID8
End Event
Event Form1.继续8.Click//继续8
    ContinueThread 线程ID8
    ContinueThread 线程卡ID8
    Form1.开始8.Visible = 1
    Form1.暂停8.Visible = 1
    Form1.继续8.Visible = 0
End Event
Event Form1.开始9.Click//---------------------------------------开始9
    Set dm = createobject("dm.dmsoft")
    Form1.开始9.Visible=0
    运行窗口 = 梦幻窗口9内
    dm_ret = dm.SetWindowState(梦幻窗口9外, 1)
    Delay 100
    dm_ret = dm.SetWindowSize(梦幻窗口9外, 1066, 804)
    TracePrint "窗口统一完成"
    日志1=1 
    线程ID9= BeginThread(按顺序执行)
    TracePrint 线程ID9
    Delay 10
    //线程卡ID9 = BeginThread(卡9)
    //TracePrint 检测脚本卡住()
    TracePrint "线程ID卡9="&线程卡ID9
End Event
Event Form1.结束9.Click //结束9
    TracePrint "9结束"
    StopThread 线程ID9
    StopThread 线程卡ID9
    TracePrint "结束线程ID卡9="&线程卡ID9
    Form1.开始9.Visible = 1
    dm_ret = dm.ForceUnBindWindow(梦幻窗口9内)
    TracePrint dm_ret
End Event
Event Form1.暂停9.Click//暂停9
    Form1.开始9.Visible = 0
    Form1.结束9.Visible=1
    Form1.暂停9.Visible=0
    PauseThread 线程ID9
    PauseThread 线程卡ID9
End Event
Event Form1.继续9.Click//继续9
    ContinueThread 线程ID9
    ContinueThread 线程卡ID9
    Form1.开始9.Visible = 1
    Form1.暂停9.Visible = 1
    Form1.继续9.Visible = 0
End Event
Event Form1.开始10.Click//---------------------------------------开始10
    Set dm = createobject("dm.dmsoft")
    Form1.开始10.Visible=0
    运行窗口 = 梦幻窗口10内
    dm_ret = dm.SetWindowState(梦幻窗口10外, 1)
    Delay 100
    dm_ret = dm.SetWindowSize(梦幻窗口10外, 1066, 804)
    TracePrint "窗口统一完成"
    日志1=1 
    线程ID10= BeginThread(按顺序执行)
    TracePrint 线程ID10
    Delay 10
    //线程卡ID10 = BeginThread(卡10)
    //TracePrint 检测脚本卡住()
    TracePrint "线程ID卡10="&线程卡ID10
End Event
Event Form1.结束10.Click //结束10
    TracePrint "10结束"
    StopThread 线程ID10
    StopThread 线程卡ID10
    TracePrint "结束线程ID卡10="&线程卡ID10
    Form1.开始10.Visible = 1
    dm_ret = dm.ForceUnBindWindow(梦幻窗口10内)
    TracePrint dm_ret
End Event
Event Form1.暂停10.Click//暂停10
    Form1.开始10.Visible = 0
    Form1.结束10.Visible=1
    Form1.暂停10.Visible=0
    PauseThread 线程ID10
    PauseThread 线程卡ID10
End Event
Event Form1.继续10.Click//继续10
    ContinueThread 线程ID10
    ContinueThread 线程卡ID10
    Form1.开始10.Visible = 1
    Form1.暂停10.Visible = 1
    Form1.继续10.Visible = 0
End Event
Event Form1.开始11.Click//---------------------------------------开始11
    Set dm = createobject("dm.dmsoft")
    Form1.开始11.Visible=0
    运行窗口 = 梦幻窗口11内
    dm_ret = dm.SetWindowState(梦幻窗口11外, 1)
    Delay 100
    dm_ret = dm.SetWindowSize(梦幻窗口11外, 1066, 804)
    TracePrint "窗口统一完成"
    日志1=1 
    线程ID11= BeginThread(按顺序执行)
    TracePrint 线程ID11
    Delay 10
    //线程卡ID11 = BeginThread(卡11)
    //TracePrint 检测脚本卡住()
    TracePrint "线程ID卡11="&线程卡ID11
End Event
Event Form1.结束11.Click //结束11
    TracePrint "11结束"
    StopThread 线程ID11
    StopThread 线程卡ID11
    TracePrint "结束线程ID卡11="&线程卡ID11
    Form1.开始11.Visible = 1
    dm_ret = dm.ForceUnBindWindow(梦幻窗口11内)
    TracePrint dm_ret
End Event
Event Form1.暂停11.Click//暂停11
    Form1.开始11.Visible = 0
    Form1.结束11.Visible=1
    Form1.暂停11.Visible=0
    ContinueThread 线程ID11
    ContinueThread 线程卡ID11
End Event
Event Form1.继续11.Click//继续11
    PauseThread 线程ID11
    PauseThread 线程卡ID11
    Form1.开始11.Visible = 1
    Form1.暂停11.Visible = 1
    Form1.继续11.Visible = 0
End Event
Event Form1.开始12.Click//---------------------------------------开始12
    Set dm = createobject("dm.dmsoft")
    Form1.开始12.Visible=0
    运行窗口 = 梦幻窗口12内
    dm_ret = dm.SetWindowState(梦幻窗口12外, 1)
    Delay 100
    dm_ret = dm.SetWindowSize(梦幻窗口12外, 1066, 804)
    TracePrint "窗口统一完成"
    日志1=1 
    线程ID12= BeginThread(按顺序执行)
    TracePrint 线程ID12
    Delay 10
    //线程卡ID12 = BeginThread(卡12)
    //TracePrint 检测脚本卡住()
    TracePrint "线程ID卡12="&线程卡ID12
End Event
Event Form1.结束12.Click //结束12
    TracePrint "12结束"
    StopThread 线程ID12
    StopThread 线程卡ID12
    TracePrint "结束线程ID卡12="&线程卡ID12
    Form1.开始12.Visible = 1
    dm_ret = dm.ForceUnBindWindow(梦幻窗口12内)
    TracePrint dm_ret
End Event
Event Form1.暂停12.Click//暂停12
    Form1.开始12.Visible = 0
    Form1.结束12.Visible=1
    Form1.暂停12.Visible=0
    PauseThread 线程ID12
    PauseThread 线程卡ID12
End Event
Event Form1.继续12.Click//继续12
    ContinueThread 线程ID12
    ContinueThread 线程卡ID12
    Form1.开始12.Visible = 1
    Form1.暂停12.Visible = 1
    Form1.继续12.Visible = 0
End Event
Event Form1.开始13.Click//---------------------------------------开始13
    Set dm = createobject("dm.dmsoft")
    Form1.开始13.Visible=0
    运行窗口 = 梦幻窗口13内
    dm_ret = dm.SetWindowState(梦幻窗口13外, 1)
    Delay 100
    dm_ret = dm.SetWindowSize(梦幻窗口13外, 1066, 804)
    //
    TracePrint "窗口统一完成"
    日志1=1 
    线程ID13= BeginThread(按顺序执行)
    TracePrint 线程ID13
    Delay 10
    //线程卡ID13 = BeginThread(卡13)
    //TracePrint 检测脚本卡住()
    TracePrint "线程ID卡13="&线程卡ID13
End Event
Event Form1.结束13.Click //结束13
    TracePrint "13结束"
    StopThread 线程ID13
    StopThread 线程卡ID13
    TracePrint "结束线程ID卡13="&线程卡ID13
    Form1.开始13.Visible = 1
    dm_ret = dm.ForceUnBindWindow(梦幻窗口13内)
    TracePrint dm_ret
End Event
Event Form1.暂停13.Click//暂停13
    Form1.开始13.Visible = 0
    Form1.结束13.Visible=1
    Form1.暂停13.Visible=0
    PauseThread 线程ID13
    PauseThread 线程卡ID13
End Event
Event Form1.继续13.Click//继续13
    ContinueThread 线程ID13
    ContinueThread 线程卡ID13
    Form1.开始13.Visible = 1
    Form1.暂停13.Visible = 1
    Form1.继续13.Visible = 0
End Event
Event Form1.开始14.Click//---------------------------------------开始14
    Set dm = createobject("dm.dmsoft")
    Form1.开始14.Visible=0
    运行窗口 = 梦幻窗口14内
    dm_ret = dm.SetWindowState(梦幻窗口14外, 1)
    Delay 100
    dm_ret = dm.SetWindowSize(梦幻窗口14外, 1066, 804)
    TracePrint "窗口统一完成"
    日志1=1 
    线程ID14= BeginThread(按顺序执行)
    TracePrint 线程ID14
    Delay 10
    //线程卡ID14 = BeginThread(卡14)
    //TracePrint 检测脚本卡住()
    TracePrint "线程ID卡14="&线程卡ID14
End Event
Event Form1.结束14.Click //结束14
    TracePrint "14结束"
    StopThread 线程ID14
    StopThread 线程卡ID14
    TracePrint "结束线程ID卡14="&线程卡ID14
    Form1.开始14.Visible = 1
    dm_ret = dm.ForceUnBindWindow(梦幻窗口14内)
    TracePrint dm_ret
End Event
Event Form1.暂停14.Click//暂停14
    Form1.开始14.Visible = 0
    Form1.结束14.Visible=1
    Form1.暂停14.Visible=0
    PauseThread 线程ID14
    PauseThread 线程卡ID14
End Event
Event Form1.继续14.Click//继续14
    ContinueThread 线程ID14
    ContinueThread 线程卡ID14
    Form1.开始14.Visible = 1
    Form1.暂停14.Visible = 1
    Form1.继续14.Visible = 0
End Event
Event Form1.开始15.Click//---------------------------------------开始15
    Set dm = createobject("dm.dmsoft")
    Form1.开始15.Visible=0
    运行窗口 = 梦幻窗口15内
    dm_ret = dm.SetWindowState(梦幻窗口15外, 1)
    Delay 100
    dm_ret = dm.SetWindowSize(梦幻窗口15外, 1066, 804)
    TracePrint "窗口统一完成"
    日志1=1 
    线程ID15= BeginThread(按顺序执行)
    TracePrint 线程ID15
    Delay 10
    //线程卡ID15 = BeginThread(卡15)
    //TracePrint 检测脚本卡住()
    TracePrint "线程ID卡15="&线程卡ID15
End Event
Event Form1.结束15.Click //结束15
    TracePrint "15结束"
    StopThread 线程ID15
    StopThread 线程卡ID15
    TracePrint "结束线程ID卡15="&线程卡ID15
    Form1.开始15.Visible = 1
    dm_ret = dm.ForceUnBindWindow(梦幻窗口15内)
    TracePrint dm_ret
End Event
Event Form1.暂停15.Click//暂停15
    Form1.开始15.Visible = 0
    Form1.结束15.Visible=1
    Form1.暂停15.Visible=0
    PauseThread 线程ID15
    PauseThread 线程卡ID15
End Event
Event Form1.继续15.Click//继续15
    ContinueThread 线程ID15
    ContinueThread 线程卡ID15
    Form1.开始15.Visible = 1
    Form1.暂停15.Visible = 1
    Form1.继续15.Visible = 0
End Event
Sub 不动检测()
    卡顿 = dm.GetColor(910,133)
    Delay 10
    卡顿1 = dm.GetColor(511,687 )
    Delay 10
    卡顿2  = dm.GetColor(665,15 )
    Delay 10
    卡顿3 = dm.GetColor(502, 443)
    Delay 10
    卡顿4 = dm.GetColor(578, 707)
    Delay 10
    卡顿5 = dm.GetColor(899,114)
    Delay 10
    卡顿6 = dm.GetColor(921,674)
    Delay 10
    卡顿7 = dm.GetColor(873,707)
    Delay 10
    卡顿8 = dm.GetColor(324,119)
    Delay 10
    卡顿9 = dm.GetColor(920,507)
    Delay 10
    卡顿10 = dm.GetColor(355,195)
    Delay 10
    卡顿11 = dm.GetColor(335,588)
    Delay 10
    卡顿12 = dm.GetColor(939,162 )
    Delay 10
    卡顿13  = dm.GetColor(761,284 )
    Delay 10
    卡顿14 = dm.GetColor(798,604)
    Delay 10
    卡顿15 = dm.GetColor(355,300)
    Delay 10
    卡顿16 = dm.GetColor(537,682)
    Delay 10
    卡顿17= dm.GetColor(815,560)
    Delay 10
    卡顿18 = dm.GetColor(738,558)
    Delay 10
    卡顿19 = dm.GetColor(519,552)
    Delay 10
    卡顿20 = dm.GetColor(416,552)
    Delay 10
End Sub
Function 不动判断()
    If 卡顿 = dm.GetColor(910,133) Then 
        卡=卡+1
    End If
    Delay 10
    If 卡顿1 = dm.GetColor(511,687) Then 
        卡=卡+1
    End If
    Delay 10
    If 卡顿2 = dm.GetColor(665, 15) Then 
        卡=卡+1
    End If
    Delay 10
    If 卡顿3 = dm.GetColor(502, 443) Then 
        卡=卡+1
    End If
    Delay 10
    If 卡顿4 = dm.GetColor(578, 707) Then 
        卡=卡+1
    End If
    Delay 10
    If 卡顿5 = dm.GetColor(899,114) Then 
        卡=卡+1
    End If
    Delay 10
    If 卡顿6 = dm.GetColor(921,674) Then 
        卡=卡+1
    End If
    Delay 10
    If 卡顿7 = dm.GetColor(873,707) Then 
        卡=卡+1
    End If
    Delay 10
    If 卡顿8 = dm.GetColor(324,119) Then 
        卡=卡+1
    End If
    Delay 10
    If 卡顿9 = dm.GetColor(920,507) Then 
        卡=卡+1
    End If
    Delay 10
    If 卡顿10 = dm.GetColor(355,195) Then 
        卡=卡+1
    End If
    If 卡顿11 = dm.GetColor(335, 588) Then 
        卡=卡+1
    End If
    Delay 10
    If 卡顿12 = dm.GetColor(939, 162) Then 
        卡=卡+1
    End If
    delay 50
    If 卡顿13 = dm.GetColor(761, 284) Then 
        卡=卡+1
    End If
    Delay 10
    If 卡顿14 = dm.GetColor(798, 604) Then 
        卡=卡+1
    End If
    Delay 10
    If 卡顿15 = dm.GetColor(355, 300) Then 
        卡=卡+1
    End If
    Delay 10
    If 卡顿16 = dm.GetColor(537, 682) Then 
        卡=卡+1
    End If
    Delay 10
    If 卡顿17 = dm.GetColor(815, 560) Then 
    End If
    Delay 10
    If 卡顿18 = dm.GetColor(738, 558) Then 
        卡=卡+1
    End If
    Delay 10
    If 卡顿19 = dm.GetColor(519, 552) Then 
        卡=卡+1
    End If
    Delay 10
    If 卡顿20 = dm.GetColor(416, 552) Then 
        卡=卡+1
    End If
    Delay 10
    If 卡 >= 5 Then 
        不动判断=True
        TracePrint "==========卡了"
        Call 回到主界面()
    Else 
        TracePrint "3==========没卡"
        不动判断=False
    End If
    卡=0
End Function
Function 不动判断1()
    If 卡顿 = dm.GetColor(910,133) Then 
        卡=卡+1
    End If
    Delay 10
    If 卡顿1 = dm.GetColor(511,687) Then 
        卡=卡+1
    End If
    Delay 10
    If 卡顿2 = dm.GetColor(665, 15) Then 
        卡=卡+1
    End If
    Delay 10
    If 卡顿3 = dm.GetColor(502, 443) Then 
        卡=卡+1
    End If
    Delay 10
    If 卡顿4 = dm.GetColor(578, 707) Then 
        卡=卡+1
    End If
    Delay 10
    If 卡顿5 = dm.GetColor(899,114) Then 
        卡=卡+1
    End If
    Delay 10
    If 卡顿6 = dm.GetColor(921,674) Then 
        卡=卡+1
    End If
    Delay 10
    If 卡顿7 = dm.GetColor(873,707) Then 
        卡=卡+1
    End If
    Delay 10
    If 卡顿8 = dm.GetColor(324,119) Then 
        卡=卡+1
    End If
    Delay 10
    If 卡顿9 = dm.GetColor(920,507) Then 
        卡=卡+1
    End If
    Delay 10
    If 卡顿10 = dm.GetColor(355,195) Then 
        卡=卡+1
    End If
    If 卡顿11 = dm.GetColor(335, 588) Then 
        卡=卡+1
    End If
    Delay 10
    If 卡顿12 = dm.GetColor(939, 162) Then 
        卡=卡+1
    End If
    Delay 10
    If 卡顿13 = dm.GetColor(761, 284) Then 
        卡=卡+1
    End If
    Delay 10
    If 卡顿14 = dm.GetColor(798, 604) Then 
        卡=卡+1
    End If
    Delay 10
    If 卡顿15 = dm.GetColor(355, 300) Then 
        卡=卡+1
    End If
    Delay 10
    If 卡顿16 = dm.GetColor(537, 682) Then 
        卡=卡+1
    End If
    Delay 10
    If 卡顿17 = dm.GetColor(815, 560) Then 
    End If
    Delay 10
    If 卡顿18 = dm.GetColor(738, 558) Then 
        卡=卡+1
    End If
    Delay 10
    If 卡顿19 = dm.GetColor(519, 552) Then 
        卡=卡+1
    End If
    Delay 10
    If 卡顿20 = dm.GetColor(416, 552) Then 
        卡=卡+1
    End If
    Delay 50
    If 卡 >= 5 Then 
        不动判断1=True
        TracePrint "==========卡了"
    Else 
        TracePrint "3==========没卡"
        不动判断1=False
    End If
    卡=0
End Function
Function 寻路中()
    For 200
        Call 不动检测()
        Delay 300
        If 不动判断1 = True Then 
            TracePrint "没有在寻路"
            寻路中 = False
            Exit Function
        Else 
            Delay 1000
            TracePrint "正在寻路"
            寻路中=True
        End If
    Next 
End Function
Event Form1.打开1.Click
    Delay 300
    Call Plugin.Window.Active(梦幻窗口1外)
End Event
Event Form1.打开2.Click
    Delay 300
    Call Plugin.Window.Active(梦幻窗口2外)
End Event
Event Form1.打开3.Click
    Delay 300
    Call Plugin.Window.Active(梦幻窗口3外)
End Event
Event Form1.打开4.Click
    Delay 300
    Call Plugin.Window.Active(梦幻窗口4外)
End Event
Event Form1.打开5.Click
    Delay 300
    Call Plugin.Window.Active(梦幻窗口5外)
End Event
Event Form1.打开6.Click
    Delay 300
    Call Plugin.Window.Active(梦幻窗口6外)
End Event
Event Form1.打开7.Click
    Delay 300
    Call Plugin.Window.Active(梦幻窗口7外)
End Event
Event Form1.打开8.Click
    Delay 300
    Call Plugin.Window.Active(梦幻窗口8外)
End Event
Event Form1.打开9.Click
    Delay 300
    Call Plugin.Window.Active(梦幻窗口9外)
End Event
Event Form1.打开10.Click
    Delay 300
    Call Plugin.Window.Active(梦幻窗口10外)
End Event
Event Form1.打开11.Click
    Delay 300
    Call Plugin.Window.Active(梦幻窗口11外)
End Event
Event Form1.打开12.Click
    Delay 300
    Call Plugin.Window.Active(梦幻窗口12外)
End Event
Event Form1.打开13.Click
    Delay 300
    Call Plugin.Window.Active(梦幻窗口13外)
End Event
Event Form1.打开14.Click
    Delay 300
    Call Plugin.Window.Active(梦幻窗口14外)
End Event
Event Form1.打开15.Click
    Delay 300
    Call Plugin.Window.Active(梦幻窗口15外)
End Event
Event Form1.Button10.Click
    Form1.Button10.Visible=0
    TracePrint 窗口数量
    // 窗口数量=窗口数量+1
    If 窗口数量 = 1 Then 
        Form1.勾选1.Value = 1
    End If
    If 窗口数量 = 2 Then 
        Form1.勾选1.Value = 1
        Form1.勾选2.Value = 1
    End If
    If 窗口数量 = 3 Then 
        Form1.勾选1.Value = 1
        Form1.勾选2.Value = 1
        Form1.勾选3.Value = 1
    End If
    If 窗口数量 = 4 Then 
        Form1.勾选1.Value = 1
        Form1.勾选2.Value = 1
        Form1.勾选3.Value = 1
        Form1.勾选4.Value = 1
    End If
    If 窗口数量 = 5 Then 
        Form1.勾选1.Value = 1
        Form1.勾选2.Value = 1
        Form1.勾选3.Value = 1
        Form1.勾选4.Value = 1
        Form1.勾选5.Value = 1
    End If
    If 窗口数量 = 6 Then 
        Form1.勾选1.Value = 1
        Form1.勾选2.Value = 1
        Form1.勾选3.Value = 1
        Form1.勾选4.Value = 1
        Form1.勾选5.Value = 1
        Form1.勾选6.Value = 1
    End If
    If 窗口数量 = 7 Then 
        Form1.勾选1.Value = 1
        Form1.勾选2.Value = 1
        Form1.勾选3.Value = 1
        Form1.勾选4.Value = 1
        Form1.勾选5.Value = 1
        Form1.勾选6.Value = 1
        Form1.勾选7.Value = 1
    End If
    If 窗口数量 = 8 Then 
        Form1.勾选1.Value = 1
        Form1.勾选2.Value = 1
        Form1.勾选3.Value = 1
        Form1.勾选4.Value = 1
        Form1.勾选5.Value = 1
        Form1.勾选6.Value = 1
        Form1.勾选7.Value = 1
        Form1.勾选8.Value = 1
    End If
    If 窗口数量 = 9 Then 
        Form1.勾选1.Value = 1
        Form1.勾选2.Value = 1
        Form1.勾选3.Value = 1
        Form1.勾选4.Value = 1
        Form1.勾选5.Value = 1
        Form1.勾选6.Value = 1
        Form1.勾选7.Value = 1
        Form1.勾选8.Value = 1
        Form1.勾选9.Value = 1
    End If
    If 窗口数量 = 10 Then 
        Form1.勾选1.Value = 1
        Form1.勾选2.Value = 1
        Form1.勾选3.Value = 1
        Form1.勾选4.Value = 1
        Form1.勾选5.Value = 1
        Form1.勾选6.Value = 1
        Form1.勾选7.Value = 1
        Form1.勾选8.Value = 1
        Form1.勾选9.Value = 1
        Form1.勾选10.Value = 1
    End If
    If 窗口数量 = 11 Then 
        Form1.勾选1.Value = 1
        Form1.勾选2.Value = 1
        Form1.勾选3.Value = 1
        Form1.勾选4.Value = 1
        Form1.勾选5.Value = 1
        Form1.勾选6.Value = 1
        Form1.勾选7.Value = 1
        Form1.勾选8.Value = 1
        Form1.勾选9.Value = 1
        Form1.勾选10.Value = 1
        Form1.勾选11.Value = 1
    End If
    If 窗口数量 = 12 Then 
        Form1.勾选1.Value = 1
        Form1.勾选2.Value = 1
        Form1.勾选3.Value = 1
        Form1.勾选4.Value = 1
        Form1.勾选5.Value = 1
        Form1.勾选6.Value = 1
        Form1.勾选7.Value = 1
        Form1.勾选8.Value = 1
        Form1.勾选9.Value = 1
        Form1.勾选10.Value = 1
        Form1.勾选11.Value = 1
        Form1.勾选12.Value = 1
    End If
    If 窗口数量 = 13 Then 
        Form1.勾选1.Value = 1
        Form1.勾选2.Value = 1
        Form1.勾选3.Value = 1
        Form1.勾选4.Value = 1
        Form1.勾选5.Value = 1
        Form1.勾选6.Value = 1
        Form1.勾选7.Value = 1
        Form1.勾选8.Value = 1
        Form1.勾选9.Value = 1
        Form1.勾选10.Value = 1
        Form1.勾选11.Value = 1
        Form1.勾选12.Value = 1
        Form1.勾选13.Value = 1
    End If
    If 窗口数量 = 14 Then 
        Form1.勾选1.Value = 1
        Form1.勾选2.Value = 1
        Form1.勾选3.Value = 1
        Form1.勾选4.Value = 1
        Form1.勾选5.Value = 1
        Form1.勾选6.Value = 1
        Form1.勾选7.Value = 1
        Form1.勾选8.Value = 1
        Form1.勾选9.Value = 1
        Form1.勾选10.Value = 1
        Form1.勾选11.Value = 1
        Form1.勾选12.Value = 1
        Form1.勾选13.Value = 1
        Form1.勾选14.Value = 1
    End If
    If 窗口数量 = 15 Then 
        Form1.勾选1.Value = 1
        Form1.勾选2.Value = 1
        Form1.勾选3.Value = 1
        Form1.勾选4.Value = 1
        Form1.勾选5.Value = 1
        Form1.勾选6.Value = 1
        Form1.勾选7.Value = 1
        Form1.勾选8.Value = 1
        Form1.勾选9.Value = 1
        Form1.勾选10.Value = 1
        Form1.勾选11.Value = 1
        Form1.勾选12.Value = 1
        Form1.勾选13.Value = 1
        Form1.勾选14.Value = 1
        Form1.勾选15.Value = 1
    End If
End Event
Event Form1.Button3.Click//一键启动模拟器
    BeginThread 一键启动()
End Event
Sub 一键启动()
    call 子程序1()
    Delay 200
    Call 子程序2()
    Delay 200
    Call 子程序3()
    Delay 200
    Call 子程序4()
    Delay 200
    Call 子程序5()
    Delay 200
    Call 子程序6()
    Delay 200
    Call 子程序7()
    Delay 200
    Call 子程序8()
    Delay 200
    Call 子程序9()
    Delay 200
    Call 子程序10()
    Delay 200
    Call 子程序11()
    Delay 200
    Call 子程序12()
    Delay 200
    Call 子程序13()
    Delay 200
    Call 子程序14()
    Delay 200
    call 子程序15()
End Sub
Sub 子程序1()
    If Form1.勾选1.Value = 1 Then 
        Set dm = createobject("dm.dmsoft")
        线程ID1 = BeginThread(按顺序执行)
        Delay 10
        TracePrint 线程ID1
        Form1.Button1.Visible=0
        运行窗口 = 梦幻窗口1内
        TracePrint "第一行运行窗口句柄=" & 运行窗口
        dm_ret = dm.SetWindowState(梦幻窗口1外, 1)
        Delay 100
        dm_ret = dm.SetWindowSize(梦幻窗口1外, 1066, 804)
        Delay 10
        TracePrint "窗口统一完成"
        TracePrint "点击的时候的运行窗口="&运行窗口
        //线程卡ID1 = BeginThread(卡1)
        TracePrint 线程卡ID1
    End if
End Sub
Sub 子程序2()
    If Form1.勾选2.Value = 1 Then 
        线程ID2 = BeginThread(按顺序执行)
        TracePrint 线程ID2
        Delay 10
        Form1.Button2.Visible=0
        运行窗口 = 梦幻窗口2内
        TracePrint "第二行运行窗口句柄="&运行窗口
        dm_ret = dm.SetWindowState(梦幻窗口2外, 1)
        Delay 100
        dm_ret = dm.SetWindowSize(梦幻窗口2外, 1066, 804)
        TracePrint "窗口统一完成"
        //线程卡ID2 = BeginThread(卡2)
        //TracePrint 检测脚本卡住()
        TracePrint 线程卡ID2
    End If	
End Sub
Sub 子程序3()
    If Form1.勾选3.Value = 1 Then 
        Set dm = createobject("dm.dmsoft")
        Form1.开始3.Visible=0
        运行窗口 = 梦幻窗口3内
        TracePrint "第三行运行窗口句柄="&运行窗口
        dm_ret = dm.SetWindowState(梦幻窗口3外, 1)
        Delay 100
        dm_ret = dm.SetWindowSize(梦幻窗口3外, 1066, 804)
        TracePrint "窗口统一完成"
        日志1=1 
        线程ID3 = BeginThread(按顺序执行)
        TracePrint 线程ID3
        Delay 10
        //线程卡ID3 = BeginThread(卡3)
        //TracePrint 检测脚本卡住()
        TracePrint 线程卡ID3
    End If	
End Sub
Sub 子程序4()
    If Form1.勾选4.Value = 1 Then 
        Set dm = createobject("dm.dmsoft")
        Form1.开始4.Visible=0
        运行窗口 = 梦幻窗口4内
        dm_ret = dm.SetWindowState(梦幻窗口4外, 1)
        Delay 100
        dm_ret = dm.SetWindowSize(梦幻窗口4外, 1066, 804)
        //
        TracePrint "窗口统一完成"
        日志1=1 
        线程ID4 = BeginThread(按顺序执行)
        TracePrint 线程ID4
        Delay 10
        //线程卡ID4 = BeginThread(卡4)
        //TracePrint 检测脚本卡住()
        TracePrint 线程卡ID4
    End If	
End Sub
Sub 子程序5()
    If Form1.勾选5.Value = 1 Then 
        Set dm = createobject("dm.dmsoft")
        Form1.开始5.Visible=0
        运行窗口 = 梦幻窗口5内
        dm_ret = dm.SetWindowState(梦幻窗口5外, 1)
        Delay 100
        dm_ret = dm.SetWindowSize(梦幻窗口5外, 1066, 804)
        //
        TracePrint "窗口统一完成"
        日志1=1 
        线程ID5 = BeginThread(按顺序执行)
        TracePrint 线程ID5
        Delay 10
        //线程卡ID5 = BeginThread(卡5)
        //TracePrint 检测脚本卡住()
        TracePrint 线程卡ID5
    End If	
End Sub
Sub 子程序6()
    If Form1.勾选6.Value = 1 Then 
        Set dm = createobject("dm.dmsoft")
        Form1.开始6.Visible=0
        运行窗口 = 梦幻窗口6内
        dm_ret = dm.SetWindowState(梦幻窗口6外, 1)
        Delay 100
        dm_ret = dm.SetWindowSize(梦幻窗口6外, 1066, 804)
        //
        TracePrint "窗口统一完成"
        日志1=1 
        线程ID6 = BeginThread(按顺序执行)
        TracePrint 线程ID6
        Delay 10
        //线程卡ID6 = BeginThread(卡6)
        //TracePrint 检测脚本卡住()
        TracePrint 线程卡ID6
    End If	
End Sub
Sub 子程序7()
    If Form1.勾选7.Value = 1 Then 
        Set dm = createobject("dm.dmsoft")
        Form1.开始7.Visible=0
        运行窗口 = 梦幻窗口7内
        dm_ret = dm.SetWindowState(梦幻窗口7外, 1)
        Delay 100
        dm_ret = dm.SetWindowSize(梦幻窗口7外, 1066, 804)
        //
        TracePrint "窗口统一完成"
        日志1=1 
        线程ID7= BeginThread(按顺序执行)
        TracePrint 线程ID7
        Delay 10
        //线程卡ID7 = BeginThread(卡7)
        //TracePrint 检测脚本卡住()
        TracePrint "线程ID卡7="&线程卡ID7
    End If	
End Sub
Sub 子程序8()
    If Form1.勾选8.Value = 1 Then 
        Set dm = createobject("dm.dmsoft")
        Form1.开始8.Visible=0
        运行窗口 = 梦幻窗口8内
        dm_ret = dm.SetWindowState(梦幻窗口8外, 1)
        Delay 100
        dm_ret = dm.SetWindowSize(梦幻窗口8外, 1066, 804)
        TracePrint "窗口统一完成"
        日志1=1 
        线程ID8= BeginThread(按顺序执行)
        TracePrint 线程ID8
        Delay 10
        //线程卡ID8 = BeginThread(卡8)
        //TracePrint 检测脚本卡住()
        TracePrint "线程ID卡8="&线程卡ID8
    End If	
End Sub
Sub 子程序9()
    If Form1.勾选9.Value = 1 Then 
        Set dm = createobject("dm.dmsoft")
        Form1.开始9.Visible=0
        运行窗口 = 梦幻窗口9内
        dm_ret = dm.SetWindowState(梦幻窗口9外, 1)
        Delay 100
        dm_ret = dm.SetWindowSize(梦幻窗口9外, 1066, 804)
        TracePrint "窗口统一完成"
        日志1=1 
        线程ID9= BeginThread(按顺序执行)
        TracePrint 线程ID9
        Delay 10
        //线程卡ID9 = BeginThread(卡9)
        //TracePrint 检测脚本卡住()
        TracePrint "线程ID卡9="&线程卡ID9
    End If	
End Sub
Sub 子程序10()
    If Form1.勾选10.Value = 1 Then 
        Set dm = createobject("dm.dmsoft")
        Form1.开始10.Visible=0
        运行窗口 = 梦幻窗口10内
        dm_ret = dm.SetWindowState(梦幻窗口10外, 1)
        Delay 100
        dm_ret = dm.SetWindowSize(梦幻窗口10外, 1066, 804)
        TracePrint "窗口统一完成"
        日志1=1 
        线程ID10= BeginThread(按顺序执行)
        TracePrint 线程ID10
        Delay 10
        //线程卡ID10 = BeginThread(卡10)
        //TracePrint 检测脚本卡住()
        TracePrint "线程ID卡10="&线程卡ID10
    End If	
End Sub
Sub 子程序11()
    If Form1.勾选11.Value = 1 Then 
        Set dm = createobject("dm.dmsoft")
        Form1.开始11.Visible=0
        运行窗口 = 梦幻窗口11内
        dm_ret = dm.SetWindowState(梦幻窗口11外, 1)
        Delay 100
        dm_ret = dm.SetWindowSize(梦幻窗口11外, 1066, 804)
        TracePrint "窗口统一完成"
        日志1=1 
        线程ID11= BeginThread(按顺序执行)
        TracePrint 线程ID11
        Delay 10
        //线程卡ID11 = BeginThread(卡11)
        //TracePrint 检测脚本卡住()
        TracePrint "线程ID卡11="&线程卡ID11
    End If	
End Sub
Sub 子程序12()
    If Form1.勾选12.Value = 1 Then 
        Set dm = createobject("dm.dmsoft")
        Form1.开始12.Visible=0
        运行窗口 = 梦幻窗口12内
        dm_ret = dm.SetWindowState(梦幻窗口12外, 1)
        Delay 100
        dm_ret = dm.SetWindowSize(梦幻窗口12外, 1066, 804)
        TracePrint "窗口统一完成"
        日志1=1 
        线程ID12= BeginThread(按顺序执行)
        TracePrint 线程ID12
        Delay 10
        //线程卡ID12 = BeginThread(卡12)
        //TracePrint 检测脚本卡住()
        TracePrint "线程ID卡12="&线程卡ID12
    End If	
End Sub
Sub 子程序13()
    If Form1.勾选13.Value = 1 Then 
        Set dm = createobject("dm.dmsoft")
        Form1.开始13.Visible=0
        运行窗口 = 梦幻窗口13内
        dm_ret = dm.SetWindowState(梦幻窗口13外, 1)
        Delay 100
        dm_ret = dm.SetWindowSize(梦幻窗口13外, 1066, 804)
        //
        TracePrint "窗口统一完成"
        日志1=1 
        线程ID13= BeginThread(按顺序执行)
        TracePrint 线程ID13
        Delay 10
        //线程卡ID13 = BeginThread(卡13)
        //TracePrint 检测脚本卡住()
        TracePrint "线程ID卡13="&线程卡ID13
    End If	
End Sub
Sub 子程序14()
    If Form1.勾选14.Value = 1 Then 
        Set dm = createobject("dm.dmsoft")
        Form1.开始14.Visible=0
        运行窗口 = 梦幻窗口14内
        dm_ret = dm.SetWindowState(梦幻窗口14外, 1)
        Delay 100
        dm_ret = dm.SetWindowSize(梦幻窗口14外, 1066, 804)
        TracePrint "窗口统一完成"
        日志1=1 
        线程ID14= BeginThread(按顺序执行)
        TracePrint 线程ID14
        Delay 10
        //线程卡ID14 = BeginThread(卡14)
        //TracePrint 检测脚本卡住()
        TracePrint "线程ID卡14="&线程卡ID14
    End If	
End Sub
Sub 子程序15()
    If Form1.勾选15.Value = 1 Then 
        Set dm = createobject("dm.dmsoft")
        Form1.开始15.Visible=0
        运行窗口 = 梦幻窗口15内
        dm_ret = dm.SetWindowState(梦幻窗口15外, 1)
        Delay 100
        dm_ret = dm.SetWindowSize(梦幻窗口15外, 1066, 804)
        TracePrint "窗口统一完成"
        日志1=1 
        线程ID15= BeginThread(按顺序执行)
        TracePrint 线程ID15
        Delay 10
        //线程卡ID15 = BeginThread(卡15)
        //TracePrint 检测脚本卡住()
        TracePrint "线程ID卡15="&线程卡ID15
    End If	
End Sub
Event Form1.Button11.Click
    Call 句柄()
End Event
Sub OnScriptExit()//==================================脚本停止执行
      Form1.SaveSetting 
       Form2.SaveSetting 
              Form3.SaveSetting 
    //保存 控件
    Set dm = createobject("dm.dmsoft")
    dm.UnBindWindow
    dm_ret = dm.ForceUnBindWindow(梦幻窗口1内)
    Delay 10
    dm_ret = dm.ForceUnBindWindow(梦幻窗口2内)
    Delay 10
    dm_ret = dm.ForceUnBindWindow(梦幻窗口3内)
    Delay 10
    dm_ret = dm.ForceUnBindWindow(梦幻窗口4内)
    Delay 10
    dm_ret = dm.ForceUnBindWindow(梦幻窗口5内)
    Delay 10
    dm_ret = dm.ForceUnBindWindow(梦幻窗口6内)
    Delay 10
    dm_ret = dm.ForceUnBindWindow(梦幻窗口7内)
    Delay 10
    dm_ret = dm.ForceUnBindWindow(梦幻窗口8内)
    Delay 10
    dm_ret = dm.ForceUnBindWindow(梦幻窗口9内)
    Delay 10
    dm_ret = dm.ForceUnBindWindow(梦幻窗口10内)
    Delay 10
    dm_ret = dm.ForceUnBindWindow(梦幻窗口11内)
    Delay 10
    dm_ret = dm.ForceUnBindWindow(梦幻窗口12内)
    Delay 10
    dm_ret = dm.ForceUnBindWindow(梦幻窗口13内)
    Delay 10
    dm_ret = dm.ForceUnBindWindow(梦幻窗口14内)
    Delay 10
    dm_ret = dm.ForceUnBindWindow(梦幻窗口15内)
    Delay 10
End Sub

Event Form1.Load
  Form1.LoadSetting 
 Form2.LoadSetting 
   Form3.LoadSetting 
    TracePrint lib.文件.读取指定行文本内容("TP\任务选择.txt", 1)
    任务保存的内容 = Plugin.File.ReadFileEx("TP\任务内容.txt")
    TracePrint "任务内容"&任务保存的内容
    Form1.任务保存.List=任务保存的内容
    //载入控件
End Event
Event Form1.任务选择.SelectChange
    任务模式 = Form1.任务选择.ListIndex
    If 任务模式 = 0 Then 
        TracePrint "全部任务"
        Call 全部任务()
    ElseIf 任务模式 = 1 Then
        TracePrint "日常任务"
        Call 日常任务()
    ElseIf 任务模式 = 2 Then
        TracePrint "升级任务"
        Call 升级任务()
    ElseIf 任务模式 = 3 Then
        TracePrint "其他功能"
        Call 其他功能()
    End If
End Event
//Event Form2.任务选择.SelectChange
//    任务模式 = Form2.任务选择.ListIndex
//    If 任务模式 = 0 Then 
//        TracePrint "全部任务"
//        Call 全部任务2()
//    ElseIf 任务模式 = 1 Then
//        TracePrint "日常任务"
//         Call 日常任务2()
//    ElseIf 任务模式 = 2 Then
//        TracePrint "升级任务"
//           Call 升级任务2()
//    ElseIf 任务模式 = 3 Then
//        TracePrint "其他功能"
//            Call 其他功能2()
//    End If
//End Event
Sub 全部任务()
    Form1.ListBox1.List="自动排队|师门任务|运镖|打藏宝图|挖宝图|带队抓鬼|三界奇缘|混队抓鬼|秘境降妖|帮派任务|任务链|科举任务|竞技场|自动喊话|钓鱼|秒商城|秒工坊|活力使用|摆摊出售|整理背包|获取信息"
End Sub
Sub 日常任务()
    Form1.ListBox1.List="师门任务|运镖|打藏宝图|挖宝图|带队抓鬼|三界奇缘|混队抓鬼|秘境降妖|科举任务"
End Sub
Sub 升级任务()
    Form1.ListBox1.List="任务链"
End Sub
Sub 其他功能()
    Form1.ListBox1.List="自动喊话|钓鱼|秒商城|秒工坊|活力使用|摆摊出售|整理背包|获取信息"
End Sub
Sub 全部任务2()
    Form2.ListBox1.List="自动排队|师门任务|运镖|打藏宝图|挖宝图|带队抓鬼|三界奇缘|混队抓鬼|秘境降妖|帮派任务|任务链|科举任务|竞技场|自动喊话|钓鱼|秒商城|秒工坊|活力使用|摆摊出售|整理背包"
End Sub
Sub 日常任务2()
    Form2.ListBox1.List="师门任务|运镖|打藏宝图|挖宝图|带队抓鬼|三界奇缘|混队抓鬼|秘境降妖|科举任务"
End Sub
Sub 升级任务2()
    Form2.ListBox1.List="任务链"
End Sub
Sub 其他功能2()
    Form2.ListBox1.List="自动喊话|钓鱼|秒商城|秒工坊|活力使用|摆摊出售|整理背包"
End Sub
//Event Form1.任务保存.SelectChange
//  任务保存 = Form1.任务保存.ListIndex
//
//   If 任务保存 = 1 Then
//        TracePrint "日常一条龙"
//         Call 日常一条龙()
//    ElseIf 任务保存 = 2 Then
//        TracePrint "保存抓鬼"
//           Call 保存抓鬼()
//               ElseIf 任务保存 = 3 Then
//        TracePrint "自定义1"
//           Call 自定义1()
// 
//    End If
//End Event
///==============改变窗口
Sub 日常一条龙()
    Form1.ListBox2.List="师门任务|打藏宝图|挖宝图|带队抓鬼|三界奇缘|混队抓鬼|秘境降妖|科举任务|运镖"
End Sub
Sub 保存抓鬼()
    Form1.ListBox2.List="带队抓鬼"
End Sub
Sub 自定义1()
    Form1.ListBox2.List="带队抓鬼"
End Sub
Event Form1.Button16.Click
    内容 = Form1.ListBox2.List
    TracePrint 内容
    模板名称 = Form1.InputBox1.Text
    TracePrint 模板名称
    Call Plugin.File.WriteFileEx("TP\任务选择.txt",模板名称 )
    Call Plugin.File.WriteFileEx("TP\任务内容.txt", 内容)
    MsgBox "保存成功，可在选择模板查看"
    Form1.InputBox1.Text="输入模板名称"
End Event
Sub 读取信息()
End Sub
Event Form1.选择任务.Click
    Form2.Show 
    GetCursorPos mx,my 
    Form2.Left = MX-150
    Form2.Top=MY-50
End Event
Event Form2.LoadOver
    TracePrint "加载窗口"
    Text = Plugin.File.ReadFileEx("tP\任务选择.txt") 
    Form2.窗体2列表1.List=Text
End Event
Event Form2.窗体2列表1.Click
    行数 = Form2.窗体2列表1.ListIndex
    TracePrint "选中行数="&行数
    列表内容 = Lib.文件.读取指定行文本内容("TP\任务内容.txt", 行数+1)
    TracePrint "列表1内容="&列表内容
    Form2.窗体2列表2.List=列表内容
End Event
Event Form2.Button2.Click//点击加载计划
    行数 = Form2.窗体2列表1.ListIndex
    TracePrint "选中行数=" & 行数
    列表内容 = Lib.文件.读取指定行文本内容("TP\任务内容.txt", 行数+1)
    TracePrint "列表1内容="&列表内容
    Form1.ListBox2.List=列表内容
    form2.Hide 
End Event
Event Form2.Button1.Click//删除计划
    行数 = Form2.窗体2列表1.ListIndex
    TracePrint "选中行数=" & 行数
    Call lib.文件.删除指定行文本内容("TP\任务内容.txt",行数+1)
    Call Lib.文件.删除指定行文本内容("TP\任务选择.txt", 行数 + 1)
    //===
    Text = Plugin.File.ReadFileEx("tP\任务选择.txt") 
    Form2.窗体2列表1.List = Text
    Form2.窗体2列表2.List=""
End Event
//==========================>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>======登录界面
Global 用户卡密, 所属软件, 卡密Needle
DimEnv 心跳检测id,退出登录卡密, 退出登录卡密Needle
Function Request(api, 参数)
    //-------------------------------------------这里是需要修改的参数
    API密码 = "422498"
    开发者ID = "16226"
    本机机器码 = Plugin.Sys.GetHDDSN //这块是防破解的一个因素，如果不知道怎么写，给个空字符串也行，不修改也行，这个变量的值只要是能获取到软件运行机器的唯一标识码就行，没有特定要求必须是什么值,写了这个也不会绑定机器，你可以理解为没有任何作用，不写也行，不过最好是获取到机器的唯一标示码，可以有效防止破解。
    //--------------------------------------------------------------
    Randomize
    ApiList = array("http://napi.2cccc.cc/", "http://api2.2cccc.cc/", "http://api3.2cccc.cc/")
    Host = ApiList(Int((UBound(ApiList) - 0 + 1) * Rnd() + 0))
    时间戳 = 获取时间戳
    本机机器码 = Plugin.Encrypt.Md5String(API密码 & 本机机器码 & 时间戳)
    API密码 = API密码 & 本机机器码
    签名 = Plugin.Encrypt.Md5String(API密码 & 时间戳)
    common_params = "center_id=" & 开发者ID & "&tmstamp=" & 时间戳 & "&sign=" & 签名 & "&client_key=" & 本机机器码
    Res = Lib.网络.获得网页源文件_增强版(Host & api & "?" & common_params & "&" & 参数, "utf-8")
    connect_times = 0
    Do While Instr(1, Res, "code", 1) = 0
        Rand = Int((UBound(ApiList) - 0 + 1) * Rnd() + 0)
        Host = ApiList(Int((UBound(ApiList) - 0 + 1) * Rnd() + 0))
        Res = Lib.网络.获得网页源文件_增强版(Host & api & "?" & common_params & "&" & 参数, "utf-8")
        connect_times = connect_times + 1
        If Instr(1, Res, "code", 1) = 0 and connect_times > 1 Then  
            TracePrint "连接服务器失败，正在尝试重新请求"
            Delay 2000
        End If
        If connect_times > 30 Then 
            Request = array(0, "无法连接服务器")
            Exit Do
        End If
    Loop
    If json_decode(Res, "code") = 1 Then 
        If UCase(Plugin.Encrypt.Md5String(json_decode(Res, "timestamp") & API密码)) = Ucase(json_decode(Res, "sign")) and Abs(clng(时间戳) - clng(json_decode(Res,"timestamp"))) < 600 Then 
            Request = array(1, Res)
        Else 
            Request = array(0,"请检查API密码是否填写正确")
        End If
    Else 
        msg = json_decode(Res, "msg")
        If msg = "" Then 
            msg = "连接服务器失败"
        End If
        Request = array(0,msg)
    End If
End Function
Function json_decode(str,json路径)
    Set sc = CreateObject("MSScriptControl.ScriptControl")
    sc.Language = "JScript"
    sc.AddCode "var tmp = " & str & ";"
    json_decode = sc.Eval("tmp." & json路径)
End Function
Function 获取时间戳
    时间戳 = ""
    connect_times = 0
    Do While 时间戳 = ""
        时间戳 = left(json_decode(Lib.网络.获得网页源文件_增强版("http://api.m.taobao.com/rest/api3.do?api=mtop.common.getTimestamp", "utf-8"), "data.t"), 10)
        connect_times = connect_times + 1
        If 时间戳 = "" and connect_times > 1 Then  
            TracePrint "获取淘宝时间戳失败，尝试重新请求"
        End If
        If connect_times > 15 Then 
            获取时间戳 = false
            //获取淘宝时间戳失败，一般是设备网络问题或运营商网络问题，可以自行在这里写其他获取时间戳的方法，必须是实时的10位时间戳
            Exit Do
        End If
    Loop
    获取时间戳 = 时间戳
End Function
Event Form3.登录.Click
    Form3.启动中标签.Visibl = 1
    //-------------------------------------------这里是需要修改的参数
    Form3.SaveSetting
    用户卡密 = Form3.激活码.Text
    Form1.卡密.Caption=用户卡密
    所属软件 = "mhfz2022"
    //--------------------------------------------------------------
    卡密登录 = Request("apiv3/card_login", "card=" & 用户卡密 & "&software=" & 所属软件)
    If (卡密登录(0) = 1) Then 
        //-----------------------------------登录成功了，成功之后要干啥，自己写
        卡密到期时间 = json_decode(卡密登录(1),"data.endtime")
        卡密剩余时间 = json_decode(卡密登录(1),"data.less_time")
        卡密Needle = json_decode(卡密登录(1), "data.needle")
        退出登录卡密Needle = 卡密Needle
        Form1.退出卡密.Caption= 卡密Needle 
        TracePrint "启动成功"
        退出登录卡密= Form1.卡密.Caption
        退出登录卡密Needle= Form1.退出卡密.Caption
        Call 启动后运行()
        Delay 500
        心跳检测id= BeginThread (心跳检测)
        Form1.Show 
    Else 
        //----------------------------------------登录失败了，失败之后怎么办，自己写
        失败原因 = 卡密登录(1)
        MsgBox 失败原因
        TracePrint 失败原因
        ExitScript
    End If
    //---------------------------------------这个必须配合卡密登录功能来做，防止他登录了一直挂机，两个人同时用一个卡密，或者卡密挂机中到期了，这里就是为了防止这种情况出现的。所以需要一直执行，官方建议10秒执行一次
End Event
Sub 心跳检测()
    TracePrint "开始心跳检测"
    TracePrint 用户卡密
    所属软件 ="mhfz2022"
    TracePrint 卡密Needle
    用户卡密=Form1.卡密.Caption 
    卡密Needle=Form1.退出卡密.Caption
    Do
        卡密状态实时监测 = Request("apiv3/card_ping", "card=" & 用户卡密 & "&software=" & 所属软件 & "&needle=" & 卡密Needle)
        If 卡密状态实时监测(0) = 1 Then 
            //---------------------------------检测到一切正常，
            账号到期时间 = json_decode(卡密状态实时监测(1), "data.endtime")
            账号剩余时间 = json_decode(卡密状态实时监测(1), "data.less_time")
            TracePrint 账号剩余时间
            Form1.剩余时间.Caption=账号剩余时间 
            Delay json_decode(卡密状态实时监测(1), "data.heartbeat_second") * 1000
        Else 
            //----------------------------------检测到异常，账号状态异常或者账号被挤下线。
            失败原因 = 卡密状态实时监测(1)
            TracePrint 失败原因
            //           Form3.登录提示.Caption = 失败原因
            MsgBox 失败原因
            Form1.Close 
            ExitScript
        End If
    Loop
End Sub
//Do
//    
//    TracePrint "这里是主线程"
//    
//    Delay 3000
//    
//Loop
Event Form3.Load//窗口3加载中
    Call 获取公告()
    call 版本更新()
End Event
Sub 关闭登录()//
    For 10
        Hwnd = Plugin.Window.Find(0, "大熊猫辅助")
        Call Plugin.Window.Close(Hwnd)
        If hwnd = "" Then 
            TracePrint "关闭成功"
            Exit Sub 
        Else 
            TracePrint "没关闭"
            Delay 300
        End If
    Next 
End Sub
Sub 获取公告
    获取哪个软件的公告 = "mhfz2022"//软件名，如果不填，则默认获取的是【设置】中的【开发者公告】
    获取软件公告 = Request("apiv3/inform", "software=" & 获取哪个软件的公告)
    If 获取软件公告(0) = 1 Then 
        公告内容 = json_decode(获取软件公告(1), "data.inform")
        TracePrint 公告内容
        Form3.公告.Caption=公告内容
    Else 
        TracePrint "获取公告失败，失败原因：" & 获取软件公告(1)
    End If
End Sub
Event Form1.PictureBox33.Click
End Event
Sub 版本更新()
    TracePrint "开始版本"
    软件名 = "mhfz2022"
    当前版本 = "1.01"
    TracePrint "开始版本"
    Form3.当前版本1.Caption = 当前版本
    Form1.当前版本2.Caption=当前版本
    //------------------------------------------
    TracePrint "开始版本"
    版本控制操作 = Request("apiv3/version", "version=" & 当前版本 & "&software=" & 软件名)
    TracePrint 版本控制操作(1)
    TracePrint "开始版本"
    //获取最新版本
    //获取下载链接
    下载链接 = Mid(版本控制操作(1), 21,100)
    TracePrint 下载链接
    下载链接 = Replace(下载链接, "【", "", 1, - 1 , 1)
    Delay  10
    下载链接 = Replace(下载链接, "】", "", 1, -1, 1) 
    TracePrint 下载链接
    TracePrint "开始版本"
    Form3.下载链接.Text=下载链接
    If 版本控制操作(0) = 1 Then 
        TracePrint "开始版本"
        //    Call Plugin.Window.Close(Form3.启动等待.Hwnd )
        TracePrint "无需更新"
        Form3.Label1.Visible = 0
        Form3.新版本.Caption=当前版本
    Else 
        TracePrint "开始版本"
        //       Call Plugin.Window.Close(Form3.启动等待.Hwnd )
        MyVar = Mid(版本控制操作(1), 9, 4)
        TracePrint "执行到这"&MyVar
        Form3.新版本.Caption=MyVar
        Form3.Label1.Visible=0
        MsgBox 版本控制操作(1)
        TracePrint 版本控制操作(1)
    End If
End Sub
Event Form3.Button1.Click
    文本=Form3.下载链接.Text 
    TracePrint 文本
    Plugin.Sys.SetCLB 文本
    TracePrint "复制下载链接"
End Event
Sub 启动后运行()
    BeginThread  关闭登录()
    TracePrint "检测加载"
    Form1.LoadSetting 
    Delay 10
 Form2.LoadSetting 
  Delay 10
   Form3.LoadSetting 
    Delay 1000
    Call 大漠注册()
    Delay 1000
    Call 句柄()
    i=1
    For 15
        //表格加载序号 
        Form1.Grid1.SetItemText i,0,i   
        Form1.Grid1.SetItemText i, 7, "1.添加完任务 2.点击开始"
        i=i+1
    Next 	
End Sub
Sub 退出登录()
    用户卡密 = 退出登录卡密
    TracePrint "用户卡密"&用户卡密
    needle =退出登录卡密Needle//注意，这个needle，就是在卡密登录的时候拿到的那个卡密Needle，要注意变量传值。
       TracePrint "needle"&needle
    卡密退出登录 = Request("apiv3/card_logout", "card=" & 用户卡密 & "&needle=" & needle)
    If 卡密退出登录(0) = 1 Then 
        TracePrint "退出成功"
    Else 
        TracePrint "退出失败，失败原因：" & 卡密退出登录(1)
    End If
End Sub
Event Form1.UnLoad//窗口1关闭
    StopThread 心跳检测id
    
        Form1.SaveSetting 
       Form2.SaveSetting 
              Form3.SaveSetting 
    Call 退出登录
End Event
Event Form1.反选.Click//取消全选
    Form1.Button10.Visible=1
    Form1.勾选1.Value = 0
    Form1.勾选2.Value = 0
    Form1.勾选3.Value = 0
    Form1.勾选4.Value = 0
    Form1.勾选5.Value = 0
    Form1.勾选6.Value = 0
    Form1.勾选7.Value = 0
    Form1.勾选8.Value = 0
    Form1.勾选9.Value = 0
    Form1.勾选10.Value = 0
    Form1.勾选11.Value = 0
    Form1.勾选12.Value = 0
    Form1.勾选13.Value = 0
    Form1.勾选14.Value = 0
    Form1.勾选15.Value = 0
End Event
Event Form1.Button8.Click//脚本停止
    Form1.SaveSetting 
       Form2.SaveSetting 
              Form3.SaveSetting 
    //保存 控件
    Set dm = createobject("dm.dmsoft")
    If Form1.勾选1.Value = 1 Then 
        dm_ret = dm.ForceUnBindWindow(梦幻窗口1内)
        Form1.Button1.Visible = 1//启动键显示
        StopThread 线程ID1
        Form1.Grid1.SetItemText 1, 6, "任务终止"
        Form1.Grid1.SetItemText 1, 7, "已解绑窗口并终止"
        Delay 10
    End If
    If Form1.勾选2.Value = 1 Then 
        dm_ret = dm.ForceUnBindWindow(梦幻窗口2内)
        Delay 10
        Form1.Button2.Visible = 1
    End If
    If Form1.勾选3.Value = 1 Then 
        Delay 10
        dm_ret = dm.ForceUnBindWindow(梦幻窗口3内)
        Form1.开始3.Visible = 1
    End If
    If Form1.勾选4.Value = 1 Then 
        Delay 10
        Form1.开始4.Visible = 1
        dm_ret = dm.ForceUnBindWindow(梦幻窗口4内)
    End If
    If Form1.勾选5.Value = 1 Then 
        Delay 10
        dm_ret = dm.ForceUnBindWindow(梦幻窗口5内)
        Form1.开始5.Visible = 1
    End If
    If Form1.勾选6.Value = 1 Then 
        Delay 10
        dm_ret = dm.ForceUnBindWindow(梦幻窗口6内)
        Form1.开始6.Visible = 1
    End If
    If Form1.勾选7.Value = 1 Then 
        Delay 10
        dm_ret = dm.ForceUnBindWindow(梦幻窗口7内)
        Form1.开始7.Visible = 1
    End If
    If Form1.勾选8.Value = 1 Then 
        Delay 10
        dm_ret = dm.ForceUnBindWindow(梦幻窗口8内)
        Form1.开始8.Visible = 1
    End If
    If Form1.勾选9.Value = 1 Then 
        Delay 10
        dm_ret = dm.ForceUnBindWindow(梦幻窗口9内)
        Form1.开始9.Visible = 1
    End If
    If Form1.勾选10.Value = 1 Then 
        Delay 10
        dm_ret = dm.ForceUnBindWindow(梦幻窗口10内)
        Form1.开始10.Visible = 1
    End If
    If Form1.勾选11.Value = 1 Then 
        Delay 10
        dm_ret = dm.ForceUnBindWindow(梦幻窗口11内)
        Form1.开始11.Visible = 1
    End If
    If Form1.勾选12.Value = 1 Then 
        Delay 10
        dm_ret = dm.ForceUnBindWindow(梦幻窗口12内)
        Form1.开始12.Visible = 1
    End If
    If Form1.勾选13.Value = 1 Then 
        Delay 10
        dm_ret = dm.ForceUnBindWindow(梦幻窗口13内)
        Form1.开始13.Visible = 1
    End If
    If Form1.勾选14.Value = 1 Then 
        Delay 10
        dm_ret = dm.ForceUnBindWindow(梦幻窗口14内)
        Form1.开始14.Visible = 1
    End If
    If Form1.勾选15.Value = 1 Then 
        dm_ret = dm.ForceUnBindWindow(梦幻窗口15内)
        Form1.开始15.Visible = 1
        Delay 10
    End If
    //解绑窗口
    //恢复按钮
End Event