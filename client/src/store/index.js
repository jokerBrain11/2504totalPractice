import { createStore } from 'vuex';
import user from './modules/user';
import product from './modules/product';

export default createStore({
  state: {
    count: 0
  },
  mutations: {
    increment(state) {
      state.count++;
    },
    decrement(state) {
      state.count--;
    }
  },
  actions: {
    increment({ commit }) {
      commit('increment');
    },
    decrement({ commit }) {
      commit('decrement');
    }
  },
  getters: {
    doubleCount: state => state.count * 2
  },
  modules: {
    user,
    product
  }
});
