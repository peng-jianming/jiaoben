const fs = require('fs');
const path = require('path');
const EventEmitter = require('events');

/**
 * 热重载管理器
 * 监听文件变化并触发重载事件
 */
class HotReloadManager extends EventEmitter {
    constructor() {
        super();
        this.watchers = new Map();
        this.debounceTimers = new Map();
        this.debounceDelay = 300; // 防抖延迟（毫秒）
    }

    /**
     * 开始监听目录
     */
    watchDirectory(dirPath, options = {}) {
        // 标准化路径
        const normalizedPath = path.resolve(dirPath);
        
        if (this.watchers.has(normalizedPath)) {
            console.log(`目录 ${normalizedPath} 已在监听中`);
            return;
        }

        try {
            const watcher = fs.watch(normalizedPath, { recursive: true }, (eventType, filename) => {
                if (!filename) return;

                const fullPath = path.resolve(normalizedPath, filename);
                
                // 根据目录类型决定监听的文件类型
                const isUIDir = normalizedPath.includes(path.sep + 'ui' + path.sep) || 
                               normalizedPath.endsWith(path.sep + 'ui');
                
                if (isUIDir) {
                    // UI 目录：监听 .js, .css, .html 文件
                    const ext = path.extname(filename).toLowerCase();
                    if (!['.js', '.css', '.html'].includes(ext)) {
                        return;
                    }
                } else {
                    // 其他目录：只监听 .js 文件
                    if (!filename.endsWith('.js')) {
                        return;
                    }
                }

                // 跳过 node_modules 和其他不需要监听的目录
                if (fullPath.includes('node_modules') || 
                    fullPath.includes('.git') ||
                    fullPath.includes('cache')) {
                    return;
                }

                // 防抖处理
                const timerKey = fullPath;
                if (this.debounceTimers.has(timerKey)) {
                    clearTimeout(this.debounceTimers.get(timerKey));
                }

                const timer = setTimeout(() => {
                    this.debounceTimers.delete(timerKey);
                    this.handleFileChange(eventType, fullPath, filename);
                }, this.debounceDelay);

                this.debounceTimers.set(timerKey, timer);
            });

            this.watchers.set(normalizedPath, watcher);
            console.log(`✓ 开始监听目录: ${normalizedPath}`);
        } catch (error) {
            console.error(`监听目录 ${normalizedPath} 失败:`, error.message);
        }
    }

    /**
     * 处理文件变化
     */
    handleFileChange(eventType, fullPath, filename) {
        // 确保文件存在（避免删除事件）
        if (eventType === 'rename' && !fs.existsSync(fullPath)) {
            return;
        }

        console.log(`\n🔄 检测到文件变化: ${filename} (${eventType})`);

        // 标准化路径用于比较
        const normalizedPath = path.resolve(fullPath);
        const taskDirPattern = path.sep + 'task' + path.sep;
        const toolsDirPattern = path.sep + 'tools' + path.sep;
        const uiDirPattern = path.sep + 'ui' + path.sep;
        
        // 获取根目录路径（项目根目录）
        const rootDir = path.resolve(__dirname);
        const isRootFile = path.dirname(normalizedPath) === rootDir;

        // 先检查根目录下的特殊文件（worker.js 和 index.js）
        if (isRootFile) {
            if (filename === 'worker.js') {
                // worker.js 变化
                this.emit('workerChanged', { filePath: normalizedPath });
                return;
            } else if (filename === 'index.js') {
                // 主文件变化（需要手动重启）
                console.log('⚠ 主文件 index.js 已修改，请手动重启服务器');
                return;
            }
        }

        // 判断文件类型并触发相应事件
        if (normalizedPath.includes(uiDirPattern)) {
            // UI 文件变化
            this.emit('uiChanged', { filePath: normalizedPath, filename });
        } else if (normalizedPath.includes(taskDirPattern)) {
            // 任务文件变化
            const taskName = path.basename(filename, '.js');
            // 跳过非任务文件
            if (taskName !== 'index' && taskName !== 'config' && taskName !== 'TaskRegistry') {
                this.emit('taskChanged', { taskName, filePath: normalizedPath, filename });
            }
        } else if (normalizedPath.includes(toolsDirPattern)) {
            // 工具文件变化
            this.emit('toolChanged', { filePath: normalizedPath, filename });
        }
    }

    /**
     * 停止监听目录
     */
    unwatchDirectory(dirPath) {
        const watcher = this.watchers.get(dirPath);
        if (watcher) {
            watcher.close();
            this.watchers.delete(dirPath);
            console.log(`✓ 已停止监听目录: ${dirPath}`);
        }
    }

    /**
     * 停止所有监听
     */
    stopAll() {
        this.watchers.forEach((watcher, dirPath) => {
            watcher.close();
        });
        this.watchers.clear();
        
        // 清除所有防抖定时器
        this.debounceTimers.forEach(timer => clearTimeout(timer));
        this.debounceTimers.clear();
        
        console.log('✓ 已停止所有文件监听');
    }
}

module.exports = new HotReloadManager();

