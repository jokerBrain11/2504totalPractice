import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';
import { fileURLToPath, URL } from 'node:url';
import fs from 'node:fs';
import path from 'node:path';

// 定義並匯出 Vite 配置
export default defineConfig({
  server: {
    host: process.env.VITE_HOST || 'localhost', // 從環境變量獲取主機地址，默認為 0.0.0.0
    port: parseInt(process.env.VITE_PORT, 10) || 3000, // 從環境變量獲取端口，默認為 3000
    // https: {
    //   key: fs.readFileSync(path.resolve(__dirname, 'certs/localhost-key.pem')),
    //   cert: fs.readFileSync(path.resolve(__dirname, 'certs/localhost.pem')),
    // },
    // proxy: {
    //   '/api': {
    //     target: 'https://10.25.4.111:7116', // 代理目標
    //     changeOrigin: true,
    //     secure: false, // 忽略 SSL 憑證問題（在開發環境中）
    //   },
    // },
  },
  plugins: [
    vue(), // 使用 Vue 插件
  ],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url)), // 設置 '@' 的別名指向 'src' 目錄
    },
  },
});
