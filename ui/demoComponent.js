export default {
    props: {
        deviceList: {
            type: Array,
            default() {
                return []
            }
        }
    },
    methods: {
        async handleStart(list) {
            const aaa = await fetch('/start', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(list)
            }).then(response => {
                if (!response.ok) {
                    throw new Error(`HTTP错误! 状态: ${response.status}`);
                }
                return response.json();
            })
            console.log(aaa, "----------");
        },
        async handleStop(list) {
            const aaa = await fetch('/stop', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(list)
            }).then(response => {
                if (!response.ok) {
                    throw new Error(`HTTP错误! 状态: ${response.status}`);
                }
                return response.json();
            })
            console.log(aaa, "----------");
        },
    },
    template: `
    <div>
        <el-button @click="handleStart(deviceList)">全部开始 </el-button>
        <el-button @click="handleStop(deviceList)">全部停止 </el-button>  
        <el-table :data="deviceList" border >
            <el-table-column prop="hwnd" label="句柄" width="180">
            </el-table-column>
            <el-table-column prop="name" label="窗口名" width="180">
            </el-table-column>
            <el-table-column prop="status" label="状态" width="180">
            </el-table-column>
            <el-table-column label="操作" >
                <template slot-scope="scope">
                    <el-button @click="handleStart([scope.row])" type="text" size="small">开始</el-button>
                    <el-button @click="handleStop([scope.row])" type="text" size="small">结束</el-button>
                </template>
            </el-table-column>
        </el-table>
        </div>
    </div>
    `
};
