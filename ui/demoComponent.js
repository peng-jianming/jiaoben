export default {
    props: {
        deviceList: {
            type: Array,
            default() {
                return []
            }
        },
        socket: {
            type: WebSocket,
            default: null
        }
    },
    methods: {
        async handleStart(list) {
            this.socket.send(JSON.stringify({
                type: 'start',
                data: {
                    deviceList: list,
                    taskConfig: ['demo', 'shimen']
                }
            }))
            console.log("发送开始指令");
        },
        async handleStop(list) {
            this.socket.send(JSON.stringify({
                type: 'stop',
                data: {
                    deviceList: list
                }
            }))
            console.log("发送停止指令");
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
