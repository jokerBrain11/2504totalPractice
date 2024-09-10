import './assets/main.css'

import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import store from '@/store'
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';

const app = createApp(App)

app.use(router)
app.use(store)


app.mount('#app')

// export const isLogin = () => {
//   return !!store.getters['user/SET_USER'].userName;
// };


const logout = () => {
  store.dispatch('user/logoutUser');
  router.push({ name: 'Login' });
}

app.config.globalProperties.$logout = logout;