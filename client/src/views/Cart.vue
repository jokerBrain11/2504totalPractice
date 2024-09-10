<template>
  <div class="container mt-5">
    <h1 class="text-center mb-4">購物車</h1>
    <ul class="list-group mb-4">
      <li v-for="item in cart" :key="item.id" class="list-group-item d-flex justify-content-between align-items-center">
        <div>
          <h5 class="mb-1">{{ item.name }}</h5>
          <small class="text-muted">{{ item.quantity }} x ${{ item.price }}</small>
        </div>
        <button class="btn btn-danger btn-sm" @click="removeFromCart(item.id)">刪除</button>
      </li>
    </ul>
    <div class="text-right">
      <h4>總價: ${{ cartTotal }}</h4>
    </div>
  </div>
</template>

<script>
import { mapActions, mapState, mapGetters } from "vuex";

export default {
  computed: {
    ...mapState("product", ["cart"]),
    ...mapGetters("product", ["cartTotal"]),
  },
  methods: {
    ...mapActions("product", ["removeFromCart"]),
  },
};
</script>

<style scoped>
.container {
  max-width: 800px;
}

.list-group-item {
  border: 1px solid #ddd;
}

h1 {
  font-size: 2rem;
  font-weight: bold;
}

h5 {
  margin-bottom: 0;
}

.text-right {
  text-align: right;
}

.btn-danger {
  background-color: #dc3545;
  border-color: #dc3545;
}

.btn-danger:hover {
  background-color: #c82333;
  border-color: #bd2130;
}
</style>
