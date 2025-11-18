import demo from './demoComponent.js'

new Vue({
    el: '#app',
    components: {
        demo
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
            }
        };
    },
    template: `
    <div>
        <demo :deviceList="list" :socket="socket"/>
    </div>
    `
});



















