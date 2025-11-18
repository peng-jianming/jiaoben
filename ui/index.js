import Index from './components/index.js'

new Vue({
    el: '#app',
    components: {
        Index
    },
    data() {
        return {
            list: [],
            socket: null
        }
    },
    mounted() {
        this.socket = new WebSocket('ws://localhost:8081');

        this.socket.onopen = (event) => {
            console.log('WebSocket连接已建立');
        };

        this.socket.onmessage = (event) => {
            const result = JSON.parse(event.data)
            console.log(`收到消息:`, result);

            if (result.type == "init") {
                // 全量更新：初始化或重置时使用
                this.list = result.data.list
            } else if (result.type == "updateItem") {
                // 更新或添加 worker
                const index = this.list.findIndex(item => item.hwnd === result.data.hwnd);
                if (index >= 0) {
                    // 更新现有项
                    this.$set(this.list, index, result.data);
                } else {
                    // 添加新项
                    this.list.push(result.data);
                }
            } else if (result.type == "refresh") {
                // UI 文件变化，自动刷新页面
                console.log(result.data.message);
                // 延迟一小段时间再刷新，确保消息已处理
                setTimeout(() => {
                    location.reload();
                }, 100);
            }
        };
    },
    template: `
    <div>
        <Index :deviceList="list" :socket="socket"/>
    </div>
    `
});



















