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
            this.socket.send(JSON.stringify({ type: 'init' }))
        };

        this.socket.onmessage = (event) => {
            const result = JSON.parse(event.data)
            console.log(`收到消息:`, result);

            if (result.type == "update") {
                this.list = result.data
            }
        };

        setTimeout(() => {
            this.socket.send(JSON.stringify({
                type: 'start',
                data: {
                    list: this.list,
                    taskConfig: ['demo']
                }
            }))
        }, 5000);
    },
    template: `
    <div>
        <demo :deviceList="list"/>
    </div>
    `
});



















