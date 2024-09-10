import api from '@/services/api';
const state = {
  products: [],
  cart: [],
};

const mutations = {
  SET_PRODUCTS(state, products) {
    state.products = products;
  },
  ADD_TO_CART(state, product) {
    const item = state.cart.find((item) => item.id === product.id);
    if (item) {
      item.quantity++;
    } else {
      state.cart.push({ ...product, quantity: 1 });
    }
  },
  REMOVE_FROM_CART(state, productId) {
    state.cart = state.cart.filter((item) => item.id !== productId);
  },
};

const actions = {
  async fetchProducts({ commit }) {
    try {
      const response = await api.getProducts();
      commit("SET_PRODUCTS", response.data);
    } catch (error) {
      console.error("Error fetching products:", error);
    }
  },
  addToCart({ commit }, product) {
    commit("ADD_TO_CART", product);
  },
  removeFromCart({ commit }, productId) {
    commit("REMOVE_FROM_CART", productId);
  },
};

const getters = {
  products: (state) => state.products,
  cart: (state) => state.cart,
  cartTotal: (state) => {
    return state.cart.reduce((total, item) => {
      return total + item.price * item.quantity;
    }, 0);
  },
};

export default {
  namespaced: true,
  state,
  mutations,
  actions,
  getters,
};
